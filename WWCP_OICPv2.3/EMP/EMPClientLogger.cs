/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP client logger.
    /// </summary>
    public class EMPClientLogger : AClientLogger
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

        #region EMPClientLogger(EMPClient, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new EMP client logger using the default logging delegates.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPClientLogger(EMPClient                EMPClient,
                               String?                  LoggingPath      = null,
                               String?                  Context          = DefaultContext,
                               LogfileCreatorDelegate?  LogfileCreator   = null)

            : this(EMPClient,
                   LoggingPath,
                   Context is not null && Context.IsNotNullOrEmpty()
                       ? Context
                       : DefaultContext,
                   null,
                   null,
                   null,
                   null,

                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region EMPClientLogger(EMPClient, Context, ... Logging delegates ...)

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
        public EMPClientLogger(EMPClient                EMPClient,
                               String?                  LoggingPath                 = null,
                               String?                  Context                     = DefaultContext,

                               RequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                               ResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                               RequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                               ResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                               RequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                               ResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                               RequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                               ResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                               ResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                               ResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                               ResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                               ResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                               LogfileCreatorDelegate?  LogfileCreator              = null)

            : base(EMPClient,
                   LoggingPath,
                   Context is not null && Context.IsNotNullOrEmpty()
                       ? Context
                       : DefaultContext,

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

            RegisterRequestEvent("PullEVSEDataRequest",
                                 handler => EMPClient.OnPullEVSEDataRequest                 += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEDataRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPullEVSEDataRequest                 -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEDataRequestConverter(timestamp, sender, request)),
                                 "PullEVSEData", "pull", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PullEVSEDataResponse",
                                  handler => EMPClient.OnPullEVSEDataResponse               += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEDataRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPullEVSEDataResponse               -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEDataRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PullEVSEData", "pull", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterRequestEvent("PullEVSEStatusRequest",
                                 handler => EMPClient.OnPullEVSEStatusRequest               += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEStatusRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPullEVSEStatusRequest               -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEStatusRequestConverter(timestamp, sender, request)),
                                 "PullEVSEStatus", "pull", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PullEVSEStatusResponse",
                                  handler => EMPClient.OnPullEVSEStatusResponse             += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEStatusRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPullEVSEStatusResponse             -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEStatusRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PullEVSEStatus", "pull", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterRequestEvent("PullEVSEStatusByIdRequest",
                                 handler => EMPClient.OnPullEVSEStatusByIdRequest           += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPullEVSEStatusByIdRequest           -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request)),
                                 "PullEVSEStatusById", "pull", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PullEVSEStatusByIdResponse",
                                  handler => EMPClient.OnPullEVSEStatusByIdResponse         += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEStatusByIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPullEVSEStatusByIdResponse         -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEStatusByIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PullEVSEStatusById", "pull", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterRequestEvent("PullEVSEStatusByOperatorIdRequest",
                                 handler => EMPClient.OnPullEVSEStatusByOperatorIdRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPullEVSEStatusByOperatorIdRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request)),
                                 "PullEVSEStatusByOperatorId", "pull", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PullEVSEStatusByOperatorIdResponse",
                                  handler => EMPClient.OnPullEVSEStatusByOperatorIdResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEStatusByOperatorIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPullEVSEStatusByOperatorIdResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEStatusByOperatorIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PullEVSEStatusByOperatorId", "pull", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region PullPricingProductData/EVSEPricing

            RegisterRequestEvent("PullPricingProductDataRequest",
                                 handler => EMPClient.OnPullPricingProductDataRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullPricingProductDataRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPullPricingProductDataRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullPricingProductDataRequestConverter(timestamp, sender, request)),
                                 "PullPricingProductData", "PullPricing", "pull", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PullPricingProductDataResponse",
                                  handler => EMPClient.OnPullPricingProductDataResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullPricingProductDataRequestConverter(timestamp, sender, request), EMPClient?.PullPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPullPricingProductDataResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullPricingProductDataRequestConverter(timestamp, sender, request), EMPClient?.PullPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PullPricingProductData", "PullPricing", "pull", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterRequestEvent("PullEVSEPricingRequest",
                                 handler => EMPClient.OnPullEVSEPricingRequest          += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEPricingRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPullEVSEPricingRequest          -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PullEVSEPricingRequestConverter(timestamp, sender, request)),
                                 "PullEVSEPricing", "PullPricing", "pull", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PullEVSEPricingResponse",
                                  handler => EMPClient.OnPullEVSEPricingResponse        += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEPricingRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPullEVSEPricingResponse        -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PullEVSEPricingRequestConverter(timestamp, sender, request), EMPClient?.PullEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PullEVSEPricing", "PullPricing", "pull", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region PushAuthenticationData

            RegisterRequestEvent("PushAuthenticationDataRequest",
                                 handler => EMPClient.OnPushAuthenticationDataRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PushAuthenticationDataRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnPushAuthenticationDataRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.PushAuthenticationDataRequestConverter(timestamp, sender, request)),
                                 "PushAuthenticationData", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("PushAuthenticationDataResponse",
                                  handler => EMPClient.OnPushAuthenticationDataResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PushAuthenticationDataRequestConverter(timestamp, sender, request), EMPClient?.PushAuthenticationDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnPushAuthenticationDataResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.PushAuthenticationDataRequestConverter(timestamp, sender, request), EMPClient?.PushAuthenticationDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "PushAuthenticationData", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region AuthorizeRemoteReservationStart/Stop

            RegisterRequestEvent("AuthorizeRemoteReservationStartRequest",
                                 handler => EMPClient.OnAuthorizeRemoteReservationStartRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnAuthorizeRemoteReservationStartRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request)),
                                 "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("AuthorizeRemoteReservationStartResponse",
                                  handler => EMPClient.OnAuthorizeRemoteReservationStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteReservationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnAuthorizeRemoteReservationStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteReservationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "AuthorizeRemoteReservationStart", "AuthorizeRemoteReservation", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterRequestEvent("AuthorizeRemoteReservationStopRequest",
                                 handler => EMPClient.OnAuthorizeRemoteReservationStopRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnAuthorizeRemoteReservationStopRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request)),
                                 "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("AuthorizeRemoteReservationStopResponse",
                                  handler => EMPClient.OnAuthorizeRemoteReservationStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteReservationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnAuthorizeRemoteReservationStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteReservationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "AuthorizeRemoteReservationStop", "AuthorizeRemoteReservation", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region AuthorizeRemoteStart/Stop

            RegisterRequestEvent("AuthorizeRemoteStartRequest",
                                 handler => EMPClient.OnAuthorizeRemoteStartRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnAuthorizeRemoteStartRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request)),
                                 "AuthorizeRemoteStart", "AuthorizeRemote", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("AuthorizeRemoteStartResponse",
                                  handler => EMPClient.OnAuthorizeRemoteStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnAuthorizeRemoteStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "AuthorizeRemoteStart", "AuthorizeRemote", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterRequestEvent("AuthorizeRemoteStopRequest",
                                 handler => EMPClient.OnAuthorizeRemoteStopRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnAuthorizeRemoteStopRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request)),
                                 "AuthorizeRemoteStop", "AuthorizeRemote", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("AuthorizeRemoteStopResponse",
                                  handler => EMPClient.OnAuthorizeRemoteStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnAuthorizeRemoteStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request), EMPClient?.AuthorizeRemoteStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "AuthorizeRemoteStop", "AuthorizeRemote", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetChargeDetailRecords

            RegisterRequestEvent("GetChargeDetailRecordsRequest",
                                 handler => EMPClient.OnGetChargeDetailRecordsRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request)),
                                 handler => EMPClient.OnGetChargeDetailRecordsRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClient?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request)),
                                 "GetChargeDetailRecords", "requests", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterResponseEvent("GetChargeDetailRecordsResponse",
                                  handler => EMPClient.OnGetChargeDetailRecordsResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request), EMPClient?.GetChargeDetailRecordsResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  handler => EMPClient.OnGetChargeDetailRecordsResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClient?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request), EMPClient?.GetChargeDetailRecordsResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                  "GetChargeDetailRecords", "responses", "all").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
