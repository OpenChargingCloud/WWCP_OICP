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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The unique identification of an electric vehicle contract identification (EVCOId).
    /// </summary>
    public readonly struct EVCO_Id : IId<EVCO_Id>
    {

        #region Data

        private readonly String InternalId;

        /// <summary>
        /// The regular expression for parsing an electric vehicle contract identification.
        /// </summary>
        public static readonly Regex EVCOId_RegEx  = new Regex(@"^([A-Za-z]{2}\-?[A-Za-z0-9]{3})\-?C([A-Za-z0-9]{8})\-?([\d|A-Za-z])$|" +         // ISO
                                                               @"^([A-Za-z]{2}[\*|\-]?[A-Za-z0-9]{3})[\*|\-]?([A-Za-z0-9]{6})[\*|\-]?([\d|X])$",  // DIN
                                                               RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id  ProviderId    { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String       Suffix        { get; }

        /// <summary>
        /// An optional check digit of the electric vehicle contract identification.
        /// </summary>
        public Char?        CheckDigit    { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new electric vehicle contract identification.
        /// </summary>
        /// <param name="InternalId">The internal representation of the EVCO identification, as there are some many optional characters.</param>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric vehicle contract identification.</param>
        /// <param name="CheckDigit">An optional check digit of the electric vehicle contract identification.</param>
        private EVCO_Id(String       InternalId,
                        Provider_Id  ProviderId,
                        String       Suffix,
                        Char?        CheckDigit = null)
        {

            this.InternalId  = InternalId;
            this.ProviderId  = ProviderId;
            this.Suffix      = Suffix;
            this.CheckDigit  = CheckDigit;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text-representation of an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text-representation of an electric vehicle contract identification.</param>
        public static EVCO_Id Parse(String Text)
        {

            if (TryParse(Text, out EVCO_Id EVCOId))
                return EVCOId;

            throw new ArgumentException("Invalid text-representation of an electric vehicle contract identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse   (ProviderId, Suffix)

        /// <summary>
        /// Parse the given electric vehicle contract identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric vehicle contract identification.</param>
        public static EVCO_Id Parse(Provider_Id  ProviderId,
                                    String       Suffix)
        {

            #region Initial checks

            if (Suffix != null)
                Suffix = Suffix.Trim();

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given electric vehicle contract identification suffix must not be null or empty!");

            #endregion

            switch (ProviderId.Format)
            {

                case ProviderIdFormats.DIN:
                    return Parse(ProviderId +       Suffix);

                case ProviderIdFormats.DIN_STAR:
                    return Parse(ProviderId + "*" + Suffix);

                case ProviderIdFormats.DIN_HYPHEN:
                    return Parse(ProviderId + "-" + Suffix);


                case ProviderIdFormats.ISO:
                    return Parse(ProviderId +       Suffix);

                default: // ISO_HYPHEN
                    return Parse(ProviderId + "-" + Suffix);

            }

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text-representation of an electric vehicle contract identification.</param>
        public static EVCO_Id? TryParse(String Text)
        {

            if (TryParse(Text, out EVCO_Id EVCOId))
                return EVCOId;

            return default;

        }

        #endregion

        #region (static) TryParse(Text, out EVCOId)

        /// <summary>
        /// Try to parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text-representation of an electric vehicle contract identification.</param>
        /// <param name="EVCOId">The parsed electric vehicle contract identification.</param>
        public static Boolean TryParse(String Text, out EVCO_Id EVCOId)
        {

            #region Initial checks

            EVCOId  = default;
            Text    = Text?.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = EVCOId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;


                // ISO: DE-GDF-C12022187-X, DEGDFC12022187X
                if (Provider_Id.TryParse(matchCollection[0].Groups[1].Value, out Provider_Id providerId))
                {

                    EVCOId = new EVCO_Id(Text,
                                         providerId,
                                         matchCollection[0].Groups[2].Value,
                                         matchCollection[0].Groups[3].Value[0]);

                    return true;

                }


                // DIN: DE*GDF*0010LY*3, DE-GDF-0010LY-3, DEGDF0010LY3
                if (Provider_Id.TryParse(matchCollection[0].Groups[4].Value,  out providerId))
                {

                    if (providerId.Format == ProviderIdFormats.ISO_HYPHEN)
                        providerId = providerId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN);

                    EVCOId = new EVCO_Id(Text,
                                         providerId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
                                         matchCollection[0].Groups[5].Value,
                                         matchCollection[0].Groups[6].Value[0]);

                    return true;

                }

            }
            catch (Exception)
            { }

            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE identification.
        /// </summary>
        public EVCO_Id Clone

            => new EVCO_Id(new String(InternalId.ToCharArray()),
                           ProviderId.Clone,
                           Suffix);

        #endregion


        #region Operator overloading

        #region Operator == (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVCO_Id EVCOId1,
                                           EVCO_Id EVCOId2)

            => EVCOId1.Equals(EVCOId2);

        #endregion

        #region Operator != (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVCO_Id EVCOId1,
                                           EVCO_Id EVCOId2)

            => !EVCOId1.Equals(EVCOId2);

        #endregion

        #region Operator <  (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVCO_Id EVCOId1,
                                          EVCO_Id EVCOId2)

            => EVCOId1.CompareTo(EVCOId2) < 0;

        #endregion

        #region Operator <= (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVCO_Id EVCOId1,
                                           EVCO_Id EVCOId2)

            => EVCOId1.CompareTo(EVCOId2) <= 0;

        #endregion

        #region Operator >  (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVCO_Id EVCOId1,
                                          EVCO_Id EVCOId2)

            => EVCOId1.CompareTo(EVCOId2) > 0;

        #endregion

        #region Operator >= (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVCO_Id EVCOId1,
                                           EVCO_Id EVCOId2)

            => EVCOId1.CompareTo(EVCOId2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVCOId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is EVCO_Id EVCOId
                   ? CompareTo(EVCOId)
                   : throw new ArgumentException("The given object is not a EVCOId!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVCOId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId">An object to compare with.</param>
        public Int32 CompareTo(EVCO_Id EVCOId)

            => String.Compare(InternalId,
                              EVCOId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EVCOId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is EVCO_Id EVCOId &&
                   Equals(EVCOId);

        #endregion

        #region Equals(EVCOId)

        /// <summary>
        /// Compares two contract identifications for equality.
        /// </summary>
        /// <param name="EVCOId">A contract identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVCO_Id EVCOId)

            => String.Equals(InternalId,
                             EVCOId.InternalId,
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
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
