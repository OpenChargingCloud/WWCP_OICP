/*
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.WebAPI
{

    /// <summary>
    /// OICP HTTP API extention methods.
    /// </summary>
    public static class ExtentionMethods
    {

        #region ParseRoamingNetwork(this HTTPRequest, WWCPAPI, out RoamingNetwork, out HTTPResponse)

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
        public static Boolean ParseRoamingNetwork(this HTTPRequest    HTTPRequest,
                                                  HTTPServer<RoamingNetworks, RoamingNetwork> HTTPServer,
                                                  out RoamingNetwork  RoamingNetwork,
                                                  out HTTPResponse    HTTPResponse)
        {

            if (HTTPServer == null)
                Console.WriteLine("HTTPServer == null!");

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException("HTTPRequest",  "The given HTTP request must not be null!");

            if (HTTPServer == null)
                throw new ArgumentNullException("HTTPServer",   "The given HTTP server must not be null!");

            #endregion

            RoamingNetwork_Id RoamingNetworkId  = null;
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
    /// A HTTP API providing OICP data structures.
    /// </summary>
    public class OICPWebAPI
    {

        #region Data

        /// <summary>
        /// The name or identification string of this HTTP API.
        /// </summary>
        public  const     String                        ServerName  = "Belectric Drive OICP" + Version.Number + " WebAPI";

        private readonly  XMLNamespacesDelegate         XMLNamespaces;
        private readonly  EVSE2EVSEDataRecordDelegate   EVSE2EVSEDataRecord;
        private readonly  EVSEDataRecord2XMLDelegate    EVSEDataRecord2XML;
        private readonly  EVSEStatusRecord2XMLDelegate  EVSEStatusRecord2XML;
        private readonly  XMLPostProcessingDelegate     XMLPostProcessing;

        #endregion

        #region Properties

        #region HTTPServer

        private readonly HTTPServer<RoamingNetworks, RoamingNetwork> _HTTPServer;

        public HTTPServer<RoamingNetworks, RoamingNetwork> HTTPServer
        {
            get
            {
                return _HTTPServer;
            }
        }

        #endregion

        public IEnumerable<KeyValuePair<String, String>> HTTPLogins { get; }

        #region URIPrefix

        private readonly String _URIPrefix;

        public String URIPrefix
        {
            get
            {
                return _URIPrefix;
            }
        }

        #endregion

        #region DNSClient

        private readonly DNSClient _DNSClient;

        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

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
        /// Attach the OICP API to the given HTTP server.
        /// </summary>
        /// <param name="RoamingNetwork">The Bosch E-Bike roaming network.</param>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="HTTPLogins">HTTP logins for Basic Auth.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSE2EVSEDataRecord">An optional delegate to process an EVSE data record before converting it to XML.</param>
        /// <param name="EVSEDataRecord2XML">An optional delegate to process an EVSE data record XML before sending it somewhere.</param>
        /// <param name="EVSEStatusRecord2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public OICPWebAPI(RoamingNetwork                               RoamingNetwork,
                          HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                          IEnumerable<KeyValuePair<String, String>>    HTTPLogins,
                          String                                       URIPrefix             = "/ext/OICPPlus",
                          XMLNamespacesDelegate                        XMLNamespaces         = null,
                          EVSE2EVSEDataRecordDelegate                  EVSE2EVSEDataRecord   = null,
                          EVSEDataRecord2XMLDelegate                   EVSEDataRecord2XML    = null,
                          EVSEStatusRecord2XMLDelegate                 EVSEStatusRecord2XML  = null,
                          XMLPostProcessingDelegate                    XMLPostProcessing     = null)
        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given parameter must not be null!");

            if (HTTPServer == null)
                throw new ArgumentNullException(nameof(HTTPServer),      "The given parameter must not be null!");

            if (URIPrefix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(URIPrefix),       "The given parameter must not be null or empty!");

            if (!URIPrefix.StartsWith("/", StringComparison.Ordinal))
                URIPrefix = "/" + URIPrefix;

            #endregion

            this._HTTPServer           = HTTPServer;
            this.HTTPLogins            = HTTPLogins ?? new KeyValuePair<String, String>[0];
            this._URIPrefix            = URIPrefix;
            this._DNSClient            = HTTPServer.DNSClient;

            this.XMLNamespaces         = XMLNamespaces;
            this.EVSE2EVSEDataRecord   = EVSE2EVSEDataRecord;
            this.EVSEDataRecord2XML    = EVSEDataRecord2XML;
            this.EVSEStatusRecord2XML  = EVSEStatusRecord2XML;
            this.XMLPostProcessing     = XMLPostProcessing;

            RegisterURITemplates();

        }

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.RegisterResourcesFolder(HTTPHostname.Any,
                                               URIPrefix + "/",
                                               "org.GraphDefined.WWCP.OICPv2_1.Server.HTTPServer.HTTPRoot",
                                               DefaultFilename: "index.html");

            #endregion


            #region GET     ~/EVSEs

            // -----------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEs
            // -----------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}" + _URIPrefix + "/EVSEs",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: Request => {

                                             #region Check HTTP Basic Authentication

                                             if (Request.Authorization == null ||
                                                 !HTTPLogins.Any(kvp => kvp.Key   == Request.Authorization.Username &&
                                                                        kvp.Value == Request.Authorization.Password))
                                             {

                                                 return Task.FromResult(
                                                     new HTTPResponseBuilder(Request) {
                                                         HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                         WWWAuthenticate  = @"Basic realm=""Belectric Drive OICPv2.0 Plus""",
                                                         Server           = _HTTPServer.DefaultServerName,
                                                         Date             = DateTime.Now,
                                                         Connection       = "close"
                                                     }.AsImmutable());

                                             }

                                             #endregion

                                             #region Check parameters

                                             HTTPResponse    _HTTPResponse;
                                             RoamingNetwork  _RoamingNetwork;

                                             if (!Request.ParseRoamingNetwork(_HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                                                 return Task.FromResult(_HTTPResponse);

                                             #endregion

                                             var skip   = Request.QueryString.GetUInt32OrDefault("skip");
                                             var take   = Request.QueryString.GetUInt32("take");

                                             //ToDo: Getting the expected total is very expensive!
                                             var _ExpectedCount = _RoamingNetwork.EVSEs.ULongCount();

                                             return Task.FromResult(
                                                 new HTTPResponseBuilder(Request) {
                                                     HTTPStatusCode                = HTTPStatusCode.OK,
                                                     Server                        = _HTTPServer.DefaultServerName,
                                                     Date                          = DateTime.Now,
                                                     AccessControlAllowOrigin      = "*",
                                                     AccessControlAllowMethods     = "GET",
                                                     AccessControlAllowHeaders     = "Content-Type, Authorization",
                                                     ETag                          = "1",
                                                     ContentType                   = HTTPContentType.XML_UTF8,
                                                     Content                       = _RoamingNetwork.EVSEs.
                                                                                         OrderBy(evse => evse.Id).
                                                                                         Skip(skip).
                                                                                         Take(take).
                                                                                         Select (evse => {

                                                                                             try
                                                                                             {
                                                                                                 return OICPMapper.AsOICPEVSEDataRecord(evse, EVSE2EVSEDataRecord);
                                                                                             }
                                                                                             catch (Exception e)
                                                                                             {

                                                                                             }

                                                                                             return null;

                                                                                         }).
                                                                                         Where(evsedatarecord => evsedatarecord != null).
                                                                                         ToXML(_RoamingNetwork, XMLNamespaces, EVSEDataRecord2XML, XMLPostProcessing).
                                                                                         ToUTF8Bytes(),
                                                     X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                 }.AsImmutable());

                                         });

            #endregion

            #region GET     ~/EVSEStatus

            // ----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ext/OICPPlus/EVSEStatus
            // ----------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         "/RNs/{RoamingNetworkId}" + _URIPrefix + "/EVSEStatus",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: Request => {

                                             #region Check HTTP Basic Authentication

                                             if (Request.Authorization == null ||
                                                 !HTTPLogins.Any(kvp => kvp.Key   == Request.Authorization.Username &&
                                                                        kvp.Value == Request.Authorization.Password))
                                             {

                                                 return Task.FromResult(
                                                     new HTTPResponseBuilder(Request) {
                                                         HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                         WWWAuthenticate  = @"Basic realm=""Belectric Drive OICPv2.0 Plus""",
                                                         Server           = _HTTPServer.DefaultServerName,
                                                         Date             = DateTime.Now,
                                                         Connection       = "close"
                                                     }.AsImmutable());

                                             }

                                             #endregion

                                             #region Check parameters

                                             HTTPResponse    _HTTPResponse;
                                             RoamingNetwork  _RoamingNetwork;

                                             if (!Request.ParseRoamingNetwork(_HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                                                 return Task.FromResult(_HTTPResponse);

                                             #endregion

                                             var skip   = Request.QueryString.GetUInt32OrDefault("skip");
                                             var take   = Request.QueryString.GetUInt32("take");

                                             //ToDo: Getting the expected total is very expensive!
                                             var _ExpectedCount = _RoamingNetwork.EVSEStatus(1).ULongCount();

                                             return Task.FromResult(
                                                 new HTTPResponseBuilder(Request) {
                                                     HTTPStatusCode                = HTTPStatusCode.OK,
                                                     Server                        = _HTTPServer.DefaultServerName,
                                                     Date                          = DateTime.Now,
                                                     AccessControlAllowOrigin      = "*",
                                                     AccessControlAllowMethods     = "GET",
                                                     AccessControlAllowHeaders     = "Content-Type, Authorization",
                                                     ETag                          = "1",
                                                     ContentType                   = HTTPContentType.XML_UTF8,
                                                     Content                       = _RoamingNetwork.EVSEs.
                                                                                         OrderBy(evse => evse.Id).
                                                                                         Skip(skip).
                                                                                         Take(take).
                                                                                         ToXML(_RoamingNetwork, XMLNamespaces, EVSEStatusRecord2XML, XMLPostProcessing).
                                                                                         ToUTF8Bytes(),
                                                     X_ExpectedTotalNumberOfItems  = _ExpectedCount
                                                }.AsImmutable());

                                         });

            #endregion

        }

        #endregion


    }

}
