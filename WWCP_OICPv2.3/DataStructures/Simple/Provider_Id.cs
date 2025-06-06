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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for provider identifications.
    /// </summary>
    public static class ProviderIdExtensions
    {

        /// <summary>
        /// Indicates whether this provider identification is null or empty.
        /// </summary>
        /// <param name="ProviderId">A provider identification.</param>
        public static Boolean IsNullOrEmpty(this Provider_Id? ProviderId)
            => !ProviderId.HasValue || ProviderId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this provider identification is null or empty.
        /// </summary>
        /// <param name="ProviderId">A provider identification.</param>
        public static Boolean IsNotNullOrEmpty(this Provider_Id? ProviderId)
            => ProviderId.HasValue && ProviderId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an e-mobility provider.
    /// </summary>
    public readonly struct Provider_Id : IId<Provider_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a e-mobility provider identification:
        /// ^([A-Za-z]{2}([\*|\-]?)[A-Za-z0-9]{3})$
        /// </summary>
        public static readonly Regex  ProviderId_RegEx = new (@"^([A-Za-z]{2})([\*|\-]?)([A-Za-z0-9]{3})$",
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
        /// The format of the e-mobility provider identification.
        /// </summary>
        public ProviderIdFormats  Format        { get; }

        /// <summary>
        /// Return an URL encoded text representation of the e-mobility provider identification.
        /// </summary>
        public String             URLEncoded
            => ToString(Format).Replace("*", "%2A");

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case ProviderIdFormats.DIN_STAR:
                        return (UInt64) (CountryCode.Alpha2Code.Length + 1 + (Suffix?.Length ?? 0));

                    case ProviderIdFormats.ISO:
                        return (UInt64) (CountryCode.Alpha2Code.Length     + (Suffix?.Length ?? 0));

                    default: // ISO_HYPHEN
                        return (UInt64) (CountryCode.Alpha2Code.Length + 1 + (Suffix?.Length ?? 0));

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the e-mobility provider identification.</param>
        /// <param name="Format">The format of the e-mobility provider identification.</param>
        private Provider_Id(Country            CountryCode,
                            String             Suffix,
                            ProviderIdFormats  Format = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The e-mobility provider identification suffix must not be null or empty!");

            #endregion

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static Provider_Id Parse(String Text)
        {

            if (TryParse(Text, out var providerId))
                return providerId;

            throw new ArgumentException($"Invalid text representation of an e-mobility provider identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse   (CountryCode, Suffix, IdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility provider identification.</param>
        public static Provider_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        ProviderIdFormats  IdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (CountryCode is null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given e-mobility provider identification suffix must not be null or empty!");

            #endregion

            return IdFormat switch {

                ProviderIdFormats.DIN        => Parse(CountryCode.Alpha2Code +       Suffix),

                ProviderIdFormats.DIN_STAR   => Parse(CountryCode.Alpha2Code + "*" + Suffix),

                ProviderIdFormats.DIN_HYPHEN => Parse(CountryCode.Alpha2Code + "-" + Suffix),

                ProviderIdFormats.ISO        => Parse(CountryCode.Alpha2Code +       Suffix),

                _                            => Parse(CountryCode.Alpha2Code + "-" + Suffix)

            };

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static Provider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var providerId))
                return providerId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ProviderId)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        /// <param name="ProviderId">The parsed e-mobility provider identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Provider_Id  ProviderId)
        {

            if (Text.IsNotNullOrEmpty())
            {

                try
                {

                    var matchCollection = ProviderId_RegEx.Matches(Text.Trim());

                    if (matchCollection.Count == 1)
                    {
                        if (Country.TryParseAlpha2Code(matchCollection[0].Groups[1].Value, out Country country))
                        {

                            ProviderId = new Provider_Id(country,
                                                         matchCollection[0].Groups[3].Value,
                                                         matchCollection[0].Groups[2].Value switch {
                                                             "-" => ProviderIdFormats.ISO_HYPHEN,
                                                             "*" => ProviderIdFormats.DIN_STAR,
                                                             _ => ProviderIdFormats.ISO,
                                                         });

                            return true;

                        }
                    }

                }
                catch
                { }

            }

            ProviderId = default;
            return false;

        }

        #endregion

        #region (static) TryParse(CountryCode, Suffix, out ProviderId, IdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility provider identification.</param>
        /// <param name="ProviderId">The parsed e-mobility provider identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility provider identification.</param>
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out Provider_Id    ProviderId,
                                       ProviderIdFormats  IdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            if (CountryCode is not null && Suffix.IsNeitherNullNorEmpty())
            {
                return IdFormat switch {

                    ProviderIdFormats.DIN        => TryParse(CountryCode.Alpha2Code +       Suffix,
                                                             out ProviderId),

                    ProviderIdFormats.DIN_STAR   => TryParse(CountryCode.Alpha2Code + "*" + Suffix,
                                                             out ProviderId),

                    ProviderIdFormats.DIN_HYPHEN => TryParse(CountryCode.Alpha2Code + "-" + Suffix,
                                                             out ProviderId),

                    ProviderIdFormats.ISO        => TryParse(CountryCode.Alpha2Code +       Suffix,
                                                             out ProviderId),

                    _                            => TryParse(CountryCode.Alpha2Code + "-" + Suffix,
                                                             out ProviderId),

                };
            }

            ProviderId = default;
            return false;

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new e-mobility provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new e-mobility provider identification format.</param>
        public Provider_Id ChangeFormat(ProviderIdFormats NewFormat)

            => new (CountryCode,
                    Suffix,
                    NewFormat);

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this e-mobility provider identification.
        /// </summary>
        public Provider_Id Clone()

            => new (
                   CountryCode,
                   Suffix.CloneString(),
                   Format
               );

        #endregion


        #region Operator overloading

        #region Operator == (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => ProviderId1.Equals(ProviderId2);

        #endregion

        #region Operator != (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => !ProviderId1.Equals(ProviderId2);

        #endregion

        #region Operator <  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Provider_Id ProviderId1,
                                          Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) < 0;

        #endregion

        #region Operator <= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) <= 0;

        #endregion

        #region Operator >  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Provider_Id ProviderId1,
                                          Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) > 0;

        #endregion

        #region Operator >= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">An e-mobility provider identification.</param>
        /// <param name="ProviderId2">Another e-mobility provider identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Provider_Id ProviderId1,
                                           Provider_Id ProviderId2)

            => ProviderId1.CompareTo(ProviderId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two e-mobility provider identifications.
        /// </summary>
        /// <param name="Object">An e-mobility provider identifications to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Provider_Id providerId
                   ? CompareTo(providerId)
                   : throw new ArgumentException("The given object is not an e-mobility provider identification!", nameof(Object));

        #endregion

        #region CompareTo(ProviderId)

        /// <summary>
        /// Compares two e-mobility provider identifications.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identifications to compare with.</param>
        public Int32 CompareTo(Provider_Id ProviderId)
        {

            var c = CountryCode.CompareTo(ProviderId.CountryCode);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ProviderId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ProviderId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two e-mobility provider identifications for equality.
        /// </summary>
        /// <param name="Object">An e-mobility provider identifications to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Provider_Id providerId &&
                   Equals(providerId);

        #endregion

        #region Equals(ProviderId)

        /// <summary>
        /// Compares two e-mobility provider identifications for equality.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identifications to compare with.</param>
        public Boolean Equals(Provider_Id ProviderId)

            => CountryCode.Equals(ProviderId.CountryCode) &&
               String.Equals(Suffix, ProviderId.Suffix, StringComparison.OrdinalIgnoreCase);

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
        public String ToString(ProviderIdFormats Format)

            => Format switch {
                   ProviderIdFormats.DIN       => CountryCode.Alpha2Code + Suffix,
                   ProviderIdFormats.DIN_STAR  => $"{CountryCode.Alpha2Code}*{Suffix}",
                   ProviderIdFormats.ISO       => CountryCode.Alpha2Code + Suffix,
                   _                           => $"{CountryCode.Alpha2Code}-{Suffix}"  // ISO_HYPHEN
               };

        #endregion

    }

}
