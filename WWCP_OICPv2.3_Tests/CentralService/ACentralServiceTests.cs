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
using cloud.charging.open.protocols.OICPv2_3.CentralService;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CentralService
{

    /// <summary>
    /// OICP central service test defaults.
    /// </summary>
    public abstract class ACentralServiceTests
    {

        #region Data

        protected CentralServiceAPI?  centralServiceAPI;

        protected EMPClient?          empClient;
        protected CPOClient?          cpoClient;

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

            centralServiceAPI = new CentralServiceAPI(
                                    ExternalDNSName:  "open.charging.cloud",
                                    HTTPServerPort:   IPPort.Parse(6000),
                                    LoggingPath:      "tests",
                                    Autostart:        true
                                );

            Assert.IsNotNull(centralServiceAPI);


            #region EMPClientAPI delegates...

            #region OnAuthorizeRemoteReservationStart/-Stop

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteReservationStart += (timestamp, sender, authorizeRemoteReservationStartRequest) => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Success(
                               authorizeRemoteReservationStartRequest,
                               Acknowledgement<AuthorizeRemoteReservationStartRequest>.Success(
                                   Request:                   authorizeRemoteReservationStartRequest,
                                   SessionId:                 authorizeRemoteReservationStartRequest.SessionId,
                                   CPOPartnerSessionId:       CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:       authorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                   StatusCodeDescription:     null,
                                   StatusCodeAdditionalInfo:  null,
                                   ResponseTimestamp:         Timestamp.Now,
                                   EventTrackingId:           authorizeRemoteReservationStartRequest.EventTrackingId,
                                   Runtime:                   TimeSpan.FromMilliseconds(23),
                                   ProcessId:                 processId,
                                   HTTPResponse:              null,
                                   CustomData:                null
                               ),
                               ProcessId:                 processId
                           )
                       );

            };

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteReservationStop  += (timestamp, sender, authorizeRemoteReservationStopRequest) => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Success(
                               authorizeRemoteReservationStopRequest,
                               Acknowledgement<AuthorizeRemoteReservationStopRequest>.Success(
                                   Request:                   authorizeRemoteReservationStopRequest,
                                   SessionId:                 authorizeRemoteReservationStopRequest.SessionId,
                                   CPOPartnerSessionId:       CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:       authorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                   StatusCodeDescription:     null,
                                   StatusCodeAdditionalInfo:  null,
                                   ResponseTimestamp:         Timestamp.Now,
                                   EventTrackingId:           authorizeRemoteReservationStopRequest.EventTrackingId,
                                   Runtime:                   TimeSpan.FromMilliseconds(23),
                                   ProcessId:                 processId,
                                   HTTPResponse:              null,
                                   CustomData:                null
                               ),
                               ProcessId:                 processId
                           )
                       );

            };

            #endregion

            #region OnAuthorizeRemoteStart/-Stop

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteStart += (timestamp, sender, authorizeRemoteStartRequest) => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Success(
                               authorizeRemoteStartRequest,
                               Acknowledgement<AuthorizeRemoteStartRequest>.Success(
                                   Request:                   authorizeRemoteStartRequest,
                                   SessionId:                 authorizeRemoteStartRequest.SessionId,
                                   CPOPartnerSessionId:       CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:       authorizeRemoteStartRequest.EMPPartnerSessionId,
                                   StatusCodeDescription:     null,
                                   StatusCodeAdditionalInfo:  null,
                                   ResponseTimestamp:         Timestamp.Now,
                                   EventTrackingId:           authorizeRemoteStartRequest.EventTrackingId,
                                   Runtime:                   TimeSpan.FromMilliseconds(23),
                                   ProcessId:                 processId,
                                   HTTPResponse:              null,
                                   CustomData:                null
                               ),
                               ProcessId:                 processId
                           )
                       );

            };

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteStop  += (timestamp, sender, authorizeRemoteStopRequest) => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Success(
                               authorizeRemoteStopRequest,
                               Acknowledgement<AuthorizeRemoteStopRequest>.Success(
                                   Request:                   authorizeRemoteStopRequest,
                                   SessionId:                 authorizeRemoteStopRequest.SessionId,
                                   CPOPartnerSessionId:       CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:       authorizeRemoteStopRequest.EMPPartnerSessionId,
                                   StatusCodeDescription:     null,
                                   StatusCodeAdditionalInfo:  null,
                                   ResponseTimestamp:         Timestamp.Now,
                                   EventTrackingId:           authorizeRemoteStopRequest.EventTrackingId,
                                   Runtime:                   TimeSpan.FromMilliseconds(23),
                                   ProcessId:                 processId,
                                   HTTPResponse:              null,
                                   CustomData:                null
                               ),
                               ProcessId:                 processId
                           )
                       );

            };

            #endregion

            #region OnAuthorizeRemoteStart/-Stop

            centralServiceAPI.EMPClientAPI.OnGetChargeDetailRecords += (timestamp, sender, getChargeDetailRecordsRequest) => {

                var processId = Process_Id.NewRandom;

                return Task.FromResult(
                           OICPResult<GetChargeDetailRecordsResponse>.Success(
                               getChargeDetailRecordsRequest,
                               new GetChargeDetailRecordsResponse(
                                   Request:                   getChargeDetailRecordsRequest,
                                   ResponseTimestamp:         Timestamp.Now,
                                   EventTrackingId:           getChargeDetailRecordsRequest.EventTrackingId ?? EventTracking_Id.New,
                                   Runtime:                   TimeSpan.FromMilliseconds(23),
                                   ChargeDetailRecords:       new ChargeDetailRecord[] {

                                                                  new ChargeDetailRecord(
                                                                      SessionId:                       Session_Id.NewRandom,
                                                                      EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                                      Identification:                  Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                      SessionStart:                    Timestamp.Now - TimeSpan.FromMinutes(60),
                                                                      SessionEnd:                      Timestamp.Now - TimeSpan.FromMinutes(10),
                                                                      ChargingStart:                   Timestamp.Now - TimeSpan.FromMinutes(50),
                                                                      ChargingEnd:                     Timestamp.Now - TimeSpan.FromMinutes(20),
                                                                      ConsumedEnergy:                  35,

                                                                      PartnerProductId:                PartnerProduct_Id.Parse("AC1"),
                                                                      CPOPartnerSessionId:             CPOPartnerSession_Id.NewRandom,
                                                                      EMPPartnerSessionId:             EMPPartnerSession_Id.NewRandom,
                                                                      MeterValueStart:                 3,
                                                                      MeterValueEnd:                   38,
                                                                      MeterValuesInBetween:            Array.Empty<Decimal>(),
                                                                      SignedMeteringValues:            Array.Empty<SignedMeteringValue>(),
                                                                      CalibrationLawVerificationInfo:  new CalibrationLawVerification(),
                                                                      HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                                                      HubProviderId:                   Provider_Id.Parse("DE*GDF"),

                                                                      CustomData:                      null,
                                                                      InternalData:                    null
                                                                  ),

                                                                  new ChargeDetailRecord(
                                                                      SessionId:                       Session_Id.NewRandom,
                                                                      EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*2"),
                                                                      Identification:                  Identification.FromUID(UID.Parse("CCDDEEFFAABBCC")),
                                                                      SessionStart:                    Timestamp.Now - TimeSpan.FromMinutes(60),
                                                                      SessionEnd:                      Timestamp.Now - TimeSpan.FromMinutes(10),
                                                                      ChargingStart:                   Timestamp.Now - TimeSpan.FromMinutes(50),
                                                                      ChargingEnd:                     Timestamp.Now - TimeSpan.FromMinutes(20),
                                                                      ConsumedEnergy:                  35,

                                                                      PartnerProductId:                PartnerProduct_Id.Parse("AC3"),
                                                                      CPOPartnerSessionId:             CPOPartnerSession_Id.NewRandom,
                                                                      EMPPartnerSessionId:             EMPPartnerSession_Id.NewRandom,
                                                                      MeterValueStart:                 3,
                                                                      MeterValueEnd:                   38,
                                                                      MeterValuesInBetween:            Array.Empty<Decimal>(),
                                                                      SignedMeteringValues:            Array.Empty<SignedMeteringValue>(),
                                                                      CalibrationLawVerificationInfo:  new CalibrationLawVerification(),
                                                                      HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                                                      HubProviderId:                   Provider_Id.Parse("DE*GDF"),

                                                                      CustomData:                      null,
                                                                      InternalData:                    null
                                                                  )

                                                              },
                                   HTTPResponse:              null,
                                   ProcessId:                 processId,
                                   StatusCode:                null,
                                   FirstPage:                 true,
                                   LastPage:                  true,
                                   Number:                    1,
                                   NumberOfElements:          2,
                                   Size:                      getChargeDetailRecordsRequest.Size,
                                   TotalElements:             2,
                                   TotalPages:                1,
                                   CustomData:                null
                               ),
                               ProcessId:                 processId
                           )
                       );

            };

            #endregion

            #endregion


            #region CPOClientAPI delegates...

            centralServiceAPI.CPOClientAPI.OnAuthorizeStart += (timestamp, sender, authorizeStartRequest) => {

                return Task.FromResult(
                           OICPResult<AuthorizationStartResponse>.Success(
                               authorizeStartRequest,
                               AuthorizationStartResponse.Authorized(
                                   Request:                           authorizeStartRequest,
                                   SessionId:                         Session_Id.NewRandom,
                                   CPOPartnerSessionId:               CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:               EMPPartnerSession_Id.NewRandom,
                                   ProviderId:                        Provider_Id.Parse("DE-GDF"),
                                   StatusCodeDescription:             null,
                                   StatusCodeAdditionalInfo:          null,
                                   AuthorizationStopIdentifications:  null,
                                   ResponseTimestamp:                 Timestamp.Now,
                                   EventTrackingId:                   authorizeStartRequest.EventTrackingId,
                                   Runtime:                           TimeSpan.FromMilliseconds(23),
                                   ProcessId:                         Process_Id.NewRandom,
                                   HTTPResponse:                      null,
                                   CustomData:                        null
                               )
                           )
                       );;

            };

            #endregion

            #region Setup EMPClient...

            empClient = new EMPClient(URL.Parse("http://127.0.0.1:6000"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            Assert.IsNotNull(empClient);

            #endregion

            #region Setup CPOClient...

            cpoClient = new CPOClient(URL.Parse("http://127.0.0.1:6000"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            Assert.IsNotNull(cpoClient);

            #endregion

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            centralServiceAPI?.Shutdown();
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
