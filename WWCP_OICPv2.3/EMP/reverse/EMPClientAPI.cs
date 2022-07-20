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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP HTTP Client API.
    /// </summary>
    public partial class EMPClientAPI : HTTPAPI
    {

        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  PullEVSEData                    { get; }
            public APICounterValues  AuthorizeStop                   { get; }
            public APICounterValues  ChargingStartNotification       { get; }
            public APICounterValues  ChargingProgressNotification    { get; }
            public APICounterValues  ChargingEndNotification         { get; }
            public APICounterValues  ChargingErrorNotification       { get; }
            public APICounterValues  ChargeDetailRecord              { get; }


            public APICounters(APICounterValues? PullEVSEData                   = null,
                               APICounterValues? AuthorizeStop                  = null,
                               APICounterValues? ChargingStartNotification      = null,
                               APICounterValues? ChargingProgressNotification   = null,
                               APICounterValues? ChargingEndNotification        = null,
                               APICounterValues? ChargingErrorNotification      = null,
                               APICounterValues? ChargeDetailRecord             = null)
            {

                this.PullEVSEData                  = PullEVSEData                 ?? new APICounterValues();
                this.AuthorizeStop                 = AuthorizeStop                ?? new APICounterValues();
                this.ChargingStartNotification     = ChargingStartNotification    ?? new APICounterValues();
                this.ChargingProgressNotification  = ChargingProgressNotification ?? new APICounterValues();
                this.ChargingEndNotification       = ChargingEndNotification      ?? new APICounterValues();
                this.ChargingErrorNotification     = ChargingErrorNotification    ?? new APICounterValues();
                this.ChargeDetailRecord            = ChargeDetailRecord           ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PullEVSEData",                  PullEVSEData.                ToJSON()),
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
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "EMPServerAPI";

        #endregion

        #region Properties

        public APICounters                                                        Counters                                           { get; }

        // Custom JSON parsers

        public CustomJObjectParserDelegate<PullEVSEDataRequest>?                  CustomPullEVSEDataRequestParser                    { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEDataResponse>?             CustomPullEVSEDataResponseSerializer               { get; set; }
        public CustomJObjectSerializerDelegate<EVSEDataRecord>?                   CustomEVSEDataRecordSerializer                     { get; set; }
        public CustomJObjectSerializerDelegate<Address>?                          CustomAddressSerializer                            { get; set; }
        public CustomJObjectSerializerDelegate<ChargingFacility>?                 CustomChargingFacilitySerializer                   { get; set; }
        public CustomJObjectSerializerDelegate<GeoCoordinates>?                   CustomGeoCoordinatesSerializer                     { get; set; }
        public CustomJObjectSerializerDelegate<EnergySource>?                     CustomEnergySourceSerializer                       { get; set; }
        public CustomJObjectSerializerDelegate<EnvironmentalImpact>?              CustomEnvironmentalImpactSerializer                { get; set; }
        public CustomJObjectSerializerDelegate<OpeningTime>?                      CustomOpeningTimesSerializer                       { get; set; }
        public CustomJObjectSerializerDelegate<StatusCode>?                       CustomStatusCodeSerializer                         { get; set; }


        public Newtonsoft.Json.Formatting                                         JSONFormatting                                     { get; set; }

        #endregion

        #region Events

        #region (protected internal) OnPullEVSEDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullEVSEDataHTTPRequest(DateTime     Timestamp,
                                                           HTTPAPI      API,
                                                           HTTPRequest  Request)

            => OnPullEVSEDataHTTPRequest.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (event)              OnPullEVSEData(Request-/Response)

        /// <summary>
        /// An event send whenever a PullEVSEData request was received.
        /// </summary>
        public event OnPullEVSEDataAPIRequestDelegate?   OnPullEVSEDataRequest;

        /// <summary>
        /// An event send whenever a PullEVSEData request was received.
        /// </summary>
        public event OnPullEVSEDataAPIDelegate?          OnPullEVSEData;

        /// <summary>
        /// An event send whenever a response to a PullEVSEData request was sent.
        /// </summary>
        public event OnPullEVSEDataAPIResponseDelegate?  OnPullEVSEDataResponse;

        #endregion

        #region (protected internal) OnPullEVSEDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullEVSEDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullEVSEDataHTTPResponse(DateTime      Timestamp,
                                                            HTTPAPI       API,
                                                            HTTPRequest   Request,
                                                            HTTPResponse  Response)

            => OnPullEVSEDataHTTPResponse.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP HTTP Client API.
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
        public EMPClientAPI(HTTPHostname?                         HTTPHostname                       = null,
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


            //Note: OperatorId is the remote EMP sending an authorize start/stop request!

            #region POST  ~/api/oicp/evsepull/v23/providers/{providerId}/data-records

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v23/providers/DE-GDF/data-records
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepull/v23/providers/{providerId}/data-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullEVSEDataHTTPRequest,
                                         HTTPResponseLogger:  logPullEVSEDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             OICPResult<PullEVSEDataResponse>? pullEVSEDataResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                this,
                                                                                new PullEVSEDataResponse(
                                                                                    null,
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    Array.Empty<EVSEDataRecord>(),
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.SystemError,
                                                                                                    "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                )
                                                                                )
                                                                            );

                                                 #endregion

                                                 else if (PullEVSEDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                       //     Request.Timeout ?? DefaultRequestTimeout,
                                                                                       //     providerId ????
                                                                                       out PullEVSEDataRequest?          pullEVSEDataRequest,
                                                                                       out String?                       errorResponse,
                                                                                       Page:                             0,
                                                                                       Size:                             0,
                                                                                       SortOrder:                        null,
                                                                                       Timestamp:                        Request.Timestamp,
                                                                                       EventTrackingId:                  Request.EventTrackingId,
                                                                                       CustomPullEVSEDataRequestParser:  null))
                                                 {

                                                     Counters.PullEVSEData.IncRequests_OK();

                                                     #region Send OnPullEVSEDataRequest event

                                                     try
                                                     {

                                                         if (OnPullEVSEDataRequest is not null)
                                                             await Task.WhenAll(OnPullEVSEDataRequest.GetInvocationList().
                                                                                Cast<OnPullEVSEDataAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEDataRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullEVSEDataLocal = OnPullEVSEData;
                                                     if (OnPullEVSEDataLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEDataResponse = (await Task.WhenAll(OnPullEVSEDataLocal.GetInvocationList().
                                                                                                                                Cast<OnPullEVSEDataAPIDelegate>().
                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                              this,
                                                                                                                                              pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.PullEVSEData.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                        this,
                                                                                        new PullEVSEDataResponse(
                                                                                            pullEVSEDataRequest!,
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            Array.Empty<EVSEDataRecord>(),
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            e.Message,
                                                                                                            e.StackTrace
                                                                                                        )
                                                                                        )
                                                                                    );
                                                         }
                                                     }

                                                     if (pullEVSEDataResponse is null)
                                                         pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                    this,
                                                                                    new PullEVSEDataResponse(
                                                                                        pullEVSEDataRequest!,
                                                                                        Timestamp.Now,
                                                                                        Request.EventTrackingId,
                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                        Array.Empty<EVSEDataRecord>(),
                                                                                        StatusCode: new StatusCode(
                                                                                                        StatusCodes.SystemError,
                                                                                                        "Could not process the received PullEVSEData request!"
                                                                                                    )
                                                                                    )
                                                                                );

                                                     #endregion

                                                     #region Send OnPullEVSEDataResponse event

                                                     try
                                                     {

                                                         if (OnPullEVSEDataResponse is not null)
                                                             await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
                                                                                Cast<OnPullEVSEDataAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEDataResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                this,
                                                                                new PullEVSEDataResponse(
                                                                                    pullEVSEDataRequest!,
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    Array.Empty<EVSEDataRecord>(),
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    "We could not parse the given PullEVSEData request!",
                                                                                                    errorResponse
                                                                                                )
                                                                                )
                                                                            );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                            this,
                                                                            new PullEVSEDataResponse(
                                                                                null,
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                Array.Empty<EVSEDataRecord>(),
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
                                                        Content                    = pullEVSEDataResponse.Response.ToJSON(CustomPullEVSEDataResponseSerializer,
                                                                                                                          CustomEVSEDataRecordSerializer,
                                                                                                                          CustomAddressSerializer,
                                                                                                                          CustomChargingFacilitySerializer,
                                                                                                                          CustomGeoCoordinatesSerializer,
                                                                                                                          CustomEnergySourceSerializer,
                                                                                                                          CustomEnvironmentalImpactSerializer,
                                                                                                                          CustomOpeningTimesSerializer,
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
