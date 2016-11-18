/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public enum ProviderIdFormats
    {
        DIN,
        DIN_STAR,
        DIN_HYPHEN,
        ISO,
        ISO_HYPHEN
    }


    /// <summary>
    /// The unique identification of an OICP e-mobility provider.
    /// </summary>
    public struct Provider_Id : IId,
                                IEquatable <Provider_Id>,
                                IComparable<Provider_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an e-mobility provider identification.
        /// </summary>
        public static readonly Regex  ProviderId_RegEx            = new Regex(@"^[A-Z0-9]{3}$",
                                                                              RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an e-mobility provider identification.
        /// The ISO format onyl allows '-' as a separator!
        /// </summary>
        public static readonly Regex  CountryAndProviderId_RegEx  = new Regex(@"^([A-Z]{2})([\*|\-]?)([A-Z0-9]{3})$",
                                                                              RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The internal e-mobility provider identification.
        /// </summary>
        public String             ProviderId    { get; }

        /// <summary>
        /// The format of the e-mobility provider identification.
        /// </summary>
        public ProviderIdFormats  Format        { get; }

        /// <summary>
        /// Returns the original representation of the e-mobility provider identification.
        /// </summary>
        public String             OriginId
            => ToFormat(Format);

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case ProviderIdFormats.DIN:
                        return (UInt64) (1 + CountryCode.TelefonCode.ToString().Length     + ProviderId.Length);

                    case ProviderIdFormats.DIN_STAR:
                    case ProviderIdFormats.DIN_HYPHEN:
                        return (UInt64) (1 + CountryCode.TelefonCode.ToString().Length + 1 + ProviderId.Length);


                    case ProviderIdFormats.ISO:
                        return (UInt64)     (CountryCode.Alpha2Code.Length                 + ProviderId.Length);

                    default:
                        return (UInt64)     (CountryCode.Alpha2Code.Length             + 1 + ProviderId.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="ProviderId">The e-mobility provider identification.</param>
        /// <param name="Format">The id format '-' (ISO) or '*|-' DIN to use.</param>
        private Provider_Id(Country            CountryCode,
                                     String             ProviderId,
                                     ProviderIdFormats  Format = ProviderIdFormats.ISO_HYPHEN)
        {

            this.CountryCode  = CountryCode;
            this.Format       = Format;
            this.ProviderId   = ProviderId;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        public static Provider_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The given text representation of an e-mobility provider identification must not be null or empty!", nameof(Text));

            #endregion

            var MatchCollection = CountryAndProviderId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of an e-mobility provider identification: '" + Text + "'!", nameof(Text));

            Country _CountryCode;

            if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
            {

                ProviderIdFormats Separator = ProviderIdFormats.ISO_HYPHEN;

                switch (MatchCollection[0].Groups[2].Value)
                {

                    case ""  : Separator = ProviderIdFormats.DIN|ProviderIdFormats.ISO; break;
                    case "-" : Separator = ProviderIdFormats.DIN_HYPHEN|ProviderIdFormats.ISO_HYPHEN; break;
                    case "*" : Separator = ProviderIdFormats.DIN_STAR; break;

                    default: throw new ArgumentException("Illegal e-mobility provider identification!", nameof(Text));

                }

                return new Provider_Id(_CountryCode,
                                                MatchCollection[0].Groups[3].Value,
                                                Separator);
            }

            throw new ArgumentException("Unknown country code in the given text representation of an e-mobility provider identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Text, IdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as an e-mobility provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Text">An e-mobility provider identification as a string.</param>
        /// <param name="IdFormat">The optional format of the provider identification.</param>
        public static Provider_Id Parse(Country            CountryCode,
                                                 String             Text,
                                                 ProviderIdFormats  IdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode), "The given country must not be null!");

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given e-mobility provider identification suffix must not be null or empty!");

            #endregion

            if (!ProviderId_RegEx.IsMatch(Text))
                throw new ArgumentException("Illegal e-mobility provider identification '" + CountryCode + "' / '" + Text + "'!",
                                            nameof(Text));

            return new Provider_Id(CountryCode,
                                            Text,
                                            IdFormat);

        }

        #endregion

        #region TryParse(Text, out eMobilityProviderId)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId">The parsed e-mobility provider identification.</param>
        public static Boolean TryParse(String                    Text,
                                       out Provider_Id  eMobilityProviderId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                eMobilityProviderId = default(Provider_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = CountryAndProviderId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    eMobilityProviderId = default(Provider_Id);
                    return false;
                }

                Country _CountryCode;

                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
                {

                    var Separator = ProviderIdFormats.ISO_HYPHEN;

                    switch (MatchCollection[0].Groups[2].Value)
                    {

                        case ""  : Separator = ProviderIdFormats.DIN|ProviderIdFormats.ISO; break;
                        case "-" : Separator = ProviderIdFormats.ISO_HYPHEN;                break;
                        case "*" : Separator = ProviderIdFormats.DIN_STAR;                  break;

                        default: throw new ArgumentException("Illegal e-mobility provider identification!", nameof(Text));

                    }

                    eMobilityProviderId = new Provider_Id(_CountryCode,
                                                                   MatchCollection[0].Groups[3].Value,
                                                                   Separator);

                    return true;

                }

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            eMobilityProviderId = default(Provider_Id);
            return false;

        }

        #endregion

        #region TryParse(CountryCode, Text, out eMobilityProviderId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Text">An Charging Station Operator identification as a string.</param>
        /// <param name="eMobilityProviderId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(Country                   CountryCode,
                                       String                    Text,
                                       out Provider_Id  eMobilityProviderId)
        {

            #region Initial checks

            if (CountryCode == null || Text.IsNullOrEmpty())
            {
                eMobilityProviderId = default(Provider_Id);
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = ProviderId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                {
                    eMobilityProviderId = default(Provider_Id);
                    return false;
                }

                eMobilityProviderId = new Provider_Id(CountryCode,
                                             _MatchCollection[0].Value,
                                             ProviderIdFormats.DIN | ProviderIdFormats.ISO);

                return true;

            }

            catch (Exception)
            {
                eMobilityProviderId = default(Provider_Id);
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this e-mobility provider identification.
        /// </summary>
        public Provider_Id Clone

            => new Provider_Id(CountryCode,
                                        ProviderId,
                                        Format);

        #endregion


        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new e-mobility provider identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new e-mobility provider identification format.</param>
        public Provider_Id ChangeFormat(ProviderIdFormats NewFormat)

            => new Provider_Id(CountryCode,
                                        ProviderId,
                                        NewFormat);

        #endregion


        #region ToFormat(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToFormat(ProviderIdFormats Format)
        {

            switch (Format)
            {

                case ProviderIdFormats.DIN:
                    return String.Concat(CountryCode.Alpha2Code,
                                         ProviderId);

                case ProviderIdFormats.DIN_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         ProviderId);

                case ProviderIdFormats.DIN_HYPHEN:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         ProviderId);


                case ProviderIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         ProviderId);

                default:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         ProviderId);

            }

        }

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Provider_Id eMobilityProviderId1, Provider_Id eMobilityProviderId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(eMobilityProviderId1, eMobilityProviderId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMobilityProviderId1 == null) || ((Object) eMobilityProviderId2 == null))
                return false;

            return eMobilityProviderId1.Equals(eMobilityProviderId2);

        }

        #endregion

        #region Operator != (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Provider_Id eMobilityProviderId1, Provider_Id eMobilityProviderId2)
        {
            return !(eMobilityProviderId1 == eMobilityProviderId2);
        }

        #endregion

        #region Operator <  (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Provider_Id eMobilityProviderId1, Provider_Id eMobilityProviderId2)
        {

            if ((Object) eMobilityProviderId1 == null)
                throw new ArgumentNullException("The given eMobilityProviderId1 must not be null!");

            return eMobilityProviderId1.CompareTo(eMobilityProviderId2) < 0;

        }

        #endregion

        #region Operator <= (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Provider_Id eMobilityProviderId1, Provider_Id eMobilityProviderId2)
        {
            return !(eMobilityProviderId1 > eMobilityProviderId2);
        }

        #endregion

        #region Operator >  (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Provider_Id eMobilityProviderId1, Provider_Id eMobilityProviderId2)
        {

            if ((Object) eMobilityProviderId1 == null)
                throw new ArgumentNullException("The given eMobilityProviderId1 must not be null!");

            return eMobilityProviderId1.CompareTo(eMobilityProviderId2) > 0;

        }

        #endregion

        #region Operator >= (eMobilityProviderId1, eMobilityProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId1">An e-mobility provider identification.</param>
        /// <param name="eMobilityProviderId2">Another e-mobility provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Provider_Id eMobilityProviderId1, Provider_Id eMobilityProviderId2)
        {
            return !(eMobilityProviderId1 < eMobilityProviderId2);
        }

        #endregion

        #endregion

        #region IComparable<eMobilityProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Provider_Id))
                throw new ArgumentException("The given object is not an e-mobility provider identification!", nameof(Object));

            return CompareTo((Provider_Id) Object);

        }

        #endregion

        #region CompareTo(eMobilityProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProviderId">An object to compare with.</param>
        public Int32 CompareTo(Provider_Id eMobilityProviderId)
        {

            if ((Object) eMobilityProviderId == null)
                throw new ArgumentNullException(nameof(eMobilityProviderId), "The given e-mobility provider identification must not be null!");

            // Compare the length of the eMobilityProviderIds
            var _Result = this.Length.CompareTo(eMobilityProviderId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(eMobilityProviderId.CountryCode);

            // If equal: Compare provider ids
            if (_Result == 0)
                _Result = String.Compare(ProviderId, eMobilityProviderId.ProviderId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityProviderId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is Provider_Id))
                return false;

            return this.Equals((Provider_Id) Object);

        }

        #endregion

        #region Equals(eMobilityProviderId)

        /// <summary>
        /// Compares two eMobilityProviderIds for equality.
        /// </summary>
        /// <param name="eMobilityProviderId">A eMobilityProviderId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Provider_Id eMobilityProviderId)
        {

            if ((Object) eMobilityProviderId == null)
                return false;

            return CountryCode.Equals(eMobilityProviderId.CountryCode) &&
                   ProviderId. Equals(eMobilityProviderId.ProviderId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => CountryCode.Alpha2Code.GetHashCode() ^
               ProviderId.            GetHashCode();


        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => OriginId;

        #endregion


    }

}
