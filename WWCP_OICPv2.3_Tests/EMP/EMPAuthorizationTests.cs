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

namespace cloud.charging.open.protocols.OICPv2_3.EMP.tests
{

    /// <summary>
    /// EMP authorization tests.
    /// </summary>
    [TestFixture]
    public class EMPAuthorizationTests : AEMPTests
    {

        //ToDo: OperatorId != OperatorIdURL


        #region AuthorizeStart_UID_Test1()

        [Test]
        public async Task AuthorizeStart_UID_Test1()
        {

            var request       = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                          Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                                          EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                          EMPPartnerSessionId:  null);

            Assert.IsNotNull(request);

            var httpresult    = await SendEMPAuthorizeStart(request);

            Assert.IsNotNull(httpresult);
            Assert.AreEqual (HTTPStatusCode.OK, httpresult.HTTPStatusCode);

            var jsonResponse  = JObject.Parse(httpresult.HTTPBody.ToUTF8String());

            Assert.IsNotNull(jsonResponse);

            var response      = AuthorizationStartResponse.Parse(request,
                                                                 jsonResponse);

            Assert.IsNotNull(response);
            Assert.AreEqual(AuthorizationStatusTypes.Authorized, response.AuthorizationStatus);

        }

        #endregion

        #region AuthorizeStart_UID_Test2()

        [Test]
        public async Task AuthorizeStart_UID_Test2()
        {

            var request       = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                          Identification:       Identification.FromUID(UID.Parse("CCDDAABB")),
                                                          EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                          EMPPartnerSessionId:  null);

            Assert.IsNotNull(request);

            var httpresult    = await SendEMPAuthorizeStart(request);

            Assert.IsNotNull(httpresult);
            Assert.AreEqual (HTTPStatusCode.OK, httpresult.HTTPStatusCode);

            var jsonResponse  = JObject.Parse(httpresult.HTTPBody.ToUTF8String());

            Assert.IsNotNull(jsonResponse);

            var response      = AuthorizationStartResponse.Parse(request,
                                                                 jsonResponse);

            Assert.IsNotNull(response);
            Assert.AreEqual(AuthorizationStatusTypes.NotAuthorized, response.AuthorizationStatus);

        }

        #endregion


        #region AuthorizeStart_RFIDIdentification_Test1()

        [Test]
        public async Task AuthorizeStart_RFIDIdentification_Test1()
        {

            var request       = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                          Identification:       Identification.FromRFIDIdentification(new RFIDIdentification(UID.Parse("AABBCCDD"), RFIDTypes.MifareClassic)),
                                                          EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                          EMPPartnerSessionId:  null);

            Assert.IsNotNull(request);

            var httpresult    = await SendEMPAuthorizeStart(request);

            Assert.IsNotNull(httpresult);
            Assert.AreEqual (HTTPStatusCode.OK, httpresult.HTTPStatusCode);

            var jsonResponse  = JObject.Parse(httpresult.HTTPBody.ToUTF8String());

            Assert.IsNotNull(jsonResponse);

            var response      = AuthorizationStartResponse.Parse(request,
                                                                 jsonResponse);

            Assert.IsNotNull(response);
            Assert.AreEqual(AuthorizationStatusTypes.Authorized, response.AuthorizationStatus);

        }

        #endregion

        #region AuthorizeStart_RFIDIdentification_Test2()

        [Test]
        public async Task AuthorizeStart_RFIDIdentification_Test2()
        {

            var request       = new AuthorizeStartRequest(OperatorId:           Operator_Id.Parse("DE*GEF"),
                                                          Identification:       Identification.FromRFIDIdentification(new RFIDIdentification(UID.Parse("CCDDAABB"), RFIDTypes.MifareClassic)),
                                                          EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*1"),
                                                          PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                                          SessionId:            Session_Id.NewRandom,
                                                          CPOPartnerSessionId:  CPOPartnerSession_Id.NewRandom,
                                                          EMPPartnerSessionId:  null);

            Assert.IsNotNull(request);

            var httpresult    = await SendEMPAuthorizeStart(request);

            Assert.IsNotNull(httpresult);
            Assert.AreEqual (HTTPStatusCode.OK, httpresult.HTTPStatusCode);

            var jsonResponse  = JObject.Parse(httpresult.HTTPBody.ToUTF8String());

            Assert.IsNotNull(jsonResponse);

            var response      = AuthorizationStartResponse.Parse(request,
                                                                 jsonResponse);

            Assert.IsNotNull(response);
            Assert.AreEqual(AuthorizationStatusTypes.NotAuthorized, response.AuthorizationStatus);

        }

        #endregion


    }

}
