﻿/*
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
    public struct EMPPartnerSession_Id : IId,
                                         IEquatable <EMPPartnerSession_Id>,
                                         IComparable<EMPPartnerSession_Id>

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
        private EMPPartnerSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an EMP partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of an EMP partner charging session identification.</param>
        public static EMPPartnerSession_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a partner charging session identification must not be null or empty!");

            #endregion

            return new EMPPartnerSession_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an EMP partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of an EMP partner charging session identification.</param>
        public static EMPPartnerSession_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new EMPPartnerSession_Id?()
                       : new EMPPartnerSession_Id(Text);

        }

        #endregion

        #region (static) TryParse(JToken)

        /// <summary>
        /// Try to parse the given JSON token as an EMP partner charging session identification.
        /// </summary>
        /// <param name="JToken">A JSON token representation of an EMP partner charging session identification.</param>
        public static EMPPartnerSession_Id? TryParse(JToken JToken)
            => TryParse(JToken?.Value<String>());

        #endregion

        #region (static) TryParse(Text, out EMPPartnerSessionId)

        /// <summary>
        /// Try to parse the given string as an EMP partner charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of an EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">The parsed EMP partner charging session identification.</param>
        public static Boolean TryParse(String Text, out EMPPartnerSession_Id EMPPartnerSessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                EMPPartnerSessionId = default(EMPPartnerSession_Id);
                return false;
            }

            #endregion

            try
            {

                EMPPartnerSessionId = new EMPPartnerSession_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            EMPPartnerSessionId = default(EMPPartnerSession_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this partner charging session identification.
        /// </summary>
        public EMPPartnerSession_Id Clone

            => new EMPPartnerSession_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerEMPPartnerSessionId1">A EMP partner charging session identification.</param>
        /// <param name="PartnerEMPPartnerSessionId2">Another EMP partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EMPPartnerSession_Id PartnerEMPPartnerSessionId1, EMPPartnerSession_Id PartnerEMPPartnerSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PartnerEMPPartnerSessionId1 == null) || ((Object) PartnerEMPPartnerSessionId2 == null))
                return false;

            return PartnerEMPPartnerSessionId1.Equals(PartnerEMPPartnerSessionId2);

        }

        #endregion

        #region Operator != (PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerEMPPartnerSessionId1">A EMP partner charging session identification.</param>
        /// <param name="PartnerEMPPartnerSessionId2">Another EMP partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EMPPartnerSession_Id PartnerEMPPartnerSessionId1, EMPPartnerSession_Id PartnerEMPPartnerSessionId2)
            => !(PartnerEMPPartnerSessionId1 == PartnerEMPPartnerSessionId2);

        #endregion

        #region Operator <  (PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerEMPPartnerSessionId1">A EMP partner charging session identification.</param>
        /// <param name="PartnerEMPPartnerSessionId2">Another EMP partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EMPPartnerSession_Id PartnerEMPPartnerSessionId1, EMPPartnerSession_Id PartnerEMPPartnerSessionId2)
        {

            if ((Object) PartnerEMPPartnerSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerEMPPartnerSessionId1), "The given PartnerEMPPartnerSessionId1 must not be null!");

            return PartnerEMPPartnerSessionId1.CompareTo(PartnerEMPPartnerSessionId2) < 0;

        }

        #endregion

        #region Operator <= (PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerEMPPartnerSessionId1">A EMP partner charging session identification.</param>
        /// <param name="PartnerEMPPartnerSessionId2">Another EMP partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EMPPartnerSession_Id PartnerEMPPartnerSessionId1, EMPPartnerSession_Id PartnerEMPPartnerSessionId2)
            => !(PartnerEMPPartnerSessionId1 > PartnerEMPPartnerSessionId2);

        #endregion

        #region Operator >  (PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerEMPPartnerSessionId1">A EMP partner charging session identification.</param>
        /// <param name="PartnerEMPPartnerSessionId2">Another EMP partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EMPPartnerSession_Id PartnerEMPPartnerSessionId1, EMPPartnerSession_Id PartnerEMPPartnerSessionId2)
        {

            if ((Object) PartnerEMPPartnerSessionId1 == null)
                throw new ArgumentNullException(nameof(PartnerEMPPartnerSessionId1), "The given PartnerEMPPartnerSessionId1 must not be null!");

            return PartnerEMPPartnerSessionId1.CompareTo(PartnerEMPPartnerSessionId2) > 0;

        }

        #endregion

        #region Operator >= (PartnerEMPPartnerSessionId1, PartnerEMPPartnerSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerEMPPartnerSessionId1">A EMP partner charging session identification.</param>
        /// <param name="PartnerEMPPartnerSessionId2">Another EMP partner charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EMPPartnerSession_Id PartnerEMPPartnerSessionId1, EMPPartnerSession_Id PartnerEMPPartnerSessionId2)
            => !(PartnerEMPPartnerSessionId1 < PartnerEMPPartnerSessionId2);

        #endregion

        #endregion

        #region IComparable<EMPPartnerSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EMPPartnerSession_Id))
                throw new ArgumentException("The given object is not a partner charging session identification!",
                                            nameof(Object));

            return CompareTo((EMPPartnerSession_Id) Object);

        }

        #endregion

        #region CompareTo(EMPPartnerSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPPartnerSessionId">An object to compare with.</param>
        public Int32 CompareTo(EMPPartnerSession_Id EMPPartnerSessionId)
        {

            if ((Object) EMPPartnerSessionId == null)
                throw new ArgumentNullException(nameof(EMPPartnerSessionId),  "The given partner charging session identification must not be null!");

            // Compare the length of the EMPPartnerSessionIds
            var _Result = this.Length.CompareTo(EMPPartnerSessionId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, EMPPartnerSessionId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EMPPartnerSessionId> Members

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

            if (!(Object is EMPPartnerSession_Id))
                return false;

            return Equals((EMPPartnerSession_Id) Object);

        }

        #endregion

        #region Equals(EMPPartnerSessionId)

        /// <summary>
        /// Compares two EMPPartnerSessionIds for equality.
        /// </summary>
        /// <param name="EMPPartnerSessionId">A EMP partner charging session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMPPartnerSession_Id EMPPartnerSessionId)
        {

            if ((Object) EMPPartnerSessionId == null)
                return false;

            return InternalId.Equals(EMPPartnerSessionId.InternalId);

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