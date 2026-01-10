/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for phone numbers.
    /// </summary>
    public static class PhoneNumberExtensions
    {

        /// <summary>
        /// Indicates whether this phone number is null or empty.
        /// </summary>
        /// <param name="PhoneNumber">An phone number.</param>
        public static Boolean IsNullOrEmpty(this Phone_Number? PhoneNumber)
            => !PhoneNumber.HasValue || PhoneNumber.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this phone number is null or empty.
        /// </summary>
        /// <param name="PhoneNumber">An phone number.</param>
        public static Boolean IsNotNullOrEmpty(this Phone_Number? PhoneNumber)
            => PhoneNumber.HasValue && PhoneNumber.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// A phone number.
    /// </summary>
    public readonly struct Phone_Number : IId<Phone_Number>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a phone number.
        /// </summary>
        /// <remarks>https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#PhoneNumberType</remarks>
        public static readonly Regex Phone_Number_RegEx = new (@"^\+[0-9]{5,15}$",
                                                               RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this phone number is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this phone number is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the phone number identifier.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new phone number.
        /// based on the given string.
        /// </summary>
        private Phone_Number(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a phone number.
        /// </summary>
        /// <param name="Text">A text representation of a phone number.</param>
        public static Phone_Number Parse(String Text)
        {

            if (TryParse(Text, out var phoneNumber))
                return phoneNumber;

            throw new ArgumentException($"Invalid text representation of a phone number: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a phone number.
        /// </summary>
        /// <param name="Text">A text representation of a phone number.</param>
        public static Phone_Number? TryParse(String Text)
        {

            if (TryParse(Text, out var phoneNumber))
                return phoneNumber;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out PhoneNumber)

        /// <summary>
        /// Try to parse the given string as a phone number.
        /// </summary>
        /// <param name="Text">A text representation of a phone number.</param>
        /// <param name="PhoneNumber">The parsed phone number.</param>
        public static Boolean TryParse(String Text, out Phone_Number PhoneNumber)
        {

            if (!Text.IsNullOrEmpty())
            {

                Text = Text.Trim();

                if (Phone_Number_RegEx.IsMatch(Text))
                {
                    try
                    {
                        PhoneNumber = new Phone_Number(Text);
                        return true;
                    }
                    catch
                    { }
                }

            }

            PhoneNumber = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this phone number.
        /// </summary>
        public Phone_Number Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.Equals(PhoneNumber2);

        #endregion

        #region Operator != (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => !PhoneNumber1.Equals(PhoneNumber2);

        #endregion

        #region Operator <  (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) < 0;

        #endregion

        #region Operator <= (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) <= 0;

        #endregion

        #region Operator >  (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) > 0;

        #endregion

        #region Operator >= (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) >= 0;

        #endregion

        #endregion

        #region IComparable<PhoneNumber> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two phone numbers.
        /// </summary>
        /// <param name="Object">A phone number to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Phone_Number phoneNumber
                   ? CompareTo(phoneNumber)
                   : throw new ArgumentException("The given object is not a phone number!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PhoneNumber)

        /// <summary>
        /// Compares two phone numbers.
        /// </summary>
        /// <param name="PhoneNumber">A phone number to compare with.</param>
        public Int32 CompareTo(Phone_Number PhoneNumber)

            => String.Compare(InternalId,
                              PhoneNumber.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<PhoneNumber> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two phone numbers for equality.
        /// </summary>
        /// <param name="Object">A phone number to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Phone_Number phoneNumber &&
                   Equals(phoneNumber);

        #endregion

        #region Equals(PhoneNumber)

        /// <summary>
        /// Compares two phone numbers for equality.
        /// </summary>
        /// <param name="PhoneNumber">A phone number to compare with.</param>
        public Boolean Equals(Phone_Number PhoneNumber)

            => String.Equals(InternalId,
                             PhoneNumber.InternalId,
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
