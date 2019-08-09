/*
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The unique identification of an OICP clearing house.
    /// </summary>
    public struct ClearingHouse_Id : IId,
                                     IEquatable <ClearingHouse_Id>,
                                     IComparable<ClearingHouse_Id>

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
        /// The length of the clearing house identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new clearing house identification.
        /// based on the given string.
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
        /// <param name="Text">A text representation of a clearing house identification.</param>
        public static ClearingHouse_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a clearing house identification must not be null or empty!");

            #endregion

            return new ClearingHouse_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a clearing house identification.
        /// </summary>
        /// <param name="Text">A text representation of a clearing house identification.</param>
        public static ClearingHouse_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new ClearingHouse_Id?()
                       : new ClearingHouse_Id(Text);

        }

        #endregion

        #region TryParse(Text, out HubProviderId)

        /// <summary>
        /// Try to parse the given string as a clearing house identification.
        /// </summary>
        /// <param name="Text">A text representation of a clearing house identification.</param>
        /// <param name="HubProviderId">The parsed clearing house identification.</param>
        public static Boolean TryParse(String Text, out ClearingHouse_Id HubProviderId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                HubProviderId = default(ClearingHouse_Id);
                return false;
            }

            #endregion

            try
            {

                HubProviderId = new ClearingHouse_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            HubProviderId = default(ClearingHouse_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this clearing house identification.
        /// </summary>
        public ClearingHouse_Id Clone

            => new ClearingHouse_Id(
                   new String(InternalId.ToCharArray())
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
        public static Boolean operator == (ClearingHouse_Id ClearingHouseIdId1, ClearingHouse_Id ClearingHouseIdId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ClearingHouseIdId1, ClearingHouseIdId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ClearingHouseIdId1 == null) || ((Object) ClearingHouseIdId2 == null))
                return false;

            return ClearingHouseIdId1.Equals(ClearingHouseIdId2);

        }

        #endregion

        #region Operator != (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ClearingHouse_Id ClearingHouseIdId1, ClearingHouse_Id ClearingHouseIdId2)
            => !(ClearingHouseIdId1 == ClearingHouseIdId2);

        #endregion

        #region Operator <  (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ClearingHouse_Id ClearingHouseIdId1, ClearingHouse_Id ClearingHouseIdId2)
        {

            if ((Object) ClearingHouseIdId1 == null)
                throw new ArgumentNullException(nameof(ClearingHouseIdId1), "The given ClearingHouseIdId1 must not be null!");

            return ClearingHouseIdId1.CompareTo(ClearingHouseIdId2) < 0;

        }

        #endregion

        #region Operator <= (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ClearingHouse_Id ClearingHouseIdId1, ClearingHouse_Id ClearingHouseIdId2)
            => !(ClearingHouseIdId1 > ClearingHouseIdId2);

        #endregion

        #region Operator >  (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ClearingHouse_Id ClearingHouseIdId1, ClearingHouse_Id ClearingHouseIdId2)
        {

            if ((Object) ClearingHouseIdId1 == null)
                throw new ArgumentNullException(nameof(ClearingHouseIdId1), "The given ClearingHouseIdId1 must not be null!");

            return ClearingHouseIdId1.CompareTo(ClearingHouseIdId2) > 0;

        }

        #endregion

        #region Operator >= (ClearingHouseIdId1, ClearingHouseIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ClearingHouseIdId1">A clearing house identification.</param>
        /// <param name="ClearingHouseIdId2">Another clearing house identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ClearingHouse_Id ClearingHouseIdId1, ClearingHouse_Id ClearingHouseIdId2)
            => !(ClearingHouseIdId1 < ClearingHouseIdId2);

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

            if (!(Object is ClearingHouse_Id))
                throw new ArgumentException("The given object is not a clearing house identification!",
                                            nameof(Object));

            return CompareTo((ClearingHouse_Id) Object);

        }

        #endregion

        #region CompareTo(HubProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId">An object to compare with.</param>
        public Int32 CompareTo(ClearingHouse_Id HubProviderId)
        {

            if ((Object) HubProviderId == null)
                throw new ArgumentNullException(nameof(HubProviderId),  "The given clearing house identification must not be null!");

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

            if (!(Object is ClearingHouse_Id))
                return false;

            return Equals((ClearingHouse_Id) Object);

        }

        #endregion

        #region Equals(HubProviderId)

        /// <summary>
        /// Compares two HubProviderIds for equality.
        /// </summary>
        /// <param name="HubProviderId">A clearing house identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ClearingHouse_Id HubProviderId)
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
