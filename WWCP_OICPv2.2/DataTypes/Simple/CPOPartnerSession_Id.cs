﻿/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The unique identification of a charging session.
    /// </summary>
    public readonly struct CPOPartnerSession_Id : IId<CPOPartnerSession_Id>
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
        /// The length of the charging session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging session identification.
        /// based on the given string.
        /// </summary>
        private CPOPartnerSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom

        /// <summary>
        /// Create a new random charging session identification.
        /// </summary>
        public static CPOPartnerSession_Id NewRandom
            => Parse(Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging session identification.</param>
        public static CPOPartnerSession_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text-representation of a CPO charging session identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out CPOPartnerSession_Id sessionId))
                return sessionId;

            throw new ArgumentException("Illegal text-representation of a CPO charging session identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging session identification.</param>
        public static CPOPartnerSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out CPOPartnerSession_Id sessionId))
                return sessionId;

            return new CPOPartnerSession_Id?();

        }

        #endregion

        #region (static) TryParse(JToken)

        /// <summary>
        /// Try to parse the given JSON token as a charging session identification.
        /// </summary>
        /// <param name="JToken">A JSON token representation of a charging session identification.</param>
        public static CPOPartnerSession_Id? TryParse(JToken JToken)
            => TryParse(JToken?.Value<String>());

        #endregion

        #region (static) TryParse(Text, out SessionId)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging session identification.</param>
        /// <param name="SessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(String Text, out CPOPartnerSession_Id SessionId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                SessionId = default;
                return false;
            }

            #endregion

            try
            {
                SessionId = new CPOPartnerSession_Id(Text);
                return true;
            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            SessionId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging session identification.
        /// </summary>
        public CPOPartnerSession_Id Clone

            => new CPOPartnerSession_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CPOPartnerSession_Id SessionId1, CPOPartnerSession_Id SessionId2)
            => SessionId1.Equals(SessionId2);

        #endregion

        #region Operator != (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CPOPartnerSession_Id SessionId1, CPOPartnerSession_Id SessionId2)
            => !SessionId1.Equals(SessionId2);

        #endregion

        #region Operator <  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CPOPartnerSession_Id SessionId1, CPOPartnerSession_Id SessionId2)
            => SessionId1.CompareTo(SessionId2) < 0;

        #endregion

        #region Operator <= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CPOPartnerSession_Id SessionId1, CPOPartnerSession_Id SessionId2)
            => SessionId1.CompareTo(SessionId2) <= 0;

        #endregion

        #region Operator >  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CPOPartnerSession_Id SessionId1, CPOPartnerSession_Id SessionId2)
            => SessionId1.CompareTo(SessionId2) > 0;

        #endregion

        #region Operator >= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CPOPartnerSession_Id SessionId1, CPOPartnerSession_Id SessionId2)
            => SessionId1.CompareTo(SessionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is CPOPartnerSession_Id SessionId
                   ? CompareTo(SessionId)
                   : throw new ArgumentException("The given object is not a charging session identification!");

        #endregion

        #region CompareTo(SessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId">An object to compare with.</param>
        public Int32 CompareTo(CPOPartnerSession_Id SessionId)

            => String.Compare(InternalId,
                              SessionId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SessionId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is CPOPartnerSession_Id CPOPartnerSessionId
                   ? Equals(CPOPartnerSessionId)
                   : false;

        #endregion

        #region Equals(SessionId)

        /// <summary>
        /// Compares two SessionIds for equality.
        /// </summary>
        /// <param name="SessionId">A charging session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CPOPartnerSession_Id SessionId)

            => String.Equals(InternalId,
                             SessionId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

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
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
