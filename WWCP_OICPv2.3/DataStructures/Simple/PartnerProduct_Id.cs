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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for partner product identifications.
    /// </summary>
    public static class PartnerProductIdExtensions
    {

        /// <summary>
        /// Indicates whether this partner product identification is null or empty.
        /// </summary>
        /// <param name="PartnerProductId">A partner product identification.</param>
        public static Boolean IsNullOrEmpty(this PartnerProduct_Id? PartnerProductId)
            => !PartnerProductId.HasValue || PartnerProductId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this partner product identification is null or empty.
        /// </summary>
        /// <param name="PartnerProductId">A partner product identification.</param>
        public static Boolean IsNotNullOrEmpty(this PartnerProduct_Id? PartnerProductId)
            => PartnerProductId.HasValue && PartnerProductId.Value.IsNotNullOrEmpty;

    }


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
        /// Indicates whether this partner product identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this partner product identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the partner product identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

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
        /// <param name="Text">A text representation of a partner product identification.</param>
        public static PartnerProduct_Id Parse(String Text)
        {

            if (TryParse(Text, out var partnerProductId))
                return partnerProductId;

            throw new ArgumentException("Invalid text representation of a partner product identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a partner product identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner product identification.</param>
        public static PartnerProduct_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var partnerProductId))
                return partnerProductId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out PartnerProductId)

        /// <summary>
        /// Parse the given string as a partner product identification.
        /// </summary>
        /// <param name="Text">A text representation of a partner product identification.</param>
        /// <param name="PartnerProductId">The parsed partner product identification.</param>
        public static Boolean TryParse(String Text, out PartnerProduct_Id PartnerProductId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    PartnerProductId = new PartnerProduct_Id(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            PartnerProductId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this partner product identification.
        /// </summary>
        public PartnerProduct_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region (static) StandardPrice

        /// <summary>
        /// Standard Price.
        /// </summary>
        public static PartnerProduct_Id StandardPrice
            => Parse("Standard Price");

        #endregion

        #region (static) AC1

        /// <summary>
        /// Product for AC 1 Phase charging.
        /// </summary>
        public static PartnerProduct_Id AC1
            => Parse("AC1");

        #endregion

        #region (static) AC3

        /// <summary>
        /// Product for AC 3 Phase charging.
        /// </summary>
        public static PartnerProduct_Id AC3
            => Parse("AC3");

        #endregion

        #region (static) DC

        /// <summary>
        /// Product for DC charging.
        /// </summary>
        public static PartnerProduct_Id DC
            => Parse("DC");

        #endregion


        #region Operator overloading

        #region Operator == (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PartnerProduct_Id PartnerProductId1,
                                           PartnerProduct_Id PartnerProductId2)

            => PartnerProductId1.Equals(PartnerProductId2);

        #endregion

        #region Operator != (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PartnerProduct_Id PartnerProductId1,
                                           PartnerProduct_Id PartnerProductId2)

            => !PartnerProductId1.Equals(PartnerProductId2);

        #endregion

        #region Operator <  (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (PartnerProduct_Id PartnerProductId1,
                                          PartnerProduct_Id PartnerProductId2)

            => PartnerProductId1.CompareTo(PartnerProductId2) < 0;

        #endregion

        #region Operator <= (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PartnerProduct_Id PartnerProductId1,
                                           PartnerProduct_Id PartnerProductId2)

            => PartnerProductId1.CompareTo(PartnerProductId2) <= 0;

        #endregion

        #region Operator >  (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (PartnerProduct_Id PartnerProductId1,
                                          PartnerProduct_Id PartnerProductId2)

            => PartnerProductId1.CompareTo(PartnerProductId2) > 0;

        #endregion

        #region Operator >= (PartnerProductId1, PartnerProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PartnerProductId1">A partner product identification.</param>
        /// <param name="PartnerProductId2">Another partner product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PartnerProduct_Id PartnerProductId1,
                                           PartnerProduct_Id PartnerProductId2)

            => PartnerProductId1.CompareTo(PartnerProductId2) >= 0;

        #endregion

        #endregion

        #region IComparable<PartnerProductId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two partner product identifications.
        /// </summary>
        /// <param name="Object">A partner product identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is PartnerProduct_Id partnerProductId
                   ? CompareTo(partnerProductId)
                   : throw new ArgumentException("The given object is not a partner product identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PartnerProductId)

        /// <summary>
        /// Compares two partner product identifications.
        /// </summary>
        /// <param name="PartnerProductId">A partner product identification to compare with.</param>
        public Int32 CompareTo(PartnerProduct_Id PartnerProductId)

            => String.Compare(InternalId,
                              PartnerProductId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<PartnerProductId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two partner product identifications for equality.
        /// </summary>
        /// <param name="PartnerProductId">A partner product identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PartnerProduct_Id partnerProductId &&
                   Equals(partnerProductId);

        #endregion

        #region Equals(PartnerProductId)

        /// <summary>
        /// Compares two partner product identifications for equality.
        /// </summary>
        /// <param name="PartnerProductId">A partner product identification to compare with.</param>
        public Boolean Equals(PartnerProduct_Id PartnerProductId)

            => String.Equals(InternalId,
                             PartnerProductId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

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
