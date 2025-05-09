﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for week days.
    /// </summary>
    public static class WeekDayExtensions
    {

        /// <summary>
        /// Indicates whether this week day is null or empty.
        /// </summary>
        /// <param name="WeekDay">A week day.</param>
        public static Boolean IsNullOrEmpty(this WeekDay? WeekDay)
            => !WeekDay.HasValue || WeekDay.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this week day is null or empty.
        /// </summary>
        /// <param name="WeekDay">A week day.</param>
        public static Boolean IsNotNullOrEmpty(this WeekDay? WeekDay)
            => WeekDay.HasValue && WeekDay.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging pool.
    /// </summary>
    public readonly struct WeekDay : IId<WeekDay>
    {

        #region Data

        //ToDo: Implement proper charging pool id format!
        // ([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?P[A-Za-z0-9\*]{1,30})

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the week day.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new week day.
        /// based on the given string.
        /// </summary>
        private WeekDay(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a week day.
        /// </summary>
        /// <param name="Text">A text representation of a week day.</param>
        public static WeekDay Parse(String Text)
        {

            if (TryParse(Text, out var weekDay))
                return weekDay;

            throw new ArgumentException($"Invalid text representation of a week day: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a week day.
        /// </summary>
        /// <param name="Text">A text representation of a week day.</param>
        public static WeekDay? TryParse(String Text)
        {

            if (TryParse(Text, out var weekDay))
                return weekDay;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out WeekDay)

        /// <summary>
        /// Try to parse the given string as a week day.
        /// </summary>
        /// <param name="Text">A text representation of a week day.</param>
        /// <param name="WeekDay">The parsed week day.</param>
        public static Boolean TryParse(String Text, out WeekDay WeekDay)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    WeekDay = new WeekDay(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            WeekDay = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this week day.
        /// </summary>
        public WeekDay Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static Definitions

        /// <summary>
        /// Everyday
        /// </summary>
        public static WeekDay Everyday
            => new ("Everyday");

        /// <summary>
        /// Workdays
        /// </summary>
        public static WeekDay Workdays
            => new ("Workdays");

        /// <summary>
        /// Weekend
        /// </summary>
        public static WeekDay Weekend
            => new ("Weekend");

        /// <summary>
        /// Monday
        /// </summary>
        public static WeekDay Monday
            => new ("Monday");

        /// <summary>
        /// Tuesday
        /// </summary>
        public static WeekDay Tuesday
            => new ("Tuesday");

        /// <summary>
        /// Wednesday
        /// </summary>
        public static WeekDay Wednesday
            => new ("Wednesday");

        /// <summary>
        /// Thursday
        /// </summary>
        public static WeekDay Thursday
            => new ("Thursday");

        /// <summary>
        /// Friday
        /// </summary>
        public static WeekDay Friday
            => new ("Friday");

        /// <summary>
        /// Saturday
        /// </summary>
        public static WeekDay Saturday
            => new ("Saturday");

        /// <summary>
        /// Sunday
        /// </summary>
        public static WeekDay Sunday
            => new ("Sunday");

        #endregion


        #region Operator overloading

        #region Operator == (WeekDay1, WeekDay2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WeekDay1">A week day.</param>
        /// <param name="WeekDay2">Another week day.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WeekDay WeekDay1,
                                           WeekDay WeekDay2)

            => WeekDay1.Equals(WeekDay2);

        #endregion

        #region Operator != (WeekDay1, WeekDay2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WeekDay1">A week day.</param>
        /// <param name="WeekDay2">Another week day.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WeekDay WeekDay1,
                                           WeekDay WeekDay2)

            => !WeekDay1.Equals(WeekDay2);

        #endregion

        #region Operator <  (WeekDay1, WeekDay2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WeekDay1">A week day.</param>
        /// <param name="WeekDay2">Another week day.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (WeekDay WeekDay1,
                                          WeekDay WeekDay2)

            => WeekDay1.CompareTo(WeekDay2) < 0;

        #endregion

        #region Operator <= (WeekDay1, WeekDay2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WeekDay1">A week day.</param>
        /// <param name="WeekDay2">Another week day.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (WeekDay WeekDay1,
                                           WeekDay WeekDay2)

            => WeekDay1.CompareTo(WeekDay2) <= 0;

        #endregion

        #region Operator >  (WeekDay1, WeekDay2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WeekDay1">A week day.</param>
        /// <param name="WeekDay2">Another week day.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (WeekDay WeekDay1,
                                          WeekDay WeekDay2)

            => WeekDay1.CompareTo(WeekDay2) > 0;

        #endregion

        #region Operator >= (WeekDay1, WeekDay2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WeekDay1">A week day.</param>
        /// <param name="WeekDay2">Another week day.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (WeekDay WeekDay1,
                                           WeekDay WeekDay2)

            => WeekDay1.CompareTo(WeekDay2) >= 0;

        #endregion

        #endregion

        #region IComparable<WeekDay> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two week days.
        /// </summary>
        /// <param name="Object">A week day to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is WeekDay weekDay
                   ? CompareTo(weekDay)
                   : throw new ArgumentException("The given object is not a week day!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(WeekDay)

        /// <summary>
        /// Compares two week days.
        /// </summary>
        /// <param name="WeekDay">A week day to compare with.</param>
        public Int32 CompareTo(WeekDay WeekDay)

            => String.Compare(InternalId,
                              WeekDay.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<WeekDay> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two week days for equality.
        /// </summary>
        /// <param name="Object">A week day to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is WeekDay weekDay &&
                   Equals(weekDay);

        #endregion

        #region Equals(WeekDay)

        /// <summary>
        /// Compares two week days for equality.
        /// </summary>
        /// <param name="WeekDay">A week day to compare with.</param>
        public Boolean Equals(WeekDay WeekDay)

            => String.Equals(InternalId,
                             WeekDay.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
