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
    /// Extension methods for hash values.
    /// </summary>
    public static class HashValueExtensions
    {

        /// <summary>
        /// Indicates whether this hash value is null or empty.
        /// </summary>
        /// <param name="HashValue">A hash value.</param>
        public static Boolean IsNullOrEmpty(this Hash_Value? HashValue)
            => !HashValue.HasValue || HashValue.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this hash value is null or empty.
        /// </summary>
        /// <param name="HashValue">A hash value.</param>
        public static Boolean IsNotNullOrEmpty(this Hash_Value? HashValue)
            => HashValue.HasValue && HashValue.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// A hash value.
    /// </summary>
    public readonly struct Hash_Value : IId<Hash_Value>
    {

        #region Data

        //ToDo: Implement proper hash value format!
        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#HashValueType
        // ^[0-9A-Za-z\\.+/=\\$]{10,100}$

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this hash value is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this hash value is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the hash value.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new hash value based on the given string.
        /// </summary>
        private Hash_Value(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a hash value.
        /// </summary>
        /// <param name="Text">A text representation of a hash value.</param>
        public static Hash_Value Parse(String Text)
        {

            if (TryParse(Text, out var hashValue))
                return hashValue;

            throw new ArgumentException($"Invalid text representation of a hash value: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a hash value.
        /// </summary>
        /// <param name="Text">A text representation of a hash value.</param>
        public static Hash_Value? TryParse(String Text)
        {

            if (TryParse(Text, out var hashValue))
                return hashValue;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out HashValue)

        /// <summary>
        /// Try to parse the given string as a hash value.
        /// </summary>
        /// <param name="Text">A text representation of a hash value.</param>
        /// <param name="HashValue">The parsed hash value.</param>
        public static Boolean TryParse(String Text, out Hash_Value HashValue)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    HashValue = new Hash_Value(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            HashValue = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this hash value.
        /// </summary>
        public Hash_Value Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (HashValue1, HashValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashValue1">A hash value.</param>
        /// <param name="HashValue2">Another hash value.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Hash_Value HashValue1,
                                           Hash_Value HashValue2)

            => HashValue1.Equals(HashValue2);

        #endregion

        #region Operator != (HashValue1, HashValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashValue1">A hash value.</param>
        /// <param name="HashValue2">Another hash value.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Hash_Value HashValue1,
                                           Hash_Value HashValue2)

            => !HashValue1.Equals(HashValue2);

        #endregion

        #region Operator <  (HashValue1, HashValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashValue1">A hash value.</param>
        /// <param name="HashValue2">Another hash value.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Hash_Value HashValue1,
                                          Hash_Value HashValue2)

            => HashValue1.CompareTo(HashValue2) < 0;

        #endregion

        #region Operator <= (HashValue1, HashValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashValue1">A hash value.</param>
        /// <param name="HashValue2">Another hash value.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Hash_Value HashValue1,
                                           Hash_Value HashValue2)

            => HashValue1.CompareTo(HashValue2) <= 0;

        #endregion

        #region Operator >  (HashValue1, HashValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashValue1">A hash value.</param>
        /// <param name="HashValue2">Another hash value.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Hash_Value HashValue1,
                                          Hash_Value HashValue2)

            => HashValue1.CompareTo(HashValue2) > 0;

        #endregion

        #region Operator >= (HashValue1, HashValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashValue1">A hash value.</param>
        /// <param name="HashValue2">Another hash value.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Hash_Value HashValue1,
                                           Hash_Value HashValue2)

            => HashValue1.CompareTo(HashValue2) >= 0;

        #endregion

        #endregion

        #region IComparable<HashValue> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two hash values.
        /// </summary>
        /// <param name="Object">A hash value to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Hash_Value hashValue
                   ? CompareTo(hashValue)
                   : throw new ArgumentException("The given object is not a hash value!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(HashValue)

        /// <summary>
        /// Compares two hash values.
        /// </summary>
        /// <param name="HashValue">A hash value to compare with.</param>
        public Int32 CompareTo(Hash_Value HashValue)

            => String.Compare(InternalId,
                              HashValue.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<HashValue> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two hash values for equality.
        /// </summary>
        /// <param name="Object">A hash value to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Hash_Value hashValue &&
                   Equals(hashValue);

        #endregion

        #region Equals(HashValue)

        /// <summary>
        /// Compares two hash values for equality.
        /// </summary>
        /// <param name="HashValue">A hash value to compare with.</param>
        public Boolean Equals(Hash_Value HashValue)

            => String.Equals(InternalId,
                             HashValue.InternalId,
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
