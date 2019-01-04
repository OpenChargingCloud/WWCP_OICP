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
    /// OICP charging facilities.
    /// </summary>
    [Flags]
    public enum ChargingFacilities
    {

        /// <summary>
        /// Unspecified charging facilities.
        /// </summary>
        Unspecified                             = 0,

        /// <summary>
        /// 100 - 120V, 1-Phase ≤ 10A
        /// </summary>
        CF_100_120V_1Phase_lessOrEquals10A      = 1,

        /// <summary>
        /// 100 - 120V, 1-Phase ≤ 16A
        /// </summary>
        CF_100_120V_1Phase_lessOrEquals16A      = 1 << 1,

        /// <summary>
        /// 100 - 120V, 1-Phase ≤ 32A
        /// </summary>
        CF_100_120V_1Phase_lessOrEquals32A      = 1 << 2,

        /// <summary>
        /// 200 - 240V, 1-Phase ≤ 10A
        /// </summary>
        CF_200_240V_1Phase_lessOrEquals10A      = 1 << 3,

        /// <summary>
        /// 200 - 240V, 1-Phase ≤ 16A
        /// </summary>
        CF_200_240V_1Phase_lessOrEquals16A      = 1 << 4,

        /// <summary>
        /// 200 - 240V, 1-Phase ≤ 32A
        /// </summary>
        CF_200_240V_1Phase_lessOrEquals32A      = 1 << 5,

        /// <summary>
        /// 200 - 240V, 1-Phase > 32A
        /// </summary>
        CF_200_240V_1Phase_moreThan32A          = 1 << 6,

        /// <summary>
        /// 380 - 480V, 3-Phase ≤ 16A
        /// </summary>
        CF_380_480V_3Phase_lessOrEquals16A      = 1 << 7,

        /// <summary>
        /// 380 - 480V, 3-Phase ≤ 32A
        /// </summary>
        CF_380_480V_3Phase_lessOrEquals32A      = 1 << 8,

        /// <summary>
        /// 380 - 480V, 3-Phase ≤ 63A
        /// </summary>
        CF_380_480V_3Phase_lessOrEquals63A      = 1 << 9,

        /// <summary>
        /// Battery exchange
        /// </summary>
        Battery_exchange                        = 1 << 10,

        /// <summary>
        /// DC Charging ≤ 20kW
        /// </summary>
        DCCharging_lessOrEquals20kW             = 1 << 11,

        /// <summary>
        /// DC Charging ≤ 50kW
        /// </summary>
        DCCharging_lessOrEquals50kW             = 1 << 12,

        /// <summary>
        /// DC Charging > 50kW
        /// </summary>
        DCCharging_moreThan50kW                 = 1 << 13

    }

}
