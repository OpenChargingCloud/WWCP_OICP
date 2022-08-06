﻿/*
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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CentralService.tests
{

    /// <summary>
    /// EMP client authorization tests.
    /// </summary>
    [TestFixture]
    public class EMPClientRemoteAuthorizationTests : ACentralServiceTests
    {

        #region AuthorizeRemoteStart_Test1()

        [Test]
        public async Task AuthorizeRemoteStart_Test1()
        {

            if (centralServiceAPI is null ||
                empClient         is null)
            {
                Assert.Fail("centralServiceAPI or empClient is null!");
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

            Assert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_Error);

            Assert.AreEqual(0, empClient.                     Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, empClient.                     Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(0, empClient.                     Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, empClient.                     Counters.AuthorizeRemoteStart.Responses_Error);

            var oicpResult = await empClient.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (true,                oicpResult.Response?.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            Assert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteStart.Responses_Error);

            Assert.AreEqual(1, empClient.                     Counters.AuthorizeRemoteStart.Requests_OK);
            Assert.AreEqual(0, empClient.                     Counters.AuthorizeRemoteStart.Requests_Error);
            Assert.AreEqual(1, empClient.                     Counters.AuthorizeRemoteStart.Responses_OK);
            Assert.AreEqual(0, empClient.                     Counters.AuthorizeRemoteStart.Responses_Error);

        }

        #endregion

    }

}
