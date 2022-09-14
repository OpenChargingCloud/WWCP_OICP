/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
    /// Extension methods for clearing house identifications.
    /// </summary>
    public static class ClearingHouseIdExtensions
    {

        /// <summary>
        /// Indicates whether this clearing house identification is null or empty.
        /// </summary>
        /// <param name="ClearingHouseId">A clearing house identification.</param>
        public static Boolean IsNullOrEmpty(this ClearingHouse_Id? ClearingHouseId)
            => !ClearingHouseId.HasValue || ClearingHouseId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this clearing house identification is null or empty.
        /// </summary>
        /// <param name="ClearingHouseId">A clearing house identification.</param>
        public static Boolean IsNotNullOrEmpty(this ClearingHouse_Id? ClearingHouseId)
            => ClearingHouseId.HasValue && ClearingHouseId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a clearing house.
    /// </summary>
    public readonly struct ClearingHouse_Id : IId<ClearingHouse_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this clearing house identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this clearing house identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the clearing house identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new clearing house identification based on the given string.
        /// </summary>
        private ClearingHouse_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a clearing house identification.
        /// </summary>
        /// <param name="Text">A text-representation of a clearing house identification.</param>
        public static ClearingHouse_Id Parse(String Text)
        {

            if (TryParse(Text, out ClearingHouse_Id clearingHouseId))
                return clearingHouseId;

            throw new ArgumentException("Invalid text-representation of a clearing house identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a clearing house identification.
        /// </summary>
        /// <param name="Text">A text-representation of a clearing house identification.</param>
        public static ClearingHouse_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ClearingHouse_Id clearingHouseId))
                return clearingHouseId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ClearingHouseId)

        /// <summary>
        /// Try to parse the given string as a clearing house identification.
        /// </summary>
        /// <param name="Text">A text-representation of a clearing house identification.</param>
        /// <param name="ClearingHouseId">The parsed clearing house identification.</param>
        public static Boolean TryParse(String Text, out ClearingHouse_Id ClearingHouseId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ClearingHouseId = new ClearingHouse_Id(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            ClearingHouseId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this clearing house identification.
        /// </summary>
        public ClearingHouse_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ClearingHouse_Id ClearingHouseIdId1,
                                           ClearingHouse_Id ClearingHouseIdId2)

            => ClearingHouseIdId1.Equals(ClearingHouseIdId2);

        #endregion

        #region Operator != (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ClearingHouse_Id ClearingHouseIdId1,
                                           ClearingHouse_Id ClearingHouseIdId2)

            => !ClearingHouseIdId1.Equals(ClearingHouseIdId2);

        #endregion

        #region Operator <  (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ClearingHouse_Id ClearingHouseIdId1,
                                          ClearingHouse_Id ClearingHouseIdId2)

            => ClearingHouseIdId1.CompareTo(ClearingHouseIdId2) < 0;

        #endregion

        #region Operator <= (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ClearingHouse_Id ClearingHouseIdId1,
                                           ClearingHouse_Id ClearingHouseIdId2)

            => ClearingHouseIdId1.CompareTo(ClearingHouseIdId2) <= 0;

        #endregion

        #region Operator >  (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ClearingHouse_Id ClearingHouseIdId1,
                                          ClearingHouse_Id ClearingHouseIdId2)

            => ClearingHouseIdId1.CompareTo(ClearingHouseIdId2) > 0;

        #endregion

        #region Operator >= (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ClearingHouse_Id ClearingHouseIdId1,
                                           ClearingHouse_Id ClearingHouseIdId2)

            => ClearingHouseIdId1.CompareTo(ClearingHouseIdId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ClearingHouseId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ClearingHouse_Id clearingHouseId
                   ? CompareTo(clearingHouseId)
                   : throw new ArgumentException("The given object is not a clearing house identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ClearingHouseId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseId">An object to compare with.</param>
        public Int32 CompareTo(ClearingHouse_Id ClearingHouseId)

            => String.Compare(InternalId,
                              ClearingHouseId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ClearingHouseId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ClearingHouse_Id clearingHouseId &&
                   Equals(clearingHouseId);

        #endregion

        #region Equals(ClearingHouseId)

        /// <summary>
        /// Compares two ClearingHouseIds for equality.
        /// </summary>
        /// <param name="ClearingHouseId">A clearing house identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ClearingHouse_Id ClearingHouseId)

            => String.Equals(InternalId,
                             ClearingHouseId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower()?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
