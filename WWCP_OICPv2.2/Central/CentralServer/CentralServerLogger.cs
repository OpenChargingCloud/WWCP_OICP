/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

namespace org.GraphDefined.WWCP.OICPv2_2.Central
{

    /// <summary>
    /// An OICP Central server logger.
    /// </summary>
    public class CentralServerLogger : HTTPServerLogger
    {

        #region Data

        /// <summary>
        /// The default context for this logger.
        /// </summary>
        public const String DefaultContext = "OICP_CentralServer";

        #endregion

        #region Properties

        /// <summary>
        /// The linked OICP Central server.
        /// </summary>
        public CentralServer CentralServer { get; }

        #endregion

        #region Constructor(s)

        #region CentralServerLogger(CentralServer, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new OICP Central server logger using the default logging delegates.
        /// </summary>
        /// <param name="CentralServer">A OICP Central server.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CentralServerLogger(CentralServer           CentralServer,
                                   String                  Context         = DefaultContext,
                                   LogfileCreatorDelegate  LogfileCreator  = null)

            : this(CentralServer,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                   null,
                   null,
                   null,
                   null,

                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region CentralServerLogger(CentralServer, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP Central server logger using the given logging delegates.
        /// </summary>
        /// <param name="CentralServer">A OICP Central server.</param>
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
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CentralServerLogger(CentralServer               CentralServer,
                                   String                      Context,

                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toConsole,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toConsole,
                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toDisc,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toDisc,

                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toNetwork    = null,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toNetwork   = null,
                                   HTTPRequestLoggerDelegate   LogHTTPRequest_toHTTPSSE    = null,
                                   HTTPResponseLoggerDelegate  LogHTTPResponse_toHTTPSSE   = null,

                                   HTTPResponseLoggerDelegate  LogHTTPError_toConsole      = null,
                                   HTTPResponseLoggerDelegate  LogHTTPError_toDisc         = null,
                                   HTTPResponseLoggerDelegate  LogHTTPError_toNetwork      = null,
                                   HTTPResponseLoggerDelegate  LogHTTPError_toHTTPSSE      = null,

                                   LogfileCreatorDelegate      LogfileCreator              = null)

            : base(CentralServer.SOAPServer.HTTPServer,
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

                   LogfileCreator)

        {

            #region Initial checks

            if (CentralServer == null)
                throw new ArgumentNullException(nameof(CentralServer), "The given Central server must not be null!");

            #endregion

            this.CentralServer = CentralServer;

            #region Register remote start/stop log events

            RegisterEvent("RemoteReservationStart",
                          handler => CentralServer.OnAuthorizeRemoteReservationStartSOAPRequest   += handler,
                          handler => CentralServer.OnAuthorizeRemoteReservationStartSOAPRequest   -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteReservationStarted",
                          handler => CentralServer.OnAuthorizeRemoteReservationStartSOAPResponse += handler,
                          handler => CentralServer.OnAuthorizeRemoteReservationStartSOAPResponse -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteReservationStop",
                          handler => CentralServer.OnAuthorizeRemoteReservationStopSOAPRequest    += handler,
                          handler => CentralServer.OnAuthorizeRemoteReservationStopSOAPRequest    -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteReservationStopped",
                          handler => CentralServer.OnAuthorizeRemoteReservationStopSOAPResponse += handler,
                          handler => CentralServer.OnAuthorizeRemoteReservationStopSOAPResponse -= handler,
                          "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("RemoteStart",
                          handler => CentralServer.OnAuthorizeRemoteStartSOAPResponse   += handler,
                          handler => CentralServer.OnAuthorizeRemoteStartSOAPResponse   -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteStarted",
                          handler => CentralServer.OnAuthorizeRemoteStartSOAPResponse += handler,
                          handler => CentralServer.OnAuthorizeRemoteStartSOAPResponse -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteStop",
                          handler => CentralServer.OnAuthorizeRemoteStopSOAPResponse    += handler,
                          handler => CentralServer.OnAuthorizeRemoteStopSOAPResponse    -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("RemoteStopped",
                          handler => CentralServer.OnAuthorizeRemoteStopSOAPResponse += handler,
                          handler => CentralServer.OnAuthorizeRemoteStopSOAPResponse -= handler,
                          "Remote", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
