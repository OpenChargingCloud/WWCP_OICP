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

using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
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

        public class APICounters(APICounterValues?  PullEVSEData                      = null,
                                 APICounterValues?  PullEVSEStatus                    = null,
                                 APICounterValues?  PullEVSEStatusById                = null,

                                 APICounterValues?  PullPricingProductData            = null,
                                 APICounterValues?  PullEVSEPricing                   = null,

                                 APICounterValues?  PushAuthenticationData            = null,

                                 APICounterValues?  AuthorizeRemoteReservationStart   = null,
                                 APICounterValues?  AuthorizeRemoteReservationStop    = null,
                                 APICounterValues?  AuthorizeRemoteStart              = null,
                                 APICounterValues?  AuthorizeRemoteStop               = null,

                                 APICounterValues?  GetChargeDetailRecords            = null)
        {

            public APICounterValues PullEVSEData                       { get; } = PullEVSEData                    ?? new APICounterValues();
            public APICounterValues PullEVSEStatus                     { get; } = PullEVSEStatus                  ?? new APICounterValues();
            public APICounterValues PullEVSEStatusById                 { get; } = PullEVSEStatusById              ?? new APICounterValues();

            public APICounterValues PullPricingProductData             { get; } = PullPricingProductData          ?? new APICounterValues();
            public APICounterValues PullEVSEPricing                    { get; } = PullEVSEPricing                 ?? new APICounterValues();

            public APICounterValues PushAuthenticationData             { get; } = PushAuthenticationData          ?? new APICounterValues();

            public APICounterValues AuthorizeRemoteReservationStart    { get; } = AuthorizeRemoteReservationStart ?? new APICounterValues();
            public APICounterValues AuthorizeRemoteReservationStop     { get; } = AuthorizeRemoteReservationStop  ?? new APICounterValues();
            public APICounterValues AuthorizeRemoteStart               { get; } = AuthorizeRemoteStart            ?? new APICounterValues();
            public APICounterValues AuthorizeRemoteStop                { get; } = AuthorizeRemoteStop             ?? new APICounterValues();

            public APICounterValues GetChargeDetailRecords             { get; } = GetChargeDetailRecords          ?? new APICounterValues();


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

        /// <summary>
        /// The e-mobility provider identification of this peer.
        /// </summary>
        public Provider_Id   ProviderId      { get; }

        /// <summary>
        /// EMP Peer API counters.
        /// </summary>
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
        public HTTPRequestLogEvent?   RequestLog
            => httpAPI?.RequestLog;

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent?  ResponseLog
            => httpAPI?.ResponseLog;

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent?     ErrorLog
            => httpAPI?.ErrorLog;

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

        #region EMPPeer(ProviderId, KeyPair = null, ...)

        /// <summary>
        /// Create a new EMP p2p service.
        /// </summary>
        public EMPPeer(Provider_Id                                                ProviderId,
                       AsymmetricCipherKeyPair?                                   KeyPair                      = null,

                       HTTPHostname?                                              HTTPHostname                 = null,
                       String?                                                    ExternalDNSName              = null,
                       IPPort?                                                    HTTPServerPort               = null,
                       HTTPPath?                                                  BasePath                     = null,
                       String?                                                    HTTPServerName               = DefaultHTTPServerName,

                       HTTPPath?                                                  URLPathPrefix                = null,
                       String?                                                    HTTPServiceName              = DefaultHTTPServiceName,
                       String?                                                    HTMLTemplate                 = null,
                       JObject?                                                   APIVersionHashes             = null,

                       ServerCertificateSelectorDelegate?                         ServerCertificateSelector    = null,
                       RemoteTLSClientCertificateValidationHandler<IHTTPServer>?  ClientCertificateValidator   = null,
                       LocalCertificateSelectionHandler?                          ClientCertificateSelector    = null,
                       SslProtocols?                                              AllowedTLSProtocols          = null,
                       Boolean?                                                   ClientCertificateRequired    = null,
                       Boolean?                                                   CheckCertificateRevocation   = null,

                       ServerThreadNameCreatorDelegate?                           ServerThreadNameCreator      = null,
                       ServerThreadPriorityDelegate?                              ServerThreadPrioritySetter   = null,
                       Boolean?                                                   ServerThreadIsBackground     = null,
                       ConnectionIdBuilder?                                       ConnectionIdBuilder          = null,
                       TimeSpan?                                                  ConnectionTimeout            = null,
                       UInt32?                                                    MaxClientConnections         = null,

                       Boolean?                                                   DisableMaintenanceTasks      = null,
                       TimeSpan?                                                  MaintenanceInitialDelay      = null,
                       TimeSpan?                                                  MaintenanceEvery             = null,

                       Boolean?                                                   DisableWardenTasks           = null,
                       TimeSpan?                                                  WardenInitialDelay           = null,
                       TimeSpan?                                                  WardenCheckEvery             = null,

                       Boolean?                                                   IsDevelopment                = null,
                       IEnumerable<String>?                                       DevelopmentServers           = null,
                       Boolean?                                                   DisableLogging               = null,
                       String?                                                    LoggingPath                  = null,
                       String?                                                    LogfileName                  = null,
                       LogfileCreatorDelegate?                                    LogfileCreator               = null,
                       DNSClient?                                                 DNSClient                    = null,
                       Boolean                                                    AutoStart                    = false)

            : base(KeyPair)

        {

            httpAPI = new HTTPAPI(
                          HTTPHostname,
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

                          ServerThreadNameCreator,
                          ServerThreadPrioritySetter,
                          ServerThreadIsBackground,
                          ConnectionIdBuilder,
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
                          false
                      );

            this.ProviderId    = ProviderId;
            this.CPOClientAPI  = new CPOClientAPI(httpAPI);
            this.empClients    = [];
            this.Counters      = new APICounters();

            httpAPI.AddMethodCallback(org.GraphDefined.Vanaheimr.Hermod.HTTP.HTTPHostname.Any,
                                      HTTPMethod.GET,
                                      [
                                          URLPathPrefix + "/",
                                          URLPathPrefix + "/{FileName}"
                                      ],
                                      HTTPDelegate: Request => {
                                          return Task.FromResult(
                                              new HTTPResponse.Builder(Request) {
                                                  HTTPStatusCode  = HTTPStatusCode.OK,
                                                  Server          = httpAPI.HTTPServer.DefaultServerName,
                                                  Date            = Timestamp.Now,
                                                  ContentType     = HTTPContentType.Text.PLAIN,
                                                  Content         = "This is an OICP v2.3 EMP p2p HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                  CacheControl    = "public, max-age=300",
                                                  Connection      = "close"
                                              }.AsImmutable);
                                      });

            if (AutoStart)
                httpAPI.Start();

        }

        #endregion

        #region EMPPeer(ProviderId, HTTPAPI, KeyPair = null, ...)

        /// <summary>
        /// Attach an EMP p2p service to the given HTTP API.
        /// </summary>
        public EMPPeer(Provider_Id                           ProviderId,
                       HTTPAPI                               HTTPAPI,
                       AsymmetricCipherKeyPair?              KeyPair                            = null,

                       HTTPHostname?                         HTTPHostname                       = null,
                       String?                               ExternalDNSName                    = null,
                       HTTPPath?                             BasePath                           = null,

                       HTTPPath?                             URLPathPrefix                      = null,
                       String?                               HTMLTemplate                       = null,
                       JObject?                              APIVersionHashes                   = null)

            : base(KeyPair)

        {

            this.ProviderId    = ProviderId;
            this.httpAPI       = HTTPAPI;
            this.CPOClientAPI  = new CPOClientAPI(httpAPI);
            this.empClients    = [];
            this.Counters      = new APICounters();

            if (URLPathPrefix.HasValue)
                httpAPI.AddMethodCallback(org.GraphDefined.Vanaheimr.Hermod.HTTP.HTTPHostname.Any,
                                          HTTPMethod.GET,
                                          [
                                              URLPathPrefix + "/",
                                              URLPathPrefix + "/{FileName}"
                                          ],
                                          HTTPDelegate: Request => {
                                              return Task.FromResult(
                                                  new HTTPResponse.Builder(Request) {
                                                      HTTPStatusCode  = HTTPStatusCode.OK,
                                                      Server          = httpAPI.HTTPServer.DefaultServerName,
                                                      Date            = Timestamp.Now,
                                                      ContentType     = HTTPContentType.Text.PLAIN,
                                                      Content         = "This is an OICP v2.3 EMP p2p HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                      CacheControl    = "public, max-age=300",
                                                      Connection      = "close"
                                                  }.AsImmutable);
                                          });

        }

        #endregion

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

        #region GetEMPClient   (OperatorId)

        public EMPClient? GetEMPClient(Operator_Id OperatorId)
        {
            lock (empClients)
            {
                return empClients.GetValueOrDefault(OperatorId);
            }
        }

        #endregion

        #region TryGetEMPClient(OperatorId, out EMPClient)

        public Boolean TryGetEMPClient(Operator_Id OperatorId, out EMPClient? EMPClient)
        {
            lock (empClients)
            {

                if (empClients.TryGetValue(OperatorId, out EMPClient))
                    return true;

                return false;

            }
        }

        #endregion


        #region PullEVSEData          (OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">A PullEVSEData request.</param>
        public async Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(Operator_Id          OperatorId,
                         PullEVSEDataRequest  Request)

        {

            #region Send OnPullEVSEDataRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEData.IncRequests_OK();

            try
            {

                if (OnPullEVSEDataRequest is not null)
                    await Task.WhenAll(OnPullEVSEDataRequest.GetInvocationList().
                                       Cast<OnPullEVSEDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEDataRequest));
            }

            #endregion


            var processId = Process_Id.NewRandom();

            OICPResult<PullEVSEDataResponse> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.PullEVSEData(Request);

            }

            else
                result = OICPResult<PullEVSEDataResponse>.Failed(
                             Request,
                             new PullEVSEDataResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 processId,
                                 TimeSpan.FromMilliseconds(23),
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             ),
                             processId
                         );


            if (result.IsSuccessful)
                Counters.PullEVSEData.IncResponses_OK();
            else
                Counters.PullEVSEData.IncResponses_Error();

            #region Send OnPullEVSEDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEDataResponse is not null)
                    await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
                                       Cast<OnPullEVSEDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatus        (OperatorId, Request)

        /// <summary>
        /// Download EVSE status records.
        /// The request might have an optional search radius and/or status filter.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public async Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(Operator_Id            OperatorId,
                           PullEVSEStatusRequest  Request)

        {

            #region Send OnPullEVSEStatusRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEStatus.IncRequests_OK();

            try
            {

                if (OnPullEVSEStatusRequest is not null)
                    await Task.WhenAll(OnPullEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPullEVSEStatusRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusRequest));
            }

            #endregion


            var processId = Process_Id.NewRandom();

            OICPResult<PullEVSEStatusResponse> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.PullEVSEStatus(Request);

            }

            else
                result = OICPResult<PullEVSEStatusResponse>.Failed(
                             Request,
                             new PullEVSEStatusResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 processId,
                                 TimeSpan.FromMilliseconds(23),
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             ),
                             processId
                         );


            if (result.IsSuccessful)
                Counters.PullEVSEStatus.IncResponses_OK();
            else
                Counters.PullEVSEStatus.IncResponses_Error();

            #region Send OnPullEVSEStatusResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEStatusResponse is not null)
                    await Task.WhenAll(OnPullEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPullEVSEStatusResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatusById    (OperatorId, Request)

        /// <summary>
        /// Download the current status of up to 100 EVSEs.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public async Task<OICPResult<PullEVSEStatusByIdResponse>>

            PullEVSEStatusById(Operator_Id                OperatorId,
                               PullEVSEStatusByIdRequest  Request)

        {

            #region Send OnPullEVSEStatusByIdRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEStatusById.IncRequests_OK();

            try
            {

                if (OnPullEVSEStatusByIdRequest is not null)
                    await Task.WhenAll(OnPullEVSEStatusByIdRequest.GetInvocationList().
                                       Cast<OnPullEVSEStatusByIdRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdRequest));
            }

            #endregion


            var processId = Process_Id.NewRandom();

            OICPResult<PullEVSEStatusByIdResponse> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.PullEVSEStatusById(Request);

            }

            else
                result = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                             Request,
                             new PullEVSEStatusByIdResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 processId,
                                 TimeSpan.FromMilliseconds(23),
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             ),
                             processId
                         );


            if (result.IsSuccessful)
                Counters.PullEVSEStatusById.IncResponses_OK();
            else
                Counters.PullEVSEStatusById.IncResponses_Error();

            #region Send OnPullEVSEStatusByIdResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEStatusByIdResponse is not null)
                    await Task.WhenAll(OnPullEVSEStatusByIdResponse.GetInvocationList().
                                       Cast<OnPullEVSEStatusByIdResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PullPricingProductData(OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">A PullPricingProductData request.</param>
        public async Task<OICPResult<PullPricingProductDataResponse>>

            PullPricingProductData(Operator_Id                    OperatorId,
                                   PullPricingProductDataRequest  Request)

        {

            #region Send OnPullPricingProductDataRequest event

            var startTime = Timestamp.Now;

            Counters.PullPricingProductData.IncRequests_OK();

            try
            {

                if (OnPullPricingProductDataRequest is not null)
                    await Task.WhenAll(OnPullPricingProductDataRequest.GetInvocationList().
                                       Cast<OnPullPricingProductDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullPricingProductDataRequest));
            }

            #endregion


            var processId = Process_Id.NewRandom();

            OICPResult<PullPricingProductDataResponse> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.PullPricingProductData(Request);

            }

            else
                result = OICPResult<PullPricingProductDataResponse>.Failed(
                             Request,
                             new PullPricingProductDataResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 processId,
                                 TimeSpan.FromMilliseconds(23),
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             ),
                             processId
                         );


            if (result.IsSuccessful)
                Counters.PullPricingProductData.IncResponses_OK();
            else
                Counters.PullPricingProductData.IncResponses_Error();

            #region Send OnPullPricingProductDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullPricingProductDataResponse is not null)
                    await Task.WhenAll(OnPullPricingProductDataResponse.GetInvocationList().
                                       Cast<OnPullPricingProductDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullPricingProductDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEPricing       (OperatorId, Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">A PullEVSEPricing request.</param>
        public async Task<OICPResult<PullEVSEPricingResponse>>

            PullEVSEPricing(Operator_Id             OperatorId,
                            PullEVSEPricingRequest  Request)

        {

            #region Send OnPullEVSEPricingRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEPricing.IncRequests_OK();

            try
            {

                if (OnPullEVSEPricingRequest is not null)
                    await Task.WhenAll(OnPullEVSEPricingRequest.GetInvocationList().
                                       Cast<OnPullEVSEPricingRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEPricingRequest));
            }

            #endregion


            var processId = Process_Id.NewRandom();

            OICPResult<PullEVSEPricingResponse> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.PullEVSEPricing(Request);

            }

            else
                result = OICPResult<PullEVSEPricingResponse>.Failed(
                             Request,
                             new PullEVSEPricingResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 processId,
                                 TimeSpan.FromMilliseconds(23),
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             ),
                             processId
                         );


            if (result.IsSuccessful)
                Counters.PullEVSEPricing.IncResponses_OK();
            else
                Counters.PullEVSEPricing.IncResponses_Error();

            #region Send OnPullEVSEPricingResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEPricingResponse is not null)
                    await Task.WhenAll(OnPullEVSEPricingResponse.GetInvocationList().
                                       Cast<OnPullEVSEPricingResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnPullEVSEPricingResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PushAuthenticationData(OperatorId, Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public async Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(Operator_Id                    OperatorId,
                                   PushAuthenticationDataRequest  Request)

        {

            #region Send OnPushAuthenticationDataRequest event

            var startTime = Timestamp.Now;

            Counters.PushAuthenticationData.IncRequests_OK();

            try
            {

                if (OnPushAuthenticationDataRequest is not null)
                    await Task.WhenAll(OnPushAuthenticationDataRequest.GetInvocationList().
                                       Cast<OnPushAuthenticationDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnPushAuthenticationDataRequest));
            }

            #endregion


            OICPResult<Acknowledgement<PushAuthenticationDataRequest>> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.PushAuthenticationData(Request);

            }

            else
                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                             Request,
                             Acknowledgement<PushAuthenticationDataRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.PushAuthenticationData.IncResponses_OK();
            else
                Counters.PushAuthenticationData.IncResponses_Error();

            #region Send OnPushAuthenticationDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushAuthenticationDataResponse is not null)
                    await Task.WhenAll(OnPushAuthenticationDataResponse.GetInvocationList().
                                       Cast<OnPushAuthenticationDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnPushAuthenticationDataResponse));
            }

            #endregion

            return result;

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

            #region Send OnAuthorizeRemoteReservationStartRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteReservationStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
            }

            #endregion


            OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>> result;

            if (empClients.TryGetValue(Request.EVSEId.OperatorId, out EMPClient? empClient))
            {

                result = await empClient.AuthorizeRemoteReservationStart(Request);

            }

            else
                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteReservationStartRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.AuthorizeRemoteReservationStart.IncResponses_OK();
            else
                Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

            #region Send OnAuthorizeRemoteReservationStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
            }

            #endregion

            return result;

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

            #region Send OnAuthorizeRemoteReservationStopRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteReservationStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
            }

            #endregion


            OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>> result;

            if (empClients.TryGetValue(Request.EVSEId.OperatorId, out EMPClient? empClient))
            {

                result = await empClient.AuthorizeRemoteReservationStop(Request);

            }

            else
                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteReservationStopRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.AuthorizeRemoteReservationStop.IncResponses_OK();
            else
                Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

            #region Send OnAuthorizeRemoteReservationStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
            }

            #endregion

            return result;

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


            if (result.IsSuccessful)
                Counters.AuthorizeRemoteStart.IncResponses_OK();
            else
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
                                                     result,
                                                     endtime - startTime))).
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


            if (result.IsSuccessful)
                Counters.AuthorizeRemoteStop.IncResponses_OK();
            else
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
                                                     result,
                                                     endtime - startTime))).
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
        /// <param name="OperatorId">The unique identification of a registered charge point operator.</param>
        /// <param name="Request">A GetChargeDetailRecords request.</param>
        public async Task<OICPResult<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(Operator_Id                    OperatorId,
                                   GetChargeDetailRecordsRequest  Request)

        {

            #region Send OnGetChargeDetailRecordsRequest event

            var startTime = Timestamp.Now;

            Counters.GetChargeDetailRecords.IncRequests_OK();

            try
            {

                if (OnGetChargeDetailRecordsRequest is not null)
                    await Task.WhenAll(OnGetChargeDetailRecordsRequest.GetInvocationList().
                                       Cast<OnGetChargeDetailRecordsRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnGetChargeDetailRecordsRequest));
            }

            #endregion



            var processId = Process_Id.NewRandom();

            OICPResult<GetChargeDetailRecordsResponse> result;

            if (empClients.TryGetValue(OperatorId, out EMPClient? empClient))
            {

                result = await empClient.GetChargeDetailRecords(Request);

            }

            else
                result = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                             Request,
                             new GetChargeDetailRecordsResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 processId,
                                 TimeSpan.FromMilliseconds(23),
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             ),
                             processId
                         );


            if (result.IsSuccessful)
                Counters.GetChargeDetailRecords.IncResponses_OK();
            else
                Counters.GetChargeDetailRecords.IncResponses_Error();

            #region Send OnGetChargeDetailRecordsResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnGetChargeDetailRecordsResponse is not null)
                    await Task.WhenAll(OnGetChargeDetailRecordsResponse.GetInvocationList().
                                       Cast<OnGetChargeDetailRecordsResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPPeer) + "." + nameof(OnGetChargeDetailRecordsResponse));
            }

            #endregion

            return result;

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
                CPOClientAPI?.Dispose();

            foreach (var cpoClient in empClients.Values)
                cpoClient.Dispose();

        }

        #endregion

    }

}
