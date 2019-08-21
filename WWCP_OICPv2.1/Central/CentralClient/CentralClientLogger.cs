/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// An OICP Central client.
    /// </summary>
    public partial class CentralClient : ASOAPClient
    {

        /// <summary>
        /// An OICP Central client (HTTP/SOAP client) logger.
        /// </summary>
        public class CentralClientLogger : HTTPClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICP_CentralClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached OICP Central client.
            /// </summary>
            public ICentralClient CentralClient { get; }

            #endregion

            #region Constructor(s)

            #region CentralClientLogger(CentralClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new OICP Central client logger using the default logging delegates.
            /// </summary>
            /// <param name="CentralClient">A OICP Central client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CentralClientLogger(ICentralClient          CentralClient,
                                       String                  Context         = DefaultContext,
                                       LogfileCreatorDelegate  LogfileCreator  = null)

                : this(CentralClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region CentralClientLogger(CentralClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new OICP Central client logger using the given logging delegates.
            /// </summary>
            /// <param name="CentralClient">A OICP Central client.</param>
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
            public CentralClientLogger(ICentralClient              CentralClient,
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

                : base(CentralClient,
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

                #region Initial checks

                if (CentralClient == null)
                    throw new ArgumentNullException(nameof(CentralClient), "The given Central client must not be null!");

                this.CentralClient = CentralClient;

                #endregion


                // Register EVSE data/status push log events

                //#region PullEVSEData

                //RegisterEvent("PullEVSEDataRequest",
                //              handler => CentralClient.OnPullEVSEDataSOAPRequest += handler,
                //              handler => CentralClient.OnPullEVSEDataSOAPRequest -= handler,
                //              "PullEVSEData", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PullEVSEDataResponse",
                //              handler => CentralClient.OnPullEVSEDataSOAPResponse += handler,
                //              handler => CentralClient.OnPullEVSEDataSOAPResponse -= handler,
                //              "PullEVSEData", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

                //#region PullAuthorizationStart/-ById

                //RegisterEvent("AuthorizeStartRequest",
                //              handler => CentralClient.OnPullAuthorizationStartSOAPRequest += handler,
                //              handler => CentralClient.OnPullAuthorizationStartSOAPRequest -= handler,
                //              "PullAuthorizationStart", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PullAuthorizationStartResponse",
                //              handler => CentralClient.OnPullAuthorizationStartSOAPResponse += handler,
                //              handler => CentralClient.OnPullAuthorizationStartSOAPResponse -= handler,
                //              "PullAuthorizationStart", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("PullAuthorizationStartByIdRequest",
                //              handler => CentralClient.OnPullAuthorizationStartByIdSOAPRequest += handler,
                //              handler => CentralClient.OnPullAuthorizationStartByIdSOAPRequest -= handler,
                //              "PullAuthorizationStartById", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PullAuthorizationStartByIdResponse",
                //              handler => CentralClient.OnPullAuthorizationStartByIdSOAPResponse += handler,
                //              handler => CentralClient.OnPullAuthorizationStartByIdSOAPResponse -= handler,
                //              "PullAuthorizationStartById", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

                //#region PushAuthenticationData

                //RegisterEvent("PushAuthenticationDataRequest",
                //              handler => CentralClient.OnPushAuthenticationDataSOAPRequest += handler,
                //              handler => CentralClient.OnPushAuthenticationDataSOAPRequest -= handler,
                //              "PushAuthenticationData", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("PushAuthenticationDataResponse",
                //              handler => CentralClient.OnPushAuthenticationDataSOAPResponse += handler,
                //              handler => CentralClient.OnPushAuthenticationDataSOAPResponse -= handler,
                //              "PushAuthenticationData", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

                //#region ReservationStart/-Stop

                //RegisterEvent("ReservationStartRequest",
                //              handler => CentralClient.OnAuthorizeRemoteReservationStartSOAPRequest  += handler,
                //              handler => CentralClient.OnAuthorizeRemoteReservationStartSOAPRequest  -= handler,
                //              "ReservationStart", "Reservation", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ReservationStartResponse",
                //              handler => CentralClient.OnAuthorizeRemoteReservationStartSOAPResponse += handler,
                //              handler => CentralClient.OnAuthorizeRemoteReservationStartSOAPResponse -= handler,
                //              "ReservationStart", "Reservation", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("ReservationStopRequest",
                //              handler => CentralClient.OnAuthorizeRemoteReservationStopSOAPRequest += handler,
                //              handler => CentralClient.OnAuthorizeRemoteReservationStopSOAPRequest -= handler,
                //              "ReservationStop", "Reservation", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ReservationStopResponse",
                //              handler => CentralClient.OnAuthorizeRemoteReservationStopSOAPResponse += handler,
                //              handler => CentralClient.OnAuthorizeRemoteReservationStopSOAPResponse -= handler,
                //              "ReservationStop", "Reservation", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

                //#region AuthorizeRemoteStart/-Stop

                //RegisterEvent("AuthorizeRemoteStartRequest",
                //              handler => CentralClient.OnAuthorizeRemoteStartSOAPRequest  += handler,
                //              handler => CentralClient.OnAuthorizeRemoteStartSOAPRequest  -= handler,
                //              "AuthorizeRemoteStart", "AuthorizeRemote", "Authorize", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("AuthorizeRemoteStartResponse",
                //              handler => CentralClient.OnAuthorizeRemoteStartSOAPResponse += handler,
                //              handler => CentralClient.OnAuthorizeRemoteStartSOAPResponse -= handler,
                //              "AuthorizeRemoteStart", "AuthorizeRemote", "Authorize", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("AuthorizeRemoteStopRequest",
                //              handler => CentralClient.OnAuthorizeRemoteStopSOAPRequest += handler,
                //              handler => CentralClient.OnAuthorizeRemoteStopSOAPRequest -= handler,
                //              "AuthorizeRemoteStop", "AuthorizeRemote", "Authorize", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("AuthorizeRemoteStopResponse",
                //              handler => CentralClient.OnAuthorizeRemoteStopSOAPResponse += handler,
                //              handler => CentralClient.OnAuthorizeRemoteStopSOAPResponse -= handler,
                //              "AuthorizeRemoteStop", "AuthorizeRemote", "Authorize", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

                //#region GetChargeDetailRecords

                //RegisterEvent("GetChargeDetailRecordsRequest",
                //              handler => CentralClient.OnGetChargeDetailRecordsSOAPRequest += handler,
                //              handler => CentralClient.OnGetChargeDetailRecordsSOAPRequest -= handler,
                //              "GetChargeDetailRecords", "Requests", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("GetChargeDetailRecordsResponse",
                //              handler => CentralClient.OnGetChargeDetailRecordsSOAPResponse += handler,
                //              handler => CentralClient.OnGetChargeDetailRecordsSOAPResponse -= handler,
                //              "GetChargeDetailRecords", "Responses", "All").
                //    RegisterDefaultConsoleLogTarget(this).
                //    RegisterDefaultDiscLogTarget(this);

                //#endregion

            }

            #endregion

            #endregion

        }

    }

}
