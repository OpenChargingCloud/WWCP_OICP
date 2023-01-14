/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
        public class HTTP_Logger : HTTPClientLogger
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

            #region HTTP_Logger(CPOServerAPIClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO server API client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOServerAPIClient">An CPO server API client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public HTTP_Logger(CPOServerAPIClient       CPOServerAPIClient,
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

            #region HTTP_Logger(CPOServerAPIClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO server API client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOServerAPIClient">A CPO server API client.</param>
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
            /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP client sent events source.</param>
            /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
            /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
            /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
            /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public HTTP_Logger(CPOServerAPIClient           CPOServerAPIClient,
                               String?                      LoggingPath,
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

                : base(CPOServerAPIClient,
                       LoggingPath,
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

                this.CPOServerAPIClient = CPOServerAPIClient ?? throw new ArgumentNullException(nameof(CPOServerAPIClient), "The given CPO server API client must not be null!");

                #region AuthorizeRemoteReservationStart/Stop

                RegisterEvent("AuthorizeRemoteReservationStartRequest",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartHTTPRequest += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartHTTPRequest -= handler,
                              "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteReservationStartResponse",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartHTTPResponse += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStartHTTPResponse -= handler,
                              "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteReservationStopRequest",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopHTTPRequest += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopHTTPRequest -= handler,
                              "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteReservationStopResponse",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopHTTPResponse += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteReservationStopHTTPResponse -= handler,
                              "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/Stop

                RegisterEvent("AuthorizeRemoteStartRequest",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStartHTTPRequest += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStartHTTPRequest -= handler,
                              "AuthorizeRemoteStart", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStartResponse",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStartHTTPResponse += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStartHTTPResponse -= handler,
                              "AuthorizeRemoteStart", "AuthorizeRemote", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteStopRequest",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStopHTTPRequest += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStopHTTPRequest -= handler,
                              "AuthorizeRemoteStop", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStopResponse",
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStopHTTPResponse += handler,
                              handler => CPOServerAPIClient.OnAuthorizeRemoteStopHTTPResponse -= handler,
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
