/*
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The format of geographical coordinates.
    /// </summary>
    public enum GeoCoordinatesFormats
    {

        /// <summary>
        /// Google geo format, e.g. "47.662249 9.360922".
        /// </summary>
        Google,

        /// <summary>
        /// DecimalDegree geo format, e.g. "9.360922" "-21.568201".
        /// </summary>
        DecimalDegree,

        /// <summary>
        /// DegreeMinuteSeconds geo format, e.g. "9° 21' 39.32''", "-21° 34' 23.16".
        /// </summary>
        DegreeMinuteSeconds

    }

}
