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

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Extentions methods for EVSE status types.
    /// </summary>
    public static class EVSEStatusTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of an EVSE status.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of an EVSE status.</param>
        public static EVSEStatusTypes Parse(String Text)

            => Text.Trim() switch {
                "Available"     => EVSEStatusTypes.Available,
                "Reserved"      => EVSEStatusTypes.Reserved,
                "Occupied"      => EVSEStatusTypes.Occupied,
                "OutOfService"  => EVSEStatusTypes.OutOfService,
                "EvseNotFound"  => EVSEStatusTypes.EvseNotFound,
                _               => EVSEStatusTypes.Unknown,
            };

        #endregion

        #region AsString(EVSEStatusType)

        /// <summary>
        /// Return a text-representation of the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status.</param>
        public static String AsString(this EVSEStatusTypes EVSEStatusType)

            => EVSEStatusType switch {
                EVSEStatusTypes.Available     => "Available",
                EVSEStatusTypes.Reserved      => "Reserved",
                EVSEStatusTypes.Occupied      => "Occupied",
                EVSEStatusTypes.OutOfService  => "OutOfService",
                EVSEStatusTypes.EvseNotFound  => "EvseNotFound",
                _                             => "Unknown",
            };

        #endregion

    }


    /// <summary>
    /// The current dynamic status of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public enum EVSEStatusTypes
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
        /// (PullAuthorizationStartById)
        /// </summary>
        EvseNotFound

    }

}
