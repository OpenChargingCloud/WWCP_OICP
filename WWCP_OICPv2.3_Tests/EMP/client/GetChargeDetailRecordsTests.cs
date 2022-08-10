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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.client
{

    /// <summary>
    /// EMP requesting charge detail records (CDRs) tests.
    /// </summary>
    [TestFixture]
    public class GetChargeDetailRecordsTests : AEMPClientAPITests
    {

        #region GetChargeDetailRecords_Empty()

        [Test]
        public async Task GetChargeDetailRecords_Empty()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
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

                                                            Timestamp:          Timestamp.Now,
                                                            CancellationToken:  null,
                                                            EventTrackingId:    EventTracking_Id.New,
                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Responses_Error);

            var oicpResult  = await empClient.GetChargeDetailRecords(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.ChargeDetailRecords);
            Assert.IsFalse  (oicpResult.Response?.ChargeDetailRecords.Any());

            Assert.AreEqual(1, empClient.   Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Responses_Error);

        }

        #endregion

        #region GetChargeDetailRecords_Test1()

        [Test]
        public async Task GetChargeDetailRecords_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
                return;
            }

            var request = new GetChargeDetailRecordsRequest(ProviderId:         Provider_Id.Parse("DE-GDF"),
                                                            From:               Timestamp.Now,
                                                            To:                 Timestamp.Now - TimeSpan.FromDays(1),
                                                            SessionIds:         null,
                                                            OperatorIds:        null,
                                                            CDRForwarded:       null,

                                                            Page:               null,
                                                            Size:               null,
                                                            SortOrder:          null,
                                                            CustomData:         null,

                                                            Timestamp:          Timestamp.Now,
                                                            CancellationToken:  null,
                                                            EventTrackingId:    EventTracking_Id.New,
                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Responses_Error);

            var oicpResult  = await empClient.GetChargeDetailRecords(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsNotNull(oicpResult.Response?.ChargeDetailRecords);
            Assert.AreEqual (1, oicpResult.Response?.ChargeDetailRecords.Count());

            var cdr = oicpResult.Response?.ChargeDetailRecords.FirstOrDefault();

            Assert.AreEqual(Session_Id.          Parse  ("4cfe3192-87ec-4757-9560-a6ce896bb88b"),        cdr?.SessionId);
            Assert.AreEqual(EVSE_Id.             Parse  ("DE*GEF*E1234567*A*1"),                           cdr?.EVSEId);
            Assert.AreEqual(Identification.      FromUID(UID.Parse("AABBCCDD")),                         cdr?.Identification);
            Assert.AreEqual(DateTime.            Parse  ("2022-08-09T10:18:25.229Z").ToUniversalTime(),  cdr?.SessionStart);
            Assert.AreEqual(DateTime.            Parse  ("2022-08-09T11:18:25.229Z").ToUniversalTime(),  cdr?.SessionEnd);
            Assert.AreEqual(DateTime.            Parse  ("2022-08-09T10:20:25.229Z").ToUniversalTime(),  cdr?.ChargingStart);
            Assert.AreEqual(DateTime.            Parse  ("2022-08-09T11:13:25.229Z").ToUniversalTime(),  cdr?.ChargingEnd);
            Assert.AreEqual(35,                                                                          cdr?.ConsumedEnergy);

            Assert.AreEqual(PartnerProduct_Id.   Parse("AC3"),                                           cdr?.PartnerProductId);
            Assert.AreEqual(CPOPartnerSession_Id.Parse("e9c6faad-75c8-4f5b-9b5c-164ae7459804"),          cdr?.CPOPartnerSessionId);
            Assert.AreEqual(EMPPartnerSession_Id.Parse("290b96b3-57df-4021-b8f8-50d9c211c767"),          cdr?.EMPPartnerSessionId);
            Assert.AreEqual(3,                                                                           cdr?.MeterValueStart);
            Assert.AreEqual(38,                                                                          cdr?.MeterValueEnd);
            Assert.AreEqual(3,                                                                           cdr?.MeterValuesInBetween?.Count());
            Assert.IsTrue  (cdr?.MeterValuesInBetween?.Contains(4));
            Assert.IsTrue  (cdr?.MeterValuesInBetween?.Contains(5));
            Assert.IsTrue  (cdr?.MeterValuesInBetween?.Contains(6));

            Assert.AreEqual(3,                                                                           cdr?.SignedMeteringValues?.Count());
            Assert.AreEqual("loooong start...",                                                          cdr?.SignedMeteringValues?.ElementAt(0).Value);
            Assert.AreEqual(MeteringStatusTypes.Start,                                                   cdr?.SignedMeteringValues?.ElementAt(0).MeteringStatus);
            Assert.AreEqual("loooong progress...",                                                       cdr?.SignedMeteringValues?.ElementAt(1).Value);
            Assert.AreEqual(MeteringStatusTypes.Progress,                                                cdr?.SignedMeteringValues?.ElementAt(1).MeteringStatus);
            Assert.AreEqual("loooong end...",                                                            cdr?.SignedMeteringValues?.ElementAt(2).Value);
            Assert.AreEqual(MeteringStatusTypes.End,                                                     cdr?.SignedMeteringValues?.ElementAt(2).MeteringStatus);

            Assert.AreEqual("4c6da173-6427-49ed-9b7d-ab0c674d4bc2",                                      cdr?.CalibrationLawVerificationInfo?.CalibrationLawCertificateId);
            Assert.AreEqual("0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9",      cdr?.CalibrationLawVerificationInfo?.PublicKey);
            Assert.AreEqual(URL.Parse("https://open.charging.cloud"),                                    cdr?.CalibrationLawVerificationInfo?.MeteringSignatureURL);
            Assert.AreEqual("plain",                                                                     cdr?.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat);
            Assert.AreEqual("Just use the Chargy Transparency Software!",                                cdr?.CalibrationLawVerificationInfo?.SignedMeteringValuesVerificationInstruction);

            Assert.AreEqual(Operator_Id.Parse("DE*GEF"),                                                 cdr?.HubOperatorId);
            Assert.AreEqual(Provider_Id.Parse("DE*GDF"),                                                 cdr?.HubProviderId);

            Assert.AreEqual(1, empClient.   Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.GetChargeDetailRecords.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.GetChargeDetailRecords.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.GetChargeDetailRecords.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.GetChargeDetailRecords.Responses_Error);

        }

        #endregion

    }

}
