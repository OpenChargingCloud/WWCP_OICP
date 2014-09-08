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

//using eu.Vanaheimr.Hermod.HTTP;

//#endregion

//namespace com.graphdefined.eMI3.IO.OICP
//{

//    /// <summary>
//    /// The base inetrface for all OICP services.
//    /// </summary>
//    [HTTPService(HostAuthentication: false)]
//    public interface IOICPHTTPService : IHTTPBaseService
//    {

//        #region Properties

//        /// <summary>
//        /// The internal HTTP server.
//        /// </summary>
//        OICPHTTPServer InternalHTTPServer { get; set; }

//        #endregion


//        #region GET /

//        [HTTPMapping(HTTPMethods.GET, "/"), NoAuthentication]
//        HTTPResponse GET_Root();

//        #endregion

//        #region GET /RemoteStartStop

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
//        [HTTPMapping(HTTPMethods.GET,  "/RNs/{RoamingNetwork}/RemoteStartStop"), NoAuthentication]
//        HTTPResponse GET_RemoteStartStop(String RoamingNetwork_Id);

//        #endregion

//        #region POST /RemoteStartStop

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
//        [HTTPMapping(HTTPMethods.POST, "/RNs/{RoamingNetwork}/RemoteStartStop"), NoAuthentication]
//        HTTPResponse POST_RemoteStartStop(String RoamingNetwork_Id);

//        #endregion

//    }

//}
