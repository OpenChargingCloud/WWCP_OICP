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

using System.Security.Cryptography;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OICPv2_3.CPO;
using Newtonsoft.Json.Converters;
using cloud.charging.open.protocols.OICPv2_3.p2p;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.Signed.CPO
{

    /// <summary>
    /// P2P CPO sending AuthorizeStarts/-Stops tests.
    /// </summary>
    [TestFixture]
    public class SignedMessagesTests : AP2PTests
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

            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Requests_Error);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_OK);
            //Assert.AreEqual(0, cpoRoaming_DEGEF.CPOClient.    Counters.AuthorizeStart.Responses_Error);

            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Requests_Error);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_OK);
            Assert.AreEqual(0, empP2P_DEGDF.CPOClientAPI.Counters.AuthorizeStart.Responses_Error);



            var keyPair_DEGEF = cpoP2P_DEGEF.GenerateKeys(SecNamedCurves.GetByName("secp256r1"));
            cpoP2P_DEGEF.PrivateKey = keyPair_DEGEF?.Private as ECPrivateKeyParameters;
            cpoP2P_DEGEF.PublicKey  = keyPair_DEGEF?.Public  as ECPublicKeyParameters;

            var keyPair_DEGDF = empP2P_DEGDF.GenerateKeys(SecNamedCurves.GetByName("secp256r1"));
            empP2P_DEGDF.PrivateKey = keyPair_DEGDF?.Private as ECPrivateKeyParameters;
            empP2P_DEGDF.PublicKey  = keyPair_DEGDF?.Public  as ECPublicKeyParameters;

            if (cpoP2P_DEGEF.GetProvider(Provider_Id.Parse("DE-GDF")) is CPOClient DEGDF)
            {

                DEGDF.CustomAuthorizeStartRequestSerializer  = (authorizeStartRequest, json) => {

                    if (cpoP2P_DEGEF.PrivateKey is not null) {

                        var signer = SignerUtilities.GetSigner("NONEwithECDSA");
                        signer.Init(true, cpoP2P_DEGEF.PrivateKey);
                        signer.BlockUpdate(SHA256.Create().ComputeHash(json.ToString(Newtonsoft.Json.Formatting.None,
                                                                                     APeer.JSONDateTimeConverter).
                                                                            ToUTF8Bytes()),
                                           0, 32);

                        json.Add(new JProperty("signature", Convert.ToBase64String(signer.GenerateSignature())));

                    }

                    return json;

                };

                DEGDF.CustomAuthorizationStartResponseParser = (json, authorizationStartResponse) => {

                    if (json["signature"]?.Value<String>() is String signatureTXT) {

                        if (json["signatureValidation"] is not null)
                            json.Remove("signatureValidation");

                        var json2    = JObject.Parse(json.ToString(Newtonsoft.Json.Formatting.None,
                                                                   APeer.JSONDateTimeConverter));
                        json2.Remove("signature");

                        var verifier = SignerUtilities.GetSigner("NONEwithECDSA");
                        verifier.Init(false, empP2P_DEGDF.PublicKey);
                        verifier.BlockUpdate(SHA256.Create().ComputeHash(json2.ToString(Newtonsoft.Json.Formatting.None,
                                                                                        APeer.JSONDateTimeConverter).
                                                                               ToUTF8Bytes()),
                                             0, 32);

                        authorizationStartResponse.CustomData ??= new();
                        authorizationStartResponse.CustomData?.Add("signatureValidation", verifier.VerifySignature(signatureTXT.FromBase64()));

                    }

                    return authorizationStartResponse;

                };

            }

            empP2P_DEGDF.CPOClientAPI.CustomAuthorizeStartRequestParser  = (json, authorizeStartRequest) => {

                if (json["signature"]?.Value<String>() is String signatureTXT) {

                    if (json["signatureValidation"] is not null)
                        json.Remove("signatureValidation");

                    var json2    = JObject.Parse(json.ToString(Newtonsoft.Json.Formatting.None,
                                                               APeer.JSONDateTimeConverter));
                    json2.Remove("signature");

                    var verifier = SignerUtilities.GetSigner("NONEwithECDSA");
                    verifier.Init(false, cpoP2P_DEGEF.PublicKey);
                    verifier.BlockUpdate(SHA256.Create().ComputeHash(json2.ToString(Newtonsoft.Json.Formatting.None,
                                                                                    APeer.JSONDateTimeConverter).
                                                                           ToUTF8Bytes()),
                                         0, 32);

                    authorizeStartRequest.CustomData ??= new();
                    authorizeStartRequest.CustomData?.Add("signatureValidation", verifier.VerifySignature(signatureTXT.FromBase64()));

                }

                return authorizeStartRequest;

            };

            empP2P_DEGDF.CPOClientAPI.CustomAuthorizationStartSerializer = (authorizationStartResponse, json) => {

                if (cpoP2P_DEGEF.PrivateKey is not null) {

                    var signer = SignerUtilities.GetSigner("NONEwithECDSA");
                    signer.Init(true, empP2P_DEGDF.PrivateKey);
                    signer.BlockUpdate(SHA256.Create().ComputeHash(json.ToString(Newtonsoft.Json.Formatting.None,
                                                                                 APeer.JSONDateTimeConverter).
                                                                        ToUTF8Bytes()),
                                       0, 32);

                    json.Add(new JProperty("signature", Convert.ToBase64String(signer.GenerateSignature())));

                }

                return json;

            };



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

            Assert.IsTrue   (oicpResult.Response?.CustomData?["signatureValidation"]?.Value<Boolean>());


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


    }

}
