/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Server API.
    /// </summary>
    public partial class CPOServerAPI
    {

        /// <summary>
        /// A CPO HTTP Server API logger.
        /// </summary>
        public class Logger : HTTPServerLogger
        {

            #region Data

            /// <summary>
            /// The default context of this logger.
            /// </summary>
            public const String DefaultContext = "CPOServerAPI";

            #endregion

            #region Properties

            /// <summary>
            /// The linked CPO Server API.
            /// </summary>
            public CPOServerAPI  CPOServerAPI    { get; }

            #endregion

            #region Constructor(s)

            #region Logger(CPOServerAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO Server API logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOServerAPI">An CPO Server API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(CPOServerAPI             CPOServerAPI,
                          String                   LoggingPath,
                          String                   Context         = DefaultContext,
                          LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(CPOServerAPI,
                       LoggingPath,
                       Context,
                       null,
                       null,
                       null,
                       null,
                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region Logger(CPOServerAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO Server API logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOServerAPI">An CPO Server API.</param>
            /// <param name="LoggingPath">The logging path.</param>
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
            public Logger(CPOServerAPI                 CPOServerAPI,
                          String                       LoggingPath,
                          String                       Context,

                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                          HTTPResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                          LogfileCreatorDelegate?      LogfileCreator              = null)

                : base(CPOServerAPI.HTTPServer,
                       LoggingPath,
                       Context,

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

                this.CPOServerAPI = CPOServerAPI ?? throw new ArgumentNullException(nameof(CPOServerAPI), "The given CPO Server API must not be null!");

                #region AuthorizeRemoteReservationStart/-Stop

                RegisterEvent2("AuthorizeRemoteReservationStartRequest",
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPRequest += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPRequest -= handler,
                               "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteReservationStartResponse",
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPResponse += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPResponse -= handler,
                               "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "reservations", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("AuthorizeRemoteReservationStopRequest",
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPRequest += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPRequest -= handler,
                               "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteReservationStopResponse",
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPResponse += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPResponse -= handler,
                               "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "reservations", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/-Stop

                RegisterEvent2("AuthorizeRemoteStartRequest",
                               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPRequest += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPRequest -= handler,
                               "AuthorizeRemoteStart", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteStartResponse",
                               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPResponse += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPResponse -= handler,
                               "AuthorizeRemoteStart", "AuthorizeRemote", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("AuthorizeRemoteStopRequest",
                               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPRequest += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPRequest -= handler,
                               "AuthorizeRemoteStop", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteStopResponse",
                               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPResponse += handler,
                               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPResponse -= handler,
                               "AuthorizeRemoteStop", "AuthorizeRemote", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
