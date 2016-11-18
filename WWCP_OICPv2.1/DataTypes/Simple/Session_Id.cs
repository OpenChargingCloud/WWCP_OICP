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

        #endregion

        #region Properties

        #region New

        /// <summary>
        /// Returns a new charging session identification.
        /// </summary>
        public static Session_Id New

            => Session_Id.Parse(Guid.NewGuid().ToString());

        #endregion

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length

            => (UInt64) InternalId.Length;

        #endregion

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


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static Session_Id Parse(String Text)

            => new Session_Id(Text);

        #endregion

        #region TryParse(Text, out ChargingSessionId)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        /// <param name="ChargingSessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(String Text, out Session_Id ChargingSessionId)
        {
            try
            {

                ChargingSessionId = new Session_Id(Text);

                return true;

            }
            catch (Exception)
            {
                ChargingSessionId = default(Session_Id);
                return false;
            }
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

        #region Operator == (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Session_Id ChargingSessionId1, Session_Id ChargingSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingSessionId1, ChargingSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingSessionId1 == null) || ((Object) ChargingSessionId2 == null))
                return false;

            return ChargingSessionId1.Equals(ChargingSessionId2);

        }

        #endregion

        #region Operator != (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Session_Id ChargingSessionId1, Session_Id ChargingSessionId2)
            => !(ChargingSessionId1 == ChargingSessionId2);

        #endregion

        #region Operator <  (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Session_Id ChargingSessionId1, Session_Id ChargingSessionId2)
        {

            if ((Object) ChargingSessionId1 == null)
                throw new ArgumentNullException(nameof(ChargingSessionId1), "The given ChargingSessionId1 must not be null!");

            return ChargingSessionId1.CompareTo(ChargingSessionId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Session_Id ChargingSessionId1, Session_Id ChargingSessionId2)
            => !(ChargingSessionId1 > ChargingSessionId2);

        #endregion

        #region Operator >  (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Session_Id ChargingSessionId1, Session_Id ChargingSessionId2)
        {

            if ((Object) ChargingSessionId1 == null)
                throw new ArgumentNullException(nameof(ChargingSessionId1), "The given ChargingSessionId1 must not be null!");

            return ChargingSessionId1.CompareTo(ChargingSessionId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Session_Id ChargingSessionId1, Session_Id ChargingSessionId2)
            => !(ChargingSessionId1 < ChargingSessionId2);

        #endregion

        #endregion

        #region IComparable<ChargingSessionId> Members

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

        #region CompareTo(ChargingSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId">An object to compare with.</param>
        public Int32 CompareTo(Session_Id ChargingSessionId)
        {

            if ((Object) ChargingSessionId == null)
                throw new ArgumentNullException(nameof(ChargingSessionId),  "The given charging session identification must not be null!");

            // Compare the length of the ChargingSessionIds
            var _Result = this.Length.CompareTo(ChargingSessionId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, ChargingSessionId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingSessionId> Members

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

        #region Equals(ChargingSessionId)

        /// <summary>
        /// Compares two ChargingSessionIds for equality.
        /// </summary>
        /// <param name="ChargingSessionId">A ChargingSessionId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Session_Id ChargingSessionId)
        {

            if ((Object) ChargingSessionId == null)
                return false;

            return InternalId.Equals(ChargingSessionId.InternalId);

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
