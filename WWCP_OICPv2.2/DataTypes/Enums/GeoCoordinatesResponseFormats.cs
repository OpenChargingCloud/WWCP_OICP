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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Defines the format of geo coordinates that shall be provided with the response.
    /// </summary>
    public enum GeoCoordinatesResponseFormats
    {

        /// <summary>
        /// Google format.
        /// </summary>
        Google,

        /// <summary>
        /// e.g. 42° 23' 5''.
        /// </summary>
        DegreeMinuteSeconds,

        /// <summary>
        /// e.g. 51.23234°.
        /// </summary>
        DecimalDegree

    }

}
