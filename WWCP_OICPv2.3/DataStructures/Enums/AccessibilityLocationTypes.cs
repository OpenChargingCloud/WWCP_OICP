/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extensions methods for accessibility location types.
    /// </summary>
    public static class AccessibilityLocationTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an accessibility location type.
        /// </summary>
        /// <param name="Text">A text representation of an accessibility location type.</param>
        public static AccessibilityLocationTypes Parse(String Text)
        {

            if (TryParse(Text, out var accessibilityLocationType))
                return accessibilityLocationType;

            throw new ArgumentException("Undefined accessibility location type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an accessibility location type.
        /// </summary>
        /// <param name="Text">A text representation of an accessibility location type.</param>
        public static AccessibilityLocationTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var accessibilityLocationType))
                return accessibilityLocationType;

            return default;

        }

        #endregion

        #region TryParse(Text, out AccessibilityLocationType)

        /// <summary>
        /// Parses the given text representation of an accessibility location type.
        /// </summary>
        /// <param name="Text">A text representation of an accessibility location type.</param>
        /// <param name="AccessibilityLocationType">The parsed accessibility location type.</param>
        public static Boolean TryParse(String Text, out AccessibilityLocationTypes AccessibilityLocationType)
        {
            switch (Text?.Trim())
            {

                case "OnStreet":
                    AccessibilityLocationType = AccessibilityLocationTypes.OnStreet;
                    return true;

                case "ParkingLot":
                    AccessibilityLocationType = AccessibilityLocationTypes.ParkingLot;
                    return true;

                case "ParkingGarage":
                    AccessibilityLocationType = AccessibilityLocationTypes.ParkingGarage;
                    return true;

                case "UndergroundParkingGarage":
                    AccessibilityLocationType = AccessibilityLocationTypes.UndergroundParkingGarage;
                    return true;

                default:
                    AccessibilityLocationType = AccessibilityLocationTypes.OnStreet;
                    return false;

            }
        }

        #endregion

        #region AsString(AccessibilityLocationType)

        /// <summary>
        /// Return a text representation of the given accessibility location type.
        /// </summary>
        /// <param name="AccessibilityLocationType">An accessibility location type.</param>
        public static String AsString(this AccessibilityLocationTypes AccessibilityLocationType)

            => AccessibilityLocationType switch {
                   AccessibilityLocationTypes.OnStreet                  => "OnStreet",
                   AccessibilityLocationTypes.ParkingLot                => "ParkingLot",
                   AccessibilityLocationTypes.ParkingGarage             => "ParkingGarage",
                   AccessibilityLocationTypes.UndergroundParkingGarage  => "UndergroundParkingGarage",
                   _                                                    => "OnStreet"
               };

        #endregion

    }


    /// <summary>
    /// The accessibility of an EVSE.
    /// </summary>
    public enum AccessibilityLocationTypes
    {

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
