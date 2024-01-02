﻿/*
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

namespace cloud.charging.open.protocols.OICPv2_3.tests.CentralService.EMP
{

    /// <summary>
    /// EMP client sending remote reservation authorization start/stop tests.
    /// </summary>
    [TestFixture]
    public class ReservationStartStopTests : ACentralServiceTests
    {

        #region AuthorizeRemoteReservationStart_Test1()

        [Test]
        public async Task AuthorizeRemoteReservationStart_Test1()
        {

            if (centralServiceAPI is null ||
                empRoaming_DEGDF  is null)
            {
                Assert.Fail(nameof(centralServiceAPI) + " or " + nameof(empRoaming_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStartRequest(ProviderId:           Provider_Id.   Parse("DE-GDF"),
                                                                     Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                                                                     EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                                                                     PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                                     SessionId:            Session_Id.NewRandom(),
                                                                     CPOPartnerSessionId:  null,
                                                                     EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom(),

                                                                     CustomData:           null,
                                                                     RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_Error);

            var oicpResult = await empRoaming_DEGDF.AuthorizeRemoteReservationStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (true,                oicpResult.Response?.Result);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_Error);

        }

        #endregion

        #region AuthorizeRemoteReservationStart_Test2()

        [Test]
        public async Task AuthorizeRemoteReservationStart_Test2()
        {

            if (centralServiceAPI is null ||
                empRoaming_DEGDF  is null)
            {
                Assert.Fail(nameof(centralServiceAPI) + " or " + nameof(empRoaming_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStartRequest(ProviderId:           Provider_Id.   Parse("DE-GDF"),
                                                                     Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                                                                     EVSEId:               EVSE_Id.       Parse("DE*XXX*E1234567*A*1"),
                                                                     PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                                     SessionId:            Session_Id.NewRandom(),
                                                                     CPOPartnerSessionId:  null,
                                                                     EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom(),

                                                                     CustomData:           null,
                                                                     RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_Error);

            var oicpResult = await empRoaming_DEGDF.AuthorizeRemoteReservationStart(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (false,                       oicpResult.Response?.Result);
            ClassicAssert.AreEqual (StatusCodes.NoValidContract, oicpResult.Response?.StatusCode.Code);

            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStart.Responses_Error);

            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Requests_Error);
            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStart.Responses_Error);

        }

        #endregion


        #region AuthorizeRemoteReservationStop_Test1()

        [Test]
        public async Task AuthorizeRemoteReservationStop_Test1()
        {

            if (centralServiceAPI is null ||
                empRoaming_DEGDF  is null)
            {
                Assert.Fail(nameof(centralServiceAPI) + " or " + nameof(empRoaming_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStopRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                                    EVSEId:               EVSE_Id.    Parse("DE*GEF*E1234567*A*1"),
                                                                    SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                                                                    CPOPartnerSessionId:  null,
                                                                    EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom(),
                                                                    CustomData:           null,

                                                                    RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_Error);

            var oicpResult = await empRoaming_DEGDF.AuthorizeRemoteReservationStop(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (true,                oicpResult.Response?.Result);
            ClassicAssert.AreEqual (StatusCodes.Success, oicpResult.Response?.StatusCode.Code);

            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_Error);

        }

        #endregion

        #region AuthorizeRemoteReservationStop_Test2()

        [Test]
        public async Task AuthorizeRemoteReservationStop_Test2()
        {

            if (centralServiceAPI is null ||
                empRoaming_DEGDF  is null)
            {
                Assert.Fail(nameof(centralServiceAPI) + " or " + nameof(empRoaming_DEGDF) + " is null!");
                return;
            }

            var request = new AuthorizeRemoteReservationStopRequest(ProviderId:           Provider_Id.Parse("DE-GDF"),
                                                                    EVSEId:               EVSE_Id.    Parse("DE*XXX*E1234567*A*1"),
                                                                    SessionId:            Session_Id. Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85"),
                                                                    CPOPartnerSessionId:  null,
                                                                    EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom(),
                                                                    CustomData:           null,

                                                                    RequestTimeout:       TimeSpan.FromSeconds(10));

            ClassicAssert.IsNotNull(request);

            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_Error);

            var oicpResult = await empRoaming_DEGDF.AuthorizeRemoteReservationStop(request);

            ClassicAssert.IsNotNull(oicpResult);
            ClassicAssert.IsTrue   (oicpResult.IsSuccessful);
            ClassicAssert.AreEqual (false,                       oicpResult.Response?.Result);
            ClassicAssert.AreEqual (StatusCodes.NoValidContract, oicpResult.Response?.StatusCode.Code);

            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(1, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, centralServiceAPI.EMPClientAPI.Counters.AuthorizeRemoteReservationStop.Responses_Error);

            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Requests_Error);
            ClassicAssert.AreEqual(1, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_OK);
            ClassicAssert.AreEqual(0, empRoaming_DEGDF. EMPClient.   Counters.AuthorizeRemoteReservationStop.Responses_Error);

        }

        #endregion

    }

}
