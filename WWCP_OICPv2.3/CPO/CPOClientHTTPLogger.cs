/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
        /// The CPO client (HTTP client) logger.
        /// </summary>
        public class HTTP_Logger : HTTPClientLogger
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

            #region HTTP_Logger(CPOClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public HTTP_Logger(CPOClient                CPOClient,
                               String?                  LoggingPath     = null,
                               String?                  Context         = DefaultContext,
                               LogfileCreatorDelegate?  LogfileCreator  = null)

                : this(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region HTTP_Logger(CPOClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClient">A CPO client.</param>
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
            public HTTP_Logger(CPOClient                    CPOClient,
                               String?                      LoggingPath                 = null,
                               String?                      Context                     = null,

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

                : base(CPOClient,
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

                #region PushPricingProductData/EVSEPricing

                RegisterEvent("PushPricingProductDataHTTPRequest",
                              handler => CPOClient.OnPushPricingProductDataHTTPRequest += handler,
                              handler => CPOClient.OnPushPricingProductDataHTTPRequest -= handler,
                              "PushPricingProductData", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushPricingProductDataHTTPResponse",
                              handler => CPOClient.OnPushPricingProductDataHTTPResponse += handler,
                              handler => CPOClient.OnPushPricingProductDataHTTPResponse -= handler,
                              "PushPricingProductData", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("PushEVSEPricingHTTPRequest",
                              handler => CPOClient.OnPushEVSEPricingHTTPRequest += handler,
                              handler => CPOClient.OnPushEVSEPricingHTTPRequest -= handler,
                              "PushEVSEPricing", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("PushEVSEPricingHTTPResponse",
                              handler => CPOClient.OnPushEVSEPricingHTTPResponse += handler,
                              handler => CPOClient.OnPushEVSEPricingHTTPResponse -= handler,
                              "PushEVSEPricing", "PushPricing", "push", "responses", "all").
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

                RegisterEvent("ChargingStartNotificationHTTPRequest",
                              handler => CPOClient.OnChargingStartNotificationHTTPRequest += handler,
                              handler => CPOClient.OnChargingStartNotificationHTTPRequest -= handler,
                              "chargingStartNotification", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingStartNotificationHTTPResponse",
                              handler => CPOClient.OnChargingStartNotificationHTTPResponse += handler,
                              handler => CPOClient.OnChargingStartNotificationHTTPResponse -= handler,
                              "chargingStartNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingProgressNotificationHTTPRequest",
                              handler => CPOClient.OnChargingProgressNotificationHTTPRequest += handler,
                              handler => CPOClient.OnChargingProgressNotificationHTTPRequest -= handler,
                              "chargingProgressNotification", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingProgressNotificationHTTPResponse",
                              handler => CPOClient.OnChargingProgressNotificationHTTPResponse += handler,
                              handler => CPOClient.OnChargingProgressNotificationHTTPResponse -= handler,
                              "chargingProgressNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingEndNotificationHTTPRequest",
                              handler => CPOClient.OnChargingEndNotificationHTTPRequest += handler,
                              handler => CPOClient.OnChargingEndNotificationHTTPRequest -= handler,
                              "chargingEndNotification", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingEndNotificationHTTPResponse",
                              handler => CPOClient.OnChargingEndNotificationHTTPResponse += handler,
                              handler => CPOClient.OnChargingEndNotificationHTTPResponse -= handler,
                              "chargingEndNotification", "chargingNotifications", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent("ChargingErrorNotificationHTTPRequest",
                              handler => CPOClient.OnChargingErrorNotificationHTTPRequest += handler,
                              handler => CPOClient.OnChargingErrorNotificationHTTPRequest -= handler,
                              "authorizeStop", "chargingNotifications", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ChargingErrorNotificationHTTPResponse",
                              handler => CPOClient.OnChargingErrorNotificationHTTPResponse += handler,
                              handler => CPOClient.OnChargingErrorNotificationHTTPResponse -= handler,
                              "chargingErrorNotification", "chargingNotifications", "responses", "all").
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
