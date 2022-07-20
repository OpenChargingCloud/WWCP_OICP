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

using System;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO.tests
{

    /// <summary>
    /// CPO authorize remote start/stop tests.
    /// </summary>
    [TestFixture]
    public class CPOAuthorizeRemoteStartStopTests : ACPOTests
    {

        #region AuthorizeRemoteStart_Test1()

        [Test]
        public async Task AuthorizeRemoteStart_Test1()
        {

            if (cpoServerAPI       is null ||
                cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPI or cpoServerAPIClient is null!");
                return;
            }

            var request = new AuthorizeRemoteStartRequest(ProviderId:           Provider_Id.   Parse("DE*GDF"),
                                                          Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                                                          EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  null,
                                                          EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                          CustomData:           null,
                                                          Timestamp:            Timestamp.Now,
                                                          CancellationToken:    null,
                                                          EventTrackingId:      EventTracking_Id.New,
                                                          RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error);

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.WasSuccessful);
            Assert.AreEqual (true,                oicpResult.Response.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response.StatusCode.Code);

            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error);

        }

        #endregion

        #region AuthorizeRemoteStart_Test2()

        [Test]
        public async Task AuthorizeRemoteStart_Test2()
        {

            if (cpoServerAPI       is null ||
                cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPI or cpoServerAPIClient is null!");
                return;
            }

            var request = new AuthorizeRemoteStartRequest(ProviderId:           Provider_Id.Parse("DE*GDF"),
                                                          Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C22222222X")),
                                                          EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*2"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  null,
                                                          EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                          CustomData:           null,
                                                          Timestamp:            Timestamp.Now,
                                                          CancellationToken:    null,
                                                          EventTrackingId:      EventTracking_Id.New,
                                                          RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error);

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.WasSuccessful);
            Assert.AreEqual (false,                                 oicpResult.Response.Result);
            Assert.AreEqual (StatusCodes.CommunicationToEVSEFailed, oicpResult.Response.StatusCode.Code);

            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error);

        }

        #endregion


        #region AuthorizeRemoteStop_Test1()

        [Test]
        public async Task AuthorizeRemoteStop_Test1()
        {

            if (cpoServerAPI       is null ||
                cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPI or cpoServerAPIClient is null!");
                return;
            }

            var request = new AuthorizeRemoteStopRequest(ProviderId:           Provider_Id.Parse("DE*GDF"),
                                                         EVSEId:               EVSE_Id.    Parse("DE*GEF*E1234567*1"),
                                                         SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                                                         CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                         EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                         CustomData:           null,
                                                         Timestamp:            Timestamp.Now,
                                                         CancellationToken:    null,
                                                         EventTrackingId:      EventTracking_Id.New,
                                                         RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error);

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStop(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.WasSuccessful);
            Assert.AreEqual (true,                oicpResult.Response.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response.StatusCode.Code);

            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error);
            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error);

        }

        #endregion

        #region AuthorizeRemoteStop_Test2()

        [Test]
        public async Task AuthorizeRemoteStop_Test2()
        {

            if (cpoServerAPI       is null ||
                cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPI or cpoServerAPIClient is null!");
                return;
            }

            var request = new AuthorizeRemoteStopRequest(ProviderId:           Provider_Id.Parse("DE*GDF"),
                                                         EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*2"),
                                                         SessionId:            Session_Id. Parse("ae8f35a6-23d4-4b37-1994-21314c83e85c"),
                                                         CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                         EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                                                         CustomData:           null,
                                                         Timestamp:            Timestamp.Now,
                                                         CancellationToken:    null,
                                                         EventTrackingId:      EventTracking_Id.New,
                                                         RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error);

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStop(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.WasSuccessful);
            Assert.AreEqual (false,                                 oicpResult.Response.Result);
            Assert.AreEqual (StatusCodes.CommunicationToEVSEFailed, oicpResult.Response.StatusCode.Code);

            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error);
            Assert.AreEqual(1, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK);
            Assert.AreEqual(0, cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error);

        }

        #endregion

    }

}
