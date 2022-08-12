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

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.EMP
{

    /// <summary>
    /// P2P EMP client sending remote authorization start/stop tests.
    /// </summary>
    [TestFixture]
    public class RemoteStartStopTests : AP2PTests
    {

        #region AuthorizeRemoteStart_Test1()

        [Test]
        public async Task AuthorizeRemoteStart_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteStartRequest(ProviderId:           Provider_Id.   Parse("DE-GDF"),
                                                          Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                                                          EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  null,
                                                          EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                          CustomData:           null);

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_Error);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_Error);

            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_Error);

            var oicpResult = await empP2P_DEGDF.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_Error);
            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_Error);

            Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_Error);

        }

        #endregion

        #region AuthorizeRemoteStart_Test2()

        [Test]
        public async Task AuthorizeRemoteStart_Test2()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteStartRequest(ProviderId:           Provider_Id.   Parse("DE-GDF"),
                                                          Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                                                          EVSEId:               EVSE_Id.       Parse("DE*XXX*E1234567*A*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  null,
                                                          EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                          CustomData:           null);

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_Error);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_Error);

            var oicpResult = await empP2P_DEGDF.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsFalse  (oicpResult.IsSuccessful);
            Assert.AreEqual (false,                       oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.NoValidContract, oicpResult.Response?.StatusCode.Code);

            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Requests_Error);
            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStart.Responses_Error);

        }

        #endregion


        #region AuthorizeRemoteStop_Test1()

        [Test]
        public async Task AuthorizeRemoteStop_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteStopRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                         EVSEId:               EVSE_Id.    Parse("DE*GEF*E1234567*A*1"),
                                                         SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                                                         CPOPartnerSessionId:  null,
                                                         EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                         CustomData:           null);

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_Error);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_Error);

            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Requests_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Requests_Error);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Responses_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Responses_Error);

            var oicpResult = await empP2P_DEGDF.AuthorizeRemoteStop(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_Error);
            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_Error);

            Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Requests_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Requests_Error);
            Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Responses_OK);
            Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteStop.Responses_Error);

        }

        #endregion

        #region AuthorizeRemoteStop_Test2()

        [Test]
        public async Task AuthorizeRemoteStop_Test2()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteStopRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                         EVSEId:               EVSE_Id.    Parse("DE*XXX*E1234567*A*1"),
                                                         SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                                                         CPOPartnerSessionId:  null,
                                                         EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                         CustomData:           null);

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_Error);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_Error);

            var oicpResult = await empP2P_DEGDF.AuthorizeRemoteStop(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsFalse  (oicpResult.IsSuccessful);
            Assert.AreEqual (false,                       oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.NoValidContract, oicpResult.Response?.StatusCode.Code);

            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Requests_Error);
            //Assert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_OK);
            //Assert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteStop.Responses_Error);

        }

        #endregion

    }

}
