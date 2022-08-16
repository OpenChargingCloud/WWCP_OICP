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

using System.Net.Security;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.p2p.EMP
{

    /// <summary>
    /// The EMP p2p combines EMPClient(s) and the CPOClientAPI,
    /// and adds additional logging for all.
    /// </summary>
    public class EMPPeer : APeer, IEMPPeer
    {

        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  PullEVSEData                       { get; }
            public APICounterValues  PullEVSEStatus                     { get; }
            public APICounterValues  PullEVSEStatusById                 { get; }

            public APICounterValues  PullPricingProductData             { get; }
            public APICounterValues  PullEVSEPricing                    { get; }

            public APICounterValues  PushAuthenticationData             { get; }

            public APICounterValues  AuthorizeRemoteReservationStart    { get; }
            public APICounterValues  AuthorizeRemoteReservationStop     { get; }
            public APICounterValues  AuthorizeRemoteStart               { get; }
            public APICounterValues  AuthorizeRemoteStop                { get; }

            public APICounterValues  GetChargeDetailRecords             { get; }

            public APICounters(APICounterValues? PullEVSEData                       = null,
                               APICounterValues? PullEVSEStatus                     = null,
                               APICounterValues? PullEVSEStatusById                 = null,

                               APICounterValues? PullPricingProductData             = null,
                               APICounterValues? PullEVSEPricing                    = null,

                               APICounterValues? PushAuthenticationData             = null,

                               APICounterValues? AuthorizeRemoteReservationStart    = null,
                               APICounterValues? AuthorizeRemoteReservationStop     = null,
                               APICounterValues? AuthorizeRemoteStart               = null,
                               APICounterValues? AuthorizeRemoteStop                = null,

                               APICounterValues? GetChargeDetailRecords             = null)
            {

                this.PullEVSEData                     = PullEVSEData                    ?? new APICounterValues();
                this.PullEVSEStatus                   = PullEVSEStatus                  ?? new APICounterValues();
                this.PullEVSEStatusById               = PullEVSEStatusById              ?? new APICounterValues();

                this.PullPricingProductData           = PullPricingProductData          ?? new APICounterValues();
                this.PullEVSEPricing                  = PullEVSEPricing                 ?? new APICounterValues();

                this.PushAuthenticationData           = PushAuthenticationData          ?? new APICounterValues();

                this.AuthorizeRemoteReservationStart  = AuthorizeRemoteReservationStart ?? new APICounterValues();
                this.AuthorizeRemoteReservationStop   = AuthorizeRemoteReservationStop  ?? new APICounterValues();
                this.AuthorizeRemoteStart             = AuthorizeRemoteStart            ?? new APICounterValues();
                this.AuthorizeRemoteStop              = AuthorizeRemoteStop             ?? new APICounterValues();

                this.GetChargeDetailRecords           = GetChargeDetailRecords          ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PullEVSEData",                     PullEVSEData.                   ToJSON()),
                       new JProperty("PullEVSEStatus",                   PullEVSEStatus.                 ToJSON()),
                       new JProperty("PullEVSEStatusById",               PullEVSEStatusById.             ToJSON()),

                       new JProperty("PullPricingProductData",           PullPricingProductData.         ToJSON()),
                       new JProperty("PullEVSEPricing",                  PullEVSEPricing.                ToJSON()),

                       new JProperty("PushAuthenticationData",           PushAuthenticationData.         ToJSON()),

                       new JProperty("AuthorizeRemoteReservationStart",  AuthorizeRemoteReservationStart.ToJSON()),
                       new JProperty("AuthorizeRemoteReservationStop",   AuthorizeRemoteReservationStop. ToJSON()),
                       new JProperty("AuthorizeRemoteStart",             AuthorizeRemoteStart.           ToJSON()),
                       new JProperty("AuthorizeRemoteStop",              AuthorizeRemoteStop.            ToJSON()),

                       new JProperty("GetChargeDetailRecords",           GetChargeDetailRecords.         ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public  const              String                    DefaultHTTPServerName           = "Open Charging Cloud - EMP p2p HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public  const              String                    DefaultHTTPServiceName          = "Open Charging Cloud - EMP p2p HTTP API";

        private readonly HTTPAPI?                            httpAPI;

        private readonly Dictionary<Operator_Id, EMPClient>  empClients;

        #endregion

        #region Properties

        public APICounters   Counters        { get; }

        /// <summary>
        /// The CPO client API.
        /// </summary>
        public CPOClientAPI  CPOClientAPI    { get; }

        #endregion

        #region Events

        #region Generic HTTP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog
            => httpAPI.RequestLog;

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog
            => httpAPI.ResponseLog;

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog
            => httpAPI.ErrorLog;

        #endregion


        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEData request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate?   OnPullEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?         OnPullEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?        OnPullEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEData request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate?  OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatus request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate?   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEStatus HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?           OnPullEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatus HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?          OnPullEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatus request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate?  OnPullEVSEStatusResponse;

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatusById request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestDelegate?   OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEStatusById HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?               OnPullEVSEStatusByIdHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatusById HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?              OnPullEVSEStatusByIdHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatusById request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseDelegate?  OnPullEVSEStatusByIdResponse;

        #endregion


        #region OnPullPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullPricingProductData request will be send.
        /// </summary>
        public event OnPullPricingProductDataRequestDelegate?   OnPullPricingProductDataRequest;

        /// <summary>
        /// An event fired whenever a PullPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPullPricingProductDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPullPricingProductDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullPricingProductData request had been received.
        /// </summary>
        public event OnPullPricingProductDataResponseDelegate?  OnPullPricingProductDataResponse;

        #endregion

        #region OnPullEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEPricing request will be send.
        /// </summary>
        public event OnPullEVSEPricingRequestDelegate?   OnPullEVSEPricingRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?            OnPullEVSEPricingHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?           OnPullEVSEPricingHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEPricing request had been received.
        /// </summary>
        public event OnPullEVSEPricingResponseDelegate?  OnPullEVSEPricingResponse;

        #endregion


        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever an PushAuthenticationData request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestDelegate?   OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever an PushAuthenticationData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPushAuthenticationDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an PushAuthenticationData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPushAuthenticationDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an PushAuthenticationData request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseDelegate?  OnPushAuthenticationDataResponse;

        #endregion


        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestDelegate?   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                            OnAuthorizeRemoteReservationStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                           OnAuthorizeRemoteReservationStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStart request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseDelegate?  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestDelegate?   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                           OnAuthorizeRemoteReservationStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                          OnAuthorizeRemoteReservationStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStop request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseDelegate?  OnAuthorizeRemoteReservationStopResponse;

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestDelegate?   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                 OnAuthorizeRemoteStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                OnAuthorizeRemoteStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStart request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseDelegate?  OnAuthorizeRemoteStartResponse;

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestDelegate?   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                OnAuthorizeRemoteStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?               OnAuthorizeRemoteStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStop request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseDelegate?  OnAuthorizeRemoteStopResponse;

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a GetChargeDetailRecords request will be send.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestDelegate?   OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event fired whenever a GetChargeDetailRecords HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnGetChargeDetailRecordsHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a GetChargeDetailRecords HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnGetChargeDetailRecordsHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a GetChargeDetailRecords request had been received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseDelegate?  OnGetChargeDetailRecordsResponse;

        #endregion

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP p2p service.
        /// </summary>
        public EMPPeer(AsymmetricCipherKeyPair?              KeyPair                            = null,

                       HTTPHostname?                         HTTPHostname                       = null,
                       String?                               ExternalDNSName                    = null,
                       IPPort?                               HTTPServerPort                     = null,
                       HTTPPath?                             BasePath                           = null,
                       String?                               HTTPServerName                     = DefaultHTTPServerName,

                       HTTPPath?                             URLPathPrefix                      = null,
                       String?                               HTTPServiceName                    = DefaultHTTPServiceName,
                       String?                               HTMLTemplate                       = null,
                       JObject?                              APIVersionHashes                   = null,
                       ServerCertificateSelectorDelegate?    ServerCertificateSelector          = null,
                       RemoteCertificateValidationCallback?  ClientCertificateValidator         = null,
                       LocalCertificateSelectionCallback?    ClientCertificateSelector          = null,
                       SslProtocols?                         AllowedTLSProtocols                = null,
                       Boolean?                              ClientCertificateRequired          = null,
                       Boolean?                              CheckCertificateRevocation         = null,

                       String?                               ServerThreadName                   = null,
                       ThreadPriority?                       ServerThreadPriority               = null,
                       Boolean?                              ServerThreadIsBackground           = null,
                       ConnectionIdBuilder?                  ConnectionIdBuilder                = null,
                       ConnectionThreadsNameBuilder?         ConnectionThreadsNameBuilder       = null,
                       ConnectionThreadsPriorityBuilder?     ConnectionThreadsPriorityBuilder   = null,
                       Boolean?                              ConnectionThreadsAreBackground     = null,
                       TimeSpan?                             ConnectionTimeout                  = null,
                       UInt32?                               MaxClientConnections               = null,

                       Boolean?                              DisableMaintenanceTasks            = null,
                       TimeSpan?                             MaintenanceInitialDelay            = null,
                       TimeSpan?                             MaintenanceEvery                   = null,

                       Boolean?                              DisableWardenTasks                 = null,
                       TimeSpan?                             WardenInitialDelay                 = null,
                       TimeSpan?                             WardenCheckEvery                   = null,

                       Boolean?                              IsDevelopment                      = null,
                       IEnumerable<String>?                  DevelopmentServers                 = null,
                       Boolean?                              DisableLogging                     = null,
                       String?                               LoggingPath                        = null,
                       String?                               LogfileName                        = null,
                       LogfileCreatorDelegate?               LogfileCreator                     = null,
                       DNSClient?                            DNSClient                          = null,
                       Boolean                               Autostart                          = false)

            : base(KeyPair)

        {

            httpAPI = new HTTPAPI(HTTPHostname,
                                  ExternalDNSName,
                                  HTTPServerPort,
                                  BasePath,
                                  HTTPServerName,

                                  URLPathPrefix,
                                  HTTPServiceName,
                                  HTMLTemplate,
                                  APIVersionHashes,

                                  ServerCertificateSelector,
                                  ClientCertificateValidator,
                                  ClientCertificateSelector,
                                  AllowedTLSProtocols,
                                  ClientCertificateRequired,
                                  CheckCertificateRevocation,

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
                                  false);

            httpAPI.HTTPServer.AddMethodCallback(org.GraphDefined.Vanaheimr.Hermod.HTTP.HTTPHostname.Any,
                                                 HTTPMethod.GET,
                                                 new HTTPPath[] {
                                                     URLPathPrefix + "/",
                                                     URLPathPrefix + "/{FileName}"
                                                 },
                                                 HTTPDelegate: Request => {
                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.OK,
                                                             Server          = httpAPI.HTTPServer.DefaultServerName,
                                                             Date            = Timestamp.Now,
                                                             ContentType     = HTTPContentType.TEXT_UTF8,
                                                             Content         = "This is an OICP v2.3 EMP p2p HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                             CacheControl    = "public, max-age=300",
                                                             Connection      = "close"
                                                         }.AsImmutable);
                                                 });

            this.CPOClientAPI  = new CPOClientAPI(httpAPI);
            this.empClients    = new Dictionary<Operator_Id, EMPClient>();
            this.Counters      = new APICounters();

            if (Autostart)
                httpAPI.Start();

        }

        #endregion


        #region RegisterOperator(OperatorId, EMPClient)

        /// <summary>
        /// Register the given EMPClient for the given charge point operator identification.
        /// </summary>
        /// <param name="OperatorId">An charge point operator identification.</param>
        /// <param name="EMPClient">An EMP client.</param>
        public Boolean RegisterOperator(Operator_Id  OperatorId,
                                        EMPClient    EMPClient)
        {
            lock (empClients)
            {
                return empClients.TryAdd(OperatorId, EMPClient);
            }
        }

        #endregion

        #region GetEMPClient(OperatorId)

        public EMPClient? GetEMPClient(Operator_Id OperatorId)
        {
            lock (empClients)
            {
                return empClients.GetValueOrDefault(OperatorId);
            }
        }

        #endregion


        #region PullEVSEData          (OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">A PullEVSEData request.</param>
        public async Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(Operator_Id          OperatorId,
                         PullEVSEDataRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.PullEVSEData(Request);
            }

            return OICPResult<PullEVSEDataResponse>.Failed(
                       Request,
                       new PullEVSEDataResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<EVSEDataRecord>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       ),
                       processId
                   );

        }

        #endregion

        #region PullEVSEStatus        (OperatorId, Request)

        /// <summary>
        /// Download EVSE status records.
        /// The request might have an optional search radius and/or status filter.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public async Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(Operator_Id            OperatorId,
                           PullEVSEStatusRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.PullEVSEStatus(Request);
            }

            return OICPResult<PullEVSEStatusResponse>.Failed(
                       Request,
                       new PullEVSEStatusResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<OperatorEVSEStatus>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       ),
                       processId
                   );

        }

        #endregion

        #region PullEVSEStatusById    (OperatorId, Request)

        /// <summary>
        /// Download the current status of up to 100 EVSEs.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public async Task<OICPResult<PullEVSEStatusByIdResponse>>

            PullEVSEStatusById(Operator_Id                OperatorId,
                               PullEVSEStatusByIdRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.PullEVSEStatusById(Request);
            }

            return OICPResult<PullEVSEStatusByIdResponse>.Failed(
                       Request,
                       new PullEVSEStatusByIdResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<EVSEStatusRecord>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       ),
                       processId
                   );

        }

        #endregion


        #region PullPricingProductData(OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">A PullPricingProductData request.</param>
        public async Task<OICPResult<PullPricingProductDataResponse>>

            PullPricingProductData(Operator_Id                    OperatorId,
                                   PullPricingProductDataRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.PullPricingProductData(Request);
            }

            return OICPResult<PullPricingProductDataResponse>.Failed(
                       Request,
                       new PullPricingProductDataResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<PricingProductData>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       ),
                       processId
                   );

        }

        #endregion

        #region PullEVSEPricing       (OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">A PullEVSEPricing request.</param>
        public async Task<OICPResult<PullEVSEPricingResponse>>

            PullEVSEPricing(Operator_Id             OperatorId,
                            PullEVSEPricingRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.PullEVSEPricing(Request);
            }

            return OICPResult<PullEVSEPricingResponse>.Failed(
                       Request,
                       new PullEVSEPricingResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<OperatorEVSEPricing>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       ),
                       processId
                   );

        }

        #endregion


        #region PushAuthenticationData(OperatorId, Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public async Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(Operator_Id                    OperatorId,
                                   PushAuthenticationDataRequest  Request)

        {

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.PushAuthenticationData(Request);
            }

            return OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                       Request,
                       Acknowledgement<PushAuthenticationDataRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion


        #region AuthorizeRemoteReservationStart   (Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request)

        {

            if (empClients.TryGetValue(Request.EVSEId.OperatorId, out EMPClient? empClient))
            {
                return await empClient.AuthorizeRemoteReservationStart(Request);
            }

            return OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                       Request,
                       Acknowledgement<AuthorizeRemoteReservationStartRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion

        #region AuthorizeRemoteReservationStop    (Request)

        /// <summary>
        /// Stop a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(AuthorizeRemoteReservationStopRequest  Request)

        {

            if (empClients.TryGetValue(Request.EVSEId.OperatorId, out EMPClient? empClient))
            {
                return await empClient.AuthorizeRemoteReservationStop(Request);
            }

            return OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                       Request,
                       Acknowledgement<AuthorizeRemoteReservationStopRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion


        #region AuthorizeRemoteStart              (Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(AuthorizeRemoteStartRequest  Request)

        {

            #region Send OnAuthorizeRemoteStartRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteStartRequest));
            }

            #endregion


            OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>> result;

            if (empClients.TryGetValue(Request.EVSEId.OperatorId, out EMPClient? empClient))
            {

                result = await empClient.AuthorizeRemoteStart(Request);

            }

            else
                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteStartRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteStart.IncResponses_Error();

            #region Send OnAuthorizeRemoteStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStop               (Request)

        /// <summary>
        /// Stop a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(AuthorizeRemoteStopRequest  Request)

        {

            #region Send OnAuthorizeRemoteStopRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteStopRequest));
            }

            #endregion


            OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>> result;

            if (empClients.TryGetValue(Request.EVSEId.OperatorId, out EMPClient? empClient))
            {

                result = await empClient.AuthorizeRemoteStop(Request);

            }

            else
                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteStopRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteStop.IncResponses_Error();

            #region Send OnAuthorizeRemoteStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetChargeDetailRecords(OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">A registered charge point operator.</param>
        /// <param name="Request">A GetChargeDetailRecords request.</param>
        public async Task<OICPResult<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(Operator_Id                    OperatorId,
                                   GetChargeDetailRecordsRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {
                return await empClient.GetChargeDetailRecords(Request);
            }

            return OICPResult<GetChargeDetailRecordsResponse>.Failed(
                       Request,
                       new GetChargeDetailRecordsResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<ChargeDetailRecord>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       ),
                       processId
                   );

        }

        #endregion


        #region Start()

        public void Start()
        {

            if (httpAPI is not null)
                httpAPI.Start();

            else
            {
                CPOClientAPI.Start();
            }

        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String?  Message   = null,
                             Boolean  Wait      = true)
        {

            if (httpAPI is not null)
                httpAPI.Shutdown(Message, Wait);

            else
            {
                CPOClientAPI.Shutdown(Message, Wait);
            }

        }

        #endregion

        #region Dispose()

        public void Dispose()
        {

            if (httpAPI is not null)
                httpAPI.Dispose();

            else
            {

                if (CPOClientAPI is not null)
                    CPOClientAPI.Dispose();

            }

            foreach (var cpoClient in empClients.Values)
                cpoClient.Dispose();

        }

        #endregion

    }

}
