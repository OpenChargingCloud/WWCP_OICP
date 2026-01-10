/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extensions methods for power types.
    /// </summary>
    public static class PowerTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a power type.
        /// </summary>
        /// <param name="Text">A text representation of a power type.</param>
        public static PowerTypes Parse(String Text)
        {

            if (TryParse(Text, out var powerType))
                return powerType;

            throw new ArgumentException("Undefined power type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a power type.
        /// </summary>
        /// <param name="Text">A text representation of a power type.</param>
        public static PowerTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var powerType))
                return powerType;

            return default;

        }

        #endregion

        #region TryParse(Text, out PowerType)

        /// <summary>
        /// Parses the given text representation of a power type.
        /// </summary>
        /// <param name="Text">A text representation of a power type.</param>
        /// <param name="PowerType">The parsed power type.</param>
        public static Boolean TryParse(String Text, out PowerTypes PowerType)
        {
            switch (Text?.Trim())
            {

                case "AC_1_PHASE":
                    PowerType = PowerTypes.AC_1_PHASE;
                    return true;

                case "AC_3_PHASE":
                    PowerType = PowerTypes.AC_3_PHASE;
                    return true;

                case "DC":
                    PowerType = PowerTypes.DC;
                    return true;

                case "Unspecified":
                    PowerType = PowerTypes.Unspecified;
                    return true;

                default:
                    PowerType = PowerTypes.Unspecified;
                    return false;

            };
        }

        #endregion

        #region AsString(this PowerType)

        /// <summary>
        /// Return a text representation of the given power type.
        /// </summary>
        /// <param name="PowerType">A power type.</param>
        public static String AsString(this PowerTypes PowerType)

            => PowerType switch {
                   PowerTypes.AC_1_PHASE   => "AC_1_PHASE",
                   PowerTypes.AC_3_PHASE   => "AC_3_PHASE",
                   PowerTypes.DC           => "DC",
                   PowerTypes.Unspecified  => "Unspecified",
                   _                       => "unknown",
               };

        #endregion

    }


    /// <summary>
    /// Power types.
    /// </summary>
    public enum PowerTypes
    {

        /// <summary>
        /// AC 1 phase
        /// </summary>
        AC_1_PHASE,

        /// <summary>
        /// AC 3 phases
        /// </summary>
        AC_3_PHASE,

        /// <summary>
        /// DC
        /// </summary>
        DC,

        /// <summary>
        /// Unspecified power type.
        /// </summary>
        Unspecified

    }

}
