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
    /// The unique identification of a phone number.
    /// </summary>
    public readonly struct Phone_Number : IId<Phone_Number>
    {

        #region Data

        //ToDo: Implement proper phone number id format!
        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#PhoneNumberType
        // ^\+[0-9]{5,15}$

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
        /// The length of the phone number identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

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


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a phone number.
        /// </summary>
        /// <param name="Text">A text-representation of a phone number.</param>
        public static Phone_Number Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text-representation of a phone number must not be null or empty!");

            #endregion

            if (TryParse(Text, out Phone_Number chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException("Illegal text-representation of a phone number: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a phone number.
        /// </summary>
        /// <param name="Text">A text-representation of a phone number.</param>
        public static Phone_Number? TryParse(String Text)
        {

            if (TryParse(Text, out Phone_Number chargingPoolId))
                return chargingPoolId;

            return new Phone_Number?();

        }

        #endregion

        #region TryParse(Text, out PhoneNumber)

        /// <summary>
        /// Try to parse the given string as a phone number.
        /// </summary>
        /// <param name="Text">A text-representation of a phone number.</param>
        /// <param name="PhoneNumber">The parsed phone number.</param>
        public static Boolean TryParse(String Text, out Phone_Number PhoneNumber)
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
                PhoneNumber = new Phone_Number(Text);
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
        /// Clone this phone number.
        /// </summary>
        public Phone_Number Clone

            => new Phone_Number(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.Equals(PhoneNumber2);

        #endregion

        #region Operator != (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => !PhoneNumber1.Equals(PhoneNumber2);

        #endregion

        #region Operator <  (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) < 0;

        #endregion

        #region Operator <= (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) <= 0;

        #endregion

        #region Operator >  (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
            => PhoneNumber1.CompareTo(PhoneNumber2) > 0;

        #endregion

        #region Operator >= (PhoneNumber1, PhoneNumber2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber1">A phone number.</param>
        /// <param name="PhoneNumber2">Another phone number.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Phone_Number PhoneNumber1, Phone_Number PhoneNumber2)
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

            => Object is Phone_Number chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a phone number!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PhoneNumber)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PhoneNumber">An object to compare with.</param>
        public Int32 CompareTo(Phone_Number PhoneNumber)

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

            => Object is Phone_Number chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(PhoneNumber)

        /// <summary>
        /// Compares two PhoneNumbers for equality.
        /// </summary>
        /// <param name="PhoneNumber">A phone number to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Phone_Number PhoneNumber)

            => String.Equals(InternalId,
                             PhoneNumber.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
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
