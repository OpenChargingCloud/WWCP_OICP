/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Contract identification (EVCOId).
    /// </summary>
    public struct EVCO_Id : IId,
                            IEquatable<EVCO_Id>,
                            IComparable<EVCO_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an electric vehicle contract identification.
        /// </summary>
        public static readonly Regex EVCOId_RegEx  = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*([A-Za-z0-9]{6})\*([0-9|X])$ |"  +   // Hubject DIN STAR:  DE*BMW*0010LY*3
                                                               @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{6})-([0-9|X])$ |"     +   // Hubject DIN HYPEN: DE-BMW-0010LY-3
                                                               @"^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{6})([0-9|X])$ |"        +   // Hubject DIN:       DEBMW0010LY3

                                                               @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-C([A-Za-z0-9]{8})-([0-9|X])$ |"    +   // Hubject ISO Hypen: DE-BMW-001000LY-3
                                                               @"^([A-Za-z]{2}[A-Za-z0-9]{3})C([A-Za-z0-9]{8})([0-9|X])$",            // Hubject ISO:       DEBMW001000LY3

                                                               RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an electric vehicle contract identification suffix.
        /// </summary>
        public static readonly Regex IdSuffix_RegEx  = new Regex(@"^[A-Za-z0-9]{9}-[A-Za-z0-9]$ | ^[A-Za-z0-9]{9}[A-Za-z0-9]$ | ^[A-Za-z0-9]{9}$",
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
        public String                Suffix        { get; }

        /// <summary>
        /// An optional check digit of the electric vehicle contract identification.
        /// </summary>
        public Char?                 CheckDigit    { get; }

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length

            => ProviderId.Length +
               1UL +
               (UInt64) Suffix.Length +
               (CheckDigit.HasValue ? 2UL : 0UL);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric vehicle contract identification
        /// based on the given string.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="IdSuffix">The suffix of the electric vehicle contract identification.</param>
        /// <param name="CheckDigit">An optional check digit of the electric vehicle contract identification.</param>
        private EVCO_Id(Provider_Id  ProviderId,
                        String                IdSuffix,
                        Char?                 CheckDigit = null)
        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),  "The identification suffix must not be null or empty!");

            #endregion

            this.ProviderId  = ProviderId;
            this.Suffix      = IdSuffix;
            this.CheckDigit  = CheckDigit;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a contract identification.
        /// </summary>
        public static EVCO_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = EVCOId_RegEx.Matches(Text);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal contract identification '" + Text + "'!");

            Provider_Id _ProviderId;

            if (Provider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _ProviderId))
                return new EVCO_Id(_ProviderId,
                                       _MatchCollection[0].Groups[2].Value,
                                       _MatchCollection[0].Groups[3].Value[0]);

            if (Provider_Id.TryParse(_MatchCollection[0].Groups[4].Value, out _ProviderId))
                return new EVCO_Id(_ProviderId,
                                       _MatchCollection[0].Groups[5].Value,
                                       _MatchCollection[0].Groups[6].Value[0]);

            if (Provider_Id.TryParse(_MatchCollection[0].Groups[7].Value, out _ProviderId))
                return new EVCO_Id(_ProviderId,
                                       _MatchCollection[0].Groups[8].Value,
                                       _MatchCollection[0].Groups[9].Value[0]);

            if (Provider_Id.TryParse(_MatchCollection[0].Groups[10].Value, out _ProviderId))
                return new EVCO_Id(_ProviderId,
                                       _MatchCollection[0].Groups[11].Value);


            throw new ArgumentException("Illegal contract identification '" + Text + "'!");

        }

        #endregion

        #region Parse(ProviderId, IdSuffix)

        /// <summary>
        /// Parse the given string as an contract identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="IdSuffix">The suffix of the electric vehicle contract identification.</param>
        public static EVCO_Id Parse(Provider_Id  ProviderId,
                                                String                IdSuffix)

            => new EVCO_Id(ProviderId,
                                       IdSuffix);

        #endregion

        #region TryParse(Text, out eMobilityAccountId)

        /// <summary>
        /// Parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        /// <param name="eMobilityAccountId">The parsed electric vehicle contract identification.</param>
        public static Boolean TryParse(String Text, out EVCO_Id eMobilityAccountId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                eMobilityAccountId = default(EVCO_Id);
                return false;
            }

            #endregion

            try
            {

                eMobilityAccountId = default(EVCO_Id);

                var _MatchCollection = EVCOId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                Provider_Id _Provider;

                // ISO...
                if (Provider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _Provider))
                {

                    eMobilityAccountId = new EVCO_Id(_Provider,
                                                                 _MatchCollection[0].Groups[2].Value,
                                                                 _MatchCollection[0].Groups[3].Value[0]);

                    return true;

                }

                // DIN...
                if (Provider_Id.TryParse(_MatchCollection[0].Groups[4].Value, out _Provider))
                {

                    eMobilityAccountId = new EVCO_Id(_Provider,
                                                                 _MatchCollection[0].Groups[5].Value,
                                                                 _MatchCollection[0].Groups[6].Value[0]);

                    return true;

                }

                // Without check digit...
                if (Provider_Id.TryParse(_MatchCollection[0].Groups[7].Value, out _Provider))
                {

                    eMobilityAccountId = new EVCO_Id(_Provider,
                                                 _MatchCollection[0].Groups[8].Value);

                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            eMobilityAccountId = default(EVCO_Id);
            return false;

        }


        /// <summary>
        /// Parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        /// <param name="eMobilityAccountId">The parsed electric vehicle contract identification.</param>
        public static Boolean TryParse(String Text, out EVCO_Id? eMobilityAccountId)
        {

            EVCO_Id eMAId;

            if (TryParse(Text, out eMAId))
            {
                eMobilityAccountId = eMAId;
                return true;
            }

            eMobilityAccountId = new EVCO_Id?();

            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE identification.
        /// </summary>
        public EVCO_Id Clone

            => new EVCO_Id(ProviderId.Clone,
                                       Suffix);

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVCO_Id eMobilityAccountId1, EVCO_Id eMobilityAccountId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(eMobilityAccountId1, eMobilityAccountId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMobilityAccountId1 == null) || ((Object) eMobilityAccountId2 == null))
                return false;

            if ((Object) eMobilityAccountId1 == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId1),  "The given contract identification must not be null!");

            return eMobilityAccountId1.Equals(eMobilityAccountId2);

        }

        #endregion

        #region Operator != (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVCO_Id eMobilityAccountId1, EVCO_Id eMobilityAccountId2)
            => !(eMobilityAccountId1 == eMobilityAccountId2);

        #endregion

        #region Operator <  (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVCO_Id eMobilityAccountId1, EVCO_Id eMobilityAccountId2)
        {

            if ((Object) eMobilityAccountId1 == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId1),  "The given contract identification must not be null!");

            return eMobilityAccountId1.CompareTo(eMobilityAccountId2) < 0;

        }

        #endregion

        #region Operator <= (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVCO_Id eMobilityAccountId1, EVCO_Id eMobilityAccountId2)
            => !(eMobilityAccountId1 > eMobilityAccountId2);

        #endregion

        #region Operator >  (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVCO_Id eMobilityAccountId1, EVCO_Id eMobilityAccountId2)
        {

            if ((Object) eMobilityAccountId1 == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId1),  "The given contract identification must not be null!");

            return eMobilityAccountId1.CompareTo(eMobilityAccountId2) > 0;

        }

        #endregion

        #region Operator >= (eMobilityAccountId1, eMobilityAccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId1">A contract identification.</param>
        /// <param name="eMobilityAccountId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVCO_Id eMobilityAccountId1, EVCO_Id eMobilityAccountId2)
            => !(eMobilityAccountId1 < eMobilityAccountId2);

        #endregion

        #endregion

        #region IComparable<eMobilityAccountId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a contract identification.
            if (!(Object is EVCO_Id))
                throw new ArgumentException("The given object is not a eMobilityAccountId!", nameof(Object));

            return CompareTo((EVCO_Id) Object);

        }

        #endregion

        #region CompareTo(eMobilityAccountId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccountId">An object to compare with.</param>
        public Int32 CompareTo(EVCO_Id eMobilityAccountId)
        {

            if ((Object) eMobilityAccountId == null)
                throw new ArgumentNullException(nameof(eMobilityAccountId),  "The given contract identification must not be null!");

            // Compare the length of the contract identifications
            var _Result = this.Length.CompareTo(eMobilityAccountId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = ProviderId.CompareTo(eMobilityAccountId.ProviderId);

            // If equal: Compare contract identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, eMobilityAccountId.Suffix, StringComparison.Ordinal);

            // If equal: Compare contract check digit
            if (_Result == 0)
            {

                if (!CheckDigit.HasValue && !eMobilityAccountId.CheckDigit.HasValue)
                    _Result = 0;

                if ( CheckDigit.HasValue &&  eMobilityAccountId.CheckDigit.HasValue)
                    _Result = CheckDigit.Value.CompareTo(eMobilityAccountId.CheckDigit.Value);

            }

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityAccountId> Members

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

            // Check if the given object is a contract identification.
            if (!(Object is EVCO_Id))
                return false;

            return this.Equals((EVCO_Id) Object);

        }

        #endregion

        #region Equals(eMobilityAccountId)

        /// <summary>
        /// Compares two contract identifications for equality.
        /// </summary>
        /// <param name="eMobilityAccountId">A contract identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVCO_Id eMobilityAccountId)
        {

            if ((Object) eMobilityAccountId == null)
                return false;

            return ProviderId.Equals(eMobilityAccountId.ProviderId) &&
                   Suffix.    Equals(eMobilityAccountId.Suffix)     &&

                   ((!CheckDigit.HasValue && !eMobilityAccountId.CheckDigit.HasValue) ||
                     (CheckDigit.HasValue &&  eMobilityAccountId.CheckDigit.HasValue && CheckDigit.Value.Equals(eMobilityAccountId.CheckDigit.Value)));

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return ProviderId.GetHashCode() * 7 ^
                       Suffix.    GetHashCode() * 5 ^

                       (CheckDigit.HasValue
                            ? CheckDigit.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (ProviderId.Format)
            {

                case ProviderIdFormats.DIN_STAR:
                    return String.Concat(ProviderId, "*",
                                         Suffix,
                                         CheckDigit.HasValue
                                             ? "*" + CheckDigit
                                             : "");

                default: // ISO_HYPHEN
                    return String.Concat(ProviderId, "-",
                                         "C", Suffix,
                                         CheckDigit.HasValue
                                             ? "-" + CheckDigit
                                             : "");

            }

        }

        #endregion

    }

}
