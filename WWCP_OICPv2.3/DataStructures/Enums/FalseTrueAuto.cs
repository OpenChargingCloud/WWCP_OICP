/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extensions methods for FalseTrueAuto.
    /// </summary>
    public static class FalseTrueAutoExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a FalseTrueAuto type.
        /// </summary>
        /// <param name="Text">A text representation of a FalseTrueAuto type.</param>
        public static FalseTrueAuto Parse(String Text)
        {

            if (TryParse(Text, out var falseTrueAuto))
                return falseTrueAuto;

            throw new ArgumentException("Undefined FalseTrueAuto '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a FalseTrueAuto type.
        /// </summary>
        /// <param name="Text">A text representation of a FalseTrueAuto type.</param>
        public static FalseTrueAuto? TryParse(String Text)
        {

            if (TryParse(Text, out var falseTrueAuto))
                return falseTrueAuto;

            return default;

        }

        #endregion

        #region TryParse(Text, out FalseTrueAuto)

        /// <summary>
        /// Parses the given text representation of a FalseTrueAuto type.
        /// </summary>
        /// <param name="Text">A text representation of a FalseTrueAuto type.</param>
        /// <param name="FalseTrueAuto">The parsed FalseTrueAuto type.</param>
        public static Boolean TryParse(String Text, out FalseTrueAuto FalseTrueAuto)
        {
            switch (Text.Trim().ToLower())
            {

                case "false":
                    FalseTrueAuto = FalseTrueAuto.False;
                    return true;
 
                case "true":
                    FalseTrueAuto = FalseTrueAuto.True;
                    return true;

                case "auto":
                    FalseTrueAuto = FalseTrueAuto.Auto;
                    return true;

                default:
                    FalseTrueAuto = FalseTrueAuto.Auto;
                    return false;

            }
        }

        #endregion

        #region AsString(FalseTrueAuto)

        /// <summary>
        /// Return a text representation of the given FalseTrueAuto type.
        /// </summary>
        /// <param name="FalseTrueAuto">A FalseTrueAuto type.</param>
        public static String AsString(this FalseTrueAuto FalseTrueAuto)

            => FalseTrueAuto switch {
                   FalseTrueAuto.False  => "false",
                   FalseTrueAuto.True   => "true",
                   _                    => "auto",
               };

        #endregion

    }


    /// <summary>
    /// False|True|Auto
    /// </summary>
    public enum FalseTrueAuto
    {

        /// <summary>
        /// false
        /// </summary>
        False,

        /// <summary>
        /// true
        /// </summary>
        True,

        /// <summary>
        /// auto
        /// </summary>
        Auto

    }

}
