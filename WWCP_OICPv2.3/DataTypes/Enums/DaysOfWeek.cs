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
    public static class DaysOfWeekExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of an EVSE status.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of an EVSE status.</param>
        public static DaysOfWeek Parse(String Text)
        {
            switch (Text?.Trim()) {

                case "Everyday":   return DaysOfWeek.Everyday;
                case "Workdays":   return DaysOfWeek.Workdays;
                case "Weekend":    return DaysOfWeek.Weekend;

                case "Monday":     return DaysOfWeek.Monday;
                case "Tuesday":    return DaysOfWeek.Tuesday;
                case "Wednesday":  return DaysOfWeek.Wednesday;
                case "Thursday":   return DaysOfWeek.Thursday;
                case "Friday":     return DaysOfWeek.Friday;
                case "Saturday":   return DaysOfWeek.Saturday;
                case "Sunday":     return DaysOfWeek.Sunday;

                default:           return DaysOfWeek.Unknown;

            };
        }

        #endregion

        #region AsString(EVSEStatusType)

        /// <summary>
        /// Return a text-representation of the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status.</param>
        public static String AsString(this DaysOfWeek EVSEStatusType)
        {

            if (EVSEStatusType.HasFlag(DaysOfWeek.Everyday))
                return "Everyday";

            if (EVSEStatusType.HasFlag(DaysOfWeek.Workdays))
                return "Workdays";

            if (EVSEStatusType.HasFlag(DaysOfWeek.Weekend))
                return "Weekend";

            switch (EVSEStatusType) {

                case DaysOfWeek.Monday:     return "Monday";
                case DaysOfWeek.Tuesday:    return "Tuesday";
                case DaysOfWeek.Wednesday:  return "Wednesday";
                case DaysOfWeek.Thursday:   return "Thursday";
                case DaysOfWeek.Friday:     return "Friday";
                case DaysOfWeek.Saturday:   return "Saturday";
                case DaysOfWeek.Sunday:     return "Sunday";

                default:                    return "Unknown";

            };
        }

        #endregion

    }

    /// <summary>
    /// The current dynamic status of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    [Flags]
    public enum DaysOfWeek
    {

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown     =  0,

        /// <summary>
        /// Everyday
        /// </summary>
        Everyday    = Workdays & Weekend,

        /// <summary>
        /// Workdays
        /// </summary>
        Workdays    = Monday & Tuesday & Wednesday & Thursday & Friday,

        /// <summary>
        /// Weekend
        /// </summary>
        Weekend     = Saturday & Sunday,

        /// <summary>
        /// Monday
        /// </summary>
        Monday      =  1,

        /// <summary>
        /// Tuesday
        /// </summary>
        Tuesday     =  2,

        /// <summary>
        /// Wednesday
        /// </summary>
        Wednesday   =  4,

        /// <summary>
        /// Thursday
        /// </summary>
        Thursday    =  8,

        /// <summary>
        /// Friday
        /// </summary>
        Friday      = 16,

        /// <summary>
        /// Saturday
        /// </summary>
        Saturday    = 32,

        /// <summary>
        /// Sunday
        /// </summary>
        Sunday      = 64

    }

}
