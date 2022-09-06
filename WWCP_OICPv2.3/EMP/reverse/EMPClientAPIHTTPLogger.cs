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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP HTTP Client API.
    /// </summary>
    public partial class EMPClientAPI
    {

        /// <summary>
        /// A EMP HTTP Client API logger.
        /// </summary>
        public class HTTP_Logger : HTTPServerLogger
        {

            #region Data

            /// <summary>
            /// The default context of this logger.
            /// </summary>
            public const String DefaultContext = "EMPClientAPI";

            #endregion

            #region Properties

            /// <summary>
            /// The linked EMP Client API.
            /// </summary>
            public EMPClientAPI  EMPClientAPI    { get; }

            #endregion

            #region Constructor(s)

            #region HTTP_Logger(EMPClientAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new EMP Client API logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClientAPI">The EMP Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public HTTP_Logger(EMPClientAPI             EMPClientAPI,
                               String                   LoggingPath,
                               String                   Context         = DefaultContext,
                               LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(EMPClientAPI,
                       LoggingPath,
                       Context,
                       null,
                       null,
                       null,
                       null,
                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region HTTP_Logger(EMPClientAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new EMP Client API logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClientAPI">An EMP Client API.</param>
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
            /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
            /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
            /// 
            /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
            /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
            /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
            /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public HTTP_Logger(EMPClientAPI                 EMPClientAPI,
                               String                       LoggingPath,
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

                : base(EMPClientAPI.HTTPServer,
                       LoggingPath,
                       Context,

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

                this.EMPClientAPI = EMPClientAPI ?? throw new ArgumentNullException(nameof(EMPClientAPI), "The given EMP Client API must not be null!");

                #region PullEVSEData/-Status

                RegisterEvent2("PullEVSEDataRequest",
                               handler => EMPClientAPI.OnPullEVSEDataHTTPRequest += handler,
                               handler => EMPClientAPI.OnPullEVSEDataHTTPRequest -= handler,
                               "PullEVSEData", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PullEVSEDataResponse",
                               handler => EMPClientAPI.OnPullEVSEDataHTTPResponse += handler,
                               handler => EMPClientAPI.OnPullEVSEDataHTTPResponse -= handler,
                               "PullEVSEData", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("PullEVSEStatusRequest",
                               handler => EMPClientAPI.OnPullEVSEStatusHTTPRequest += handler,
                               handler => EMPClientAPI.OnPullEVSEStatusHTTPRequest -= handler,
                               "PullEVSEStatus", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PullEVSEStatusResponse",
                               handler => EMPClientAPI.OnPullEVSEStatusHTTPResponse += handler,
                               handler => EMPClientAPI.OnPullEVSEStatusHTTPResponse -= handler,
                               "PullEVSEStatus", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("PullEVSEStatusByIdRequest",
                               handler => EMPClientAPI.OnPullEVSEStatusByIdHTTPRequest += handler,
                               handler => EMPClientAPI.OnPullEVSEStatusByIdHTTPRequest -= handler,
                               "PullEVSEStatusById", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PullEVSEStatusByIdResponse",
                               handler => EMPClientAPI.OnPullEVSEStatusByIdHTTPResponse += handler,
                               handler => EMPClientAPI.OnPullEVSEStatusByIdHTTPResponse -= handler,
                               "PullEVSEStatusById", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("PullEVSEStatusByOperatorIdRequest",
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdHTTPRequest += handler,
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdHTTPRequest -= handler,
                               "PullEVSEStatusByOperatorId", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PullEVSEStatusByOperatorIdResponse",
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdHTTPResponse += handler,
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdHTTPResponse -= handler,
                               "PullEVSEStatusByOperatorId", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PullPricingProductData/-EVSEPricing

                RegisterEvent2("PullPricingProductDataRequest",
                               handler => EMPClientAPI.OnPullPricingProductDataHTTPRequest += handler,
                               handler => EMPClientAPI.OnPullPricingProductDataHTTPRequest -= handler,
                               "PullPricingProductData", "PullPricing", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PullPricingProductDataResponse",
                               handler => EMPClientAPI.OnPullPricingProductDataHTTPResponse += handler,
                               handler => EMPClientAPI.OnPullPricingProductDataHTTPResponse -= handler,
                               "PullPricingProductData", "PullPricing", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("PullEVSEPricingRequest",
                               handler => EMPClientAPI.OnPullEVSEPricingHTTPRequest += handler,
                               handler => EMPClientAPI.OnPullEVSEPricingHTTPRequest -= handler,
                               "PullEVSEPricing", "PullPricing", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PullEVSEPricingResponse",
                               handler => EMPClientAPI.OnPullEVSEPricingHTTPResponse += handler,
                               handler => EMPClientAPI.OnPullEVSEPricingHTTPResponse -= handler,
                               "PullEVSEPricing", "PullPricing", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushAuthenticationData

                RegisterEvent2("PushAuthenticationDataRequest",
                               handler => EMPClientAPI.OnPushAuthenticationDataHTTPRequest += handler,
                               handler => EMPClientAPI.OnPushAuthenticationDataHTTPRequest -= handler,
                               "PushAuthenticationData", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PushAuthenticationDataResponse",
                               handler => EMPClientAPI.OnPushAuthenticationDataHTTPResponse += handler,
                               handler => EMPClientAPI.OnPushAuthenticationDataHTTPResponse -= handler,
                               "PushAuthenticationData", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteReservationStart/-Stop

                RegisterEvent2("AuthorizeRemoteReservationStartRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartHTTPRequest  += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartHTTPRequest  -= handler,
                               "AuthorizeRemoteReservationStart", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteReservationStartResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartHTTPResponse += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartHTTPResponse -= handler,
                               "AuthorizeRemoteReservationStart", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("AuthorizeRemoteReservationStopRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopHTTPRequest   += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopHTTPRequest   -= handler,
                               "AuthorizeRemoteReservationStop", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteReservationStopResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopHTTPResponse  += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopHTTPResponse  -= handler,
                               "AuthorizeRemoteReservationStop", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/-Stop

                RegisterEvent2("AuthorizeRemoteStartRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteStartHTTPRequest  += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteStartHTTPRequest  -= handler,
                               "AuthorizeRemoteStart", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteStartResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteStartHTTPResponse += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteStartHTTPResponse -= handler,
                               "AuthorizeRemoteStart", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("AuthorizeRemoteStopRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteStopHTTPRequest   += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteStopHTTPRequest   -= handler,
                               "AuthorizeRemoteStop", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizeRemoteStopResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteStopHTTPResponse  += handler,
                               handler => EMPClientAPI.OnAuthorizeRemoteStopHTTPResponse  -= handler,
                               "AuthorizeRemoteStop", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetChargeDetailRecords

                RegisterEvent2("GetChargeDetailRecordsRequest",
                               handler => EMPClientAPI.OnGetChargeDetailRecordsHTTPRequest += handler,
                               handler => EMPClientAPI.OnGetChargeDetailRecordsHTTPRequest -= handler,
                               "GetChargeDetailRecords", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("GetChargeDetailRecordsResponse",
                               handler => EMPClientAPI.OnGetChargeDetailRecordsHTTPResponse += handler,
                               handler => EMPClientAPI.OnGetChargeDetailRecordsHTTPResponse -= handler,
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
