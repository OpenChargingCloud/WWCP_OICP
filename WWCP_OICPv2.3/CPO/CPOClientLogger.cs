﻿/*
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO client.
    /// </summary>
    public partial class CPOClient
    {

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public class CPOClientLogger : AClientLogger
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

            #region CPOClientLogger(CPOClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CPOClientLogger(CPOClient                CPOClient,
                                   String?                  LoggingPath      = null,
                                   String?                  Context          = DefaultContext,
                                   LogfileCreatorDelegate?  LogfileCreator   = null)

                : this(CPOClient,
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

            #region CPOClientLogger(CPOClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
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
            /// <param name="LogRequest_toHTTPSSE">A delegate to log incoming requests to a client sent events source.</param>
            /// <param name="LogResponse_toHTTPSSE">A delegate to log requests/responses to a client sent events source.</param>
            /// 
            /// <param name="LogError_toConsole">A delegate to log errors to console.</param>
            /// <param name="LogError_toDisc">A delegate to log errors to disc.</param>
            /// <param name="LogError_toNetwork">A delegate to log errors to a network target.</param>
            /// <param name="LogError_toHTTPSSE">A delegate to log errors to a client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public CPOClientLogger(CPOClient                CPOClient,
                                   String?                  LoggingPath             = null,
                                   String?                  Context                 = DefaultContext,

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

                : base(CPOClient,
                       LoggingPath,
                       Context is not null && Context.IsNotNullOrEmpty()
                           ? Context
                           : DefaultContext,

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

                this.CPOClient = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPO client must not be null!");

                #region PushEVSEData/Status

                RegisterRequestEvent("PushEVSEDataRequest",
                                     handler => CPOClient.OnPushEVSEDataRequest     += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushEVSEDataRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnPushEVSEDataRequest     -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushEVSEDataRequestConverter(timestamp, sender, request)),
                                     "PushEVSEData", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("PushEVSEDataResponse",
                                      handler => CPOClient.OnPushEVSEDataResponse   += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushEVSEDataRequestConverter(timestamp, sender, request), CPOClient?.PushEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnPushEVSEDataResponse   -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushEVSEDataRequestConverter(timestamp, sender, request), CPOClient?.PushEVSEDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "PushEVSEData", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("PushEVSEStatusRequest",
                                     handler => CPOClient.OnPushEVSEStatusRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushEVSEStatusRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnPushEVSEStatusRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushEVSEStatusRequestConverter(timestamp, sender, request)),
                                     "PushEVSEStatus", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("PushEVSEStatusResponse",
                                      handler => CPOClient.OnPushEVSEStatusResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushEVSEStatusRequestConverter(timestamp, sender, request), CPOClient?.PushEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnPushEVSEStatusResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushEVSEStatusRequestConverter(timestamp, sender, request), CPOClient?.PushEVSEStatusResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "PushEVSEStatus", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushPricingProductData/EVSEPricing

                RegisterRequestEvent("PushPricingProductDataRequest",
                                     handler => CPOClient.OnPushPricingProductDataRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushPricingProductDataRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnPushPricingProductDataRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushPricingProductDataRequestConverter(timestamp, sender, request)),
                                     "PushPricingProductData", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("PushPricingProductDataResponse",
                                      handler => CPOClient.OnPushPricingProductDataResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushPricingProductDataRequestConverter(timestamp, sender, request), CPOClient?.PushPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnPushPricingProductDataResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushPricingProductDataRequestConverter(timestamp, sender, request), CPOClient?.PushPricingProductDataResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "PushPricingProductData", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("PushEVSEPricingRequest",
                                     handler => CPOClient.OnPushEVSEPricingRequest          += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushEVSEPricingRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnPushEVSEPricingRequest          -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.PushEVSEPricingRequestConverter(timestamp, sender, request)),
                                     "PushEVSEPricing", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("PushEVSEPricingResponse",
                                      handler => CPOClient.OnPushEVSEPricingResponse        += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushEVSEPricingRequestConverter(timestamp, sender, request), CPOClient?.PushEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnPushEVSEPricingResponse        -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.PushEVSEPricingRequestConverter(timestamp, sender, request), CPOClient?.PushEVSEPricingResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "PushEVSEPricing", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeStart/Stop

                RegisterRequestEvent("AuthorizeStartRequest",
                                     handler => CPOClient.OnAuthorizeStartRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnAuthorizeStartRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request)),
                                     "authorizeStart", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeStartResponse",
                                      handler => CPOClient.OnAuthorizeStartResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request), CPOClient?.AuthorizationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnAuthorizeStartResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.AuthorizeStartRequestConverter(timestamp, sender, request), CPOClient?.AuthorizationStartResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "authorizeStart", "authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("AuthorizeStopRequest",
                                     handler => CPOClient.OnAuthorizeStopRequest    += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnAuthorizeStopRequest    -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.AuthorizeStopRequestConverter(timestamp, sender, request)),
                                     "authorizeStop", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("AuthorizeStopResponse",
                                      handler => CPOClient.OnAuthorizeStopResponse  += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.AuthorizeStopRequestConverter(timestamp, sender, request), CPOClient?.AuthorizationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnAuthorizeStopResponse  -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.AuthorizeStopRequestConverter(timestamp, sender, request), CPOClient?.AuthorizationStopResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "authorizeStop", "authorize", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotifications

                RegisterRequestEvent("ChargingStartNotificationRequest",
                                     handler => CPOClient.OnChargingStartNotificationRequest      += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnChargingStartNotificationRequest      -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request)),
                                     "chargingStartNotification", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingStartNotificationResponse",
                                      handler => CPOClient.OnChargingStartNotificationResponse    += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnChargingStartNotificationResponse    -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingStartNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingStartNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "chargingStartNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("ChargingProgressNotificationRequest",
                                     handler => CPOClient.OnChargingProgressNotificationRequest   += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnChargingProgressNotificationRequest   -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request)),
                                     "chargingProgressNotification", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingProgressNotificationResponse",
                                      handler => CPOClient.OnChargingProgressNotificationResponse += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnChargingProgressNotificationResponse -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingProgressNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingProgressNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "chargingProgressNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("ChargingEndNotificationRequest",
                                     handler => CPOClient.OnChargingEndNotificationRequest        += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnChargingEndNotificationRequest        -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request)),
                                     "chargingEndNotification", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingEndNotificationResponse",
                                      handler => CPOClient.OnChargingEndNotificationResponse      += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnChargingEndNotificationResponse      -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingEndNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingEndNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "chargingEndNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterRequestEvent("ChargingErrorNotificationRequest",
                                     handler => CPOClient.OnChargingErrorNotificationRequest      += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnChargingErrorNotificationRequest      -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request)),
                                     "authorizeStop", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("ChargingErrorNotificationResponse",
                                      handler => CPOClient.OnChargingErrorNotificationResponse    += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnChargingErrorNotificationResponse    -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.ChargingErrorNotificationRequestConverter(timestamp, sender, request), CPOClient?.ChargingErrorNotificationResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      "chargingErrorNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SendChargeDetailRecord

                RegisterRequestEvent("SendChargeDetailRecordRequest",
                                     handler => CPOClient.OnSendChargeDetailRecordRequest         += (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.SendChargeDetailRecordRequestConverter(timestamp, sender, request)),
                                     handler => CPOClient.OnSendChargeDetailRecordRequest         -= (timestamp, sender, request)                    => handler(timestamp, sender, CPOClient?.SendChargeDetailRecordRequestConverter(timestamp, sender, request)),
                                     "sendChargeDetailRecord", "cdr", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterResponseEvent("SendChargeDetailRecordResponse",
                                      handler => CPOClient.OnSendChargeDetailRecordResponse       += (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.SendChargeDetailRecordRequestConverter(timestamp, sender, request), CPOClient?.SendChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
                                      handler => CPOClient.OnSendChargeDetailRecordResponse       -= (timestamp, sender, request, response, runtime) => handler(timestamp, sender, CPOClient?.SendChargeDetailRecordRequestConverter(timestamp, sender, request), CPOClient?.SendChargeDetailRecordResponseConverter(timestamp, sender, request, response, runtime), runtime),
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
