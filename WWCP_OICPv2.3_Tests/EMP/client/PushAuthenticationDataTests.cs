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

namespace cloud.charging.open.protocols.OICPv2_3.EMP.client.tests
{

    /// <summary>
    /// EMP sending authentication data tests.
    /// </summary>
    [TestFixture]
    public class PushAuthenticationDataTests : AEMPClientAPITests
    {

        #region EMPPushAuthenticationData_Test1()

        [Test]
        public async Task EMPPushAuthenticationData_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
                return;
            }

            var request = new PushAuthenticationDataRequest(
                              new ProviderAuthenticationData(
                                  new Identification[] {
                                      Identification.FromUID(UID.Parse("11223344"))
                                  },
                                  Provider_Id.Parse("DE-GDF")
                              ),
                              ActionTypes.FullLoad
                          );

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.PushAuthenticationData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PushAuthenticationData.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.PushAuthenticationData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PushAuthenticationData.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.PushAuthenticationData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PushAuthenticationData.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.PushAuthenticationData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PushAuthenticationData.Responses_Error);

            var oicpResult  = await empClient.PushAuthenticationData(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, empClient.   Counters.PushAuthenticationData.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.PushAuthenticationData.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.PushAuthenticationData.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.PushAuthenticationData.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.PushAuthenticationData.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PushAuthenticationData.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.PushAuthenticationData.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.PushAuthenticationData.Responses_Error);

        }

        #endregion

    }

}
