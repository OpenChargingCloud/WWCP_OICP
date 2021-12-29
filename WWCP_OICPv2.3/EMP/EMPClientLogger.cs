/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP client.
    /// </summary>
    public partial class EMPClient
    {

        /// <summary>
        /// The EMP client (HTTP client) logger.
        /// </summary>
        public class Logger : HTTPClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICPEMPClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached EMP client.
            /// </summary>
            public EMPClient  EMPClient    { get; }

            #endregion

            #region Constructor(s)

            #region Logger(EMPClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new EMP client logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClient">A EMP client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(EMPClient               EMPClient,
                          String                  LoggingPath,
                          String                  Context         = DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator  = null)

                : this(EMPClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region Logger(EMPClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new EMP client logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClient">A EMP client.</param>
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
            public Logger(EMPClient                   EMPClient,
                          String                      LoggingPath,
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

                : base(EMPClient,
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

                this.EMPClient = EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMP client must not be null!");

                #region PullEVSEData/Status

                RegisterEvent("PullEVSEDataHTTPRequest",
                              handler => EMPClient.OnPullEVSEDataHTTPRequest += handler,
                              handler => EMPClient.OnPullEVSEDataHTTPRequest -= handler,
                              "PullEVSEData", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEDataHTTPResponse",
                              handler => EMPClient.OnPullEVSEDataHTTPResponse += handler,
                              handler => EMPClient.OnPullEVSEDataHTTPResponse -= handler,
                              "PullEVSEData", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusHTTPRequest",
                              handler => EMPClient.OnPullEVSEStatusHTTPRequest += handler,
                              handler => EMPClient.OnPullEVSEStatusHTTPRequest -= handler,
                              "PullEVSEStatus", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusHTTPResponse",
                              handler => EMPClient.OnPullEVSEStatusHTTPResponse += handler,
                              handler => EMPClient.OnPullEVSEStatusHTTPResponse -= handler,
                              "PullEVSEStatus", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusByIdHTTPRequest",
                              handler => EMPClient.OnPullEVSEStatusByIdHTTPRequest += handler,
                              handler => EMPClient.OnPullEVSEStatusByIdHTTPRequest -= handler,
                              "PullEVSEStatusById", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusByIdHTTPResponse",
                              handler => EMPClient.OnPullEVSEStatusByIdHTTPResponse += handler,
                              handler => EMPClient.OnPullEVSEStatusByIdHTTPResponse -= handler,
                              "PullEVSEStatusById", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusByOperatorIdHTTPRequest",
                              handler => EMPClient.OnPullEVSEStatusByOperatorIdHTTPRequest += handler,
                              handler => EMPClient.OnPullEVSEStatusByOperatorIdHTTPRequest -= handler,
                              "PullEVSEStatusByOperatorId", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusByOperatorIdHTTPResponse",
                              handler => EMPClient.OnPullEVSEStatusByOperatorIdHTTPResponse += handler,
                              handler => EMPClient.OnPullEVSEStatusByOperatorIdHTTPResponse -= handler,
                              "PullEVSEStatusByOperatorId", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushAuthenticationData

                //RegisterEvent("PushAuthenticationDataRequest",
                //              handler => EMPClient.OnPushAuthenticationDataHTTPRequest += handler,
                //              handler => EMPClient.OnPushAuthenticationDataHTTPRequest -= handler,
                //              "PushAuthenticationData", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PushAuthenticationDataResponse",
                //              handler => EMPClient.OnPushAuthenticationDataHTTPResponse += handler,
                //              handler => EMPClient.OnPushAuthenticationDataHTTPResponse -= handler,
                //              "PushAuthenticationData", "responses", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteReservationStart/Stop

                RegisterEvent("AuthorizeRemoteReservationStartRequest",
                              handler => EMPClient.OnAuthorizeRemoteReservationStartHTTPRequest += handler,
                              handler => EMPClient.OnAuthorizeRemoteReservationStartHTTPRequest -= handler,
                              "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteReservationStartResponse",
                              handler => EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse += handler,
                              handler => EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse -= handler,
                              "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteReservationStopRequest",
                              handler => EMPClient.OnAuthorizeRemoteReservationStopHTTPRequest += handler,
                              handler => EMPClient.OnAuthorizeRemoteReservationStopHTTPRequest -= handler,
                              "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteReservationStopResponse",
                              handler => EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse += handler,
                              handler => EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse -= handler,
                              "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/Stop

                RegisterEvent("AuthorizeRemoteStartRequest",
                              handler => EMPClient.OnAuthorizeRemoteStartHTTPRequest += handler,
                              handler => EMPClient.OnAuthorizeRemoteStartHTTPRequest -= handler,
                              "AuthorizeRemoteStart", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStartResponse",
                              handler => EMPClient.OnAuthorizeRemoteStartHTTPResponse  += handler,
                              handler => EMPClient.OnAuthorizeRemoteStartHTTPResponse  -= handler,
                              "AuthorizeRemoteStart", "AuthorizeRemote", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteStopRequest",
                              handler => EMPClient.OnAuthorizeRemoteStopHTTPRequest += handler,
                              handler => EMPClient.OnAuthorizeRemoteStopHTTPRequest -= handler,
                              "AuthorizeRemoteStop", "AuthorizeRemote", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStopResponse",
                              handler => EMPClient.OnAuthorizeRemoteStopHTTPResponse += handler,
                              handler => EMPClient.OnAuthorizeRemoteStopHTTPResponse -= handler,
                              "AuthorizeRemoteStop", "AuthorizeRemote", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetChargeDetailRecords

                RegisterEvent("GetChargeDetailRecordsRequest",
                              handler => EMPClient.OnGetChargeDetailRecordsHTTPRequest += handler,
                              handler => EMPClient.OnGetChargeDetailRecordsHTTPRequest -= handler,
                              "GetChargeDetailRecords", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetChargeDetailRecordsResponse",
                              handler => EMPClient.OnGetChargeDetailRecordsHTTPResponse += handler,
                              handler => EMPClient.OnGetChargeDetailRecordsHTTPResponse -= handler,
                              "GetChargeDetailRecords", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
