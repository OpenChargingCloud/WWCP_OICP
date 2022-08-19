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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO sending charging notifications tests.
    /// </summary>
    [TestFixture]
    public class SendChargingNotificationsTests : ACPOClientAPITests
    {

        #region SendChargingStartNotification_Test1()

        [Test]
        public async Task SendChargingStartNotification_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new ChargingStartNotificationRequest(
                              SessionId:             Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:        Identification.FromUID(
                                                                      UID.Parse("11223344")
                                                     ),
                              EVSEId:                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ChargingStart:         DateTime.Parse("2022-08-09T10:20:25.229Z"),

                              CPOPartnerSessionId:   CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:   EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              SessionStart:          DateTime.Parse("2022-08-09T10:18:25.229Z"),
                              MeterValueStart:       3,
                              OperatorId:            Operator_Id.Parse("DE*GEF"),
                              PartnerProductId:      PartnerProduct_Id.AC1,
                              CustomData:            null);

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Requests_Error);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Responses_Error);

            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingStartNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingStartNotification.    Requests_Error);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingStartNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingStartNotification.    Responses_Error);

            var oicpResult  = await cpoClient.SendChargingStartNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, cpoClient.   Counters.SendChargingStartNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Requests_Error);
            Assert.AreEqual(1, cpoClient.   Counters.SendChargingStartNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Responses_Error);

            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingStartNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingStartNotification.    Requests_Error);
            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingStartNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingStartNotification.    Responses_Error);

        }

        #endregion

        #region SendChargingProgressNotification_Test1()

        [Test]
        public async Task SendChargingProgressNotification_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new ChargingProgressNotificationRequest(
                              SessionId:                Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:           Identification.FromUID(
                                                            UID.Parse("11223344")
                                                        ),
                              EVSEId:                   EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ChargingStart:            DateTime.Parse("2022-08-09T10:20:25.229Z"),
                              EventOccurred:            DateTime.Parse("2022-08-09T10:21:13.451Z"),

                              CPOPartnerSessionId:      CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:      EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              ChargingDuration:         TimeSpan.FromSeconds(48),
                              SessionStart:             DateTime.Parse("2022-08-09T10:18:25.229Z"),
                              ConsumedEnergyProgress:   5,
                              MeterValueStart:          3,
                              MeterValuesInBetween:     new Decimal[] { 4, 5 },
                              OperatorId:               Operator_Id.Parse("DE*GEF"),
                              PartnerProductId:         PartnerProduct_Id.AC1,
                              CustomData:               null);

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoClient.   Counters.SendChargingProgressNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingProgressNotification.Requests_Error);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingProgressNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingProgressNotification.Responses_Error);

            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingProgressNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingProgressNotification.    Requests_Error);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingProgressNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingProgressNotification.    Responses_Error);

            var oicpResult  = await cpoClient.SendChargingProgressNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, cpoClient.   Counters.SendChargingProgressNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingProgressNotification.Requests_Error);
            Assert.AreEqual(1, cpoClient.   Counters.SendChargingProgressNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingProgressNotification.Responses_Error);

            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingProgressNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingProgressNotification.    Requests_Error);
            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingProgressNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingProgressNotification.    Responses_Error);

        }

        #endregion

        #region SendChargingEndNotification_Test1()

        [Test]
        public async Task SendChargingEndNotification_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new ChargingEndNotificationRequest(
                              SessionId:              Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:         Identification.FromUID(
                                                          UID.Parse("11223344")
                                                      ),
                              EVSEId:                 EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ChargingStart:          DateTime.Parse("2022-08-09T10:20:25.229Z"),
                              ChargingEnd:            DateTime.Parse("2022-08-09T11:13:25.229Z"),

                              CPOPartnerSessionId:    CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:    EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              SessionStart:           DateTime.Parse("2022-08-09T10:18:25.229Z"),
                              SessionEnd:             DateTime.Parse("2022-08-09T11:18:25.229Z"),
                              ConsumedEnergy:         35,
                              MeterValueStart:        3,
                              MeterValueEnd:          38,
                              MeterValuesInBetween:   new Decimal[] {
                                                          4, 5 ,6
                                                      },
                              OperatorId:             Operator_Id.Parse("DE*GEF"),
                              PartnerProductId:       PartnerProduct_Id.AC1,
                              PenaltyTimeStart:       DateTime.Parse("2022-08-09T11:19:00.000Z"),

                              CustomData:             null);

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoClient.   Counters.SendChargingEndNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingEndNotification.Requests_Error);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingEndNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingEndNotification.Responses_Error);

            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingEndNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingEndNotification.    Requests_Error);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingEndNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingEndNotification.    Responses_Error);

            var oicpResult  = await cpoClient.SendChargingEndNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, cpoClient.   Counters.SendChargingEndNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingEndNotification.Requests_Error);
            Assert.AreEqual(1, cpoClient.   Counters.SendChargingEndNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingEndNotification.Responses_Error);

            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingEndNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingEndNotification.    Requests_Error);
            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingEndNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingEndNotification.    Responses_Error);

        }

        #endregion

        #region SendChargingErrorNotification_Test1()

        [Test]
        public async Task SendChargingErrorNotification_Test1()
        {

            if (cpoClientAPI is null ||
                cpoClient    is null)
            {
                Assert.Fail("cpoClientAPI or cpoClient is null!");
                return;
            }

            var request = new ChargingErrorNotificationRequest(
                              SessionId:             Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:        Identification.FromUID(
                                                         UID.Parse("11223344")
                                                     ),
                              OperatorId:            Operator_Id.Parse("DE*GEF"),
                              EVSEId:                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ErrorType:             ErrorClassTypes.CriticalError,

                              CPOPartnerSessionId:   CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:   EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              ErrorAdditionalInfo:   "Something wicked happend!",

                              CustomData:            null);

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoClient.   Counters.SendChargingErrorNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingErrorNotification.Requests_Error);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingErrorNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingErrorNotification.Responses_Error);

            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingErrorNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingErrorNotification.    Requests_Error);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingErrorNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingErrorNotification.    Responses_Error);

            var oicpResult  = await cpoClient.SendChargingErrorNotification(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, cpoClient.   Counters.SendChargingErrorNotification.Requests_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingErrorNotification.Requests_Error);
            Assert.AreEqual(1, cpoClient.   Counters.SendChargingErrorNotification.Responses_OK);
            Assert.AreEqual(0, cpoClient.   Counters.SendChargingErrorNotification.Responses_Error);

            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingErrorNotification.    Requests_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingErrorNotification.    Requests_Error);
            Assert.AreEqual(1, cpoClientAPI.Counters.ChargingErrorNotification.    Responses_OK);
            Assert.AreEqual(0, cpoClientAPI.Counters.ChargingErrorNotification.    Responses_Error);

        }

        #endregion

    }

}
