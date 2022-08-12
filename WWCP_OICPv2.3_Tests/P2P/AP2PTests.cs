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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.p2p;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P
{

    /// <summary>
    /// OICP P2P test defaults.
    /// </summary>
    public abstract class AP2PTests
    {

        #region Data

        protected CPOp2pAPI?  cpoP2P_DEGEF;
        protected CPOp2pAPI?  cpoP2P_DEBDO;

        protected EMPp2pAPI?  empP2P_DEGDF;
        protected EMPp2pAPI?  empP2P_DEBDP;

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


            #region Register CPO "DE*GEF"

            cpoP2P_DEGEF = new CPOp2pAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(7001),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(cpoP2P_DEGEF);
            Assert.IsNotNull(cpoP2P_DEGEF.EMPClientAPI);


            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteReservationStart += (timestamp, cpoServerAPI, authorizeRemoteReservationStartRequest) => {

                if (authorizeRemoteReservationStartRequest.Identification is not null)
                {

                    if (authorizeRemoteReservationStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteReservationStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(
                                        authorizeRemoteReservationStartRequest,
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
                                            CustomData:            null),
                                        true)),

                            _ =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(
                                        authorizeRemoteReservationStartRequest,
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
                                            CustomData:            null),
                                        false))
                        };

                    }

                }

                return Task.FromResult(
                    new OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(
                        authorizeRemoteReservationStartRequest,
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
                            CustomData:            null),
                        false));

            };

            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteReservationStop  += (timestamp, cpoServerAPI, authorizeRemoteReservationStopRequest)  => {

                return authorizeRemoteReservationStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>(
                                authorizeRemoteReservationStopRequest,
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
                                    CustomData:            null),
                                true)),

                    _ =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>(
                                authorizeRemoteReservationStopRequest,
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
                                    CustomData:            null),
                                false))
                };

            };


            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteStart            += (timestamp, cpoServerAPI, authorizeRemoteStartRequest) => {

                if (authorizeRemoteStartRequest.Identification is not null)
                {

                    if (authorizeRemoteStartRequest.Identification.RemoteIdentification is not null)
                    {
                        return authorizeRemoteStartRequest.Identification.RemoteIdentification.ToString() switch
                        {

                            "DE-GDF-C12345678X" =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>(
                                        authorizeRemoteStartRequest,
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
                                            CustomData:            null),
                                        true)),

                            _ =>
                                Task.FromResult(
                                    new OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>(
                                        authorizeRemoteStartRequest,
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
                                            CustomData:            null),
                                        false))
                        };

                    }

                }

                return Task.FromResult(
                    new OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>(
                        authorizeRemoteStartRequest,
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
                            CustomData:            null),
                        true));

            };

            cpoP2P_DEGEF.EMPClientAPI.OnAuthorizeRemoteStop             += (timestamp, cpoServerAPI, authorizeRemoteStopRequest)  => {

                return authorizeRemoteStopRequest.SessionId.ToString() switch {

                    "7e8f35a6-13c8-4b37-8099-b21323c83e85" =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>(
                                authorizeRemoteStopRequest,
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
                                    CustomData:            null),
                                true)),

                    _ =>
                        Task.FromResult(
                            new OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>(
                                authorizeRemoteStopRequest,
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
                                    CustomData:            null),
                                false))
                };

            };

            #endregion


            #region Register EMP "DE-GDF"

            empP2P_DEGDF = new EMPp2pAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8001),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(empP2P_DEGDF);
            Assert.IsNotNull(empP2P_DEGDF.CPOClientAPI);


            empP2P_DEGDF.CPOClientAPI.OnAuthorizeStart     += (timestamp, empClientAPI, authorizeStartRequest)     => {

                if (authorizeStartRequest.Identification.RFIDId?.ToString() == "11223344")
                    return Task.FromResult(
                               new OICPResult<AuthorizationStartResponse>(
                                   authorizeStartRequest,
                                   AuthorizationStartResponse.Authorized(
                                       authorizeStartRequest,
                                       Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"), // generated by Hubject!
                                       authorizeStartRequest.CPOPartnerSessionId,
                                       EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                                       Provider_Id.Parse("DE-GDF"),
                                       "Nice to see you!",
                                       "Hello world!",
                                       new Identification[] {
                                           Identification.FromUID(UID.Parse("11223344")),
                                           Identification.FromUID(UID.Parse("55667788"))
                                       }
                                   ),
                                   true)
                           );

                return Task.FromResult(
                           new OICPResult<AuthorizationStartResponse>(
                               authorizeStartRequest,
                               AuthorizationStartResponse.NotAuthorized(
                                   Request:               authorizeStartRequest,
                                   StatusCode:            new StatusCode(
                                                              StatusCodes.NoPositiveAuthenticationResponse,
                                                              "Unknown RFID UID!"
                                                          ),
                                   CPOPartnerSessionId:   authorizeStartRequest.CPOPartnerSessionId,
                                   ProviderId:            Provider_Id.Parse("DE-GDF")
                               ),
                               false)
                           );

            };

            empP2P_DEGDF.CPOClientAPI.OnAuthorizeStop      += (timestamp, empClientAPI, authorizeStopRequest)      => {

                return Task.FromResult(
                           new OICPResult<AuthorizationStopResponse>(
                               authorizeStopRequest,
                               AuthorizationStopResponse.Authorized(
                                   authorizeStopRequest,
                                   authorizeStopRequest.SessionId,
                                   authorizeStopRequest.CPOPartnerSessionId,
                                   authorizeStopRequest.EMPPartnerSessionId,
                                   Provider_Id.Parse("DE-GDF"),
                                   "Have a nice day!",
                                   "bye bye!"
                               ),
                               true)
                       );

            };


            empP2P_DEGDF.CPOClientAPI.OnChargeDetailRecord += (timestamp, cpoServerAPI, chargeDetailRecordRequest) => {

                return Task.FromResult(
                           new OICPResult<Acknowledgement<ChargeDetailRecordRequest>>(
                               chargeDetailRecordRequest,
                               new Acknowledgement<ChargeDetailRecordRequest>(
                                   Request:             chargeDetailRecordRequest,
                                   ResponseTimestamp:   Timestamp.Now,
                                   EventTrackingId:     EventTracking_Id.New,
                                   Runtime:             TimeSpan.FromMilliseconds(2),
                                   StatusCode:          new StatusCode(
                                                            StatusCodes.Success
                                                        ),
                                   HTTPResponse:        null,
                                   Result:              true,
                                   ProcessId:           Process_Id.NewRandom,
                                   CustomData:          null),
                               true));

            };

            #endregion



            cpoP2P_DEGEF.RegisterProvider(Provider_Id.Parse("DE-GDF"),
                                          new CPOClient(
                                              URL.Parse("http://127.0.0.1:8001"),
                                              RequestTimeout: TimeSpan.FromSeconds(10)
                                          ));

            empP2P_DEGDF.RegisterOperator(Operator_Id.Parse("DE*GEF"),
                                          new EMPClient(
                                              URL.Parse("http://127.0.0.1:7001"),
                                              RequestTimeout: TimeSpan.FromSeconds(10)
                                          ));

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {

            cpoP2P_DEGEF?. Shutdown();
            empP2P_DEGDF?. Shutdown();

        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion

    }

}
