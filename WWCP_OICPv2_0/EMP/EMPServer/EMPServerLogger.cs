/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 EMP server logger.
    /// </summary>
    public class EMPServerLogger : HTTPLogger
    {

        #region Data

        private readonly EMPServer _EMPServer;

        #endregion

        #region Constructor(s)

        #region EMPServerLogger(EMPServer)

        /// <summary>
        /// Create a new OICP v2.0 EMP server logger using the default logging delegates.
        /// </summary>
        /// <param name="EMPServer">A OICP v2.0 EMP server.</param>
        public EMPServerLogger(EMPServer EMPServer)

            : this(EMPServer,

                   Default_LogHTTPRequest_toConsole,
                   Default_LogHTTPRequest_toDisc,
                   null,
                   null,

                   Default_LogHTTPResponse_toConsole,
                   Default_LogHTTPResponse_toDisc,
                   null,
                   null)

        { }

        #endregion

        #region EMPServerLogger(EMPServer, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP v2.0 EMP server logger using the given logging delegates.
        /// </summary>
        /// <param name="EMPServer">A OICP v2.0 EMP server.</param>
        /// 
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        public EMPServerLogger(EMPServer                                  EMPServer,

                               Action<String, HTTPRequest>                LogHTTPRequest_toConsole,
                               Action<String, HTTPRequest>                LogHTTPRequest_toDisc,
                               Action<String, HTTPRequest>                LogHTTPRequest_toNetwork,
                               Action<String, HTTPRequest>                LogHTTPRequest_toHTTPSSE,

                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toConsole,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toDisc,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toNetwork,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toHTTPSSE)

            : base(EMPServer.HTTPServer,

                   LogHTTPRequest_toConsole,
                   LogHTTPRequest_toDisc,
                   LogHTTPRequest_toNetwork,
                   LogHTTPRequest_toHTTPSSE,

                   LogHTTPResponse_toConsole,
                   LogHTTPResponse_toDisc,
                   LogHTTPResponse_toNetwork,
                   LogHTTPResponse_toHTTPSSE)

        {

            #region Initial checks

            if (EMPServer == null)
                throw new ArgumentNullException("EMPServer", "The given OICP v2.0 EMP server must not be null!");

            #endregion

            this._EMPServer = EMPServer;

            #region Register authorize start/stop log events

            RegisterEvent("AuthorizeStart",
                          handler => _EMPServer.OnLogAuthorizeStart += handler,
                          handler => _EMPServer.OnLogAuthorizeStart -= handler,
                          "Authorize", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            RegisterEvent("AuthorizeStarted",
                          handler => _EMPServer.OnLogAuthorizeStarted += handler,
                          handler => _EMPServer.OnLogAuthorizeStarted -= handler,
                          "Authorize", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            RegisterEvent("AuthorizeStop",
                          handler => _EMPServer.OnLogAuthorizeStop += handler,
                          handler => _EMPServer.OnLogAuthorizeStop -= handler,
                          "Authorize", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            RegisterEvent("AuthorizeStopped",
                          handler => _EMPServer.OnLogAuthorizeStopped += handler,
                          handler => _EMPServer.OnLogAuthorizeStopped -= handler,
                          "Authorize", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            #endregion

        }

        #endregion

        #endregion

    }

}
