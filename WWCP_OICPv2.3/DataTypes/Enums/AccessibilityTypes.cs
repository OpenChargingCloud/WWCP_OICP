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
        {
            switch (Text?.Trim())
            {
                case "Free publicly accessible"   : return AccessibilityTypes.Free_publicly_accessible;
                case "Restricted access"          : return AccessibilityTypes.Restricted_access;
                case "Paying publicly accessible" : return AccessibilityTypes.Paying_publicly_accessible;
                case "Test Station"               : return AccessibilityTypes.Test_Station;
                default                           : return AccessibilityTypes.Unspecified;
            };
        }

        #endregion

        #region AsString(AccessibilityType)

        /// <summary>
        /// Return a text-representation of the given accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">An accessibility type.</param>
        public static String AsString(this AccessibilityTypes AccessibilityType)
        {
            switch (AccessibilityType)
            {
                case AccessibilityTypes.Free_publicly_accessible   : return "Free publicly accessible";
                case AccessibilityTypes.Restricted_access          : return "Restricted access";
                case AccessibilityTypes.Paying_publicly_accessible : return "Paying publicly accessible";
                case AccessibilityTypes.Test_Station               : return "Test Station";
                default                                            : return "Unspecified";
            };
        }

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
