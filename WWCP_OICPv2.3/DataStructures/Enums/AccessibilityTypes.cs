/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extensions methods for accessibility types.
    /// </summary>
    public static class AccessibilityTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an accessibility type.
        /// </summary>
        /// <param name="Text">A text representation of an accessibility type.</param>
        public static AccessibilityTypes Parse(String Text)
        {

            if (TryParse(Text, out var accessibilityType))
                return accessibilityType;

            throw new ArgumentException("Undefined accessibility type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an accessibility type.
        /// </summary>
        /// <param name="Text">A text representation of an accessibility type.</param>
        public static AccessibilityTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var accessibilityType))
                return accessibilityType;

            return default;

        }

        #endregion

        #region TryParse(Text, out AccessibilityType)

        /// <summary>
        /// Parses the given text representation of an accessibility type.
        /// </summary>
        /// <param name="Text">A text representation of an accessibility type.</param>
        /// <param name="AccessibilityType">The parsed accessibility type.</param>
        public static Boolean TryParse(String Text, out AccessibilityTypes AccessibilityType)
        {
            switch (Text?.Trim())
            {

                case "Unspecified":
                    AccessibilityType = AccessibilityTypes.Unspecified;
                    return true;

                case "Free publicly accessible":
                    AccessibilityType = AccessibilityTypes.FreePubliclyAccessible;
                    return true;

                case "Restricted access":
                    AccessibilityType = AccessibilityTypes.RestrictedAccess;
                    return true;

                case "Paying publicly accessible":
                    AccessibilityType = AccessibilityTypes.PayingPubliclyAccessible;
                    return true;

                case "Test Station":
                    AccessibilityType = AccessibilityTypes.TestStation;
                    return true;

                default:
                    AccessibilityType = AccessibilityTypes.Unspecified;
                    return false;

            }
        }

        #endregion

        #region AsString(AccessibilityType)

        /// <summary>
        /// Return a text representation of the given accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">An accessibility type.</param>
        public static String AsString(this AccessibilityTypes AccessibilityType)

            => AccessibilityType switch {
                   AccessibilityTypes.Unspecified               => "Unspecified",
                   AccessibilityTypes.FreePubliclyAccessible    => "Free publicly accessible",
                   AccessibilityTypes.RestrictedAccess          => "Restricted access",
                   AccessibilityTypes.PayingPubliclyAccessible  => "Paying publicly accessible",
                   AccessibilityTypes.TestStation               => "Test Station",
                   _                                            => "Unknown"
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
        Unspecified,

        /// <summary>
        /// Free for everyone.
        /// </summary>
        FreePubliclyAccessible,

        /// <summary>
        /// Limited access, e.g. only for customers or employees.
        /// </summary>
        RestrictedAccess,

        /// <summary>
        /// Free for everyone who pays.
        /// </summary>
        PayingPubliclyAccessible,

        /// <summary>
        /// This station is only usable for testing.
        /// </summary>
        TestStation

    }

}
