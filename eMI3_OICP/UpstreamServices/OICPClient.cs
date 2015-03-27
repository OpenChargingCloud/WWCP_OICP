/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.eMI3.IO.OICP
{

    /// <summary>
    /// A specialized HTTP client for the Open InterCharge Protocol.
    /// </summary>
    public class OICPClient : HTTPClient
    {

        #region Properties

        #region HTTPVirtualHost

        private readonly String _HTTPVirtualHost;

        /// <summary>
        /// The HTTP virtual host to use.
        /// </summary>
        public String HTTPVirtualHost
        {
            get
            {
                return _HTTPVirtualHost;
            }
        }

        #endregion

        #region URIPrefix

        private readonly String _URIPrefix;

        /// <summary>
        /// The URI-prefix of the OICP service.
        /// </summary>
        public String URIPrefix
        {
            get
            {
                return _URIPrefix;
            }
        }

        #endregion

        #region UserAgent

        private readonly String _UserAgent;

        /// <summary>
        /// The HTTP user agent.
        /// </summary>
        public String UserAgent
        {
            get
            {
                return _UserAgent;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new specialized HTTP client for the Open InterCharge Protocol.
        /// </summary>
        /// <param name="OICPHost">The hostname of the remote OICP service.</param>
        /// <param name="OICPPort">The TCP port of the remote OICP service.</param>
        /// <param name="HTTPVirtualHost">The HTTP virtual host to use.</param>
        /// <param name="URIPrefix">The URI-prefix of the OICP service.</param>
        /// <param name="UserAgent">The HTTP user agent to use.</param>
        public OICPClient(String     OICPHost,
                          IPPort     OICPPort,
                          String     HTTPVirtualHost,
                          String     URIPrefix,
                          String     UserAgent  = "GraphDefined Hubject Gateway",
                          DNSClient  DNSClient  = null)

            : base(OICPHost, OICPPort, DNSClient)

        {

            this._HTTPVirtualHost  = HTTPVirtualHost;
            this._URIPrefix        = URIPrefix;
            this._UserAgent        = UserAgent;

        }

        #endregion


        #region Query

        public HTTPResponse Query(String Query, String SOAPAction)
        {

            var builder = this.POST(_URIPrefix);
            builder.Host         = HTTPVirtualHost;
            builder.Content      = Query.ToUTF8Bytes();
            builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
            builder.Set("SOAPAction", SOAPAction);
            builder.UserAgent    = UserAgent;

            return this.Execute_Synced(builder);

        }

        #endregion

    }

}
