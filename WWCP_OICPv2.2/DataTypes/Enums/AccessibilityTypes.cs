/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The accessibility of an EVSE.
    /// </summary>
    public enum AccessibilityTypes
    {

        /// <summary>
        /// Unknown accessibility.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Free for everyone.
        /// </summary>
        Free_publicly_accessible,

        /// <summary>
        /// Limited access, e.g. only for customers or employees.
        /// </summary>
        Restricted_access,

        /// <summary>
        /// Free for everyone who pays.
        /// </summary>
        Paying_publicly_accessible,

        /// <summary>
        /// This station is only usable for testing.
        /// </summary>
        Test_Station

    }

}
