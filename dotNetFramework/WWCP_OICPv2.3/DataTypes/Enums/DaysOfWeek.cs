/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
    /// Extensions methods for day(s) of week.
    /// </summary>
    public static class DaysOfWeekExtensions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a day(s) of week.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of a day(s) of week.</param>
        public static DaysOfWeek Parse(String Text)
        {

            if (TryParse(Text, out DaysOfWeek daysOfWeek))
                return daysOfWeek;

            throw new ArgumentException("Undefined day(s) of week '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text-representation of a day(s) of week..
        /// </summary>
        /// <param name="Text">A text-representation of a day(s) of week..</param>
        public static DaysOfWeek? TryParse(String Text)
        {

            if (TryParse(Text, out DaysOfWeek daysOfWeek))
                return daysOfWeek;

            return default;

        }

        #endregion

        #region TryParse(Text, out DayOfWeek)

        /// <summary>
        /// Parses the given text-representation of a day(s) of week..
        /// </summary>
        /// <param name="Text">A text-representation of a day(s) of week..</param>
        /// <param name="DayOfWeek">The parsed day(s) of week..</param>
        public static Boolean TryParse(String Text, out DaysOfWeek DayOfWeek)
        {
            switch (Text?.Trim())
            {

                case "Everyday":
                    DayOfWeek = DaysOfWeek.Everyday;
                    return true;

                case "Workdays":
                    DayOfWeek = DaysOfWeek.Workdays;
                    return true;

                case "Weekend":
                    DayOfWeek = DaysOfWeek.Weekend;
                    return true;

                case "Monday":
                    DayOfWeek = DaysOfWeek.Monday;
                    return true;

                case "Tuesday":
                    DayOfWeek = DaysOfWeek.Tuesday;
                    return true;

                case "Wednesday":
                    DayOfWeek = DaysOfWeek.Wednesday;
                    return true;

                case "Thursday":
                    DayOfWeek = DaysOfWeek.Thursday;
                    return true;

                case "Friday":
                    DayOfWeek = DaysOfWeek.Friday;
                    return true;

                case "Saturday":
                    DayOfWeek = DaysOfWeek.Saturday;
                    return true;

                case "Sunday":
                    DayOfWeek = DaysOfWeek.Sunday;
                    return true;

                default:
                    DayOfWeek = DaysOfWeek.Everyday;
                    return false;

            }
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

            return EVSEStatusType switch {
                       DaysOfWeek.Monday     => "Monday",
                       DaysOfWeek.Tuesday    => "Tuesday",
                       DaysOfWeek.Wednesday  => "Wednesday",
                       DaysOfWeek.Thursday   => "Thursday",
                       DaysOfWeek.Friday     => "Friday",
                       DaysOfWeek.Saturday   => "Saturday",
                       DaysOfWeek.Sunday     => "Sunday",
                       _                     => "Unknown",
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
