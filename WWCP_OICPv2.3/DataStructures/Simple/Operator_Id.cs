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
    /// Extension methods for charging station operator identifications.
    /// </summary>
    public static class OperatorIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station operator identification is null or empty.
        /// </summary>
        /// <param name="OperatorId">A charging station operator identification.</param>
        public static Boolean IsNullOrEmpty(this Operator_Id? OperatorId)
            => !OperatorId.HasValue || OperatorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station operator identification is null or empty.
        /// </summary>
        /// <param name="OperatorId">A charging station operator identification.</param>
        public static Boolean IsNotNullOrEmpty(this Operator_Id? OperatorId)
            => OperatorId.HasValue && OperatorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging station operator.
    /// </summary>
    public readonly struct Operator_Id : IId<Operator_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station operator identification.
        /// Official regular expression: ^(([A-Za-z]{2}\*?[A-Za-z0-9]{3})|(\+?[0-9]{1,3}\*[0-9]{3}))$
        /// </summary>
        /// <remarks>https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OperatorIDType</remarks>
        public static readonly Regex OperatorId_RegEx = new (@"^([A-Za-z]{2})(\*?)([A-Za-z0-9]{3})$ | " +
                                                             @"^\+?([0-9]{1,3})\*([0-9]{3})$",
                                                             RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identifier suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the charging station operator identification.
        /// </summary>
        public OperatorIdFormats  Format        { get; }

        /// <summary>
        /// Return an URL encoded text representation of the charging station operator identification.
        /// </summary>
        public String             URLEncoded
            => ToString(Format).Replace("*", "%2A");

        /// <summary>
        /// Indicates whether this charging station operator identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging station operator identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the charging station operator identification.
        /// </summary>
        public UInt64 Length

            => Format switch {
                   OperatorIdFormats.DIN       => (UInt64) (CountryCode.TelefonCode.ToString().Length + 1 + (Suffix?.Length ?? 0)),
                   OperatorIdFormats.ISO_STAR  => (UInt64) (CountryCode.Alpha2Code.Length +             1 + (Suffix?.Length ?? 0)),
                   _                           => (UInt64) (CountryCode.Alpha2Code.Length +                 (Suffix?.Length ?? 0)), // ISO
               };

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator identification.
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


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static Operator_Id Parse(String Text)
        {

            if (TryParse(Text, out var operatorId))
                return operatorId;

            throw new ArgumentException($"Invalid text representation of an EVSE charging station operator identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse   (CountryCode, Suffix, IdFormat = IdFormatType.ISO)

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

            if (CountryCode is null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            return IdFormat switch {

                OperatorIdFormats.DIN => Parse("+" + CountryCode.TelefonCode.ToString() + "*" + Suffix),

                OperatorIdFormats.ISO => Parse(CountryCode.Alpha2Code +       Suffix),

                _                     => Parse(CountryCode.Alpha2Code + "*" + Suffix)

            };

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static Operator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var operatorId))
                return operatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out OperatorId)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        /// <param name="OperatorId">The parsed charging station operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Operator_Id  OperatorId)
        {

            if (Text.IsNotNullOrEmpty())
            {

                try
                {

                    var matchCollection = OperatorId_RegEx.Matches(Text.Trim());

                    if (matchCollection.Count == 1)
                    {

                        // DE...
                        if (Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out Country country))
                        {

                            OperatorId = new Operator_Id(
                                             country,
                                             matchCollection[0].Groups[3].Value,
                                             matchCollection[0].Groups[2].Value == "*"
                                                 ? OperatorIdFormats.ISO_STAR
                                                 : OperatorIdFormats.ISO
                                         );

                            return true;

                        }

                        // +49*...
                        if (Country.TryParseTelefonCode(matchCollection[0].Groups[4].Value, out country))
                        {

                            OperatorId = new Operator_Id(
                                             country,
                                             matchCollection[0].Groups[5].Value,
                                             OperatorIdFormats.DIN
                                         );

                            return true;

                        }

                        // An unknown/unassigned alpha-2 country code, like e.g. "DT"...
                        if (Country.Alpha2Codes_RegEx.IsMatch(matchCollection[0].Groups[1].Value))
                        {

                            OperatorId = new Operator_Id(
                                             new Country(
                                                 I18NString.Create(matchCollection[0].Groups[1].Value),
                                                 matchCollection[0].Groups[1].Value,
                                                 matchCollection[0].Groups[1].Value + "X",
                                                 0,
                                                 0
                                             ),
                                             matchCollection[0].Groups[3].Value,
                                             matchCollection[0].Groups[2].Value == "*"
                                                 ? OperatorIdFormats.ISO_STAR
                                                 : OperatorIdFormats.ISO
                                         );

                            return true;

                        }

                    }

                }

                catch
                { }

            }

            OperatorId = default;
            return false;

        }

        #endregion

        #region (static) TryParse(CountryCode, Suffix, out OperatorId, IdFormat = OperatorIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of a charging station operator identification.</param>
        /// <param name="OperatorId">The parsed e-mobility charging station operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility charging station operator identification.</param>
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out Operator_Id    OperatorId,
                                       OperatorIdFormats  IdFormat = OperatorIdFormats.ISO_STAR)
        {

            if (CountryCode is not null && Suffix.IsNeitherNullNorEmpty())
            {
                return IdFormat switch {

                    OperatorIdFormats.DIN => TryParse("+" + CountryCode.TelefonCode.ToString() + "*" + Suffix,
                                                      out OperatorId),

                    OperatorIdFormats.ISO => TryParse(CountryCode.Alpha2Code +       Suffix,
                                                      out OperatorId),

                    _                     => TryParse(CountryCode.Alpha2Code + "*" + Suffix,
                                                      out OperatorId),

                };
            }

            OperatorId = default;
            return false;

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public Operator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging station operator identification.
        /// </summary>
        public Operator_Id Clone()

            => new (
                   CountryCode,
                   Suffix.CloneString(),
                   Format
               );

        #endregion


        #region Operator overloading

        #region Operator == (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator != (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => !OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator <  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Operator_Id OperatorId1,
                                          Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) < 0;

        #endregion

        #region Operator <= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) <= 0;

        #endregion

        #region Operator >  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Operator_Id OperatorId1,
                                          Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) > 0;

        #endregion

        #region Operator >= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Operator_Id OperatorId1,
                                           Operator_Id OperatorId2)

            => OperatorId1.CompareTo(OperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<OperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station operator identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station operator identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Operator_Id operatorId
                   ? CompareTo(operatorId)
                   : throw new ArgumentException("The given object is not a charging station operator identification!", nameof(Object));

        #endregion

        #region CompareTo(OperatorId)

        /// <summary>
        /// Compares two charging station operator identifications for equality.
        /// </summary>
        /// <param name="OperatorId">A charging station operator identification to compare with.</param>
        public Int32 CompareTo(Operator_Id OperatorId)
        {

            var c = CountryCode.CompareTo(OperatorId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   OperatorId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<OperatorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station operator identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station operator identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Operator_Id operatorId &&
                   Equals(operatorId);

        #endregion

        #region Equals(OperatorId)

        /// <summary>
        /// Compares two charging station operator identifications for equality.
        /// </summary>
        /// <param name="OperatorId">A charging station operator identification to compare with.</param>
        public Boolean Equals(Operator_Id OperatorId)

            => CountryCode.Equals(OperatorId.CountryCode) &&

                   String.Equals(Suffix,
                                 OperatorId.Suffix,
                                 StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => ToString(Format);

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)

            => Format switch {
                   OperatorIdFormats.DIN       => $"+{CountryCode.TelefonCode}*{Suffix}",
                   OperatorIdFormats.ISO_STAR  => $"{ CountryCode.Alpha2Code }*{Suffix}",
                   _                           =>     CountryCode.Alpha2Code + Suffix  // ISO
               };

        #endregion


    }

}
