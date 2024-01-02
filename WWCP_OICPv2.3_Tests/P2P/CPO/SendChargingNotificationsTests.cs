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

using NUnit.Framework;
using NUnit.Framework.Legacy;

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.CPO
{

    /// <summary>
    /// P2P CPO sending charging notifications tests.
    /// </summary>
    [TestFixture]
    public class SendChargingNotificationsTests : AP2PTests
    {

        #region SendChargingStartNotification_Test1()

        [Test]
        public async Task SendChargingStartNotification_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
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

            ClassicAssert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingStartNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingStartNotification.Requests_Error);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingStartNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingStartNotification.Responses_Error);

                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_Error);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_Error);


                var oicpResult  = await cpoP2P_DEGEF.SendChargingStartNotification(Provider_Id.Parse("DE*GDF"), request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsTrue   (oicpResult.Response?.Result);


                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingStartNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingStartNotification.Requests_Error);
                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingStartNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingStartNotification.Responses_Error);

                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_Error);
                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region SendChargingProgressNotification_Test1()

        [Test]
        public async Task SendChargingProgressNotification_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
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

            ClassicAssert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingProgressNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingProgressNotification.Requests_Error);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingProgressNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingProgressNotification.Responses_Error);

                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Requests_Error);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Responses_Error);


                var oicpResult  = await cpoClient.SendChargingProgressNotification(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsTrue   (oicpResult.Response?.Result);


                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingProgressNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingProgressNotification.Requests_Error);
                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingProgressNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingProgressNotification.Responses_Error);

                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Requests_Error);
                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingProgressNotification.    Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region SendChargingEndNotification_Test1()

        [Test]
        public async Task SendChargingEndNotification_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
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

            ClassicAssert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingEndNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingEndNotification.Requests_Error);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingEndNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingEndNotification.Responses_Error);

                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Requests_Error);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Responses_Error);


                var oicpResult  = await cpoClient.SendChargingEndNotification(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsTrue   (oicpResult.Response?.Result);


                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingEndNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingEndNotification.Requests_Error);
                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingEndNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingEndNotification.Responses_Error);

                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Requests_Error);
                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingEndNotification.    Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region SendChargingErrorNotification_Test1()

        [Test]
        public async Task SendChargingErrorNotification_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
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

            ClassicAssert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingErrorNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingErrorNotification.Requests_Error);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingErrorNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingErrorNotification.Responses_Error);

                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Requests_Error);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Responses_Error);


                var oicpResult  = await cpoClient.SendChargingErrorNotification(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsTrue   (oicpResult.Response?.Result);


                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingErrorNotification.Requests_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingErrorNotification.Requests_Error);
                ClassicAssert.AreEqual(1, cpoClient.                Counters.SendChargingErrorNotification.Responses_OK);
                ClassicAssert.AreEqual(0, cpoClient.                Counters.SendChargingErrorNotification.Responses_Error);

                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Requests_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Requests_Error);
                ClassicAssert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Responses_OK);
                ClassicAssert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingErrorNotification.    Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

    }

}
