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
        public class EMPClientAPILogger : AServerLogger
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

            #region EMPClientAPILogger(EMPClientAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new EMP Client API logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClientAPI">The EMP Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPClientAPILogger(EMPClientAPI             EMPClientAPI,
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

            #region EMPClientAPILogger(EMPClientAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new EMP Client API logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClientAPI">An EMP Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// 
            /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming requests to console.</param>
            /// <param name="LogHTTPResponse_toConsole">A delegate to log requests/responses to console.</param>
            /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming requests to disc.</param>
            /// <param name="LogHTTPResponse_toDisc">A delegate to log requests/responses to disc.</param>
            /// 
            /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming requests to a network target.</param>
            /// <param name="LogHTTPResponse_toNetwork">A delegate to log requests/responses to a network target.</param>
            /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming requests to a server sent events source.</param>
            /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log requests/responses to a server sent events source.</param>
            /// 
            /// <param name="LogHTTPError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogHTTPError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogHTTPError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogHTTPError_toHTTPSSE">A delegate to log errors to a server sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPClientAPILogger(EMPClientAPI             EMPClientAPI,
                                      String                   LoggingPath,
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

                : base(EMPClientAPI.HTTPServer,
                       LoggingPath,
                       Context,

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

                this.EMPClientAPI = EMPClientAPI ?? throw new ArgumentNullException(nameof(EMPClientAPI), "The given EMP Client API must not be null!");

                #region PullEVSEData/-Status

                RegisterEvent("PullEVSEDataRequest",
                               handler => EMPClientAPI.OnPullEVSEDataRequest               += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEDataRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPullEVSEDataRequest               -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEDataRequestConverter(timestamp, sender, request)),
                               "PullEVSEData", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEDataResponse",
                              handler => EMPClientAPI.OnPullEVSEDataResponse               += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEDataRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => EMPClientAPI.OnPullEVSEDataResponse               -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEDataRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "PullEVSEData", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusRequest",
                               handler => EMPClientAPI.OnPullEVSEStatusRequest             += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPullEVSEStatusRequest             -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusRequestConverter(timestamp, sender, request)),
                               "PullEVSEStatus", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusResponse",
                               handler => EMPClientAPI.OnPullEVSEStatusResponse            += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnPullEVSEStatusResponse            -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "PullEVSEStatus", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusByIdRequest",
                               handler => EMPClientAPI.OnPullEVSEStatusByIdRequest         += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPullEVSEStatusByIdRequest         -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request)),
                               "PullEVSEStatusById", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusByIdResponse",
                               handler => EMPClientAPI.OnPullEVSEStatusByIdResponse        += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEStatusByIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnPullEVSEStatusByIdResponse        -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByIdRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEStatusByIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "PullEVSEStatusById", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEStatusByOperatorIdRequest",
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdRequest += (timestamp, sender, request)                     => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdRequest -= (timestamp, sender, request)                     => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request)),
                               "PullEVSEStatusByOperatorId", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEStatusByOperatorIdResponse",
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEStatusByOperatorIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnPullEVSEStatusByOperatorIdResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEStatusByOperatorIdRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEStatusByOperatorIdResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "PullEVSEStatusByOperatorId", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PullPricingProductData/-EVSEPricing

                RegisterEvent("PullPricingProductDataRequest",
                               handler => EMPClientAPI.OnPullPricingProductDataRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullPricingProductDataRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPullPricingProductDataRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullPricingProductDataRequestConverter(timestamp, sender, request)),
                               "PullPricingProductData", "PullPricing", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullPricingProductDataResponse",
                               handler => EMPClientAPI.OnPullPricingProductDataResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullPricingProductDataRequestConverter(timestamp, sender, request), EMPClientAPI?.PullPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnPullPricingProductDataResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullPricingProductDataRequestConverter(timestamp, sender, request), EMPClientAPI?.PullPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "PullPricingProductData", "PullPricing", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PullEVSEPricingRequest",
                               handler => EMPClientAPI.OnPullEVSEPricingRequest         += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEPricingRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPullEVSEPricingRequest         -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PullEVSEPricingRequestConverter(timestamp, sender, request)),
                               "PullEVSEPricing", "PullPricing", "pull", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PullEVSEPricingResponse",
                               handler => EMPClientAPI.OnPullEVSEPricingResponse        += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEPricingRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnPullEVSEPricingResponse        -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PullEVSEPricingRequestConverter(timestamp, sender, request), EMPClientAPI?.PullEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "PullEVSEPricing", "PullPricing", "pull", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushAuthenticationData

                RegisterEvent("PushAuthenticationDataRequest",
                               handler => EMPClientAPI.OnPushAuthenticationDataRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PushAuthenticationDataRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnPushAuthenticationDataRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.PushAuthenticationDataRequestConverter(timestamp, sender, request)),
                               "PushAuthenticationData", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushAuthenticationDataResponse",
                               handler => EMPClientAPI.OnPushAuthenticationDataResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PushAuthenticationDataRequestConverter(timestamp, sender, request), EMPClientAPI?.PushAuthenticationDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnPushAuthenticationDataResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.PushAuthenticationDataRequestConverter(timestamp, sender, request), EMPClientAPI?.PushAuthenticationDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "PushAuthenticationData", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteReservationStart/-Stop

                RegisterEvent("AuthorizeRemoteReservationStartRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request)),
                               "AuthorizeRemoteReservationStart", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteReservationStartResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteReservationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStartRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteReservationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "AuthorizeRemoteReservationStart", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteReservationStopRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request)),
                               "AuthorizeRemoteReservationStop", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteReservationStopResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteReservationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnAuthorizeRemoteReservationStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteReservationStopRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteReservationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "AuthorizeRemoteReservationStop", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeRemoteStart/-Stop

                RegisterEvent("AuthorizeRemoteStartRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteStartRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnAuthorizeRemoteStartRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request)),
                               "AuthorizeRemoteStart", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStartResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnAuthorizeRemoteStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStartRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "AuthorizeRemoteStart", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeRemoteStopRequest",
                               handler => EMPClientAPI.OnAuthorizeRemoteStopRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnAuthorizeRemoteStopRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request)),
                               "AuthorizeRemoteStop", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizeRemoteStopResponse",
                               handler => EMPClientAPI.OnAuthorizeRemoteStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnAuthorizeRemoteStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.AuthorizeRemoteStopRequestConverter(timestamp, sender, request), EMPClientAPI?.AuthorizeRemoteStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               "AuthorizeRemoteStop", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetChargeDetailRecords

                RegisterEvent("GetChargeDetailRecordsRequest",
                               handler => EMPClientAPI.OnGetChargeDetailRecordsRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request)),
                               handler => EMPClientAPI.OnGetChargeDetailRecordsRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, EMPClientAPI?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request)),
                               "GetChargeDetailRecords", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetChargeDetailRecordsResponse",
                               handler => EMPClientAPI.OnGetChargeDetailRecordsResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request), EMPClientAPI?.GetChargeDetailRecordsResponseConverter(timestamp, sender, request, response, runtime), runtime),
                               handler => EMPClientAPI.OnGetChargeDetailRecordsResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, EMPClientAPI?.GetChargeDetailRecordsRequestConverter(timestamp, sender, request), EMPClientAPI?.GetChargeDetailRecordsResponseConverter(timestamp, sender, request, response, runtime), runtime),
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
