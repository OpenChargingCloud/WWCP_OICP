/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extensions methods for EVSE status types.
    /// </summary>
    public static class EVSEStatusTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE status type.</param>
        public static EVSEStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out var evseStatusType))
                return evseStatusType;

            throw new ArgumentException("Undefined EVSE status type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE status type.</param>
        public static EVSEStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var evseStatusType))
                return evseStatusType;

            return default;

        }

        #endregion

        #region TryParse(Text, out EVSEStatusType)

        /// <summary>
        /// Parses the given text representation of an EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE status type.</param>
        /// <param name="EVSEStatusType">The parsed EVSE status type.</param>
        public static Boolean TryParse(String Text, out EVSEStatusTypes EVSEStatusType)
        {
            switch (Text?.Trim())
            {

                case "Available":
                    EVSEStatusType = EVSEStatusTypes.Available;
                    return true;

                case "Reserved":
                    EVSEStatusType = EVSEStatusTypes.Reserved;
                    return true;

                case "Occupied":
                    EVSEStatusType = EVSEStatusTypes.Occupied;
                    return true;

                case "OutOfService":
                    EVSEStatusType = EVSEStatusTypes.OutOfService;
                    return true;

                case "Unknown":
                    EVSEStatusType = EVSEStatusTypes.Unknown;
                    return true;

                case "EvseNotFound":
                    EVSEStatusType = EVSEStatusTypes.EVSENotFound;
                    return true;

                default:
                    EVSEStatusType = EVSEStatusTypes.Unknown;
                    return false;

            }
        }

        #endregion

        #region AsString(EVSEStatusType)

        /// <summary>
        /// Return a text representation of the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status.</param>
        public static String AsString(this EVSEStatusTypes EVSEStatusType)

            => EVSEStatusType switch {
                   EVSEStatusTypes.Available     => "Available",
                   EVSEStatusTypes.Reserved      => "Reserved",
                   EVSEStatusTypes.Occupied      => "Occupied",
                   EVSEStatusTypes.OutOfService  => "OutOfService",
                   EVSEStatusTypes.EVSENotFound  => "EvseNotFound",
                   _                             => "Unknown"
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
        EVSENotFound

    }

}
