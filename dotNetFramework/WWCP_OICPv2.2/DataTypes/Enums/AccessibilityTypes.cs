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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Extentions methods for accessibility types.
    /// </summary>
    public static class AccessibilityTypesExtentions
    {

        #region Parse(AccessibilityType)

        /// <summary>
        /// Parses the given text-representation of an accessibility type.
        /// </summary>
        /// <param name="Text">A text-representation of an accessibility type.</param>
        public static AccessibilityTypes Parse(String Text)

            => Text.Trim() switch {
                "Free publicly accessible"    => AccessibilityTypes.Free_publicly_accessible,
                "Restricted access"           => AccessibilityTypes.Restricted_access,
                "Paying publicly accessible"  => AccessibilityTypes.Paying_publicly_accessible,
                "Test Station"                => AccessibilityTypes.Test_Station,
                _                             => AccessibilityTypes.Unspecified,
            };

        #endregion

        #region AsString(AccessibilityType)

        /// <summary>
        /// Return a text-representation of the given accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">An accessibility type.</param>
        public static String AsString(this AccessibilityTypes AccessibilityType)

            => AccessibilityType switch {
                AccessibilityTypes.Free_publicly_accessible    => "Free publicly accessible",
                AccessibilityTypes.Restricted_access           => "Restricted access",
                AccessibilityTypes.Paying_publicly_accessible  => "Paying publicly accessible",
                AccessibilityTypes.Test_Station                => "Test Station",
                _ => "Unspecified",
            };

        #endregion

    }


    /// <summary>
    /// The accessibility of an EVSE.
    /// </summary>
    public enum AccessibilityTypes
    {

        /// <summary>
        /// Unknown accessibility.
        /// </summary>
        Unspecified                 = 0,

        /// <summary>
        /// Free for everyone.
        /// </summary>
        Free_publicly_accessible    = 1,

        /// <summary>
        /// Limited access, e.g. only for customers or employees.
        /// </summary>
        Restricted_access           = 2,

        /// <summary>
        /// Free for everyone who pays.
        /// </summary>
        Paying_publicly_accessible  = 4,

        /// <summary>
        /// This station is only usable for testing.
        /// </summary>
        Test_Station                = 8

    }

}
