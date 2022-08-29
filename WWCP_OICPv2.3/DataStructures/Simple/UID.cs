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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for user identifications.
    /// </summary>
    public static class UIDExtensions
    {

        /// <summary>
        /// Indicates whether this user identification is null or empty.
        /// </summary>
        /// <param name="UID">An user identification.</param>
        public static Boolean IsNullOrEmpty(this UID? UID)
            => !UID.HasValue || UID.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this user identification is null or empty.
        /// </summary>
        /// <param name="UID">An user identification.</param>
        public static Boolean IsNotNullOrEmpty(this UID? UID)
            => UID.HasValue && UID.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a RFID card (user).
    /// </summary>
    public readonly struct UID : IId<UID>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        /// <summary>
        /// The regular expression for parsing a RFID card (user) identification.
        /// </summary>
        public static readonly Regex UID_RegEx  = new Regex("^([A-F0-9]{8})$ | ^([A-F0-9]{14})$ | ^([A-F0-9]{20})$",
                                                            RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this RFID card (user) identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this RFID card (user) identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the RFID card (user) identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RFID card (user) identification based on the given string.
        /// </summary>
        /// <param name="Text">The value of the RFID card (user) identification.</param>
        private UID(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a RFID card (user) identification.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID card (user) identification.</param>
        public static UID Parse(String Text)
        {

            if (TryParse(Text, out UID uid))
                return uid;

            throw new ArgumentException("Invalid text-representation of a RFID card (user) identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a RFID card (user) identification.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID card (user) identification.</param>
        public static UID? TryParse(String Text)
        {

            if (TryParse(Text, out UID uid))
                return uid;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out UID)

        /// <summary>
        /// Try to parse the given string as a RFID card (user) identification.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID card (user) identification.</param>
        /// <param name="UID">The parsed RFID card (user) identification.</param>
        public static Boolean TryParse(String Text, out UID UID)
        {

            Text = Text?.Trim();

            if (!Text.IsNullOrEmpty() &&
                UID_RegEx.IsMatch(Text))
            {
                try
                {
                    UID = new UID(Text);
                    return true;
                }
                catch
                { }
            }

            UID = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this RFID card identification.
        /// </summary>
        public UID Clone

            => new UID(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card (user) identification.</param>
        /// <param name="UID2">Another RFID card (user) identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (UID UID1,
                                           UID UID2)

            => UID1.Equals(UID2);

        #endregion

        #region Operator != (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card (user) identification.</param>
        /// <param name="UID2">Another RFID card (user) identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (UID UID1,
                                           UID UID2)

            => !(UID1 == UID2);

        #endregion

        #region Operator <  (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card (user) identification.</param>
        /// <param name="UID2">Another RFID card (user) identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (UID UID1,
                                          UID UID2)

            => UID1.CompareTo(UID2) < 0;

        #endregion

        #region Operator <= (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card (user) identification.</param>
        /// <param name="UID2">Another RFID card (user) identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (UID UID1,
                                           UID UID2)

            => !(UID1 > UID2);

        #endregion

        #region Operator >  (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card (user) identification.</param>
        /// <param name="UID2">Another RFID card (user) identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (UID UID1,
                                          UID UID2)

            => UID1.CompareTo(UID2) > 0;

        #endregion

        #region Operator >= (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card (user) identification.</param>
        /// <param name="UID2">Another RFID card (user) identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (UID UID1,
                                           UID UID2)

            => !(UID1 < UID2);

        #endregion

        #endregion

        #region IComparable<UID> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is UID uid
                   ? CompareTo(uid)
                   : throw new ArgumentException("The given object is not a RFID card (user) identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(UID)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID">An object to compare with.</param>
        public Int32 CompareTo(UID UID)

            => String.Compare(InternalId,
                              UID.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<UID> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is UID uid &&
                   Equals(uid);

        #endregion

        #region Equals(UID)

        /// <summary>
        /// Compares two UIDs for equality.
        /// </summary>
        /// <param name="UID">A RFID card identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(UID UID)

            => String.Equals(InternalId,
                             UID.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
