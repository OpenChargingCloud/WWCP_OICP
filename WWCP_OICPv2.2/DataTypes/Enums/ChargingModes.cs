/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// OICP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        #region AsChargingMode(ChargingMode)

        /// <summary>
        /// Maps an OICP charging mode to a WWCP charging mode.
        /// </summary>
        /// <param name="ChargingMode">A charging mode.</param>
        public static ChargingModes AsChargingMode(String ChargingMode)
        {

            switch (ChargingMode.Trim())
            {

                case "Mode_1":   return ChargingModes.Mode_1;
                case "Mode_2":   return ChargingModes.Mode_2;
                case "Mode_3":   return ChargingModes.Mode_3;
                case "Mode_4":   return ChargingModes.Mode_4;
                case "CHAdeMO":  return ChargingModes.CHAdeMO;

                default: return ChargingModes.Unspecified;

            }

        }

        #endregion

        #region AsString(ChargingMode)

        public static String AsString(this ChargingModes ChargingMode)
        {

            switch (ChargingMode)
            {

                case ChargingModes.Mode_1:
                    return "Mode_1";

                case ChargingModes.Mode_2:
                    return "Mode_2";

                case ChargingModes.Mode_3:
                    return "Mode_3";

                case ChargingModes.Mode_4:
                    return "Mode_4";

                case ChargingModes.CHAdeMO:
                    return "CHAdeMO";


                default:
                    return "Unspecified";

            }

        }

        #endregion

    }


    /// <summary>
    /// OICP charging modes.
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
