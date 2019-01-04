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
    /// The unique identification of an OICP hub provider.
    /// </summary>
    public struct HubProvider_Id : IId,
                                   IEquatable <HubProvider_Id>,
                                   IComparable<HubProvider_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the hub provider identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new hub provider identification.
        /// based on the given string.
        /// </summary>
        private HubProvider_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse   (Text)

        /// <summary>
        /// Parse the given string as a hub provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a hub provider identification.</param>
        public static HubProvider_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a hub provider identification must not be null or empty!");

            #endregion

            return new HubProvider_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a hub provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a hub provider identification.</param>
        public static HubProvider_Id? TryParse(String Text)
        {

            if (Text != null)
                Text = Text.Trim();

            return Text.IsNullOrEmpty()
                       ? new HubProvider_Id?()
                       : new HubProvider_Id(Text);

        }

        #endregion

        #region TryParse(Text, out HubProviderId)

        /// <summary>
        /// Try to parse the given string as a hub provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a hub provider identification.</param>
        /// <param name="HubProviderId">The parsed hub provider identification.</param>
        public static Boolean TryParse(String Text, out HubProvider_Id HubProviderId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                HubProviderId = default(HubProvider_Id);
                return false;
            }

            #endregion

            try
            {

                HubProviderId = new HubProvider_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            HubProviderId = default(HubProvider_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this hub provider identification.
        /// </summary>
        public HubProvider_Id Clone

            => new HubProvider_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Provider overloading

        #region Provider == (HubProviderId1, HubProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId1">A hub provider identification.</param>
        /// <param name="HubProviderId2">Another hub provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (HubProvider_Id HubProviderId1, HubProvider_Id HubProviderId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(HubProviderId1, HubProviderId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) HubProviderId1 == null) || ((Object) HubProviderId2 == null))
                return false;

            return HubProviderId1.Equals(HubProviderId2);

        }

        #endregion

        #region Provider != (HubProviderId1, HubProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId1">A hub provider identification.</param>
        /// <param name="HubProviderId2">Another hub provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (HubProvider_Id HubProviderId1, HubProvider_Id HubProviderId2)
            => !(HubProviderId1 == HubProviderId2);

        #endregion

        #region Provider <  (HubProviderId1, HubProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId1">A hub provider identification.</param>
        /// <param name="HubProviderId2">Another hub provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (HubProvider_Id HubProviderId1, HubProvider_Id HubProviderId2)
        {

            if ((Object) HubProviderId1 == null)
                throw new ArgumentNullException(nameof(HubProviderId1), "The given HubProviderId1 must not be null!");

            return HubProviderId1.CompareTo(HubProviderId2) < 0;

        }

        #endregion

        #region Provider <= (HubProviderId1, HubProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId1">A hub provider identification.</param>
        /// <param name="HubProviderId2">Another hub provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (HubProvider_Id HubProviderId1, HubProvider_Id HubProviderId2)
            => !(HubProviderId1 > HubProviderId2);

        #endregion

        #region Provider >  (HubProviderId1, HubProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId1">A hub provider identification.</param>
        /// <param name="HubProviderId2">Another hub provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (HubProvider_Id HubProviderId1, HubProvider_Id HubProviderId2)
        {

            if ((Object) HubProviderId1 == null)
                throw new ArgumentNullException(nameof(HubProviderId1), "The given HubProviderId1 must not be null!");

            return HubProviderId1.CompareTo(HubProviderId2) > 0;

        }

        #endregion

        #region Provider >= (HubProviderId1, HubProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId1">A hub provider identification.</param>
        /// <param name="HubProviderId2">Another hub provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (HubProvider_Id HubProviderId1, HubProvider_Id HubProviderId2)
            => !(HubProviderId1 < HubProviderId2);

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

            if (!(Object is HubProvider_Id))
                throw new ArgumentException("The given object is not a hub provider identification!",
                                            nameof(Object));

            return CompareTo((HubProvider_Id) Object);

        }

        #endregion

        #region CompareTo(HubProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HubProviderId">An object to compare with.</param>
        public Int32 CompareTo(HubProvider_Id HubProviderId)
        {

            if ((Object) HubProviderId == null)
                throw new ArgumentNullException(nameof(HubProviderId),  "The given hub provider identification must not be null!");

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

            if (!(Object is HubProvider_Id))
                return false;

            return Equals((HubProvider_Id) Object);

        }

        #endregion

        #region Equals(HubProviderId)

        /// <summary>
        /// Compares two HubProviderIds for equality.
        /// </summary>
        /// <param name="HubProviderId">A hub provider identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(HubProvider_Id HubProviderId)
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
