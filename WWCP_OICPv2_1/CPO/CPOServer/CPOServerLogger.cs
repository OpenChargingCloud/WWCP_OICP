﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP v2.0 CPO Server logger.
    /// </summary>
    public class CPOServerLogger : HTTPLogger
    {

        #region Data

        /// <summary>
        /// The default context for this logger.
        /// </summary>
        public const String DefaultContext = "OICP_CPOServer";

        #endregion

        #region Properties

        #region CPOServer

        private readonly CPOServer _CPOServer;

        /// <summary>
        /// The linked OICP v2.0 CPO server.
        /// </summary>
        public CPOServer CPOServer
        {
            get
            {
                return _CPOServer;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOServerLogger(CPOServer, Context = DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OICP v2.0 CPO server logger using the default logging delegates.
        /// </summary>
        /// <param name="CPOServer">A OICP v2.0 CPO server.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOServerLogger(CPOServer                    CPOServer,
                               String                       Context         = DefaultContext,
                               Func<String, String, String> LogFileCreator  = null)

            : this(CPOServer,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                   null,
                   null,
                   null,
                   null,

                   LogFileCreator: LogFileCreator)

        { }

        #endregion

        #region CPOServerLogger(CPOServer, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP v2.0 CPO server logger using the given logging delegates.
        /// </summary>
        /// <param name="CPOServer">A OICP v2.0 CPO server.</param>
        /// <param name="Context">A context of this API.</param>
        /// 
        /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
        /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
        /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
        /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
        /// 
        /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
        /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOServerLogger(CPOServer                     CPOServer,
                               String                        Context,

                               HTTPRequestLoggerDelegate     LogHTTPRequest_toConsole,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toConsole,
                               HTTPRequestLoggerDelegate     LogHTTPRequest_toDisc,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toDisc,

                               HTTPRequestLoggerDelegate     LogHTTPRequest_toNetwork   = null,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toNetwork  = null,
                               HTTPRequestLoggerDelegate     LogHTTPRequest_toHTTPSSE   = null,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toHTTPSSE  = null,

                               HTTPResponseLoggerDelegate    LogHTTPError_toConsole     = null,
                               HTTPResponseLoggerDelegate    LogHTTPError_toDisc        = null,
                               HTTPResponseLoggerDelegate    LogHTTPError_toNetwork     = null,
                               HTTPResponseLoggerDelegate    LogHTTPError_toHTTPSSE     = null,

                               Func<String, String, String>  LogFileCreator             = null)

            : base(CPOServer.SOAPServer,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,

                   LogHTTPRequest_toConsole,
                   LogHTTPResponse_toConsole,
                   LogHTTPRequest_toDisc,
                   LogHTTPResponse_toDisc,

                   LogHTTPRequest_toNetwork,
                   LogHTTPResponse_toNetwork,
                   LogHTTPRequest_toHTTPSSE,
                   LogHTTPResponse_toHTTPSSE,

                   LogHTTPError_toConsole,
                   LogHTTPError_toDisc,
                   LogHTTPError_toNetwork,
                   LogHTTPError_toHTTPSSE,

                   LogFileCreator)

        {

            #region Initial checks

            if (CPOServer == null)
                throw new ArgumentNullException(nameof(CPOServer), "The given CPO server must not be null!");

            this._CPOServer = CPOServer;

            #endregion

            #region Register remote start/stop log events

            RegisterEvent("RemoteReservationStart",
                          handler => _CPOServer.OnLogRemoteReservationStart   += handler,
                          handler => _CPOServer.OnLogRemoteReservationStart   -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteReservationStarted",
                          handler => _CPOServer.OnLogRemoteReservationStarted += handler,
                          handler => _CPOServer.OnLogRemoteReservationStarted -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteReservationStop",
                          handler => _CPOServer.OnLogRemoteReservationStop    += handler,
                          handler => _CPOServer.OnLogRemoteReservationStop    -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteReservationStopped",
                          handler => _CPOServer.OnLogRemoteReservationStopped += handler,
                          handler => _CPOServer.OnLogRemoteReservationStopped -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("RemoteStart",
                          handler => _CPOServer.OnLogRemoteStart   += handler,
                          handler => _CPOServer.OnLogRemoteStart   -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteStarted",
                          handler => _CPOServer.OnLogRemoteStarted += handler,
                          handler => _CPOServer.OnLogRemoteStarted -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteStop",
                          handler => _CPOServer.OnLogRemoteStop    += handler,
                          handler => _CPOServer.OnLogRemoteStop    -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteStopped",
                          handler => _CPOServer.OnLogRemoteStopped += handler,
                          handler => _CPOServer.OnLogRemoteStopped -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}