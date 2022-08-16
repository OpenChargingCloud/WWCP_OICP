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

using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.p2p.CPO
{

    /// <summary>
    /// The CPO p2p combines CPOClient(s) and the EMPClientAPI,
    /// and adds additional logging for all.
    /// </summary>
    public class CPOPeer : APeer, ICPOPeer
    {

        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  PushEVSEData                        { get; }
            public APICounterValues  PushEVSEStatus                      { get; }

            public APICounterValues  PushPricingProductData              { get; }
            public APICounterValues  PushEVSEPricing                     { get; }


            public APICounterValues  PullAuthenticationData              { get; }

            public APICounterValues  AuthorizeStart                      { get; }
            public APICounterValues  AuthorizeStop                       { get; }

            public APICounterValues  SendChargingStartNotification       { get; }
            public APICounterValues  SendChargingProgressNotification    { get; }
            public APICounterValues  SendChargingEndNotification         { get; }
            public APICounterValues  SendChargingErrorNotification       { get; }

            public APICounterValues  SendChargeDetailRecord              { get; }


            public APICounters(APICounterValues? PushEVSEData                       = null,
                               APICounterValues? PushEVSEStatus                     = null,

                               APICounterValues? PushPricingProductData             = null,
                               APICounterValues? PushEVSEPricing                    = null,

                               APICounterValues? PullAuthenticationData             = null,

                               APICounterValues? AuthorizeStart                     = null,
                               APICounterValues? AuthorizeStop                      = null,

                               APICounterValues? SendChargingStartNotification      = null,
                               APICounterValues? SendChargingProgressNotification   = null,
                               APICounterValues? SendChargingEndNotification        = null,
                               APICounterValues? SendChargingErrorNotification      = null,

                               APICounterValues? SendChargeDetailRecord             = null)

            {

                this.PushEVSEData                      = PushEVSEData                     ?? new APICounterValues();
                this.PushEVSEStatus                    = PushEVSEStatus                   ?? new APICounterValues();

                this.PushPricingProductData            = PushPricingProductData           ?? new APICounterValues();
                this.PushEVSEPricing                   = PushEVSEPricing                  ?? new APICounterValues();

                this.PullAuthenticationData            = PullAuthenticationData           ?? new APICounterValues();

                this.AuthorizeStart                    = AuthorizeStart                   ?? new APICounterValues();
                this.AuthorizeStop                     = AuthorizeStop                    ?? new APICounterValues();

                this.SendChargingStartNotification     = SendChargingStartNotification    ?? new APICounterValues();
                this.SendChargingProgressNotification  = SendChargingProgressNotification ?? new APICounterValues();
                this.SendChargingEndNotification       = SendChargingEndNotification      ?? new APICounterValues();
                this.SendChargingErrorNotification     = SendChargingErrorNotification    ?? new APICounterValues();

                this.SendChargeDetailRecord            = SendChargeDetailRecord           ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PushEVSEData",                 PushEVSEData.                ToJSON()),
                       new JProperty("PushEVSEStatus",               PushEVSEStatus.              ToJSON()),

                       new JProperty("PushPricingProductData",       PushPricingProductData.      ToJSON()),
                       new JProperty("PushEVSEPricing",              PushEVSEPricing.             ToJSON()),

                       new JProperty("PullAuthenticationData",       PullAuthenticationData.      ToJSON()),

                       new JProperty("AuthorizeStart",               AuthorizeStart.              ToJSON()),
                       new JProperty("AuthorizeStop",                AuthorizeStop.               ToJSON()),

                       new JProperty("ChargingStartNotification",    SendChargingStartNotification.   ToJSON()),
                       new JProperty("ChargingProgressNotification", SendChargingProgressNotification.ToJSON()),
                       new JProperty("ChargingEndNotification",      SendChargingEndNotification.     ToJSON()),
                       new JProperty("ChargingErrorNotification",    SendChargingErrorNotification.   ToJSON()),

                       new JProperty("SendChargeDetailRecord",       SendChargeDetailRecord.          ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public  const              String                    DefaultHTTPServerName           = "Open Charging Cloud - CPO p2p HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public  const              String                    DefaultHTTPServiceName          = "Open Charging Cloud - CPO p2p HTTP API";

        private readonly HTTPAPI?                            httpAPI;

        private readonly Dictionary<Provider_Id, CPOClient>  cpoClients;

        #endregion

        #region Properties

        public APICounters   Counters        { get; }

        /// <summary>
        /// The EMP client API.
        /// </summary>
        public EMPClientAPI  EMPClientAPI    { get; }

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


        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEData will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate?   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?         OnPushEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?        OnPushEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEData HTTP request had been received.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate?  OnPushEVSEDataResponse;

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEStatus will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate?   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEStatus HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?           OnPushEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEStatus HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?          OnPushEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEStatus HTTP request had been received.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate?  OnPushEVSEStatusResponse;

        #endregion


        #region OnPushPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushPricingProductData will be send.
        /// </summary>
        public event OnPushPricingProductDataRequestDelegate?   OnPushPricingProductDataRequest;

        /// <summary>
        /// An event fired whenever a PushPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPushPricingProductDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPushPricingProductDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event OnPushPricingProductDataResponseDelegate?  OnPushPricingProductDataResponse;

        #endregion

        #region OnPushEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEPricing will be send.
        /// </summary>
        public event OnPushEVSEPricingRequestDelegate?   OnPushEVSEPricingRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?            OnPushEVSEPricingHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?           OnPushEVSEPricingHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event OnPushEVSEPricingResponseDelegate?  OnPushEVSEPricingResponse;

        #endregion


        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullAuthenticationData will be send.
        /// </summary>
        public event OnPullAuthenticationDataRequestDelegate?   OnPullAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a PullAuthenticationData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPullAuthenticationDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PullAuthenticationData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPullAuthenticationDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PullAuthenticationData HTTP request had been received.
        /// </summary>
        public event OnPullAuthenticationDataResponseDelegate?  OnPullAuthenticationDataResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStart request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?     OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?             OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?            OnAuthorizeStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart request had been received.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?    OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStop request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?          OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?         OnAuthorizeStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop request had been received.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?  OnAuthorizeStopResponse;

        #endregion


        #region OnChargingStartNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingStartNotification will be send.
        /// </summary>
        public event OnChargingStartNotificationRequestDelegate?   OnChargingStartNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingStartNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                      OnChargingStartNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                     OnChargingStartNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification had been received.
        /// </summary>
        public event OnChargingStartNotificationResponseDelegate?  OnChargingStartNotificationResponse;

        #endregion

        #region OnChargingProgressNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification will be send.
        /// </summary>
        public event OnChargingProgressNotificationRequestDelegate?   OnChargingProgressNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                         OnChargingProgressNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                        OnChargingProgressNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification had been received.
        /// </summary>
        public event OnChargingProgressNotificationResponseDelegate?  OnChargingProgressNotificationResponse;

        #endregion

        #region OnChargingEndNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingEndNotification will be send.
        /// </summary>
        public event OnChargingEndNotificationRequestDelegate?   OnChargingEndNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingEndNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                    OnChargingEndNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                   OnChargingEndNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification had been received.
        /// </summary>
        public event OnChargingEndNotificationResponseDelegate?  OnChargingEndNotificationResponse;

        #endregion

        #region OnChargingErrorNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification will be send.
        /// </summary>
        public event OnChargingErrorNotificationRequestDelegate?   OnChargingErrorNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                      OnChargingErrorNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                     OnChargingErrorNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification had been received.
        /// </summary>
        public event OnChargingErrorNotificationResponseDelegate?  OnChargingErrorNotificationResponse;

        #endregion


        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestDelegate?   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnSendChargeDetailRecordHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargeDetailRecord HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnSendChargeDetailRecordHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargeDetailRecord had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseDelegate?  OnSendChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO p2p service.
        /// </summary>
        public CPOPeer(AsymmetricCipherKeyPair?              KeyPair                            = null,

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
                                                             Content         = "This is an OICP v2.3 CPO p2p HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                             CacheControl    = "public, max-age=300",
                                                             Connection      = "close"
                                                         }.AsImmutable);
                                                 });

            this.EMPClientAPI  = new EMPClientAPI(httpAPI);
            this.cpoClients    = new Dictionary<Provider_Id, CPOClient>();
            this.Counters      = new APICounters();

            if (Autostart)
                httpAPI.Start();

        }

        #endregion


        #region RegisterProvider(ProviderId, CPOClient)

        /// <summary>
        /// Register the given CPOClient for the given e-mobility provider identification.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="CPOClient">A CPO client.</param>
        public Boolean RegisterProvider(Provider_Id  ProviderId,
                                        CPOClient    CPOClient)
        {
            lock (cpoClients)
            {
                return cpoClients.TryAdd(ProviderId, CPOClient);
            }
        }

        #endregion

        #region GetCPOClient(ProviderId)

        public CPOClient? GetCPOClient(Provider_Id ProviderId)
        {
            lock (cpoClients)
            {
                return cpoClients.GetValueOrDefault(ProviderId);
            }
        }

        #endregion


        #region PushEVSEData                    (            Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>[]>

            PushEVSEData(PushEVSEDataRequest  Request)

        {

            if (cpoClients.Any())
                return await Task.WhenAll(cpoClients.Values.Select(cpoClient => cpoClient.PushEVSEData(Request)));

            return new OICPResult<Acknowledgement<PushEVSEDataRequest>>[] {
                       OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                              Request,
                              Acknowledgement<PushEVSEDataRequest>.NoValidContract(
                                  Request,
                                  "No e-mobility providers registered!"
                              )
                          )
                   };

        }

        #endregion

        #region PushEVSEData                    (ProviderId, Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(Provider_Id          ProviderId,
                         PushEVSEDataRequest  Request)

        {

            #region Send OnPushEVSEDataRequest event

            var startTime = Timestamp.Now;

            Counters.PushEVSEData.IncRequests_OK();

            try
            {

                if (OnPushEVSEDataRequest is not null)
                    await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                       Cast<OnPushEVSEDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushEVSEDataRequest));
            }

            #endregion


            OICPResult<Acknowledgement<PushEVSEDataRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.PushEVSEData(Request);

            }

            else
                result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                             Request,
                             Acknowledgement<PushEVSEDataRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.PushEVSEData.IncResponses_OK();
            else
                Counters.PushEVSEData.IncResponses_Error();

            #region Send OnPushEVSEDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEDataResponse is not null)
                    await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                       Cast<OnPushEVSEDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEStatus                  (            Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>[]>

            PushEVSEStatus(PushEVSEStatusRequest  Request)

        {

            if (cpoClients.Any())
                return await Task.WhenAll(cpoClients.Values.Select(cpoClient => cpoClient.PushEVSEStatus(Request)));

            return new OICPResult<Acknowledgement<PushEVSEStatusRequest>>[] {
                       OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                              Request,
                              Acknowledgement<PushEVSEStatusRequest>.NoValidContract(
                                  Request,
                                  "No e-mobility providers registered!"
                              )
                          )
                   };

        }

        #endregion

        #region PushEVSEStatus                  (ProviderId, Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(Provider_Id            ProviderId,
                           PushEVSEStatusRequest  Request)

        {

            #region Send OnPushEVSEStatusRequest event

            var startTime = Timestamp.Now;

            Counters.PushEVSEStatus.IncRequests_OK();

            try
            {

                if (OnPushEVSEStatusRequest is not null)
                    await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPushEVSEStatusRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            OICPResult<Acknowledgement<PushEVSEStatusRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.PushEVSEStatus(Request);

            }

            else
                result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                             Request,
                             Acknowledgement<PushEVSEStatusRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.PushEVSEStatus.IncResponses_OK();
            else
                Counters.PushEVSEStatus.IncResponses_Error();

            #region Send OnPushEVSEStatusResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEStatusResponse is not null)
                    await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPushEVSEStatusResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PushPricingProductData          (            Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>[]>

            PushPricingProductData(PushPricingProductDataRequest  Request)

        {

            if (cpoClients.Any())
                return await Task.WhenAll(cpoClients.Values.Select(cpoClient => cpoClient.PushPricingProductData(Request)));

            return new OICPResult<Acknowledgement<PushPricingProductDataRequest>>[] {
                       OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                              Request,
                              Acknowledgement<PushPricingProductDataRequest>.NoValidContract(
                                  Request,
                                  "No e-mobility providers registered!"
                              )
                          )
                   };

        }

        #endregion

        #region PushPricingProductData          (ProviderId, Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>

            PushPricingProductData(Provider_Id                    ProviderId,
                                   PushPricingProductDataRequest  Request)

        {

            #region Send OnPushPricingProductDataRequest event

            var startTime = Timestamp.Now;

            Counters.PushPricingProductData.IncRequests_OK();

            try
            {

                if (OnPushPricingProductDataRequest is not null)
                    await Task.WhenAll(OnPushPricingProductDataRequest.GetInvocationList().
                                       Cast<OnPushPricingProductDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushPricingProductDataRequest));
            }

            #endregion


            OICPResult<Acknowledgement<PushPricingProductDataRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.PushPricingProductData(Request);

            }

            else
                result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                             Request,
                             Acknowledgement<PushPricingProductDataRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.PushPricingProductData.IncResponses_OK();
            else
                Counters.PushPricingProductData.IncResponses_Error();

            #region Send OnPushPricingProductDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushPricingProductDataResponse is not null)
                    await Task.WhenAll(OnPushPricingProductDataResponse.GetInvocationList().
                                       Cast<OnPushPricingProductDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushPricingProductDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEPricing                 (            Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>[]>

            PushEVSEPricing(PushEVSEPricingRequest  Request)

        {

            if (cpoClients.Any())
                return await Task.WhenAll(cpoClients.Values.Select(cpoClient => cpoClient.PushEVSEPricing(Request)));

            return new OICPResult<Acknowledgement<PushEVSEPricingRequest>>[] {
                       OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                              Request,
                              Acknowledgement<PushEVSEPricingRequest>.NoValidContract(
                                  Request,
                                  "No e-mobility providers registered!"
                              )
                          )
                   };

        }

        #endregion

        #region PushEVSEPricing                 (ProviderId, Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>

            PushEVSEPricing(Provider_Id             ProviderId,
                            PushEVSEPricingRequest  Request)

        {

            #region Send OnPushEVSEPricingRequest event

            var startTime = Timestamp.Now;

            Counters.PushEVSEPricing.IncRequests_OK();

            try
            {

                if (OnPushEVSEPricingRequest is not null)
                    await Task.WhenAll(OnPushEVSEPricingRequest.GetInvocationList().
                                       Cast<OnPushEVSEPricingRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushEVSEPricingRequest));
            }

            #endregion


            OICPResult<Acknowledgement<PushEVSEPricingRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.PushEVSEPricing(Request);

            }

            else
                result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                             Request,
                             Acknowledgement<PushEVSEPricingRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.PushEVSEPricing.IncResponses_OK();
            else
                Counters.PushEVSEPricing.IncResponses_Error();

            #region Send OnPushEVSEPricingResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEPricingResponse is not null)
                    await Task.WhenAll(OnPushEVSEPricingResponse.GetInvocationList().
                                       Cast<OnPushEVSEPricingResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPushEVSEPricingResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PullAuthenticationData          (ProviderId, Request)

        /// <summary>
        /// Download provider authentication data.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A PullAuthenticationData request.</param>
        public async Task<OICPResult<PullAuthenticationDataResponse>>

            PullAuthenticationData(Provider_Id                    ProviderId,
                                   PullAuthenticationDataRequest  Request)

        {

            #region Send OnPullAuthenticationDataRequest event

            var startTime = Timestamp.Now;

            Counters.PullAuthenticationData.IncRequests_OK();

            try
            {

                if (OnPullAuthenticationDataRequest is not null)
                    await Task.WhenAll(OnPullAuthenticationDataRequest.GetInvocationList().
                                       Cast<OnPullAuthenticationDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPullAuthenticationDataRequest));
            }

            #endregion


            OICPResult<PullAuthenticationDataResponse> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.PullAuthenticationData(Request);

            }

            else
                result = OICPResult<PullAuthenticationDataResponse>.Failed(
                             Request,
                             new PullAuthenticationDataResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 TimeSpan.FromMilliseconds(23),
                                 Array.Empty<ProviderAuthenticationData>(),
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.NoValidContract,
                                                 "Unknown e-mobility provider!"
                                             )
                             )
                         );


            if (result.IsSuccessful)
                Counters.PullAuthenticationData.IncResponses_OK();
            else
                Counters.PullAuthenticationData.IncResponses_Error();

            #region Send OnPullAuthenticationDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullAuthenticationDataResponse is not null)
                    await Task.WhenAll(OnPullAuthenticationDataResponse.GetInvocationList().
                                       Cast<OnPullAuthenticationDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnPullAuthenticationDataResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeStart                  (            Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(AuthorizeStartRequest  Request)

        {

            if (cpoClients.Any())
            {

                var responses = await Task.WhenAll(cpoClients.Values.Select(cpoClient => cpoClient.AuthorizeStart(Request)));

                var success   = responses.First(response => response.IsSuccessful);

                if (success is not null)
                    return success;

                return responses.First();

            }

            return OICPResult<AuthorizationStartResponse>.Failed(
                       Request,
                       AuthorizationStartResponse.NotAuthorized(
                           Request,
                           new StatusCode(
                               StatusCodes.NoValidContract,
                               "No e-mobility providers registered!"
                           )
                       )
                   );

        }

        #endregion

        #region AuthorizeStart                  (ProviderId, Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(Provider_Id            ProviderId,
                           AuthorizeStartRequest  Request)

        {

            #region Send OnAuthorizeStartRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            OICPResult<AuthorizationStartResponse> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.AuthorizeStart(Request);

            }

            else
                result = OICPResult<AuthorizationStartResponse>.Failed(
                             Request,
                             AuthorizationStartResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.NoValidContract,
                                     "Unknown e-mobility provider!"
                                 )
                             )
                         );


            if (result.IsSuccessful)
                Counters.AuthorizeStart.IncResponses_OK();
            else
                Counters.AuthorizeStart.IncResponses_Error();

            #region Send OnAuthorizeStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop                   (            Request)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(AuthorizeStopRequest  Request)

        {

            if (cpoClients.Any())
            {

                var responses = await Task.WhenAll(cpoClients.Values.Select(cpoClient => cpoClient.AuthorizeStop(Request)));

                var success   = responses.First(response => response.IsSuccessful);

                if (success is not null)
                    return success;

                return responses.First();

            }

            return OICPResult<AuthorizationStopResponse>.Failed(
                       Request,
                       AuthorizationStopResponse.NotAuthorized(
                           Request,
                           new StatusCode(
                               StatusCodes.NoValidContract,
                               "No e-mobility providers registered!"
                           )
                       )
                   );

        }

        #endregion

        #region AuthorizeStop                   (ProviderId, Request)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(Provider_Id           ProviderId,
                          AuthorizeStopRequest  Request)

        {

            #region Send OnAuthorizeStopRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            OICPResult<AuthorizationStopResponse> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.AuthorizeStop(Request);

            }

            else
                result = OICPResult<AuthorizationStopResponse>.Failed(
                             Request,
                             AuthorizationStopResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.NoValidContract,
                                     "Unknown e-mobility provider!"
                                 )
                             )
                         );


            if (result.IsSuccessful)
                Counters.AuthorizeStop.IncResponses_OK();
            else
                Counters.AuthorizeStop.IncResponses_Error();

            #region Send OnAuthorizeStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargingStartNotification   (ProviderId, Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A ChargingStartNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

            SendChargingStartNotification(Provider_Id                       ProviderId,
                                          ChargingStartNotificationRequest  Request)

        {

            #region  OnChargingStartNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingStartNotification.IncRequests_OK();

            try
            {

                if (OnChargingStartNotificationRequest is not null)
                    await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                       Cast<OnChargingStartNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingStartNotificationRequest));
            }

            #endregion


            OICPResult<Acknowledgement<ChargingStartNotificationRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.SendChargingStartNotification(Request);

            }

            else
                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingStartNotificationRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.SendChargingStartNotification.IncResponses_OK();
            else
                Counters.SendChargingStartNotification.IncResponses_Error();

            #region  OnChargingStartNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingStartNotificationResponse is not null)
                    await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                       Cast<OnChargingStartNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingStartNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingProgressNotification(ProviderId, Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A ChargingProgressNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

            SendChargingProgressNotification(Provider_Id                       ProviderId,
                                             ChargingProgressNotificationRequest  Request)

        {

            #region  OnChargingProgressNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingProgressNotification.IncRequests_OK();

            try
            {

                if (OnChargingProgressNotificationRequest is not null)
                    await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                       Cast<OnChargingProgressNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingProgressNotificationRequest));
            }

            #endregion


            OICPResult<Acknowledgement<ChargingProgressNotificationRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.SendChargingProgressNotification(Request);

            }

            else
                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingProgressNotificationRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.SendChargingProgressNotification.IncResponses_OK();
            else
                Counters.SendChargingProgressNotification.IncResponses_Error();

            #region  OnChargingProgressNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingProgressNotificationResponse is not null)
                    await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                       Cast<OnChargingProgressNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingProgressNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingEndNotification     (ProviderId, Request)

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A ChargingEndNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

            SendChargingEndNotification(Provider_Id                     ProviderId,
                                        ChargingEndNotificationRequest  Request)

        {

            #region  OnChargingEndNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingEndNotification.IncRequests_OK();

            try
            {

                if (OnChargingEndNotificationRequest is not null)
                    await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                       Cast<OnChargingEndNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingEndNotificationRequest));
            }

            #endregion


            OICPResult<Acknowledgement<ChargingEndNotificationRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.SendChargingEndNotification(Request);

            }

            else
                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingEndNotificationRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.SendChargingEndNotification.IncResponses_OK();
            else
                Counters.SendChargingEndNotification.IncResponses_Error();

            #region  OnChargingEndNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingEndNotificationResponse is not null)
                    await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                       Cast<OnChargingEndNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingEndNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingErrorNotification   (ProviderId, Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A ChargingErrorNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>

            SendChargingErrorNotification(Provider_Id                       ProviderId,
                                          ChargingErrorNotificationRequest  Request)

        {

            #region  OnChargingErrorNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingErrorNotification.IncRequests_OK();

            try
            {

                if (OnChargingErrorNotificationRequest is not null)
                    await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                       Cast<OnChargingErrorNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingErrorNotificationRequest));
            }

            #endregion


            OICPResult<Acknowledgement<ChargingErrorNotificationRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.SendChargingErrorNotification(Request);

            }

            else
                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingErrorNotificationRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.SendChargingErrorNotification.IncResponses_OK();
            else
                Counters.SendChargingErrorNotification.IncResponses_Error();

            #region  OnChargingErrorNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingErrorNotificationResponse is not null)
                    await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                       Cast<OnChargingErrorNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnChargingErrorNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord          (ProviderId, Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ProviderId">The unique identification of a registered e-mobility provider.</param>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

            SendChargeDetailRecord(Provider_Id                ProviderId,
                                   ChargeDetailRecordRequest  Request)

        {

            #region Send OnSendChargeDetailRecord event

            var startTime = Timestamp.Now;

            Counters.SendChargeDetailRecord.IncRequests_OK();

            try
            {

                if (OnSendChargeDetailRecordRequest is not null)
                    await Task.WhenAll(OnSendChargeDetailRecordRequest.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion


            OICPResult<Acknowledgement<ChargeDetailRecordRequest>> result;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {

                result = await cpoClient.SendChargeDetailRecord(Request);

            }

            else
                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargeDetailRecordRequest>.NoValidContract(
                                 Request,
                                 "Unknown e-mobility provider!"
                             )
                         );


            if (result.IsSuccessful)
                Counters.SendChargeDetailRecord.IncResponses_OK();
            else
                Counters.SendChargeDetailRecord.IncResponses_Error();

            #region Send OnChargeDetailRecordSent event

            var endtime = Timestamp.Now;

            try
            {

                if (OnSendChargeDetailRecordResponse is not null)
                    await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOPeer) + "." + nameof(OnSendChargeDetailRecordResponse));
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
                EMPClientAPI.Start();
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
                EMPClientAPI.Shutdown(Message, Wait);
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

                if (EMPClientAPI is not null)
                    EMPClientAPI.Dispose();

            }

            foreach (var cpoClient in cpoClients.Values)
                cpoClient.Dispose();

        }

        #endregion

    }

}
