/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
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
    /// An OICP CPO client logger.
    /// </summary>
    public class CPOClientLogger : HTTPLogger
    {

        #region Data

        /// <summary>
        /// The default context for this logger.
        /// </summary>
        public const String DefaultContext = "OICP_CPOClient";

        #endregion

        #region Properties

        /// <summary>
        /// The linked OICP CPO client.
        /// </summary>
        public CPOClient CPOClient { get; }

        #endregion

        #region Constructor(s)

        #region CPOClientLogger(CPOClient, Context = DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OICP CPO client logger using the default logging delegates.
        /// </summary>
        /// <param name="CPOClient">A OICP CPO client.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOClientLogger(CPOClient                    CPOClient,
                               String                       Context         = DefaultContext,
                               Func<String, String, String> LogFileCreator  = null)

            : this(CPOClient,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                   null,
                   null,
                   null,
                   null,

                   LogFileCreator: LogFileCreator)

        { }

        #endregion

        #region CPOClientLogger(CPOClient, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new OICP CPO client logger using the given logging delegates.
        /// </summary>
        /// <param name="CPOClient">A OICP CPO client.</param>
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
        public CPOClientLogger(CPOClient                     CPOClient,
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

            if (CPOClient == null)
                throw new ArgumentNullException(nameof(CPOClient), "The given CPO client must not be null!");

            this.CPOClient = CPOClient;

            #endregion

            #region Register EVSE data/status push log events

            RegisterEvent("EVSEDataPush",
                          handler => CPOClient.OnEVSEDataPushRequest    += handler,
                          handler => CPOClient.OnEVSEDataPushRequest    -= handler,
                          "EVSE", "EVSEData", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("EVSEDataPushed",
                          handler => CPOClient.OnEVSEDataPushResponse   += handler,
                          handler => CPOClient.OnEVSEDataPushResponse   -= handler,
                          "EVSE", "EVSEData", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("EVSEStatusPush",
                          handler => CPOClient.OnEVSEStatusPushRequest  += handler,
                          handler => CPOClient.OnEVSEStatusPushRequest  -= handler,
                          "EVSE", "EVSEStatus", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("EVSEStatusPushed",
                          handler => CPOClient.OnEVSEStatusPushResponse += handler,
                          handler => CPOClient.OnEVSEStatusPushResponse -= handler,
                          "EVSE", "EVSEStatus", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);



            RegisterEvent("AuthorizeStart",
                          handler => CPOClient.OnAuthorizeStartRequest += handler,
                          handler => CPOClient.OnAuthorizeStartRequest -= handler,
                          "Authorize", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthorizeStarted",
                          handler => CPOClient.OnAuthorizeStartResponse += handler,
                          handler => CPOClient.OnAuthorizeStartResponse -= handler,
                          "Authorize", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("AuthorizeStop",
                          handler => CPOClient.OnAuthorizeStopRequest += handler,
                          handler => CPOClient.OnAuthorizeStopRequest -= handler,
                          "Authorize", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthorizeStopped",
                          handler => CPOClient.OnAuthorizeStopResponse += handler,
                          handler => CPOClient.OnAuthorizeStopResponse -= handler,
                          "Authorize", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);



            RegisterEvent("SendChargeDetailRecord",
                          handler => CPOClient.OnSendChargeDetailRecordSOAPRequest += handler,
                          handler => CPOClient.OnSendChargeDetailRecordSOAPRequest -= handler,
                          "ChargeDetailRecord", "CDR", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("ChargeDetailRecordSent",
                          handler => CPOClient.OnSendChargeDetailRecordSOAPResponse += handler,
                          handler => CPOClient.OnSendChargeDetailRecordSOAPResponse -= handler,
                          "ChargeDetailRecord", "CDR", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);


            RegisterEvent("PullAuthenticationData",
                          handler => CPOClient.OnPullAuthenticationDataSOAPRequest += handler,
                          handler => CPOClient.OnPullAuthenticationDataSOAPRequest -= handler,
                          "AuthenticationData", "Request", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AuthenticationDataPulled",
                          handler => CPOClient.OnPullAuthenticationDataSOAPResponse += handler,
                          handler => CPOClient.OnPullAuthenticationDataSOAPResponse -= handler,
                          "AuthenticationData", "Response", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

        }

        #endregion

        #endregion

    }

}
