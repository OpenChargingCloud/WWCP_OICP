﻿/*
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

using System;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP HTTP Client API.
    /// </summary>
    public partial class EMPClientAPI
    {

        /// <summary>
        /// A EMP HTTP Client API logger.
        /// </summary>
        public class Logger : HTTPServerLogger
        {

            #region Data

            /// <summary>
            /// The default context of this logger.
            /// </summary>
            public const String DefaultContext = "EMPClientAPI";

            #endregion

            #region Properties

            /// <summary>
            /// The linked EMP Client API.
            /// </summary>
            public EMPClientAPI  EMPClientAPI    { get; }

            #endregion

            #region Constructor(s)

            #region EMPClientAPILogger(EMPClientAPI, Context = DefaultContext, LogFileCreator = null)

            /// <summary>
            /// Create a new EMP Client API logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClientAPI">An EMP Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(EMPClientAPI             EMPClientAPI,
                          String                   LoggingPath,
                          String                   Context         = DefaultContext,
                          LogfileCreatorDelegate?  LogFileCreator  = null)

                : this(EMPClientAPI,
                       LoggingPath,
                       Context,
                       null,
                       null,
                       null,
                       null,
                       LogFileCreator: LogFileCreator)

            { }

            #endregion

            #region EMPClientAPILogger(EMPClientAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new EMP Client API logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClientAPI">An EMP Client API.</param>
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
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(EMPClientAPI                 EMPClientAPI,
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

                          LogfileCreatorDelegate?      LogFileCreator              = null)

                : base(EMPClientAPI.HTTPServer,
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

                       LogFileCreator)

            {

                this.EMPClientAPI = EMPClientAPI ?? throw new ArgumentNullException(nameof(EMPClientAPI), "The given EMP Client API must not be null!");

                //#region AuthorizeStart/-Stop

                //RegisterEvent2("AuthorizeStartRequest",
                //               handler => EMPClientAPI.OnAuthorizeStartHTTPRequest += handler,
                //               handler => EMPClientAPI.OnAuthorizeStartHTTPRequest -= handler,
                //               "AuthorizeStart", "authorize", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent2("AuthorizationStartResponse",
                //               handler => EMPClientAPI.OnAuthorizationStartHTTPResponse += handler,
                //               handler => EMPClientAPI.OnAuthorizationStartHTTPResponse -= handler,
                //               "AuthorizeStart", "authorize", "authorization", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent2("AuthorizeStopRequest",
                //               handler => EMPClientAPI.OnAuthorizeStopHTTPRequest += handler,
                //               handler => EMPClientAPI.OnAuthorizeStopHTTPRequest -= handler,
                //               "AuthorizeStop", "authorize", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent2("AuthorizationStopResponse",
                //               handler => EMPClientAPI.OnAuthorizationStopHTTPResponse += handler,
                //               handler => EMPClientAPI.OnAuthorizationStopHTTPResponse -= handler,
                //               "AuthorizeStop", "authorize", "authorization", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

            }

            #endregion

            #endregion

        }

    }

}
