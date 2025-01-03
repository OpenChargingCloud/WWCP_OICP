/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for metering status types.
    /// </summary>
    public static class MeteringStatusTypeExtensions
    {

        /// <summary>
        /// Indicates whether this metering status type is null or empty.
        /// </summary>
        /// <param name="MeteringStatusType">A metering status type.</param>
        public static Boolean IsNullOrEmpty(this MeteringStatusType? MeteringStatusType)
            => !MeteringStatusType.HasValue || MeteringStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this metering status type is null or empty.
        /// </summary>
        /// <param name="MeteringStatusType">A metering status type.</param>
        public static Boolean IsNotNullOrEmpty(this MeteringStatusType? MeteringStatusType)
            => MeteringStatusType.HasValue && MeteringStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// A metering status type.
    /// </summary>
    /// <seealso cref="https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#signedmeteringvaluestype"/>
    public readonly struct MeteringStatusType : IId<MeteringStatusType>
    {

        #region Data

        private readonly static Dictionary<String, MeteringStatusType>  lookup = new (StringComparer.OrdinalIgnoreCase);
        private readonly        String                                  InternalId;

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
        /// The length of the metering status type.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new metering status type.
        /// based on the given string.
        /// </summary>
        private MeteringStatusType(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (private static) Register(Text)

        private static MeteringStatusType Register(String Text)

            => lookup.AddAndReturnValue(
                   Text,
                   new MeteringStatusType(Text)
               );

        #endregion


        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a metering status type.
        /// </summary>
        /// <param name="Text">A text representation of a metering status type.</param>
        public static MeteringStatusType Parse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException($"Invalid text representation of a metering status type: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a metering status type.
        /// </summary>
        /// <param name="Text">A text representation of a metering status type.</param>
        public static MeteringStatusType? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingPoolId))
                return chargingPoolId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out MeteringStatusType)

        /// <summary>
        /// Try to parse the given string as a metering status type.
        /// </summary>
        /// <param name="Text">A text representation of a metering status type.</param>
        /// <param name="MeteringStatusType">The parsed metering status type.</param>
        public static Boolean TryParse(String Text, out MeteringStatusType MeteringStatusType)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    MeteringStatusType = new MeteringStatusType(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            MeteringStatusType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this metering status type.
        /// </summary>
        public MeteringStatusType Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Metering signature value of the beginning of charging process.
        /// </summary>
        public static MeteringStatusType  Start       { get; }
            = Register("Start");

        /// <summary>
        /// An intermediate metering signature value of the charging process.
        /// Could also be: Progress1, Progress2, ... ProgressN
        /// </summary>
        /// <seealso cref="https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#signedmeteringvaluestype"/>
        public static MeteringStatusType  Progress    { get; }
            = Register("Progress");

        /// <summary>
        /// Metering Signature Value of the end of the charging process.
        /// </summary>
        public static MeteringStatusType  End         { get; }
            = Register("End");

        #endregion


        #region Operator overloading

        #region Operator == (MeteringStatusType1, MeteringStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeteringStatusType1">A metering status type.</param>
        /// <param name="MeteringStatusType2">Another metering status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (MeteringStatusType MeteringStatusType1,
                                           MeteringStatusType MeteringStatusType2)

            => MeteringStatusType1.Equals(MeteringStatusType2);

        #endregion

        #region Operator != (MeteringStatusType1, MeteringStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeteringStatusType1">A metering status type.</param>
        /// <param name="MeteringStatusType2">Another metering status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (MeteringStatusType MeteringStatusType1,
                                           MeteringStatusType MeteringStatusType2)

            => !MeteringStatusType1.Equals(MeteringStatusType2);

        #endregion

        #region Operator <  (MeteringStatusType1, MeteringStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeteringStatusType1">A metering status type.</param>
        /// <param name="MeteringStatusType2">Another metering status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (MeteringStatusType MeteringStatusType1,
                                          MeteringStatusType MeteringStatusType2)

            => MeteringStatusType1.CompareTo(MeteringStatusType2) < 0;

        #endregion

        #region Operator <= (MeteringStatusType1, MeteringStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeteringStatusType1">A metering status type.</param>
        /// <param name="MeteringStatusType2">Another metering status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (MeteringStatusType MeteringStatusType1,
                                           MeteringStatusType MeteringStatusType2)

            => MeteringStatusType1.CompareTo(MeteringStatusType2) <= 0;

        #endregion

        #region Operator >  (MeteringStatusType1, MeteringStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeteringStatusType1">A metering status type.</param>
        /// <param name="MeteringStatusType2">Another metering status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (MeteringStatusType MeteringStatusType1,
                                          MeteringStatusType MeteringStatusType2)

            => MeteringStatusType1.CompareTo(MeteringStatusType2) > 0;

        #endregion

        #region Operator >= (MeteringStatusType1, MeteringStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="MeteringStatusType1">A metering status type.</param>
        /// <param name="MeteringStatusType2">Another metering status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (MeteringStatusType MeteringStatusType1,
                                           MeteringStatusType MeteringStatusType2)

            => MeteringStatusType1.CompareTo(MeteringStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<MeteringStatusType> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two metering status types.
        /// </summary>
        /// <param name="Object">A metering status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is MeteringStatusType chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a metering status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(MeteringStatusType)

        /// <summary>
        /// Compares two metering status types.
        /// </summary>
        /// <param name="MeteringStatusType">A metering status type to compare with.</param>
        public Int32 CompareTo(MeteringStatusType MeteringStatusType)

            => String.Compare(InternalId,
                              MeteringStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<MeteringStatusType> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two metering status types for equality.
        /// </summary>
        /// <param name="Object">A metering status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is MeteringStatusType chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(MeteringStatusType)

        /// <summary>
        /// Compares two metering status types for equality.
        /// </summary>
        /// <param name="MeteringStatusType">A metering status type to compare with.</param>
        public Boolean Equals(MeteringStatusType MeteringStatusType)

            => String.Equals(InternalId,
                             MeteringStatusType.InternalId,
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
