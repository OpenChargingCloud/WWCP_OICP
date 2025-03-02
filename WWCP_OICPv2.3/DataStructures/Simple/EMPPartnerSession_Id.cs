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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for EMP partner session identifications.
    /// </summary>
    public static class EMPPartnerSessionIdExtensions
    {

        /// <summary>
        /// Indicates whether this EMP partner session identification is null or empty.
        /// </summary>
        /// <param name="EMPPartnerSessionId">A EMP partner session identification.</param>
        public static Boolean IsNullOrEmpty(this EMPPartnerSession_Id? EMPPartnerSessionId)
            => !EMPPartnerSessionId.HasValue || EMPPartnerSessionId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EMP partner session identification is null or empty.
        /// </summary>
        /// <param name="EMPPartnerSessionId">A EMP partner session identification.</param>
        public static Boolean IsNotNullOrEmpty(this EMPPartnerSession_Id? EMPPartnerSessionId)
            => EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging session.
    /// </summary>
    public readonly struct EMPPartnerSession_Id : IId<EMPPartnerSession_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging session identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging session identifier.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging session identification.
        /// based on the given string.
        /// </summary>
        private EMPPartnerSession_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Mapper = null)

        /// <summary>
        /// Generate a new random charging session identification.
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated charging session identification.</param>
        public static EMPPartnerSession_Id NewRandom(Func<String, String>? Mapper = null)

            => new(Mapper is not null
                        ? Mapper(Guid.NewGuid().ToString())
                        : Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static EMPPartnerSession_Id Parse(String Text)
        {

            if (TryParse(Text, out var sessionId))
                return sessionId;

            throw new ArgumentException($"Invalid text representation of an EMP charging session identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static EMPPartnerSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var sessionId))
                return sessionId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out SessionId)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        /// <param name="SessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(String Text, out EMPPartnerSession_Id SessionId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    SessionId = new EMPPartnerSession_Id(Text.Trim().SubstringMax(250));
                    return true;
                }
                catch
                { }
            }

            SessionId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging session identification.
        /// </summary>
        public EMPPartnerSession_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMPPartnerSession_Id SessionId1,
                                           EMPPartnerSession_Id SessionId2)

            => SessionId1.Equals(SessionId2);

        #endregion

        #region Operator != (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMPPartnerSession_Id SessionId1,
                                           EMPPartnerSession_Id SessionId2)

            => !SessionId1.Equals(SessionId2);

        #endregion

        #region Operator <  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMPPartnerSession_Id SessionId1,
                                          EMPPartnerSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) < 0;

        #endregion

        #region Operator <= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMPPartnerSession_Id SessionId1,
                                           EMPPartnerSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) <= 0;

        #endregion

        #region Operator >  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMPPartnerSession_Id SessionId1,
                                          EMPPartnerSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) > 0;

        #endregion

        #region Operator >= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A charging session identification.</param>
        /// <param name="SessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMPPartnerSession_Id SessionId1,
                                           EMPPartnerSession_Id SessionId2)

            => SessionId1.CompareTo(SessionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging session identifications.
        /// </summary>
        /// <param name="Object">A charging session identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMPPartnerSession_Id sessionId
                   ? CompareTo(sessionId)
                   : throw new ArgumentException("The given object is not a charging session identification!");

        #endregion

        #region CompareTo(SessionId)

        /// <summary>
        /// Compares two charging session identifications.
        /// </summary>
        /// <param name="SessionId">A charging session identification to compare with.</param>
        public Int32 CompareTo(EMPPartnerSession_Id SessionId)

            => String.Compare(InternalId,
                              SessionId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SessionId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging session identifications for equality.
        /// </summary>
        /// <param name="Object">A charging session identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMPPartnerSession_Id sessionId &&
                   Equals(sessionId);

        #endregion

        #region Equals(SessionId)

        /// <summary>
        /// Compares two charging session identifications for equality.
        /// </summary>
        /// <param name="SessionId">A charging session identification to compare with.</param>
        public Boolean Equals(EMPPartnerSession_Id SessionId)

            => String.Equals(InternalId,
                             SessionId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

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
