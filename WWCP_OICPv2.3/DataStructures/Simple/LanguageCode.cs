/*
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for language codes.
    /// </summary>
    public static class LanguageCodeExtensions
    {

        /// <summary>
        /// Indicates whether this language code is null or empty.
        /// </summary>
        /// <param name="LanguageCode">A language code.</param>
        public static Boolean IsNullOrEmpty(this LanguageCode? LanguageCode)
            => !LanguageCode.HasValue || LanguageCode.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this language code is null or empty.
        /// </summary>
        /// <param name="LanguageCode">A language code.</param>
        public static Boolean IsNotNullOrEmpty(this LanguageCode? LanguageCode)
            => LanguageCode.HasValue && LanguageCode.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an ISO-639-1 or ISO-639-2/T language code.
    /// </summary>
    public readonly struct LanguageCode : IId<LanguageCode>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        // ^[a-z]{2,3}(?:-[A-Z]{2,3}(?:-[a-zA-Z]{4})?)?(?:-x-[a-zA-Z0-9]{1,8})?$

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this language code is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this language code is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the language code.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        /// <param name="String">The string representation of the ISO-639-1 or ISO-639-2/T language code.</param>
        private LanguageCode(String String)
        {
            this.InternalId  = String;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        /// <param name="Text">A text representation of a ISO-639-1 or ISO-639-2/T language code.</param>
        public static LanguageCode Parse(String Text)
        {

            if (TryParse(Text, out var languageCode))
                return languageCode;

            throw new ArgumentException("The given text representation of a ISO-639-1 or ISO-639-2/T language code is invalid!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        /// <param name="Text">A text representation of a ISO-639-1 or ISO-639-2/T language code.</param>
        public static LanguageCode? TryParse(String Text)
        {

            if (TryParse(Text, out var languageCode))
                return languageCode;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out LanguageCode)

        /// <summary>
        /// Try to parse the given text as a ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        /// <param name="Text">A text representation of a ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode">The parsed ISO-639-1 or ISO-639-2/T language code.</param>
        public static Boolean TryParse(String Text, out LanguageCode LanguageCode)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    LanguageCode = new LanguageCode(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            LanguageCode = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        public LanguageCode Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        public static LanguageCode en
            => Parse("en");

        public static LanguageCode de
            => Parse("de");



        #region Operator overloading

        #region Operator == (LanguageCode1, LanguageCode2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LanguageCode1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (LanguageCode LanguageCode1,
                                           LanguageCode LanguageCode2)

            => LanguageCode1.Equals(LanguageCode2);

        #endregion

        #region Operator != (LanguageCode1, LanguageCode2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LanguageCode1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (LanguageCode LanguageCode1,
                                           LanguageCode LanguageCode2)

            => !(LanguageCode1 == LanguageCode2);

        #endregion

        #region Operator <  (LanguageCode1, LanguageCode2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LanguageCode1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (LanguageCode LanguageCode1,
                                          LanguageCode LanguageCode2)

            => LanguageCode1.CompareTo(LanguageCode2) < 0;

        #endregion

        #region Operator <= (LanguageCode1, LanguageCode2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LanguageCode1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (LanguageCode LanguageCode1,
                                           LanguageCode LanguageCode2)

            => !(LanguageCode1 > LanguageCode2);

        #endregion

        #region Operator >  (LanguageCode1, LanguageCode2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LanguageCode1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (LanguageCode LanguageCode1,
                                          LanguageCode LanguageCode2)

            => LanguageCode1.CompareTo(LanguageCode2) > 0;

        #endregion

        #region Operator >= (LanguageCode1, LanguageCode2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LanguageCode1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="LanguageCode2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (LanguageCode LanguageCode1,
                                           LanguageCode LanguageCode2)

            => !(LanguageCode1 < LanguageCode2);

        #endregion

        #endregion

        #region IComparable<LanguageCode> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two ISO-639-1 or ISO-639-2/T language codes.
        /// </summary>
        /// <param name="Object">An ISO-639-1 or ISO-639-2/T language code to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is LanguageCode languageCode
                   ? CompareTo(languageCode)
                   : throw new ArgumentException("The given object is not a ISO-639-1 or ISO-639-2/T language code!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(LanguageCode)

        /// <summary>
        /// Compares two ISO-639-1 or ISO-639-2/T language codes.
        /// </summary>
        /// <param name="LanguageCode">An ISO-639-1 or ISO-639-2/T language code to compare with.</param>
        public Int32 CompareTo(LanguageCode LanguageCode)

            => String.Compare(InternalId,
                              LanguageCode.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<LanguageCode> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two ISO-639-1 or ISO-639-2/T language codes for equality.
        /// </summary>
        /// <param name="Object">An ISO-639-1 or ISO-639-2/T language code to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is LanguageCode languageCode &&
                   Equals(languageCode);

        #endregion

        #region Equals(LanguageCode)

        /// <summary>
        /// Compares two ISO-639-1 or ISO-639-2/T language codes for equality.
        /// </summary>
        /// <param name="LanguageCode">An ISO-639-1 or ISO-639-2/T language code to compare with.</param>
        public Boolean Equals(LanguageCode LanguageCode)

            => String.Equals(InternalId,
                             LanguageCode.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

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
