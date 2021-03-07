﻿/*
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
        public new class Logger //: HTTPClient.Logger
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
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(EMPClient               EMPClient,
                          String                  Context         = DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator  = null)

                : this(EMPClient,
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

                //: base(EMPClient,
                //       Context.IsNotNullOrEmpty() ? Context : DefaultContext,

                //       LogHTTPRequest_toConsole,
                //       LogHTTPResponse_toConsole,
                //       LogHTTPRequest_toDisc,
                //       LogHTTPResponse_toDisc,

                //       LogHTTPRequest_toNetwork,
                //       LogHTTPResponse_toNetwork,
                //       LogHTTPRequest_toHTTPSSE,
                //       LogHTTPResponse_toHTTPSSE,

                //       LogHTTPError_toConsole,
                //       LogHTTPError_toDisc,
                //       LogHTTPError_toNetwork,
                //       LogHTTPError_toHTTPSSE,

                //       LogfileCreator)

            {

                this.EMPClient = EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMP client must not be null!");

                #region Register log events

                //RegisterEvent("SendHeartbeatRequest",
                //              handler => EMPClient.OnSendHeartbeatSOAPRequest  += handler,
                //              handler => EMPClient.OnSendHeartbeatSOAPRequest  -= handler,
                //              "SendHeartbeat", "Heartbeat", "requests", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("SendHeartbeatResponse",
                //              handler => EMPClient.OnSendHeartbeatSOAPResponse += handler,
                //              handler => EMPClient.OnSendHeartbeatSOAPResponse -= handler,
                //              "SendHeartbeat", "Heartbeat", "Response", "all").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                #endregion

            }

            #endregion

            #endregion

        }

     }

}
