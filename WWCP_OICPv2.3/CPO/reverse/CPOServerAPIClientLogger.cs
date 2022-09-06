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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO server API client.
    /// </summary>
    public partial class CPOServerAPIClient
    {

        /// <summary>
        /// The CPO server API client (HTTP client) logger.
        /// </summary>
        public class CPOServerAPIClientLogger : AClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICPCPOServerAPIClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached CPO server API client.
            /// </summary>
            public CPOServerAPIClient  CPOServerAPIClient    { get; }

            #endregion

            #region Constructor(s)

            #region CPOServerAPIClientLogger(CPOServerAPIClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO server API client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOServerAPIClient">An CPO server API client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CPOServerAPIClientLogger(CPOServerAPIClient       CPOServerAPIClient,
                                            String?                  LoggingPath,
                                            String                   Context          = DefaultContext,
                                            LogfileCreatorDelegate?  LogfileCreator   = null)

                : this(CPOServerAPIClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region CPOServerAPIClientLogger(CPOServerAPIClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO server API client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOServerAPIClient">A CPO server API client.</param>
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
            /// <param name="LogRequest_toHTTPSSE">A delegate to log incoming requests to a HTTP client sent events source.</param>
            /// <param name="LogResponse_toHTTPSSE">A delegate to log requests/responses to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogError_toHTTPSSE">A delegate to log errors to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CPOServerAPIClientLogger(CPOServerAPIClient       CPOServerAPIClient,
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

                : base(CPOServerAPIClient,
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

                this.CPOServerAPIClient = CPOServerAPIClient ?? throw new ArgumentNullException(nameof(CPOServerAPIClient), "The given CPO server API client must not be null!");

                #region AuthorizeRemoteReservationStart/Stop

                RegisterRequestEvent("AuthorizeRemoteReservationStartRequest",
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request)),
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request)),
                                     "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeRemoteReservationStartResponse",
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteReservationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteReservationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("AuthorizeRemoteReservationStopRequest",
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request)),
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request)),
                                     "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeRemoteReservationStopResponse",
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteReservationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteReservationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/Stop

                RegisterRequestEvent("AuthorizeRemoteStartRequest",
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteStartRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request)),
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteStartRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request)),
                                     "AuthorizeRemoteStart", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeRemoteStartResponse",
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeRemoteStart", "AuthorizeRemote", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("AuthorizeRemoteStopRequest",
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteStopRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request)),
                                     handler => CPOServerAPIClient.OnAuthorizeRemoteStopRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request)),
                                     "AuthorizeRemoteStop", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeRemoteStopResponse",
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOServerAPIClient.OnAuthorizeRemoteStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOServerAPIClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request), CPOServerAPIClient?.AuthorizeRemoteStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "AuthorizeRemoteStop", "AuthorizeRemote", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
