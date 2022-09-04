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

using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO Server API.
    /// </summary>
    public partial class CPOServerAPI
    {

        /// <summary>
        /// The CPO Server API logger.
        /// </summary>
        public class ServerAPILogger : AServerLogger
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

            #region ServerAPILogger(CPOServerAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO Server API logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOServerAPI">An CPO Server API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public ServerAPILogger(CPOServerAPI             CPOServerAPI,
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

            #region ServerAPILogger(CPOServerAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO Server API logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOServerAPI">An CPO Server API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// 
            /// <param name="LogRequest_toConsole">A delegate to log incoming requests to console.</param>
            /// <param name="LogResponse_toConsole">A delegate to log requests/responses to console.</param>
            /// <param name="LogRequest_toDisc">A delegate to log incoming requests to disc.</param>
            /// <param name="LogResponse_toDisc">A delegate to log requests/responses to disc.</param>
            /// 
            /// <param name="LogRequest_toNetwork">A delegate to log incoming requests to a network target.</param>
            /// <param name="LogResponse_toNetwork">A delegate to log requests/responses to a network target.</param>
            /// <param name="LogRequest_toHTTPSSE">A delegate to log incoming requests to a HTTP server sent events source.</param>
            /// <param name="LogResponse_toHTTPSSE">A delegate to log requests/responses to a HTTP server sent events source.</param>
            /// 
            /// <param name="LogError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogError_toHTTPSSE">A delegate to log errors to a HTTP server sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public ServerAPILogger(CPOServerAPI             CPOServerAPI,
                                   String                   LoggingPath,
                                   String                   Context,

                                   RequestLoggerDelegate?   LogRequest_toConsole    = null,
                                   ResponseLoggerDelegate?  LogResponse_toConsole   = null,
                                   RequestLoggerDelegate?   LogRequest_toDisc       = null,
                                   ResponseLoggerDelegate?  LogResponse_toDisc      = null,

                                   RequestLoggerDelegate?   LogRequest_toNetwork    = null,
                                   ResponseLoggerDelegate?  LogResponse_toNetwork   = null,
                                   RequestLoggerDelegate?   LogRequest_toHTTPSSE    = null,
                                   ResponseLoggerDelegate?  LogResponse_toHTTPSSE   = null,

                                   ResponseLoggerDelegate?  LogError_toConsole      = null,
                                   ResponseLoggerDelegate?  LogError_toDisc         = null,
                                   ResponseLoggerDelegate?  LogError_toNetwork      = null,
                                   ResponseLoggerDelegate?  LogError_toHTTPSSE      = null,

                                   LogfileCreatorDelegate?  LogfileCreator          = null)

                : base(CPOServerAPI.HTTPServer,
                       LoggingPath,
                       Context,

                       LogRequest_toConsole,
                       LogResponse_toConsole,
                       LogRequest_toDisc,
                       LogResponse_toDisc,

                       LogRequest_toNetwork,
                       LogResponse_toNetwork,
                       LogRequest_toHTTPSSE,
                       LogResponse_toHTTPSSE,

                       LogError_toConsole,
                       LogError_toDisc,
                       LogError_toNetwork,
                       LogError_toHTTPSSE,

                       LogfileCreator)

            {

                this.CPOServerAPI = CPOServerAPI ?? throw new ArgumentNullException(nameof(CPOServerAPI), "The given CPO Server API must not be null!");

                #region AuthorizeRemoteReservationStart/-Stop

                //RegisterEvent2("AuthorizeRemoteReservationStartRequest",
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPRequest += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPRequest -= handler,
                //               "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent2("AuthorizeRemoteReservationStartResponse",
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPResponse += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPResponse -= handler,
                //               "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "reservations", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent2("AuthorizeRemoteReservationStopRequest",
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPRequest += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPRequest -= handler,
                //               "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent2("AuthorizeRemoteReservationStopResponse",
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPResponse += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPResponse -= handler,
                //               "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "reservations", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/-Stop

                //RegisterEvent2("AuthorizeRemoteStartRequest",
                //               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPRequest += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPRequest -= handler,
                //               "AuthorizeRemoteStart", "AuthorizeRemote", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent2("AuthorizeRemoteStartResponse",
                //               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPResponse += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteStartHTTPResponse -= handler,
                //               "AuthorizeRemoteStart", "AuthorizeRemote", "authorization", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent2("AuthorizeRemoteStopRequest",
                //               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPRequest += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPRequest -= handler,
                //               "AuthorizeRemoteStop", "AuthorizeRemote", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent2("AuthorizeRemoteStopResponse",
                //               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPResponse += handler,
                //               handler => CPOServerAPI.OnAuthorizeRemoteStopHTTPResponse -= handler,
                //               "AuthorizeRemoteStop", "AuthorizeRemote", "authorization", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
