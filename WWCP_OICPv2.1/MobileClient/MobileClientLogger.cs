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

    public partial class MobileClient
    {

        /// <summary>
        /// An OICP Mobile client (HTTP/SOAP client) logger.
        /// </summary>
        public class MobileClientLogger : HTTPLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OICP_MobileClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached OICP Mobile client.
            /// </summary>
            public MobileClient MobileClient { get; }

            #endregion

            #region Constructor(s)

            #region MobileClientLogger(MobileClient, Context = DefaultContext, LogFileCreator = null)

            /// <summary>
            /// Create a new OICP Mobile client logger using the default logging delegates.
            /// </summary>
            /// <param name="MobileClient">A OICP Mobile client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public MobileClientLogger(MobileClient                  MobileClient,
                                      String                        Context         = DefaultContext,
                                      Func<String, String, String>  LogFileCreator  = null)

                : this(MobileClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogFileCreator: LogFileCreator)

            { }

            #endregion

            #region MobileClientLogger(MobileClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new OICP CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="MobileClient">A OICP Mobile client.</param>
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
            public MobileClientLogger(MobileClient                  MobileClient,
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

                if (MobileClient == null)
                    throw new ArgumentNullException(nameof(MobileClient),  "The given mobile client must not be null!");

                this.MobileClient = MobileClient;

                #endregion

                #region Register EVSE data/status push log events

                //RegisterEvent("EVSEDataPush",
                //handler => MobileClient.OnPushEVSEDataSOAPRequest    += handler,
                //handler => MobileClient.OnPushEVSEDataSOAPRequest    -= handler,
                //"EVSE", "EVSEData", "Request", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("EVSEDataPushed",
                //handler => MobileClient.OnPushEVSEDataSOAPResponse   += handler,
                //handler => MobileClient.OnPushEVSEDataSOAPResponse   -= handler,
                //"EVSE", "EVSEData", "Response", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("EVSEStatusPush",
                //handler => MobileClient.OnPushEVSEStatusSOAPRequest  += handler,
                //handler => MobileClient.OnPushEVSEStatusSOAPRequest  -= handler,
                //"EVSE", "EVSEStatus", "Request", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("EVSEStatusPushed",
                //handler => MobileClient.OnPushEVSEStatusSOAPResponse += handler,
                //handler => MobileClient.OnPushEVSEStatusSOAPResponse -= handler,
                //"EVSE", "EVSEStatus", "Response", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);



                //RegisterEvent("AuthorizeStart",
                //handler => MobileClient.OnAuthorizeStartSOAPRequest += handler,
                //handler => MobileClient.OnAuthorizeStartSOAPRequest -= handler,
                //"Authorize", "Request", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("AuthorizeStarted",
                //handler => MobileClient.OnAuthorizeStartSOAPResponse += handler,
                //handler => MobileClient.OnAuthorizeStartSOAPResponse -= handler,
                //"Authorize", "Response", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("AuthorizeStop",
                //handler => MobileClient.OnAuthorizeStopSOAPRequest += handler,
                //handler => MobileClient.OnAuthorizeStopSOAPRequest -= handler,
                //"Authorize", "Request", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("AuthorizeStopped",
                //handler => MobileClient.OnAuthorizeStopSOAPResponse += handler,
                //handler => MobileClient.OnAuthorizeStopSOAPResponse -= handler,
                //"Authorize", "Response", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);



                //RegisterEvent("SendChargeDetailRecord",
                //handler => MobileClient.OnSendChargeDetailRecordSOAPRequest += handler,
                //handler => MobileClient.OnSendChargeDetailRecordSOAPRequest -= handler,
                //"ChargeDetailRecord", "CDR", "Request", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("ChargeDetailRecordSent",
                //handler => MobileClient.OnSendChargeDetailRecordSOAPResponse += handler,
                //handler => MobileClient.OnSendChargeDetailRecordSOAPResponse -= handler,
                //"ChargeDetailRecord", "CDR", "Response", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);


                //RegisterEvent("PullAuthenticationData",
                //handler => MobileClient.OnPullAuthenticationDataSOAPRequest += handler,
                //handler => MobileClient.OnPullAuthenticationDataSOAPRequest -= handler,
                //"AuthenticationData", "Request", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                //RegisterEvent("AuthenticationDataPulled",
                //handler => MobileClient.OnPullAuthenticationDataSOAPResponse += handler,
                //handler => MobileClient.OnPullAuthenticationDataSOAPResponse -= handler,
                //"AuthenticationData", "Response", "All").
                //RegisterDefaultConsoleLogTarget(this).
                //RegisterDefaultDiscLogTarget(this);

                #endregion

            }

            #endregion

            #endregion

        }

    }

}
