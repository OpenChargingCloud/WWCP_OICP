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

    public enum OperatorIdFormats
    {
        DIN,
        DIN_STAR,
        DIN_HYPHEN,
        ISO,
        ISO_HYPHEN
    }


    /// <summary>
    /// The unique identification of an OICP charging station operator.
    /// </summary>
    public struct Operator_Id : IId,
                                IEquatable <Operator_Id>,
                                IComparable<Operator_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        /// <summary>
        /// The regular expression for parsing a charging station operator identification.
        /// </summary>
        public static readonly Regex  OperatorId_RegEx            = new Regex(@"^[A-Z0-9]{3}$",
                                                                              RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and a charging station operator identification.
        /// The ISO format onyl allows '-' as a separator!
        /// </summary>
        public static readonly Regex  CountryAndOperatorId_RegEx  = new Regex(@"^([A-Z]{2})([\*|\-]?)([A-Z0-9]{3})$",
                                                                              RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The format of the charging station operator identification.
        /// </summary>
        public OperatorIdFormats  Format        { get; }

        /// <summary>
        /// Returns the original representation of the charging station operator identification.
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

                    case OperatorIdFormats.DIN:
                        return (UInt64) (1 + CountryCode.TelefonCode.ToString().Length     + InternalId.Length);

                    case OperatorIdFormats.DIN_STAR:
                    case OperatorIdFormats.DIN_HYPHEN:
                        return (UInt64) (1 + CountryCode.TelefonCode.ToString().Length + 1 + InternalId.Length);


                    case OperatorIdFormats.ISO:
                        return (UInt64)     (CountryCode.Alpha2Code.Length                 + InternalId.Length);

                    default:
                        return (UInt64)     (CountryCode.Alpha2Code.Length             + 1 + InternalId.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="OperatorId">The charging station operator identification.</param>
        /// <param name="Format">The id format '-' (ISO) or '*|-' DIN to use.</param>
        private Operator_Id(Country            CountryCode,
                            String             OperatorId,
                            OperatorIdFormats  Format = OperatorIdFormats.ISO_HYPHEN)
        {

            this.CountryCode  = CountryCode;
            this.Format       = Format;
            this.InternalId   = OperatorId;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static Operator_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station operator identification must not be null or empty!");

            #endregion

            var MatchCollection = CountryAndOperatorId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging station operator identification: '" + Text + "'!", nameof(Text));

            Country _CountryCode;

            if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
            {

                OperatorIdFormats Separator = OperatorIdFormats.ISO_HYPHEN;

                switch (MatchCollection[0].Groups[2].Value)
                {

                    case ""  : Separator = OperatorIdFormats.DIN|OperatorIdFormats.ISO; break;
                    case "-" : Separator = OperatorIdFormats.DIN_HYPHEN|OperatorIdFormats.ISO_HYPHEN; break;
                    case "*" : Separator = OperatorIdFormats.DIN_STAR; break;

                    default: throw new ArgumentException("Illegal charging station operator identification!", nameof(Text));

                }

                return new Operator_Id(_CountryCode,
                                                MatchCollection[0].Groups[3].Value,
                                                Separator);
            }

            throw new ArgumentException("Unknown country code in the given text representation of a charging station operator identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Text, IdFormat = OperatorIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as a charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Text">An charging station operator identification as a string.</param>
        /// <param name="IdFormat">The optional format of the provider identification.</param>
        public static Operator_Id Parse(Country            CountryCode,
                                        String             Text,
                                        OperatorIdFormats  IdFormat = OperatorIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),         "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            if (!OperatorId_RegEx.IsMatch(Text))
                throw new ArgumentException("Illegal charging station operator identification '" + CountryCode + "' / '" + Text + "'!",
                                            nameof(Text));

            return new Operator_Id(CountryCode,
                                   Text,
                                   IdFormat);

        }

        #endregion

        #region TryParse(Text, out OperatorId)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        /// <param name="OperatorId">The parsed charging station operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Operator_Id  OperatorId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                OperatorId = default(Operator_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = CountryAndOperatorId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    OperatorId = default(Operator_Id);
                    return false;
                }

                Country _CountryCode;

                if (Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out _CountryCode))
                {

                    var Separator = OperatorIdFormats.ISO_HYPHEN;

                    switch (MatchCollection[0].Groups[2].Value)
                    {

                        case ""  : Separator = OperatorIdFormats.DIN|OperatorIdFormats.ISO; break;
                        case "-" : Separator = OperatorIdFormats.ISO_HYPHEN;                break;
                        case "*" : Separator = OperatorIdFormats.DIN_STAR;                  break;

                        default: throw new ArgumentException("Illegal charging station operator identification!", nameof(Text));

                    }

                    OperatorId = new Operator_Id(_CountryCode,
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

            OperatorId = default(Operator_Id);
            return false;

        }

        #endregion

        #region TryParse(CountryCode, Text, out OperatorId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Text">An Charging Station Operator identification as a string.</param>
        /// <param name="OperatorId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(Country                   CountryCode,
                                       String                    Text,
                                       out Operator_Id  OperatorId)
        {

            #region Initial checks

            if (CountryCode == null || Text.IsNullOrEmpty())
            {
                OperatorId = default(Operator_Id);
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = OperatorId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                {
                    OperatorId = default(Operator_Id);
                    return false;
                }

                OperatorId = new Operator_Id(CountryCode,
                                             _MatchCollection[0].Value,
                                             OperatorIdFormats.DIN | OperatorIdFormats.ISO);

                return true;

            }

            catch (Exception)
            {
                OperatorId = default(Operator_Id);
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station operator identification.
        /// </summary>
        public Operator_Id Clone

            => new Operator_Id(CountryCode,
                               new String(InternalId.ToCharArray()),
                               Format);

        #endregion


        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public Operator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new Operator_Id(CountryCode,
                               InternalId,
                               NewFormat);

        #endregion


        #region ToFormat(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToFormat(OperatorIdFormats Format)
        {

            switch (Format)
            {

                case OperatorIdFormats.DIN:
                    return String.Concat(CountryCode.Alpha2Code,
                                         InternalId);

                case OperatorIdFormats.DIN_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         InternalId);

                case OperatorIdFormats.DIN_HYPHEN:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         InternalId);


                case OperatorIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         InternalId);

                default:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "-",
                                         InternalId);

            }

        }

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
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(OperatorId1, OperatorId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) OperatorId1 == null) || ((Object) OperatorId2 == null))
                return false;

            return OperatorId1.Equals(OperatorId2);

        }

        #endregion

        #region Operator != (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {
            return !(OperatorId1 == OperatorId2);
        }

        #endregion

        #region Operator <  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {

            if ((Object) OperatorId1 == null)
                throw new ArgumentNullException("The given OperatorId1 must not be null!");

            return OperatorId1.CompareTo(OperatorId2) < 0;

        }

        #endregion

        #region Operator <= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {
            return !(OperatorId1 > OperatorId2);
        }

        #endregion

        #region Operator >  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {

            if ((Object) OperatorId1 == null)
                throw new ArgumentNullException("The given OperatorId1 must not be null!");

            return OperatorId1.CompareTo(OperatorId2) > 0;

        }

        #endregion

        #region Operator >= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {
            return !(OperatorId1 < OperatorId2);
        }

        #endregion

        #endregion

        #region IComparable<OperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Operator_Id))
                throw new ArgumentException("The given object is not a charging station operator identification!", nameof(Object));

            return CompareTo((Operator_Id) Object);

        }

        #endregion

        #region CompareTo(OperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId">An object to compare with.</param>
        public Int32 CompareTo(Operator_Id OperatorId)
        {

            if ((Object) OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given charging station operator identification must not be null!");

            // Compare the length of the OperatorIds
            var _Result = this.Length.CompareTo(OperatorId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(OperatorId.CountryCode);

            // If equal: Compare provider ids
            if (_Result == 0)
                _Result = String.Compare(InternalId, OperatorId.InternalId, StringComparison.Ordinal);

            return _Result;

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
        {

            if (Object == null)
                return false;

            if (!(Object is Operator_Id))
                return false;

            return this.Equals((Operator_Id) Object);

        }

        #endregion

        #region Equals(OperatorId)

        /// <summary>
        /// Compares two OperatorIds for equality.
        /// </summary>
        /// <param name="OperatorId">A OperatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Operator_Id OperatorId)
        {

            if ((Object) OperatorId == null)
                return false;

            return CountryCode.Equals(OperatorId.CountryCode) &&
                   InternalId. Equals(OperatorId.InternalId);

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
               InternalId.            GetHashCode();


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
