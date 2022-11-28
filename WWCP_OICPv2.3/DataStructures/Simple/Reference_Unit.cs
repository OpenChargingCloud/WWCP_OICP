﻿/*
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
    /// Extension methods for reference unit identifications.
    /// </summary>
    public static class ReferenceUnitExtensions
    {

        /// <summary>
        /// Indicates whether this reference unit identification is null or empty.
        /// </summary>
        /// <param name="ReferenceUnit">A reference unit identification.</param>
        public static Boolean IsNullOrEmpty(this Reference_Unit? ReferenceUnit)
            => !ReferenceUnit.HasValue || ReferenceUnit.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this reference unit identification is null or empty.
        /// </summary>
        /// <param name="ReferenceUnit">A reference unit identification.</param>
        public static Boolean IsNotNullOrEmpty(this Reference_Unit? ReferenceUnit)
            => ReferenceUnit.HasValue && ReferenceUnit.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a reference unit.
    /// </summary>
    public readonly struct Reference_Unit : IId<Reference_Unit>
    {

        #region Data

        //ToDo: Implement proper reference unit id format!
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
        /// The length of the reference unit identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new reference unit identification.
        /// based on the given string.
        /// </summary>
        private Reference_Unit(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a reference unit identification.
        /// </summary>
        /// <param name="Text">A text representation of a reference unit identification.</param>
        public static Reference_Unit Parse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException("Invalid text representation of a reference unit identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a reference unit identification.
        /// </summary>
        /// <param name="Text">A text representation of a reference unit identification.</param>
        public static Reference_Unit? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ReferenceUnit)

        /// <summary>
        /// Try to parse the given string as a reference unit identification.
        /// </summary>
        /// <param name="Text">A text representation of a reference unit identification.</param>
        /// <param name="ReferenceUnit">The parsed reference unit identification.</param>
        public static Boolean TryParse(String Text, out Reference_Unit ReferenceUnit)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ReferenceUnit = new Reference_Unit(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            ReferenceUnit = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this reference unit identification.
        /// </summary>
        public Reference_Unit Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static Definitions

        /// <summary>
        /// Hour
        /// </summary>
        public static Reference_Unit HOUR
            => new ("HOUR");

        /// <summary>
        /// kWh
        /// </summary>
        public static Reference_Unit KILOWATT_HOUR
            => new ("KILOWATT_HOUR");

        /// <summary>
        /// Minute
        /// </summary>
        public static Reference_Unit MINUTE
            => new ("MINUTE");

        #endregion


        #region Operator overloading

        #region Operator == (ReferenceUnit1, ReferenceUnit2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReferenceUnit1">A reference unit identification.</param>
        /// <param name="ReferenceUnit2">Another reference unit identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Reference_Unit ReferenceUnit1,
                                           Reference_Unit ReferenceUnit2)

            => ReferenceUnit1.Equals(ReferenceUnit2);

        #endregion

        #region Operator != (ReferenceUnit1, ReferenceUnit2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReferenceUnit1">A reference unit identification.</param>
        /// <param name="ReferenceUnit2">Another reference unit identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Reference_Unit ReferenceUnit1,
                                           Reference_Unit ReferenceUnit2)

            => !ReferenceUnit1.Equals(ReferenceUnit2);

        #endregion

        #region Operator <  (ReferenceUnit1, ReferenceUnit2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReferenceUnit1">A reference unit identification.</param>
        /// <param name="ReferenceUnit2">Another reference unit identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Reference_Unit ReferenceUnit1,
                                          Reference_Unit ReferenceUnit2)

            => ReferenceUnit1.CompareTo(ReferenceUnit2) < 0;

        #endregion

        #region Operator <= (ReferenceUnit1, ReferenceUnit2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReferenceUnit1">A reference unit identification.</param>
        /// <param name="ReferenceUnit2">Another reference unit identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Reference_Unit ReferenceUnit1,
                                           Reference_Unit ReferenceUnit2)

            => ReferenceUnit1.CompareTo(ReferenceUnit2) <= 0;

        #endregion

        #region Operator >  (ReferenceUnit1, ReferenceUnit2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReferenceUnit1">A reference unit identification.</param>
        /// <param name="ReferenceUnit2">Another reference unit identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Reference_Unit ReferenceUnit1,
                                          Reference_Unit ReferenceUnit2)

            => ReferenceUnit1.CompareTo(ReferenceUnit2) > 0;

        #endregion

        #region Operator >= (ReferenceUnit1, ReferenceUnit2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReferenceUnit1">A reference unit identification.</param>
        /// <param name="ReferenceUnit2">Another reference unit identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Reference_Unit ReferenceUnit1,
                                           Reference_Unit ReferenceUnit2)

            => ReferenceUnit1.CompareTo(ReferenceUnit2) >= 0;

        #endregion

        #endregion

        #region IComparable<ReferenceUnit> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two reference units.
        /// </summary>
        /// <param name="Object">A reference unit to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Reference_Unit chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a reference unit identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ReferenceUnit)

        /// <summary>
        /// Compares two reference units.
        /// </summary>
        /// <param name="ReferenceUnit">A reference unit to compare with.</param>
        public Int32 CompareTo(Reference_Unit ReferenceUnit)

            => String.Compare(InternalId,
                              ReferenceUnit.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ReferenceUnit> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two reference units for equality.
        /// </summary>
        /// <param name="Object">A reference unit to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Reference_Unit chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(ReferenceUnit)

        /// <summary>
        /// Compares two reference units for equality.
        /// </summary>
        /// <param name="ReferenceUnit">A reference unit to compare with.</param>
        public Boolean Equals(Reference_Unit ReferenceUnit)

            => String.Equals(InternalId,
                             ReferenceUnit.InternalId,
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
