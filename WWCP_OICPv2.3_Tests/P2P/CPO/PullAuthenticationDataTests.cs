/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.CPO
{

    /// <summary>
    /// P2P CPO requesting authentication data tests.
    /// </summary>
    [TestFixture]
    public class PullAuthenticationDataTests : AP2PTests
    {

        #region PullAuthenticationData_Test_Empty()

        [Test]
        public async Task PullAuthenticationData_Test_Empty()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullAuthenticationDataRequest(OperatorId:         Operator_Id.Parse("DE*XXX"),
                                                            CustomData:         null,

                                                            Timestamp:          Timestamp.Now,
                                                            CancellationToken:  null,
                                                            EventTrackingId:    EventTracking_Id.New,
                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_Error);


                var oicpResult  = await cpoClient.PullAuthenticationData(request);

                Assert.IsNotNull(oicpResult);
                Assert.IsNotNull(oicpResult.Response);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                Assert.IsNotNull(oicpResult.Response?.ProviderAuthenticationData);
                Assert.IsFalse  (oicpResult.Response?.ProviderAuthenticationData.Any());


                Assert.AreEqual(1, cpoClient.                Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region PullAuthenticationData_Test1()

        [Test]
        public async Task PullAuthenticationData_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PullAuthenticationDataRequest(OperatorId:         Operator_Id.Parse("DE*GEF"),
                                                            CustomData:         null,

                                                            Timestamp:          Timestamp.Now,
                                                            CancellationToken:  null,
                                                            EventTrackingId:    EventTracking_Id.New,
                                                            RequestTimeout:     TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_Error);


                var oicpResult                = await cpoClient.PullAuthenticationData(request);

                Assert.IsNotNull(oicpResult);
                Assert.IsNotNull(oicpResult.Response);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
                Assert.IsNotNull(oicpResult.Response?.ProviderAuthenticationData);
                Assert.AreEqual (1, oicpResult.Response?.ProviderAuthenticationData.Count());

                var providerAuthenticationData  = oicpResult.Response?.ProviderAuthenticationData.FirstOrDefault();
                Assert.IsNotNull(providerAuthenticationData);
                Assert.AreEqual (Provider_Id.Parse("DE-GDF"),                                   providerAuthenticationData?.ProviderId);
                Assert.AreEqual (5,                                                             providerAuthenticationData?.Identifications.Count());

                var identification1  = providerAuthenticationData?.Identifications.ElementAt(0);
                Assert.IsNotNull(identification1);
                Assert.AreEqual (UID.Parse("11223344"),                                         identification1?.RFIDId);

                var identification2  = providerAuthenticationData?.Identifications.ElementAt(1);
                Assert.IsNotNull(identification2);
                Assert.AreEqual (UID.Parse("55667788"),                                         identification2?.RFIDIdentification?.UID);
                Assert.AreEqual (RFIDTypes.MifareClassic,                                       identification2?.RFIDIdentification?.RFIDType);
                Assert.AreEqual (EVCO_Id.Parse("DE-GDF-C12345678-X"),                           identification2?.RFIDIdentification?.EVCOId);
                Assert.AreEqual ("GDF-0001",                                                    identification2?.RFIDIdentification?.PrintedNumber);
                Assert.AreEqual (DateTime.Parse("2022-08-09T10:18:25.229Z").ToUniversalTime(),  identification2?.RFIDIdentification?.ExpiryDate);

                var identification3  = providerAuthenticationData?.Identifications.ElementAt(2);
                Assert.IsNotNull(identification3);
                Assert.AreEqual (EVCO_Id.Parse("DE-GDF-C56781234-X"),                           identification3?.QRCodeIdentification?.EVCOId);
                Assert.AreEqual (HashFunctions.Bcrypt,                                          identification3?.QRCodeIdentification?.HashedPIN?.Function);
                Assert.AreEqual (Hash_Value.Parse("XXX"),                                       identification3?.QRCodeIdentification?.HashedPIN?.Value);

                var identification4  = providerAuthenticationData?.Identifications.ElementAt(3);
                Assert.IsNotNull(identification4);
                Assert.AreEqual (EVCO_Id.Parse("DE-GDF-C23456781-X"),                           identification4?.RemoteIdentification);

                var identification5  = providerAuthenticationData?.Identifications.ElementAt(4);
                Assert.IsNotNull(identification5);
                Assert.AreEqual (EVCO_Id.Parse("DE-GDF-C81235674-X"),                           identification5?.PlugAndChargeIdentification);


                Assert.AreEqual(1, cpoClient.                Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.PullAuthenticationData.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.PullAuthenticationData.Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

    }

}
