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
    /// The unique identification of an OICP EMP partner charging session.
    /// </summary>
    public struct CPOPartnerSession_Id : IId,
                                         IEquatable <CPOPartnerSession_Id>,
                                         IComparable<CPOPartnerSession_Id>

    {

        #region Data

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
        /// The length of the EMP partner charging session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP partner charging session identification.
        /// based on the given string.
        /// </summary>
        private CPOPartnerSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a CPO partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a CPO partner charging session identification.</param>
        public static CPOPartnerSession_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a partner charging session identification must not be null or empty!");

            #endregion

            return new CPOPartnerSession_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a CPO partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a CPO partner charging session identification.</param>
        public static CPOPartnerSession_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new CPOPartnerSession_Id?()
                       : new CPOPartnerSession_Id(Text);

        }

        #endregion

        #region (static) TryParse(JToken)

        /// <summary>
        /// Try to parse the given JSON token as a CPO partner charging session identification.
        /// </summary>
        /// <param name="JToken">A JSON token representation of a CPO partner charging session identification.</param>
        public static CPOPartnerSession_Id? TryParse(JToken JToken)
            => TryParse(JToken?.Value<String>());

        #endregion

        #region (static) TryParse(Text, out CPOPartnerSessionId)

        /// <summary>
        /// Try to parse the given string as a CPO partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a CPO partner charging session identification.</param>
        /// <param name="CPOPartnerSessionId">The parsed EMP partner charging session identification.</param>
        public static Boolean TryParse(String Text, out CPOPartnerSession_Id CPOPartnerSessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                CPOPartnerSessionId = default(CPOPartnerSession_Id);
                return false;
            }

            #endregion

            try
            {

                CPOPartnerSessionId = new CPOPartnerSession_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            CPOPartnerSessionId = default(CPOPartnerSession_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this partner charging session identification.
        /// </summary>
        public CPOPartnerSession_Id Clone

            => new CPOPartnerSession_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerCPOPartnerSessionId1">A CPO partner charging session identification.</param>
        /// <param name="PartnerCPOPartnerSessionId2">Another CPO partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CPOPartnerSession_Id PartnerCPOPartnerSessionId1, CPOPartnerSession_Id PartnerCPOPartnerSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PartnerCPOPartnerSessionId1 == null) || ((Object) PartnerCPOPartnerSessionId2 == null))
                return false;

            return PartnerCPOPartnerSessionId1.Equals(PartnerCPOPartnerSessionId2);

        }

        #endregion

        #region Operator != (PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerCPOPartnerSessionId1">A CPO partner charging session identification.</param>
        /// <param name="PartnerCPOPartnerSessionId2">Another CPO partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CPOPartnerSession_Id PartnerCPOPartnerSessionId1, CPOPartnerSession_Id PartnerCPOPartnerSessionId2)
            => !(PartnerCPOPartnerSessionId1 == PartnerCPOPartnerSessionId2);

        #endregion

        #region Operator <  (PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerCPOPartnerSessionId1">A CPO partner charging session identification.</param>
        /// <param name="PartnerCPOPartnerSessionId2">Another CPO partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CPOPartnerSession_Id PartnerCPOPartnerSessionId1, CPOPartnerSession_Id PartnerCPOPartnerSessionId2)
        {

            if ((Object) PartnerCPOPartnerSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerCPOPartnerSessionId1), "The given PartnerCPOPartnerSessionId1 must not be null!");

            return PartnerCPOPartnerSessionId1.CompareTo(PartnerCPOPartnerSessionId2) < 0;

        }

        #endregion

        #region Operator <= (PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerCPOPartnerSessionId1">A CPO partner charging session identification.</param>
        /// <param name="PartnerCPOPartnerSessionId2">Another CPO partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CPOPartnerSession_Id PartnerCPOPartnerSessionId1, CPOPartnerSession_Id PartnerCPOPartnerSessionId2)
            => !(PartnerCPOPartnerSessionId1 > PartnerCPOPartnerSessionId2);

        #endregion

        #region Operator >  (PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerCPOPartnerSessionId1">A CPO partner charging session identification.</param>
        /// <param name="PartnerCPOPartnerSessionId2">Another CPO partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CPOPartnerSession_Id PartnerCPOPartnerSessionId1, CPOPartnerSession_Id PartnerCPOPartnerSessionId2)
        {

            if ((Object) PartnerCPOPartnerSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerCPOPartnerSessionId1), "The given PartnerCPOPartnerSessionId1 must not be null!");

            return PartnerCPOPartnerSessionId1.CompareTo(PartnerCPOPartnerSessionId2) > 0;

        }

        #endregion

        #region Operator >= (PartnerCPOPartnerSessionId1, PartnerCPOPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerCPOPartnerSessionId1">A CPO partner charging session identification.</param>
        /// <param name="PartnerCPOPartnerSessionId2">Another CPO partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CPOPartnerSession_Id PartnerCPOPartnerSessionId1, CPOPartnerSession_Id PartnerCPOPartnerSessionId2)
            => !(PartnerCPOPartnerSessionId1 < PartnerCPOPartnerSessionId2);

        #endregion

        #endregion

        #region IComparable<CPOPartnerSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is CPOPartnerSession_Id))
                throw new ArgumentException("The given object is not a partner charging session identification!",
                                            nameof(Object));

            return CompareTo((CPOPartnerSession_Id) Object);

        }

        #endregion

        #region CompareTo(CPOPartnerSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CPOPartnerSessionId">An object to compare with.</param>
        public Int32 CompareTo(CPOPartnerSession_Id CPOPartnerSessionId)
        {

            if ((Object) CPOPartnerSessionId == null)
                throw new ArgumentNullException(nameof(CPOPartnerSessionId),  "The given partner charging session identification must not be null!");

            // Compare the length of the CPOPartnerSessionIds
            var _Result = this.Length.CompareTo(CPOPartnerSessionId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, CPOPartnerSessionId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<CPOPartnerSessionId> Members

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

            if (!(Object is CPOPartnerSession_Id))
                return false;

            return Equals((CPOPartnerSession_Id) Object);

        }

        #endregion

        #region Equals(CPOPartnerSessionId)

        /// <summary>
        /// Compares two CPOPartnerSessionIds for equality.
        /// </summary>
        /// <param name="CPOPartnerSessionId">A CPO partner charging session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CPOPartnerSession_Id CPOPartnerSessionId)
        {

            if ((Object) CPOPartnerSessionId == null)
                return false;

            return InternalId.Equals(CPOPartnerSessionId.InternalId);

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
