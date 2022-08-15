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

using NUnit.Framework;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OICPv2_3.p2p;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.P2P.Signed.CPO
{

    /// <summary>
    /// OICP P2P Crypto test defaults.
    /// </summary>
    public abstract class ASignedP2PTests : AP2PTests
    {

        #region Data

        protected readonly X9ECParameters secp256r1;

        #endregion

        #region Constructor(s)

        public ASignedP2PTests()
        {

            this.secp256r1 = SecNamedCurves.GetByName("secp256r1");

        }

        #endregion


        #region (override) SetupEachTest()

        [SetUp]
        public override void SetupEachTest()
        {

            base.SetupEachTest();

            if (cpoP2P_DEGEF is not null) {
                var keyPair_DEGEF = APeer.GenerateKeys(secp256r1);
                cpoP2P_DEGEF.PrivateKey = keyPair_DEGEF?.Private as ECPrivateKeyParameters;
                cpoP2P_DEGEF.PublicKey  = keyPair_DEGEF?.Public  as ECPublicKeyParameters;
            }

            if (empP2P_DEGDF is not null) {
                var keyPair_DEGDF = APeer.GenerateKeys(secp256r1);
                empP2P_DEGDF.PrivateKey = keyPair_DEGDF?.Private as ECPrivateKeyParameters;
                empP2P_DEGDF.PublicKey  = keyPair_DEGDF?.Public  as ECPublicKeyParameters;
            }

        }

        #endregion


        #region CryptoSerializer       (JSON, PrivateKey)

        public static JObject CryptoSerializer(JObject                 JSON,
                                               ECPrivateKeyParameters  PrivateKey)
        {

            var signer = SignerUtilities.GetSigner("NONEwithECDSA");
            signer.Init(true, PrivateKey);
            signer.BlockUpdate(SHA256.Create().ComputeHash(JSON.ToString(Newtonsoft.Json.Formatting.None,
                                                                         APeer.JSONDateTimeConverter).
                                                                ToUTF8Bytes()),
                               0, 32);

            JSON.Add(new JProperty("signature", Convert.ToBase64String(signer.GenerateSignature())));

            return JSON;

        }

        #endregion

        #region CryptoRequestParser<T> (JSON, PublicKey, Request)

        public static T CryptoRequestParser<T>(JObject                JSON,
                                               ECPublicKeyParameters  PublicKey,
                                               T                      Request)

            where T : IRequest

        {

            if (JSON["signature"]?.Value<String>() is String signatureTXT) {

                if (Request.CustomData?["signatureValidation"]?.Value<Boolean>() is not null)
                    Request.CustomData.Remove("signatureValidation");

                var json     = JObject.Parse(JSON.ToString(Newtonsoft.Json.Formatting.None,
                                                           APeer.JSONDateTimeConverter));
                json.Remove("signature");

                var verifier = SignerUtilities.GetSigner("NONEwithECDSA");
                verifier.Init(false, PublicKey);
                verifier.BlockUpdate(SHA256.Create().ComputeHash(json.ToString(Newtonsoft.Json.Formatting.None,
                                                                               APeer.JSONDateTimeConverter).
                                                                      ToUTF8Bytes()),
                                     0, 32);

                Request.CustomData ??= new();
                Request.CustomData?.Add("signatureValidation", verifier.VerifySignature(signatureTXT.FromBase64()));

                return Request;

            }
            else
            {

                Request.CustomData ??= new();
                Request.CustomData?.Add("signatureValidation", false);

                return Request;

            }

        }

        #endregion

        #region CryptoResponseParser<T>(JSON, PublicKey, Request)

        public static T CryptoResponseParser<T>(JObject                JSON,
                                                ECPublicKeyParameters  PublicKey,
                                                T                      Response)

            where T : IResponse

        {

            if (JSON["signature"]?.Value<String>() is String signatureTXT) {

                if (Response.CustomData?["signatureValidation"]?.Value<Boolean>() is not null)
                    JSON.Remove("signatureValidation");

                var json     = JObject.Parse(JSON.ToString(Newtonsoft.Json.Formatting.None,
                                                           APeer.JSONDateTimeConverter));
                json.Remove("signature");

                var verifier = SignerUtilities.GetSigner("NONEwithECDSA");
                verifier.Init(false, PublicKey);
                verifier.BlockUpdate(SHA256.Create().ComputeHash(json.ToString(Newtonsoft.Json.Formatting.None,
                                                                               APeer.JSONDateTimeConverter).
                                                                      ToUTF8Bytes()),
                                     0, 32);

                Response.CustomData ??= new();
                Response.CustomData?.Add("signatureValidation", verifier.VerifySignature(signatureTXT.FromBase64()));

                return Response;

            }
            else
            {

                Response.CustomData ??= new();
                Response.CustomData?.Add("signatureValidation", false);

                return Response;

            }

        }

        #endregion


    }

}
