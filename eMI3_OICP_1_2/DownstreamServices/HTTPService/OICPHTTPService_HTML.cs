///*
// * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
// * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;

//using eu.Vanaheimr.Illias.Commons;
//using eu.Vanaheimr.Hermod.HTTP;

//#endregion

//namespace com.graphdefined.eMI3.IO.OICP_1_2
//{

//    /// <summary>
//    /// HTML content representation.
//    /// </summary>
//    public class OICPHTTPService_HTML : AOICPHTTPService
//    {

//        #region Constructor(s)

//        #region OICPHTTPService_HTML()

//        /// <summary>
//        /// HTML content representation.
//        /// </summary>
//        public OICPHTTPService_HTML()
//            : base(HTTPContentType.HTML_UTF8)
//        { }

//        #endregion

//        #region OICPHTTPService_HTML(IHTTPConnection)

//        /// <summary>
//        /// HTML content representation.
//        /// </summary>
//        /// <param name="IHTTPConnection">The http connection for this request.</param>
//        public OICPHTTPService_HTML(IHTTPConnection IHTTPConnection)
//            : base(IHTTPConnection, HTTPContentType.HTML_UTF8)
//        { }

//        #endregion

//        #endregion

//        #region GET /

//        /// <summary>
//        /// Get /
//        /// </summary>
//        /// <example>curl -H "Accept: text/html" http://127.0.0.1:3001/</example>
//        public override HTTPResponse GET_Root()
//        {

//            var path = IHTTPConnection.RequestHeader.UrlPath.Remove(0, 1);

//            return (path != "")
//                       ? GetResources(HTTPRoot, path)
//                       : GetResources(HTTPRoot, "index.html");

//        }

//        #endregion

//        #region GET /RemoteStartStop

//        /// <summary>
//        /// Get /RemoteStartStop
//        /// </summary>
//        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
//        /// <example>curl -H "Accept: text/html" http://127.0.0.1:3001/RemoteStartStop</example>
//        public override HTTPResponse GET_RemoteStartStop(String RoamingNetwork_Id)
//        {
//            return new HTTPResponseBuilder() {
//                HTTPStatusCode  = HTTPStatusCode.OK,
//                ContentType     = HTTPContentType.HTML_UTF8,
//                Content         = ("/RNs/" + RoamingNetwork_Id + "/RemoteStartStop is a HTTP/SOAP endpoint!").ToUTF8Bytes()
//            };
//        }

//        #endregion

//    }

//}
