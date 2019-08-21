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
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The unique identification of an electric vehicle contract identification (EVCOId).
    /// </summary>
    public struct EVCO_Id : IId,
                            IEquatable<EVCO_Id>,
                            IComparable<EVCO_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an electric vehicle contract identification.
        /// </summary>
        public static readonly Regex EVCOId_RegEx  = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*([A-Za-z0-9]{6})\*([0-9|X])$ |"  +   // DIN STAR:  DE*BMW*0010LY*3
                                                               @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{6})-([0-9|X])$ |"     +   // DIN HYPEN: DE-BMW-0010LY-3
                                                               @"^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{6})([0-9|X])$ |"        +   // DIN:       DEBMW0010LY3

                                                             //@"^([A-Za-z]{2}\-?[A-Za-z0-9]{3})\-?C([A-Za-z0-9]{8})\-?([A-Za-z0-9]{1})$
                                                               @"^([A-Za-z]{2}-[A-Za-z0-9]{3})-C([A-Za-z0-9]{8})-([0-9|X])$ |"    +   // ISO Hypen: DE-BMW-C001000LY-3
                                                               @"^([A-Za-z]{2}[A-Za-z0-9]{3})C([A-Za-z0-9]{8})([0-9|X])$",            // ISO:       DEBMWC001000LY3

                                                               RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id  ProviderId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String       Suffix       { get; }

        /// <summary>
        /// An optional check digit of the electric vehicle contract identification.
        /// </summary>
        public Char?        CheckDigit   { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length

            => ProviderId.Length +
               1UL +
               (UInt64) Suffix.Length +
               (CheckDigit.HasValue ? 2UL : 0UL);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new electric vehicle contract identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the electric vehicle contract identification.</param>
        /// <param name="CheckDigit">An optional check digit of the electric vehicle contract identification.</param>
        private EVCO_Id(Provider_Id  ProviderId,
                        String       Suffix,
                        Char?        CheckDigit = null)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The identification suffix must not be null or empty!");

            #endregion

            this.ProviderId  = ProviderId;
            this.Suffix      = Suffix;
            this.CheckDigit  = CheckDigit;

        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given text representation of an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        public static EVCO_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The text representation of the electric vehicle contract identification must not be null or empty!");

            #endregion

            var _MatchCollection = EVCOId_RegEx.Matches(Text);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal electric vehicle contract identification '" + Text + "'!");

            Provider_Id _ProviderId;

            // DIN STAR:  DE*BMW*0010LY*3
            if (Provider_Id.TryParse(_MatchCollection[0].Groups[1].Value,  out _ProviderId))
                return new EVCO_Id(_ProviderId,
                                   _MatchCollection[0].Groups[2].Value,
                                   _MatchCollection[0].Groups[3].Value[0]);

            // DIN HYPEN: DE-BMW-0010LY-3
            if (Provider_Id.TryParse(_MatchCollection[0].Groups[4].Value,  out _ProviderId))
                return new EVCO_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
                                   _MatchCollection[0].Groups[5].Value,
                                   _MatchCollection[0].Groups[6].Value[0]);

            // DIN:       DEBMW0010LY3
            if (Provider_Id.TryParse(_MatchCollection[0].Groups[7].Value,  out _ProviderId))
                return new EVCO_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN),
                                   _MatchCollection[0].Groups[8].Value,
                                   _MatchCollection[0].Groups[9].Value[0]);


            // ISO Hypen: DE-BMW-C001000LY-3
            if (Provider_Id.TryParse(_MatchCollection[0].Groups[10].Value, out _ProviderId))
                return new EVCO_Id(_ProviderId,
                                   _MatchCollection[0].Groups[11].Value,
                                   _MatchCollection[0].Groups[12].Value[0]);

            // ISO:       DEBMWC001000LY3
            if (Provider_Id.TryParse(_MatchCollection[0].Groups[13].Value, out _ProviderId))
                return new EVCO_Id(_ProviderId.ChangeFormat(ProviderIdFormats.ISO_HYPHEN),
                                   _MatchCollection[0].Groups[14].Value,
                                   _MatchCollection[0].Groups[15].Value[0]);


            throw new ArgumentException("Illegal electric vehicle contract identification '" + Text + "'!");

        }

        #endregion

        #region (static) Parse(ProviderId, Suffix)

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
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        public static EVCO_Id? TryParse(String Text)
        {

            if (TryParse(Text, out EVCO_Id _EVCOId))
                return _EVCOId;

            return new EVCO_Id?();

        }

        #endregion

        #region (static) TryParse(JToken)

        /// <summary>
        /// Try to parse the given JSON token as a partner charging session identification.
        /// </summary>
        /// <param name="JToken">A JSON token representation of a partner charging session identification.</param>
        public static EVCO_Id? TryParse(JToken JToken)
            => TryParse(JToken?.Value<String>());

        #endregion

        #region (static) TryParse(Text, out EVCOId)

        /// <summary>
        /// Try to parse the given string as an electric vehicle contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle contract identification.</param>
        /// <param name="EVCOId">The parsed electric vehicle contract identification.</param>
        public static Boolean TryParse(String Text, out EVCO_Id EVCOId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                EVCOId = default(EVCO_Id);
                return false;
            }

            #endregion

            try
            {

                EVCOId = default(EVCO_Id);

                var _MatchCollection = EVCOId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                Provider_Id _ProviderId;

                #region DIN STAR:  DE*BMW*0010LY*3

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _ProviderId))
                {

                    EVCOId = new EVCO_Id(_ProviderId,
                                         _MatchCollection[0].Groups[2].Value,
                                         _MatchCollection[0].Groups[3].Value[0]);

                    return true;

                }

                #endregion

                #region DIN HYPEN: DE-BMW-0010LY-3

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[4].Value,  out _ProviderId))
                {

                    EVCOId = new EVCO_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN_HYPHEN),
                                         _MatchCollection[0].Groups[5].Value,
                                         _MatchCollection[0].Groups[6].Value[0]);

                    return true;

                }

                #endregion

                #region DIN:       DEBMW0010LY3

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[7].Value,  out _ProviderId))
                {

                    EVCOId = new EVCO_Id(_ProviderId.ChangeFormat(ProviderIdFormats.DIN),
                                         _MatchCollection[0].Groups[8].Value,
                                         _MatchCollection[0].Groups[9].Value[0]);

                    return true;

                }

                #endregion


                #region ISO Hypen: DE-BMW-C001000LY-3

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[10].Value, out _ProviderId))
                {

                    EVCOId = new EVCO_Id(_ProviderId,
                                         _MatchCollection[0].Groups[11].Value,
                                         _MatchCollection[0].Groups[12].Value[0]);

                    return true;

                }

                #endregion

                #region ISO:       DEBMWC001000LY3

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[13].Value, out _ProviderId))
                {

                    EVCOId = new EVCO_Id(_ProviderId.ChangeFormat(ProviderIdFormats.ISO_HYPHEN),
                                         _MatchCollection[0].Groups[14].Value,
                                         _MatchCollection[0].Groups[15].Value[0]);

                    return true;

                }

                #endregion

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            EVCOId = default(EVCO_Id);
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

        #region Operator == (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVCO_Id EVCOId1, EVCO_Id EVCOId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVCOId1, EVCOId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVCOId1 == null) || ((Object) EVCOId2 == null))
                return false;

            if ((Object) EVCOId1 == null)
                throw new ArgumentNullException(nameof(EVCOId1),  "The given contract identification must not be null!");

            return EVCOId1.Equals(EVCOId2);

        }

        #endregion

        #region Operator != (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVCO_Id EVCOId1, EVCO_Id EVCOId2)
            => !(EVCOId1 == EVCOId2);

        #endregion

        #region Operator <  (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVCO_Id EVCOId1, EVCO_Id EVCOId2)
        {

            if ((Object) EVCOId1 == null)
                throw new ArgumentNullException(nameof(EVCOId1),  "The given contract identification must not be null!");

            return EVCOId1.CompareTo(EVCOId2) < 0;

        }

        #endregion

        #region Operator <= (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVCO_Id EVCOId1, EVCO_Id EVCOId2)
            => !(EVCOId1 > EVCOId2);

        #endregion

        #region Operator >  (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVCO_Id EVCOId1, EVCO_Id EVCOId2)
        {

            if ((Object) EVCOId1 == null)
                throw new ArgumentNullException(nameof(EVCOId1),  "The given contract identification must not be null!");

            return EVCOId1.CompareTo(EVCOId2) > 0;

        }

        #endregion

        #region Operator >= (EVCOId1, EVCOId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId1">A contract identification.</param>
        /// <param name="EVCOId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVCO_Id EVCOId1, EVCO_Id EVCOId2)
            => !(EVCOId1 < EVCOId2);

        #endregion

        #endregion

        #region IComparable<EVCOId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is EVCO_Id))
                throw new ArgumentException("The given object is not a EVCOId!", nameof(Object));

            return CompareTo((EVCO_Id) Object);

        }

        #endregion

        #region CompareTo(EVCOId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCOId">An object to compare with.</param>
        public Int32 CompareTo(EVCO_Id EVCOId)
        {

            if ((Object) EVCOId == null)
                throw new ArgumentNullException(nameof(EVCOId),  "The given contract identification must not be null!");

            // Compare the length of the contract identifications
            var _Result = Length.CompareTo(EVCOId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = ProviderId.CompareTo(EVCOId.ProviderId);

            // If equal: Compare contract identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, EVCOId.Suffix, StringComparison.Ordinal);

            // If equal: Compare contract check digit
            if (_Result == 0)
            {

                if (!CheckDigit.HasValue && !EVCOId.CheckDigit.HasValue)
                    _Result = 0;

                if ( CheckDigit.HasValue &&  EVCOId.CheckDigit.HasValue)
                    _Result = CheckDigit.Value.CompareTo(EVCOId.CheckDigit.Value);

            }

            return _Result;

        }

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
        {

            if (Object == null)
                return false;

            if (!(Object is EVCO_Id))
                return false;

            return Equals((EVCO_Id) Object);

        }

        #endregion

        #region Equals(EVCOId)

        /// <summary>
        /// Compares two contract identifications for equality.
        /// </summary>
        /// <param name="EVCOId">A contract identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVCO_Id EVCOId)
        {

            if ((Object) EVCOId == null)
                return false;

            return ProviderId.Equals(EVCOId.ProviderId) &&
                   Suffix.    Equals(EVCOId.Suffix)     &&

                   ((!CheckDigit.HasValue && !EVCOId.CheckDigit.HasValue) ||
                     (CheckDigit.HasValue &&  EVCOId.CheckDigit.HasValue && CheckDigit.Value.Equals(EVCOId.CheckDigit.Value)));

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (ProviderId.Format)
            {

                case ProviderIdFormats.DIN:
                    return String.Concat(ProviderId,
                                         Suffix,
                                         CheckDigit.HasValue
                                             ? "" + CheckDigit
                                             : "");

                case ProviderIdFormats.DIN_STAR:
                    return String.Concat(ProviderId, "*",
                                         Suffix,
                                         CheckDigit.HasValue
                                             ? "*" + CheckDigit
                                             : "");

                case ProviderIdFormats.DIN_HYPHEN:
                    return String.Concat(ProviderId, "-",
                                         Suffix,
                                         CheckDigit.HasValue
                                             ? "-" + CheckDigit
                                             : "");


                case ProviderIdFormats.ISO:
                    return String.Concat(ProviderId,
                                         "C", Suffix,
                                         CheckDigit.HasValue
                                             ? "" + CheckDigit
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
