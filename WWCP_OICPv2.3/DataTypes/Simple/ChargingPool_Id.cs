/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

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
        /// The length of the charging pool identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

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


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text-representation of a charging pool identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out ChargingPool_Id chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException("Invalid text-representation of a charging pool identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging pool identification.</param>
        public static ChargingPool_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingPool_Id chargingPoolId))
                return chargingPoolId;

            return default;

        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Try to parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text-representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingPoolId = default;
                return false;
            }

            #endregion

            try
            {
                ChargingPoolId = new ChargingPool_Id(Text);
                return true;
            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ChargingPoolId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging pool identification.
        /// </summary>
        public ChargingPool_Id Clone

            => new ChargingPool_Id(
                   new String(InternalId.ToCharArray())
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
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is ChargingPool_Id chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a charging pool identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId">An object to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)

            => String.Compare(InternalId,
                              ChargingPoolId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingPoolId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ChargingPool_Id chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two ChargingPoolIds for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)

            => String.Equals(InternalId,
                             ChargingPoolId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
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
