/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.Signed.CPO
{

    /// <summary>
    /// P2P CPO sending cryptographical signed messages tests.
    /// </summary>
    [TestFixture]
    public class SignedMessagesTests : ASignedP2PTests
    {

        #region SignedAuthorizeStart_Test1()

        [Test]
        public async Task SignedAuthorizeStart_Test1()
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

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);


                if (cpoP2P_DEGEF.PrivateKey is not null)
                    cpoClient.CustomAuthorizeStartRequestSerializer               = (authorizeStartRequest, json)      =>
                        CryptoSerializer(json,
                                         cpoP2P_DEGEF.PrivateKey);

                if (cpoP2P_DEGEF.PublicKey  is not null)
                    empP2P_DEGDF.CPOClientAPI.CustomAuthorizeStartRequestParser   = (json, authorizeStartRequest)      =>
                        CryptoRequestParser(json,
                                            cpoP2P_DEGEF.PublicKey,
                                            authorizeStartRequest);

                if (empP2P_DEGDF.PrivateKey is not null)
                    empP2P_DEGDF.CPOClientAPI.CustomAuthorizationStartSerializer  = (authorizationStartResponse, json) => {

                        json.Add("requestSignatureValidation", authorizationStartResponse.Request?.CustomData?["signatureValidation"]?.Value<Boolean>());

                        return CryptoSerializer(json,
                                                empP2P_DEGDF.PrivateKey);

                    };

                if (empP2P_DEGDF.PublicKey  is not null)
                    cpoClient.CustomAuthorizationStartResponseParser              = (json, authorizationStartResponse) => {

                        var requestSignatureValidationValue = json["requestSignatureValidation"]?.Value<Boolean>();

                        if (requestSignatureValidationValue is not null)
                        {
                            authorizationStartResponse.CustomData ??= new();
                            authorizationStartResponse.CustomData.Add("requestSignatureValidation", requestSignatureValidationValue);
                        }

                        return CryptoResponseParser(json,
                                                    empP2P_DEGDF.PublicKey,
                                                    authorizationStartResponse);

                    };


                var oicpResult  = await cpoP2P_DEGEF.AuthorizeStart(DEGDF_Id, request);


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


                Assert.IsTrue   (oicpResult.Response?.CustomData?["requestSignatureValidation"]?.Value<Boolean>());
                Assert.IsTrue   (oicpResult.Response?.CustomData?["signatureValidation"]?.       Value<Boolean>());


                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region SignedAuthorizeStart_MissingSignature_Test1()

        [Test]
        public async Task SignedAuthorizeStart_MissingSignature_Test1()
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

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);


                if (cpoP2P_DEGEF.PublicKey  is not null)
                    empP2P_DEGDF.CPOClientAPI.CustomAuthorizeStartRequestParser  = (json, authorizeStartRequest) =>
                        CryptoRequestParser(json,
                                            cpoP2P_DEGEF.PublicKey,
                                            authorizeStartRequest);


                var oicpResult  = await cpoP2P_DEGEF.AuthorizeStart(DEGDF_Id, request);


                Assert.IsNotNull(oicpResult);
                Assert.IsNotNull(oicpResult.Response);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (StatusCodes.NoPositiveAuthenticationResponse,                         oicpResult.Response?.StatusCode?.Code);
                Assert.AreEqual ("Invalid crypto signature!",                                          oicpResult.Response?.StatusCode?.Description);
                Assert.AreEqual (AuthorizationStatusTypes.NotAuthorized,                               oicpResult.Response?.AuthorizationStatus);
                Assert.IsNull   (oicpResult.Response?.SessionId);
                Assert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
                Assert.IsNull   (oicpResult.Response?.EMPPartnerSessionId);
                Assert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

                Assert.AreEqual (0,                                                                    oicpResult.Response?.AuthorizationStopIdentifications?.Count());


                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region SignedAuthorizeStart_InvalidSignature_Test1()

        [Test]
        public async Task SignedAuthorizeStart_InvalidSignature_Test1()
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

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);


                if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient DEGDF)
                {

                    if (cpoP2P_DEGEF.PrivateKey is not null)
                        DEGDF.CustomAuthorizeStartRequestSerializer  = (authorizeStartRequest, json) => {
                            json.Add("signature", "1234");
                            return json;
                        };

                }

                if (cpoP2P_DEGEF.PublicKey  is not null)
                    empP2P_DEGDF.CPOClientAPI.CustomAuthorizeStartRequestParser  = (json, authorizeStartRequest) =>
                        CryptoRequestParser(json,
                                            cpoP2P_DEGEF.PublicKey,
                                            authorizeStartRequest);


                var oicpResult  = await cpoP2P_DEGEF.AuthorizeStart(DEGDF_Id, request);


                Assert.IsNotNull(oicpResult);
                Assert.IsNotNull(oicpResult.Response);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (StatusCodes.NoPositiveAuthenticationResponse,                         oicpResult.Response?.StatusCode?.Code);
                Assert.AreEqual ("Invalid crypto signature!",                                          oicpResult.Response?.StatusCode?.Description);
                Assert.AreEqual (AuthorizationStatusTypes.NotAuthorized,                               oicpResult.Response?.AuthorizationStatus);
                Assert.IsNull   (oicpResult.Response?.SessionId);
                Assert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
                Assert.IsNull   (oicpResult.Response?.EMPPartnerSessionId);
                Assert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

                Assert.AreEqual (0,                                                                    oicpResult.Response?.AuthorizationStopIdentifications?.Count());


                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

        #region SignedAuthorizeStart_FakeSignatureValidation_Test1()

        [Test]
        public async Task SignedAuthorizeStart_FakeSignatureValidation_Test1()
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
                              CustomData:           new JObject(
                                                        new JProperty("signatureValidation", true)
                                                    )
                          );

            Assert.IsNotNull(request);

            if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient cpoClient)
            {

                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);


                if (cpoP2P_DEGEF.GetCPOClient(DEGDF_Id) is CPOClient DEGDF)
                {

                    if (cpoP2P_DEGEF.PrivateKey is not null)
                        DEGDF.CustomAuthorizeStartRequestSerializer  = (authorizeStartRequest, json) => {
                            json.Add("signature", "1234");
                            return json;
                        };

                }

                if (cpoP2P_DEGEF.PublicKey  is not null)
                    empP2P_DEGDF.CPOClientAPI.CustomAuthorizeStartRequestParser  = (json, authorizeStartRequest) =>
                        CryptoRequestParser(json,
                                            cpoP2P_DEGEF.PublicKey,
                                            authorizeStartRequest);


                var oicpResult  = await cpoP2P_DEGEF.AuthorizeStart(DEGDF_Id, request);


                Assert.IsNotNull(oicpResult);
                Assert.IsNotNull(oicpResult.Response);
                Assert.IsTrue   (oicpResult.IsSuccessful);
                Assert.AreEqual (StatusCodes.NoPositiveAuthenticationResponse,                         oicpResult.Response?.StatusCode?.Code);
                Assert.AreEqual ("Invalid crypto signature!",                                          oicpResult.Response?.StatusCode?.Description);
                Assert.AreEqual (AuthorizationStatusTypes.NotAuthorized,                               oicpResult.Response?.AuthorizationStatus);
                Assert.IsNull   (oicpResult.Response?.SessionId);
                Assert.AreEqual (CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),   oicpResult.Response?.CPOPartnerSessionId);
                Assert.IsNull   (oicpResult.Response?.EMPPartnerSessionId);
                Assert.AreEqual (Provider_Id.         Parse("DE-GDF"),                                 oicpResult.Response?.ProviderId);

                Assert.AreEqual (0,                                                                    oicpResult.Response?.AuthorizationStopIdentifications?.Count());


                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, cpoClient.                Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, cpoClient.                Counters.AuthorizeStart.Responses_Error);

                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
                Assert.AreEqual(1, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
                Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);

            }
            else
                Assert.Fail("Missing CPOClient!");

        }

        #endregion

    }

}
