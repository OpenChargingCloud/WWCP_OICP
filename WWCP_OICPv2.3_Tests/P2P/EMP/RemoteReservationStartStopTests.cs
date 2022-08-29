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

using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.EMP
{

    /// <summary>
    /// P2P EMP client sending remote authorization start/stop tests.
    /// </summary>
    [TestFixture]
    public class RemoteReservationStartStopTests : AP2PTests
    {

        #region AuthorizeRemoteReservationStart_Test1()

        [Test]
        public async Task AuthorizeRemoteReservationStart_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStartRequest(
                              ProviderId:           Provider_Id.   Parse("DE-GDF"),
                              Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                              EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                              PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                              SessionId:            Session_Id.NewRandom,
                              CPOPartnerSessionId:  null,
                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                              CustomData:           null
                          );

            Assert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStart.Responses_Error);

                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);


                var oicpResult = await empP2P_DEGDF.AuthorizeRemoteReservationStart(request);

                Assert.IsNotNull(oicpResult);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (true,                oicpResult.Response?.Result);
                Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);


                Assert.AreEqual(1, empClient.                Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(1, empClient.                Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStart.Responses_Error);

                Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region AuthorizeRemoteReservationStart_Test2()

        [Test]
        public async Task AuthorizeRemoteReservationStart_Test2()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStartRequest(
                              ProviderId:           Provider_Id.   Parse("DE-GDF"),
                              Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                              EVSEId:               EVSE_Id.       Parse("DE*XXX*E1234567*A*1"),
                              PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                              SessionId:            Session_Id.NewRandom,
                              CPOPartnerSessionId:  null,
                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                              CustomData:           null
                          );

            Assert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Responses_Error);


                var oicpResult = await empP2P_DEGDF.AuthorizeRemoteReservationStart(request);

                Assert.IsNotNull(oicpResult);
                Assert.IsFalse  (oicpResult.IsSuccessful);
                Assert.AreEqual (false,                       oicpResult.Response?.Result);
                Assert.AreEqual (StatusCodes.NoValidContract, oicpResult.Response?.StatusCode.Code);


                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion


        #region AuthorizeRemoteReservationStop_Test1()

        [Test]
        public async Task AuthorizeRemoteReservationStop_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStopRequest(
                              ProviderId:           Provider_Id.Parse("DE-GDF"),
                              EVSEId:               EVSE_Id.    Parse("DE*GEF*E1234567*A*1"),
                              SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                              CPOPartnerSessionId:  null,
                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                              CustomData:           null
                          );

            Assert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStop.Requests_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStop.Requests_Error);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStop.Responses_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStop.Responses_Error);

                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);


                var oicpResult = await empP2P_DEGDF.AuthorizeRemoteReservationStop(request);

                Assert.IsNotNull(oicpResult);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (true,                oicpResult.Response?.Result);
                Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);


                Assert.AreEqual(1, empClient.                Counters.AuthorizeRemoteReservationStop.Requests_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStop.Requests_Error);
                Assert.AreEqual(1, empClient.                Counters.AuthorizeRemoteReservationStop.Responses_OK);
                Assert.AreEqual(0, empClient.                Counters.AuthorizeRemoteReservationStop.Responses_Error);

                Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
                Assert.AreEqual(1, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
                Assert.AreEqual(0, cpoP2P_DEGEF.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

        #region AuthorizeRemoteReservationStop_Test2()

        [Test]
        public async Task AuthorizeRemoteReservationStop_Test2()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStopRequest(
                              ProviderId:           Provider_Id.Parse("DE-GDF"),
                              EVSEId:               EVSE_Id.    Parse("DE*XXX*E1234567*A*1"),
                              SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                              CPOPartnerSessionId:  null,
                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                              CustomData:           null
                          );

            Assert.IsNotNull(request);

            if (empP2P_DEGDF.GetEMPClient(DEGEF_Id) is EMPClient empClient)
            {

                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStop.Requests_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStop.Requests_Error);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStop.Responses_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStop.Responses_Error);


                var oicpResult = await empP2P_DEGDF.AuthorizeRemoteReservationStop(request);

                Assert.IsNotNull(oicpResult);
                Assert.IsFalse  (oicpResult.IsSuccessful);
                Assert.AreEqual (false,                       oicpResult.Response?.Result);
                Assert.AreEqual (StatusCodes.NoValidContract, oicpResult.Response?.StatusCode.Code);


                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Requests_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Requests_Error);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Responses_OK);
                Assert.AreEqual(0, empClient.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            }
            else
                Assert.Fail("Missing EMPClient!");

        }

        #endregion

    }

}
