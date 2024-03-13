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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.server
{

    /// <summary>
    /// EMP receive charge detail records tests.
    /// </summary>
    [TestFixture]
    public class ReceiveChargeDetailRecordTests : AEMPTests
    {

        #region ReceiveChargeDetailRecord_Test1()

        [Test]
        public async Task ReceiveChargeDetailRecord_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request  = new ChargeDetailRecordRequest(

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
                                   MeterValuesInBetween:            [
                                                                        4, 5 ,6
                                                                    ],
                                   SignedMeteringValues:            [
                                                                        new SignedMeteringValue(
                                                                            "loooong start...",
                                                                            MeteringStatusType.Start
                                                                        ),
                                                                        new SignedMeteringValue(
                                                                            "loooong progress...",
                                                                            MeteringStatusType.Progress
                                                                        ),
                                                                        new SignedMeteringValue(
                                                                            "loooong end...",
                                                                            MeteringStatusType.End
                                                                        )
                                                                    ],
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

                               OperatorId:         Operator_Id.Parse("DE*GEF"),
                               CustomData:         null,

                               RequestTimeout:     TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error);

            ClassicAssert.AreEqual(0, empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error);

            var oicpResult  = await empServerAPIClient.SendChargeDetailRecord(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (true,                oicpResult.Response?.Result);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error);

            ClassicAssert.AreEqual(1, empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error);

        }

        #endregion

    }

}
