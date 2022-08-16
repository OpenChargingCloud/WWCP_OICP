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

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.CPO
{

    /// <summary>
    /// P2P CPO sending charge detail records tests.
    /// </summary>
    [TestFixture]
    public class SendChargeDetailRecordTests : AP2PTests
    {

        #region SendChargeDetailRecord_Test1()

        [Test]
        public async Task SendChargeDetailRecord_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new ChargeDetailRecordRequest(

                              new ChargeDetailRecord(
                                  SessionId:                       Session_Id.Parse("4cfe3192-87ec-4757-9560-a6ce896bb88b"),
                                  EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                  Identification:                  Identification.FromUID(UID.Parse("AABBCCDD")),
                                  SessionStart:                    DateTime.Parse("2022-08-09T10:18:25.229Z"),
                                  SessionEnd:                      DateTime.Parse("2022-08-09T11:18:25.229Z"),
                                  ChargingStart:                   DateTime.Parse("2022-08-09T10:20:25.229Z"),
                                  ChargingEnd:                     DateTime.Parse("2022-08-09T11:13:25.229Z"),
                                  ConsumedEnergy:                  35,

                                  PartnerProductId:                PartnerProduct_Id.Parse("AC3"),
                                  CPOPartnerSessionId:             CPOPartnerSession_Id.Parse("e9c6faad-75c8-4f5b-9b5c-164ae7459804"),
                                  EMPPartnerSessionId:             EMPPartnerSession_Id.Parse("290b96b3-57df-4021-b8f8-50d9c211c767"),
                                  MeterValueStart:                 3,
                                  MeterValueEnd:                   38,
                                  MeterValuesInBetween:            new Decimal[] {
                                                                       4, 5 ,6
                                                                   },
                                  SignedMeteringValues:            new SignedMeteringValue[] {
                                                                       new SignedMeteringValue(
                                                                           "loooong start...",
                                                                           MeteringStatusTypes.Start
                                                                       ),
                                                                       new SignedMeteringValue(
                                                                           "loooong progress...",
                                                                           MeteringStatusTypes.Progress
                                                                       ),
                                                                       new SignedMeteringValue(
                                                                           "loooong end...",
                                                                           MeteringStatusTypes.End
                                                                       )
                                                                   },
                                  CalibrationLawVerificationInfo:  new CalibrationLawVerification(
                                                                       CalibrationLawCertificateId:                  "4c6da173-6427-49ed-9b7d-ab0c674d4bc2",
                                                                       PublicKey:                                    "0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9",
                                                                       MeteringSignatureURL:                         URL.Parse("https://open.charging.cloud"),
                                                                       MeteringSignatureEncodingFormat:              "plain",
                                                                       SignedMeteringValuesVerificationInstruction:  "Just use the Chargy Transparency Software!",
                                                                       CustomData:                                   null
                                                                   ),
                                  HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                  HubProviderId:                   Provider_Id.Parse("DE-GDF"),

                                  CustomData:                      null,
                                  InternalData:                    null
                              ),

                              OperatorId:   Operator_Id.Parse("DE*GEF"),
                              CustomData:   null);

            Assert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.SendChargeDetailRecord.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.SendChargeDetailRecord.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.SendChargeDetailRecord.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.SendChargeDetailRecord.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Responses_Error);


                var oicpResult  = await cpoP2P_DEGEF.SendChargeDetailRecord(Provider_Id.Parse("DE*GDF"), request);

                Assert.IsNotNull(oicpResult);
                Assert.IsNotNull(oicpResult.Response);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                Assert.IsTrue   (oicpResult.Response?.Result);


                Assert.AreEqual(1, cpoClient.                Counters.SendChargeDetailRecord.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.SendChargeDetailRecord.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.SendChargeDetailRecord.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.SendChargeDetailRecord.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.ChargeDetailRecord.    Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

    }

}
