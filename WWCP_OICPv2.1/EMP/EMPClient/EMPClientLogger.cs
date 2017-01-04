/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient
    {

        /// <summary>
        /// An OICP EMP client (HTTP/SOAP client) logger.
        /// </summary>
        public class EMPClientLogger : HTTPLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICP_EMPClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached OICP EMP client.
            /// </summary>
            public EMPClient EMPClient { get; }

            #endregion

            #region Constructor(s)

            #region EMPClientLogger(EMPClient, Context = DefaultContext, LogFileCreator = null)

            /// <summary>
            /// Create a new OICP EMP client logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClient">A OICP EMP client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPClientLogger(EMPClient                     EMPClient,
                                   String                        Context         = DefaultContext,
                                   Func<String, String, String>  LogFileCreator  = null)

                : this(EMPClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogFileCreator: LogFileCreator)

            { }

            #endregion

            #region EMPClientLogger(EMPClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new OICP EMP client logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClient">A OICP EMP client.</param>
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
            public EMPClientLogger(EMPClient                     EMPClient,
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

                if (EMPClient == null)
                    throw new ArgumentNullException(nameof(EMPClient), "The given EMP client must not be null!");

                this.EMPClient = EMPClient;

                #endregion


                // Register EVSE data/status push log events

                #region PullEVSEData

                RegisterEvent("PullEVSEDataRequest",
                              handler => EMPClient.OnPullEVSEDataSOAPRequest += handler,
                              handler => EMPClient.OnPullEVSEDataSOAPRequest -= handler,
                              "PullEVSEData", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEDataResponse",
                              handler => EMPClient.OnPullEVSEDataSOAPResponse += handler,
                              handler => EMPClient.OnPullEVSEDataSOAPResponse -= handler,
                              "PullEVSEData", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SearchEVSE

                RegisterEvent("SearchEVSERequest",
                              handler => EMPClient.OnSearchEVSESOAPRequest += handler,
                              handler => EMPClient.OnSearchEVSESOAPRequest -= handler,
                              "SearchEVSE", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SearchEVSEResponse",
                              handler => EMPClient.OnSearchEVSESOAPResponse += handler,
                              handler => EMPClient.OnSearchEVSESOAPResponse -= handler,
                              "SearchEVSE", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PullEVSEStatus/-ById

                RegisterEvent("PullEVSEStatusRequest",
                              handler => EMPClient.OnPullEVSEStatusSOAPRequest += handler,
                              handler => EMPClient.OnPullEVSEStatusSOAPRequest -= handler,
                              "PullEVSEStatus", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusResponse",
                              handler => EMPClient.OnPullEVSEStatusSOAPResponse += handler,
                              handler => EMPClient.OnPullEVSEStatusSOAPResponse -= handler,
                              "PullEVSEStatus", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusByIdRequest",
                              handler => EMPClient.OnPullEVSEStatusByIdSOAPRequest += handler,
                              handler => EMPClient.OnPullEVSEStatusByIdSOAPRequest -= handler,
                              "PullEVSEStatusById", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusByIdResponse",
                              handler => EMPClient.OnPullEVSEStatusByIdSOAPResponse += handler,
                              handler => EMPClient.OnPullEVSEStatusByIdSOAPResponse -= handler,
                              "PullEVSEStatusById", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushAuthenticationData

                RegisterEvent("PushAuthenticationDataRequest",
                              handler => EMPClient.OnPushAuthenticationDataSOAPRequest += handler,
                              handler => EMPClient.OnPushAuthenticationDataSOAPRequest -= handler,
                              "PushAuthenticationData", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushAuthenticationDataResponse",
                              handler => EMPClient.OnPushAuthenticationDataSOAPResponse += handler,
                              handler => EMPClient.OnPushAuthenticationDataSOAPResponse -= handler,
                              "PushAuthenticationData", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ReservationStart/-Stop

                RegisterEvent("ReservationStartRequest",
                              handler => EMPClient.OnReservationStartSOAPRequest  += handler,
                              handler => EMPClient.OnReservationStartSOAPRequest  -= handler,
                              "ReservationStart", "Reservation", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ReservationStartResponse",
                              handler => EMPClient.OnReservationStartSOAPResponse += handler,
                              handler => EMPClient.OnReservationStartSOAPResponse -= handler,
                              "ReservationStart", "Reservation", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ReservationStopRequest",
                              handler => EMPClient.OnReservationStopSOAPRequest += handler,
                              handler => EMPClient.OnReservationStopSOAPRequest -= handler,
                              "ReservationStop", "Reservation", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ReservationStopResponse",
                              handler => EMPClient.OnReservationStopSOAPResponse += handler,
                              handler => EMPClient.OnReservationStopSOAPResponse -= handler,
                              "ReservationStop", "Reservation", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/-Stop

                RegisterEvent("AuthorizeRemoteStartRequest",
                              handler => EMPClient.OnAuthorizeRemoteStartSOAPRequest  += handler,
                              handler => EMPClient.OnAuthorizeRemoteStartSOAPRequest  -= handler,
                              "AuthorizeRemoteStart", "AuthorizeRemote", "Authorize", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStartResponse",
                              handler => EMPClient.OnAuthorizeRemoteStartSOAPResponse += handler,
                              handler => EMPClient.OnAuthorizeRemoteStartSOAPResponse -= handler,
                              "AuthorizeRemoteStart", "AuthorizeRemote", "Authorize", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteStopRequest",
                              handler => EMPClient.OnAuthorizeRemoteStopSOAPRequest += handler,
                              handler => EMPClient.OnAuthorizeRemoteStopSOAPRequest -= handler,
                              "AuthorizeRemoteStop", "AuthorizeRemote", "Authorize", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStopResponse",
                              handler => EMPClient.OnAuthorizeRemoteStopSOAPResponse += handler,
                              handler => EMPClient.OnAuthorizeRemoteStopSOAPResponse -= handler,
                              "AuthorizeRemoteStop", "AuthorizeRemote", "Authorize", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetChargeDetailRecords

                RegisterEvent("GetChargeDetailRecordsRequest",
                              handler => EMPClient.OnGetChargeDetailRecordsSOAPRequest += handler,
                              handler => EMPClient.OnGetChargeDetailRecordsSOAPRequest -= handler,
                              "GetChargeDetailRecords", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetChargeDetailRecordsResponse",
                              handler => EMPClient.OnGetChargeDetailRecordsSOAPResponse += handler,
                              handler => EMPClient.OnGetChargeDetailRecordsSOAPResponse -= handler,
                              "GetChargeDetailRecords", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
