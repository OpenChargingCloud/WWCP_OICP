/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 HTTP <http://www.github.com/eMI3/HTTP>
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
using org.emi3group.HTTP;

#endregion

namespace org.emi3group.IO.OICP
{

    /// <summary>
    /// OICP Downstream HTTP/SOAP server.
    /// </summary>
    public class OICPHTTPServer : HTTPServer<IOICPHTTPService>
    {

        #region Properties

        public String           HTTPRoot            { get; set; }
        public eMI3_HTTPServer  eMI3_HTTPServer     { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize the OICP HTTP server using IPAddress.Any.
        /// </summary>
        /// <param name="IPPort">The IP listing port.</param>
        public OICPHTTPServer(IPPort IPPort)
            : base(IPv4Address.Any, IPPort, Autostart: false)
        {

            OnNewHTTPService += IGraphDevroomService => {
                                    IGraphDevroomService.InternalHTTPServer = this;
                                    IGraphDevroomService.AllResources       = AllResources;
                                    IGraphDevroomService.HTTPRoot           = HTTPRoot;
                                };

        }

        #endregion

    }

}
