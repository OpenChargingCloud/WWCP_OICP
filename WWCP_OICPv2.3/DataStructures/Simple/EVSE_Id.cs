/*
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

using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extension methods for EVSE identifications.
    /// </summary>
    public static class EVSEIdExtensions
    {

        /// <summary>
        /// Indicates whether this EVSE identification is null or empty.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        public static Boolean IsNullOrEmpty(this EVSE_Id? EVSEId)
            => !EVSEId.HasValue || EVSEId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EVSE identification is null or empty.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        public static Boolean IsNotNullOrEmpty(this EVSE_Id? EVSEId)
            => EVSEId.HasValue && EVSEId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public readonly struct EVSE_Id : IId<EVSE_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// Official regular expression: ^(([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?E[A-Za-z0-9\*]{1,30})|(\+?[0-9]{1,3}\*[0-9]{3}\*[0-9\*]{1,32}))$
        /// </summary>
        /// <remarks>https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#EvseIDType</remarks>
        public static readonly Regex EVSEId_RegEx = new (@"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?E([A-Za-z0-9\*]{1,30})$ |" +
                                                         @"^(\+?[0-9]{1,3}\*[0-9]{3})\*([0-9\*]{1,32})$",
                                                         RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public Operator_Id        OperatorId   { get; }

        /// <summary>
        /// The suffix of the EVSE identification.
        /// </summary>
        public String             Suffix       { get; }

        /// <summary>
        /// The detected format of the EVSE identification.
        /// </summary>
        public OperatorIdFormats  Format
            => OperatorId.Format;

        /// <summary>
        /// Indicates whether this EVSE identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this EVSE identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (Format)
                {

                    case OperatorIdFormats.DIN:
                        return OperatorId.Length + 1 + (UInt64) (Suffix?.Length ?? 0);

                    case OperatorIdFormats.ISO_STAR:
                        return OperatorId.Length + 2 + (UInt64) (Suffix?.Length ?? 0);

                    default:  // ISO
                        return OperatorId.Length + 1 + (UInt64) (Suffix?.Length ?? 0);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the EVSE identification.</param>
        private EVSE_Id(Operator_Id  OperatorId,
                        String       Suffix)
        {

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The EVSE identification suffix must not be null or empty!");

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random  (OperatorId, Length = 12, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an EVSE.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The expected length of the EVSE identification suffix</param>
        /// <param name="Mapper">A delegate to modify the newly generated EVSE identification.</param>
        public static EVSE_Id Random(Operator_Id            OperatorId,
                                     Byte                   Length  = 12,
                                     Func<String, String>?  Mapper  = null)


            => new (OperatorId,
                    Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(Length))
                        :        RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        public static EVSE_Id Parse(String Text)
        {

            if (TryParse(Text, out EVSE_Id EVSEId))
                return EVSEId;

            throw new ArgumentException("Invalid text-representation of an e-mobility provider identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the EVSE identification.</param>
        public static EVSE_Id Parse(Operator_Id  OperatorId,
                                    String       Suffix)
        {

            #region Initial checks

            if (Suffix != null)
                Suffix = Suffix.Trim();

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given text-representation of an EVSE identification suffix must not be null or empty!");

            #endregion

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.DIN:
                    return Parse(OperatorId +  "*" + Suffix);

                case OperatorIdFormats.ISO:
                    return Parse(OperatorId +  "E" + Suffix);

                default: // ISO_STAR
                    return Parse(OperatorId + "*E" + Suffix);

            }

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        public static EVSE_Id? TryParse(String Text)
        {

            if (TryParse(Text, out EVSE_Id EVSEId))
                return EVSEId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EVSEId)

        /// <summary>
        /// Try to parse the given text-representation of an EVSE identification.
        /// </summary>
        /// <param name="Text">A text-representation of an EVSE identification.</param>
        /// <param name="EVSEId">The parsed EVSE identification.</param>
        public static Boolean TryParse(String Text, out EVSE_Id EVSEId)
        {

            if (Text.IsNotNullOrEmpty())
            {

                try
                {

                    var MatchCollection = EVSEId_RegEx.Matches(Text.Trim());

                    if (MatchCollection.Count == 1)
                    {

                        // New format...
                        if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out Operator_Id operatorId))
                        {

                            EVSEId = new EVSE_Id(operatorId,
                                                 MatchCollection[0].Groups[2].Value);

                            return true;

                        }

                        // Old format...
                        if (Operator_Id.TryParse(MatchCollection[0].Groups[3].Value, out operatorId))
                        {

                            EVSEId = new EVSE_Id(operatorId,
                                                 MatchCollection[0].Groups[4].Value);

                            return true;

                        }

                    }

                }
                catch
                { }

            }

            EVSEId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE identification.
        /// </summary>
        public EVSE_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.Equals(EVSEId2);

        #endregion

        #region Operator != (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !EVSEId1.Equals(EVSEId2);

        #endregion

        #region Operator <  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) < 0;

        #endregion

        #region Operator <= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) <= 0;

        #endregion

        #region Operator >  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) > 0;

        #endregion

        #region Operator >= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">An EVSE identification.</param>
        /// <param name="EVSEId2">Another EVSE identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => EVSEId1.CompareTo(EVSEId2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSE_Id EVSEId
                   ? CompareTo(EVSEId)
                   : throw new ArgumentException("The given object is not an EVSE identification!", nameof(Object));

        #endregion

        #region CompareTo(EVSEId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSEId)
        {

            var result = OperatorId.CompareTo(EVSEId.OperatorId);

            return result == 0
                       ? String.Compare(Suffix, EVSEId.Suffix, StringComparison.OrdinalIgnoreCase)
                       : result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is EVSE_Id EVSEId &&
                   Equals(EVSEId);

        #endregion

        #region Equals(EVSEId)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSEId)

            => OperatorId.Equals(EVSEId.OperatorId) &&
               String.Equals(Suffix, EVSEId.Suffix, StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
               Suffix.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (Format)
            {

                case OperatorIdFormats.DIN:
                    return String.Concat(OperatorId,  "*", Suffix);

                case OperatorIdFormats.ISO:
                    return String.Concat(OperatorId,  "E", Suffix);

                default: // ISO_STAR
                    return String.Concat(OperatorId, "*E", Suffix);

            }

        }

        #endregion

    }

}
