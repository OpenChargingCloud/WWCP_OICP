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
    /// EMP receive charge detail records tests.
    /// </summary>
    [TestFixture]
    public class EMPReceiveChargeDetailRecordTests : AEMPTests
    {

        #region EMPReceiveChargeDetailRecord_Test1()

        [Test]
        public async Task EMPReceiveChargeDetailRecord_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request  = new ChargeDetailRecordRequest(

                               new ChargeDetailRecord(
                                   SessionId:                       Session_Id.NewRandom,
                                   EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                   Identification:                  Identification.FromUID(UID.Parse("AABBCCDD")),
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
                               ),

                               OperatorId:         Operator_Id.Parse("DE*GEF"),
                               CustomData:         null,
                               Timestamp:          Timestamp.Now,
                               CancellationToken:  null,
                               EventTrackingId:    EventTracking_Id.New,
                               RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empServerAPI.Counters.ChargeDetailRecord.Requests_OK);
            Assert.AreEqual(0, empServerAPI.Counters.ChargeDetailRecord.Requests_Error);
            Assert.AreEqual(0, empServerAPI.Counters.ChargeDetailRecord.Responses_OK);
            Assert.AreEqual(0, empServerAPI.Counters.ChargeDetailRecord.Responses_Error);

            var oicpResult  = await empServerAPIClient.SendChargeDetailRecord(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            Assert.AreEqual(1, empServerAPI.Counters.ChargeDetailRecord.Requests_OK);
            Assert.AreEqual(0, empServerAPI.Counters.ChargeDetailRecord.Requests_Error);
            Assert.AreEqual(1, empServerAPI.Counters.ChargeDetailRecord.Responses_OK);
            Assert.AreEqual(0, empServerAPI.Counters.ChargeDetailRecord.Responses_Error);

        }

        #endregion

    }

}
