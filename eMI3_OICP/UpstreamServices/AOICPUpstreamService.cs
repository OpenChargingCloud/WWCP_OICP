/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
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

    public abstract class AOICPUpstreamService
    {

        #region Data

        protected readonly DNSClient DNSClient;

        #endregion

        #region Properties

        #region AuthorizatorId

        private readonly AuthorizatorId _AuthorizatorId;

        public AuthorizatorId AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region OICPHost

        private readonly String _OICPHost;

        public String OICPHost
        {
            get
            {
                return _OICPHost;
            }
        }

        #endregion

        #region OICPPort

        private readonly IPPort _OICPPort;

        public IPPort OICPPort
        {
            get
            {
                return _OICPPort;
            }
        }

        #endregion

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

        #region Constructor(s)

        public AOICPUpstreamService(String          OICPHost,
                                    IPPort          OICPPort,
                                    String          HTTPVirtualHost = null,
                                    String          URLPrefix       = "",
                                    AuthorizatorId  AuthorizatorId  = null,
                                    DNSClient       DNSClient       = null)
        {

            this._OICPHost         = OICPHost;

            this._OICPPort         = OICPPort;

            this._HTTPVirtualHost  = (HTTPVirtualHost != null)
                                         ? HTTPVirtualHost
                                         : OICPHost;

            this._URLPrefix        = URLPrefix;

            this._AuthorizatorId   = (AuthorizatorId  == null)
                                         ? AuthorizatorId.Parse("OICP Gateway")
                                         : AuthorizatorId;

            this.DNSClient         = (DNSClient       == null)
                                         ? new DNSClient(SearchForIPv6Servers: false)
                                         : DNSClient;

        }

        #endregion

    }

}
