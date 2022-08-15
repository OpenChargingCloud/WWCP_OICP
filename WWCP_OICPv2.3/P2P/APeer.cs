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

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Newtonsoft.Json.Converters;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.p2p
{

    public abstract class APeer
    {

        #region Data

        public static readonly IsoDateTimeConverter JSONDateTimeConverter = new() {
                                                                                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"
                                                                            };

        #endregion

        #region Properties

        public ECPrivateKeyParameters?  PrivateKey    { get; set; }
        public ECPublicKeyParameters?   PublicKey     { get; set; }

        #endregion


        #region GenerateKeys(ECParameters)

        public AsymmetricCipherKeyPair GenerateKeys(X9ECParameters ECParameters)
        {

            var EllipticCurveSpec = new ECDomainParameters(ECParameters.Curve,
                                                           ECParameters.G,
                                                           ECParameters.N,
                                                           ECParameters.H,
                                                           ECParameters.GetSeed());

            var g = GeneratorUtilities.GetKeyPairGenerator("ECDH");
            g.Init(new ECKeyGenerationParameters(EllipticCurveSpec, new SecureRandom()));

            return g.GenerateKeyPair();

        }

        #endregion

    }

}
