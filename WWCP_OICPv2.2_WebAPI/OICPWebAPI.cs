/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.WWCP.OICPv2_2.EMP;
using org.GraphDefined.WWCP.OICPv2_2.CPO;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.WebAPI
{

    /// <summary>
    /// OICP+ HTTP API extention methods.
    /// </summary>
    public static class ExtentionMethods
    {

        #region ParseRoamingNetwork(this HTTPRequest, HTTPServer, out RoamingNetwork, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network
        /// for the given HTTP hostname and HTTP query parameter
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetwork(this HTTPRequest                             HTTPRequest,
                                                  HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                                                  out RoamingNetwork                           RoamingNetwork,
                                                  out HTTPResponse                             HTTPResponse)
        {

            if (HTTPServer == null)
                Console.WriteLine("HTTPServer == null!");

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException("HTTPRequest",  "The given HTTP request must not be null!");

            if (HTTPServer == null)
                throw new ArgumentNullException("HTTPServer",   "The given HTTP server must not be null!");

            #endregion

            RoamingNetwork_Id RoamingNetworkId;
                              RoamingNetwork    = null;
                              HTTPResponse      = null;

            if (HTTPRequest.ParsedURLParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURLParameters[0], out RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            RoamingNetwork  = HTTPServer.
                                  GetAllTenants(HTTPRequest.Host).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion

    }


    /// <summary>
    /// A HTTP API providing OICP+ data structures.
    /// </summary>
    public class OICPWebAPI
    {

        #region Data

        /// <summary>
        /// The default HTTP URI prefix.
        /// </summary>
        public static readonly HTTPPath                             DefaultURLPathPrefix        = HTTPPath.Parse("/ext/OICPPlus");

        /// <summary>
        /// The default HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public const           String                               DefaultHTTPRealm            = "Open Charging Cloud OICPPlus WebAPI";

        //ToDo: http://www.iana.org/form/media-types

        /// <summary>
        /// The HTTP content type for serving OICP+ XML data.
        /// </summary>
        public static readonly HTTPContentType                      OICPPlusXMLContentType      = new HTTPContentType("application", "vnd.OICPPlus+xml", "utf-8", null, null);

        /// <summary>
        /// The HTTP content type for serving OICP+ HTML data.
        /// </summary>
        public static readonly HTTPContentType                      OICPPlusHTMLContentType     = new HTTPContentType("application", "vnd.OICPPlus+html", "utf-8", null, null);


        private readonly XMLNamespacesDelegate                      XMLNamespaces;
        private readonly EVSE2EVSEDataRecordDelegate                EVSE2EVSEDataRecord;
        private readonly EVSEStatusUpdate2EVSEStatusRecordDelegate  EVSEStatusUpdate2EVSEStatusRecord;
        //private readonly EVSEDataRecord2XMLDelegate                 EVSEDataRecord2XML;
        //private readonly EVSEStatusRecord2XMLDelegate               EVSEStatusRecord2XML;
        private readonly XMLPostProcessingDelegate                  XMLPostProcessing;

        public static readonly HTTPEventSource_Id                   DebugLogId                  = HTTPEventSource_Id.Parse("OICPDebugLog");

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP server for serving the OICP+ WebAPI.
        /// </summary>
        public HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer          { get; }

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public HTTPPath                                     URLPathPrefix       { get; }

        /// <summary>
        /// The HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public String                                       HTTPRealm           { get; }

        /// <summary>
        /// An enumeration of logins for an optional HTTP Basic Authentication.
        /// </summary>
        public IEnumerable<KeyValuePair<String, String>>    HTTPLogins          { get; }


        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<JObject>                     DebugLog            { get; }


        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient                                    DNSClient           { get; }



        private readonly List<WWCPEMPAdapter> _CPOAdapters;

        public IEnumerable<WWCPEMPAdapter> CPOAdapters
            => _CPOAdapters;


        private readonly List<WWCPCSOAdapter> _EMPAdapters;

        public IEnumerable<WWCPCSOAdapter> EMPAdapters
            => _EMPAdapters;

        #endregion

        #region Events

        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Attach the OICP+ WebAPI to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="URLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="HTTPRealm">The HTTP realm, if HTTP Basic Authentication is used.</param>
        /// <param name="HTTPLogins">An enumeration of logins for an optional HTTP Basic Authentication.</param>
        /// 
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSE2EVSEDataRecord">An optional delegate to process an EVSE data record before converting it to XML.</param>
        /// <param name="EVSEDataRecord2XML">An optional delegate to process an EVSE data record XML before sending it somewhere.</param>
        /// <param name="EVSEStatusRecord2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public OICPWebAPI(HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                          HTTPPath?                                    URLPathPrefix                       = null,
                          String                                       HTTPRealm                           = DefaultHTTPRealm,
                          IEnumerable<KeyValuePair<String, String>>    HTTPLogins                          = null,

                          XMLNamespacesDelegate                        XMLNamespaces                       = null,
                          EVSE2EVSEDataRecordDelegate                  EVSE2EVSEDataRecord                 = null,
                          EVSEStatusUpdate2EVSEStatusRecordDelegate    EVSEStatusUpdate2EVSEStatusRecord   = null,
                          //EVSEDataRecord2XMLDelegate                   EVSEDataRecord2XML                  = null,
                          //EVSEStatusRecord2XMLDelegate                 EVSEStatusRecord2XML                = null,
                          XMLPostProcessingDelegate                    XMLPostProcessing                   = null)
        {

            this.HTTPServer                         = HTTPServer    ?? throw new ArgumentNullException(nameof(HTTPServer), "The given HTTP server must not be null!");
            this.URLPathPrefix                      = URLPathPrefix ?? DefaultURLPathPrefix;
            this.HTTPRealm                          = HTTPRealm.IsNotNullOrEmpty() ? HTTPRealm : DefaultHTTPRealm;
            this.HTTPLogins                         = HTTPLogins    ?? new KeyValuePair<String, String>[0];
            this.DNSClient                          = HTTPServer.DNSClient;

            this.XMLNamespaces                      = XMLNamespaces;
            this.EVSE2EVSEDataRecord                = EVSE2EVSEDataRecord;
            this.EVSEStatusUpdate2EVSEStatusRecord  = EVSEStatusUpdate2EVSEStatusRecord;
            //this.EVSEDataRecord2XML                 = EVSEDataRecord2XML;
            //this.EVSEStatusRecord2XML               = EVSEStatusRecord2XML;
            this.XMLPostProcessing                  = XMLPostProcessing;

            this._CPOAdapters                       = new List<WWCPEMPAdapter>();

            // Link HTTP events...
            HTTPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            HTTPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            HTTPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            Directory.CreateDirectory("HTTPSSEs");

            var LogfilePrefix                       = "HTTPSSEs" + Path.DirectorySeparatorChar;

            this.DebugLog                           = HTTPServer.AddJSONEventSource(EventIdentification:      DebugLogId,
                                                                                    URLTemplate:              this.URLPathPrefix + "/DebugLog",
                                                                                    MaxNumberOfCachedEvents:  10000,
                                                                                    RetryIntervall:           TimeSpan.FromSeconds(5),
                                                                                    EnableLogging:            true,
                                                                                    LogfilePrefix:            LogfilePrefix);

            RegisterURITemplates();

        }

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.RegisterResourcesFolder(HTTPHostname.Any,
                                               URLPathPrefix + "/",
                                               "org.GraphDefined.WWCP.OICPv2_2.WebAPI.HTTPRoot",
                                               DefaultFilename: "index.html");

            #endregion


            #region GET     ~/EVSEs

            #region EVSEsDelegate

            HTTPDelegate EVSEsDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication).Username &&
                                           kvp.Value == (Request.Authorization as HTTPBasicAuthentication).Password))
                {

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.UtcNow,
                            Connection       = "close"
                        }.AsImmutable);

                }

                #endregion

                #region Check parameters

                HTTPResponse   _HTTPResponse;
                RoamingNetwork _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                var skip = Request.QueryString.GetUInt32("skip");
                var take = Request.QueryString.GetUInt32("take");

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEs.ULongCount();

                return Task.FromResult(
                    new HTTPResponse.Builder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.UtcNow,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = _RoamingNetwork.EVSEs.
                                                            OrderBy(evse => evse.Id).
                                                            Skip   (skip).
                                                            Take   (take).
                                                            Select (evse => {

                                                                        try
                                                                        {
                                                                            return OICPMapper.ToOICP(evse, EVSE2EVSEDataRecord);
                                                                        }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                                                                        catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
                                                                        { }

                                                                        return null;

                                                                    }).
                                                                    Where(evsedatarecord => evsedatarecord != null).
                                                                    ToXML(_RoamingNetwork, XMLNamespaces, XMLPostProcessing: XMLPostProcessing).
                                                                    ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                    }.AsImmutable);

            };

            #endregion

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEs
            // -----------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}" + URLPathPrefix + "/EVSEs",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: EVSEsDelegate);

            // ----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OICPPlus+xml" http://127.0.0.1:3004/RNs/Prod/EVSEs
            // ----------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEs",
                                         OICPPlusXMLContentType,
                                         HTTPDelegate: EVSEsDelegate);

            #endregion

            #region GET     ~/EVSEStatus

            #region XMLDelegate

            HTTPDelegate EVSEStatusXMLDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication).Username &&
                                           kvp.Value == (Request.Authorization as HTTPBasicAuthentication).Password))
                {

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.UtcNow,
                            Connection       = "close"
                        }.AsImmutable);

                }

                #endregion

                #region Check parameters

                HTTPResponse   _HTTPResponse;
                RoamingNetwork _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                #region Check Query String parameters

                var skip          = Request.QueryString.GetUInt32("skip");
                var take          = Request.QueryString.GetUInt32("take");

                var statusFilter  = Request.QueryString.CreateEnumFilter<EVSE, EVSEStatusTypes>("status",
                                                                                                (evse, status) => evse.Status == status.AsWWCPEVSEStatus());

                var matchFilter   = Request.QueryString.CreateStringFilter<EVSE>  ("match",
                                                                                   (evse, match) => evse.Id.ToString().Contains(match) ||
                                                                                                    evse.ChargingStation.Name.FirstText().Contains(match));

                var sinceFilter   = Request.QueryString.CreateDateTimeFilter<EVSE>("since",
                                                                                   (evse, timestamp) => evse.Status.Timestamp >= timestamp);

                #endregion

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEStatus().ULongCount();

                return Task.FromResult(
                    new HTTPResponse.Builder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.UtcNow,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = _RoamingNetwork.EVSEs.
                                                            Where  (statusFilter).
                                                            Where  (matchFilter).
                                                            Where  (sinceFilter).
                                                            OrderBy(evse => evse.Id).
                                                            Skip   (skip).
                                                            Take   (take).
                                                            ToXML  (_RoamingNetwork,
                                                                    XMLNamespaces,
                                                                    //EVSEStatusRecord2XML,
                                                                    XMLPostProcessing: XMLPostProcessing).
                                                            ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                    }.AsImmutable);

            };

            #endregion

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/xml" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEStatus
            // ---------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}" + URLPathPrefix + "/EVSEStatus",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: EVSEStatusXMLDelegate);

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OICPPlus+xml" http://127.0.0.1:3004/RNs/Prod/EVSEStatus
            // ---------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEStatus",
                                         OICPPlusXMLContentType,
                                         HTTPDelegate: EVSEStatusXMLDelegate);


            #region HTMLDelegate

            Func<EVSE, String> EVSEStatusColorSelector = evse =>
            {
                switch (evse.Status.Value.AsOICPEVSEStatus())
                {

                    case EVSEStatusTypes.Available:
                        return @"style=""background-color: rgba(170, 232, 170, 0.55);""";

                    case EVSEStatusTypes.Reserved:
                        return @"style=""background-color: rgba(170, 170, 232, 0.55);""";

                    case EVSEStatusTypes.Occupied:
                        return @"style=""background-color: rgba(232, 170, 170, 0.55);""";

                    case EVSEStatusTypes.OutOfService:
                        return @"style=""background-color: rgba(170, 232, 232, 0.55);""";

                    default:
                        return @"style=""background-color: rgba(100, 100, 100, 0.55);""";

                }
            };


            HTTPDelegate EVSEStatusHTMLDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication).Username &&
                                           kvp.Value == (Request.Authorization as HTTPBasicAuthentication).Password))
                {

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.UtcNow,
                            Connection       = "close"
                        }.AsImmutable);

                }

                #endregion

                #region Check parameters

                HTTPResponse    _HTTPResponse;
                RoamingNetwork  _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                #region Check Query String parameters

                var skip          = Request.QueryString.GetUInt32("skip");
                var take          = Request.QueryString.GetUInt32("take");

                var statusFilter  = Request.QueryString.CreateEnumFilter<EVSE, EVSEStatusTypes>("status",
                                                                                                (evse, status) => evse.Status == status.AsWWCPEVSEStatus());

                var matchFilter   = Request.QueryString.CreateStringFilter<EVSE>  ("match",
                                                                                   (evse, match) => evse.Id.ToString().Contains(match) ||
                                                                                                    evse.ChargingStation.Name.FirstText().Contains(match));

                var sinceFilter   = Request.QueryString.CreateDateTimeFilter<EVSE>("since",
                                                                                   (evse, timestamp) => evse.Status.Timestamp >= timestamp);

                #endregion

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEStatus().ULongCount();

                return Task.FromResult(
                    new HTTPResponse.Builder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.UtcNow,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = String.Concat(@"<html>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <head>", Environment.NewLine,
                                                                      @"    <link href=""" + URLPathPrefix + @"/styles.css"" type=""text/css"" rel=""stylesheet"" />", Environment.NewLine,
                                                                      @"  </head>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <script>", Environment.NewLine,
                                                                      @"    function refresh() {", Environment.NewLine,
                                                                      @"        setTimeout(function () {", Environment.NewLine,
                                                                      @"            location.reload()", Environment.NewLine,
                                                                      @"        }, 10000);", Environment.NewLine,
                                                                      @"    }", Environment.NewLine,
                                                                      @"  </script>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <body onLoad=""refresh()"">", Environment.NewLine,
                                                                      @"    <div class=""evsestatuslist"">", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      _RoamingNetwork.EVSEs.
                                                                          Where  (statusFilter).
                                                                          Where  (matchFilter).
                                                                          Where  (sinceFilter).
                                                                          OrderBy(evse => evse.Id).
                                                                          Skip   (skip).
                                                                          Take   (take).
                                                                          Select (evse => String.Concat(@"      <div class=""evsestatus"" " + EVSEStatusColorSelector(evse) + ">", Environment.NewLine,
                                                                                                        @"        <div class=""id"">",            evse.Id.ToString(),                    @"</div>", Environment.NewLine,
                                                                                                        @"        <div class=""description"">",   evse.ChargingStation.Name.FirstText(), @"</div>", Environment.NewLine,
                                                                                                        @"        <div class=""statuslist"">", Environment.NewLine,
                                                                                                        @"          <div class=""timestampedstatus"">", Environment.NewLine,
                                                                                                        @"            <div class=""timestamp"">", evse.Status.Timestamp.ToString("dd.MM.yyyy HH:mm:ss"), @"</div>", Environment.NewLine,
                                                                                                        @"            <div class=""status"">",    evse.Status.Value.AsOICPEVSEStatus().ToString(),       @"</div>", Environment.NewLine,
                                                                                                        @"          </div>", Environment.NewLine,
                                                                                                        @"        </div>", Environment.NewLine,
                                                                                                        @"      </div>", Environment.NewLine
                                                                                                       )).
                                                                          AggregateWith(Environment.NewLine),
                                                                      Environment.NewLine,

                                                                      @"    </div>", Environment.NewLine,
                                                                      @"  </body>", Environment.NewLine,
                                                                      @"</html>", Environment.NewLine).
                                                               ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                   }.AsImmutable);

            };

            #endregion

            // ---------------------------------------------------------------------------------------
            // curl -v -H "Accept: text/html" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEStatus
            // ---------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}" + URLPathPrefix + "/EVSEStatus",
                                         HTTPContentType.HTML_UTF8,
                                         HTTPDelegate: EVSEStatusHTMLDelegate);

            // ----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OICPPlus+html" http://127.0.0.1:3004/RNs/Prod/EVSEStatus
            // ----------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEStatus",
                                         OICPPlusHTMLContentType,
                                         HTTPDelegate: EVSEStatusHTMLDelegate);

            #endregion



            #region GET     ~/PushStatus

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEs
            // -----------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/PushStatus",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: async Request  => {

                                             #region Check HTTP Basic Authentication

                                             if (Request.Authorization == null ||
                                                 !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication).Username &&
                                                                        kvp.Value == (Request.Authorization as HTTPBasicAuthentication).Password))
                                             {

                                                 return new HTTPResponse.Builder(Request) {
                                                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                                                            Server           = HTTPServer.DefaultServerName,
                                                            Date             = DateTime.UtcNow,
                                                            Connection       = "close"
                                                        }.AsImmutable;

                                             }

                                             #endregion

                                             #region Check parameters

                                             if (!Request.ParseRoamingNetwork(HTTPServer, out RoamingNetwork _RoamingNetwork, out HTTPResponse _HTTPResponse))
                                                 return _HTTPResponse;

                                             #endregion


                                             var response = await _CPOAdapters.FirstOrDefault()?.CPOClient.PushEVSEStatus(
                                                                                         new PushEVSEStatusRequest(
                                                                                             new OperatorEVSEStatus(
                                                                                                 _RoamingNetwork.EVSEs.Select(evse => {
                                                                                                     try
                                                                                                     {
                                                                                                         var s = evse.Id.ToString();
                                                                                                         if (s.StartsWith("DE*BDO") || s.StartsWith("DE*SLB") || s.StartsWith("+49"))
                                                                                                            return new EVSEStatusRecord(evse.Id.ToOICP().Value, evse.Status.Value.AsOICPEVSEStatus());
                                                                                                         return null;
                                                                                                     }
                                                                                                     catch (Exception e)
                                                                                                     {
                                                                                                         return null;
                                                                                                     }
                                                                                                 }).Where(evse => evse != null),
                                                                                                 Operator_Id.Parse("DE*BDO")
                                                                                             ),
                                                                                             ActionTypes.update));

                                             return new HTTPResponse.Builder(Request) {
                                                 HTTPStatusCode                = HTTPStatusCode.OK,
                                                 Server                        = HTTPServer.DefaultServerName,
                                                 Date                          = DateTime.UtcNow,
                                                 AccessControlAllowOrigin      = "*",
                                                 AccessControlAllowMethods     = "GET",
                                                 AccessControlAllowHeaders     = "Content-Type, Authorization",
                                                 ETag                          = "1",
                                                 ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                                                 Content                       = response.Content.ToXML().ToUTF8Bytes()
                                             }.AsImmutable;

                                         });

                                         #endregion

        }

        #endregion



        public void Add(WWCPEMPAdapter CPOAdapter)
        {

            _CPOAdapters.Add(CPOAdapter);

            // CPOClient

            #region OnPushEVSEDataRequest/-Response

            CPOAdapter.CPOClient.OnPushEVSEDataRequest += async (LogTimestamp,
                                                                 RequestTimestamp,
                                                                 Sender,
                                                                 SenderId,
                                                                 EventTrackingId,
                                                                 Action,
                                                                 NumberOfEVSEDataRecords,
                                                                 EVSEDataRecords,
                                                                 RequestTimeout) => await DebugLog.SubmitEvent("PushEVSEDataRequest",
                                                                                                               JSONObject.Create(
                                                                                                                   new JProperty("timestamp",                RequestTimestamp.ToIso8601()),
                                                                                                                   new JProperty("eventTrackingId",          EventTrackingId. ToString()),

                                                                                                                   new JProperty("action",                   Action.          ToString()),
                                                                                                                   new JProperty("numberOfEVSEDataRecords",  NumberOfEVSEDataRecords),
                                                                                                                   new JProperty("EVSEDataRecords",          new JArray(EVSEDataRecords.SafeSelect(evseDataRecord => evseDataRecord.Id.ToString()))),
                                                                                                                   new JProperty("requestTimeout",           Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                               ));

            CPOAdapter.CPOClient.OnPushEVSEDataResponse += async (LogTimestamp,
                                                                  RequestTimestamp,
                                                                  Sender,
                                                                  SenderId,
                                                                  EventTrackingId,
                                                                  Action,
                                                                  NumberOfEVSEDataRecords,
                                                                  EVSEDataRecords,
                                                                  RequestTimeout,
                                                                  Result,
                                                                  Runtime) => await DebugLog.SubmitEvent("PushEVSEDataResponse",
                                                                                                         JSONObject.Create(
                                                                                                             new JProperty("timestamp",                RequestTimestamp.ToIso8601()),
                                                                                                             new JProperty("eventTrackingId",          EventTrackingId. ToString()),

                                                                                                             new JProperty("action",                   Action.          ToString()),
                                                                                                             new JProperty("numberOfEVSEDataRecords",  NumberOfEVSEDataRecords),
                                                                                                             new JProperty("EVSEDataRecords",          new JArray(EVSEDataRecords.SafeSelect(evseDataRecord => evseDataRecord.Id.ToString()))),
                                                                                                             new JProperty("requestTimeout",           Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                             new JProperty("result",                   Result.          ToJSON()),
                                                                                                             new JProperty("runtime",                  Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                         ));

            #endregion

            #region OnPushEVSEStatusRequest/-Response

            CPOAdapter.CPOClient.OnPushEVSEStatusRequest += async (LogTimestamp,
                                                                   RequestTimestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   Action,
                                                                   NumberOfEVSEDataRecords,
                                                                   EVSEStatusRecords,
                                                                   RequestTimeout) => await DebugLog.SubmitEvent("PushEVSEStatusRequest",
                                                                                                                 JSONObject.Create(
                                                                                                                     new JProperty("timestamp",                  RequestTimestamp.ToIso8601()),
                                                                                                                     new JProperty("eventTrackingId",            EventTrackingId. ToString()),

                                                                                                                     new JProperty("action",                     Action.          ToString()),
                                                                                                                     new JProperty("numberOfEVSEStatusRecords",  NumberOfEVSEDataRecords),
                                                                                                                     new JProperty("EVSEStatusRecords",          new JArray(EVSEStatusRecords.SafeSelect(evseStatusRecord => new JArray(evseStatusRecord.Id.    ToString(),
                                                                                                                                                                                                                                        evseStatusRecord.Status.ToString())))),
                                                                                                                     new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                 ));

            CPOAdapter.CPOClient.OnPushEVSEStatusResponse += async (LogTimestamp,
                                                                    RequestTimestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    Action,
                                                                    NumberOfEVSEDataRecords,
                                                                    EVSEStatusRecords,
                                                                    RequestTimeout,
                                                                    Result,
                                                                    Runtime) => await DebugLog.SubmitEvent("PushEVSEStatusResponse",
                                                                                                           JSONObject.Create(
                                                                                                               new JProperty("timestamp",                  RequestTimestamp.ToIso8601()),
                                                                                                               new JProperty("eventTrackingId",            EventTrackingId. ToString()),

                                                                                                               new JProperty("action",                     Action.          ToString()),
                                                                                                               new JProperty("numberOfEVSEStatusRecords",  NumberOfEVSEDataRecords),
                                                                                                               new JProperty("EVSEStatusRecords",          new JArray(EVSEStatusRecords.SafeSelect(evseStatusRecord => new JArray(evseStatusRecord.Id.    ToString(),
                                                                                                                                                                                                                                  evseStatusRecord.Status.ToString())))),
                                                                                                               new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                               new JProperty("result",                     Result.          ToJSON()),
                                                                                                               new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                           ));

            #endregion


            #region OnAuthorizeStartRequest/-Response

            CPOAdapter.CPOClient.OnAuthorizeStartRequest += async  (LogTimestamp,
                                                                    RequestTimestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    OperatorId,
                                                                    Identification,
                                                                    EVSEId,
                                                                    SessionId,
                                                                    PartnerProductId,
                                                                    CPOPartnerSessionId,
                                                                    EMPPartnerSessionId,
                                                                    RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeStartRequest",
                                                                                                                  JSONObject.Create(
                                                                                                                      new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                                      new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                                      //new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                                                                                                                      //EMPRoamingProviderId.HasValue
                                                                                                                      //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                                      //    : null,
                                                                                                                      //CSORoamingProviderId.HasValue
                                                                                                                      //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                                      //    : null,
                                                                                                                      new JProperty("operatorId",                  OperatorId.          ToString()),
                                                                                                                      new JProperty("identification",              Identification.      ToJSON()),
                                                                                                                      EVSEId.HasValue
                                                                                                                         ? new JProperty("EVSEId",                 EVSEId.              ToString())
                                                                                                                         : null,
                                                                                                                      SessionId.HasValue
                                                                                                                          ? new JProperty("sessionId",             SessionId.           ToString())
                                                                                                                          : null,
                                                                                                                      PartnerProductId.HasValue
                                                                                                                          ? new JProperty("partnerProductId",      PartnerProductId.    ToString())
                                                                                                                          : null,
                                                                                                                      CPOPartnerSessionId.HasValue
                                                                                                                          ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                                                                                                                          : null,
                                                                                                                      EMPPartnerSessionId.HasValue
                                                                                                                          ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId. ToString())
                                                                                                                          : null,
                                                                                                                      new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                 ));

            CPOAdapter.CPOClient.OnAuthorizeStartResponse += async (LogTimestamp,
                                                                    RequestTimestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    OperatorId,
                                                                    Identification,
                                                                    EVSEId,
                                                                    SessionId,
                                                                    PartnerProductId,
                                                                    CPOPartnerSessionId,
                                                                    EMPPartnerSessionId,
                                                                    RequestTimeout,
                                                                    Result,
                                                                    Runtime) => await DebugLog.SubmitEvent("AuthorizeStartResponse",
                                                                                                           JSONObject.Create(
                                                                                                               new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                               new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                               //new JProperty("roamingNetworkId",            RoamingNetwork.Id.   ToString()),
                                                                                                               //EMPRoamingProviderId.HasValue
                                                                                                               //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                               //    : null,
                                                                                                               //CSORoamingProviderId.HasValue
                                                                                                               //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                               //    : null,
                                                                                                               new JProperty("operatorId",                  OperatorId.          ToString()),
                                                                                                               new JProperty("identification",              Identification.      ToJSON()),
                                                                                                               EVSEId.HasValue
                                                                                                                   ? new JProperty("EVSEId",                EVSEId.              ToString())
                                                                                                                   : null,
                                                                                                               SessionId.HasValue
                                                                                                                   ? new JProperty("sessionId",             SessionId.           ToString())
                                                                                                                   : null,
                                                                                                               PartnerProductId.HasValue
                                                                                                                   ? new JProperty("partnerProductId",      PartnerProductId.    ToString())
                                                                                                                   : null,
                                                                                                               CPOPartnerSessionId.HasValue
                                                                                                                   ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                                                                                                                   : null,
                                                                                                               EMPPartnerSessionId.HasValue
                                                                                                                   ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId. ToString())
                                                                                                                   : null,
                                                                                                               new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                               new JProperty("result",                      Result.              ToJSON()),
                                                                                                               new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                           ));

            #endregion

            #region OnAuthorizeStopRequest/-Response

            CPOAdapter.CPOClient.OnAuthorizeStopRequest += async  (LogTimestamp,
                                                                   RequestTimestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   OperatorId,
                                                                   SessionId,
                                                                   Identification,
                                                                   EVSEId,
                                                                   CPOPartnerSessionId,
                                                                   EMPPartnerSessionId,
                                                                   RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeStopRequest",
                                                                                                                 JSONObject.Create(
                                                                                                                     new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                                     new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                                     //new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                                                                                                                     //EMPRoamingProviderId.HasValue
                                                                                                                     //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                                     //    : null,
                                                                                                                     //CSORoamingProviderId.HasValue
                                                                                                                     //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                                     //    : null,
                                                                                                                     new JProperty("operatorId",                  OperatorId.          ToString()),
                                                                                                                     new JProperty("identification",              Identification.      ToJSON()),
                                                                                                                     EVSEId.HasValue
                                                                                                                         ? new JProperty("EVSEId",                EVSEId.              ToString())
                                                                                                                         : null,
                                                                                                                     CPOPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                                                                                                                         : null,
                                                                                                                     EMPPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId. ToString())
                                                                                                                         : null,
                                                                                                                     new JProperty("sessionId",                   SessionId.           ToString()),
                                                                                                                     new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                ));

            CPOAdapter.CPOClient.OnAuthorizeStopResponse += async (LogTimestamp,
                                                                   RequestTimestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   OperatorId,
                                                                   SessionId,
                                                                   Identification,
                                                                   EVSEId,
                                                                   CPOPartnerSessionId,
                                                                   EMPPartnerSessionId,
                                                                   RequestTimeout,
                                                                   Result,
                                                                   Runtime) => await DebugLog.SubmitEvent("AuthorizeStopResponse",
                                                                                                          JSONObject.Create(
                                                                                                              new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                              new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                              //new JProperty("roamingNetworkId",            RoamingNetwork.Id.   ToString()),
                                                                                                              //EMPRoamingProviderId.HasValue
                                                                                                              //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                              //    : null,
                                                                                                              //CSORoamingProviderId.HasValue
                                                                                                              //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                              //    : null,
                                                                                                              new JProperty("operatorId",                  OperatorId.          ToString()),
                                                                                                              new JProperty("sessionId",                   SessionId.           ToString()),
                                                                                                              new JProperty("identification",              Identification.      ToJSON()),
                                                                                                              EVSEId.HasValue
                                                                                                                  ? new JProperty("EVSEId",                EVSEId.              ToString())
                                                                                                                  : null,
                                                                                                              CPOPartnerSessionId.HasValue
                                                                                                                  ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId. ToString())
                                                                                                                  : null,
                                                                                                              EMPPartnerSessionId.HasValue
                                                                                                                  ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId. ToString())
                                                                                                                  : null,
                                                                                                              new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                              new JProperty("result",                      Result.              ToJSON()),
                                                                                                              new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                          ));

            #endregion

            #region OnSendChargeDetailRecordRequest/-Response

            CPOAdapter.CPOClient.OnSendChargeDetailRecordRequest += async  (LogTimestamp,
                                                                            RequestTimestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            ChargeDetailRecord,
                                                                            RequestTimeout) => await DebugLog.SubmitEvent("SendChargeDetailRecordRequest",
                                                                                                                          JSONObject.Create(
                                                                                                                              new JProperty("timestamp",           RequestTimestamp.    ToIso8601()),
                                                                                                                              new JProperty("eventTrackingId",     EventTrackingId.     ToString()),
                                                                                                                              new JProperty("chargeDetailRecord",  ChargeDetailRecord.  ToJSON()),
                                                                                                                              new JProperty("requestTimeout",      Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                         ));

            CPOAdapter.CPOClient.OnSendChargeDetailRecordResponse += async (Timestamp,
                                                                            RequestTimestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            ChargeDetailRecord,
                                                                            RequestTimeout,
                                                                            Result,
                                                                            Runtime) => await DebugLog.SubmitEvent("SendChargeDetailRecordResponse",
                                                                                                                   JSONObject.Create(
                                                                                                                       new JProperty("timestamp",          RequestTimestamp.    ToIso8601()),
                                                                                                                       new JProperty("eventTrackingId",    EventTrackingId.     ToString()),
                                                                                                                       new JProperty("chargeDetailRecord", ChargeDetailRecord.  ToJSON()),
                                                                                                                       new JProperty("requestTimeout",     Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                       new JProperty("result",             Result.              ToJSON()),
                                                                                                                       new JProperty("runtime",            Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                   ));

            #endregion


            #region OnPullAuthenticationDataRequest/-Response

            CPOAdapter.CPOClient.OnPullAuthenticationDataRequest += async  (LogTimestamp,
                                                                            RequestTimestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            OperatorId,
                                                                            RequestTimeout) => await DebugLog.SubmitEvent("PullAuthenticationDataRequest",
                                                                                                                          JSONObject.Create(
                                                                                                                              new JProperty("timestamp",        RequestTimestamp.ToIso8601()),
                                                                                                                              new JProperty("eventTrackingId",  EventTrackingId. ToString()),
                                                                                                                              new JProperty("operatorId",       OperatorId.      ToString()),
                                                                                                                              new JProperty("requestTimeout",   Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                         ));

            CPOAdapter.CPOClient.OnPullAuthenticationDataResponse += async (Timestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            OperatorId,
                                                                            RequestTimeout,
                                                                            Result,
                                                                            Runtime) => await DebugLog.SubmitEvent("PullAuthenticationDataResponse",
                                                                                                                   JSONObject.Create(
                                                                                                                       new JProperty("timestamp",           Timestamp.           ToIso8601()),
                                                                                                                       new JProperty("eventTrackingId",     EventTrackingId.     ToString()),
                                                                                                                       new JProperty("operatorId",          OperatorId.          ToString()),
                                                                                                                       new JProperty("requestTimeout",      Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                       new JProperty("numberOfRecords",     Result.ProviderAuthenticationDataRecords.Count()),
                                                                                                                       Result.StatusCode.HasValue
                                                                                                                           ? new JProperty("resultStatus",  Result.StatusCode.Value.ToJSON())
                                                                                                                           : null,
                                                                                                                       new JProperty("runtime",          Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                   ));

            #endregion


            // CPOServer

            #region OnAuthorizeRemoteReservationStartRequest/-Response

            CPOAdapter.CPOServer.OnAuthorizeRemoteReservationStartRequest += async  (LogTimestamp,
                                                                                     RequestTimestamp,
                                                                                     Sender,
                                                                                     SenderId,
                                                                                     EventTrackingId,
                                                                                     EVSEId,
                                                                                     PartnerProductId,
                                                                                     SessionId,
                                                                                     CPOPartnerSessionId,
                                                                                     EMPPartnerSessionId,
                                                                                     ProviderId,
                                                                                     Identification,
                                                                                     RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStartRequest",
                                                                                                                                   JSONObject.Create(
                                                                                                                                       new JProperty("timestamp",                   RequestTimestamp.         ToIso8601()),
                                                                                                                                       new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                                       new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                                       PartnerProductId.HasValue
                                                                                                                                           ? new JProperty("partnerProductId",      PartnerProductId.   Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       SessionId.HasValue
                                                                                                                                           ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       CPOPartnerSessionId.HasValue
                                                                                                                                           ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       EMPPartnerSessionId.HasValue
                                                                                                                                           ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       ProviderId.HasValue
                                                                                                                                           ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       new JProperty("Identification",              Identification.           ToJSON()),
                                                                                                                                       new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                                  ));

            CPOAdapter.CPOServer.OnAuthorizeRemoteReservationStartResponse += async (Timestamp,
                                                                                     Sender,
                                                                                     SenderId,
                                                                                     EventTrackingId,
                                                                                     EVSEId,
                                                                                     PartnerProductId,
                                                                                     SessionId,
                                                                                     CPOPartnerSessionId,
                                                                                     EMPPartnerSessionId,
                                                                                     ProviderId,
                                                                                     Identification,
                                                                                     RequestTimeout,
                                                                                     Result,
                                                                                     Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStartResponse",
                                                                                                                            JSONObject.Create(
                                                                                                                                new JProperty("timestamp",                   Timestamp.                ToIso8601()),
                                                                                                                                new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                                new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                                PartnerProductId.HasValue
                                                                                                                                    ? new JProperty("partnerProductId",      PartnerProductId.   Value.ToString())
                                                                                                                                    : null,
                                                                                                                                SessionId.HasValue
                                                                                                                                    ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                                    : null,
                                                                                                                                CPOPartnerSessionId.HasValue
                                                                                                                                    ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                                    : null,
                                                                                                                                EMPPartnerSessionId.HasValue
                                                                                                                                    ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                                    : null,
                                                                                                                                ProviderId.HasValue
                                                                                                                                    ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                                    : null,
                                                                                                                                new JProperty("Identification",              Identification.           ToJSON()),
                                                                                                                                new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                                new JProperty("result",                      Result.                   ToJSON()),
                                                                                                                                new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                            ));

            #endregion

            #region OnAuthorizeRemoteReservationStopRequest/-Response

            CPOAdapter.CPOServer.OnAuthorizeRemoteReservationStopRequest += async  (LogTimestamp,
                                                                                    RequestTimestamp,
                                                                                    Sender,
                                                                                    SenderId,
                                                                                    EventTrackingId,
                                                                                    EVSEId,
                                                                                    SessionId,
                                                                                    CPOPartnerSessionId,
                                                                                    EMPPartnerSessionId,
                                                                                    ProviderId,
                                                                                    RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStopRequest",
                                                                                                                                   JSONObject.Create(
                                                                                                                                       new JProperty("timestamp",                   RequestTimestamp.         ToIso8601()),
                                                                                                                                       new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                                       new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                                       SessionId.HasValue
                                                                                                                                           ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       CPOPartnerSessionId.HasValue
                                                                                                                                           ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       EMPPartnerSessionId.HasValue
                                                                                                                                           ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       ProviderId.HasValue
                                                                                                                                           ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                                           : null,
                                                                                                                                       new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                                  ));

            CPOAdapter.CPOServer.OnAuthorizeRemoteReservationStopResponse += async (Timestamp,
                                                                                    Sender,
                                                                                    SenderId,
                                                                                    EventTrackingId,
                                                                                    EVSEId,
                                                                                    SessionId,
                                                                                    CPOPartnerSessionId,
                                                                                    EMPPartnerSessionId,
                                                                                    ProviderId,
                                                                                    RequestTimeout,
                                                                                    Result,
                                                                                    Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStopResponse",
                                                                                                                           JSONObject.Create(
                                                                                                                               new JProperty("timestamp",                   Timestamp.                ToIso8601()),
                                                                                                                               new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                               new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                               SessionId.HasValue
                                                                                                                                   ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                                   : null,
                                                                                                                               CPOPartnerSessionId.HasValue
                                                                                                                                   ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                                   : null,
                                                                                                                               EMPPartnerSessionId.HasValue
                                                                                                                                   ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                                   : null,
                                                                                                                               ProviderId.HasValue
                                                                                                                                   ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                                   : null,
                                                                                                                               new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                               new JProperty("result",                      Result.                   ToJSON()),
                                                                                                                               new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                           ));

            #endregion


            #region OnAuthorizeRemoteStartRequest/-Response

            CPOAdapter.CPOServer.OnAuthorizeRemoteStartRequest += async  (LogTimestamp,
                                                                          RequestTimestamp,
                                                                          Sender,
                                                                          SenderId,
                                                                          EventTrackingId,
                                                                          EVSEId,
                                                                          PartnerProductId,
                                                                          SessionId,
                                                                          CPOPartnerSessionId,
                                                                          EMPPartnerSessionId,
                                                                          ProviderId,
                                                                          Identification,
                                                                          RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteStartRequest",
                                                                                                                        JSONObject.Create(
                                                                                                                            new JProperty("timestamp",                   RequestTimestamp.         ToIso8601()),
                                                                                                                            new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                            new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                            PartnerProductId.HasValue
                                                                                                                                ? new JProperty("partnerProductId",      PartnerProductId.   Value.ToString())
                                                                                                                                : null,
                                                                                                                            SessionId.HasValue
                                                                                                                                ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                                : null,
                                                                                                                            CPOPartnerSessionId.HasValue
                                                                                                                                ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                                : null,
                                                                                                                            EMPPartnerSessionId.HasValue
                                                                                                                                ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                                : null,
                                                                                                                            ProviderId.HasValue
                                                                                                                                ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                                : null,
                                                                                                                            new JProperty("Identification",              Identification.           ToJSON()),
                                                                                                                            new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                       ));

            CPOAdapter.CPOServer.OnAuthorizeRemoteStartResponse += async (Timestamp,
                                                                          Sender,
                                                                          SenderId,
                                                                          EventTrackingId,
                                                                          EVSEId,
                                                                          PartnerProductId,
                                                                          SessionId,
                                                                          CPOPartnerSessionId,
                                                                          EMPPartnerSessionId,
                                                                          ProviderId,
                                                                          Identification,
                                                                          RequestTimeout,
                                                                          Result,
                                                                          Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteStartResponse",
                                                                                                                 JSONObject.Create(
                                                                                                                     new JProperty("timestamp",                   Timestamp.                ToIso8601()),
                                                                                                                     new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                     new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                     PartnerProductId.HasValue
                                                                                                                         ? new JProperty("partnerProductId",      PartnerProductId.   Value.ToString())
                                                                                                                         : null,
                                                                                                                     SessionId.HasValue
                                                                                                                         ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                         : null,
                                                                                                                     CPOPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                         : null,
                                                                                                                     EMPPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                         : null,
                                                                                                                     ProviderId.HasValue
                                                                                                                         ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                         : null,
                                                                                                                     new JProperty("Identification",              Identification.           ToJSON()),
                                                                                                                     new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                     new JProperty("result",                      Result.                   ToJSON()),
                                                                                                                     new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                 ));

            #endregion

            #region OnAuthorizeRemoteStopRequest/-Response

            CPOAdapter.CPOServer.OnAuthorizeRemoteStopRequest += async  (LogTimestamp,
                                                                         RequestTimestamp,
                                                                         Sender,
                                                                         SenderId,
                                                                         EventTrackingId,
                                                                         EVSEId,
                                                                         SessionId,
                                                                         CPOPartnerSessionId,
                                                                         EMPPartnerSessionId,
                                                                         ProviderId,
                                                                         RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteStopRequest",
                                                                                                                        JSONObject.Create(
                                                                                                                            new JProperty("timestamp",                   RequestTimestamp.         ToIso8601()),
                                                                                                                            new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                            new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                            SessionId.HasValue
                                                                                                                                ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                                : null,
                                                                                                                            CPOPartnerSessionId.HasValue
                                                                                                                                ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                                : null,
                                                                                                                            EMPPartnerSessionId.HasValue
                                                                                                                                ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                                : null,
                                                                                                                            ProviderId.HasValue
                                                                                                                                ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                                : null,
                                                                                                                            new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                       ));

            CPOAdapter.CPOServer.OnAuthorizeRemoteStopResponse += async (Timestamp,
                                                                         Sender,
                                                                         SenderId,
                                                                         EventTrackingId,
                                                                         EVSEId,
                                                                         SessionId,
                                                                         CPOPartnerSessionId,
                                                                         EMPPartnerSessionId,
                                                                         ProviderId,
                                                                         RequestTimeout,
                                                                         Result,
                                                                         Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteStopResponse",
                                                                                                                JSONObject.Create(
                                                                                                                    new JProperty("timestamp",                   Timestamp.                ToIso8601()),
                                                                                                                    new JProperty("eventTrackingId",             EventTrackingId.          ToString()),

                                                                                                                    new JProperty("EVSEId",                      EVSEId.                   ToString()),
                                                                                                                    SessionId.HasValue
                                                                                                                        ? new JProperty("sessionId",             SessionId.          Value.ToString())
                                                                                                                        : null,
                                                                                                                    CPOPartnerSessionId.HasValue
                                                                                                                        ? new JProperty("CPOPartnerSessionId",   CPOPartnerSessionId.Value.ToString())
                                                                                                                        : null,
                                                                                                                    EMPPartnerSessionId.HasValue
                                                                                                                        ? new JProperty("EMPPartnerSessionId",   EMPPartnerSessionId.Value.ToString())
                                                                                                                        : null,
                                                                                                                    ProviderId.HasValue
                                                                                                                        ? new JProperty("providerId",            ProviderId.         Value.ToString())
                                                                                                                        : null,
                                                                                                                    new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                    new JProperty("result",                      Result.                   ToJSON()),
                                                                                                                    new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                ));

            #endregion


        }


        public void Add(WWCPCSOAdapter EMPAdapter)
        {

            _EMPAdapters.Add(EMPAdapter);

            // EMPClient

            #region OnPullEVSEDataRequest/-Response

            EMPAdapter.EMPClient.OnPullEVSEDataRequest += async (LogTimestamp,
                                                                 RequestTimestamp,
                                                                 Sender,
                                                                 SenderId,
                                                                 EventTrackingId,
                                                                 ProviderId,
                                                                 SearchCenter,
                                                                 DistanceKM,
                                                                 LastCall,
                                                                 GeoCoordinatesResponseFormat,
                                                                 RequestTimeout) => await DebugLog.SubmitEvent("PullEVSEDataRequest",
                                                                                                               JSONObject.Create(
                                                                                                                   new JProperty("timestamp",                     RequestTimestamp.                      ToIso8601()),
                                                                                                                   new JProperty("eventTrackingId",               EventTrackingId.                       ToString()),

                                                                                                                   new JProperty("providerId",                    ProviderId.                            ToString()),
                                                                                                                   SearchCenter.HasValue
                                                                                                                       ? new JProperty("searchCenter",            SearchCenter.                    Value.ToJSON())
                                                                                                                       : null,
                                                                                                                   new JProperty("distanceKM",                    DistanceKM.                            ToString()),
                                                                                                                   LastCall.HasValue
                                                                                                                       ? new JProperty("lastCall",                LastCall.                        Value.ToIso8601())
                                                                                                                       : null,
                                                                                                                   new JProperty("geoCoordinatesResponseFormat",  GeoCoordinatesResponseFormat.          ToString()),
                                                                                                                   new JProperty("requestTimeout",                Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                               ));

            EMPAdapter.EMPClient.OnPullEVSEDataResponse += async (Timestamp,
                                                                  Sender,
                                                                  SenderId,
                                                                  EventTrackingId,
                                                                  ProviderId,
                                                                  SearchCenter,
                                                                  DistanceKM,
                                                                  LastCall,
                                                                  GeoCoordinatesResponseFormat,
                                                                  RequestTimeout,
                                                                  EVSEData,
                                                                  StatusCode,
                                                                  Runtime) => await DebugLog.SubmitEvent("PullEVSEDataResponse",
                                                                                                         JSONObject.Create(
                                                                                                             new JProperty("timestamp",                     Timestamp.                             ToIso8601()),
                                                                                                             new JProperty("eventTrackingId",               EventTrackingId.                       ToString()),

                                                                                                             new JProperty("providerId",                    ProviderId.                            ToString()),
                                                                                                             SearchCenter.HasValue
                                                                                                                 ? new JProperty("searchCenter",            SearchCenter.                    Value.ToJSON())
                                                                                                                 : null,
                                                                                                             new JProperty("distanceKM",                    DistanceKM.                            ToString()),
                                                                                                             LastCall.HasValue
                                                                                                                 ? new JProperty("lastCall",                LastCall.                        Value.ToIso8601())
                                                                                                                 : null,
                                                                                                             new JProperty("geoCoordinatesResponseFormat",  GeoCoordinatesResponseFormat.          ToString()),
                                                                                                             new JProperty("requestTimeout",                Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                             EVSEData.OperatorEVSEData.SafeAny()
                                                                                                                   ? new JProperty("EVSEOperators",         EVSEData.OperatorEVSEData.SafeSelect(operatorData =>
                                                                                                                                                                JSONObject.Create(
                                                                                                                                                                    new JProperty("id",          operatorData.OperatorId.ToString()),
                                                                                                                                                                    operatorData.OperatorName.IsNotNullOrEmpty()
                                                                                                                                                                        ? new JProperty("name",  operatorData.OperatorName)
                                                                                                                                                                        : null,
                                                                                                                                                                    new JProperty("dataRecords",  new JArray(operatorData.EVSEDataRecords.SafeSelect(dataRecord => dataRecord.Id.    ToString())))
                                                                                                                                                                ))
                                                                                                                                                            )
                                                                                                                   : null,
                                                                                                             StatusCode.HasValue
                                                                                                                 ? new JProperty("statusCode",              StatusCode.                      Value.ToJSON())
                                                                                                                 : null,
                                                                                                             new JProperty("runtime",                       Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                         ));

            #endregion

            #region OnPullEVSEStatusRequest/-Response

            EMPAdapter.EMPClient.OnPullEVSEStatusRequest += async (LogTimestamp,
                                                                   RequestTimestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   ProviderId,
                                                                   SearchCenter,
                                                                   DistanceKM,
                                                                   EVSEStatusFilter,
                                                                   RequestTimeout) => await DebugLog.SubmitEvent("PullEVSEStatusRequest",
                                                                                                                 JSONObject.Create(
                                                                                                                     new JProperty("timestamp",               RequestTimestamp.      ToIso8601()),
                                                                                                                     new JProperty("eventTrackingId",         EventTrackingId.       ToString()),

                                                                                                                     new JProperty("providerId",              ProviderId.            ToString()),
                                                                                                                     SearchCenter.HasValue
                                                                                                                         ? new JProperty("searchCenter",      SearchCenter.    Value.ToJSON())
                                                                                                                         : null,
                                                                                                                     new JProperty("distanceKM",              DistanceKM.            ToString()),
                                                                                                                     EVSEStatusFilter.HasValue
                                                                                                                         ? new JProperty("EVSEStatusFilter",  EVSEStatusFilter.Value.ToString())
                                                                                                                         : null,
                                                                                                                     new JProperty("requestTimeout",          Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                 ));

            EMPAdapter.EMPClient.OnPullEVSEStatusResponse += async (Timestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    ProviderId,
                                                                    SearchCenter,
                                                                    DistanceKM,
                                                                    EVSEStatusFilter,
                                                                    RequestTimeout,
                                                                    EVSEStatus,
                                                                    Runtime) => await DebugLog.SubmitEvent("PullEVSEStatusResponse",
                                                                                                           JSONObject.Create(
                                                                                                               new JProperty("timestamp",               Timestamp.              ToIso8601()),
                                                                                                               new JProperty("eventTrackingId",         EventTrackingId.        ToString()),

                                                                                                               new JProperty("providerId",              ProviderId.             ToString()),
                                                                                                               SearchCenter.HasValue
                                                                                                                   ? new JProperty("searchCenter",      SearchCenter.     Value.ToJSON())
                                                                                                                   : null,
                                                                                                               new JProperty("distanceKM",              DistanceKM.             ToString()),
                                                                                                               EVSEStatusFilter.HasValue
                                                                                                                   ? new JProperty("EVSEStatusFilter",  EVSEStatusFilter. Value.ToString())
                                                                                                                   : null,
                                                                                                               new JProperty("requestTimeout",          Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                               EVSEStatus.OperatorEVSEStatus.SafeAny()
                                                                                                                   ? new JProperty("EVSEOperators",     EVSEStatus.OperatorEVSEStatus.SafeSelect(operatorStatus =>
                                                                                                                                                            JSONObject.Create(
                                                                                                                                                                new JProperty("id",          operatorStatus.OperatorId.ToString()),
                                                                                                                                                                operatorStatus.OperatorName.IsNotNullOrEmpty()
                                                                                                                                                                    ? new JProperty("name",  operatorStatus.OperatorName)
                                                                                                                                                                    : null,
                                                                                                                                                                new JProperty("statusRecords",  new JArray(
                                                                                                                                                                    operatorStatus.EVSEStatusRecords.SafeSelect(statusRecord => new JArray(statusRecord.Id.    ToString(),
                                                                                                                                                                                                                                           statusRecord.Status.ToString()))))
                                                                                                                                                            ))
                                                                                                                                                        )
                                                                                                                   : null,
                                                                                                               EVSEStatus.StatusCode.HasValue
                                                                                                                   ? new JProperty("resultStatus",      EVSEStatus.StatusCode.Value.ToJSON())
                                                                                                                   : null,
                                                                                                               new JProperty("runtime",                 Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                           ));

            #endregion

            #region OnPullEVSEStatusByIdRequest/-Response

            EMPAdapter.EMPClient.OnPullEVSEStatusByIdRequest += async (LogTimestamp,
                                                                       RequestTimestamp,
                                                                       Sender,
                                                                       SenderId,
                                                                       EventTrackingId,
                                                                       ProviderId,
                                                                       EVSEIds,
                                                                       RequestTimeout) => await DebugLog.SubmitEvent("PullEVSEStatusByIdRequest",
                                                                                                                     JSONObject.Create(
                                                                                                                         new JProperty("timestamp",        RequestTimestamp.ToIso8601()),
                                                                                                                         new JProperty("eventTrackingId",  EventTrackingId. ToString()),

                                                                                                                         new JProperty("providerId",       ProviderId.      ToString()),
                                                                                                                         EVSEIds.SafeAny()
                                                                                                                             ? new JProperty("EVSEIds",    new JArray(EVSEIds))
                                                                                                                             : null,
                                                                                                                         new JProperty("requestTimeout",   Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                     ));

            EMPAdapter.EMPClient.OnPullEVSEStatusByIdResponse += async (Timestamp,
                                                                        Sender,
                                                                        SenderId,
                                                                        EventTrackingId,
                                                                        ProviderId,
                                                                        EVSEIds,
                                                                        RequestTimeout,
                                                                        EVSEStatusById,
                                                                        Runtime) => await DebugLog.SubmitEvent("PullEVSEStatusByIdResponse",
                                                                                                               JSONObject.Create(
                                                                                                                   new JProperty("timestamp",                Timestamp.              ToIso8601()),
                                                                                                                   new JProperty("eventTrackingId",          EventTrackingId.        ToString()),

                                                                                                                   new JProperty("providerId",               ProviderId.      ToString()),
                                                                                                                   EVSEIds.SafeAny()
                                                                                                                       ? new JProperty("EVSEIds",            new JArray(EVSEIds))
                                                                                                                       : null,
                                                                                                                   new JProperty("requestTimeout",           Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                   EVSEStatusById.EVSEStatusRecords.SafeAny()
                                                                                                                       ? new JProperty("EVSEStatusRecords",  new JArray(
                                                                                                                                                                 EVSEStatusById.EVSEStatusRecords.SafeSelect(statusRecord => new JArray(statusRecord.Id.    ToString(),
                                                                                                                                                                                                                                        statusRecord.Status.ToString()))
                                                                                                                                                             ))
                                                                                                                       : null,
                                                                                                                   EVSEStatusById.StatusCode.HasValue
                                                                                                                       ? new JProperty("resultStatus",       EVSEStatusById.StatusCode.Value.ToJSON())
                                                                                                                       : null,
                                                                                                                   new JProperty("runtime",                  Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                               ));

            #endregion


            #region OnPushAuthenticationDataRequest/-Response

            EMPAdapter.EMPClient.OnPushAuthenticationDataRequest += async (LogTimestamp,
                                                                           RequestTimestamp,
                                                                           Sender,
                                                                           SenderId,
                                                                           EventTrackingId,
                                                                           ProviderAuthenticationData,
                                                                           OICPAction,
                                                                           RequestTimeout) => await DebugLog.SubmitEvent("PushAuthenticationDataRequest",
                                                                                                                         JSONObject.Create(
                                                                                                                             new JProperty("timestamp",        RequestTimestamp.ToIso8601()),
                                                                                                                             new JProperty("eventTrackingId",  EventTrackingId. ToString()),

                                                                                                                             ProviderAuthenticationData.AuthorizationIdentifications.SafeAny()
                                                                                                                                 ? new JProperty("authorizationIdentifications",  new JArray(ProviderAuthenticationData.AuthorizationIdentifications.SafeSelect(auth => auth.ToJSON())))
                                                                                                                                 : null,
                                                                                                                             new JProperty("OICPAction",       OICPAction.      ToString()),
                                                                                                                             new JProperty("requestTimeout",   Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                        ));

            EMPAdapter.EMPClient.OnPushAuthenticationDataResponse += async (Timestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            ProviderAuthenticationData,
                                                                            OICPAction,
                                                                            RequestTimeout,
                                                                            Acknowledgement,
                                                                            Runtime) => await DebugLog.SubmitEvent("PushAuthenticationDataResponse",
                                                                                                                   JSONObject.Create(
                                                                                                                       new JProperty("timestamp",        Timestamp.      ToIso8601()),
                                                                                                                       new JProperty("eventTrackingId",  EventTrackingId.ToString()),

                                                                                                                       ProviderAuthenticationData.AuthorizationIdentifications.SafeAny()
                                                                                                                           ? new JProperty("authorizationIdentifications",  new JArray(ProviderAuthenticationData.AuthorizationIdentifications.SafeSelect(auth => auth.ToJSON())))
                                                                                                                           : null,
                                                                                                                       new JProperty("OICPAction",       OICPAction.     ToString()),
                                                                                                                       new JProperty("requestTimeout",   Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                       new JProperty("result",           Acknowledgement.ToJSON()),
                                                                                                                       new JProperty("runtime",          Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                   ));

            #endregion


            #region OnAuthorizeRemoteReservationStartRequest/-Response

            EMPAdapter.EMPClient.OnAuthorizeRemoteReservationStartRequest += async (LogTimestamp,
                                                                                    RequestTimestamp,
                                                                                    Sender,
                                                                                    SenderId,
                                                                                    EventTrackingId,
                                                                                    ProviderId,
                                                                                    EVSEId,
                                                                                    Identification,
                                                                                    SessionId,
                                                                                    CPOPartnerSessionId,
                                                                                    EMPPartnerSessionId,
                                                                                    PartnerProductId,
                                                                                    RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStartRequest",
                                                                                                                                  JSONObject.Create(
                                                                                                                                      new JProperty("timestamp",                  RequestTimestamp.         ToIso8601()),
                                                                                                                                      new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                                      new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                                      new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                                      new JProperty("identification",             Identification.           ToJSON()),
                                                                                                                                      SessionId.HasValue
                                                                                                                                          ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                                          : null,
                                                                                                                                      CPOPartnerSessionId.HasValue
                                                                                                                                          ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                                          : null,
                                                                                                                                      EMPPartnerSessionId.HasValue
                                                                                                                                          ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                                          : null,
                                                                                                                                      PartnerProductId.HasValue
                                                                                                                                          ? new JProperty("partnerProductId",     PartnerProductId.   Value.ToString())
                                                                                                                                          : null,
                                                                                                                                      new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                                 ));

            EMPAdapter.EMPClient.OnAuthorizeRemoteReservationStartResponse += async (Timestamp,
                                                                                     Sender,
                                                                                     SenderId,
                                                                                     EventTrackingId,
                                                                                     ProviderId,
                                                                                     EVSEId,
                                                                                     Identification,
                                                                                     SessionId,
                                                                                     CPOPartnerSessionId,
                                                                                     EMPPartnerSessionId,
                                                                                     PartnerProductId,
                                                                                     RequestTimeout,
                                                                                     Acknowledgement,
                                                                                     Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStartResponse",
                                                                                                                            JSONObject.Create(
                                                                                                                                new JProperty("timestamp",                  Timestamp.                ToIso8601()),
                                                                                                                                new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                                new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                                new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                                new JProperty("identification",             Identification.           ToJSON()),
                                                                                                                                SessionId.HasValue
                                                                                                                                    ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                                    : null,
                                                                                                                                CPOPartnerSessionId.HasValue
                                                                                                                                    ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                                    : null,
                                                                                                                                EMPPartnerSessionId.HasValue
                                                                                                                                    ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                                    : null,
                                                                                                                                PartnerProductId.HasValue
                                                                                                                                    ? new JProperty("partnerProductId",     PartnerProductId.   Value.ToString())
                                                                                                                                    : null,
                                                                                                                                new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                                new JProperty("result",                     Acknowledgement.ToJSON()),
                                                                                                                                new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                            ));

            #endregion

            #region OnAuthorizeRemoteReservationStopRequest/-Response

            EMPAdapter.EMPClient.OnAuthorizeRemoteReservationStopRequest += async (LogTimestamp,
                                                                                   RequestTimestamp,
                                                                                   Sender,
                                                                                   SenderId,
                                                                                   EventTrackingId,
                                                                                   SessionId,
                                                                                   ProviderId,
                                                                                   EVSEId,
                                                                                   CPOPartnerSessionId,
                                                                                   EMPPartnerSessionId,
                                                                                   RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStopRequest",
                                                                                                                                 JSONObject.Create(
                                                                                                                                     new JProperty("timestamp",                  RequestTimestamp.         ToIso8601()),
                                                                                                                                     new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                                     new JProperty("sessionId",                  SessionId.                ToString()),
                                                                                                                                     new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                                     new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                                     CPOPartnerSessionId.HasValue
                                                                                                                                         ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                                         : null,
                                                                                                                                     EMPPartnerSessionId.HasValue
                                                                                                                                         ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                                         : null,
                                                                                                                                     new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                                ));

            EMPAdapter.EMPClient.OnAuthorizeRemoteReservationStopResponse += async (Timestamp,
                                                                                    Sender,
                                                                                    SenderId,
                                                                                    EventTrackingId,
                                                                                    SessionId,
                                                                                    ProviderId,
                                                                                    EVSEId,
                                                                                    CPOPartnerSessionId,
                                                                                    EMPPartnerSessionId,
                                                                                    RequestTimeout,
                                                                                    Acknowledgement,
                                                                                    Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteReservationStopResponse",
                                                                                                                           JSONObject.Create(
                                                                                                                               new JProperty("timestamp",                  Timestamp.                ToIso8601()),
                                                                                                                               new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                               new JProperty("sessionId",                  SessionId.                ToString()),
                                                                                                                               new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                               new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                               CPOPartnerSessionId.HasValue
                                                                                                                                   ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                                   : null,
                                                                                                                               EMPPartnerSessionId.HasValue
                                                                                                                                   ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                                   : null,
                                                                                                                               new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                               new JProperty("result",                     Acknowledgement.ToJSON()),
                                                                                                                               new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                           ));

            #endregion

            #region OnAuthorizeRemoteStartRequest/-Response

            EMPAdapter.EMPClient.OnAuthorizeRemoteStartRequest += async (LogTimestamp,
                                                                         RequestTimestamp,
                                                                         Sender,
                                                                         SenderId,
                                                                         EventTrackingId,
                                                                         ProviderId,
                                                                         EVSEId,
                                                                         Identification,
                                                                         SessionId,
                                                                         CPOPartnerSessionId,
                                                                         EMPPartnerSessionId,
                                                                         PartnerProductId,
                                                                         RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteStartRequest",
                                                                                                                       JSONObject.Create(
                                                                                                                           new JProperty("timestamp",                  RequestTimestamp.         ToIso8601()),
                                                                                                                           new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                           new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                           new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                           new JProperty("identification",             Identification.           ToJSON()),
                                                                                                                           SessionId.HasValue
                                                                                                                               ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                               : null,
                                                                                                                           CPOPartnerSessionId.HasValue
                                                                                                                               ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                               : null,
                                                                                                                           EMPPartnerSessionId.HasValue
                                                                                                                               ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                               : null,
                                                                                                                           PartnerProductId.HasValue
                                                                                                                               ? new JProperty("partnerProductId",     PartnerProductId.   Value.ToString())
                                                                                                                               : null,
                                                                                                                           new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                      ));

            EMPAdapter.EMPClient.OnAuthorizeRemoteStartResponse += async (Timestamp,
                                                                          Sender,
                                                                          SenderId,
                                                                          EventTrackingId,
                                                                          ProviderId,
                                                                          EVSEId,
                                                                          Identification,
                                                                          SessionId,
                                                                          CPOPartnerSessionId,
                                                                          EMPPartnerSessionId,
                                                                          PartnerProductId,
                                                                          RequestTimeout,
                                                                          Acknowledgement,
                                                                          Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteStartResponse",
                                                                                                                 JSONObject.Create(
                                                                                                                     new JProperty("timestamp",                  Timestamp.                ToIso8601()),
                                                                                                                     new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                     new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                     new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                     new JProperty("identification",             Identification.           ToJSON()),
                                                                                                                     SessionId.HasValue
                                                                                                                         ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                         : null,
                                                                                                                     CPOPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                         : null,
                                                                                                                     EMPPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                         : null,
                                                                                                                     PartnerProductId.HasValue
                                                                                                                         ? new JProperty("partnerProductId",     PartnerProductId.   Value.ToString())
                                                                                                                         : null,
                                                                                                                     new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                     new JProperty("result",                     Acknowledgement.ToJSON()),
                                                                                                                     new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                 ));

            #endregion

            #region OnAuthorizeRemoteStopRequest/-Response

            EMPAdapter.EMPClient.OnAuthorizeRemoteStopRequest += async (LogTimestamp,
                                                                        RequestTimestamp,
                                                                        Sender,
                                                                        SenderId,
                                                                        EventTrackingId,
                                                                        SessionId,
                                                                        ProviderId,
                                                                        EVSEId,
                                                                        CPOPartnerSessionId,
                                                                        EMPPartnerSessionId,
                                                                        RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeRemoteStopRequest",
                                                                                                                      JSONObject.Create(
                                                                                                                          new JProperty("timestamp",                  RequestTimestamp.         ToIso8601()),
                                                                                                                          new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                          new JProperty("sessionId",                  SessionId.                ToString()),
                                                                                                                          new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                          new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                          CPOPartnerSessionId.HasValue
                                                                                                                              ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                              : null,
                                                                                                                          EMPPartnerSessionId.HasValue
                                                                                                                              ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                              : null,
                                                                                                                          new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                     ));

            EMPAdapter.EMPClient.OnAuthorizeRemoteStopResponse += async (Timestamp,
                                                                         Sender,
                                                                         SenderId,
                                                                         EventTrackingId,
                                                                         SessionId,
                                                                         ProviderId,
                                                                         EVSEId,
                                                                         CPOPartnerSessionId,
                                                                         EMPPartnerSessionId,
                                                                         RequestTimeout,
                                                                         Acknowledgement,
                                                                         Runtime) => await DebugLog.SubmitEvent("AuthorizeRemoteStopResponse",
                                                                                                                JSONObject.Create(
                                                                                                                    new JProperty("timestamp",                  Timestamp.                ToIso8601()),
                                                                                                                    new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                    new JProperty("sessionId",                  SessionId.                ToString()),
                                                                                                                    new JProperty("providerId",                 ProviderId.               ToString()),
                                                                                                                    new JProperty("EVSEId",                     EVSEId.                   ToString()),
                                                                                                                    CPOPartnerSessionId.HasValue
                                                                                                                        ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                        : null,
                                                                                                                    EMPPartnerSessionId.HasValue
                                                                                                                        ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                        : null,
                                                                                                                    new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                    new JProperty("result",                     Acknowledgement.ToJSON()),
                                                                                                                    new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                ));

            #endregion


            #region OnGetChargeDetailRecordsRequest/-Response

            EMPAdapter.EMPClient.OnGetChargeDetailRecordsRequest += async  (LogTimestamp,
                                                                            RequestTimestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            ProviderId,
                                                                            From,
                                                                            To,
                                                                            RequestTimeout) => await DebugLog.SubmitEvent("GetChargeDetailRecordsRequest",
                                                                                                                          JSONObject.Create(
                                                                                                                              new JProperty("timestamp",        RequestTimestamp.    ToIso8601()),
                                                                                                                              new JProperty("eventTrackingId",  EventTrackingId.     ToString()),
                                                                                                                              new JProperty("providerId",       ProviderId.          ToString()),
                                                                                                                              new JProperty("from",             From.                ToIso8601()),
                                                                                                                              new JProperty("to",               To.                  ToIso8601()),
                                                                                                                              new JProperty("requestTimeout",   Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                         ));

            EMPAdapter.EMPClient.OnGetChargeDetailRecordsResponse += async (Timestamp,
                                                                            Sender,
                                                                            SenderId,
                                                                            EventTrackingId,
                                                                            ProviderId,
                                                                            From,
                                                                            To,
                                                                            RequestTimeout,
                                                                            ChargeDetailRecords,
                                                                            StatusCode,
                                                                            Runtime) => await DebugLog.SubmitEvent("GetChargeDetailRecordsResponse",
                                                                                                                   JSONObject.Create(
                                                                                                                       new JProperty("timestamp",            Timestamp.           ToIso8601()),
                                                                                                                       new JProperty("eventTrackingId",      EventTrackingId.     ToString()),
                                                                                                                       new JProperty("providerId",           ProviderId.          ToString()),
                                                                                                                       new JProperty("from",                 From.                ToIso8601()),
                                                                                                                       new JProperty("to",                   To.                  ToIso8601()),
                                                                                                                       new JProperty("requestTimeout",       Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                       new JProperty("chargeDetailRecords",  new JArray(ChargeDetailRecords.SafeSelect(cdr => cdr.ToJSON()))),
                                                                                                                       StatusCode.HasValue
                                                                                                                           ? new JProperty("resultStatus",   StatusCode.Value.ToJSON())
                                                                                                                           : null,
                                                                                                                       new JProperty("runtime",              Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                   ));

            #endregion


            // EMPServer

            #region OnAuthorizeStartRequest/-Response

            EMPAdapter.EMPServer.OnAuthorizeStartRequest += async (LogTimestamp,
                                                                   RequestTimestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   OperatorId,
                                                                   Identification,
                                                                   EVSEId,
                                                                   SessionId,
                                                                   PartnerProductId,
                                                                   CPOPartnerSessionId,
                                                                   EMPPartnerSessionId,
                                                                   RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeStartRequest",
                                                                                                                 JSONObject.Create(
                                                                                                                     new JProperty("timestamp",                  RequestTimestamp.         ToIso8601()),
                                                                                                                     new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                     new JProperty("operatorId",                 OperatorId.               ToString()),
                                                                                                                     new JProperty("Identification",             Identification.           ToJSON()),
                                                                                                                     EVSEId.HasValue
                                                                                                                         ? new JProperty("EVSEId",               EVSEId.                   ToString())
                                                                                                                         : null,
                                                                                                                     SessionId.HasValue
                                                                                                                         ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                         : null,
                                                                                                                     PartnerProductId.HasValue
                                                                                                                         ? new JProperty("partnerProductId",     PartnerProductId.   Value.ToString())
                                                                                                                         : null,
                                                                                                                     CPOPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                         : null,
                                                                                                                     EMPPartnerSessionId.HasValue
                                                                                                                         ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                         : null,
                                                                                                                     new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                 ));

            EMPAdapter.EMPServer.OnAuthorizeStartResponse += async (Timestamp,
                                                                    Sender,
                                                                    SenderId,
                                                                    EventTrackingId,
                                                                    OperatorId,
                                                                    Identification,
                                                                    EVSEId,
                                                                    SessionId,
                                                                    PartnerProductId,
                                                                    CPOPartnerSessionId,
                                                                    EMPPartnerSessionId,
                                                                    RequestTimeout,
                                                                    Result,
                                                                    Runtime) => await DebugLog.SubmitEvent("AuthorizeStartResponse",
                                                                                                           JSONObject.Create(
                                                                                                               new JProperty("timestamp",                  Timestamp.                ToIso8601()),
                                                                                                               new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                               new JProperty("operatorId",                 OperatorId.               ToString()),
                                                                                                               new JProperty("Identification",             Identification.           ToJSON()),
                                                                                                               EVSEId.HasValue
                                                                                                                   ? new JProperty("EVSEId",               EVSEId.                   ToString())
                                                                                                                   : null,
                                                                                                               SessionId.HasValue
                                                                                                                   ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                   : null,
                                                                                                               PartnerProductId.HasValue
                                                                                                                   ? new JProperty("partnerProductId",     PartnerProductId.   Value.ToString())
                                                                                                                   : null,
                                                                                                               CPOPartnerSessionId.HasValue
                                                                                                                   ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                   : null,
                                                                                                               EMPPartnerSessionId.HasValue
                                                                                                                   ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                   : null,
                                                                                                               new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                               new JProperty("result",                     Result.                   ToJSON()),
                                                                                                               new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                           ));

            #endregion

            #region OnAuthorizeStopRequest/-Response

            EMPAdapter.EMPServer.OnAuthorizeStopRequest += async (LogTimestamp,
                                                                  RequestTimestamp,
                                                                  Sender,
                                                                  SenderId,
                                                                  EventTrackingId,
                                                                  SessionId,
                                                                  CPOPartnerSessionId,
                                                                  EMPPartnerSessionId,
                                                                  OperatorId,
                                                                  EVSEId,
                                                                  Identification,
                                                                  RequestTimeout) => await DebugLog.SubmitEvent("AuthorizeStopRequest",
                                                                                                                JSONObject.Create(
                                                                                                                    new JProperty("timestamp",                  RequestTimestamp.         ToIso8601()),
                                                                                                                    new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                                    SessionId.HasValue
                                                                                                                        ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                        : null,
                                                                                                                    CPOPartnerSessionId.HasValue
                                                                                                                        ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                        : null,
                                                                                                                    EMPPartnerSessionId.HasValue
                                                                                                                        ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                        : null,
                                                                                                                    new JProperty("operatorId",                 OperatorId.               ToString()),
                                                                                                                    EVSEId.HasValue
                                                                                                                        ? new JProperty("EVSEId",               EVSEId.                   ToString())
                                                                                                                        : null,
                                                                                                                    new JProperty("Identification",             Identification.           ToJSON()),
                                                                                                                    new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                ));

            EMPAdapter.EMPServer.OnAuthorizeStopResponse += async (Timestamp,
                                                                   Sender,
                                                                   SenderId,
                                                                   EventTrackingId,
                                                                   SessionId,
                                                                   CPOPartnerSessionId,
                                                                   EMPPartnerSessionId,
                                                                   OperatorId,
                                                                   EVSEId,
                                                                   Identification,
                                                                   RequestTimeout,
                                                                   Result,
                                                                   Runtime) => await DebugLog.SubmitEvent("AuthorizeStopResponse",
                                                                                                          JSONObject.Create(
                                                                                                              new JProperty("timestamp",                  Timestamp.                ToIso8601()),
                                                                                                              new JProperty("eventTrackingId",            EventTrackingId.          ToString()),

                                                                                                              SessionId.HasValue
                                                                                                                  ? new JProperty("sessionId",            SessionId.          Value.ToString())
                                                                                                                  : null,
                                                                                                              CPOPartnerSessionId.HasValue
                                                                                                                  ? new JProperty("CPOPartnerSessionId",  CPOPartnerSessionId.Value.ToString())
                                                                                                                  : null,
                                                                                                              EMPPartnerSessionId.HasValue
                                                                                                                  ? new JProperty("EMPPartnerSessionId",  EMPPartnerSessionId.Value.ToString())
                                                                                                                  : null,
                                                                                                              new JProperty("operatorId",                 OperatorId.               ToString()),
                                                                                                              EVSEId.HasValue
                                                                                                                  ? new JProperty("EVSEId",               EVSEId.                   ToString())
                                                                                                                  : null,
                                                                                                              new JProperty("Identification",             Identification.           ToJSON()),
                                                                                                              new JProperty("requestTimeout",             Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                              new JProperty("result",                     Result.                   ToJSON()),
                                                                                                              new JProperty("runtime",                    Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                          ));

            #endregion

            #region OnChargeDetailRecordRequest/-Response

            EMPAdapter.EMPServer.OnChargeDetailRecordRequest += async (LogTimestamp,
                                                                       RequestTimestamp,
                                                                       Sender,
                                                                       SenderId,
                                                                       EventTrackingId,
                                                                       ChargeDetailRecord,
                                                                       RequestTimeout) => await DebugLog.SubmitEvent("ChargeDetailRecordRequest",
                                                                                                                     JSONObject.Create(
                                                                                                                         new JProperty("timestamp",           RequestTimestamp.  ToIso8601()),
                                                                                                                         new JProperty("eventTrackingId",     EventTrackingId.   ToString()),

                                                                                                                         new JProperty("chargeDetailRecord",  ChargeDetailRecord.ToJSON()),
                                                                                                                         new JProperty("requestTimeout",      Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                     ));

            EMPAdapter.EMPServer.OnChargeDetailRecordResponse += async (Timestamp,
                                                                        Sender,
                                                                        SenderId,
                                                                        EventTrackingId,
                                                                        ChargeDetailRecord,
                                                                        RequestTimeout,
                                                                        Result,
                                                                        Runtime) => await DebugLog.SubmitEvent("ChargeDetailRecordResponse",
                                                                                                               JSONObject.Create(
                                                                                                                   new JProperty("timestamp",           Timestamp.         ToIso8601()),
                                                                                                                   new JProperty("eventTrackingId",     EventTrackingId.   ToString()),

                                                                                                                   new JProperty("chargeDetailRecord",  ChargeDetailRecord.ToJSON()),
                                                                                                                   new JProperty("requestTimeout",      Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                   new JProperty("result",              Result.            ToJSON()),
                                                                                                                   new JProperty("runtime",             Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                               ));

            #endregion


        }

    }

}
