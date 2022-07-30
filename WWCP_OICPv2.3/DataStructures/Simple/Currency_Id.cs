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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for currency identifications.
    /// </summary>
    public static class CurrencyIdExtensions
    {

        /// <summary>
        /// Indicates whether this currency identification is null or empty.
        /// </summary>
        /// <param name="CurrencyId">A currency identification.</param>
        public static Boolean IsNullOrEmpty(this Currency_Id? CurrencyId)
            => !CurrencyId.HasValue || CurrencyId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this currency identification is null or empty.
        /// </summary>
        /// <param name="CurrencyId">A currency identification.</param>
        public static Boolean IsNotNullOrEmpty(this Currency_Id? CurrencyId)
            => CurrencyId.HasValue && CurrencyId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a currency.
    /// ISO 4217 currencies, see: https://www.iso.org/iso-4217-currency-codes.html
    /// </summary>
    public readonly struct Currency_Id : IId<Currency_Id>
    {

        #region Data

        //ToDo: Implement proper currency id format!
        // ([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?P[A-Za-z0-9\*]{1,30})

        //ToDo: Replace with better randomness!
        private static readonly Random _Random = new Random();

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
        /// The length of the currency identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new currency identification.
        /// based on the given string.
        /// </summary>
        private Currency_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a currency identification.
        /// </summary>
        /// <param name="Text">A text-representation of a currency identification.</param>
        public static Currency_Id Parse(String Text)
        {

            if (TryParse(Text, out Currency_Id currencyId))
                return currencyId;

            throw new ArgumentException("Invalid text-representation of a currency identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a currency identification.
        /// </summary>
        /// <param name="Text">A text-representation of a currency identification.</param>
        public static Currency_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Currency_Id currencyId))
                return currencyId;

            return null;

        }

        #endregion

        #region TryParse(Text, out CurrencyId)

        /// <summary>
        /// Try to parse the given string as a currency identification.
        /// </summary>
        /// <param name="Text">A text-representation of a currency identification.</param>
        /// <param name="CurrencyId">The parsed currency identification.</param>
        public static Boolean TryParse(String Text, out Currency_Id CurrencyId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    CurrencyId = new Currency_Id(Text);
                    return true;
                }
                catch
                { }
            }

            CurrencyId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this currency identification.
        /// </summary>
        public Currency_Id Clone

            => new Currency_Id(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => CurrencyId1.Equals(CurrencyId2);

        #endregion

        #region Operator != (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => !CurrencyId1.Equals(CurrencyId2);

        #endregion

        #region Operator <  (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Currency_Id CurrencyId1,
                                          Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) < 0;

        #endregion

        #region Operator <= (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) <= 0;

        #endregion

        #region Operator >  (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Currency_Id CurrencyId1,
                                          Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) > 0;

        #endregion

        #region Operator >= (CurrencyId1, CurrencyId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId1">A currency identification.</param>
        /// <param name="CurrencyId2">Another currency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Currency_Id CurrencyId1,
                                           Currency_Id CurrencyId2)

            => CurrencyId1.CompareTo(CurrencyId2) >= 0;

        #endregion

        #endregion

        #region IComparable<CurrencyId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Currency_Id currencyId
                   ? CompareTo(currencyId)
                   : throw new ArgumentException("The given object is not a currency identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CurrencyId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CurrencyId">An object to compare with.</param>
        public Int32 CompareTo(Currency_Id CurrencyId)

            => String.Compare(InternalId,
                              CurrencyId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<CurrencyId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is Currency_Id currencyId &&
                   Equals(currencyId);

        #endregion

        #region Equals(CurrencyId)

        /// <summary>
        /// Compares two CurrencyIds for equality.
        /// </summary>
        /// <param name="CurrencyId">A currency identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Currency_Id CurrencyId)

            => String.Equals(InternalId,
                             CurrencyId.InternalId,
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
