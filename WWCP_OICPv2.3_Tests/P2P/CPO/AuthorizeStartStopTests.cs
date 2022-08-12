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

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.CPO
{

    /// <summary>
    /// P2P CPO sending AuthorizeStarts/-Stops tests.
    /// </summary>
    [TestFixture]
    public class AuthorizeStartStopTests : AP2PTests
    {

        #region AuthorizeStart_Test1()

        [Test]
        public async Task AuthorizeStart_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeStartRequest(
                              OperatorId:           Operator_Id.         Parse("DE*GEF"),
                              Identification:       Identification.FromUID(
                                                                     UID.Parse("11223344")
                                                    ),
                              EVSEId:               EVSE_Id.             Parse("DE*GEF*E1234567*A*1"),
                              PartnerProductId:     PartnerProduct_Id.AC1,
                              CPOPartnerSessionId:  CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              CustomData:           null
                          );

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_Error);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_Error);

            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

            var oicpResult  = await cpoP2P_DEGEF.AuthorizeStart(Provider_Id.Parse("DE*GDF"), request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success,                                                  oicpResult.Response?.StatusCode?.Code);
            Assert.AreEqual (AuthorizationStatusTypes.Authorized,                                  oicpResult.Response?.AuthorizationStatus);
            Assert.AreEqual (Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),   oicpResult.Response?.SessionId);
            Assert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
            Assert.AreEqual (EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),   oicpResult.Response?.EMPPartnerSessionId);
            Assert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

            Assert.AreEqual (2,                                                                    oicpResult.Response?.AuthorizationStopIdentifications?.Count());
            Assert.AreEqual (UID.Parse("11223344"),                                                oicpResult.Response?.AuthorizationStopIdentifications?.ElementAt(0).RFIDId);
            Assert.AreEqual (UID.Parse("55667788"),                                                oicpResult.Response?.AuthorizationStopIdentifications?.ElementAt(1).RFIDId);

            //Assert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_Error);
            //Assert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_Error);

            Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
            Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

        }

        #endregion


        #region AuthorizeStop_Test1()

        [Test]
        public async Task AuthorizeStop_Test1()
        {

            if (cpoP2P_DEGEF is null ||
                empP2P_DEGDF is null)
            {
                Assert.Fail(nameof(cpoP2P_DEGEF) + " or " + nameof(empP2P_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeStopRequest(
                              OperatorId:           Operator_Id.         Parse("DE*GEF"),
                              SessionId:            Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:       Identification.FromUID(
                                                                     UID.Parse("11223344")
                                                    ),
                              EVSEId:               EVSE_Id.             Parse("DE*GEF*E1234567*A*1"),
                              CPOPartnerSessionId:  CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:  EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              CustomData:           null
                          );

            Assert.IsNotNull(request);

            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_Error);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_Error);

            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Requests_Error);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Responses_Error);

            var oicpResult  = await cpoP2P_DEGEF.AuthorizeStop(Provider_Id.Parse("DE*GDF"), request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success,                                                  oicpResult.Response?.StatusCode?.Code);
            Assert.AreEqual (AuthorizationStatusTypes.Authorized,                                  oicpResult.Response?.AuthorizationStatus);
            Assert.AreEqual (Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),   oicpResult.Response?.SessionId);
            Assert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
            Assert.AreEqual (EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),   oicpResult.Response?.EMPPartnerSessionId);
            Assert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

            //Assert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_Error);
            //Assert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_Error);

            Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Requests_Error);
            Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStop.Responses_Error);

        }

        #endregion


    }

}
