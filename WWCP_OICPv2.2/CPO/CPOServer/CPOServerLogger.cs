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

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// An OICP CPO server logger.
    /// </summary>
    public class CPOServerLogger : HTTPServerLogger
    {

        #region Data

        /// <summary>
        /// The default context for this logger.
        /// </summary>
        public const String DefaultContext = "OICP_CPOServer";

        #endregion

        #region Properties

        /// <summary>
        /// The attached CPO server.
        /// </summary>
        public CPOServer  CPOServer   { get; }

        #endregion

        #region Constructor(s)

        #region CPOServerLogger(CPOServer, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new OICP CPO server logger using the default logging delegates.
        /// </summary>
        /// <param name="CPOServer">A OICP CPO server.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOServerLogger(CPOServer               CPOServer,
                               String                  Context         = DefaultContext,
                               LogfileCreatorDelegate  LogfileCreator  = null)

            : this(CPOServer,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                   null,
                   null,
                   null,
                   null,

                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region CPOServerLogger(CPOServer, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP CPO server logger using the given logging delegates.
        /// </summary>
        /// <param name="CPOServer">A OICP CPO server.</param>
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
        public CPOServerLogger(CPOServer                   CPOServer,
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

            : base(CPOServer.SOAPServer.HTTPServer,
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

            this.CPOServer = CPOServer ?? throw new ArgumentNullException(nameof(CPOServer), "The given CPO server must not be null!");

            #endregion

            #region Register log events

            // ReservationStart/-Stop

            RegisterEvent("AuthorizeRemoteReservationStartRequest",
                          handler => CPOServer.OnAuthorizeRemoteReservationStartSOAPRequest   += handler,
                          handler => CPOServer.OnAuthorizeRemoteReservationStartSOAPRequest   -= handler,
                          "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthorizeRemoteReservationStartResponse",
                          handler => CPOServer.OnAuthorizeRemoteReservationStartSOAPResponse += handler,
                          handler => CPOServer.OnAuthorizeRemoteReservationStartSOAPResponse -= handler,
                          "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("AuthorizeRemoteReservationStopRequest",
                          handler => CPOServer.OnAuthorizeRemoteReservationStopSOAPRequest    += handler,
                          handler => CPOServer.OnAuthorizeRemoteReservationStopSOAPRequest    -= handler,
                          "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthorizeRemoteReservationStopResponse",
                          handler => CPOServer.OnAuthorizeRemoteReservationStopSOAPResponse += handler,
                          handler => CPOServer.OnAuthorizeRemoteReservationStopSOAPResponse -= handler,
                          "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "Reservation", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);



            // RemoteStart/-Stop

            RegisterEvent("AuthorizeRemoteStartRequest",
                          handler => CPOServer.OnAuthorizeRemoteStartSOAPRequest  += handler,
                          handler => CPOServer.OnAuthorizeRemoteStartSOAPRequest  -= handler,
                          "AuthorizeRemoteStart", "RemoteStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthorizeRemoteStartResponse",
                          handler => CPOServer.OnAuthorizeRemoteStartSOAPResponse += handler,
                          handler => CPOServer.OnAuthorizeRemoteStartSOAPResponse -= handler,
                          "AuthorizeRemoteStart", "RemoteStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("AuthorizeRemoteStopRequest",
                          handler => CPOServer.OnAuthorizeRemoteStopSOAPRequest   += handler,
                          handler => CPOServer.OnAuthorizeRemoteStopSOAPRequest   -= handler,
                          "AuthorizeRemoteStop", "RemoteStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthorizeRemoteStopResponse",
                          handler => CPOServer.OnAuthorizeRemoteStopSOAPResponse += handler,
                          handler => CPOServer.OnAuthorizeRemoteStopSOAPResponse -= handler,
                          "AuthorizeRemoteStop", "RemoteStart", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
