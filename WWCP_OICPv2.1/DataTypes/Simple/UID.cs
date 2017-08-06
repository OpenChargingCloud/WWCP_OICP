/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

    /// <summary>
    /// The unique identification of an OICP user.
    /// </summary>
    public struct UID : IId,
                        IEquatable <UID>,
                        IComparable<UID>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        /// <summary>
        /// The regular expression for parsing an user identification.
        /// </summary>
        public static readonly Regex UID_RegEx  = new Regex("^([A-F0-9]{8})$ | ^([A-F0-9]{14})$ | ^([A-F0-9]{20})$",
                                                            RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The length of the RFID card identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RFID card identification.
        /// based on the given string.
        /// </summary>
        /// <param name="Text">The value of the RFID identification.</param>
        private UID(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a RFID card identification.
        /// </summary>
        /// <param name="Text">A text representation of a RFID card identification.</param>
        public static UID Parse(String Text)
{

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a RFID card identification must not be null or empty!");

            #endregion

            var MatchCollection = UID_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of an user identification: '" + Text + "'!", nameof(Text));

            return new UID(Text);

        }

        #endregion

        #region TryParse(Text, out UID)

        /// <summary>
        /// Parse the given string as a RFID card identification.
        /// </summary>
        /// <param name="Text">A text representation of a RFID card identification.</param>
        /// <param name="UID">The parsed RFID card identification.</param>
        public static Boolean TryParse(String Text, out UID UID)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                UID = default(UID);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = UID_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    UID = default(UID);
                    return false;
                }

                UID = new UID(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            UID = default(UID);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this RFID card identification.
        /// </summary>
        public UID Clone

            => new UID(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card identification.</param>
        /// <param name="UID2">Another RFID card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (UID UID1, UID UID2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(UID1, UID2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UID1 == null) || ((Object) UID2 == null))
                return false;

            return UID1.Equals(UID2);

        }

        #endregion

        #region Operator != (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card identification.</param>
        /// <param name="UID2">Another RFID card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (UID UID1, UID UID2)
            => !(UID1 == UID2);

        #endregion

        #region Operator <  (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card identification.</param>
        /// <param name="UID2">Another RFID card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (UID UID1, UID UID2)
        {

            if ((Object) UID1 == null)
                throw new ArgumentNullException(nameof(UID1), "The given UID1 must not be null!");

            return UID1.CompareTo(UID2) < 0;

        }

        #endregion

        #region Operator <= (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card identification.</param>
        /// <param name="UID2">Another RFID card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (UID UID1, UID UID2)
            => !(UID1 > UID2);

        #endregion

        #region Operator >  (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card identification.</param>
        /// <param name="UID2">Another RFID card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (UID UID1, UID UID2)
        {

            if ((Object) UID1 == null)
                throw new ArgumentNullException(nameof(UID1), "The given UID1 must not be null!");

            return UID1.CompareTo(UID2) > 0;

        }

        #endregion

        #region Operator >= (UID1, UID2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID1">A RFID card identification.</param>
        /// <param name="UID2">Another RFID card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (UID UID1, UID UID2)
            => !(UID1 < UID2);

        #endregion

        #endregion

        #region IComparable<UID> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is UID))
                throw new ArgumentException("The given object is not a RFID card identification!",
                                            nameof(Object));

            return CompareTo((UID) Object);

        }

        #endregion

        #region CompareTo(UID)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UID">An object to compare with.</param>
        public Int32 CompareTo(UID UID)
        {

            if ((Object) UID == null)
                throw new ArgumentNullException(nameof(UID),  "The given RFID card identification must not be null!");

            // Compare the length of the UIDs
            var _Result = this.Length.CompareTo(UID.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, UID.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<UID> Members

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

            if (!(Object is UID))
                return false;

            return Equals((UID) Object);

        }

        #endregion

        #region Equals(UID)

        /// <summary>
        /// Compares two UIDs for equality.
        /// </summary>
        /// <param name="UID">A RFID card identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(UID UID)
        {

            if ((Object) UID == null)
                return false;

            return InternalId.Equals(UID.InternalId);

        }

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
