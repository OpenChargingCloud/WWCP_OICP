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

    /// <summary>
    /// The unique identification of an OICP charging session.
    /// </summary>
    public struct Session_Id : IId,
                               IEquatable<Session_Id>,
                               IComparable<Session_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;


        /// <summary>
        /// The regular expression for parsing a charging session identification.
        /// </summary>
        public static readonly Regex SessionId_RegEx  = new Regex(@"^[A-Za-z0-9]{8}(-[A-Za-z0-9]{4}){3}-[A-Za-z0-9]{12}$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        #endregion

        #region Properties

        /// <summary>
        /// The length of the charging session identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging session identification.
        /// based on the given string.
        /// </summary>
        private Session_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        public static Session_Id New
            => Session_Id.Parse(Guid.NewGuid().ToString());

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static Session_Id Parse(String Text)
{

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging session identification must not be null or empty!");

            #endregion

            var MatchCollection = SessionId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging session identification: '" + Text + "'!", nameof(Text));

            return new Session_Id(Text);

        }

        #endregion

        #region TryParse(Text, out SessionId)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        /// <param name="SessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(String Text, out Session_Id SessionId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                SessionId = default(Session_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = SessionId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                {
                    SessionId = default(Session_Id);
                    return false;
                }

                SessionId = new Session_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            SessionId = default(Session_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging session identification.
        /// </summary>
        public Session_Id Clone

            => new Session_Id(
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
        public static Boolean operator == (Session_Id SessionId1, Session_Id SessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SessionId1, SessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SessionId1 == null) || ((Object) SessionId2 == null))
                return false;

            return SessionId1.Equals(SessionId2);

        }

        #endregion

        #region Operator != (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Session_Id SessionId1, Session_Id SessionId2)
            => !(SessionId1 == SessionId2);

        #endregion

        #region Operator <  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Session_Id SessionId1, Session_Id SessionId2)
        {

            if ((Object) SessionId1 == null)
                throw new ArgumentNullException(nameof(SessionId1), "The given SessionId1 must not be null!");

            return SessionId1.CompareTo(SessionId2) < 0;

        }

        #endregion

        #region Operator <= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Session_Id SessionId1, Session_Id SessionId2)
            => !(SessionId1 > SessionId2);

        #endregion

        #region Operator >  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Session_Id SessionId1, Session_Id SessionId2)
        {

            if ((Object) SessionId1 == null)
                throw new ArgumentNullException(nameof(SessionId1), "The given SessionId1 must not be null!");

            return SessionId1.CompareTo(SessionId2) > 0;

        }

        #endregion

        #region Operator >= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Session_Id SessionId1, Session_Id SessionId2)
            => !(SessionId1 < SessionId2);

        #endregion

        #endregion

        #region IComparable<SessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Session_Id))
                throw new ArgumentException("The given object is not a charging session identification!",
                                            nameof(Object));

            return CompareTo((Session_Id) Object);

        }

        #endregion

        #region CompareTo(SessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId">An object to compare with.</param>
        public Int32 CompareTo(Session_Id SessionId)
        {

            if ((Object) SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            // Compare the length of the SessionIds
            var _Result = this.Length.CompareTo(SessionId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, SessionId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

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
        {

            if (Object == null)
                return false;

            if (!(Object is Session_Id))
                return false;

            return Equals((Session_Id) Object);

        }

        #endregion

        #region Equals(SessionId)

        /// <summary>
        /// Compares two SessionIds for equality.
        /// </summary>
        /// <param name="SessionId">A charging session identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Session_Id SessionId)
        {

            if ((Object) SessionId == null)
                return false;

            return InternalId.Equals(SessionId.InternalId);

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
