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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A hour and a minute.
    /// </summary>
    public readonly struct HourMinute : IEquatable<HourMinute>,
                                        IComparable<HourMinute>,
                                        IComparable
    {

        #region Properties

        /// <summary>
        /// The hour.
        /// </summary>
        public readonly Byte Hour      { get; }

        /// <summary>
        /// The minute.
        /// </summary>
        public readonly Byte Minute    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new hour/minute.
        /// </summary>
        /// <param name="Hour">The hour.</param>
        /// <param name="Minute">The minute.</param>
        public HourMinute(Byte  Hour,
                          Byte  Minute)
        {

            if (Hour > 23)
                throw new ArgumentException("The given hour is invalid!",   nameof(Hour));

            if (Minute > 59)
                throw new ArgumentException("The given minute is invalid!", nameof(Minute));

            this.Hour    = Hour;
            this.Minute  = Minute;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a HourMinute.
        /// </summary>
        /// <param name="Text">A text representation of a HourMinute.</param>
        public static HourMinute Parse(String Text)
        {

            if (TryParse(Text, out HourMinute hourMin))
                return hourMin;

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a HourMinute must not be null or empty!");

            throw new ArgumentException("The given text representation of a HourMinute îs invalid!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a HourMinute.
        /// </summary>
        /// <param name="Text">A text representation of a HourMinute.</param>
        public static HourMinute? TryParse(String Text)
        {

            if (TryParse(Text, out HourMinute hourMin))
                return hourMin;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out HourMinute)

        public static Boolean TryParse(String Text, out HourMinute HourMinute)
        {

            HourMinute = default;

            if (Text.IsNotNullOrEmpty())
            {
                try
                {

                    var Splited = Text.Split(':');

                    if (Splited.Length != 2)
                        return false;

                    if (!Byte.TryParse(Splited[0], out Byte Hour))
                        return false;

                    if (!Byte.TryParse(Splited[1], out Byte Minute))
                        return false;

                    HourMinute = new HourMinute(Hour, Minute);

                    return true;

                }
                catch (Exception)
                { }
            }

            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public HourMinute Clone

            => new HourMinute(Hour,
                              Minute);

        #endregion


        #region Operator overloading

        #region Operator == (HourMinute1, HourMinute2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute1">A HourMinute.</param>
        /// <param name="HourMinute2">Another HourMinute.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (HourMinute HourMinute1,
                                           HourMinute HourMinute2)

            => HourMinute1.Equals(HourMinute2);

        #endregion

        #region Operator != (HourMinute1, HourMinute2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute1">A HourMinute.</param>
        /// <param name="HourMinute2">Another HourMinute.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (HourMinute HourMinute1,
                                           HourMinute HourMinute2)

            => !(HourMinute1 == HourMinute2);

        #endregion

        #region Operator <  (HourMinute1, HourMinute2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute1">A HourMinute.</param>
        /// <param name="HourMinute2">Another HourMinute.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (HourMinute HourMinute1,
                                          HourMinute HourMinute2)

            => HourMinute1.CompareTo(HourMinute2) < 0;

        #endregion

        #region Operator <= (HourMinute1, HourMinute2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute1">A HourMinute.</param>
        /// <param name="HourMinute2">Another HourMinute.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (HourMinute HourMinute1,
                                           HourMinute HourMinute2)

            => !(HourMinute1 > HourMinute2);

        #endregion

        #region Operator >  (HourMinute1, HourMinute2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute1">A HourMinute.</param>
        /// <param name="HourMinute2">Another HourMinute.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (HourMinute HourMinute1,
                                          HourMinute HourMinute2)

            => HourMinute1.CompareTo(HourMinute2) > 0;

        #endregion

        #region Operator >= (HourMinute1, HourMinute2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute1">A HourMinute.</param>
        /// <param name="HourMinute2">Another HourMinute.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (HourMinute HourMinute1,
                                           HourMinute HourMinute2)

            => !(HourMinute1 < HourMinute2);

        #endregion

        #endregion

        #region IComparable<HourMinute> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is HourMinute HourMinute
                   ? CompareTo(HourMinute)
                   : throw new ArgumentException("The given object is not a HourMinute!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(HourMinute)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HourMinute">An object to compare with.</param>
        public Int32 CompareTo(HourMinute HourMinute)
        {

            var result = Hour.CompareTo(HourMinute.Hour);

            return result == 0
                       ? Minute.CompareTo(HourMinute.Minute)
                       : result;

        }

        #endregion

        #endregion

        #region IEquatable<HourMinute> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is HourMinute hourMinute &&
                   Equals(hourMinute);

        #endregion

        #region Equals(HourMinute)

        /// <summary>
        /// Compares two HourMinutes for equality.
        /// </summary>
        /// <param name="HourMinute">A HourMinute to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(HourMinute HourMinute)

            => Hour.  Equals(HourMinute.Hour) &&
               Minute.Equals(HourMinute.Minute);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return Hour.  GetHashCode() * 3 ^
                       Minute.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Hour.ToString("D2"),
                             ":",
                             Minute.ToString("D2"));

        #endregion

    }

}
