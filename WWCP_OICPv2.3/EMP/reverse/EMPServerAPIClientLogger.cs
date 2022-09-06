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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP server API client.
    /// </summary>
    public partial class EMPServerAPIClient
    {

        /// <summary>
        /// The EMP server API client logger.
        /// </summary>
        public class EMPServerAPIClientLogger : AClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICPEMPServerAPIClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached EMP server API client.
            /// </summary>
            public EMPServerAPIClient  EMPServerAPIClient    { get; }

            #endregion

            #region Constructor(s)

            #region EMPServerAPIClientLogger(EMPServerAPIClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new EMP server API client logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPServerAPIClient">An EMP server API client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPServerAPIClientLogger(EMPServerAPIClient       EMPServerAPIClient,
                                            String?                  LoggingPath,
                                            String                   Context         = DefaultContext,
                                            LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(EMPServerAPIClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region EMPServerAPIClientLogger(EMPServerAPIClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new EMP server API client logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPServerAPIClient">A EMP server API client.</param>
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
            /// <param name="LogRequest_toHTTPSSE">A delegate to log incoming requests to a client sent events source.</param>
            /// <param name="LogResponse_toHTTPSSE">A delegate to log requests/responses to a client sent events source.</param>
            /// 
            /// <param name="LogError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogError_toHTTPSSE">A delegate to log errors to a client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPServerAPIClientLogger(EMPServerAPIClient       EMPServerAPIClient,
                                            String?                  LoggingPath,
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

                : base(EMPServerAPIClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,

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

                this.EMPServerAPIClient = EMPServerAPIClient ?? throw new ArgumentNullException(nameof(EMPServerAPIClient), "The given EMP server API client must not be null!");

                #region AuthorizeStart/Stop

                RegisterRequestEvent("AuthorizeStartRequest",
                                     handler => EMPServerAPIClient.OnAuthorizeStartRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnAuthorizeStartRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                                     "AuthorizeStart", "Authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeStartResponse",
                                      handler => EMPServerAPIClient.OnAuthorizeStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStartRequestConverter(timestamp, sender, request), EMPServerAPIClient?.AuthorizeStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnAuthorizeStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStartRequestConverter(timestamp, sender, request), EMPServerAPIClient?.AuthorizeStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeStart", "Authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("AuthorizeStopRequest",
                                     handler => EMPServerAPIClient.OnAuthorizeStopRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnAuthorizeStopRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                                     "AuthorizeStop", "Authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeStopResponse",
                                      handler => EMPServerAPIClient.OnAuthorizeStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStopRequestConverter(timestamp, sender, request), EMPServerAPIClient?.AuthorizeStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnAuthorizeStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.AuthorizeStopRequestConverter(timestamp, sender, request), EMPServerAPIClient?.AuthorizeStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeStop", "Authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotifications

                RegisterRequestEvent("ChargingStartNotificationRequest",
                                     handler => EMPServerAPIClient.OnChargingStartNotificationRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnChargingStartNotificationRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                                     "ChargingStartNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingStartNotificationResponse",
                                      handler => EMPServerAPIClient.OnChargingStartNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnChargingStartNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "ChargingStartNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("ChargingProgressNotificationRequest",
                                     handler => EMPServerAPIClient.OnChargingProgressNotificationRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnChargingProgressNotificationRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                                     "ChargingProgressNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingProgressNotificationResponse",
                                      handler => EMPServerAPIClient.OnChargingProgressNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnChargingProgressNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "ChargingProgressNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("ChargingEndNotificationRequest",
                                     handler => EMPServerAPIClient.OnChargingEndNotificationRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnChargingEndNotificationRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                                     "ChargingEndNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingEndNotificationResponse",
                                      handler => EMPServerAPIClient.OnChargingEndNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnChargingEndNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "ChargingEndNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("ChargingErrorNotificationRequest",
                                     handler => EMPServerAPIClient.OnChargingErrorNotificationRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnChargingErrorNotificationRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                                     "ChargingErrorNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingErrorNotificationResponse",
                                      handler => EMPServerAPIClient.OnChargingErrorNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnChargingErrorNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "ChargingErrorNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SendChargeDetailRecord

                RegisterRequestEvent("SendChargeDetailRecordRequest",
                                     handler => EMPServerAPIClient.OnChargeDetailRecordRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargeDetailRecordRequestConverter(timestamp, sender, request)),
                                     handler => EMPServerAPIClient.OnChargeDetailRecordRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPServerAPIClient?.ChargeDetailRecordRequestConverter(timestamp, sender, request)),
                                     "AuthorizeStart", "Authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("SendChargeDetailRecordResponse",
                                      handler => EMPServerAPIClient.OnChargeDetailRecordResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargeDetailRecordRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => EMPServerAPIClient.OnChargeDetailRecordResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPServerAPIClient?.ChargeDetailRecordRequestConverter(timestamp, sender, request), EMPServerAPIClient?.ChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeStart", "Authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
