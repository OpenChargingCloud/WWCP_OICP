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

using NUnit.Framework;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO.tests
{

    /// <summary>
    /// OICP CPO test defaults.
    /// </summary>
    public abstract class ACPOTests
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

        protected CPOServerAPI              cpoServerAPI;

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
        public void SetupEachTest()
        {

            Timestamp.Reset();

            cpoServerAPI = new CPOServerAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(7000),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            cpoServerAPI.OnAuthorizeRemoteReservationStart += (timestamp, sender, authorizeRemoteReservationStartRequest) => {

                if (authorizeRemoteReservationStartRequest.Identification is not null)
                {

                    if (authorizeRemoteReservationStartRequest.Identification.RFIDId is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RFIDId.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null))
                        };

                    }


                    if (authorizeRemoteReservationStartRequest.Identification.RFIDIdentification is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RFIDIdentification.UID.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                        Request:               authorizeRemoteReservationStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null))
                        };

                    }

                }

                return Task.FromResult(
                    new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                        Request:               authorizeRemoteReservationStartRequest,
                        ResponseTimestamp:     Timestamp.Now,
                        EventTrackingId:       EventTracking_Id.New,
                        Runtime:               TimeSpan.FromMilliseconds(2),
                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                        HTTPResponse:          null,
                        Result:                false,
                        SessionId:             authorizeRemoteReservationStartRequest.SessionId,
                        CPOPartnerSessionId:   authorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:   authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                        ProcessId:             Process_Id.NewRandom,
                        CustomData:            null));

            };

            cpoServerAPI.OnAuthorizeRemoteReservationStop  += (timestamp, sender, authorizeRemoteReservationStopRequest)  => {

                return authorizeRemoteReservationStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                Request:               authorizeRemoteReservationStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.Success),
                                HTTPResponse:          null,
                                Result:                true,
                                SessionId:             authorizeRemoteReservationStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom,
                                CustomData:            null)),

                    _ =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                Request:               authorizeRemoteReservationStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                HTTPResponse:          null,
                                Result:                false,
                                SessionId:             authorizeRemoteReservationStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom,
                                CustomData:            null))
                };

            };


            cpoServerAPI.OnAuthorizeRemoteStart            += (timestamp, sender, authorizeRemoteStartRequest)            => {

                if (authorizeRemoteStartRequest.Identification is not null)
                {

                    if (authorizeRemoteStartRequest.Identification.RFIDId is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RFIDId.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null))
                        };

                    }


                    if (authorizeRemoteStartRequest.Identification.RFIDIdentification is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RFIDIdentification.UID.ToString() switch
                        {

                            "AABBCCDD" =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.Success),
                                        HTTPResponse:          null,
                                        Result:                true,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null)),

                            _ =>
                                Task.FromResult(
                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                                        Request:               authorizeRemoteStartRequest,
                                        ResponseTimestamp:     Timestamp.Now,
                                        EventTrackingId:       EventTracking_Id.New,
                                        Runtime:               TimeSpan.FromMilliseconds(2),
                                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                        HTTPResponse:          null,
                                        Result:                false,
                                        SessionId:             authorizeRemoteStartRequest.SessionId,
                                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                                        ProcessId:             Process_Id.NewRandom,
                                        CustomData:            null))
                        };

                    }

                }

                return Task.FromResult(
                    new Acknowledgement<AuthorizeRemoteStartRequest>(
                        Request:               authorizeRemoteStartRequest,
                        ResponseTimestamp:     Timestamp.Now,
                        EventTrackingId:       EventTracking_Id.New,
                        Runtime:               TimeSpan.FromMilliseconds(2),
                        StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                        HTTPResponse:          null,
                        Result:                false,
                        SessionId:             authorizeRemoteStartRequest.SessionId,
                        CPOPartnerSessionId:   authorizeRemoteStartRequest.CPOPartnerSessionId,
                        EMPPartnerSessionId:   authorizeRemoteStartRequest.EMPPartnerSessionId,
                        ProcessId:             Process_Id.NewRandom,
                        CustomData:            null));

            };

            cpoServerAPI.OnAuthorizeRemoteStop             += (timestamp, sender, authorizeRemoteStopRequest)             => {

                return authorizeRemoteStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                Request:               authorizeRemoteStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.Success),
                                HTTPResponse:          null,
                                Result:                true,
                                SessionId:             authorizeRemoteStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom,
                                CustomData:            null)),

                    _ =>
                        Task.FromResult(
                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                Request:               authorizeRemoteStopRequest,
                                ResponseTimestamp:     Timestamp.Now,
                                EventTrackingId:       EventTracking_Id.New,
                                Runtime:               TimeSpan.FromMilliseconds(2),
                                StatusCode:            new StatusCode(StatusCodes.CommunicationToEVSEFailed),
                                HTTPResponse:          null,
                                Result:                false,
                                SessionId:             authorizeRemoteStopRequest.SessionId,
                                CPOPartnerSessionId:   authorizeRemoteStopRequest.CPOPartnerSessionId,
                                EMPPartnerSessionId:   authorizeRemoteStopRequest.EMPPartnerSessionId,
                                ProcessId:             Process_Id.NewRandom,
                                CustomData:            null))
                };

            };


            //NotificationAPI    = new NotificationReceiverAPI();


        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            cpoServerAPI.Shutdown();
            //NotificationAPI.Shutdown();
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion




        protected static async Task<HTTPResponse> SendCPOAuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)
        {

            return await new HTTPSClient(URL.Parse("http://127.0.0.1:7000")).
                             Execute(client => client.POSTRequest(HTTPPath.Parse("api/oicp/charging/v21/providers/DE*GDF/authorize-remote/start"),
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
