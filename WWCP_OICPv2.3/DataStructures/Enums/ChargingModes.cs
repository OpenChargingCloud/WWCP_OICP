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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extensions methods for charging modes.
    /// </summary>
    public static class ChargingModesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a charging mode.
        /// </summary>
        /// <param name="Text">A text representation of a charging mode.</param>
        public static ChargingModes Parse(String Text)
        {

            if (TryParse(Text, out var chargingMode))
                return chargingMode;

            throw new ArgumentException("Undefined charging mode '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a charging mode.
        /// </summary>
        /// <param name="Text">A text representation of a charging mode.</param>
        public static ChargingModes? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingMode))
                return chargingMode;

            return default;

        }

        #endregion

        #region TryParse(Text, out ChargingMode)

        /// <summary>
        /// Parses the given text representation of a charging mode.
        /// </summary>
        /// <param name="Text">A text representation of a charging mode.</param>
        /// <param name="ChargingMode">The parsed charging mode.</param>
        public static Boolean TryParse(String Text, out ChargingModes ChargingMode)
        {
            switch (Text?.Trim())
            {

                case "Mode_1":
                    ChargingMode = ChargingModes.Mode_1;
                    return true;

                case "Mode_2":
                    ChargingMode = ChargingModes.Mode_2;
                    return true;

                case "Mode_3":
                    ChargingMode = ChargingModes.Mode_3;
                    return true;

                case "Mode_4":
                    ChargingMode = ChargingModes.Mode_4;
                    return true;

                case "CHAdeMO":
                    ChargingMode = ChargingModes.CHAdeMO;
                    return true;

                default:
                    ChargingMode = ChargingModes.Mode_1;
                    return false;

            }
        }

        #endregion

        #region AsString(ChargingMode)

        /// <summary>
        /// Return a text representation of the given charging mode.
        /// </summary>
        /// <param name="ChargingMode">A charging mode.</param>
        public static String AsString(this ChargingModes ChargingMode)

            => ChargingMode switch {
                   ChargingModes.Mode_1   => "Mode_1",
                   ChargingModes.Mode_2   => "Mode_2",
                   ChargingModes.Mode_3   => "Mode_3",
                   ChargingModes.Mode_4   => "Mode_4",
                   ChargingModes.CHAdeMO  => "CHAdeMO",
                   _                      => "Unspecified",
               };

        #endregion

    }


    /// <summary>
    /// Charging modes.
    /// </summary>
    public enum ChargingModes
    {

        /// <summary>
        /// IEC 61851-1 Mode 1
        /// </summary>
        Mode_1,

        /// <summary>
        /// IEC 61851-1 Mode 2
        /// </summary>
        Mode_2,

        /// <summary>
        /// IEC 61851-1 Mode 3
        /// </summary>
        Mode_3,

        /// <summary>
        /// IEC 61851-1 Mode 4
        /// </summary>
        Mode_4,

        /// <summary>
        /// CHAdeMO
        /// </summary>
        CHAdeMO

    }

}
