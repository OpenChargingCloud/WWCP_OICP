/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The unique identification of an OICP partner charging session.
    /// </summary>
    public struct PartnerSession_Id : IId,
                                      IEquatable <PartnerSession_Id>,
                                      IComparable<PartnerSession_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the partner charging session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new partner charging session identification.
        /// based on the given string.
        /// </summary>
        private PartnerSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner charging session identification.</param>
        public static PartnerSession_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a partner charging session identification must not be null or empty!");

            #endregion

            return new PartnerSession_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner charging session identification.</param>
        public static PartnerSession_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new PartnerSession_Id?()
                       : new PartnerSession_Id(Text);

        }

        #endregion

        #region (static) TryParse(JToken)

        /// <summary>
        /// Try to parse the given JSON token as a partner charging session identification.
        /// </summary>
        /// <param name="JToken">A JSON token representation of a partner charging session identification.</param>
        public static PartnerSession_Id? TryParse(JToken JToken)
            => TryParse(JToken?.Value<String>());

        #endregion

        #region (static) TryParse(Text, out PartnerSessionId)

        /// <summary>
        /// Try to parse the given string as a partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner charging session identification.</param>
        /// <param name="PartnerSessionId">The parsed partner charging session identification.</param>
        public static Boolean TryParse(String Text, out PartnerSession_Id PartnerSessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                PartnerSessionId = default(PartnerSession_Id);
                return false;
            }

            #endregion

            try
            {

                PartnerSessionId = new PartnerSession_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            PartnerSessionId = default(PartnerSession_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this partner charging session identification.
        /// </summary>
        public PartnerSession_Id Clone

            => new PartnerSession_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PartnerPartnerSessionId1, PartnerPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerPartnerSessionId1">A partner charging session identification.</param>
        /// <param name="PartnerPartnerSessionId2">Another partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PartnerSession_Id PartnerPartnerSessionId1, PartnerSession_Id PartnerPartnerSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PartnerPartnerSessionId1, PartnerPartnerSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PartnerPartnerSessionId1 == null) || ((Object) PartnerPartnerSessionId2 == null))
                return false;

            return PartnerPartnerSessionId1.Equals(PartnerPartnerSessionId2);

        }

        #endregion

        #region Operator != (PartnerPartnerSessionId1, PartnerPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerPartnerSessionId1">A partner charging session identification.</param>
        /// <param name="PartnerPartnerSessionId2">Another partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerSession_Id PartnerPartnerSessionId1, PartnerSession_Id PartnerPartnerSessionId2)
            => !(PartnerPartnerSessionId1 == PartnerPartnerSessionId2);

        #endregion

        #region Operator <  (PartnerPartnerSessionId1, PartnerPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerPartnerSessionId1">A partner charging session identification.</param>
        /// <param name="PartnerPartnerSessionId2">Another partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerSession_Id PartnerPartnerSessionId1, PartnerSession_Id PartnerPartnerSessionId2)
        {

            if ((Object) PartnerPartnerSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerPartnerSessionId1), "The given PartnerPartnerSessionId1 must not be null!");

            return PartnerPartnerSessionId1.CompareTo(PartnerPartnerSessionId2) < 0;

        }

        #endregion

        #region Operator <= (PartnerPartnerSessionId1, PartnerPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerPartnerSessionId1">A partner charging session identification.</param>
        /// <param name="PartnerPartnerSessionId2">Another partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerSession_Id PartnerPartnerSessionId1, PartnerSession_Id PartnerPartnerSessionId2)
            => !(PartnerPartnerSessionId1 > PartnerPartnerSessionId2);

        #endregion

        #region Operator >  (PartnerPartnerSessionId1, PartnerPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerPartnerSessionId1">A partner charging session identification.</param>
        /// <param name="PartnerPartnerSessionId2">Another partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerSession_Id PartnerPartnerSessionId1, PartnerSession_Id PartnerPartnerSessionId2)
        {

            if ((Object) PartnerPartnerSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerPartnerSessionId1), "The given PartnerPartnerSessionId1 must not be null!");

            return PartnerPartnerSessionId1.CompareTo(PartnerPartnerSessionId2) > 0;

        }

        #endregion

        #region Operator >= (PartnerPartnerSessionId1, PartnerPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerPartnerSessionId1">A partner charging session identification.</param>
        /// <param name="PartnerPartnerSessionId2">Another partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerSession_Id PartnerPartnerSessionId1, PartnerSession_Id PartnerPartnerSessionId2)
            => !(PartnerPartnerSessionId1 < PartnerPartnerSessionId2);

        #endregion

        #endregion

        #region IComparable<PartnerSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is PartnerSession_Id))
                throw new ArgumentException("The given object is not a partner charging session identification!",
                                            nameof(Object));

            return CompareTo((PartnerSession_Id) Object);

        }

        #endregion

        #region CompareTo(PartnerSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerSessionId">An object to compare with.</param>
        public Int32 CompareTo(PartnerSession_Id PartnerSessionId)
        {

            if ((Object) PartnerSessionId == null)
                throw new ArgumentNullException(nameof(PartnerSessionId),  "The given partner charging session identification must not be null!");

            // Compare the length of the PartnerSessionIds
            var _Result = this.Length.CompareTo(PartnerSessionId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, PartnerSessionId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<PartnerSessionId> Members

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

            if (!(Object is PartnerSession_Id))
                return false;

            return Equals((PartnerSession_Id) Object);

        }

        #endregion

        #region Equals(PartnerSessionId)

        /// <summary>
        /// Compares two PartnerSessionIds for equality.
        /// </summary>
        /// <param name="PartnerSessionId">A partner charging session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(PartnerSession_Id PartnerSessionId)
        {

            if ((Object) PartnerSessionId == null)
                return false;

            return InternalId.Equals(PartnerSessionId.InternalId);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
