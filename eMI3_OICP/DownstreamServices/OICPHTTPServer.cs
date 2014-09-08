/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;

using eu.Vanaheimr.Hermod;
using eu.Vanaheimr.Hermod.HTTP;
using com.graphdefined.eMI3.LocalService;

#endregion

namespace com.graphdefined.eMI3.IO.OICP
{

    /// <summary>
    /// OICP Downstream HTTP/SOAP server.
    /// </summary>
    public class OICPHTTPServer : HTTPServer
    {

        #region Properties

        #region HTTPRoot

        public String       HTTPRoot            { get; set; }

        #endregion

        #region RequestRouter

        private readonly RequestRouter _RequestRouter;

        public RequestRouter RequestRouter
        {
            get
            {
                return _RequestRouter;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize the OICP HTTP server using IPAddress.Any.
        /// </summary>
        /// <param name="RequestRouter">The request router.</param>
        /// <param name="IPPort">The IP listing port.</param>
        public OICPHTTPServer(RequestRouter  RequestRouter,
                              IPPort         IPPort)

        //    : base(IPv4Address.Any, IPPort, Autostart: false)

        {

            this._RequestRouter = RequestRouter;

            this.AttachTCPPort(IPPort);
            this.Start();

            //OnNewHTTPService += IGraphDevroomService => {
            //                        IGraphDevroomService.InternalHTTPServer = this;
            //                        IGraphDevroomService.AllResources       = AllResources;
            //                        IGraphDevroomService.HTTPRoot           = HTTPRoot;
            //                    };

        }

        #endregion

    }

}
