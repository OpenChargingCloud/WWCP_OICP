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
    /// Extension methods for additional references.
    /// </summary>
    public static class AdditionalReferenceExtensions
    {

        /// <summary>
        /// Indicates whether this additional reference is null or empty.
        /// </summary>
        /// <param name="AdditionalReference">A additional reference.</param>
        public static Boolean IsNullOrEmpty(this Additional_Reference? AdditionalReference)
            => !AdditionalReference.HasValue || AdditionalReference.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this additional reference is null or empty.
        /// </summary>
        /// <param name="AdditionalReference">A additional reference.</param>
        public static Boolean IsNotNullOrEmpty(this Additional_Reference? AdditionalReference)
            => AdditionalReference.HasValue && AdditionalReference.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// An additional reference for pricing information.
    /// </summary>
    public readonly struct Additional_Reference : IId<Additional_Reference>
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
        /// The length of the additional reference.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new additional reference.
        /// based on the given string.
        /// </summary>
        private Additional_Reference(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an additional reference.
        /// </summary>
        /// <param name="Text">A text representation of an additional reference.</param>
        public static Additional_Reference Parse(String Text)
        {

            if (TryParse(Text, out var additionalReference))
                return additionalReference;

            throw new ArgumentException($"Invalid text representation of an additional reference: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an additional reference.
        /// </summary>
        /// <param name="Text">A text representation of an additional reference.</param>
        public static Additional_Reference? TryParse(String Text)
        {

            if (TryParse(Text, out var additionalReference))
                return additionalReference;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out AdditionalReference)

        /// <summary>
        /// Try to parse the given string as an additional reference.
        /// </summary>
        /// <param name="Text">A text representation of an additional reference.</param>
        /// <param name="AdditionalReference">The parsed additional reference.</param>
        public static Boolean TryParse(String Text, out Additional_Reference AdditionalReference)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    AdditionalReference = new Additional_Reference(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            AdditionalReference = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this additional reference.
        /// </summary>
        public Additional_Reference Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static Definitions

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#AdditionalReferenceType

        /// <summary>
        /// Can be used in case a fixed fee is charged for the initiation of the charging session.
        /// This is a fee charged on top of the main base price defined in the field "PricePerReferenceUnit" for any particular pricing product.
        /// </summary>
        public static Additional_Reference StartFee
            => new ("START FEE");

        /// <summary>
        /// Can be used if a single price is charged irrespective of charging duration or energy consumption (for instance if all sessions are to be charged a single fixed fee).
        /// When used, the value set in the field "PricePerReferenceUnit" for the main base price of respective pricing product SHOULD be set to zero.
        /// </summary>
        public static Additional_Reference FixedFee
            => new ("FIXED FEE");

        /// <summary>
        /// Can be used in case sessions are to be charged for both parking and charging. When used, it needs to be specified in the corresponding service offer on the HBS Portal
        /// when parking applies (e.g. from session start to charging start and charging end to session end or for the entire session duration, or x-minutes after charging end, etc)
        /// </summary>
        public static Additional_Reference ParkingFee
            => new ("PARKING FEE");

        /// <summary>
        /// Can be used in case there is a minimum fee to be paid for all charging sessions. When used, this implies that the eventual price to be paid cannot be less than this
        /// minimum fee but can however be a price above/greater than the minimum fee.
        /// </summary>
        public static Additional_Reference MinimumFee
            => new ("MINIMUM FEE");

        /// <summary>
        /// Can be used in case there is a maximum fee to be charged for all charging sessions. When used, this implies that the eventual price to be paid cannot be more than
        /// this maximum fee but can however be a price below/lower than the maximum fee.
        /// </summary>
        public static Additional_Reference MaximumFee
            => new ("MAXIMUM FEE");

        #endregion


        #region Operator overloading

        #region Operator == (AdditionalReference1, AdditionalReference2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReference1">A additional reference.</param>
        /// <param name="AdditionalReference2">Another additional reference.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Additional_Reference AdditionalReference1,
                                           Additional_Reference AdditionalReference2)

            => AdditionalReference1.Equals(AdditionalReference2);

        #endregion

        #region Operator != (AdditionalReference1, AdditionalReference2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReference1">A additional reference.</param>
        /// <param name="AdditionalReference2">Another additional reference.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Additional_Reference AdditionalReference1,
                                           Additional_Reference AdditionalReference2)

            => !AdditionalReference1.Equals(AdditionalReference2);

        #endregion

        #region Operator <  (AdditionalReference1, AdditionalReference2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReference1">A additional reference.</param>
        /// <param name="AdditionalReference2">Another additional reference.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Additional_Reference AdditionalReference1,
                                          Additional_Reference AdditionalReference2)

            => AdditionalReference1.CompareTo(AdditionalReference2) < 0;

        #endregion

        #region Operator <= (AdditionalReference1, AdditionalReference2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReference1">A additional reference.</param>
        /// <param name="AdditionalReference2">Another additional reference.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Additional_Reference AdditionalReference1,
                                           Additional_Reference AdditionalReference2)

            => AdditionalReference1.CompareTo(AdditionalReference2) <= 0;

        #endregion

        #region Operator >  (AdditionalReference1, AdditionalReference2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReference1">A additional reference.</param>
        /// <param name="AdditionalReference2">Another additional reference.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Additional_Reference AdditionalReference1,
                                          Additional_Reference AdditionalReference2)

            => AdditionalReference1.CompareTo(AdditionalReference2) > 0;

        #endregion

        #region Operator >= (AdditionalReference1, AdditionalReference2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReference1">A additional reference.</param>
        /// <param name="AdditionalReference2">Another additional reference.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Additional_Reference AdditionalReference1,
                                           Additional_Reference AdditionalReference2)

            => AdditionalReference1.CompareTo(AdditionalReference2) >= 0;

        #endregion

        #endregion

        #region IComparable<AdditionalReference> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two additional references.
        /// </summary>
        /// <param name="Object">An additional reference to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Additional_Reference additionalReference
                   ? CompareTo(additionalReference)
                   : throw new ArgumentException("The given object is not an additional reference!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AdditionalReference)

        /// <summary>
        /// Compares two additional references.
        /// </summary>
        /// <param name="AdditionalReference">An additional reference to compare with.</param>
        public Int32 CompareTo(Additional_Reference AdditionalReference)

            => String.Compare(InternalId,
                              AdditionalReference.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<AdditionalReference> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two additional references for equality.
        /// </summary>
        /// <param name="Object">An additional reference to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Additional_Reference additionalReference &&
                   Equals(additionalReference);

        #endregion

        #region Equals(AdditionalReference)

        /// <summary>
        /// Compares two additional references for equality.
        /// </summary>
        /// <param name="AdditionalReference">An additional reference to compare with.</param>
        public Boolean Equals(Additional_Reference AdditionalReference)

            => String.Equals(InternalId,
                             AdditionalReference.InternalId,
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
