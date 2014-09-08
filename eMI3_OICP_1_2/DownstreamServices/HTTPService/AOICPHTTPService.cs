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
//using System.Threading;
//using System.Collections.Generic;

//using eu.Vanaheimr.Hermod.HTTP;

//#endregion

//namespace com.graphdefined.eMI3.IO.OICP_1_2
//{

//    /// <summary>
//    /// This class provides a abstract OICP HTTP service
//    /// without being bound to any specific content representation.
//    /// </summary>
//    public abstract class AOICPHTTPService : AHTTPService,
//                                             IOICPHTTPService
//    {

//        #region Data

//        private ThreadLocal<HTTPResponse>  HTTPErrorResponse;
//        private const String               EdarHTTPRoot = "com.graphdefined.eMI3.APIService.HTTPRoot.";

//        #endregion

//        #region Properties

//        /// <summary>
//        /// The internal OICP HTTP server.
//        /// </summary>
//        public OICPHTTPServer InternalHTTPServer { get; set; }

//        #endregion

//        #region Constructor(s)

//        #region AOICPHTTPService()

//        /// <summary>
//        /// Creates a new abstract OICP HTTP service.
//        /// </summary>
//        public AOICPHTTPService()
//        { }

//        #endregion

//        #region AOICPHTTPService(HTTPContentType)

//        /// <summary>
//        /// Creates a new abstract OICP HTTP service.
//        /// </summary>
//        /// <param name="HTTPContentType">A content type.</param>
//        public AOICPHTTPService(HTTPContentType HTTPContentType)
//            : base(HTTPContentType)
//        { }

//        #endregion

//        #region AOICPHTTPService(HTTPContentTypes)

//        /// <summary>
//        /// Creates a new abstract OICP HTTP service.
//        /// </summary>
//        /// <param name="HTTPContentTypes">A content type.</param>
//        public AOICPHTTPService(IEnumerable<HTTPContentType> HTTPContentTypes)
//            : base(HTTPContentTypes)
//        { }

//        #endregion

//        #region AOICPHTTPService(IHTTPConnection, HTTPContentType)

//        /// <summary>
//        /// Creates a new abstract OICP HTTP service.
//        /// </summary>
//        /// <param name="IHTTPConnection">The http connection for this request.</param>
//        /// <param name="HTTPContentType">A http content type.</param>
//        public AOICPHTTPService(IHTTPConnection IHTTPConnection, HTTPContentType HTTPContentType)
//            : base(IHTTPConnection, HTTPContentType)
//        {
//            this.Callback = new ThreadLocal<String>();
//            this.Skip     = new ThreadLocal<UInt64>();
//            this.Take     = new ThreadLocal<UInt64>();
//        }

//        #endregion

//        #region AOICPHTTPService(IHTTPConnection, HTTPContentTypes)

//        /// <summary>
//        /// Creates a new abstract OICP HTTP service.
//        /// </summary>
//        /// <param name="IHTTPConnection">The http connection for this request.</param>
//        /// <param name="HTTPContentTypes">An enumeration of content types.</param>
//        public AOICPHTTPService(IHTTPConnection IHTTPConnection, IEnumerable<HTTPContentType> HTTPContentTypes)
//            : base(IHTTPConnection, HTTPContentTypes)
//        {
//            this.Callback = new ThreadLocal<String>();
//            this.Skip     = new ThreadLocal<UInt64>();
//            this.Take     = new ThreadLocal<UInt64>();
//        }

//        #endregion

//        #endregion


//        #region GET /

//        /// <summary>
//        /// Get the landing page.
//        /// </summary>
//        public virtual HTTPResponse GET_Root()
//        {

//            var path = IHTTPConnection.RequestHeader.UrlPath.Remove(0, 1);

//            return (path != "")
//                       ? GetResources(path)
//                       : GetResources("index.html");

//            //return HTTPTools.MovedTemporarily("/combinedlog");

//        }

//        #endregion

//        #region GET /RemoteStartStop

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
//        public virtual HTTPResponse GET_RemoteStartStop(String RoamingNetwork_Id)
//        {
//            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
//        }

//        #endregion

//        #region POST /RemoteStartStop

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
//        public virtual HTTPResponse POST_RemoteStartStop(String RoamingNetwork_Id)
//        {
//            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
//        }

//        #endregion

//    }

//}
