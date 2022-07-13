/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Extentions methods for PIN cryptos.
    /// </summary>
    public static class PINCryptoExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a PIN crypto.
        /// </summary>
        /// <param name="Text">A text-representation of a PIN crypto.</param>
        public static PINCrypto Parse(String Text)

            => Text?.Trim() switch {
                   "MD5"   => PINCrypto.MD5,
                   "SHA1"  => PINCrypto.SHA1,
                   _       => PINCrypto.None,
               };

        #endregion

        #region AsString(ValueAddedService)

        /// <summary>
        /// Return a text-representation of the given PIN crypto.
        /// </summary>
        /// <param name="PINCrypto">A PIN crypto.</param>
        public static String AsString(this PINCrypto PINCrypto)

            => PINCrypto switch {
                   PINCrypto.MD5   => "MD5",
                   PINCrypto.SHA1  => "SHA1",
                   _               => "none",
               };

        #endregion

    }

    /// <summary>
    /// PIN crypto for (remote) authentication.
    /// </summary>
    public enum PINCrypto
    {

        /// <summary>
        /// The PIN is unencrypted.
        /// </summary>
        None,

        /// <summary>
        /// The PIN is hashed via MD5.
        /// </summary>
        MD5,

        /// <summary>
        /// The PIN is hashed via SHA-1.
        /// </summary>
        SHA1

    }

}
