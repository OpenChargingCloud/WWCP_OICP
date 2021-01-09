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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty

            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the ISO-639-1 or ISO-639-2/T language code.
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

            if (TryParse(Text, out LanguageCode meterId))
                return meterId;

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a ISO-639-1 or ISO-639-2/T language code must not be null or empty!");

            throw new ArgumentException("The given text representation of a ISO-639-1 or ISO-639-2/T language code is invalid!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        /// <param name="Text">A text representation of a ISO-639-1 or ISO-639-2/T language code.</param>
        public static LanguageCode? TryParse(String Text)
        {

            if (TryParse(Text, out LanguageCode meterId))
                return meterId;

            return default;

        }

        #endregion

        #region (static) TryParse(Text, out MeterId)

        /// <summary>
        /// Try to parse the given text as a ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        /// <param name="Text">A text representation of a ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId">The parsed ISO-639-1 or ISO-639-2/T language code.</param>
        public static Boolean TryParse(String Text, out LanguageCode MeterId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    MeterId = new LanguageCode(Text.Trim());
                    return true;
                }
                catch (Exception)
                { }
            }

            MeterId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this ISO-639-1 or ISO-639-2/T language code.
        /// </summary>
        public LanguageCode Clone

            => new LanguageCode(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        public static LanguageCode en
            => Parse("en");

        public static LanguageCode de
            => Parse("de");



        #region Operator overloading

        #region Operator == (MeterId1, MeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (LanguageCode MeterId1,
                                           LanguageCode MeterId2)

            => MeterId1.Equals(MeterId2);

        #endregion

        #region Operator != (MeterId1, MeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (LanguageCode MeterId1,
                                           LanguageCode MeterId2)

            => !(MeterId1 == MeterId2);

        #endregion

        #region Operator <  (MeterId1, MeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (LanguageCode MeterId1,
                                          LanguageCode MeterId2)

            => MeterId1.CompareTo(MeterId2) < 0;

        #endregion

        #region Operator <= (MeterId1, MeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (LanguageCode MeterId1,
                                           LanguageCode MeterId2)

            => !(MeterId1 > MeterId2);

        #endregion

        #region Operator >  (MeterId1, MeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (LanguageCode MeterId1,
                                          LanguageCode MeterId2)

            => MeterId1.CompareTo(MeterId2) > 0;

        #endregion

        #region Operator >= (MeterId1, MeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId1">A ISO-639-1 or ISO-639-2/T language code.</param>
        /// <param name="MeterId2">Another ISO-639-1 or ISO-639-2/T language code.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (LanguageCode MeterId1,
                                           LanguageCode MeterId2)

            => !(MeterId1 < MeterId2);

        #endregion

        #endregion

        #region IComparable<MeterId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is LanguageCode meterId
                   ? CompareTo(meterId)
                   : throw new ArgumentException("The given object is not a ISO-639-1 or ISO-639-2/T language code!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(MeterId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeterId">An object to compare with.</param>
        public Int32 CompareTo(LanguageCode MeterId)

            => String.Compare(InternalId,
                              MeterId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<MeterId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is LanguageCode meterId &&
                   Equals(meterId);

        #endregion

        #region Equals(MeterId)

        /// <summary>
        /// Compares two ISO-639-1 or ISO-639-2/T language codes for equality.
        /// </summary>
        /// <param name="MeterId">An ISO-639-1 or ISO-639-2/T language code to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(LanguageCode MeterId)

            => String.Equals(InternalId,
                             MeterId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

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
