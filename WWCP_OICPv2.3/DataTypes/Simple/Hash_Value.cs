/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
    /// The unique identification of a hash value.
    /// </summary>
    public readonly struct Hash_Value : IId<Hash_Value>
    {

        #region Data

        //ToDo: Implement proper hash value id format!
        // https://github.com/ahzf/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#HashValueType
        // ^[0-9A-Za-z\\.+/=\\$]{10,100}$

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
        /// The length of the hash value identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

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


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a hash value.
        /// </summary>
        /// <param name="Text">A text-representation of a hash value.</param>
        public static Hash_Value Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text-representation of a hash value must not be null or empty!");

            #endregion

            if (TryParse(Text, out Hash_Value chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException("Invalid text-representation of a hash value: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a hash value.
        /// </summary>
        /// <param name="Text">A text-representation of a hash value.</param>
        public static Hash_Value? TryParse(String Text)
        {

            if (TryParse(Text, out Hash_Value chargingPoolId))
                return chargingPoolId;

            return new Hash_Value?();

        }

        #endregion

        #region TryParse(Text, out PhoneNumber)

        /// <summary>
        /// Try to parse the given string as a hash value.
        /// </summary>
        /// <param name="Text">A text-representation of a hash value.</param>
        /// <param name="PhoneNumber">The parsed hash value.</param>
        public static Boolean TryParse(String Text, out Hash_Value PhoneNumber)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                PhoneNumber = default;
                return false;
            }

            #endregion

            try
            {
                PhoneNumber = new Hash_Value(Text);
                return true;
            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            PhoneNumber = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public Hash_Value Clone

            => new Hash_Value(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A hash value.</param>
        /// <param name="PhoneNumber2">Another hash value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Hash_Value PhoneNumber1, Hash_Value PhoneNumber2)
            => PhoneNumber1.Equals(PhoneNumber2);

        #endregion

        #region Operator != (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A hash value.</param>
        /// <param name="PhoneNumber2">Another hash value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Hash_Value PhoneNumber1, Hash_Value PhoneNumber2)
            => !PhoneNumber1.Equals(PhoneNumber2);

        #endregion

        #region Operator <  (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A hash value.</param>
        /// <param name="PhoneNumber2">Another hash value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Hash_Value PhoneNumber1, Hash_Value PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) < 0;

        #endregion

        #region Operator <= (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A hash value.</param>
        /// <param name="PhoneNumber2">Another hash value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Hash_Value PhoneNumber1, Hash_Value PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) <= 0;

        #endregion

        #region Operator >  (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A hash value.</param>
        /// <param name="PhoneNumber2">Another hash value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Hash_Value PhoneNumber1, Hash_Value PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) > 0;

        #endregion

        #region Operator >= (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A hash value.</param>
        /// <param name="PhoneNumber2">Another hash value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Hash_Value PhoneNumber1, Hash_Value PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) >= 0;

        #endregion

        #endregion

        #region IComparable<PhoneNumber> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is Hash_Value chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a hash value!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PhoneNumber)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber">An object to compare with.</param>
        public Int32 CompareTo(Hash_Value PhoneNumber)

            => String.Compare(InternalId,
                              PhoneNumber.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<PhoneNumber> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is Hash_Value chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(PhoneNumber)

        /// <summary>
        /// Compares two PhoneNumbers for equality.
        /// </summary>
        /// <param name="PhoneNumber">A hash value to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Hash_Value PhoneNumber)

            => String.Equals(InternalId,
                             PhoneNumber.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
