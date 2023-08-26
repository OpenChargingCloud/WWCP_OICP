/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
    /// Extension methods for PINs.
    /// </summary>
    public static class PINExtensions
    {

        /// <summary>
        /// Indicates whether this PIN is null or empty.
        /// </summary>
        /// <param name="PIN">A PIN.</param>
        public static Boolean IsNullOrEmpty(this PIN? PIN)
            => !PIN.HasValue || PIN.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this PIN is null or empty.
        /// </summary>
        /// <param name="PIN">A PIN.</param>
        public static Boolean IsNotNullOrEmpty(this PIN? PIN)
            => PIN.HasValue && PIN.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// A PIN.
    /// </summary>
    public readonly struct PIN : IId<PIN>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        /// <summary>
        /// The regular expression for parsing a PIN.
        /// </summary>
        public static readonly Regex PIN_RegEx  = new Regex("^([a-fA-F0-9]{1,20})$",
                                                            RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this PIN is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this PIN is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the PIN.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new PIN based on the given string.
        /// </summary>
        /// <param name="Text">The value of the PIN.</param>
        private PIN(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a PIN.
        /// </summary>
        /// <param name="Text">A text representation of a PIN.</param>
        public static PIN Parse(String Text)
        {

            if (TryParse(Text, out var pin))
                return pin;

            throw new ArgumentException($"Invalid text representation of a PIN: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a PIN.
        /// </summary>
        /// <param name="Text">A text representation of a PIN.</param>
        public static PIN? TryParse(String Text)
        {

            if (TryParse(Text, out var pin))
                return pin;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out PIN)

        /// <summary>
        /// Try to parse the given string as a PIN.
        /// </summary>
        /// <param name="Text">A text representation of a PIN.</param>
        /// <param name="PIN">The parsed PIN.</param>
        public static Boolean TryParse(String Text, out PIN PIN)
        {

            if (!Text.IsNullOrEmpty())
            {

                Text = Text.Trim();

                if (PIN_RegEx.IsMatch(Text))
                {
                    try
                    {
                        PIN = new PIN(Text);
                        return true;
                    }
                    catch
                    { }
                }

            }

            PIN = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this PIN.
        /// </summary>
        public PIN Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PIN1, PIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PIN1">A PIN.</param>
        /// <param name="PIN2">Another PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PIN PIN1,
                                           PIN PIN2)

            => PIN1.Equals(PIN2);

        #endregion

        #region Operator != (PIN1, PIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PIN1">A PIN.</param>
        /// <param name="PIN2">Another PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PIN PIN1,
                                           PIN PIN2)

            => !(PIN1 == PIN2);

        #endregion

        #region Operator <  (PIN1, PIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PIN1">A PIN.</param>
        /// <param name="PIN2">Another PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PIN PIN1,
                                          PIN PIN2)

            => PIN1.CompareTo(PIN2) < 0;

        #endregion

        #region Operator <= (PIN1, PIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PIN1">A PIN.</param>
        /// <param name="PIN2">Another PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PIN PIN1,
                                           PIN PIN2)

            => !(PIN1 > PIN2);

        #endregion

        #region Operator >  (PIN1, PIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PIN1">A PIN.</param>
        /// <param name="PIN2">Another PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PIN PIN1,
                                          PIN PIN2)

            => PIN1.CompareTo(PIN2) > 0;

        #endregion

        #region Operator >= (PIN1, PIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PIN1">A PIN.</param>
        /// <param name="PIN2">Another PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PIN PIN1,
                                           PIN PIN2)

            => !(PIN1 < PIN2);

        #endregion

        #endregion

        #region IComparable<PIN> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two PINs.
        /// </summary>
        /// <param name="Object">A PIN to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is PIN pin
                   ? CompareTo(pin)
                   : throw new ArgumentException("The given object is not a PIN!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PIN)

        /// <summary>
        /// Compares two PINs.
        /// </summary>
        /// <param name="PIN">A PIN to compare with.</param>
        public Int32 CompareTo(PIN PIN)

            => String.Compare(InternalId,
                              PIN.InternalId,
                              StringComparison.Ordinal);

        #endregion

        #endregion

        #region IEquatable<PIN> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two PINs for equality.
        /// </summary>
        /// <param name="Object">A PIN to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PIN pin &&
                   Equals(pin);

        #endregion

        #region Equals(PIN)

        /// <summary>
        /// Compares two PINs for equality.
        /// </summary>
        /// <param name="PIN">A PIN to compare with.</param>
        public Boolean Equals(PIN PIN)

            => String.Equals(InternalId,
                             PIN.InternalId,
                             StringComparison.Ordinal);

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
