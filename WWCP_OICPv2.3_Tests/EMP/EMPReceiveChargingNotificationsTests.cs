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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP.tests
{

    /// <summary>
    /// EMP receive charging notifications tests.
    /// </summary>
    [TestFixture]
    public class EMPReceiveChargingNotificationsTests : AEMPTests
    {

        #region EMPReceiveChargingStartNotification_Test1()

        [Test]
        public async Task EMPReceiveChargingStartNotification_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new ChargingStartNotificationRequest(SessionId:            Session_Id.NewRandom,
                                                                   Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                   EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                                   ChargingStart:        Timestamp.Now - TimeSpan.FromSeconds(5),
                                                                   CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                                   EMPPartnerSessionId:  null,
                                                                   SessionStart:         Timestamp.Now - TimeSpan.FromSeconds(10),
                                                                   MeterValueStart:      23,
                                                                   OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                                   PartnerProductId:     PartnerProduct_Id.AC1,
                                                                   CustomData:           null,
                                                                   Timestamp:            Timestamp.Now,
                                                                   CancellationToken:    null,
                                                                   EventTrackingId:      EventTracking_Id.New,
                                                                   RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingStartNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingStartNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingStartNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingStartNotification.Responses_Error);

            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingStartNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingStartNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingStartNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingStartNotification.Responses_Error);

            var oicpResult  = await empServerAPIClient.SendChargingStartNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingStartNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingStartNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingStartNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingStartNotification.Responses_Error);

            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingStartNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingStartNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingStartNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingStartNotification.Responses_Error);

        }

        #endregion

        #region EMPReceiveChargingProgressNotification_Test1()

        [Test]
        public async Task EMPReceiveChargingProgressNotification_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new ChargingProgressNotificationRequest(SessionId:               Session_Id.NewRandom,
                                                                      Identification:          Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                      EVSEId:                  EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                                      ChargingStart:           Timestamp.Now - TimeSpan.FromSeconds(5),
                                                                      EventOccurred:            Timestamp.Now,
                                                                      CPOPartnerSessionId:     CPOPartnerSession_Id.NewRandom,
                                                                      EMPPartnerSessionId:     null,
                                                                      ChargingDuration:        TimeSpan.FromSeconds(5),
                                                                      SessionStart:            Timestamp.Now - TimeSpan.FromSeconds(10),
                                                                      ConsumedEnergyProgress:  12,
                                                                      MeterValueStart:         23,
                                                                      MeterValuesInBetween:    Array.Empty<Decimal>(),
                                                                      OperatorId:              Operator_Id.Parse("DE*GEF"),
                                                                      PartnerProductId:        PartnerProduct_Id.AC1,
                                                                      CustomData:              null,
                                                                      Timestamp:               Timestamp.Now,
                                                                      CancellationToken:       null,
                                                                      EventTrackingId:         EventTracking_Id.New,
                                                                      RequestTimeout:          TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingProgressNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingProgressNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingProgressNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingProgressNotification.Responses_Error);

            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingProgressNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingProgressNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingProgressNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingProgressNotification.Responses_Error);

            var oicpResult  = await empServerAPIClient.SendChargingProgressNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingProgressNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingProgressNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingProgressNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingProgressNotification.Responses_Error);

            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingProgressNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingProgressNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingProgressNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingProgressNotification.Responses_Error);

        }

        #endregion

        #region EMPReceiveChargingEndNotification_Test1()

        [Test]
        public async Task EMPReceiveChargingEndNotification_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new ChargingEndNotificationRequest(SessionId:             Session_Id.NewRandom,
                                                                 Identification:        Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                 EVSEId:                EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                                 ChargingStart:         Timestamp.Now - TimeSpan.FromSeconds(5),
                                                                 ChargingEnd:           Timestamp.Now - TimeSpan.FromSeconds(1),
                                                                 CPOPartnerSessionId:   CPOPartnerSession_Id.NewRandom,
                                                                 EMPPartnerSessionId:   null,
                                                                 SessionStart:          Timestamp.Now - TimeSpan.FromSeconds(10),
                                                                 SessionEnd:            Timestamp.Now,
                                                                 ConsumedEnergy:        12,
                                                                 MeterValueStart:       23,
                                                                 MeterValueEnd:         42,
                                                                 MeterValuesInBetween:  Array.Empty<Decimal>(),
                                                                 OperatorId:            Operator_Id.Parse("DE*GEF"),
                                                                 PartnerProductId:      PartnerProduct_Id.AC1,
                                                                 PenaltyTimeStart:      Timestamp.Now - TimeSpan.FromSeconds(3),
                                                                 CustomData:            null,
                                                                 Timestamp:             Timestamp.Now,
                                                                 CancellationToken:     null,
                                                                 EventTrackingId:       EventTracking_Id.New,
                                                                 RequestTimeout:        TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingEndNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingEndNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingEndNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingEndNotification.Responses_Error);

            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingEndNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingEndNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingEndNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingEndNotification.Responses_Error);

            var oicpResult  = await empServerAPIClient.SendChargingEndNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingEndNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingEndNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingEndNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingEndNotification.Responses_Error);

            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingEndNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingEndNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingEndNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingEndNotification.Responses_Error);

        }

        #endregion

        #region EMPReceiveChargingErrorNotification_Test1()

        [Test]
        public async Task EMPReceiveChargingErrorNotification_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new ChargingErrorNotificationRequest(SessionId:            Session_Id.NewRandom,
                                                                   Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                                                   OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                                   EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                                   ErrorType:            ErrorClassTypes.CriticalError,
                                                                   CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                                   EMPPartnerSessionId:  null,
                                                                   ErrorAdditionalInfo:  "No space left of device!",
                                                                   CustomData:           null,
                                                                   Timestamp:            Timestamp.Now,
                                                                   CancellationToken:    null,
                                                                   EventTrackingId:      EventTracking_Id.New,
                                                                   RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingErrorNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingErrorNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingErrorNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingErrorNotification.Responses_Error);

            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingErrorNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingErrorNotification.Requests_Error);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingErrorNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingErrorNotification.Responses_Error);

            var oicpResult  = await empServerAPIClient.SendChargingErrorNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingErrorNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingErrorNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPIClient.Counters.SendChargingErrorNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPIClient.Counters.SendChargingErrorNotification.Responses_Error);

            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingErrorNotification.Requests_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingErrorNotification.Requests_Error);
            Assert.AreEqual(1, empServerAPI.      Counters.    ChargingErrorNotification.Responses_OK);
            Assert.AreEqual(0, empServerAPI.      Counters.    ChargingErrorNotification.Responses_Error);

        }

        #endregion

    }

}
