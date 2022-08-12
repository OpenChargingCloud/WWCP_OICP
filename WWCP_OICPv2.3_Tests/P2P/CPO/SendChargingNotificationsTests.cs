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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

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

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Requests_OK);
            //Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Requests_Error);
            //Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Responses_OK);
            //Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Responses_Error);

            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_Error);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_Error);

            var oicpResult  = await cpoP2P_DEGEF.SendChargingStartNotification(Provider_Id.Parse("DE*GDF"), request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            //Assert.AreEqual(1, cpoClient.   Counters.SendChargingStartNotification.Requests_OK);
            //Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Requests_Error);
            //Assert.AreEqual(1, cpoClient.   Counters.SendChargingStartNotification.Responses_OK);
            //Assert.AreEqual(0, cpoClient.   Counters.SendChargingStartNotification.Responses_Error);

            Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Requests_Error);
            Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargingStartNotification.    Responses_Error);

        }

        #endregion


    }

}
