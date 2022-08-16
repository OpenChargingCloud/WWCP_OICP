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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.EMP;
using System.Diagnostics;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.p2p.CPO
{

    /// <summary>
    /// The CPO p2p combines CPOClient(s) and the EMPClientAPI,
    /// and adds additional logging for all.
    /// </summary>
    public class CPOPeer : APeer, ICPOPeer
    {

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

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO p2p service.
        /// </summary>
        public CPOPeer(HTTPHostname?                         HTTPHostname                       = null,
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


        #region PushEVSEData                    (ProviderId, Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(Provider_Id          ProviderId,
                         PushEVSEDataRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.PushEVSEData(Request);
            }

            return OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                       Request,
                       Acknowledgement<PushEVSEDataRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion

        #region PushEVSEStatus                  (ProviderId, Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(Provider_Id          ProviderId,
                         PushEVSEStatusRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.PushEVSEStatus(Request);
            }

            return OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                       Request,
                       Acknowledgement<PushEVSEStatusRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion


        #region PushPricingProductData          (ProviderId, Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>

            PushPricingProductData(Provider_Id                    ProviderId,
                                   PushPricingProductDataRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.PushPricingProductData(Request);
            }

            return OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                       Request,
                       Acknowledgement<PushPricingProductDataRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion

        #region PushEVSEPricing                 (ProviderId, Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>

            PushEVSEPricing(Provider_Id             ProviderId,
                            PushEVSEPricingRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.PushEVSEPricing(Request);
            }

            return OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                       Request,
                       Acknowledgement<PushEVSEPricingRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion


        #region PullAuthenticationData          (ProviderId, Request)

        /// <summary>
        /// Download provider authentication data.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A PullAuthenticationData request.</param>
        public async Task<OICPResult<PullAuthenticationDataResponse>>

            PullAuthenticationData(Provider_Id                    ProviderId,
                                   PullAuthenticationDataRequest  Request)

        {

            var processId = Process_Id.NewRandom;

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.PullAuthenticationData(Request);
            }

            return OICPResult<PullAuthenticationDataResponse>.Failed(
                       Request,
                       new PullAuthenticationDataResponse(
                           Timestamp.Now,
                           Request.EventTrackingId ?? EventTracking_Id.New,
                           processId,
                           TimeSpan.FromMilliseconds(23),
                           Array.Empty<ProviderAuthenticationData>(),
                           Request,
                           StatusCode: new StatusCode(
                                           StatusCodes.NoValidContract,
                                           "Unknown e-mobility provider!"
                                       )
                       )
                   );

        }

        #endregion


        #region AuthorizeStart                  (ProviderId, Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(Provider_Id            ProviderId,
                           AuthorizeStartRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.AuthorizeStart(Request);
            }

            return OICPResult<AuthorizationStartResponse>.Failed(
                       Request,
                       AuthorizationStartResponse.NotAuthorized(
                           Request,
                           new StatusCode(
                               StatusCodes.NoValidContract,
                               "Unknown e-mobility provider!"
                           )
                       )
                   );

        }

        #endregion

        #region AuthorizeStop                   (ProviderId, Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(Provider_Id           ProviderId,
                          AuthorizeStopRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.AuthorizeStop(Request);
            }

            return OICPResult<AuthorizationStopResponse>.Failed(
                       Request,
                       AuthorizationStopResponse.NotAuthorized(
                           Request,
                           new StatusCode(
                               StatusCodes.NoValidContract,
                               "Unknown e-mobility provider!"
                           )
                       )
                   );

        }

        #endregion


        #region SendChargingStartNotification   (ProviderId, Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A ChargingStartNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

            SendChargingStartNotification(Provider_Id                       ProviderId,
                                          ChargingStartNotificationRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.SendChargingStartNotification(Request);
            }

            return OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                       Request,
                       Acknowledgement<ChargingStartNotificationRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion

        #region SendChargingProgressNotification(ProviderId, Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A ChargingProgressNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

            SendChargingProgressNotification(Provider_Id                       ProviderId,
                                             ChargingProgressNotificationRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.SendChargingProgressNotification(Request);
            }

            return OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                       Request,
                       Acknowledgement<ChargingProgressNotificationRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion

        #region SendChargingEndNotification     (ProviderId, Request)

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A ChargingEndNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

            SendChargingEndNotification(Provider_Id                     ProviderId,
                                        ChargingEndNotificationRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.SendChargingEndNotification(Request);
            }

            return OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                       Request,
                       Acknowledgement<ChargingEndNotificationRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion

        #region SendChargingErrorNotification   (ProviderId, Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A ChargingErrorNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>

            SendChargingErrorNotification(Provider_Id                       ProviderId,
                                          ChargingErrorNotificationRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.SendChargingErrorNotification(Request);
            }

            return OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                       Request,
                       Acknowledgement<ChargingErrorNotificationRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
                   );

        }

        #endregion


        #region SendChargeDetailRecord          (ProviderId, Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Provider">A registered e-mobility provider.</param>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

            SendChargeDetailRecord(Provider_Id                ProviderId,
                                   ChargeDetailRecordRequest  Request)

        {

            if (cpoClients.TryGetValue(ProviderId, out CPOClient? cpoClient))
            {
                return await cpoClient.SendChargeDetailRecord(Request);
            }

            return OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                       Request,
                       Acknowledgement<ChargeDetailRecordRequest>.NoValidContract(
                           Request,
                           "Unknown e-mobility provider!"
                       )
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
