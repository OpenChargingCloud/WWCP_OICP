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
    /// OICP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        #region AsAccessibilityType(AccessibilityType)

        /// <summary>
        /// Maps an OICP accessibility type to a WWCP accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static AccessibilityTypes AsAccessibilityType(String AccessibilityType)
        {

            switch (AccessibilityType.Trim())
            {

                case "Free publicly accessible": return AccessibilityTypes.Free_publicly_accessible;
                case "Restricted access": return AccessibilityTypes.Restricted_access;
                case "Paying publicly accessible": return AccessibilityTypes.Paying_publicly_accessible;

                default: return AccessibilityTypes.Unspecified;

            }

        }

        #endregion

        #region AsString(AccessibilityType)

        public static String AsString(this AccessibilityTypes AccessibilityType)
        {

            switch (AccessibilityType)
            {

                case AccessibilityTypes.Free_publicly_accessible:
                    return "Free publicly accessible";

                case AccessibilityTypes.Restricted_access:
                    return "Restricted access";

                case AccessibilityTypes.Paying_publicly_accessible:
                    return "Paying publicly accessible";


                default:
                    return "Unspecified";

            }

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
