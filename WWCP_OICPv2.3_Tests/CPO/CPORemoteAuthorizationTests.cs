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

using Newtonsoft.Json.Linq;
using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO.tests
{

    /// <summary>
    /// CPO remote authorization tests.
    /// </summary>
    [TestFixture]
    public class CPORemoteAuthorizationTests : ACPOTests
    {

        //ToDo: OperatorId != OperatorIdURL


        #region AuthorizeStart_UID_Test1()

        [Test]
        public async Task AuthorizeRemoteStart_UID_Test1()
        {

            if (cpoServerAPI       is null ||
                cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPI or cpoServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeRemoteStartRequest(ProviderId:           Provider_Id.Parse("DE*GDF"),
                                                              Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                                              EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                              PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                              SessionId:            Session_Id.NewRandom,
                                                              CPOPartnerSessionId:  null,
                                                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom);

            Assert.IsNotNull(request);

            var oicpResult  = await cpoServerAPIClient.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.WasSuccessful);
            Assert.AreEqual (true,                oicpResult.Response.Result);
            Assert.AreEqual (StatusCodes.Success, oicpResult.Response.StatusCode.Code);

        }

        #endregion

        #region AuthorizeRemoteStart_UID_Test2()

        [Test]
        public async Task AuthorizeRemoteStart_UID_Test2()
        {

            if (cpoServerAPI       is null ||
                cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPI or cpoServerAPIClient is null!");
                return;
            }

            var request     = new AuthorizeRemoteStartRequest(ProviderId:           Provider_Id.Parse("DE*GDF"),
                                                              Identification:       Identification.FromUID(UID.Parse("CCDDAABB")),
                                                              EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                              PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                              SessionId:            Session_Id.NewRandom,
                                                              CPOPartnerSessionId:  null,
                                                              EMPPartnerSessionId:  EMPPartnerSession_Id.NewRandom);

            Assert.IsNotNull(request);

            var oicpResult  = await cpoServerAPIClient.AuthorizeRemoteStart(request);

            Assert.IsNotNull(oicpResult);
            Assert.IsTrue   (oicpResult.WasSuccessful);
            Assert.AreEqual (false,                                 oicpResult.Response.Result);
            Assert.AreEqual (StatusCodes.CommunicationToEVSEFailed, oicpResult.Response.StatusCode.Code);

        }

        #endregion

    }

}
