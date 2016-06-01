/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// Charging facilities
    /// </summary>
    public enum ChargingFacilities
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified,

        /// <summary>
        /// 100 - 120V, 1-Phase ≤ 10A
        /// </summary>
        CF_100_120V_1Phase_lessOrEquals10A,

        /// <summary>
        /// 100 - 120V, 1-Phase ≤ 16A
        /// </summary>
        CF_100_120V_1Phase_lessOrEquals16A,

        /// <summary>
        /// 100 - 120V, 1-Phase ≤ 32A
        /// </summary>
        CF_100_120V_1Phase_lessOrEquals32A,

        /// <summary>
        /// 200 - 240V, 1-Phase ≤ 10A
        /// </summary>
        CF_200_240V_1Phase_lessOrEquals10A,

        /// <summary>
        /// 200 - 240V, 1-Phase ≤ 16A
        /// </summary>
        CF_200_240V_1Phase_lessOrEquals16A,

        /// <summary>
        /// 200 - 240V, 1-Phase ≤ 32A
        /// </summary>
        CF_200_240V_1Phase_lessOrEquals32A,

        /// <summary>
        /// 200 - 240V, 1-Phase > 32A
        /// </summary>
        CF_200_240V_1Phase_moreThan32A,

        /// <summary>
        /// 380 - 480V, 3-Phase ≤ 16A
        /// </summary>
        CF_380_480V_3Phase_lessOrEquals16A,

        /// <summary>
        /// 380 - 480V, 3-Phase ≤ 32A
        /// </summary>
        CF_380_480V_3Phase_lessOrEquals32A,

        /// <summary>
        /// 380 - 480V, 3-Phase ≤ 63A
        /// </summary>
        CF_380_480V_3Phase_lessOrEquals63A,

        /// <summary>
        /// Battery exchange
        /// </summary>
        Battery_exchange,

        /// <summary>
        /// DC Charging ≤ 20kW
        /// </summary>
        DCCharging_lessOrEquals20kW,

        /// <summary>
        /// DC Charging ≤ 50kW
        /// </summary>
        DCCharging_lessOrEquals50kW,

        /// <summary>
        /// DC Charging > 50kW
        /// </summary>
        DCCharging_moreThan50kW

    }

}
