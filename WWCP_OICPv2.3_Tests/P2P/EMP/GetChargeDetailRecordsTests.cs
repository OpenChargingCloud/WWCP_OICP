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
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.EMP
{

    /// <summary>
    /// P2P EMP requesting charge detail records (CDRs) tests.
    /// </summary>
    [TestFixture]
    public class GetChargeDetailRecordsTests : AP2PTests
    {

        #region GetChargeDetailRecords_Empty()

        [Test]
        public async Task GetChargeDetailRecords_Empty()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new GetChargeDetailRecordsRequest(ProviderId:         Provider_Id.Parse("DE-GDF"),
                                                            From:               Timestamp.Now - TimeSpan.FromDays(3),
                                                            To:                 Timestamp.Now - TimeSpan.FromDays(2),
                                                            SessionIds:         null,
                                                            OperatorIds:        null,
                                                            CDRForwarded:       null,

                                                            Page:               null,
                                                            Size:               null,
                                                            SortOrder:          null,
                                                            CustomData:         null,

                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_Error);


                var oicpResult  = await empClient.GetChargeDetailRecords(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.ChargeDetailRecords);
                ClassicAssert.IsFalse  (oicpResult.Response?.ChargeDetailRecords.Any());


                ClassicAssert.AreEqual(1, empClient.                Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region GetChargeDetailRecords_Test1()

        [Test]
        public async Task GetChargeDetailRecords_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new GetChargeDetailRecordsRequest(ProviderId:         Provider_Id.Parse("DE-GDF"),
                                                            From:               Timestamp.Now - TimeSpan.FromDays(1),
                                                            To:                 Timestamp.Now,
                                                            SessionIds:         null,
                                                            OperatorIds:        null,
                                                            CDRForwarded:       null,

                                                            Page:               null,
                                                            Size:               null,
                                                            SortOrder:          null,
                                                            CustomData:         null,

                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_Error);


                var oicpResult  = await empClient.GetChargeDetailRecords(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsNotNull(oicpResult.Response?.ChargeDetailRecords);
                ClassicAssert.AreEqual (1, oicpResult.Response?.ChargeDetailRecords.Count());

                var cdr = oicpResult.Response?.ChargeDetailRecords.FirstOrDefault();

                ClassicAssert.AreEqual(Session_Id.          Parse  ("4cfe3192-87ec-4757-9560-a6ce896bb88b"),        cdr?.SessionId);
                ClassicAssert.AreEqual(EVSE_Id.             Parse  ("DE*GEF*E1234567*A*1"),                         cdr?.EVSEId);
                ClassicAssert.AreEqual(Identification.      FromUID(UID.Parse("AABBCCDD")),                         cdr?.Identification);
                ClassicAssert.AreEqual(DateTime.            Parse  ("2022-08-09T10:18:25.229Z").ToUniversalTime(),  cdr?.SessionStart);
                ClassicAssert.AreEqual(DateTime.            Parse  ("2022-08-09T11:18:25.229Z").ToUniversalTime(),  cdr?.SessionEnd);
                ClassicAssert.AreEqual(DateTime.            Parse  ("2022-08-09T10:20:25.229Z").ToUniversalTime(),  cdr?.ChargingStart);
                ClassicAssert.AreEqual(DateTime.            Parse  ("2022-08-09T11:13:25.229Z").ToUniversalTime(),  cdr?.ChargingEnd);
                ClassicAssert.AreEqual(35,                                                                          cdr?.ConsumedEnergy.kWh);

                ClassicAssert.AreEqual(PartnerProduct_Id.   Parse("AC3"),                                           cdr?.PartnerProductId);
                ClassicAssert.AreEqual(CPOPartnerSession_Id.Parse("e9c6faad-75c8-4f5b-9b5c-164ae7459804"),          cdr?.CPOPartnerSessionId);
                ClassicAssert.AreEqual(EMPPartnerSession_Id.Parse("290b96b3-57df-4021-b8f8-50d9c211c767"),          cdr?.EMPPartnerSessionId);
                ClassicAssert.AreEqual(3,                                                                           cdr?.MeterValueStart?.kWh);
                ClassicAssert.AreEqual(38,                                                                          cdr?.MeterValueEnd?.  kWh);
                ClassicAssert.AreEqual(3,                                                                           cdr?.MeterValuesInBetween?.Count());
                ClassicAssert.IsTrue  (cdr?.MeterValuesInBetween?.Contains(WattHour.ParseWh(4)));
                ClassicAssert.IsTrue  (cdr?.MeterValuesInBetween?.Contains(WattHour.ParseWh(5)));
                ClassicAssert.IsTrue  (cdr?.MeterValuesInBetween?.Contains(WattHour.ParseWh(6)));

                ClassicAssert.AreEqual(3,                                                                           cdr?.SignedMeteringValues?.Count());
                ClassicAssert.AreEqual("loooong start...",                                                          cdr?.SignedMeteringValues?.ElementAt(0).Value);
                ClassicAssert.AreEqual(MeteringStatusType.Start,                                                    cdr?.SignedMeteringValues?.ElementAt(0).MeteringStatus);
                ClassicAssert.AreEqual("loooong progress...",                                                       cdr?.SignedMeteringValues?.ElementAt(1).Value);
                ClassicAssert.AreEqual(MeteringStatusType.Progress,                                                 cdr?.SignedMeteringValues?.ElementAt(1).MeteringStatus);
                ClassicAssert.AreEqual("loooong end...",                                                            cdr?.SignedMeteringValues?.ElementAt(2).Value);
                ClassicAssert.AreEqual(MeteringStatusType.End,                                                      cdr?.SignedMeteringValues?.ElementAt(2).MeteringStatus);

                ClassicAssert.AreEqual("4c6da173-6427-49ed-9b7d-ab0c674d4bc2",                                      cdr?.CalibrationLawVerificationInfo?.CalibrationLawCertificateId);
                ClassicAssert.AreEqual("0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9",      cdr?.CalibrationLawVerificationInfo?.PublicKey);
                ClassicAssert.AreEqual(URL.Parse("https://open.charging.cloud"),                                    cdr?.CalibrationLawVerificationInfo?.MeteringSignatureURL);
                ClassicAssert.AreEqual("plain",                                                                     cdr?.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat);
                ClassicAssert.AreEqual("Just use the Chargy Transparency Software!",                                cdr?.CalibrationLawVerificationInfo?.SignedMeteringValuesVerificationInstruction);

                ClassicAssert.AreEqual(Operator_Id.Parse("DE*GEF"),                                                 cdr?.HubOperatorId);
                ClassicAssert.AreEqual(Provider_Id.Parse("DE-GDF"),                                                 cdr?.HubProviderId);


                ClassicAssert.AreEqual(1, empClient.                Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.GetChargeDetailRecords.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.GetChargeDetailRecords.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

    }

}
