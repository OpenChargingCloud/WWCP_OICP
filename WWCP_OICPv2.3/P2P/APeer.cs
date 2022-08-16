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

using Newtonsoft.Json.Converters;

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.p2p
{

    public abstract class APeer
    {

        #region Data

        /// <summary>
        /// Some JSON helper as DateTime is not well-defined for JSON!
        /// </summary>
        public static readonly IsoDateTimeConverter JSONDateTimeConverter = new() {
            DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"
        };

        #endregion

        #region Properties

        /// <summary>
        /// The main private key of this OICP peer.
        /// </summary>
        public ECPrivateKeyParameters?  PrivateKey    { get; set; }

        /// <summary>
        /// The main public key of this OICP peer.
        /// </summary>
        public ECPublicKeyParameters?   PublicKey     { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract OICP peer.
        /// </summary>
        /// <param name="KeyPair">An optional private/public-keypair.</param>
        public APeer(AsymmetricCipherKeyPair? KeyPair = null)
        {

            if (KeyPair is not null) {
                this.PrivateKey = KeyPair.Private as ECPrivateKeyParameters;
                this.PublicKey  = KeyPair.Public  as ECPublicKeyParameters;
            }

        }

        #endregion


        #region GenerateKeys(ECParameters)

        /// <summary>
        /// Generate a private/public key pair.
        /// </summary>
        /// <param name="ECParameters">The elliptic curve parameters to use.</param>
        public static AsymmetricCipherKeyPair GenerateKeys(X9ECParameters ECParameters)
        {

            var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
            generator.Init(new ECKeyGenerationParameters(new ECDomainParameters(ECParameters.Curve,
                                                                                ECParameters.G,
                                                                                ECParameters.N,
                                                                                ECParameters.H,
                                                                                ECParameters.GetSeed()),
                                                         new SecureRandom()));

            return generator.GenerateKeyPair();

        }

        #endregion


    }

}
