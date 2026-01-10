/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.server
{

    /// <summary>
    /// OICP CPO test defaults.
    /// </summary>
    public abstract class ACPOTests
    {

        #region Data

        protected CPOServerAPI?        cpoServerAPI;
        protected CPOServerAPIClient?  cpoServerAPIClient;

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

            Timestamp.Reset();

            cpoServerAPI = await CPOServerAPI.CreateServer(
                                     ExternalDNSName:  "open.charging.cloud",
                                     HTTPServerPort:   IPPort.Parse(7000),
                                     LoggingPath:      "tests",
                                     AutoStart:        true
                                 );

            Assert.That(cpoServerAPI,  Is.Not.Null);


            cpoServerAPI.OnAuthorizeRemoteReservationStart += (timestamp, cpoServerAPI, authorizeRemoteReservationStartRequest) => {

                if (authorizeRemoteReservationStartRequest.Identification is not null)
                {

                    if (authorizeRemoteReservationStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
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
                                        ProcessId:             Process_Id.NewRandom(),
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
                                        ProcessId:             Process_Id.NewRandom(),
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
                        ProcessId:             Process_Id.NewRandom(),
                        CustomData:            null));

            };

            cpoServerAPI.OnAuthorizeRemoteReservationStop  += (timestamp, cpoServerAPI, authorizeRemoteReservationStopRequest)  => {

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
                                ProcessId:             Process_Id.NewRandom(),
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
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null))
                };

            };


            cpoServerAPI.OnAuthorizeRemoteStart            += (timestamp, cpoServerAPI, authorizeRemoteStartRequest) => {

                if (authorizeRemoteStartRequest.Identification is not null)
                {

                    if (authorizeRemoteStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
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
                                        ProcessId:             Process_Id.NewRandom(),
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
                                        ProcessId:             Process_Id.NewRandom(),
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
                        ProcessId:             Process_Id.NewRandom(),
                        CustomData:            null));

            };

            cpoServerAPI.OnAuthorizeRemoteStop             += (timestamp, cpoServerAPI, authorizeRemoteStopRequest)  => {

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
                                ProcessId:             Process_Id.NewRandom(),
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
                                ProcessId:             Process_Id.NewRandom(),
                                CustomData:            null))
                };

            };


            cpoServerAPIClient = new CPOServerAPIClient(
                                     URL.Parse("http://127.0.0.1:7000"),
                                     RequestTimeout: TimeSpan.FromSeconds(10)
                                 );

            Assert.That(cpoServerAPIClient,  Is.Not.Null);

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public async Task ShutdownEachTest()
        {

            var server = cpoServerAPI?.HTTPServer;

            if (server is not null)
                await server.Stop();

        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion




        //ToDo: RAW tests: OperatorId != OperatorIdURL


        //protected static async Task<HTTPResponse> SendCPOAuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)
        //{

        //    return await new HTTPSClient(URL.Parse("http://127.0.0.1:7000")).
        //                     Execute(client => client.POSTRequest(HTTPPath.Parse("api/oicp/charging/v21/providers/DE*GDF/authorize-remote/start"),
        //                                                          requestbuilder => {
        //                                                              requestBuilder.Host         = HTTPHostname.Localhost;
        //                                                              requestBuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
        //                                                              requestBuilder.Content      = Request.ToJSON().ToUTF8Bytes(Newtonsoft.Json.Formatting.None);
        //                                                              requestBuilder.Accept.Add(HTTPContentType.Application.JSON_UTF8);
        //                                                              requestBuilder.Connection   = ConnectionType.Close;
        //                                                          }),
        //                             //CancellationToken:    CancellationToken,
        //                             //EventTrackingId:      EventTrackingId,
        //                             RequestTimeout:       TimeSpan.FromSeconds(10)).

        //                     ConfigureAwait(false);

        //}

    }

}
