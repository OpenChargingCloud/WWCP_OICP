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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.client
{

    /// <summary>
    /// EMP sending reservation start/stop tests.
    /// </summary>
    [TestFixture]
    public class ReservationStartStopTests : AEMPClientAPITests
    {

        #region AuthorizeRemoteReservationStart_Test1()

        [Test]
        public async Task AuthorizeRemoteReservationStart_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStartRequest(
                              ProviderId:           Provider_Id.   Parse("DE-GDF"),
                              EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                              Identification:       Identification.FromUID(UID.Parse("11223344")),
                              SessionId:            Session_Id.          NewRandom,
                              CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,
                              PartnerProductId:     PartnerProduct_Id.AC1,
                              Duration:             TimeSpan.FromMinutes(5),

                              CustomData:           null,

                              Timestamp:            Timestamp.Now,
                              CancellationToken:    null,
                              EventTrackingId:      EventTracking_Id.New,
                              RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStart.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStart.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStart.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStart.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            var oicpResult  = await empClient.AuthorizeRemoteReservationStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, empClient.   Counters.AuthorizeRemoteReservationStart.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStart.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.AuthorizeRemoteReservationStart.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStart.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

        }

        #endregion


        #region AuthorizeRemoteReservationStop_Test1()

        [Test]
        public async Task AuthorizeRemoteReservationStop_Test1()
        {

            if (empClientAPI is null ||
                empClient    is null)
            {
                Assert.Fail("empClientAPI or empClient is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStopRequest(
                              ProviderId:           Provider_Id.   Parse("DE-GDF"),
                              EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                              SessionId:            Session_Id.          NewRandom,
                              CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom,

                              CustomData:           null,

                              Timestamp:            Timestamp.Now,
                              CancellationToken:    null,
                              EventTrackingId:      EventTracking_Id.New,
                              RequestTimeout:       TimeSpan.FromSeconds(10));

            Assert.IsNotNull(request);

            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStop.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStop.Requests_Error);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStop.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStop.Responses_Error);

            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

            var oicpResult  = await empClient.AuthorizeRemoteReservationStop(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsNotNull(oicpResult.Response);
            Assert.IsTrue   (oicpResult.IsSuccessful);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode?.Code);
            Assert.IsTrue   (oicpResult.Response?.Result);

            Assert.AreEqual(1, empClient.   Counters.AuthorizeRemoteReservationStop.Requests_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStop.Requests_Error);
            Assert.AreEqual(1, empClient.   Counters.AuthorizeRemoteReservationStop.Responses_OK);
            Assert.AreEqual(0, empClient.   Counters.AuthorizeRemoteReservationStop.Responses_Error);

            Assert.AreEqual(1, empClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
            Assert.AreEqual(1, empClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
            Assert.AreEqual(0, empClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

        }

        #endregion

    }

}
