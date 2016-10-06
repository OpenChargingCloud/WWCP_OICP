/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP Mobile client.
    /// </summary>
    public partial class MobileClient
    {

        /// <summary>
        /// An OICP Mobile client (HTTP/SOAP client) logger.
        /// </summary>
        public class MobileClientLogger : HTTPLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICP_MobileClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached OICP Mobile client.
            /// </summary>
            public MobileClient MobileClient { get; }

            #endregion

            #region Constructor(s)

            #region MobileClientLogger(MobileClient, Context = DefaultContext, LogFileCreator = null)

            /// <summary>
            /// Create a new OICP Mobile client logger using the default logging delegates.
            /// </summary>
            /// <param name="MobileClient">A OICP Mobile client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public MobileClientLogger(MobileClient                  MobileClient,
                                      String                        Context         = DefaultContext,
                                      Func<String, String, String>  LogFileCreator  = null)

                : this(MobileClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogFileCreator: LogFileCreator)

            { }

            #endregion

            #region MobileClientLogger(MobileClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new OICP Mobile client logger using the given logging delegates.
            /// </summary>
            /// <param name="MobileClient">A OICP Mobile client.</param>
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
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public MobileClientLogger(MobileClient                  MobileClient,
                                      String                        Context,

                                      HTTPRequestLoggerDelegate     LogHTTPRequest_toConsole,
                                      HTTPResponseLoggerDelegate    LogHTTPResponse_toConsole,
                                      HTTPRequestLoggerDelegate     LogHTTPRequest_toDisc,
                                      HTTPResponseLoggerDelegate    LogHTTPResponse_toDisc,

                                      HTTPRequestLoggerDelegate     LogHTTPRequest_toNetwork   = null,
                                      HTTPResponseLoggerDelegate    LogHTTPResponse_toNetwork  = null,
                                      HTTPRequestLoggerDelegate     LogHTTPRequest_toHTTPSSE   = null,
                                      HTTPResponseLoggerDelegate    LogHTTPResponse_toHTTPSSE  = null,

                                      HTTPResponseLoggerDelegate    LogHTTPError_toConsole     = null,
                                      HTTPResponseLoggerDelegate    LogHTTPError_toDisc        = null,
                                      HTTPResponseLoggerDelegate    LogHTTPError_toNetwork     = null,
                                      HTTPResponseLoggerDelegate    LogHTTPError_toHTTPSSE     = null,

                                      Func<String, String, String>  LogFileCreator             = null)

                : base(Context.IsNotNullOrEmpty() ? Context : DefaultContext,

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

                #region Initial checks

                if (MobileClient == null)
                    throw new ArgumentNullException(nameof(MobileClient),  "The given mobile client must not be null!");

                this.MobileClient = MobileClient;

                #endregion

                #region Register EVSE data/status push log events

                RegisterEvent("MobileAuthorizeStart",
                handler => MobileClient.OnMobileAuthorizeStartSOAPRequest  += handler,
                handler => MobileClient.OnMobileAuthorizeStartSOAPRequest  -= handler,
                "Mobile", "Authorize", "AuthorizeStart", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

                RegisterEvent("MobileAuthorizeStarted",
                handler => MobileClient.OnMobileAuthorizeStartSOAPResponse += handler,
                handler => MobileClient.OnMobileAuthorizeStartSOAPResponse -= handler,
                "Mobile", "Authorize", "AuthorizeStart", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


                RegisterEvent("MobileRemoteStart",
                handler => MobileClient.OnMobileRemoteStartSOAPRequest += handler,
                handler => MobileClient.OnMobileRemoteStartSOAPRequest -= handler,
                "Mobile", "Remote", "RemoteStart", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

                RegisterEvent("MobileRemoteStarted",
                handler => MobileClient.OnMobileRemoteStartSOAPResponse += handler,
                handler => MobileClient.OnMobileRemoteStartSOAPResponse -= handler,
                "Mobile", "Remote", "RemoteStart", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


                RegisterEvent("MobileRemoteStop",
                handler => MobileClient.OnMobileRemoteStopSOAPRequest += handler,
                handler => MobileClient.OnMobileRemoteStopSOAPRequest -= handler,
                "Mobile", "Remote", "RemoteStop", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

                RegisterEvent("MobileRemoteStopped",
                handler => MobileClient.OnMobileRemoteStopSOAPResponse += handler,
                handler => MobileClient.OnMobileRemoteStopSOAPResponse -= handler,
                "Mobile", "Remote", "RemoteStop", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
