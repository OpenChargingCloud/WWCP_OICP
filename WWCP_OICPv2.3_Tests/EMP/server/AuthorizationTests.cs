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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.server
{

    /// <summary>
    /// EMP authorization tests.
    /// </summary>
    [TestFixture]
    public class AuthorizationTests : AEMPTests
    {

        #region AuthorizeStart_UID_Test1()

        [Test]
        public async Task AuthorizeStart_UID_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                        Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                                        EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                        PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                        SessionId:            Session_Id.NewRandom(),
                                                        CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom(),
                                                        EMPPartnerSessionId:  null);

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.Authorized, oicpResult.Response?.AuthorizationStatus);

            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

        }

        #endregion

        #region AuthorizeStart_UID_Test2()

        [Test]
        public async Task AuthorizeStart_UID_Test2()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                        Identification:       Identification.FromUID(UID.Parse("CCDDAABB")),
                                                        EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                        PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                        SessionId:            Session_Id.NewRandom(),
                                                        CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom(),
                                                        EMPPartnerSessionId:  null);

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.NotAuthorized, oicpResult.Response?.AuthorizationStatus);

            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Responses_OK);    //!!!
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error); //!!!

        }

        #endregion


        #region AuthorizeStart_RFIDIdentification_Test1()

        [Test]
        public async Task AuthorizeStart_RFIDIdentification_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                        Identification:       Identification.FromRFIDIdentification(new RFIDIdentification(UID.Parse("AABBCCDD"), RFIDTypes.MifareClassic)),
                                                        EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                        PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                        SessionId:            Session_Id.NewRandom(),
                                                        CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom(),
                                                        EMPPartnerSessionId:  null);

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.Authorized, oicpResult.Response?.AuthorizationStatus);

            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

        }

        #endregion

        #region AuthorizeStart_RFIDIdentification_Test2()

        [Test]
        public async Task AuthorizeStart_RFIDIdentification_Test2()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                        Identification:       Identification.FromRFIDIdentification(new RFIDIdentification(UID.Parse("CCDDAABB"), RFIDTypes.MifareClassic)),
                                                        EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                        PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                        SessionId:            Session_Id.NewRandom(),
                                                        CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom(),
                                                        EMPPartnerSessionId:  null);

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.NotAuthorized, oicpResult.Response?.AuthorizationStatus);

            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStart.Responses_Error);

            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStart.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStart.Responses_Error);

        }

        #endregion



        //ToDo: More AuthorizeStop
        #region AuthorizeStop_UID_Test1()

        [Test]
        public async Task AuthorizeStop_UID_Test1()
        {

            if (empServerAPI       is null ||
                empServerAPIClient is null)
            {
                Assert.Fail("empServerAPI or empServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeStopRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                       Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                                       EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                       SessionId:            Session_Id.NewRandom(),
                                                       CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom(),
                                                       EMPPartnerSessionId:  null);

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStop.Responses_Error);

            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStop.Responses_Error);

            var oicpResult  = await empServerAPIClient.AuthorizeStop(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (AuthorizationStatusTypes.Authorized, oicpResult.Response?.AuthorizationStatus);

            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPIClient.Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPIClient.Counters.AuthorizeStop.Responses_Error);

            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStop.Requests_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStop.Requests_Error);
            ClassicAssert.AreEqual(1, empServerAPI.      Counters.AuthorizeStop.Responses_OK);
            ClassicAssert.AreEqual(0, empServerAPI.      Counters.AuthorizeStop.Responses_Error);

        }

        #endregion


    }

}
