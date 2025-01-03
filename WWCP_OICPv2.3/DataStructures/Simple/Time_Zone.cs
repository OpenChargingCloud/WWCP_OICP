/*
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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for time zones.
    /// </summary>
    public static class TimeZoneExtensions
    {

        /// <summary>
        /// Indicates whether this time zone is null or empty.
        /// </summary>
        /// <param name="TimeZone">A time zone.</param>
        public static Boolean IsNullOrEmpty(this Time_Zone? TimeZone)
            => !TimeZone.HasValue || TimeZone.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this time zone is null or empty.
        /// </summary>
        /// <param name="TimeZone">A time zone.</param>
        public static Boolean IsNotNullOrEmpty(this Time_Zone? TimeZone)
            => TimeZone.HasValue && TimeZone.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a time zone.
    /// </summary>
    public readonly struct Time_Zone : IId<Time_Zone>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a time zone.
        /// Official regular expression: ^[U][T][C][+,-][0-9][0-9][:][0-9][0-9]$
        /// </summary>
        /// <remarks>https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#TimeZoneType</remarks>
        public static readonly Regex TimeZone_RegEx = new (@"^[U][T][C][+,-][0-9][0-9][:][0-9][0-9]$",
                                                           RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this time zone is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this time zone is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the time zone identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new time zone identification.
        /// based on the given string.
        /// </summary>
        private Time_Zone(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a time zone identification.
        /// </summary>
        /// <param name="Text">A text representation of a time zone identification.</param>
        public static Time_Zone Parse(String Text)
        {

            if (TryParse(Text, out var timeZone))
                return timeZone;

            throw new ArgumentException($"Invalid text representation of a time zone identification: '{Text}'!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a time zone identification.
        /// </summary>
        /// <param name="Text">A text representation of a time zone identification.</param>
        public static Time_Zone? TryParse(String Text)
        {

            if (TryParse(Text, out var timeZone))
                return timeZone;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out TimeZone)

        /// <summary>
        /// Try to parse the given string as a time zone identification.
        /// </summary>
        /// <param name="Text">A text representation of a time zone identification.</param>
        /// <param name="TimeZone">The parsed time zone identification.</param>
        public static Boolean TryParse(String Text, out Time_Zone TimeZone)
        {

            if (!Text.IsNullOrEmpty())
            {

                Text = Text.Trim();

                if (TimeZone_RegEx.IsMatch(Text))
                {
                    try
                    {
                        TimeZone = new Time_Zone(Text);
                        return true;
                    }
                    catch
                    { }
                }

            }

            TimeZone = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this time zone identification.
        /// </summary>
        public Time_Zone Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (TimeZone1, TimeZone2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TimeZone1">A time zone identification.</param>
        /// <param name="TimeZone2">Another time zone identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Time_Zone TimeZone1,
                                           Time_Zone TimeZone2)

            => TimeZone1.Equals(TimeZone2);

        #endregion

        #region Operator != (TimeZone1, TimeZone2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TimeZone1">A time zone identification.</param>
        /// <param name="TimeZone2">Another time zone identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Time_Zone TimeZone1,
                                           Time_Zone TimeZone2)

            => !TimeZone1.Equals(TimeZone2);

        #endregion

        #region Operator <  (TimeZone1, TimeZone2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TimeZone1">A time zone identification.</param>
        /// <param name="TimeZone2">Another time zone identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Time_Zone TimeZone1,
                                          Time_Zone TimeZone2)

            => TimeZone1.CompareTo(TimeZone2) < 0;

        #endregion

        #region Operator <= (TimeZone1, TimeZone2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TimeZone1">A time zone identification.</param>
        /// <param name="TimeZone2">Another time zone identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Time_Zone TimeZone1,
                                           Time_Zone TimeZone2)

            => TimeZone1.CompareTo(TimeZone2) <= 0;

        #endregion

        #region Operator >  (TimeZone1, TimeZone2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TimeZone1">A time zone identification.</param>
        /// <param name="TimeZone2">Another time zone identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Time_Zone TimeZone1,
                                          Time_Zone TimeZone2)

            => TimeZone1.CompareTo(TimeZone2) > 0;

        #endregion

        #region Operator >= (TimeZone1, TimeZone2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TimeZone1">A time zone identification.</param>
        /// <param name="TimeZone2">Another time zone identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Time_Zone TimeZone1,
                                           Time_Zone TimeZone2)

            => TimeZone1.CompareTo(TimeZone2) >= 0;

        #endregion

        #endregion

        #region IComparable<TimeZone> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two time zones.
        /// </summary>
        /// <param name="Object">A time zone to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Time_Zone timeZone
                   ? CompareTo(timeZone)
                   : throw new ArgumentException("The given object is not a time zone identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(TimeZone)

        /// <summary>
        /// Compares two time zones.
        /// </summary>
        /// <param name="TimeZone">A time zone to compare with.</param>
        public Int32 CompareTo(Time_Zone TimeZone)

            => String.Compare(InternalId,
                              TimeZone.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<TimeZone> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two time zones for equality.
        /// </summary>
        /// <param name="Object">A time zone to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Time_Zone timeZone &&
                   Equals(timeZone);

        #endregion

        #region Equals(TimeZone)

        /// <summary>
        /// Compares two time zones for equality.
        /// </summary>
        /// <param name="TimeZone">A time zone to compare with.</param>
        public Boolean Equals(Time_Zone TimeZone)

            => String.Equals(InternalId,
                             TimeZone.InternalId,
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
