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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CentralService.CPO
{

    /// <summary>
    /// CPO sending AuthorizeStarts/-Stops tests.
    /// </summary>
    [TestFixture]
    public class AuthorizeStartStopTests : ACentralServiceTests
    {

        #region AuthorizeStart_Test1()

        [Test]
        public async Task AuthorizeStart_Test1()
        {

            if (cpoRoaming_DEGEF  is null ||
                centralServiceAPI is null)
            {
                Assert.Fail(nameof(cpoRoaming_DEGEF) + " or " + nameof(centralServiceAPI) + " is null!");
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

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

            var oicpResult  = await cpoRoaming_DEGEF.AuthorizeStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success,                                                  oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.Authorized,                                  oicpResult.Response?.AuthorizationStatus);
            ClassicAssert.AreEqual (Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),   oicpResult.Response?.SessionId);
            ClassicAssert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
            ClassicAssert.AreEqual (EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),   oicpResult.Response?.EMPPartnerSessionId);
            ClassicAssert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

            ClassicAssert.AreEqual (2,                                                                    oicpResult.Response?.AuthorizationStopIdentifications?.Count());
            ClassicAssert.AreEqual (UID.Parse("11223344"),                                                oicpResult.Response?.AuthorizationStopIdentifications?.ElementAt(0).RFIDId);
            ClassicAssert.AreEqual (UID.Parse("55667788"),                                                oicpResult.Response?.AuthorizationStopIdentifications?.ElementAt(1).RFIDId);

            ClassicAssert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(1, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

        }

        #endregion


        #region AuthorizeStop_Test1()

        [Test]
        public async Task AuthorizeStop_Test1()
        {

            if (cpoRoaming_DEGEF  is null ||
                centralServiceAPI is null)
            {
                Assert.Fail(nameof(cpoRoaming_DEGEF) + " or " + nameof(centralServiceAPI) + " is null!");
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

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_Error);

            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Responses_Error);

            var oicpResult  = await cpoRoaming_DEGEF.AuthorizeStop(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsNotNull(oicpResult.Response);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (StatusCodes.Success,                                                  oicpResult.Response?.StatusCode?.Code);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.Authorized,                                  oicpResult.Response?.AuthorizationStatus);
            ClassicAssert.AreEqual (Session_Id.          Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),   oicpResult.Response?.SessionId);
            ClassicAssert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
            ClassicAssert.AreEqual (EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),   oicpResult.Response?.EMPPartnerSessionId);
            ClassicAssert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

            ClassicAssert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(1, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStop.Responses_Error);

            ClassicAssert.AreEqual(1, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(1, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.CPOClientAPI.Counters.AuthorizeStop.Responses_Error);

        }

        #endregion


    }

}
