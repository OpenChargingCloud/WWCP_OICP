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

namespace cloud.charging.open.protocols.OICPv2_3
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

            => Text?.Trim() switch {
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
        /// No status information available.
        /// </summary>
        Unknown,

        /// <summary>
        /// Charging Spot is available for charging.
        /// </summary>
        Available,

        /// <summary>
        /// Charging Spot is reserved and not available for charging.
        /// </summary>
        Reserved,

        /// <summary>
        /// Charging Spot is busy.
        /// </summary>
        Occupied,

        /// <summary>
        /// Charging Spot is out of service and not available for charging.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The requested EvseID and EVSE status does not exist within the Hubject database.
        /// (PullAuthorizationStartById)
        /// </summary>
        EvseNotFound

    }

}
