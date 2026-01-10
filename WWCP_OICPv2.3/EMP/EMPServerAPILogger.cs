/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP Server API.
    /// </summary>
    public partial class EMPServerAPI
    {

        /// <summary>
        /// The EMP Server API logger.
        /// </summary>
        public class ServerAPILogger : AServerLogger
        {

            #region Data

            /// <summary>
            /// The default context of this logger.
            /// </summary>
            public const String DefaultContext = "EMPServerAPI";

            #endregion

            #region Properties

            /// <summary>
            /// The linked EMP Server API.
            /// </summary>
            public EMPServerAPI  EMPServerAPI    { get; }

            #endregion

            #region Constructor(s)

            #region ServerAPILogger(EMPServerAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new EMP Server API logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPServerAPI">An EMP Server API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public ServerAPILogger(EMPServerAPI             EMPServerAPI,
                                   String                   LoggingPath,
                                   String                   Context         = DefaultContext,
                                   LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(EMPServerAPI,
                       LoggingPath,
                       Context,
                       null,
                       null,
                       null,
                       null,
                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region ServerAPILogger(EMPServerAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new EMP Server API logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPServerAPI">An EMP Server API.</param>
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
            /// <param name="LogRequest_toHTTPSSE">A delegate to log incoming requests to a server sent events source.</param>
            /// <param name="LogResponse_toHTTPSSE">A delegate to log requests/responses to a server sent events source.</param>
            /// 
            /// <param name="LogError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogError_toHTTPSSE">A delegate to log errors to a server sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public ServerAPILogger(EMPServerAPI             EMPServerAPI,
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

                : base(EMPServerAPI.HTTPServer,
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

                this.EMPServerAPI = EMPServerAPI ?? throw new ArgumentNullException(nameof(EMPServerAPI), "The given EMP Server API must not be null!");

                #region AuthorizeStart/-Stop

                RegisterEvent("AuthorizeStartRequest",
                              handler => EMPServerAPI.OnAuthorizeStartRequest += (timestamp, sender, request)                     => handler(timestamp, sender, EMPServerAPI?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnAuthorizeStartRequest -= (timestamp, sender, request)                     => handler(timestamp, sender, EMPServerAPI?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                              "AuthorizeStart", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizationStartResponse",
                              handler => EMPServerAPI.OnAuthorizeStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPI?.AuthorizeStartRequestConverter(timestamp, sender, request), EMPServerAPI?.AuthorizeStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnAuthorizeStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPI?.AuthorizeStartRequestConverter(timestamp, sender, request), EMPServerAPI?.AuthorizeStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "AuthorizeStart", "authorize", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeStopRequest",
                              handler => EMPServerAPI.OnAuthorizeStopRequest += (timestamp, sender, request)                      => handler(timestamp, sender, EMPServerAPI?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnAuthorizeStopRequest -= (timestamp, sender, request)                      => handler(timestamp, sender, EMPServerAPI?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                              "AuthorizeStop", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizationStopResponse",
                              handler => EMPServerAPI.OnAuthorizeStopResponse += (timestamp, sender, request, response, runtime)  => handler(timestamp, sender, EMPServerAPI?.AuthorizeStopRequestConverter(timestamp, sender, request), EMPServerAPI?.AuthorizeStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnAuthorizeStopResponse -= (timestamp, sender, request, response, runtime)  => handler(timestamp, sender, EMPServerAPI?.AuthorizeStopRequestConverter(timestamp, sender, request), EMPServerAPI?.AuthorizeStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "AuthorizeStop", "authorize", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotifications

                RegisterEvent("ChargingStartNotificationRequest",
                              handler => EMPServerAPI.OnChargingStartNotificationRequest += (timestamp, sender, request)                        => handler(timestamp, sender, EMPServerAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnChargingStartNotificationRequest -= (timestamp, sender, request)                        => handler(timestamp, sender, EMPServerAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingStartNotification", "ChargingNotification", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingStartNotificationResponse",
                              handler => EMPServerAPI.OnChargingStartNotificationResponse += (timestamp, sender, request, response, runtime)    => handler(timestamp, sender, EMPServerAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnChargingStartNotificationResponse -= (timestamp, sender, request, response, runtime)    => handler(timestamp, sender, EMPServerAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingStartNotification", "ChargingNotification", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingProgressNotificationRequest",
                              handler => EMPServerAPI.OnChargingProgressNotificationRequest += (timestamp, sender, request)                     => handler(timestamp, sender, EMPServerAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnChargingProgressNotificationRequest -= (timestamp, sender, request)                     => handler(timestamp, sender, EMPServerAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingProgressNotification", "ChargingNotification", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingProgressNotificationResponse",
                              handler => EMPServerAPI.OnChargingProgressNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnChargingProgressNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingProgressNotification", "ChargingNotification", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingEndNotificationRequest",
                              handler => EMPServerAPI.OnChargingEndNotificationRequest += (timestamp, sender, request)                          => handler(timestamp, sender, EMPServerAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnChargingEndNotificationRequest -= (timestamp, sender, request)                          => handler(timestamp, sender, EMPServerAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingEndNotification", "ChargingNotification", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingEndNotificationResponse",
                              handler => EMPServerAPI.OnChargingEndNotificationResponse += (timestamp, sender, request, response, runtime)      => handler(timestamp, sender, EMPServerAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnChargingEndNotificationResponse -= (timestamp, sender, request, response, runtime)      => handler(timestamp, sender, EMPServerAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingEndNotification", "ChargingNotification", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingErrorNotificationRequest",
                              handler => EMPServerAPI.OnChargingErrorNotificationRequest += (timestamp, sender, request)                        => handler(timestamp, sender, EMPServerAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnChargingErrorNotificationRequest -= (timestamp, sender, request)                        => handler(timestamp, sender, EMPServerAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingErrorNotification", "ChargingNotification", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingErrorNotificationResponse",
                              handler => EMPServerAPI.OnChargingErrorNotificationResponse += (timestamp, sender, request, response, runtime)    => handler(timestamp, sender, EMPServerAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnChargingErrorNotificationResponse -= (timestamp, sender, request, response, runtime)    => handler(timestamp, sender, EMPServerAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingErrorNotification", "ChargingNotification", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region Charge Detail Record

                RegisterEvent("ChargeDetailRecordRequest",
                              handler => EMPServerAPI.OnChargeDetailRecordRequest += (timestamp, sender, request)                     => handler(timestamp, sender, EMPServerAPI?.ChargeDetailRecordRequestConverter(timestamp, sender, request)),
                              handler => EMPServerAPI.OnChargeDetailRecordRequest -= (timestamp, sender, request)                     => handler(timestamp, sender, EMPServerAPI?.ChargeDetailRecordRequestConverter(timestamp, sender, request)),
                              "ChargeDetailRecord", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargeDetailRecordResponse",
                              handler => EMPServerAPI.OnChargeDetailRecordResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPI?.ChargeDetailRecordRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPServerAPI.OnChargeDetailRecordResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPI?.ChargeDetailRecordRequestConverter(timestamp, sender, request), EMPServerAPI?.ChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargeDetailRecord", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
