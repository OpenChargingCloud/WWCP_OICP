﻿/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using System;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{


    /// <summary>
    /// Extentions methods for charging modes.
    /// </summary>
    public static class ChargingModesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a charging mode.
        /// </summary>
        /// <param name="Text">A text-representation of a charging mode.</param>
        public static ChargingModes Parse(String Text)

            => Text.Trim() switch {
                "Mode_1"   => ChargingModes.Mode_1,
                "Mode_2"   => ChargingModes.Mode_2,
                "Mode_3"   => ChargingModes.Mode_3,
                "Mode_4"   => ChargingModes.Mode_4,
                "CHAdeMO"  => ChargingModes.CHAdeMO,
                _          => ChargingModes.Unspecified,
            };

        #endregion

        #region AsString(ChargingMode)

        /// <summary>
        /// Return a text-representation of the given charging mode.
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
    [Flags]
    public enum ChargingModes
    {

        /// <summary>
        /// Unknown charging mode.
        /// </summary>
        Unspecified     = 0,

        /// <summary>
        /// IEC 61851-1 Mode 1
        /// </summary>
        Mode_1          = 1,

        /// <summary>
        /// IEC 61851-1 Mode 2
        /// </summary>
        Mode_2          = 2,

        /// <summary>
        /// IEC 61851-1 Mode 3
        /// </summary>
        Mode_3          = 4,

        /// <summary>
        /// IEC 61851-1 Mode 4
        /// </summary>
        Mode_4          = 8,

        /// <summary>
        /// CHAdeMO
        /// </summary>
        CHAdeMO         = 16

    }

}
