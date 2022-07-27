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

using System;
using System.Net.Security;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Client API.
    /// </summary>
    public partial class CPOClientAPI : HTTPAPI
    {

        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  PushEVSEData                    { get; }
            public APICounterValues  PushEVSEStatus                  { get; }

            public APICounterValues  AuthorizeStart                  { get; }
            public APICounterValues  AuthorizeStop                   { get; }

            public APICounterValues  ChargingStartNotification       { get; }
            public APICounterValues  ChargingProgressNotification    { get; }
            public APICounterValues  ChargingEndNotification         { get; }
            public APICounterValues  ChargingErrorNotification       { get; }

            public APICounterValues  ChargeDetailRecord              { get; }


            public APICounters(APICounterValues? PushEVSEData                   = null,
                               APICounterValues? PushEVSEStatus                 = null,

                               APICounterValues? AuthorizeStart                 = null,
                               APICounterValues? AuthorizeStop                  = null,

                               APICounterValues? ChargingStartNotification      = null,
                               APICounterValues? ChargingProgressNotification   = null,
                               APICounterValues? ChargingEndNotification        = null,
                               APICounterValues? ChargingErrorNotification      = null,

                               APICounterValues? ChargeDetailRecord             = null)
            {

                this.PushEVSEData                  = PushEVSEData                 ?? new APICounterValues();
                this.PushEVSEStatus                = PushEVSEStatus               ?? new APICounterValues();

                this.AuthorizeStart                = AuthorizeStart               ?? new APICounterValues();
                this.AuthorizeStop                 = AuthorizeStop                ?? new APICounterValues();

                this.ChargingStartNotification     = ChargingStartNotification    ?? new APICounterValues();
                this.ChargingProgressNotification  = ChargingProgressNotification ?? new APICounterValues();
                this.ChargingEndNotification       = ChargingEndNotification      ?? new APICounterValues();
                this.ChargingErrorNotification     = ChargingErrorNotification    ?? new APICounterValues();

                this.ChargeDetailRecord            = ChargeDetailRecord           ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PushEVSEData",                  PushEVSEData.                ToJSON()),
                       new JProperty("PushEVSEStatus",                PushEVSEStatus.              ToJSON()),

                       new JProperty("AuthorizeStart",                AuthorizeStart.              ToJSON()),
                       new JProperty("AuthorizeStop",                 AuthorizeStop.               ToJSON()),

                       new JProperty("ChargingStartNotification",     ChargingStartNotification.   ToJSON()),
                       new JProperty("ChargingProgressNotification",  ChargingProgressNotification.ToJSON()),
                       new JProperty("ChargingEndNotification",       ChargingEndNotification.     ToJSON()),
                       new JProperty("ChargingErrorNotification",     ChargingErrorNotification.   ToJSON()),

                       new JProperty("ChargeDetailRecord",            ChargeDetailRecord.          ToJSON())

                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " CPO HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " CPO HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "CPOServerAPI";

        #endregion

        #region Properties

        public APICounters                                                           Counters                                           { get; }

        // Custom JSON parsers
        public CustomJObjectParserDelegate<PushEVSEDataRequest>                      CustomPushEVSEDataRequestParser                    { get; set; }
        public CustomJObjectParserDelegate<PushEVSEStatusRequest>                    CustomPushEVSEStatusRequestParser                  { get; set; }

        public CustomJObjectParserDelegate<AuthorizeStartRequest>?                   CustomAuthorizeStartRequestParser                  { get; set; }
        public CustomJObjectParserDelegate<AuthorizeStopRequest>?                    CustomAuthorizeStopRequestParser                   { get; set; }


        public CustomJObjectParserDelegate<ChargingStartNotificationRequest>?        CustomChargingStartNotificationRequestParser       { get; set; }
        public CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?     CustomChargingProgressNotificationRequestParser    { get; set; }
        public CustomJObjectParserDelegate<ChargingEndNotificationRequest>?          CustomChargingEndNotificationRequestParser         { get; set; }
        public CustomJObjectParserDelegate<ChargingErrorNotificationRequest>?        CustomChargingErrorNotificationRequestParser       { get; set; }

        public CustomJObjectParserDelegate<ChargeDetailRecordRequest>?               CustomChargeDetailRecordRequestParser              { get; set; }


        // Custom JSON serializers
        public CustomJObjectSerializerDelegate<Acknowledgement>?                     CustomAcknowledgementSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<StatusCode>?                          CustomStatusCodeSerializer                         { get; set; }

        public CustomJObjectSerializerDelegate<AuthorizationStartResponse>?          CustomAuthorizationStartSerializer                 { get; set; }
        public CustomJObjectSerializerDelegate<Identification>?                      CustomIdentificationSerializer                     { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizationStopResponse>?           CustomAuthorizationStopSerializer                  { get; set; }

        public Newtonsoft.Json.Formatting                                            JSONFormatting                                     { get; set; }

        #endregion

        #region Events

        #region (protected internal) OnPushEVSEDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushEVSEDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushEVSEDataHTTPRequest(DateTime     Timestamp,
                                                           HTTPAPI      API,
                                                           HTTPRequest  Request)

            => OnPushEVSEDataHTTPRequest.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (event)              OnPushEVSEData(Request-/Response)

        /// <summary>
        /// An event send whenever a PushEVSEData request was received.
        /// </summary>
        public event OnPushEVSEDataAPIRequestDelegate?   OnPushEVSEDataRequest;

        /// <summary>
        /// An event send whenever a PushEVSEData request was received.
        /// </summary>
        public event OnPushEVSEDataAPIDelegate?          OnPushEVSEData;

        /// <summary>
        /// An event send whenever a response to a PushEVSEData request was sent.
        /// </summary>
        public event OnPushEVSEDataAPIResponseDelegate?  OnPushEVSEDataResponse;

        #endregion

        #region (protected internal) OnPushEVSEDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushEVSEDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushEVSEDataHTTPResponse(DateTime      Timestamp,
                                                            HTTPAPI       API,
                                                            HTTPRequest   Request,
                                                            HTTPResponse  Response)

            => OnPushEVSEDataHTTPResponse.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) OnPushEVSEStatusHTTPRequest

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushEVSEStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushEVSEStatusHTTPRequest(DateTime     Timestamp,
                                                             HTTPAPI      API,
                                                             HTTPRequest  Request)

            => OnPushEVSEStatusHTTPRequest.WhenAll(Timestamp,
                                                   API ?? this,
                                                   Request);

        #endregion

        #region (event)              OnPushEVSEStatus(Request-/Response)

        /// <summary>
        /// An event send whenever a PushEVSEStatus request was received.
        /// </summary>
        public event OnPushEVSEStatusAPIRequestDelegate?   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event send whenever a PushEVSEStatus request was received.
        /// </summary>
        public event OnPushEVSEStatusAPIDelegate?          OnPushEVSEStatus;

        /// <summary>
        /// An event send whenever a response to a PushEVSEStatus request was sent.
        /// </summary>
        public event OnPushEVSEStatusAPIResponseDelegate?  OnPushEVSEStatusResponse;

        #endregion

        #region (protected internal) OnPushEVSEStatusHTTPResponse

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushEVSEStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushEVSEStatusHTTPResponse(DateTime      Timestamp,
                                                              HTTPAPI       API,
                                                              HTTPRequest   Request,
                                                              HTTPResponse  Response)

            => OnPushEVSEStatusHTTPResponse.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request,
                                                    Response);

        #endregion



        #region (protected internal) OnAuthorizeStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStartHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStartHTTPRequest(DateTime     Timestamp,
                                                             HTTPAPI      API,
                                                             HTTPRequest  Request)

            => OnAuthorizeStartHTTPRequest.WhenAll(Timestamp,
                                                   API ?? this,
                                                   Request);

        #endregion

        #region (event)              OnAuthorizeStart(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartAPIRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event send whenever a AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartAPIDelegate?          OnAuthorizeStart;

        /// <summary>
        /// An event send whenever a response to a AuthorizeStart request was sent.
        /// </summary>
        public event OnAuthorizeStartAPIResponseDelegate?  OnAuthorizeStartResponse;

        #endregion

        #region (protected internal) OnAuthorizeStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeStartHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeStartHTTPResponse(DateTime      Timestamp,
                                                              HTTPAPI       API,
                                                              HTTPRequest   Request,
                                                              HTTPResponse  Response)

            => OnAuthorizeStartHTTPResponse.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request,
                                                    Response);

        #endregion


        #region (protected internal) OnAuthorizeStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStopHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStopHTTPRequest(DateTime     Timestamp,
                                                            HTTPAPI      API,
                                                            HTTPRequest  Request)

            => OnAuthorizeStopHTTPRequest.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request);

        #endregion

        #region (event)              OnAuthorizeStop(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopAPIRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event send whenever a AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopAPIDelegate?          OnAuthorizeStop;

        /// <summary>
        /// An event send whenever a response to a AuthorizeStop request was sent.
        /// </summary>
        public event OnAuthorizeStopAPIResponseDelegate?  OnAuthorizeStopResponse;

        #endregion

        #region (protected internal) OnAuthorizeStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeStopHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeStopHTTPResponse(DateTime      Timestamp,
                                                             HTTPAPI       API,
                                                             HTTPRequest   Request,
                                                             HTTPResponse  Response)

            => OnAuthorizeStopHTTPResponse.WhenAll(Timestamp,
                                                   API ?? this,
                                                   Request,
                                                   Response);

        #endregion



        #region (protected internal) OnChargingNotificationHTTPRequest

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargingNotificationHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargingNotificationHTTPRequest(DateTime     Timestamp,
                                                                   HTTPAPI      API,
                                                                   HTTPRequest  Request)

            => OnChargingNotificationHTTPRequest.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request);

        #endregion

        #region (event)              OnChargingStartNotification(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingStartNotification request was received.
        /// </summary>
        public event OnChargingStartNotificationAPIRequestDelegate?   OnChargingStartNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingStartNotification request was received.
        /// </summary>
        public event OnChargingStartNotificationAPIDelegate?          OnChargingStartNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingStartNotification request was sent.
        /// </summary>
        public event OnChargingStartNotificationAPIResponseDelegate?  OnChargingStartNotificationResponse;

        #endregion

        #region (event)              OnChargingProgressNotification(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingProgressNotification request was received.
        /// </summary>
        public event OnChargingProgressNotificationAPIRequestDelegate?   OnChargingProgressNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingProgressNotification request was received.
        /// </summary>
        public event OnChargingProgressNotificationAPIDelegate?          OnChargingProgressNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingProgressNotification request was sent.
        /// </summary>
        public event OnChargingProgressNotificationAPIResponseDelegate?  OnChargingProgressNotificationResponse;

        #endregion

        #region (event)              OnChargingEndNotification(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingEndNotification request was received.
        /// </summary>
        public event OnChargingEndNotificationAPIRequestDelegate?   OnChargingEndNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingEndNotification request was received.
        /// </summary>
        public event OnChargingEndNotificationAPIDelegate?          OnChargingEndNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingEndNotification request was sent.
        /// </summary>
        public event OnChargingEndNotificationAPIResponseDelegate?  OnChargingEndNotificationResponse;

        #endregion

        #region (event)              OnChargingErrorNotification(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingErrorNotification request was received.
        /// </summary>
        public event OnChargingErrorNotificationAPIRequestDelegate?   OnChargingErrorNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingErrorNotification request was received.
        /// </summary>
        public event OnChargingErrorNotificationAPIDelegate?          OnChargingErrorNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingErrorNotification request was sent.
        /// </summary>
        public event OnChargingErrorNotificationAPIResponseDelegate?  OnChargingErrorNotificationResponse;

        #endregion

        #region (protected internal) OnChargingNotificationHTTPResponse

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargingNotificationHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargingNotificationHTTPResponse(DateTime      Timestamp,
                                                                    HTTPAPI       API,
                                                                    HTTPRequest   Request,
                                                                    HTTPResponse  Response)

            => OnChargingNotificationHTTPResponse.WhenAll(Timestamp,
                                                          API ?? this,
                                                          Request,
                                                          Response);

        #endregion



        #region (protected internal) OnChargeDetailRecordHTTPRequest

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargeDetailRecordHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargeDetailRecordHTTPRequest(DateTime     Timestamp,
                                                                 HTTPAPI      API,
                                                                 HTTPRequest  Request)

            => OnChargeDetailRecordHTTPRequest.WhenAll(Timestamp,
                                                       API ?? this,
                                                       Request);

        #endregion

        #region (event)              OnChargeDetailRecord(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargeDetailRecord request was received.
        /// </summary>
        public event OnChargeDetailRecordAPIRequestDelegate?   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event send whenever a ChargeDetailRecord request was received.
        /// </summary>
        public event OnChargeDetailRecordAPIDelegate?          OnChargeDetailRecord;

        /// <summary>
        /// An event send whenever a response to a ChargeDetailRecord request was sent.
        /// </summary>
        public event OnChargeDetailRecordAPIResponseDelegate?  OnChargeDetailRecordResponse;

        #endregion

        #region (protected internal) OnChargeDetailRecordHTTPResponse

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargeDetailRecordHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargeDetailRecordHTTPResponse(DateTime      Timestamp,
                                                                  HTTPAPI       API,
                                                                  HTTPRequest   Request,
                                                                  HTTPResponse  Response)

            => OnChargeDetailRecordHTTPResponse.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request,
                                                        Response);

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO HTTP Client API.
        /// </summary>
        /// <param name="HTTPHostname">The HTTP hostname for all URLs within this API.</param>
        /// <param name="ExternalDNSName">The offical URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP servername, used whenever no HTTP Host-header has been given.</param>
        /// 
        /// <param name="URLPathPrefix">A common prefix for all URLs.</param>
        /// <param name="HTTPServiceName">The name of the HTTP service.</param>
        /// <param name="APIVersionHashes">The API version hashes (git commit hash values).</param>
        /// 
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionThreadsNameBuilder">An optional delegate to set the name of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsPriorityBuilder">An optional delegate to set the priority of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsAreBackground">Whether the TCP connection threads are background threads or not (default: yes).</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance intervall.</param>
        /// 
        /// <param name="DisableWardenTasks">Disable all warden tasks.</param>
        /// <param name="WardenInitialDelay">The initial delay of the warden tasks.</param>
        /// <param name="WardenCheckEvery">The warden intervall.</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="DisableLogging">Disable the log file.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LoggingContext">The context of all logfiles.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        /// <param name="DNSClient">The DNS client of the API.</param>
        /// <param name="Autostart">Whether to start the API automatically.</param>
        public CPOClientAPI(HTTPHostname?                         HTTPHostname                       = null,
                            String?                               ExternalDNSName                    = null,
                            IPPort?                               HTTPServerPort                     = null,
                            HTTPPath?                             BasePath                           = null,
                            String                                HTTPServerName                     = DefaultHTTPServerName,

                            HTTPPath?                             URLPathPrefix                      = null,
                            String                                HTTPServiceName                    = DefaultHTTPServiceName,
                            JObject?                              APIVersionHashes                   = null,

                            ServerCertificateSelectorDelegate?    ServerCertificateSelector          = null,
                            LocalCertificateSelectionCallback?    ClientCertificateSelector          = null,
                            RemoteCertificateValidationCallback?  ClientCertificateValidator         = null,
                            SslProtocols                          AllowedTLSProtocols                = SslProtocols.Tls12 | SslProtocols.Tls13,

                            String?                               ServerThreadName                   = null,
                            ThreadPriority?                       ServerThreadPriority               = null,
                            Boolean?                              ServerThreadIsBackground           = null,
                            ConnectionIdBuilder?                  ConnectionIdBuilder                = null,
                            ConnectionThreadsNameBuilder?         ConnectionThreadsNameBuilder       = null,
                            ConnectionThreadsPriorityBuilder?     ConnectionThreadsPriorityBuilder   = null,
                            Boolean?                              ConnectionThreadsAreBackground     = null,
                            TimeSpan?                             ConnectionTimeout                  = null,
                            UInt32?                               MaxClientConnections               = null,

                            Boolean?                              DisableMaintenanceTasks            = false,
                            TimeSpan?                             MaintenanceInitialDelay            = null,
                            TimeSpan?                             MaintenanceEvery                   = null,

                            Boolean?                              DisableWardenTasks                 = false,
                            TimeSpan?                             WardenInitialDelay                 = null,
                            TimeSpan?                             WardenCheckEvery                   = null,

                            Boolean?                              IsDevelopment                      = null,
                            IEnumerable<String>?                  DevelopmentServers                 = null,
                            Boolean                               DisableLogging                     = false,
                            String                                LoggingPath                        = DefaultHTTPAPI_LoggingPath,
                            String                                LoggingContext                     = DefaultLoggingContext,
                            String                                LogfileName                        = DefaultHTTPAPI_LogfileName,
                            LogfileCreatorDelegate?               LogfileCreator                     = null,
                            DNSClient?                            DNSClient                          = null,
                            Boolean                               Autostart                          = false)

            : base(HTTPHostname,
                   ExternalDNSName,
                   HTTPServerPort,
                   BasePath,
                   HTTPServerName,

                   URLPathPrefix,
                   HTTPServiceName,
                   null, //HTMLTemplate,
                   APIVersionHashes,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,

                   ServerThreadName,
                   ServerThreadPriority,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionThreadsNameBuilder,
                   ConnectionThreadsPriorityBuilder,
                   ConnectionThreadsAreBackground,
                   ConnectionTimeout,
                   MaxClientConnections,

                   DisableMaintenanceTasks,
                   MaintenanceInitialDelay,
                   MaintenanceEvery,

                   DisableWardenTasks,
                   WardenInitialDelay,
                   WardenCheckEvery,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LogfileName,
                   LogfileCreator,
                   DNSClient,
                   false) //Autostart)

        {

            RegisterURLTemplates();

            this.Counters    = new APICounters();

            this.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingPath,
                                                LoggingContext ?? DefaultLoggingContext,
                                                LogfileCreator)
                                   : null;

            if (Autostart)
                Start();

        }

        #endregion


        #region (private) RegisterURLTemplates()

        private void RegisterURLTemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         new HTTPPath[] {
                                             URLPathPrefix + "/",
                                             URLPathPrefix + "/{FileName}"
                                         },
                                         HTTPDelegate: Request => {
                                             return Task.FromResult(
                                                 new HTTPResponse.Builder(Request) {
                                                     HTTPStatusCode  = HTTPStatusCode.OK,
                                                     Server          = HTTPServer.DefaultServerName,
                                                     Date            = Timestamp.Now,
                                                     ContentType     = HTTPContentType.TEXT_UTF8,
                                                     Content         = "This is an OICP v2.3 HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                     CacheControl    = "public, max-age=300",
                                                     Connection      = "close"
                                                 }.AsImmutable);
                                         });

            #endregion


            //Note: OperatorId is the remote CPO sending an authorize start/stop request!

            #region POST  ~/api/oicp/evsepull/v23/operators/{operatorId}/data-records

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepush/v23/operators/DE-GEF/data-records
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepush/v23/operators/{operatorId}/data-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushEVSEDataHTTPRequest,
                                         HTTPResponseLogger:  logPushEVSEDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             OICPResult<Acknowledgement<PushEVSEDataRequest>>? pullEVSEDataResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                                     pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushEVSEDataRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.SystemError,
                                                                                                    "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                                )
                                                                                )
                                                                            );

                                                 #endregion

                                                 else if (PushEVSEDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                       //     operatorId ????
                                                                                       Request.Timeout ?? DefaultRequestTimeout,
                                                                                       out PushEVSEDataRequest?          pullEVSEDataRequest,
                                                                                       out String?                       errorResponse,
                                                                                       Timestamp:                        Request.Timestamp,
                                                                                       EventTrackingId:                  Request.EventTrackingId,
                                                                                       CustomPushEVSEDataRequestParser:  CustomPushEVSEDataRequestParser))
                                                 {

                                                     Counters.PushEVSEData.IncRequests_OK();

                                                     #region Send OnPushEVSEDataRequest event

                                                     try
                                                     {

                                                         if (OnPushEVSEDataRequest is not null)
                                                             await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                                                                Cast<OnPushEVSEDataAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEDataRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPushEVSEDataLocal = OnPushEVSEData;
                                                     if (OnPushEVSEDataLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEDataResponse = (await Task.WhenAll(OnPushEVSEDataLocal.GetInvocationList().
                                                                                                                                Cast<OnPushEVSEDataAPIDelegate>().
                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                              this,
                                                                                                                                              pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.PushEVSEData.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                                        this,
                                                                                        new Acknowledgement<PushEVSEDataRequest>(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            e.Message,
                                                                                                            e.StackTrace
                                                                                                        ),
                                                                                            pullEVSEDataRequest!
                                                                                        )
                                                                                    );
                                                         }
                                                     }

                                                     if (pullEVSEDataResponse is null)
                                                         pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                                    this,
                                                                                    new Acknowledgement<PushEVSEDataRequest>(
                                                                                        Timestamp.Now,
                                                                                        Request.EventTrackingId,
                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                        StatusCode: new StatusCode(
                                                                                                        StatusCodes.SystemError,
                                                                                                        "Could not process the received PushEVSEData request!"
                                                                                                    ),
                                                                                        pullEVSEDataRequest!
                                                                                    )
                                                                                );

                                                     #endregion

                                                     #region Send OnPushEVSEDataResponse event

                                                     try
                                                     {

                                                         if (OnPushEVSEDataResponse is not null)
                                                             await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                                                                Cast<OnPushEVSEDataAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEDataResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushEVSEDataRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    "We could not parse the given PushEVSEData request!",
                                                                                                    errorResponse
                                                                                                ),
                                                                                    pullEVSEDataRequest!
                                                                                )
                                                                            );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                            this,
                                                                            new Acknowledgement<PushEVSEDataRequest>(
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                StatusCode: new StatusCode(
                                                                                                StatusCodes.SystemError,
                                                                                                e.Message,
                                                                                                e.StackTrace
                                                                                            )
                                                                            )
                                                                        );
                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = Timestamp.Now,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pullEVSEDataResponse.Response.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                          CustomStatusCodeSerializer).
                                                                                                                   ToString(JSONFormatting).
                                                                                                                   ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/operators/{operatorId}/status-records

            // -------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepush/v21/operators/DE-GEF/status-records
            // -------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepush/v21/operators/{operatorId}/status-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushEVSEStatusHTTPRequest,
                                         HTTPResponseLogger:  logPushEVSEStatusHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             OICPResult<Acknowledgement<PushEVSEStatusRequest>>? pullEVSEStatusResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                                     pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushEVSEStatusRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.SystemError,
                                                                                                    "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                                )
                                                                                )
                                                                            );

                                                 #endregion

                                                 else if (PushEVSEStatusRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                       //     operatorId ????
                                                                                       Request.Timeout ?? DefaultRequestTimeout,
                                                                                       out PushEVSEStatusRequest?          pullEVSEStatusRequest,
                                                                                       out String?                         errorResponse,
                                                                                       Timestamp:                          Request.Timestamp,
                                                                                       EventTrackingId:                    Request.EventTrackingId,
                                                                                       CustomPushEVSEStatusRequestParser:  CustomPushEVSEStatusRequestParser))
                                                 {

                                                     Counters.PushEVSEStatus.IncRequests_OK();

                                                     #region Send OnPushEVSEStatusRequest event

                                                     try
                                                     {

                                                         if (OnPushEVSEStatusRequest is not null)
                                                             await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                                                                Cast<OnPushEVSEStatusAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEStatusRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPushEVSEStatusLocal = OnPushEVSEStatus;
                                                     if (OnPushEVSEStatusLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEStatusResponse = (await Task.WhenAll(OnPushEVSEStatusLocal.GetInvocationList().
                                                                                                                                Cast<OnPushEVSEStatusAPIDelegate>().
                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                              this,
                                                                                                                                              pullEVSEStatusRequest!))))?.FirstOrDefault();

                                                             Counters.PushEVSEStatus.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                                        this,
                                                                                        new Acknowledgement<PushEVSEStatusRequest>(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            e.Message,
                                                                                                            e.StackTrace
                                                                                                        ),
                                                                                            pullEVSEStatusRequest!
                                                                                        )
                                                                                    );
                                                         }
                                                     }

                                                     if (pullEVSEStatusResponse is null)
                                                         pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                                    this,
                                                                                    new Acknowledgement<PushEVSEStatusRequest>(
                                                                                        Timestamp.Now,
                                                                                        Request.EventTrackingId,
                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                        StatusCode: new StatusCode(
                                                                                                        StatusCodes.SystemError,
                                                                                                        "Could not process the received PushEVSEStatus request!"
                                                                                                    ),
                                                                                        pullEVSEStatusRequest!
                                                                                    )
                                                                                );

                                                     #endregion

                                                     #region Send OnPushEVSEStatusResponse event

                                                     try
                                                     {

                                                         if (OnPushEVSEStatusResponse is not null)
                                                             await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                                                                Cast<OnPushEVSEStatusAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEStatusResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushEVSEStatusRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    "We could not parse the given PushEVSEStatus request!",
                                                                                                    errorResponse
                                                                                                ),
                                                                                    pullEVSEStatusRequest!
                                                                                )
                                                                            );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                            this,
                                                                            new Acknowledgement<PushEVSEStatusRequest>(
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                StatusCode: new StatusCode(
                                                                                                StatusCodes.SystemError,
                                                                                                e.Message,
                                                                                                e.StackTrace
                                                                                            )
                                                                            )
                                                                        );
                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = Timestamp.Now,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pullEVSEStatusResponse.Response.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                            CustomStatusCodeSerializer).
                                                                                                                     ToString(JSONFormatting).
                                                                                                                     ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/start

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GEF/authorize/start
            // --------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             OICPResult<AuthorizationStartResponse>? authorizationStartResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                                     authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                      this,
                                                                                      AuthorizationStartResponse.SystemError(
                                                                                          null,
                                                                                          "The expected 'operatorId' URL parameter could not be parsed!",
                                                                                          ResponseTimestamp:  Timestamp.Now,
                                                                                          EventTrackingId:    Request.EventTrackingId,
                                                                                          Runtime:            Timestamp.Now - Request.Timestamp
                                                                                      )
                                                                                  );

                                                 #endregion

                                                 else if (AuthorizeStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                         operatorId,
                                                                                         Request.Timeout ?? DefaultRequestTimeout,
                                                                                         out AuthorizeStartRequest?          pullEVSEDataRequest,
                                                                                         out String?                         errorResponse,
                                                                                         Timestamp:                          Request.Timestamp,
                                                                                         EventTrackingId:                    Request.EventTrackingId,
                                                                                         CustomAuthorizeStartRequestParser:  CustomAuthorizeStartRequestParser))
                                                 {

                                                     Counters.AuthorizeStart.IncRequests_OK();

                                                     #region Send OnAuthorizeStartRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeStartRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeStartAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStartRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeStartLocal = OnAuthorizeStart;
                                                     if (OnAuthorizeStartLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             authorizationStartResponse = (await Task.WhenAll(OnAuthorizeStartLocal.GetInvocationList().
                                                                                                                                    Cast<OnAuthorizeStartAPIDelegate>().
                                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                                  this,
                                                                                                                                                  pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeStart.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                              this,
                                                                                              AuthorizationStartResponse.DataError(
                                                                                                  pullEVSEDataRequest,
                                                                                                  e.Message,
                                                                                                  e.StackTrace,
                                                                                                  ResponseTimestamp:  Timestamp.Now,
                                                                                                  EventTrackingId:    Request.EventTrackingId,
                                                                                                  Runtime:            Timestamp.Now - Request.Timestamp
                                                                                              )
                                                                                          );
                                                         }
                                                     }

                                                     if (authorizationStartResponse is null)
                                                         authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                          this,
                                                                                          AuthorizationStartResponse.SystemError(
                                                                                              pullEVSEDataRequest,
                                                                                              "Could not process the received AuthorizeStart request!",
                                                                                              ResponseTimestamp:  Timestamp.Now,
                                                                                              EventTrackingId:    Request.EventTrackingId,
                                                                                              Runtime:            Timestamp.Now - Request.Timestamp
                                                                                          )
                                                                                      );

                                                     #endregion

                                                     #region Send OnAuthorizeStartResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeStartResponse is not null)
                                                             await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeStartAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizationStartResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStartResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                      this,
                                                                                      AuthorizationStartResponse.DataError(
                                                                                          pullEVSEDataRequest,
                                                                                          "We could not parse the given AuthorizeStart request!",
                                                                                          errorResponse,
                                                                                          ResponseTimestamp:  Timestamp.Now,
                                                                                          EventTrackingId:    Request.EventTrackingId,
                                                                                          Runtime:            Timestamp.Now - Request.Timestamp
                                                                                      )
                                                                                  );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                  this,
                                                                                  AuthorizationStartResponse.SystemError(
                                                                                      null,
                                                                                      e.Message,
                                                                                      e.StackTrace,
                                                                                      ResponseTimestamp:  Timestamp.Now,
                                                                                      EventTrackingId:    Request.EventTrackingId,
                                                                                      Runtime:            Timestamp.Now - Request.Timestamp
                                                                                  )
                                                                              );
                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = Timestamp.Now,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = authorizationStartResponse.Response.ToJSON(CustomAuthorizationStartSerializer,
                                                                                                                                CustomStatusCodeSerializer,
                                                                                                                                CustomIdentificationSerializer).
                                                                                                                         ToString(JSONFormatting).
                                                                                                                         ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/stop

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GEF/authorize/stop
            // --------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var stopTime = Timestamp.Now;
                                             OICPResult<AuthorizationStopResponse>? authorizationStopResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                                     authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                     this,
                                                                                     AuthorizationStopResponse.SystemError(
                                                                                         null,
                                                                                         "The expected 'operatorId' URL parameter could not be parsed!",
                                                                                         ResponseTimestamp:  Timestamp.Now,
                                                                                         EventTrackingId:    Request.EventTrackingId,
                                                                                         Runtime:            Timestamp.Now - Request.Timestamp
                                                                                     )
                                                                                 );

                                                 #endregion

                                                 else if (AuthorizeStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                        operatorId,
                                                                                        Request.Timeout ?? DefaultRequestTimeout,
                                                                                        out AuthorizeStopRequest?          pullEVSEDataRequest,
                                                                                        out String?                        errorResponse,
                                                                                        Timestamp:                         Request.Timestamp,
                                                                                        EventTrackingId:                   Request.EventTrackingId,
                                                                                        CustomAuthorizeStopRequestParser:  CustomAuthorizeStopRequestParser))
                                                 {

                                                     Counters.AuthorizeStop.IncRequests_OK();

                                                     #region Send OnAuthorizeStopRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeStopRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeStopAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStopRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeStopLocal = OnAuthorizeStop;
                                                     if (OnAuthorizeStopLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             authorizationStopResponse = (await Task.WhenAll(OnAuthorizeStopLocal.GetInvocationList().
                                                                                                                                  Cast<OnAuthorizeStopAPIDelegate>().
                                                                                                                                  Select(e => e(Timestamp.Now,
                                                                                                                                                this,
                                                                                                                                                pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeStop.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                             this,
                                                                                             AuthorizationStopResponse.DataError(
                                                                                                 pullEVSEDataRequest,
                                                                                                 e.Message,
                                                                                                 e.StackTrace,
                                                                                                 ResponseTimestamp:  Timestamp.Now,
                                                                                                 EventTrackingId:    Request.EventTrackingId,
                                                                                                 Runtime:            Timestamp.Now - Request.Timestamp
                                                                                             )
                                                                                         );
                                                         }
                                                     }

                                                     if (authorizationStopResponse is null)
                                                         authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                         this,
                                                                                         AuthorizationStopResponse.SystemError(
                                                                                             pullEVSEDataRequest,
                                                                                             "Could not process the received AuthorizeStop request!",
                                                                                             ResponseTimestamp:  Timestamp.Now,
                                                                                             EventTrackingId:    Request.EventTrackingId,
                                                                                             Runtime:            Timestamp.Now - Request.Timestamp
                                                                                         )
                                                                                     );

                                                     #endregion

                                                     #region Send OnAuthorizeStopResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeStopResponse is not null)
                                                             await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeStopAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizationStopResponse,
                                                                                              Timestamp.Now - stopTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStopResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                     this,
                                                                                     AuthorizationStopResponse.DataError(
                                                                                         pullEVSEDataRequest,
                                                                                         "We could not parse the given AuthorizeStop request!",
                                                                                         errorResponse,
                                                                                         ResponseTimestamp:  Timestamp.Now,
                                                                                         EventTrackingId:    Request.EventTrackingId,
                                                                                         Runtime:            Timestamp.Now - Request.Timestamp
                                                                                     )
                                                                                 );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                 this,
                                                                                 AuthorizationStopResponse.SystemError(
                                                                                     null,
                                                                                     e.Message,
                                                                                     e.StackTrace,
                                                                                     ResponseTimestamp:  Timestamp.Now,
                                                                                     EventTrackingId:    Request.EventTrackingId,
                                                                                     Runtime:            Timestamp.Now - Request.Timestamp
                                                                                 )
                                                                             );
                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = Timestamp.Now,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = authorizationStopResponse.Response.ToJSON(CustomAuthorizationStopSerializer,
                                                                                                                               CustomStatusCodeSerializer).
                                                                                                                        ToString(JSONFormatting).
                                                                                                                        ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/notificationmgmt/v11/charging-notifications

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/notificationmgmt/v11/charging-notifications
            // ------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logChargingNotificationHTTPRequest,
                                         HTTPResponseLogger:  logChargingNotificationHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             OICPResult<Acknowledgement>? acknowledgement = null;

                                             try
                                             {

                                                 if (Request.TryParseJObjectRequestBody(out JObject JSONRequest, out _) &&
                                                     JSONRequest.ParseMandatory("Type",
                                                                                "charging notification type",
                                                                                ChargingNotificationTypesExtensions.TryParse,
                                                                                out ChargingNotificationTypes chargingNotificationType,
                                                                                out _))
                                                 {

                                                     switch (chargingNotificationType)
                                                     {

                                                         #region Start

                                                         case ChargingNotificationTypes.Start:

                                                             if (ChargingStartNotificationRequest.TryParse(JSONRequest,
                                                                                                           Request.Timeout ?? DefaultRequestTimeout,
                                                                                                           out ChargingStartNotificationRequest?  chargingStartNotificationRequest,
                                                                                                           out String?                            errorResponse,
                                                                                                           Request.Timestamp,
                                                                                                           Request.EventTrackingId,
                                                                                                           CustomChargingStartNotificationRequestParser))
                                                             {

                                                                 Counters.ChargingStartNotification.IncRequests_OK();

                                                                 #region Send OnChargingStartNotificationRequest event

                                                                 try
                                                                 {

                                                                     if (OnChargingStartNotificationRequest is not null)
                                                                         await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                                                                            Cast<OnChargingStartNotificationAPIRequestDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          chargingStartNotificationRequest))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingStartNotificationRequest));
                                                                 }

                                                                 #endregion

                                                                 #region Call async subscribers

                                                                 OICPResult<Acknowledgement<ChargingStartNotificationRequest>>? startAcknowledgement = null;

                                                                 var OnChargingStartNotificationLocal = OnChargingStartNotification;
                                                                 if (OnChargingStartNotificationLocal is not null)
                                                                 {
                                                                     try
                                                                     {

                                                                         startAcknowledgement = (await Task.WhenAll(OnChargingStartNotificationLocal.GetInvocationList().
                                                                                                                    Cast<OnChargingStartNotificationAPIDelegate>().
                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                  this,
                                                                                                                                  chargingStartNotificationRequest))).
                                                                                                                    ConfigureAwait(false))?.FirstOrDefault();

                                                                         Counters.ChargingStartNotification.IncResponses_OK();

                                                                     }
                                                                     catch (Exception e)
                                                                     {
                                                                         startAcknowledgement = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                                                                    this,
                                                                                                    new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                                        Timestamp.Now,
                                                                                                        Request.EventTrackingId,
                                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                                        StatusCode: new StatusCode(
                                                                                                                        StatusCodes.DataError,
                                                                                                                        e.Message,
                                                                                                                        e.StackTrace
                                                                                                                    ),
                                                                                                        chargingStartNotificationRequest
                                                                                                    )
                                                                                                );
                                                                     }
                                                                 }

                                                                 if (startAcknowledgement is null)
                                                                     startAcknowledgement = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                                                                this,
                                                                                                new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.SystemError,
                                                                                                                    "Could not process the received ChargingStartNotification request!"
                                                                                                                ),
                                                                                                    chargingStartNotificationRequest
                                                                                                )
                                                                                            );

                                                                 #endregion

                                                                 #region Send OnChargingStartNotificationResponse event

                                                                 try
                                                                 {

                                                                     if (OnChargingStartNotificationResponse is not null)
                                                                         await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                                                                            Cast<OnChargingStartNotificationAPIResponseDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          startAcknowledgement,
                                                                                                          Timestamp.Now - startTime))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingStartNotificationResponse));
                                                                 }

                                                                 #endregion

                                                                 acknowledgement = OICPResult<Acknowledgement>.From(startAcknowledgement);

                                                             }
                                                             else
                                                                 acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                                      this,
                                                                                      new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.DataError,
                                                                                                          "We could not parse the given ChargingStartNotification request!",
                                                                                                          errorResponse
                                                                                                      ),
                                                                                          chargingStartNotificationRequest
                                                                                      )
                                                                                  );

                                                             break;

                                                         #endregion

                                                         #region Progress

                                                         case ChargingNotificationTypes.Progress:

                                                             if (ChargingProgressNotificationRequest.TryParse(JSONRequest,
                                                                                                              Request.Timeout ?? DefaultRequestTimeout,
                                                                                                              out ChargingProgressNotificationRequest?  chargingProgressNotificationRequest,
                                                                                                              out                                       errorResponse,
                                                                                                              Request.Timestamp,
                                                                                                              Request.EventTrackingId,
                                                                                                              CustomChargingProgressNotificationRequestParser))
                                                             {

                                                                 Counters.ChargingProgressNotification.IncRequests_OK();

                                                                 #region Send OnChargingProgressNotificationRequest event

                                                                 try
                                                                 {

                                                                     if (OnChargingProgressNotificationRequest is not null)
                                                                         await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                                                                            Cast<OnChargingProgressNotificationAPIRequestDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          chargingProgressNotificationRequest))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingProgressNotificationRequest));
                                                                 }

                                                                 #endregion

                                                                 #region Call async subscribers

                                                                 OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>? startAcknowledgement = null;

                                                                 var OnChargingProgressNotificationLocal = OnChargingProgressNotification;
                                                                 if (OnChargingProgressNotificationLocal is not null)
                                                                 {
                                                                     try
                                                                     {

                                                                         startAcknowledgement = (await Task.WhenAll(OnChargingProgressNotificationLocal.GetInvocationList().
                                                                                                                    Cast<OnChargingProgressNotificationAPIDelegate>().
                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                  this,
                                                                                                                                  chargingProgressNotificationRequest))).
                                                                                                                    ConfigureAwait(false))?.FirstOrDefault();

                                                                         Counters.ChargingProgressNotification.IncResponses_OK();

                                                                     }
                                                                     catch (Exception e)
                                                                     {
                                                                         startAcknowledgement = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                                                                    this,
                                                                                                    new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                                        Timestamp.Now,
                                                                                                        Request.EventTrackingId,
                                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                                        StatusCode: new StatusCode(
                                                                                                                        StatusCodes.DataError,
                                                                                                                        e.Message,
                                                                                                                        e.StackTrace
                                                                                                                    ),
                                                                                                        chargingProgressNotificationRequest
                                                                                                    )
                                                                                                );
                                                                     }
                                                                 }

                                                                 if (startAcknowledgement is null)
                                                                     startAcknowledgement = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                                                                this,
                                                                                                new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.SystemError,
                                                                                                                    "Could not process the received ChargingProgressNotification request!"
                                                                                                                ),
                                                                                                    chargingProgressNotificationRequest
                                                                                                )
                                                                                            );

                                                                 #endregion

                                                                 #region Send OnChargingProgressNotificationResponse event

                                                                 try
                                                                 {

                                                                     if (OnChargingProgressNotificationResponse is not null)
                                                                         await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                                                                            Cast<OnChargingProgressNotificationAPIResponseDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          startAcknowledgement,
                                                                                                          Timestamp.Now - startTime))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingProgressNotificationResponse));
                                                                 }

                                                                 #endregion

                                                                 acknowledgement = OICPResult<Acknowledgement>.From(startAcknowledgement);

                                                             }
                                                             else
                                                                 acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                                      this,
                                                                                      new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.DataError,
                                                                                                          "We could not parse the given ChargingProgressNotification request!",
                                                                                                          errorResponse
                                                                                                      ),
                                                                                          chargingProgressNotificationRequest
                                                                                      )
                                                                                  );

                                                             break;

                                                         #endregion

                                                         #region End

                                                         case ChargingNotificationTypes.End:

                                                             if (ChargingEndNotificationRequest.TryParse(JSONRequest,
                                                                                                              Request.Timeout ?? DefaultRequestTimeout,
                                                                                                              out ChargingEndNotificationRequest?  chargingEndNotificationRequest,
                                                                                                              out                                       errorResponse,
                                                                                                              Request.Timestamp,
                                                                                                              Request.EventTrackingId,
                                                                                                              CustomChargingEndNotificationRequestParser))
                                                             {

                                                                 Counters.ChargingEndNotification.IncRequests_OK();

                                                                 #region Send OnChargingEndNotificationRequest event

                                                                 try
                                                                 {

                                                                     if (OnChargingEndNotificationRequest is not null)
                                                                         await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                                                                            Cast<OnChargingEndNotificationAPIRequestDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          chargingEndNotificationRequest))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingEndNotificationRequest));
                                                                 }

                                                                 #endregion

                                                                 #region Call async subscribers

                                                                 OICPResult<Acknowledgement<ChargingEndNotificationRequest>>? startAcknowledgement = null;

                                                                 var OnChargingEndNotificationLocal = OnChargingEndNotification;
                                                                 if (OnChargingEndNotificationLocal is not null)
                                                                 {
                                                                     try
                                                                     {

                                                                         startAcknowledgement = (await Task.WhenAll(OnChargingEndNotificationLocal.GetInvocationList().
                                                                                                                    Cast<OnChargingEndNotificationAPIDelegate>().
                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                  this,
                                                                                                                                  chargingEndNotificationRequest))).
                                                                                                                    ConfigureAwait(false))?.FirstOrDefault();

                                                                         Counters.ChargingEndNotification.IncResponses_OK();

                                                                     }
                                                                     catch (Exception e)
                                                                     {
                                                                         startAcknowledgement = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                                                                    this,
                                                                                                    new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                                        Timestamp.Now,
                                                                                                        Request.EventTrackingId,
                                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                                        StatusCode: new StatusCode(
                                                                                                                        StatusCodes.DataError,
                                                                                                                        e.Message,
                                                                                                                        e.StackTrace
                                                                                                                    ),
                                                                                                        chargingEndNotificationRequest
                                                                                                    )
                                                                                                );
                                                                     }
                                                                 }

                                                                 if (startAcknowledgement is null)
                                                                     startAcknowledgement = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                                                                this,
                                                                                                new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.SystemError,
                                                                                                                    "Could not process the received ChargingEndNotification request!"
                                                                                                                ),
                                                                                                    chargingEndNotificationRequest
                                                                                                )
                                                                                            );

                                                                 #endregion

                                                                 #region Send OnChargingEndNotificationResponse event

                                                                 try
                                                                 {

                                                                     if (OnChargingEndNotificationResponse is not null)
                                                                         await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                                                                            Cast<OnChargingEndNotificationAPIResponseDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          startAcknowledgement,
                                                                                                          Timestamp.Now - startTime))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingEndNotificationResponse));
                                                                 }

                                                                 #endregion

                                                                 acknowledgement = OICPResult<Acknowledgement>.From(startAcknowledgement);

                                                             }
                                                             else
                                                                 acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                                      this,
                                                                                      new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.DataError,
                                                                                                          "We could not parse the given ChargingEndNotification request!",
                                                                                                          errorResponse
                                                                                                      ),
                                                                                          chargingEndNotificationRequest
                                                                                      )
                                                                                  );

                                                             break;

                                                         #endregion

                                                         #region Error

                                                         case ChargingNotificationTypes.Error:

                                                             if (ChargingErrorNotificationRequest.TryParse(JSONRequest,
                                                                                                           Request.Timeout ?? DefaultRequestTimeout,
                                                                                                           out ChargingErrorNotificationRequest?  chargingErrorNotificationRequest,
                                                                                                           out                                    errorResponse,
                                                                                                           Request.Timestamp,
                                                                                                           Request.EventTrackingId,
                                                                                                           CustomChargingErrorNotificationRequestParser))
                                                             {

                                                                 Counters.ChargingErrorNotification.IncRequests_OK();

                                                                 #region Send OnChargingErrorNotificationRequest event

                                                                 try
                                                                 {

                                                                     if (OnChargingErrorNotificationRequest is not null)
                                                                         await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                                                                            Cast<OnChargingErrorNotificationAPIRequestDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          chargingErrorNotificationRequest))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingErrorNotificationRequest));
                                                                 }

                                                                 #endregion

                                                                 #region Call async subscribers

                                                                 OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>? startAcknowledgement = null;

                                                                 var OnChargingErrorNotificationLocal = OnChargingErrorNotification;
                                                                 if (OnChargingErrorNotificationLocal is not null)
                                                                 {
                                                                     try
                                                                     {

                                                                         startAcknowledgement = (await Task.WhenAll(OnChargingErrorNotificationLocal.GetInvocationList().
                                                                                                                    Cast<OnChargingErrorNotificationAPIDelegate>().
                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                  this,
                                                                                                                                  chargingErrorNotificationRequest))).
                                                                                                                    ConfigureAwait(false))?.FirstOrDefault();

                                                                         Counters.ChargingErrorNotification.IncResponses_OK();

                                                                     }
                                                                     catch (Exception e)
                                                                     {
                                                                         startAcknowledgement = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                                                                    this,
                                                                                                    new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                                        Timestamp.Now,
                                                                                                        Request.EventTrackingId,
                                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                                        StatusCode: new StatusCode(
                                                                                                                        StatusCodes.DataError,
                                                                                                                        e.Message,
                                                                                                                        e.StackTrace
                                                                                                                    ),
                                                                                                        chargingErrorNotificationRequest
                                                                                                    )
                                                                                                );
                                                                     }
                                                                 }

                                                                 if (startAcknowledgement is null)
                                                                     startAcknowledgement = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                                                                this,
                                                                                                new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.SystemError,
                                                                                                                    "Could not process the received ChargingErrorNotification request!"
                                                                                                                ),
                                                                                                    chargingErrorNotificationRequest
                                                                                                )
                                                                                            );

                                                                 #endregion

                                                                 #region Send OnChargingErrorNotificationResponse event

                                                                 try
                                                                 {

                                                                     if (OnChargingErrorNotificationResponse is not null)
                                                                         await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                                                                            Cast<OnChargingErrorNotificationAPIResponseDelegate>().
                                                                                            Select(e => e(Timestamp.Now,
                                                                                                          this,
                                                                                                          startAcknowledgement,
                                                                                                          Timestamp.Now - startTime))).
                                                                                            ConfigureAwait(false);

                                                                 }
                                                                 catch (Exception e)
                                                                 {
                                                                     DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingErrorNotificationResponse));
                                                                 }

                                                                 #endregion

                                                                 acknowledgement = OICPResult<Acknowledgement>.From(startAcknowledgement);

                                                             }
                                                             else
                                                                 acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                                      this,
                                                                                      new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.DataError,
                                                                                                          "We could not parse the given ChargingErrorNotification request!",
                                                                                                          errorResponse
                                                                                                      ),
                                                                                          chargingErrorNotificationRequest
                                                                                      )
                                                                                  );

                                                             break;

                                                         #endregion

                                                         #region ...or default

                                                         default:

                                                             acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                                   this,
                                                                                   new Acknowledgement(
                                                                                       Request.Timestamp,
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.DataError,
                                                                                                       "Unknown or invalid charging notification type `" + chargingNotificationType.ToString() + "`!"
                                                                                                       //errorResponse
                                                                                                   )
                                                                                       //chargeDetailRecordRequest!
                                                                                   )
                                                                               );

                                                             break;

                                                         #endregion

                                                     }

                                                 }
                                                 else
                                                     acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                           this,
                                                                           new Acknowledgement(
                                                                               Request.Timestamp,
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.DataError,
                                                                                               "We could not parse the given ChargingNotification request!"
                                                                                               //errorResponse
                                                                                           )
                                                                               //chargeDetailRecordRequest!
                                                                           )
                                                                       );

                                             }
                                             catch (Exception e)
                                             {
                                                 acknowledgement = OICPResult<Acknowledgement>.Failed(
                                                                       this,
                                                                       new Acknowledgement(
                                                                           Request.Timestamp,
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.SystemError,
                                                                                           e.Message,
                                                                                           e.StackTrace
                                                                                       )
                                                                       )
                                                                   );
                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = Timestamp.Now,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = acknowledgement.Response.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                     CustomStatusCodeSerializer).
                                                                                                              ToString(JSONFormatting).
                                                                                                              ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record

            // ------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/operators/DE-GEF/charge-detail-record
            // ------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logChargeDetailRecordHTTPRequest,
                                         HTTPResponseLogger:  logChargeDetailRecordHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             OICPResult<Acknowledgement<ChargeDetailRecordRequest>>? chargeDetailRecordResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                                     chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                                      this,
                                                                                      new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.SystemError,
                                                                                                          "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                                      )
                                                                                      )
                                                                                  );

                                                 #endregion

                                                 else if (ChargeDetailRecordRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                             operatorId,
                                                                                             Request.Timeout ?? DefaultRequestTimeout,
                                                                                             out ChargeDetailRecordRequest?          chargeDetailRecordRequest,
                                                                                             out String?                             errorResponse,
                                                                                             Timestamp:                              Request.Timestamp,
                                                                                             EventTrackingId:                        Request.EventTrackingId,
                                                                                             CustomChargeDetailRecordRequestParser:  CustomChargeDetailRecordRequestParser))
                                                 {

                                                     Counters.ChargeDetailRecord.IncRequests_OK();

                                                     #region Send OnChargeDetailRecordRequest event

                                                     try
                                                     {

                                                         if (OnChargeDetailRecordRequest is not null)
                                                             await Task.WhenAll(OnChargeDetailRecordRequest.GetInvocationList().
                                                                                Cast<OnChargeDetailRecordAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              chargeDetailRecordRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargeDetailRecordRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnChargeDetailRecordLocal = OnChargeDetailRecord;
                                                     if (OnChargeDetailRecordLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             chargeDetailRecordResponse = (await Task.WhenAll(OnChargeDetailRecordLocal.GetInvocationList().
                                                                                                                                        Cast<OnChargeDetailRecordAPIDelegate>().
                                                                                                                                        Select(e => e(Timestamp.Now,
                                                                                                                                                      this,
                                                                                                                                                      chargeDetailRecordRequest!))))?.FirstOrDefault();

                                                             Counters.ChargeDetailRecord.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                                              this,
                                                                                              new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                                  Timestamp.Now,
                                                                                                  Request.EventTrackingId,
                                                                                                  Timestamp.Now - Request.Timestamp,
                                                                                                  StatusCode: new StatusCode(
                                                                                                                  StatusCodes.DataError,
                                                                                                                  e.Message,
                                                                                                                  e.StackTrace
                                                                                                              ),
                                                                                                  chargeDetailRecordRequest!
                                                                                              )
                                                                                          );
                                                         }
                                                     }

                                                     if (chargeDetailRecordResponse is null)
                                                         chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                                          this,
                                                                                          new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.SystemError,
                                                                                                              "Could not process the received ChargeDetailRecord request!"
                                                                                                          ),
                                                                                              chargeDetailRecordRequest!
                                                                                          )
                                                                                      );

                                                     #endregion

                                                     #region Send OnChargeDetailRecordResponse event

                                                     try
                                                     {

                                                         if (OnChargeDetailRecordResponse is not null)
                                                             await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                                                                Cast<OnChargeDetailRecordAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              chargeDetailRecordResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargeDetailRecordResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                                      this,
                                                                                      new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.DataError,
                                                                                                          "We could not parse the given ChargeDetailRecord request!",
                                                                                                          errorResponse
                                                                                                      ),
                                                                                          chargeDetailRecordRequest!
                                                                                      )
                                                                                  );

                                             }
                                             catch (Exception e)
                                             {
                                                 chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                                  this,
                                                                                  new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                      Timestamp.Now,
                                                                                      Request.EventTrackingId,
                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                      StatusCode: new StatusCode(
                                                                                                      StatusCodes.SystemError,
                                                                                                      e.Message,
                                                                                                      e.StackTrace
                                                                                                  )
                                                                                  )
                                                                              );
                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = Timestamp.Now,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = chargeDetailRecordResponse.Response.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                CustomStatusCodeSerializer).
                                                                                                                         ToString(JSONFormatting).
                                                                                                                         ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

        }

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public override void Dispose()
        { }

        #endregion

    }

}
