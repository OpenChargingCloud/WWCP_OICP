/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Client API.
    /// </summary>
    public partial class CPOClientAPI
    {

        /// <summary>
        /// A CPO HTTP Client API logger.
        /// </summary>
        public class CPOClientAPILogger : AServerLogger
        {

            #region Data

            /// <summary>
            /// The default context of this logger.
            /// </summary>
            public const String DefaultContext = "CPOClientAPI";

            #endregion

            #region Properties

            /// <summary>
            /// The linked CPO Client API.
            /// </summary>
            public CPOClientAPI  CPOClientAPI    { get; }

            #endregion

            #region Constructor(s)

            #region CPOClientAPILogger(CPOClientAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO Client API logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClientAPI">An CPO Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CPOClientAPILogger(CPOClientAPI             CPOClientAPI,
                                      String                   LoggingPath,
                                      String                   Context         = DefaultContext,
                                      LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(CPOClientAPI,
                       LoggingPath,
                       Context,
                       null,
                       null,
                       null,
                       null,
                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region CPOClientAPILogger(CPOClientAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO Client API logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClientAPI">An CPO Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// 
            /// <param name="LogRequest_toConsole">A delegate to log incoming requests to console.</param>
            /// <param name="LogResponse_toConsole">A delegate to log requests/responses to console.</param>
            /// <param name="LogRequest_toDisc">A delegate to log incoming requests to disc.</param>
            /// <param name="LogResponse_toDisc">A delegate to log requests/responses to disc.</param>
            /// 
            /// <param name="LogRequest_toNetwork">A delegate to log incoming requests to a network target.</param>
            /// <param name="LogResponse_toNetwork">A delegate to log requests/responses to a network target.</param>
            /// <param name="LogRequest_toHTTPSSE">A delegate to log incoming requests to a server sent events source.</param>
            /// <param name="LogResponse_toHTTPSSE">A delegate to log requests/responses to a server sent events source.</param>
            /// 
            /// <param name="LogError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogError_toHTTPSSE">A delegate to log errors to a server sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CPOClientAPILogger(CPOClientAPI             CPOClientAPI,
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

                : base(CPOClientAPI.HTTPServer,
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

                this.CPOClientAPI = CPOClientAPI ?? throw new ArgumentNullException(nameof(CPOClientAPI), "The given CPO Client API must not be null!");

                #region PushEVSEData/-Status

                RegisterEvent("PushEVSEDataRequest",
                              handler => CPOClientAPI.OnPushEVSEDataRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushEVSEDataRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnPushEVSEDataRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushEVSEDataRequestConverter(timestamp, sender, request)),
                              "PushEVSEData", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushEVSEDataResponse",
                              handler => CPOClientAPI.OnPushEVSEDataResponse   += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushEVSEDataRequestConverter(timestamp, sender, request), CPOClientAPI?.PushEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnPushEVSEDataResponse   -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushEVSEDataRequestConverter(timestamp, sender, request), CPOClientAPI?.PushEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "PushEVSEData", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PushEVSEStatusRequest",
                              handler => CPOClientAPI.OnPushEVSEStatusRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushEVSEStatusRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnPushEVSEStatusRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushEVSEStatusRequestConverter(timestamp, sender, request)),
                              "PushEVSEStatus", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushEVSEStatusResponse",
                              handler => CPOClientAPI.OnPushEVSEStatusResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushEVSEStatusRequestConverter(timestamp, sender, request), CPOClientAPI?.PushEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnPushEVSEStatusResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushEVSEStatusRequestConverter(timestamp, sender, request), CPOClientAPI?.PushEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "PushEVSEStatus", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushPricingProductData/-EVSEPricing

                RegisterEvent("PushPricingProductDataRequest",
                              handler => CPOClientAPI.OnPushPricingProductDataRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushPricingProductDataRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnPushPricingProductDataRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushPricingProductDataRequestConverter(timestamp, sender, request)),
                              "PushPricingProductData", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushPricingProductDataResponse",
                              handler => CPOClientAPI.OnPushPricingProductDataResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushPricingProductDataRequestConverter(timestamp, sender, request), CPOClientAPI?.PushPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnPushPricingProductDataResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushPricingProductDataRequestConverter(timestamp, sender, request), CPOClientAPI?.PushPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "PushPricingProductData", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PushEVSEPricingRequest",
                              handler => CPOClientAPI.OnPushEVSEPricingRequest         += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushEVSEPricingRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnPushEVSEPricingRequest         -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.PushEVSEPricingRequestConverter(timestamp, sender, request)),
                              "PushEVSEPricing", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushEVSEPricingResponse",
                              handler => CPOClientAPI.OnPushEVSEPricingResponse        += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushEVSEPricingRequestConverter(timestamp, sender, request), CPOClientAPI?.PushEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnPushEVSEPricingResponse        -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.PushEVSEPricingRequestConverter(timestamp, sender, request), CPOClientAPI?.PushEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "PushEVSEPricing", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeStart/-Stop

                RegisterEvent("AuthorizeStartRequest",
                              handler => CPOClientAPI.OnAuthorizeStartRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnAuthorizeStartRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                              "AuthorizeStart", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizationStartResponse",
                              handler => CPOClientAPI.OnAuthorizeStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.AuthorizeStartRequestConverter(timestamp, sender, request), CPOClientAPI?.AuthorizationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnAuthorizeStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.AuthorizeStartRequestConverter(timestamp, sender, request), CPOClientAPI?.AuthorizationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "AuthorizeStart", "authorize", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("AuthorizeStopRequest",
                              handler => CPOClientAPI.OnAuthorizeStopRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnAuthorizeStopRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                              "AuthorizeStop", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AuthorizationStopResponse",
                              handler => CPOClientAPI.OnAuthorizeStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.AuthorizeStopRequestConverter(timestamp, sender, request), CPOClientAPI?.AuthorizationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnAuthorizeStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.AuthorizeStopRequestConverter(timestamp, sender, request), CPOClientAPI?.AuthorizationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "AuthorizeStop", "authorize", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotification

                RegisterEvent("ChargingStartNotificationRequest",
                              handler => CPOClientAPI.OnChargingStartNotificationRequest     += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnChargingStartNotificationRequest     -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingStartNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingStartNotificationResponse",
                              handler => CPOClientAPI.OnChargingStartNotificationResponse    += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnChargingStartNotificationResponse    -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingStartNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingStartNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingProgressNotificationRequest",
                              handler => CPOClientAPI.OnChargingProgressNotificationRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnChargingProgressNotificationRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingProgressNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingProgressNotificationResponse",
                              handler => CPOClientAPI.OnChargingProgressNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnChargingProgressNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingProgressNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingEndNotificationRequest",
                              handler => CPOClientAPI.OnChargingEndNotificationRequest       += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnChargingEndNotificationRequest       -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingEndNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingEndNotificationResponse",
                              handler => CPOClientAPI.OnChargingEndNotificationResponse      += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnChargingEndNotificationResponse      -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingEndNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingEndNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingErrorNotificationRequest",
                              handler => CPOClientAPI.OnChargingErrorNotificationRequest     += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnChargingErrorNotificationRequest     -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                              "ChargingErrorNotification", "ChargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingErrorNotificationResponse",
                              handler => CPOClientAPI.OnChargingErrorNotificationResponse    += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnChargingErrorNotificationResponse    -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), CPOClientAPI?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargingErrorNotification", "ChargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargeDetailRecord

                RegisterEvent("ChargeDetailRecordRequest",
                              handler => CPOClientAPI.OnChargeDetailRecordRequest  += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.SendChargeDetailRecordRequestConverter(timestamp, sender, request)),
                              handler => CPOClientAPI.OnChargeDetailRecordRequest  -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClientAPI?.SendChargeDetailRecordRequestConverter(timestamp, sender, request)),
                              "ChargeDetailRecord", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargeDetailRecordResponse",
                              handler => CPOClientAPI.OnChargeDetailRecordResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.SendChargeDetailRecordRequestConverter(timestamp, sender, request), CPOClientAPI?.SendChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              handler => CPOClientAPI.OnChargeDetailRecordResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClientAPI?.SendChargeDetailRecordRequestConverter(timestamp, sender, request), CPOClientAPI?.SendChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                              "ChargeDetailRecord", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
