/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// The current status of an OICP v2.0 Electric Vehicle Supply Equipment.
    /// </summary>
    public enum EVSEStatusType
    {

        /// <summary>
        /// The status or EVSE is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The EVSE is available.
        /// </summary>
        Available,

        /// <summary>
        /// The EVSE is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The EVSE is occupied.
        /// </summary>
        Occupied,

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE was not found.
        /// (PullEVSEStatusById)
        /// </summary>
        EvseNotFound

    }

}
