/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The unique identification of an OICP charging pool.
    /// </summary>
    public struct ChargingPool_Id : IId,
                                    IEquatable <ChargingPool_Id>,
                                    IComparable<ChargingPool_Id>

    {

        //ToDo: Implement proper charging pool id format!
        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the charging pool identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

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
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging pool identification must not be null or empty!");

            #endregion

            return new ChargingPool_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new ChargingPool_Id?()
                       : new ChargingPool_Id(Text);

        }

        #endregion

        #region TryParse(Text, out ChargingPool_Id)

        /// <summary>
        /// Try to parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingPoolId = default(ChargingPool_Id);
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

            ChargingPoolId = default(ChargingPool_Id);
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
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingPoolId1, ChargingPoolId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolId1 == null) || ((Object) ChargingPoolId2 == null))
                return false;

            return ChargingPoolId1.Equals(ChargingPoolId2);

        }

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
            => !(ChargingPoolId1 == ChargingPoolId2);

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolId1), "The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
            => !(ChargingPoolId1 > ChargingPoolId2);

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolId1), "The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
            => !(ChargingPoolId1 < ChargingPoolId2);

        #endregion

        #endregion

        #region IComparable<HubProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPool_Id))
                throw new ArgumentException("The given object is not a charging pool identification!",
                                            nameof(Object));

            return CompareTo((ChargingPool_Id) Object);

        }

        #endregion

        #region CompareTo(HubProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId">An object to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id HubProviderId)
        {

            if ((Object) HubProviderId == null)
                throw new ArgumentNullException(nameof(HubProviderId),  "The given charging pool identification must not be null!");

            // Compare the length of the HubProviderIds
            var _Result = this.Length.CompareTo(HubProviderId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, HubProviderId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<HubProviderId> Members

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

            if (!(Object is ChargingPool_Id))
                return false;

            return Equals((ChargingPool_Id) Object);

        }

        #endregion

        #region Equals(HubProviderId)

        /// <summary>
        /// Compares two HubProviderIds for equality.
        /// </summary>
        /// <param name="HubProviderId">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool_Id HubProviderId)
        {

            if ((Object) HubProviderId == null)
                return false;

            return InternalId.Equals(HubProviderId.InternalId);

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
