/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.WWCP.OICPv2_1.EMP;
using org.GraphDefined.WWCP.OICPv2_1.CPO;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.WebAPI
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

            if (HTTPRequest.ParsedURIParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            RoamingNetwork  = HTTPServer.
                                  GetAllTenants(HTTPRequest.Host).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null) {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
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
        public const           String                  DefaultURIPrefix         = "/ext/OICPPlus";

        /// <summary>
        /// The default HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public const           String                  DefaultHTTPRealm         = "Open Charging Cloud OICPPlus WebAPI";

        //ToDo: http://www.iana.org/form/media-types

        /// <summary>
        /// The HTTP content type for serving OICP+ XML data.
        /// </summary>
        public static readonly HTTPContentType         OICPPlusXMLContentType   = new HTTPContentType("application/vnd.OICPPlus+xml", CharSetType.UTF8);

        /// <summary>
        /// The HTTP content type for serving OICP+ HTML data.
        /// </summary>
        public static readonly HTTPContentType         OICPPlusHTMLContentType  = new HTTPContentType("application/vnd.OICPPlus+html", CharSetType.UTF8);


        private readonly XMLNamespacesDelegate                      XMLNamespaces;
        private readonly EVSE2EVSEDataRecordDelegate                EVSE2EVSEDataRecord;
        private readonly EVSEStatusUpdate2EVSEStatusRecordDelegate  EVSEStatusUpdate2EVSEStatusRecord;
        private readonly EVSEDataRecord2XMLDelegate                 EVSEDataRecord2XML;
        private readonly EVSEStatusRecord2XMLDelegate               EVSEStatusRecord2XML;
        private readonly XMLPostProcessingDelegate                  XMLPostProcessing;

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP server for serving the OICP+ WebAPI.
        /// </summary>
        public HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer    { get; }

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public String                                       URIPrefix     { get; }

        /// <summary>
        /// The HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public String                                       HTTPRealm     { get; }

        /// <summary>
        /// An enumeration of logins for an optional HTTP Basic Authentication.
        /// </summary>
        public IEnumerable<KeyValuePair<String, String>>    HTTPLogins    { get; }


        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient                                    DNSClient     { get; }

        #endregion

        #region Events

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                HTTPServer.RequestLog += value;
            }

            remove
            {
                HTTPServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                HTTPServer.AccessLog += value;
            }

            remove
            {
                HTTPServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                HTTPServer.ErrorLog += value;
            }

            remove
            {
                HTTPServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Attach the OICP+ WebAPI to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="HTTPRealm">The HTTP realm, if HTTP Basic Authentication is used.</param>
        /// <param name="HTTPLogins">An enumeration of logins for an optional HTTP Basic Authentication.</param>
        /// 
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSE2EVSEDataRecord">An optional delegate to process an EVSE data record before converting it to XML.</param>
        /// <param name="EVSEDataRecord2XML">An optional delegate to process an EVSE data record XML before sending it somewhere.</param>
        /// <param name="EVSEStatusRecord2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public OICPWebAPI(HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                          String                                       URIPrefix                           = DefaultURIPrefix,
                          String                                       HTTPRealm                           = DefaultHTTPRealm,
                          IEnumerable<KeyValuePair<String, String>>    HTTPLogins                          = null,

                          XMLNamespacesDelegate                        XMLNamespaces                       = null,
                          EVSE2EVSEDataRecordDelegate                  EVSE2EVSEDataRecord                 = null,
                          EVSEStatusUpdate2EVSEStatusRecordDelegate    EVSEStatusUpdate2EVSEStatusRecord   = null,
                          EVSEDataRecord2XMLDelegate                   EVSEDataRecord2XML                  = null,
                          EVSEStatusRecord2XMLDelegate                 EVSEStatusRecord2XML                = null,
                          XMLPostProcessingDelegate                    XMLPostProcessing                   = null)
        {

            #region Initial checks

            if (HTTPServer == null)
                throw new ArgumentNullException(nameof(HTTPServer), "The given HTTP server must not be null!");

            if (URIPrefix.IsNullOrEmpty())
                URIPrefix = DefaultURIPrefix;

            if (!URIPrefix.StartsWith("/", StringComparison.Ordinal))
                URIPrefix = "/" + URIPrefix;

            #endregion

            this.HTTPServer                         = HTTPServer;
            this.URIPrefix                          = URIPrefix;
            this.HTTPRealm                          = HTTPRealm.IsNotNullOrEmpty() ? HTTPRealm : DefaultHTTPRealm;
            this.HTTPLogins                         = HTTPLogins ?? new KeyValuePair<String, String>[0];
            this.DNSClient                          = HTTPServer.DNSClient;

            this.XMLNamespaces                      = XMLNamespaces;
            this.EVSE2EVSEDataRecord                = EVSE2EVSEDataRecord;
            this.EVSEStatusUpdate2EVSEStatusRecord  = EVSEStatusUpdate2EVSEStatusRecord;
            this.EVSEDataRecord2XML                 = EVSEDataRecord2XML;
            this.EVSEStatusRecord2XML               = EVSEStatusRecord2XML;
            this.XMLPostProcessing                  = XMLPostProcessing;

            RegisterURITemplates();

        }

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.RegisterResourcesFolder(HTTPHostname.Any,
                                               URIPrefix + "/",
                                               "org.GraphDefined.WWCP.OICPv2_1.WebAPI.HTTPRoot",
                                               DefaultFilename: "index.html");

            #endregion


            #region GET     ~/EVSEs

            #region EVSEsDelegate

            HTTPDelegate EVSEsDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == Request.Authorization.Username &&
                                           kvp.Value == Request.Authorization.Password))
                {

                    return Task.FromResult(
                        new HTTPResponseBuilder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.Now,
                            Connection       = "close"
                        }.AsImmutable());

                }

                #endregion

                #region Check parameters

                HTTPResponse   _HTTPResponse;
                RoamingNetwork _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                var skip = Request.QueryString.GetUInt32OrDefault("skip");
                var take = Request.QueryString.GetUInt32         ("take");

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEs.ULongCount();

                return Task.FromResult(
                    new HTTPResponseBuilder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.Now,
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
                                                                    ToXML(_RoamingNetwork, XMLNamespaces, EVSEDataRecord2XML, XMLPostProcessing).
                                                                    ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                    }.AsImmutable());

            };

            #endregion

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEs
            // -----------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}" + URIPrefix + "/EVSEs",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: EVSEsDelegate);

            // ----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OICPPlus+xml" http://127.0.0.1:3004/RNs/Prod/EVSEs
            // ----------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}/EVSEs",
                                         OICPPlusXMLContentType,
                                         HTTPDelegate: EVSEsDelegate);

            #endregion

            #region GET     ~/EVSEStatus

            #region XMLDelegate

            HTTPDelegate EVSEStatusXMLDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == Request.Authorization.Username &&
                                           kvp.Value == Request.Authorization.Password))
                {

                    return Task.FromResult(
                        new HTTPResponseBuilder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.Now,
                            Connection       = "close"
                        }.AsImmutable());

                }

                #endregion

                #region Check parameters

                HTTPResponse   _HTTPResponse;
                RoamingNetwork _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                #region Check Query String parameters

                var skip          = Request.QueryString.GetUInt32OrDefault("skip");
                var take          = Request.QueryString.GetUInt32         ("take");

                var statusFilter  = Request.QueryString.CreateEnumFilter<EVSE, EVSEStatusTypes>("status",
                                                                                                (evse, status) => evse.Status == status.AsWWCPEVSEStatus());

                var matchFilter   = Request.QueryString.CreateStringFilter<EVSE>  ("match",
                                                                                   (evse, match) => evse.Id.ToString().Contains(match) ||
                                                                                                    evse.ChargingStation.Name.FirstText.Contains(match));

                var sinceFilter   = Request.QueryString.CreateDateTimeFilter<EVSE>("since",
                                                                                   (evse, timestamp) => evse.Status.Timestamp >= timestamp);

                #endregion

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEStatus(1).ULongCount();

                return Task.FromResult(
                    new HTTPResponseBuilder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.Now,
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
                                                                    EVSEStatusRecord2XML,
                                                                    XMLPostProcessing).
                                                            ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                    }.AsImmutable());

            };

            #endregion

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/xml" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEStatus
            // ---------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}" + URIPrefix + "/EVSEStatus",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: EVSEStatusXMLDelegate);

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OICPPlus+xml" http://127.0.0.1:3004/RNs/Prod/EVSEStatus
            // ---------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}/EVSEStatus",
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
                    !HTTPLogins.Any(kvp => kvp.Key   == Request.Authorization.Username &&
                                           kvp.Value == Request.Authorization.Password))
                {

                    return Task.FromResult(
                        new HTTPResponseBuilder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.Now,
                            Connection       = "close"
                        }.AsImmutable());

                }

                #endregion

                #region Check parameters

                HTTPResponse    _HTTPResponse;
                RoamingNetwork  _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                #region Check Query String parameters

                var skip          = Request.QueryString.GetUInt32OrDefault("skip");
                var take          = Request.QueryString.GetUInt32         ("take");

                var statusFilter  = Request.QueryString.CreateEnumFilter<EVSE, EVSEStatusTypes>("status",
                                                                                                (evse, status) => evse.Status == status.AsWWCPEVSEStatus());

                var matchFilter   = Request.QueryString.CreateStringFilter<EVSE>  ("match",
                                                                                   (evse, match) => evse.Id.ToString().Contains(match) ||
                                                                                                    evse.ChargingStation.Name.FirstText.Contains(match));

                var sinceFilter   = Request.QueryString.CreateDateTimeFilter<EVSE>("since",
                                                                                   (evse, timestamp) => evse.Status.Timestamp >= timestamp);

                #endregion

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEStatus(1).ULongCount();

                return Task.FromResult(
                    new HTTPResponseBuilder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.Now,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = String.Concat(@"<html>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <head>", Environment.NewLine,
                                                                      @"    <link href=""" + URIPrefix + @"/styles.css"" type=""text/css"" rel=""stylesheet"" />", Environment.NewLine,
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
                                                                                                        @"        <div class=""id"">",            evse.Id.ToString(),                  @"</div>", Environment.NewLine,
                                                                                                        @"        <div class=""description"">",   evse.ChargingStation.Name.FirstText, @"</div>", Environment.NewLine,
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
                   }.AsImmutable());

            };

            #endregion

            // ---------------------------------------------------------------------------------------
            // curl -v -H "Accept: text/html" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEStatus
            // ---------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}" + URIPrefix + "/EVSEStatus",
                                         HTTPContentType.HTML_UTF8,
                                         HTTPDelegate: EVSEStatusHTMLDelegate);

            // ----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OICPPlus+html" http://127.0.0.1:3004/RNs/Prod/EVSEStatus
            // ----------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}/EVSEStatus",
                                         OICPPlusHTMLContentType,
                                         HTTPDelegate: EVSEStatusHTMLDelegate);

            #endregion

        }

        #endregion


    }

}
