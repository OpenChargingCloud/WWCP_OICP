/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The unique identification of a partner product.
    /// </summary>
    public readonly struct PartnerProduct_Id : IId<PartnerProduct_Id>
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
        /// The length of the partner product identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new partner product identification.
        /// based on the given string.
        /// </summary>
        private PartnerProduct_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a partner product identification.
        /// </summary>
        /// <param name="Text">A text-representation of a partner product identification.</param>
        public static PartnerProduct_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text-representation of a partner product identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out PartnerProduct_Id partnerProductId))
                return partnerProductId;

            throw new ArgumentException("Illegal text-representation of a partner product identification: '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a partner product identification.
        /// </summary>
        /// <param name="Text">A text-representation of a partner product identification.</param>
        public static PartnerProduct_Id? TryParse(String Text)
        {

            if (TryParse(Text, out PartnerProduct_Id partnerProductId))
                return partnerProductId;

            return new PartnerProduct_Id?();

        }

        #endregion

        #region (static) TryParse(JToken)

        /// <summary>
        /// Try to parse the given JSON token as a partner product identification.
        /// </summary>
        /// <param name="JToken">A JSON token representation of a partner product identification.</param>
        public static PartnerProduct_Id? TryParse(JToken JToken)
            => TryParse(JToken?.Value<String>());

        #endregion

        #region (static) TryParse(Text, out PartnerProductId)

        /// <summary>
        /// Parse the given string as a partner product identification.
        /// </summary>
        /// <param name="Text">A text-representation of a partner product identification.</param>
        /// <param name="PartnerProductId">The parsed partner product identification.</param>
        public static Boolean TryParse(String Text, out PartnerProduct_Id PartnerProductId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                PartnerProductId = default;
                return false;
            }

            #endregion

            try
            {
                PartnerProductId = new PartnerProduct_Id(Text);
                return true;
            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            PartnerProductId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this partner product identification.
        /// </summary>
        public PartnerProduct_Id Clone

            => new PartnerProduct_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PartnerProduct_Id PartnerProductId1, PartnerProduct_Id PartnerProductId2)
            => PartnerProductId1.Equals(PartnerProductId2);

        #endregion

        #region Operator != (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerProduct_Id PartnerProductId1, PartnerProduct_Id PartnerProductId2)
            => !PartnerProductId1.Equals(PartnerProductId2);

        #endregion

        #region Operator <  (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerProduct_Id PartnerProductId1, PartnerProduct_Id PartnerProductId2)
            => PartnerProductId1.CompareTo(PartnerProductId2) < 0;

        #endregion

        #region Operator <= (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerProduct_Id PartnerProductId1, PartnerProduct_Id PartnerProductId2)
            => PartnerProductId1.CompareTo(PartnerProductId2) <= 0;

        #endregion

        #region Operator >  (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerProduct_Id PartnerProductId1, PartnerProduct_Id PartnerProductId2)
            => PartnerProductId1.CompareTo(PartnerProductId2) > 0;

        #endregion

        #region Operator >= (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerProduct_Id PartnerProductId1, PartnerProduct_Id PartnerProductId2)
            => PartnerProductId1.CompareTo(PartnerProductId2) >= 0;

        #endregion

        #endregion

        #region IComparable<PartnerProductId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is PartnerProduct_Id partnerProductId
                   ? CompareTo(partnerProductId)
                   : throw new ArgumentException("The given object is not a partner product identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PartnerProductId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId">An object to compare with.</param>
        public Int32 CompareTo(PartnerProduct_Id PartnerProductId)

            => String.Compare(InternalId,
                              PartnerProductId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<PartnerProductId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is PartnerProduct_Id partnerProductId
                   ? Equals(partnerProductId)
                   : false;

        #endregion

        #region Equals(PartnerProductId)

        /// <summary>
        /// Compares two PartnerProductIds for equality.
        /// </summary>
        /// <param name="PartnerProductId">A partner product identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(PartnerProduct_Id PartnerProductId)

            => String.Equals(InternalId,
                             PartnerProductId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

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
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
