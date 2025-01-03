/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.EMP
{

    /// <summary>
    /// P2P EMP sending authentication data tests.
    /// </summary>
    [TestFixture]
    public class PushAuthenticationDataTests : AP2PTests
    {

        #region PushAuthenticationData_Test1()

        [Test]
        public async Task PushAuthenticationData_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new PushAuthenticationDataRequest(
                              new ProviderAuthenticationData(
                                  new Identification[] {

                                      Identification.FromUID(
                                          UID.Parse("11223344")
                                      ),

                                      Identification.FromRFIDIdentification(
                                          new RFIDIdentification(
                                              UID:             UID.Parse("55667788"),
                                              RFIDType:        RFIDTypes.MifareClassic,
                                              EVCOId:          EVCO_Id.Parse("DE-GDF-C12345678-X"),
                                              PrintedNumber:  "GDF-0001",
                                              ExpiryDate:      DateTime.Parse("2022-08-09T10:18:25.229Z"),
                                              CustomData:      null
                                          ),
                                          CustomData:  null
                                      ),

                                      Identification.FromQRCodeIdentification(
                                          new QRCodeIdentification(
                                              EVCOId:          EVCO_Id.Parse("DE-GDF-C56781234-X"),
                                              HashedPIN:       new HashedPIN(
                                                                   Hash_Value.Parse("XXX"),
                                                                   HashFunctions.Bcrypt
                                                               )
                                          ),
                                          CustomData:  null
                                      ),

                                      Identification.FromRemoteIdentification(
                                          EVCO_Id.Parse("DE-GDF-C23456781-X"),
                                          CustomData:  null
                                      ),

                                      Identification.FromPlugAndChargeIdentification(
                                          EVCO_Id.Parse("DE-GDF-C81235674-X"),
                                          CustomData:  null
                                      )

                                  },
                                  Provider_Id.Parse("DE-GDF")
                              ),
                              ActionTypes.FullLoad
                          );

            ClassicAssert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                ClassicAssert.AreEqual(0, empClient.                Counters.PushAuthenticationData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PushAuthenticationData.Requests_Error);
                ClassicAssert.AreEqual(0, empClient.                Counters.PushAuthenticationData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PushAuthenticationData.Responses_Error);

                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Requests_Error);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Responses_Error);


                var oicpResult  = await empClient.PushAuthenticationData(request);

                ClassicAssert.IsNotNull(oicpResult);
                ClassicAssert.IsNotNull(oicpResult.Response);
                ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
                ClassicAssert.AreEqual (StatusCodes.Success,        oicpResult.Response?.StatusCode?.Code);
                ClassicAssert.IsTrue   (oicpResult.Response?.Result);


                ClassicAssert.AreEqual(1, empClient.                Counters.PushAuthenticationData.Requests_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PushAuthenticationData.Requests_Error);
                ClassicAssert.AreEqual(1, empClient.                Counters.PushAuthenticationData.Responses_OK);
                ClassicAssert.AreEqual(0, empClient.                Counters.PushAuthenticationData.Responses_Error);

                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Requests_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Requests_Error);
                ClassicAssert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Responses_OK);
                ClassicAssert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.PushAuthenticationData.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

    }

}
