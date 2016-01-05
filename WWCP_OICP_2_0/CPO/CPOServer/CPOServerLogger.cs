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
    /// An OICP v2.0 CPO server logger.
    /// </summary>
    public class CPOServerLogger : HTTPLogger
    {

        #region Data

        private readonly CPOServer _CPOServer;

        #endregion

        #region Constructor(s)

        #region CPOServerLogger(CPOServer)

        /// <summary>
        /// Create a new OICP v2.0 CPO server logger using the default logging delegates.
        /// </summary>
        /// <param name="CPOServer">A OICP v2.0 CPO server.</param>
        public CPOServerLogger(CPOServer CPOServer)

            : this(CPOServer,
                   Default_LogHTTPRequest_toConsole,
                   Default_LogHTTPRequest_toDisc,
                   Default_LogHTTPResponse_toConsole,
                   Default_LogHTTPResponse_toDisc)

        { }

        #endregion

        #region CPOServerLogger(CPOServer, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP v2.0 CPO server logger using the given logging delegates.
        /// </summary>
        /// <param name="CPOServer">A OICP v2.0 CPO server.</param>
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        public CPOServerLogger(CPOServer                                  CPOServer,
                               Action<String, HTTPRequest>                LogHTTPRequest_toConsole,
                               Action<String, HTTPRequest>                LogHTTPRequest_toDisc,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toConsole,
                               Action<String, HTTPRequest, HTTPResponse>  LogHTTPResponse_toDisc)

            : base(CPOServer.HTTPServer,
                   LogHTTPRequest_toConsole,
                   LogHTTPRequest_toDisc,
                   LogHTTPResponse_toConsole,
                   LogHTTPResponse_toDisc)

        {

            #region Initial checks

            if (CPOServer == null)
                throw new ArgumentNullException("CPOServer", "The given OICP v2.0 CPO server must not be null!");

            #endregion

            this._CPOServer = CPOServer;

            #region Register remote start/stop log events

            RegisterEvent("RemoteStart",
                          handler => _CPOServer.OnLogRemoteStart += handler,
                          handler => _CPOServer.OnLogRemoteStart -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            RegisterEvent("RemoteStarted",
                          handler => _CPOServer.OnLogRemoteStarted += handler,
                          handler => _CPOServer.OnLogRemoteStarted -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            RegisterEvent("RemoteStop",
                          handler => _CPOServer.OnLogRemoteStop += handler,
                          handler => _CPOServer.OnLogRemoteStop -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            RegisterEvent("RemoteStopped",
                          handler => _CPOServer.OnLogRemoteStopped += handler,
                          handler => _CPOServer.OnLogRemoteStopped -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget().
                RegisterDefaultDiscLogTarget();

            #endregion

        }

        #endregion

        #endregion

    }

}
