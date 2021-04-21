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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The unique identification of an operator.
    /// </summary>
    public readonly struct Operator_Id : IId<Operator_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an operator identification.
        /// Official regular expression: ^(([A-Za-z]{2}\*?[A-Za-z0-9]{3})|(\+?[0-9]{1,3}\*[0-9]{3}))$
        /// </summary>
        /// <remarks>https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OperatorIDType</remarks>
        public static readonly Regex  OperatorId_RegEx  = new Regex(@"^([A-Za-z]{2})(\*?)([A-Za-z0-9]{3})$ | " +
                                                                    @"^\+?([0-9]{1,3})\*([0-9]{3})$",
                                                                    RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the operator identification.
        /// </summary>
        public OperatorIdFormats  Format        { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case OperatorIdFormats.DIN:
                        return (UInt64) (CountryCode.TelefonCode.ToString().Length + 1 + Suffix?.Length);

                    case OperatorIdFormats.ISO_STAR:
                        return (UInt64) (CountryCode.Alpha2Code.Length             + 1 + Suffix?.Length);

                    default:  // ISO
                        return (UInt64) (CountryCode.Alpha2Code.Length                 + Suffix?.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging station operator identification.</param>
        /// <param name="Format">The format of the charging station operator identification.</param>
        private Operator_Id(Country            CountryCode,
                            String             Suffix,
                            OperatorIdFormats  Format = OperatorIdFormats.ISO)
        {

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging station operator identification suffix must not be null or empty!");

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text-representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging station operator identification.</param>
        public static Operator_Id Parse(String Text)
        {

            if (TryParse(Text, out Operator_Id operatorId))
                return operatorId;

            throw new ArgumentException("Invalid text-representation of an EVSE operator identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.ISO)

        /// <summary>
        /// Parse the given string as an charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging station operator identification.</param>
        /// <param name="IdFormat">The format of the charging station operator identification [old|new].</param>
        public static Operator_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        OperatorIdFormats  IdFormat = OperatorIdFormats.ISO)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix != null)
                Suffix = Suffix.Trim();

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case OperatorIdFormats.ISO:
                    return Parse(CountryCode.Alpha2Code + Suffix);

                case OperatorIdFormats.ISO_STAR:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix);

                default: // DIN:
                    return Parse("+" + CountryCode.TelefonCode.ToString() + "*" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text-representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging station operator identification.</param>
        public static Operator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Operator_Id operatorId))
                return operatorId;

            return default;

        }

        #endregion

        #region TryParse(Text, out OperatorId)

        /// <summary>
        /// Try to parse the given text-representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging station operator identification.</param>
        /// <param name="OperatorId">The parsed charging station operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Operator_Id  OperatorId)
        {

            #region Initial checks

            OperatorId  = default;
            Text        = Text?.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var MatchCollection = OperatorId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                    return false;


                // DE...
                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country countryCode))
                {

                    OperatorId = new Operator_Id(countryCode,
                                                 MatchCollection[0].Groups[3].Value,
                                                 MatchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

                    return true;

                }

                // +49*...
                if (Country.TryParseTelefonCode(MatchCollection[0].Groups[4].Value, out countryCode))
                {

                    OperatorId = new Operator_Id(countryCode,
                                                 MatchCollection[0].Groups[5].Value,
                                                 OperatorIdFormats.DIN);

                    return true;

                }

            }

            catch (Exception)
            { }

            return false;

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public Operator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new Operator_Id(CountryCode,
                               Suffix,
                               NewFormat);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station operator identification.
        /// </summary>
        public Operator_Id Clone

            => new Operator_Id(CountryCode,
                               new String(Suffix.ToCharArray()),
                               Format);

        #endregion


        #region Operator overloading

        #region Operator == (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator != (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator <  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.CompareTo(OperatorId2) < 0;

        #endregion

        #region Operator <= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.CompareTo(OperatorId2) <= 0;

        #endregion

        #region Operator >  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.CompareTo(OperatorId2) > 0;

        #endregion

        #region Operator >= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.CompareTo(OperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<OperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is Operator_Id operatorId
                   ? CompareTo(operatorId)
                   : throw new ArgumentException("The given object is not a charging station operator identification!", nameof(Object));

        #endregion

        #region CompareTo(OperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId">An object to compare with.</param>
        public Int32 CompareTo(Operator_Id OperatorId)
        {

            var result = CountryCode.CompareTo(OperatorId.CountryCode);

            return result == 0
                       ? String.Compare(Suffix, OperatorId.Suffix, StringComparison.OrdinalIgnoreCase)
                       : result;

        }

        #endregion

        #endregion

        #region IEquatable<OperatorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is Operator_Id operatorId &&
                   Equals(operatorId);

        #endregion

        #region Equals(OperatorId)

        /// <summary>
        /// Compares two operator identifications for equality.
        /// </summary>
        /// <param name="OperatorId">An operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Operator_Id OperatorId)

            => CountryCode.Equals(OperatorId.CountryCode) &&
               String.Equals(Suffix, OperatorId.Suffix, StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => CountryCode.GetHashCode() ^
               Suffix.     GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (Format)
            {

                case OperatorIdFormats.DIN:
                    return "+" + CountryCode.TelefonCode.ToString() + "*" + Suffix;

                case OperatorIdFormats.ISO_STAR:
                    return CountryCode.Alpha2Code + "*" + Suffix;

                default: // ISO
                    return CountryCode.Alpha2Code       + Suffix;

            }

        }

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)
        {

            switch (Format)
            {

                case OperatorIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         Suffix);

                case OperatorIdFormats.ISO_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         Suffix);

                default: // DIN
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "*",
                                         Suffix);

            }

        }

        #endregion

    }

}
