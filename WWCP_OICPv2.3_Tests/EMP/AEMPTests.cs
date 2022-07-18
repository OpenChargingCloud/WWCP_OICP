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

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.SMTP;
using org.GraphDefined.Vanaheimr.Illias;

using social.OpenData.UsersAPI;

using WWCP = org.GraphDefined.WWCP;
using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP.tests
{

    /// <summary>
    /// OICP EMP test defaults.
    /// </summary>
    public abstract class AEMPTests
    {

        #region (class) NotificationReceiverAPI

        /// <summary>
        /// A HTTP(S) notification test receiver.
        /// </summary>
        public class NotificationReceiverAPI : HTTPAPI
        {

            #region Properties

            public List<HTTPRequest>  FailedHTTPRequests             { get; }
            public List<JObject>      ReceviedAdminNotifications     { get; }
            public List<JObject>      ReceviedMemberNotifications    { get; }

            #endregion

            #region Constructor(s)

            public NotificationReceiverAPI()

                : base(HTTPHostname:                       null,
                       ExternalDNSName:                    null,
                       HTTPServerPort:                     IPPort.Parse(24949),
                       BasePath:                           null,
                       HTTPServerName:                     "Notification Receiver API",

                       URLPathPrefix:                      null,
                       HTTPServiceName:                    "Notification Receiver API",
                       HTMLTemplate:                       null,
                       APIVersionHashes:                   null,

                       ServerCertificateSelector:          null,
                       ClientCertificateValidator:         null,
                       ClientCertificateSelector:          null,
                       AllowedTLSProtocols:                null,

                       ServerThreadName:                   null,
                       ServerThreadPriority:               null,
                       ServerThreadIsBackground:           null,
                       ConnectionIdBuilder:                null,
                       ConnectionThreadsNameBuilder:       null,
                       ConnectionThreadsPriorityBuilder:   null,
                       ConnectionThreadsAreBackground:     null,
                       ConnectionTimeout:                  null,
                       MaxClientConnections:               null,

                       DisableMaintenanceTasks:            true,
                       MaintenanceInitialDelay:            null,
                       MaintenanceEvery:                   null,

                       DisableWardenTasks:                 true,
                       WardenInitialDelay:                 null,
                       WardenCheckEvery:                   null,

                       IsDevelopment:                      null,
                       DevelopmentServers:                 null,
                       DisableLogging:                     true,
                       LoggingPath:                        null,
                       LogfileName:                        null,
                       LogfileCreator:                     null,
                       DNSClient:                          null,
                       Autostart:                          true)

            {

                this.FailedHTTPRequests           = new List<HTTPRequest>();
                this.ReceviedAdminNotifications   = new List<JObject>();
                this.ReceviedMemberNotifications  = new List<JObject>();


                #region POST  ~/adminLogs

                HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                             HTTPMethod.POST,
                                             URLPathPrefix + "/adminLogs",
                                             HTTPDelegate: Request => {

                                                 if (Request.API_Key.HasValue == false || Request.API_Key.Value.ToString() != "39cn5t235t")
                                                 {
                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.Unauthorized,
                                                             Connection      = "close"
                                                         }.AsImmutable);
                                                 }

                                                 if (!Request.TryParseJArrayRequestBody(out JArray                Notifications,
                                                                                        out HTTPResponse.Builder  httpResponse,
                                                                                        AllowEmptyHTTPBody: false))
                                                 {
                                                     FailedHTTPRequests.Add(Request);
                                                     return Task.FromResult(httpResponse.AsImmutable);
                                                 }

                                                 foreach (var notification in Notifications)
                                                 {
                                                     if (notification is JObject json)
                                                        ReceviedAdminNotifications.Add(json);
                                                 }

                                                 return Task.FromResult(
                                                     new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode  = HTTPStatusCode.OK,
                                                         Connection      = "close"
                                                     }.AsImmutable);

                                             });

                #endregion

                #region POST  ~/memberLogs

                HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                             HTTPMethod.POST,
                                             URLPathPrefix + "/memberLogs",
                                             HTTPDelegate: Request => {

                                                 if (!(Request.Authorization is HTTPBasicAuthentication basicAuth) ||
                                                     basicAuth.Username != "empServerAPIMember01" ||
                                                     basicAuth.Password != "h3f0g4wh0j")
                                                 {
                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.Unauthorized,
                                                             Connection      = "close"
                                                         }.AsImmutable);
                                                 }

                                                 if (!Request.TryParseJArrayRequestBody(out JArray                Notifications,
                                                                                        out HTTPResponse.Builder  httpResponse,
                                                                                        AllowEmptyHTTPBody: false))
                                                 {
                                                     FailedHTTPRequests.Add(Request);
                                                     return Task.FromResult(httpResponse.AsImmutable);
                                                 }

                                                 foreach (var notification in Notifications)
                                                 {
                                                     if (notification is JObject json)
                                                        ReceviedMemberNotifications.Add(json);
                                                 }

                                                 return Task.FromResult(
                                                     new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode  = HTTPStatusCode.OK,
                                                         Connection      = "close"
                                                     }.AsImmutable);

                                             });

                #endregion

            }

            #endregion


            #region ClearNotifications()

            public void ClearNotifications()
            {
                this.ReceviedAdminNotifications. Clear();
                this.ReceviedMemberNotifications.Clear();
            }

            #endregion

        }

        #endregion


        #region Data

        protected EMPServerAPI              empServerAPI;

        protected NotificationReceiverAPI   NotificationAPI;

        #endregion

        #region Properties


        #endregion


        #region SetupOnce()

        [OneTimeSetUp]
        public void SetupOnce()
        {

        }

        #endregion

        #region SetupEachTest()

        [SetUp]
        public async Task SetupEachTest()
        {

            #region Create OICPAPI

            Timestamp.Reset();

            empServerAPI = new EMPServerAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8000),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            empServerAPI.OnAuthorizeStart += (timestamp, sender, authorizeStartRequest) => {

                if (authorizeStartRequest.Identification is not null)
                {

                    if (authorizeStartRequest.Identification.RFIDId is not null)
                    {
                        return authorizeStartRequest.Identification.RFIDId.ToString() switch
                        {

                            "AABBCCDD" => Task.FromResult(AuthorizationStartResponse.Authorized   (Request:                           authorizeStartRequest,
                                                                                                   SessionId:                         authorizeStartRequest.SessionId,
                                                                                                   CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                                                                                   EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                                                                                   ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                   StatusCodeDescription:             null,
                                                                                                   StatusCodeAdditionalInfo:          null,
                                                                                                   AuthorizationStopIdentifications:  new Identification[] {
                                                                                                                                          Identification.FromUID(UID.Parse("11223344")),
                                                                                                                                          Identification.FromUID(UID.Parse("55667788"))
                                                                                                                                      },
                                                                                                   ResponseTimestamp:                 Timestamp.Now,
                                                                                                   EventTrackingId:                   EventTracking_Id.New,
                                                                                                   Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                   ProcessId:                         Process_Id.NewRandom,
                                                                                                   HTTPResponse:                      null,
                                                                                                   CustomData:                        null)),

                            _          => Task.FromResult(AuthorizationStartResponse.NotAuthorized(Request:                           authorizeStartRequest,
                                                                                                   StatusCode:                        new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                                                                                   SessionId:                         authorizeStartRequest.SessionId,
                                                                                                   CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                                                                                   EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                                                                                   ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                   ResponseTimestamp:                 Timestamp.Now,
                                                                                                   EventTrackingId:                   EventTracking_Id.New,
                                                                                                   Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                   ProcessId:                         Process_Id.NewRandom,
                                                                                                   HTTPResponse:                      null,
                                                                                                   CustomData:                        null))

                        };

                    }


                    if (authorizeStartRequest.Identification.RFIDIdentification is not null)
                    {
                        return authorizeStartRequest.Identification.RFIDIdentification.UID.ToString() switch
                        {

                            "AABBCCDD" => Task.FromResult(AuthorizationStartResponse.Authorized   (Request:                           authorizeStartRequest,
                                                                                                   SessionId:                         authorizeStartRequest.SessionId,
                                                                                                   CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                                                                                   EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                                                                                   ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                   StatusCodeDescription:             null,
                                                                                                   StatusCodeAdditionalInfo:          null,
                                                                                                   AuthorizationStopIdentifications:  new Identification[] {
                                                                                                                                          Identification.FromUID(UID.Parse("11223344")),
                                                                                                                                          Identification.FromUID(UID.Parse("55667788"))
                                                                                                                                      },
                                                                                                   ResponseTimestamp:                 Timestamp.Now,
                                                                                                   EventTrackingId:                   EventTracking_Id.New,
                                                                                                   Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                   ProcessId:                         Process_Id.NewRandom,
                                                                                                   HTTPResponse:                      null,
                                                                                                   CustomData:                        null)),

                            _          => Task.FromResult(AuthorizationStartResponse.NotAuthorized(Request:                           authorizeStartRequest,
                                                                                                   StatusCode:                        new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                                                                                   SessionId:                         authorizeStartRequest.SessionId,
                                                                                                   CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                                                                                   EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                                                                                   ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                   ResponseTimestamp:                 Timestamp.Now,
                                                                                                   EventTrackingId:                   EventTracking_Id.New,
                                                                                                   Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                   ProcessId:                         Process_Id.NewRandom,
                                                                                                   HTTPResponse:                      null,
                                                                                                   CustomData:                        null))

                        };

                    }

                }

                return Task.FromResult(AuthorizationStartResponse.DataError     (Request:                           authorizeStartRequest,
                                                                                 StatusCodeDescription:             "authorizeStartRequest.Identification is null!",
                                                                                 StatusCodeAdditionalInfo:          null,
                                                                                 SessionId:                         authorizeStartRequest.SessionId,
                                                                                 CPOPartnerSessionId:               authorizeStartRequest.CPOPartnerSessionId,
                                                                                 EMPPartnerSessionId:               authorizeStartRequest.EMPPartnerSessionId,
                                                                                 ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                 ResponseTimestamp:                 Timestamp.Now,
                                                                                 EventTrackingId:                   EventTracking_Id.New,
                                                                                 Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                 ProcessId:                         Process_Id.NewRandom,
                                                                                 HTTPResponse:                      null,
                                                                                 CustomData:                        null));

            };

            empServerAPI.OnAuthorizeStop  += (timestamp, sender, authorizeStopRequest)  => {

                if (authorizeStopRequest.Identification is not null)
                {

                    if (authorizeStopRequest.Identification.RFIDId is not null)
                    {
                        return authorizeStopRequest.Identification.RFIDId.ToString() switch
                        {

                            "AABBCCDD" => Task.FromResult(AuthorizationStopResponse.Authorized   (Request:                           authorizeStopRequest,
                                                                                                  SessionId:                         authorizeStopRequest.SessionId,
                                                                                                  CPOPartnerSessionId:               authorizeStopRequest.CPOPartnerSessionId,
                                                                                                  EMPPartnerSessionId:               authorizeStopRequest.EMPPartnerSessionId,
                                                                                                  ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                  StatusCodeDescription:             null,
                                                                                                  StatusCodeAdditionalInfo:          null,
                                                                                                  ResponseTimestamp:                 Timestamp.Now,
                                                                                                  EventTrackingId:                   EventTracking_Id.New,
                                                                                                  Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                  ProcessId:                         Process_Id.NewRandom,
                                                                                                  HTTPResponse:                      null,
                                                                                                  CustomData:                        null)),

                            _          => Task.FromResult(AuthorizationStopResponse.NotAuthorized(Request:                           authorizeStopRequest,
                                                                                                  StatusCode:                        new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                                                                                  SessionId:                         authorizeStopRequest.SessionId,
                                                                                                  CPOPartnerSessionId:               authorizeStopRequest.CPOPartnerSessionId,
                                                                                                  EMPPartnerSessionId:               authorizeStopRequest.EMPPartnerSessionId,
                                                                                                  ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                  ResponseTimestamp:                 Timestamp.Now,
                                                                                                  EventTrackingId:                   EventTracking_Id.New,
                                                                                                  Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                  ProcessId:                         Process_Id.NewRandom,
                                                                                                  HTTPResponse:                      null,
                                                                                                  CustomData:                        null))

                        };

                    }


                    if (authorizeStopRequest.Identification.RFIDIdentification is not null)
                    {
                        return authorizeStopRequest.Identification.RFIDIdentification.UID.ToString() switch
                        {

                            "AABBCCDD" => Task.FromResult(AuthorizationStopResponse.Authorized   (Request:                           authorizeStopRequest,
                                                                                                  SessionId:                         authorizeStopRequest.SessionId,
                                                                                                  CPOPartnerSessionId:               authorizeStopRequest.CPOPartnerSessionId,
                                                                                                  EMPPartnerSessionId:               authorizeStopRequest.EMPPartnerSessionId,
                                                                                                  ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                  StatusCodeDescription:             null,
                                                                                                  StatusCodeAdditionalInfo:          null,
                                                                                                  ResponseTimestamp:                 Timestamp.Now,
                                                                                                  EventTrackingId:                   EventTracking_Id.New,
                                                                                                  Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                  ProcessId:                         Process_Id.NewRandom,
                                                                                                  HTTPResponse:                      null,
                                                                                                  CustomData:                        null)),

                            _          => Task.FromResult(AuthorizationStopResponse.NotAuthorized(Request:                           authorizeStopRequest,
                                                                                                  StatusCode:                        new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                                                                                  SessionId:                         authorizeStopRequest.SessionId,
                                                                                                  CPOPartnerSessionId:               authorizeStopRequest.CPOPartnerSessionId,
                                                                                                  EMPPartnerSessionId:               authorizeStopRequest.EMPPartnerSessionId,
                                                                                                  ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                                  ResponseTimestamp:                 Timestamp.Now,
                                                                                                  EventTrackingId:                   EventTracking_Id.New,
                                                                                                  Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                                  ProcessId:                         Process_Id.NewRandom,
                                                                                                  HTTPResponse:                      null,
                                                                                                  CustomData:                        null))

                        };

                    }

                }

                return Task.FromResult(AuthorizationStopResponse.DataError     (Request:                           authorizeStopRequest,
                                                                                StatusCodeDescription:             "authorizeStopRequest.Identification is null!",
                                                                                StatusCodeAdditionalInfo:          null,
                                                                                SessionId:                         authorizeStopRequest.SessionId,
                                                                                CPOPartnerSessionId:               authorizeStopRequest.CPOPartnerSessionId,
                                                                                EMPPartnerSessionId:               authorizeStopRequest.EMPPartnerSessionId,
                                                                                ProviderId:                        Provider_Id.Parse("DE*GDF"),
                                                                                ResponseTimestamp:                 Timestamp.Now,
                                                                                EventTrackingId:                   EventTracking_Id.New,
                                                                                Runtime:                           TimeSpan.FromMilliseconds(2),
                                                                                ProcessId:                         Process_Id.NewRandom,
                                                                                HTTPResponse:                      null,
                                                                                CustomData:                        null));

            };


            //NotificationAPI    = new NotificationReceiverAPI();

            #endregion

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            empServerAPI.Shutdown();
            //NotificationAPI.Shutdown();
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion




        protected static async Task<HTTPResponse> SendEMPAuthorizeStart(AuthorizeStartRequest Request)
        {

            return await new HTTPSClient(URL.Parse("http://127.0.0.1:8000")).
                             Execute(client => client.POSTRequest(HTTPPath.Parse("/api/oicp/charging/v21/operators/DE*GEF/authorize/start"),
                                                                  requestbuilder => {
                                                                      requestbuilder.Host         = HTTPHostname.Localhost;
                                                                      requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                      requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes(Newtonsoft.Json.Formatting.None);
                                                                      requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                      requestbuilder.Connection   = "close";
                                                                  }),
                                     //CancellationToken:    CancellationToken,
                                     //EventTrackingId:      EventTrackingId,
                                     RequestTimeout:       TimeSpan.FromSeconds(10)).

                             ConfigureAwait(false);

        }

    }

}
