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

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO client.
    /// </summary>
    public partial class CPOClient
    {

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        public class Logger : HTTPClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICPCPOClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached CPO client.
            /// </summary>
            public CPOClient  CPOClient    { get; }

            #endregion

            #region Constructor(s)

            #region Logger(CPOClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(CPOClient               CPOClient,
                          String                  Context         = DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator  = null)

                : this(CPOClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region Logger(CPOClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
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
            public Logger(CPOClient                   CPOClient,
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

                : base(CPOClient,
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

                this.CPOClient = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPO client must not be null!");

                #region PushEVSEData/Status

                RegisterEvent("PushEVSEDataHTTPRequest",
                              handler => CPOClient.OnPushEVSEDataHTTPRequest += handler,
                              handler => CPOClient.OnPushEVSEDataHTTPRequest -= handler,
                              "PushEVSEData", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushEVSEDataHTTPResponse",
                              handler => CPOClient.OnPushEVSEDataHTTPResponse += handler,
                              handler => CPOClient.OnPushEVSEDataHTTPResponse -= handler,
                              "PushEVSEData", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PushEVSEStatusHTTPRequest",
                              handler => CPOClient.OnPushEVSEStatusHTTPRequest += handler,
                              handler => CPOClient.OnPushEVSEStatusHTTPRequest -= handler,
                              "PushEVSEStatus", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushEVSEStatusHTTPResponse",
                              handler => CPOClient.OnPushEVSEStatusHTTPResponse += handler,
                              handler => CPOClient.OnPushEVSEStatusHTTPResponse -= handler,
                              "PushEVSEStatus", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeStart/Stop

                RegisterEvent("AuthorizeStartHTTPRequest",
                              handler => CPOClient.OnAuthorizeStartHTTPRequest += handler,
                              handler => CPOClient.OnAuthorizeStartHTTPRequest -= handler,
                              "authorizeStart", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeStartHTTPResponse",
                              handler => CPOClient.OnAuthorizeStartHTTPResponse += handler,
                              handler => CPOClient.OnAuthorizeStartHTTPResponse -= handler,
                              "authorizeStart", "authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeStopHTTPRequest",
                              handler => CPOClient.OnAuthorizeStopHTTPRequest += handler,
                              handler => CPOClient.OnAuthorizeStopHTTPRequest -= handler,
                              "authorizeStop", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeStopHTTPResponse",
                              handler => CPOClient.OnAuthorizeStopHTTPResponse += handler,
                              handler => CPOClient.OnAuthorizeStopHTTPResponse -= handler,
                              "authorizeStop", "authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotifications

                RegisterEvent("ChargingNotificationsStartHTTPRequest",
                              handler => CPOClient.OnChargingNotificationsStartHTTPRequest += handler,
                              handler => CPOClient.OnChargingNotificationsStartHTTPRequest -= handler,
                              "chargingNotificationsStart", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingNotificationsStartHTTPResponse",
                              handler => CPOClient.OnChargingNotificationsStartHTTPResponse += handler,
                              handler => CPOClient.OnChargingNotificationsStartHTTPResponse -= handler,
                              "chargingNotificationsStart", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingNotificationsProgressHTTPRequest",
                              handler => CPOClient.OnChargingNotificationsProgressHTTPRequest += handler,
                              handler => CPOClient.OnChargingNotificationsProgressHTTPRequest -= handler,
                              "chargingNotificationsProgress", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingNotificationsProgressHTTPResponse",
                              handler => CPOClient.OnChargingNotificationsProgressHTTPResponse += handler,
                              handler => CPOClient.OnChargingNotificationsProgressHTTPResponse -= handler,
                              "chargingNotificationsProgress", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingNotificationsEndHTTPRequest",
                              handler => CPOClient.OnChargingNotificationsEndHTTPRequest += handler,
                              handler => CPOClient.OnChargingNotificationsEndHTTPRequest -= handler,
                              "chargingNotificationsEnd", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingNotificationsEndHTTPResponse",
                              handler => CPOClient.OnChargingNotificationsEndHTTPResponse += handler,
                              handler => CPOClient.OnChargingNotificationsEndHTTPResponse -= handler,
                              "chargingNotificationsEnd", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingNotificationsErrorHTTPRequest",
                              handler => CPOClient.OnChargingNotificationsErrorHTTPRequest += handler,
                              handler => CPOClient.OnChargingNotificationsErrorHTTPRequest -= handler,
                              "authorizeStop", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingNotificationsErrorHTTPResponse",
                              handler => CPOClient.OnChargingNotificationsErrorHTTPResponse += handler,
                              handler => CPOClient.OnChargingNotificationsErrorHTTPResponse -= handler,
                              "chargingNotificationsError", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SendChargeDetailRecord

                RegisterEvent("SendChargeDetailRecordHTTPRequest",
                              handler => CPOClient.OnSendChargeDetailRecordHTTPRequest += handler,
                              handler => CPOClient.OnSendChargeDetailRecordHTTPRequest -= handler,
                              "sendChargeDetailRecord", "cdr", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SendChargeDetailRecordHTTPResponse",
                              handler => CPOClient.OnSendChargeDetailRecordHTTPResponse += handler,
                              handler => CPOClient.OnSendChargeDetailRecordHTTPResponse -= handler,
                              "sendChargeDetailRecord", "cdr", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

     }

}
