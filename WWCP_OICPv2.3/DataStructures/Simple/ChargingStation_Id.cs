/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for charging station identifications.
    /// </summary>
    public static class ChargingStationIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStation_Id? ChargingStationId)
            => !ChargingStationId.HasValue || ChargingStationId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStation_Id? ChargingStationId)
            => ChargingStationId.HasValue && ChargingStationId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging station.
    /// </summary>
    public readonly struct ChargingStation_Id : IId<ChargingStation_Id>
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
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging station identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station identification.
        /// based on the given string.
        /// </summary>
        private ChargingStation_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Generate (OperatorId, Address, GeoLocation = null, SubOperatorName = null, ChargingStationName = null, Length = 15, Mapper = null)

        /// <summary>
        /// Create a valid charging station identification based on the given parameters.
        /// </summary>
        /// <param name="OperatorId">The identification of an operator.</param>
        /// <param name="Address">The address of the charging station.</param>
        /// <param name="GeoLocation">An optional geo location of the charging station.</param>
        /// <param name="SubOperatorName">An optional name of the charging station suboperator.</param>
        /// <param name="ChargingStationName">An optional multi-language name of the charging station.</param>
        /// <param name="Length">The maximum size of the generated charging station identification suffix [12 &lt; n &lt; 50].</param>
        /// <param name="Mapper">A delegate to modify a generated charging station identification suffix.</param>
        public static ChargingStation_Id Generate(Operator_Id            OperatorId,
                                                  Address                Address,
                                                  GeoCoordinates?        GeoLocation           = null,
                                                  String?                SubOperatorName       = null,
                                                  I18NText?              ChargingStationName   = null,
                                                  Byte                   Length                = 15,
                                                  Func<String, String>?  Mapper                = null)
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
                                                 GeoLocation?.ToString()                                  ?? "",
                                                 SubOperatorName                                          ?? "",
                                                 ChargingStationName?.ToJSON()?.ToString(Formatting.None) ?? ""
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
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id NewRandom(Func<String, String>? Mapper = null)

            => new (Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(50))
                        :        RandomExtensions.RandomString(50));

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingStationId))
                return chargingStationId;

            throw new ArgumentException($"Invalid text representation of a charging station identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationId))
                return chargingStationId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out ChargingStationId)

        /// <summary>
        /// Try to parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationId = new ChargingStation_Id(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            ChargingStationId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station identification.
        /// </summary>
        public ChargingStation_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.Equals(ChargingStationId2);

        #endregion

        #region Operator != (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => !ChargingStationId1.Equals(ChargingStationId2);

        #endregion

        #region Operator <  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStationId1,
                                          ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) < 0;

        #endregion

        #region Operator <= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStationId1,
                                          ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) > 0;

        #endregion

        #region Operator >= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station identifications.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStation_Id chargingStationId
                   ? CompareTo(chargingStationId)
                   : throw new ArgumentException("The given object is not a charging station identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification to compare with.</param>
        public Int32 CompareTo(ChargingStation_Id ChargingStationId)

            => String.Compare(InternalId,
                              ChargingStationId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStation_Id chargingStationId &&
                   Equals(chargingStationId);

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification to compare with.</param>
        public Boolean Equals(ChargingStation_Id ChargingStationId)

            => String.Equals(InternalId,
                             ChargingStationId.InternalId,
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
