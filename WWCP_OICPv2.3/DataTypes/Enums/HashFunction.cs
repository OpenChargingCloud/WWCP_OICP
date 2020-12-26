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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extentions methods for hash functions.
    /// </summary>
    public static class HashFunctionExtentions
    {

        #region Parse(HashFunction)

        /// <summary>
        /// Parses the given text representation of a hash function.
        /// </summary>
        /// <param name="Text">A text representation of a hash function.</param>
        public static HashFunction Parse(String Text)

            => Text?.Trim() switch {
                   "Bcrypt"  => HashFunction.Bcrypt,
                   _         => HashFunction.Undefined,
               };

        #endregion

        #region AsString(HashFunction)

        /// <summary>
        /// Return a text representation of the given hash function.
        /// </summary>
        /// <param name="HashFunction">A hash function.</param>
        public static String AsString(this HashFunction HashFunction)

            => HashFunction switch {
                   HashFunction.Bcrypt  => "Bcrypt",
                   _                    => "Undefined",
               };

        #endregion

    }


    /// <summary>
    /// A hash function.
    /// </summary>
    public enum HashFunction
    {

        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// Bcrypt
        /// </summary>
        Bcrypt

    }

}
