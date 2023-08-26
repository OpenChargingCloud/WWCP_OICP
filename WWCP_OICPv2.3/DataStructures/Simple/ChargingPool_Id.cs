/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using System.Text;
using System.Security.Cryptography;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for charging pool identifications.
    /// </summary>
    public static class ChargingPoolIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging pool identification is null or empty.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingPool_Id? ChargingPoolId)
            => !ChargingPoolId.HasValue || ChargingPoolId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging pool identification is null or empty.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingPool_Id? ChargingPoolId)
            => ChargingPoolId.HasValue && ChargingPoolId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging pool.
    /// </summary>
    public readonly struct ChargingPool_Id : IId<ChargingPool_Id>
    {

        #region Data

        //ToDo: Implement proper charging pool id format!
        // ([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?P[A-Za-z0-9\*]{1,30})

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
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging pool identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool identification.
        /// based on the given string.
        /// </summary>
        private ChargingPool_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Generate (OperatorId, Address, GeoLocation = null, SubOperatorName = null, Length = 15, Mapper = null)

        /// <summary>
        /// Create a valid charging pool identification based on the given parameters.
        /// </summary>
        /// <param name="OperatorId">The identification of an operator.</param>
        /// <param name="Address">The address of the charging pool.</param>
        /// <param name="GeoLocation">An optional geo location of the charging pool.</param>
        /// <param name="SubOperatorName">An optional name of the charging pool suboperator.</param>
        /// <param name="Length">The maximum size of the generated charging pool identification suffix [12 &lt; n &lt; 50].</param>
        /// <param name="Mapper">A delegate to modify a generated charging pool identification suffix.</param>
        public static ChargingPool_Id Generate(Operator_Id            OperatorId,
                                               Address                Address,
                                               GeoCoordinates?        GeoLocation       = null,
                                               String?                SubOperatorName   = null,
                                               Byte                   Length            = 15,
                                               Func<String, String>?  Mapper            = null)
        {

            if (Length < 12)
                Length = 12;

            if (Length > 50)
                Length = 50;

            var Suffix = SHA256.Create().
                             ComputeHash(Encoding.UTF8.GetBytes(
                                             String.Concat(
                                                 OperatorId.  ToString(),
                                                 Address.     ToString(),
                                                 GeoLocation?.ToString() ?? "",
                                                 SubOperatorName         ?? ""
                                             )
                                         )).
                                         ToHexString().
                                         SubstringMax(Length).
                                         ToUpper();

            return Parse(Mapper is not null
                            ? Mapper(Suffix)
                            : Suffix);

        }

        #endregion

        #region (static) NewRandom(Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging pool identification.
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated charging pool identification.</param>
        public static ChargingPool_Id NewRandom(Func<String, String>? Mapper = null)

            => new (Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(50))
                        :        RandomExtensions.RandomString(50));

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException($"Invalid text representation of a charging pool identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out ChargingPoolId)

        /// <summary>
        /// Try to parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingPoolId = new ChargingPool_Id(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            ChargingPoolId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging pool identification.
        /// </summary>
        public ChargingPool_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.Equals(ChargingPoolId2);

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => !ChargingPoolId1.Equals(ChargingPoolId2);

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1,
                                          ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1,
                                          ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool identifications.
        /// </summary>
        /// <param name="Object">A charging pool identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPool_Id chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a charging pool identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)

            => String.Compare(InternalId,
                              ChargingPoolId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingPoolId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="Object">A charging pool identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPool_Id chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)

            => String.Equals(InternalId,
                             ChargingPoolId.InternalId,
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
