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

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// An OICP v2.0 EMP server logger.
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
                   Default_LogHTTPResponse_toConsole,
                   Default_LogHTTPResponse_toDisc)

        { }

        #endregion

        #region EMPServerLogger(EMPServer, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP v2.0 EMP server logger using the given logging delegates.
        /// </summary>
        /// <param name="EMPServer">A OICP v2.0 EMP server.</param>
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        public EMPServerLogger(EMPServer                                  EMPServer,
                               Action<String, HTTPRequest>                LogHTTPRequest_toConsole,
                               Action<String, HTTPRequest>                LogHTTPRequest_toDisc,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toConsole,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toDisc)

            : base(EMPServer.HTTPServer,
                   LogHTTPRequest_toConsole,
                   LogHTTPRequest_toDisc,
                   LogHTTPResponse_toConsole,
                   LogHTTPResponse_toDisc)

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
