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

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Client API.
    /// </summary>
    public partial class CPOClientAPI
    {

        /// <summary>
        /// A CPO HTTP Client API HTTP logger.
        /// </summary>
        public class HTTP_Logger : HTTPServerLogger
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

            #region HTTP_Logger(CPOClientAPI, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new CPO Client API HTTP logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClientAPI">An CPO Client API.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public HTTP_Logger(CPOClientAPI             CPOClientAPI,
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

            #region HTTP_Logger(CPOClientAPI, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new CPO Client API HTTP logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClientAPI">An CPO Client API.</param>
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
            public HTTP_Logger(CPOClientAPI                 CPOClientAPI,
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

                : base(CPOClientAPI.HTTPServer,
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

                this.CPOClientAPI = CPOClientAPI ?? throw new ArgumentNullException(nameof(CPOClientAPI), "The given CPO Client API must not be null!");

                #region PushEVSEData/-Status

                RegisterEvent2("PushEVSEDataRequest",
                               handler => CPOClientAPI.OnPushEVSEDataHTTPRequest += handler,
                               handler => CPOClientAPI.OnPushEVSEDataHTTPRequest -= handler,
                               "PushEVSEData", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PushEVSEDataResponse",
                               handler => CPOClientAPI.OnPushEVSEDataHTTPResponse += handler,
                               handler => CPOClientAPI.OnPushEVSEDataHTTPResponse -= handler,
                               "PushEVSEData", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("PushEVSEStatusRequest",
                               handler => CPOClientAPI.OnPushEVSEStatusHTTPRequest += handler,
                               handler => CPOClientAPI.OnPushEVSEStatusHTTPRequest -= handler,
                               "PushEVSEStatus", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PushEVSEStatusResponse",
                               handler => CPOClientAPI.OnPushEVSEStatusHTTPResponse += handler,
                               handler => CPOClientAPI.OnPushEVSEStatusHTTPResponse -= handler,
                               "PushEVSEStatus", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region PushPricingProductData/-EVSEPricing

                RegisterEvent2("PushPricingProductDataRequest",
                               handler => CPOClientAPI.OnPushPricingProductDataHTTPRequest += handler,
                               handler => CPOClientAPI.OnPushPricingProductDataHTTPRequest -= handler,
                               "PushPricingProductData", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PushPricingProductDataResponse",
                               handler => CPOClientAPI.OnPushPricingProductDataHTTPResponse += handler,
                               handler => CPOClientAPI.OnPushPricingProductDataHTTPResponse -= handler,
                               "PushPricingProductData", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("PushEVSEPricingRequest",
                               handler => CPOClientAPI.OnPushEVSEPricingHTTPRequest += handler,
                               handler => CPOClientAPI.OnPushEVSEPricingHTTPRequest -= handler,
                               "PushEVSEPricing", "PushPricing", "push", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("PushEVSEPricingResponse",
                               handler => CPOClientAPI.OnPushEVSEPricingHTTPResponse += handler,
                               handler => CPOClientAPI.OnPushEVSEPricingHTTPResponse -= handler,
                               "PushEVSEPricing", "PushPricing", "push", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AuthorizeStart/-Stop

                RegisterEvent2("AuthorizeStartRequest",
                               handler => CPOClientAPI.OnAuthorizeStartHTTPRequest += handler,
                               handler => CPOClientAPI.OnAuthorizeStartHTTPRequest -= handler,
                               "AuthorizeStart", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizationStartResponse",
                               handler => CPOClientAPI.OnAuthorizationStartHTTPResponse += handler,
                               handler => CPOClientAPI.OnAuthorizationStartHTTPResponse -= handler,
                               "AuthorizeStart", "authorize", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);


                RegisterEvent2("AuthorizeStopRequest",
                               handler => CPOClientAPI.OnAuthorizeStopHTTPRequest += handler,
                               handler => CPOClientAPI.OnAuthorizeStopHTTPRequest -= handler,
                               "AuthorizeStop", "authorize", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("AuthorizationStopResponse",
                               handler => CPOClientAPI.OnAuthorizationStopHTTPResponse += handler,
                               handler => CPOClientAPI.OnAuthorizationStopHTTPResponse -= handler,
                               "AuthorizeStop", "authorize", "authorization", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargingNotification

                RegisterEvent2("ChargingNotificationRequest",
                               handler => CPOClientAPI.OnChargingNotificationHTTPRequest += handler,
                               handler => CPOClientAPI.OnChargingNotificationHTTPRequest -= handler,
                               "ChargingNotification", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("ChargingNotificationResponse",
                               handler => CPOClientAPI.OnChargingNotificationHTTPResponse += handler,
                               handler => CPOClientAPI.OnChargingNotificationHTTPResponse -= handler,
                               "ChargingNotification", "responses", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ChargeDetailRecord

                RegisterEvent2("ChargeDetailRecordRequest",
                               handler => CPOClientAPI.OnChargeDetailRecordHTTPRequest += handler,
                               handler => CPOClientAPI.OnChargeDetailRecordHTTPRequest -= handler,
                               "ChargeDetailRecord", "requests", "all").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent2("ChargeDetailRecordResponse",
                               handler => CPOClientAPI.OnChargeDetailRecordHTTPResponse += handler,
                               handler => CPOClientAPI.OnChargeDetailRecordHTTPResponse -= handler,
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
