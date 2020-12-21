/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extentions methods for accessibility location types.
    /// </summary>
    public static class AccessibilityLocationTypesExtentions
    {

        #region Parse(AccessibilityLocationType)

        /// <summary>
        /// Parses the given text-representation of an accessibility location type.
        /// </summary>
        /// <param name="Text">A text-representation of an accessibility location type.</param>
        public static AccessibilityLocationTypes Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "OnStreet"                 : return AccessibilityLocationTypes.OnStreet;
                case "ParkingLot"               : return AccessibilityLocationTypes.ParkingLot;
                case "ParkingGarage"            : return AccessibilityLocationTypes.ParkingGarage;
                case "UndergroundParkingGarage" : return AccessibilityLocationTypes.UndergroundParkingGarage;
                default                         : return AccessibilityLocationTypes.Unspecified;
            };
        }

        #endregion

        #region AsString(AccessibilityLocationType)

        /// <summary>
        /// Return a text-representation of the given accessibility location type.
        /// </summary>
        /// <param name="AccessibilityLocationType">An accessibility location type.</param>
        public static String AsString(this AccessibilityLocationTypes AccessibilityLocationType)
        {
            switch (AccessibilityLocationType)
            {
                case AccessibilityLocationTypes.OnStreet                 : return "OnStreet";
                case AccessibilityLocationTypes.ParkingLot               : return "ParkingLot";
                case AccessibilityLocationTypes.ParkingGarage            : return "ParkingGarage";
                case AccessibilityLocationTypes.UndergroundParkingGarage : return "UndergroundParkingGarage";
                default                                                  : return "Unspecified";
            };
        }

        #endregion

    }


    /// <summary>
    /// The accessibility of an EVSE.
    /// </summary>
    public enum AccessibilityLocationTypes
    {

        /// <summary>
        /// Unknown accessibility.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging station is located on the street.
        /// </summary>
        OnStreet,

        /// <summary>
        /// The Charging Point is located inside a Parking Lot.
        /// </summary>
        ParkingLot,

        /// <summary>
        /// The Charging Point is located inside a Parking Garage.
        /// </summary>
        ParkingGarage,

        /// <summary>
        /// The Charging Point is located inside an Underground Parking Garage.
        /// </summary>
        UndergroundParkingGarage

    }

}
