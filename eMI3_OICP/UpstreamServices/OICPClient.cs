/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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
using System.Xml.Linq;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod;
using eu.Vanaheimr.Hermod.HTTP;
using eu.Vanaheimr.Hermod.Services.DNS;

using org.emi3group.LocalService;

#endregion

namespace org.emi3group.IO.OICP
{

    public class OICPClient : HTTPClient
    {

        #region Properties

        #region HTTPVirtualHost

        private readonly String _HTTPVirtualHost;

        public String HTTPVirtualHost
        {
            get
            {
                return _HTTPVirtualHost;
            }
        }

        #endregion

        #region URLPrefix

        private readonly String _URLPrefix;

        public String URLPrefix
        {
            get
            {
                return _URLPrefix;
            }
        }

        #endregion

        #endregion

        #region Constructor

        public OICPClient(IPv4Address  OICPHost,
                          IPPort       OICPPort,
                          String       HTTPVirtualHost,
                          String       URLPrefix)

            : base(OICPHost, OICPPort)

        {

            this._HTTPVirtualHost  = HTTPVirtualHost;
            this._URLPrefix        = URLPrefix;

        }

        #endregion

        #region Query

        public HTTPResponse Query(String Query, String SOAPAction)
        {

            var builder = this.POST(_URLPrefix);
            builder.Host         = HTTPVirtualHost;
            builder.Content      = Query.ToUTF8Bytes();
            builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
            builder.Set("SOAPAction", SOAPAction);
            builder.UserAgent    = "Belectric Drive Hubject Gateway";

            return this.Execute_Synced(builder);

        }

        #endregion

    }

}
