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

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// OICP charging modes.
    /// </summary>
    [Flags]
    public enum ChargingModes
    {

        /// <summary>
        /// Unknown charging mode.
        /// </summary>
        Unspecified = 0,

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
        CHAdeMO,

    }

}
