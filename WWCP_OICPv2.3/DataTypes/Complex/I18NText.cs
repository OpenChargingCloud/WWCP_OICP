﻿/*
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
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extention methods for internationalized (I18N) multi-language text.
    /// </summary>
    public static class I18NTextExtentions
    {

        /// <summary>
        /// The multi-language string is empty.
        /// </summary>
        public static Boolean IsNullOrEmpty(this I18NText Text)
            => Text == null || !Text.Any();

        /// <summary>
        /// The multi-language string is neither null nor empty.
        /// </summary>
        public static Boolean IsNeitherNullNorEmpty(this I18NText Text)
            => Text != null && Text.Any();

        /// <summary>
        /// Return the first string of a multi-language string.
        /// </summary>
        public static String FirstText(this I18NText Text)
            => Text != null && Text.Any()
                   ? Text.First().Value
                   : null;


        #region SubstringMax(this I18NText, Length)

        /// <summary>
        /// Return a substring of the given maximum length.
        /// </summary>
        /// <param name="I18NText">A text.</param>
        /// <param name="Length">The maximum length of the substring.</param>
        public static I18NText SubstringMax(this I18NText I18NText, Int32 Length)
        {

            if (I18NText == null)
                return null;

            return new I18NText(I18NText.Select(text => new KeyValuePair<LanguageCode, String>(
                                                            text.Key,
                                                            text.Value.Substring(0, Math.Min(text.Value.Length, Length))
                                                        )));

        }

        #endregion

        #region TrimAll     (this I18NText)

        /// <summary>
        /// Trim all texts.
        /// </summary>
        /// <param name="I18NText">A text.</param>
        public static I18NText TrimAll(this I18NText I18NText)
        {

            if (I18NText == null)
                return null;

            return new I18NText(I18NText.Select(text => new KeyValuePair<LanguageCode, String>(
                                                            text.Key,
                                                            text.Value?.Trim()
                                                        )));

        }

        #endregion


        #region ToI18NText(this Text, Language = LanguageCode.en)

        /// <summary>
        /// Convert the given String into a multi-language text.
        /// </summary>
        /// <param name="Text">A string.</param>
        /// <param name="Language">A language.</param>
        public static I18NText ToI18NText(this String    Text,
                                          LanguageCode?  Language = null)

            => Text.IsNotNullOrEmpty()
                   ? I18NText.Create(Language ?? LanguageCode.en, Text)
                   : null;

        #endregion

    }

    /// <summary>
    /// An internationalized (I18N) multi-language text.
    /// </summary>
    public class I18NText : IEquatable<I18NText>,
                            IEnumerable<KeyValuePair<LanguageCode, String>>
    {

        #region Data

        private readonly Dictionary<LanguageCode, String> I18NTexts;

        #endregion

        #region Constructor(s)

        #region I18NText()

        /// <summary>
        /// Create a new internationalized (I18N) multi-language string.
        /// </summary>
        public I18NText()
        {
            this.I18NTexts = new Dictionary<LanguageCode, String>();
        }

        #endregion

        #region I18NText(Language, Text)

        /// <summary>
        /// Create a new internationalized (I18N) multi-language string
        /// based on the given language and string.
        /// </summary>
        /// <param name="Language">The internationalized (I18N) language.</param>
        /// <param name="Text">The internationalized (I18N) text.</param>
        public I18NText(LanguageCode Language, String Text)
            : this()
        {
            I18NTexts.Add(Language, Text);
        }

        #endregion

        #region I18NText(Texts)

        /// <summary>
        /// Create a new internationalized (I18N) multi-language string
        /// based on the given language and string pairs.
        /// </summary>
        public I18NText(IEnumerable<KeyValuePair<LanguageCode, String>> Texts)
            : this()
        {

            if (Texts != null)
                foreach (var Text in Texts)
                    I18NTexts.Add(Text.Key, Text.Value);

        }

        #endregion

        //#region I18NText(I18NPairs)

        ///// <summary>
        ///// Create a new internationalized (I18N) multi-language string
        ///// based on the given I18N-pairs.
        ///// </summary>
        //public I18NText(IEnumerable<I18NPair> I18NPairs)
        //    : this()
        //{

        //    if (I18NPairs != null)
        //        foreach (var Text in I18NPairs)
        //            I18NTexts.Add(Text.Language, Text.Text);

        //}

        //#endregion

        //#region I18NText(params I18NPairs)

        ///// <summary>
        ///// Create a new internationalized (I18N) multi-language string
        ///// based on the given I18N-pairs.
        ///// </summary>
        //public I18NText(params I18NPair[] I18NPairs)
        //    : this()
        //{

        //    if (I18NPairs != null)
        //        foreach (var Text in I18NPairs)
        //            I18NTexts.Add(Text.Language, Text.Text);

        //}

        //#endregion

        #endregion



        #region (static) Empty

        /// <summary>
        /// Create an empty internationalized (I18N) multi-language string.
        /// </summary>
        public static I18NText Empty
            => new I18NText();

        #endregion

        #region (static) Create(Language, Text)

        /// <summary>
        /// Create a new internationalized (I18N) multi-language string
        /// based on the given language and string.
        /// </summary>
        /// <param name="Language">The internationalized (I18N) language.</param>
        /// <param name="Text">The internationalized (I18N) text.</param>
        public static I18NText Create(LanguageCode  Language,
                                      String     Text)

            => new I18NText(Language, Text);

        #endregion

        #region has(Language)

        /// <summary>
        /// Checks if the given language representation exists.
        /// </summary>
        /// <param name="Language">The internationalized (I18N) language.</param>
        public Boolean has(LanguageCode Language)

            => I18NTexts.ContainsKey(Language);

        #endregion

        #region this[Language]

        /// <summary>
        /// Get the text specified by the given language.
        /// </summary>
        /// <param name="Language">The internationalized (I18N) language.</param>
        /// <returns>The internationalized (I18N) text or String.Empty</returns>
        public String this[LanguageCode Language]
        {

            get
            {


                if (I18NTexts.TryGetValue(Language, out String Text))
                    return Text;

                return String.Empty;

            }

            set
            {
                I18NTexts[Language] = value;
            }

        }

        #endregion


        #region (static) Parse   (Text)

        public static I18NText Parse(String Text)
        {

            String ErrorResponse;

            try
            {

                if (TryParse(JObject.Parse(Text), out I18NText I18NText, out ErrorResponse))
                    return I18NText;

            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not parse the given JSON representation of an internationalized (I18N) multi-language text: " + e.Message);
            }

            throw new ArgumentException("The given text representation of an internationalized (I18N) multi-language text is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) Parse   (JSON)

        public static I18NText Parse(JObject JSON)
        {

            if (TryParse(JSON, out I18NText i18NText, out String ErrorResponse))
                return i18NText;

            throw new ArgumentException("The given JSON representation of an internationalized (I18N) multi-language text is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) TryParse(Text, out I18NText, out ErrorResponse)

        /// <summary>
        /// Try to parse the given text representation of a internationalized (I18N) multi-language text.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="I18NText">The parsed internationalized (I18N) multi-language text.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(String        Text,
                                       out I18NText  I18NText,
                                       out String    ErrorResponse)
        {

            JObject JSON;

            try
            {
                JSON = JObject.Parse(Text);
            }
            catch (Exception e)
            {
                ErrorResponse  = "Could not parse the given internationalized (I18N) multi-language text: " + e.Message;
                I18NText       = null;
                return false;
            }

            if (TryParse(JSON, out I18NText, out ErrorResponse))
                return true;

            return false;

        }

        #endregion

        #region (static) TryParse(JSON, out I18NText, out ErrorResponse)

        /// <summary>
        /// Try to parse the given JSON representation of a internationalized (I18N) multi-language text.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="I18NText">The parsed internationalized (I18N) multi-language text.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject       JSON,
                                       out I18NText  I18NText,
                                       out String    ErrorResponse)
        {

            if (JSON?.HasValues == true)
            {

                var i18NText = new Dictionary<LanguageCode, String>();

                foreach (var JSONProperty in JSON)
                {

                    try
                    {

                        if (LanguageCode.TryParse(JSONProperty.Key, out LanguageCode language))
                        {
                            i18NText.Add(language,
                                         JSONProperty.Value?.Value<String>()?.Trim());
                        }

                        I18NText = new I18NText(i18NText);

                    }
                    catch (Exception e)
                    {
                        ErrorResponse  = "Could not parse the given internationalized (I18N) multi-language text: " + e.Message;
                        I18NText       = null;
                        return false;
                    }

                }

            }

            ErrorResponse  = default;
            I18NText       = new I18NText();
            return true;

        }

        #endregion

        #region ToJSON()

        /// <summary>
        /// Return a JSON representation of the given internationalized string.
        /// </summary>
        public JArray ToJSON()

            => I18NTexts.SafeAny()
                   ? new JArray(I18NTexts.SafeSelect(i18n => new JObject(
                                                                 new JProperty("lang",  i18n.Key.ToString()),
                                                                 new JProperty("value", i18n.Value)
                                                             )))
                   : null;

        #endregion

        #region Clone

        /// <summary>
        /// Clone this multi-language text.
        /// </summary>
        public I18NText Clone

            => new I18NText(I18NTexts.SafeSelect(i18n => new KeyValuePair<LanguageCode, String>(
                                                             i18n.Key,
                                                             new String(i18n.Value.ToCharArray())
                                                         )
                                                 ));

        #endregion


        public Boolean Is(LanguageCode  Language,
                          String        Value)
        {

            if (!I18NTexts.ContainsKey(Language))
                return false;

            return I18NTexts[Language].Equals(Value);

        }

        public Boolean IsNot(LanguageCode  Language,
                             String        Value)
        {

            if (!I18NTexts.ContainsKey(Language))
                return true;

            return !I18NTexts[Language].Equals(Value);

        }


        public Boolean Matches(String Match, Boolean IgnoreCase = false)

            => I18NTexts.Any(kvp => IgnoreCase
                                          ? kvp.Value.IndexOf(Match, StringComparison.OrdinalIgnoreCase) >= 0
                                          : kvp.Value.IndexOf(Match) >= 0);


        #region GetEnumerator()

        /// <summary>
        /// Enumerate all internationalized (I18N) texts.
        /// </summary>
        public IEnumerator<KeyValuePair<LanguageCode, String>> GetEnumerator()
            => I18NTexts.Select(kvp => new KeyValuePair<LanguageCode, String>(kvp.Key, kvp.Value)).GetEnumerator();

        /// <summary>
        /// Enumerate all internationalized (I18N) texts.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => I18NTexts.Select(kvp => new KeyValuePair<LanguageCode, String>(kvp.Key, kvp.Value)).GetEnumerator();

        #endregion


        #region Operator overloading

        #region Operator == (I18NText1, I18NText2)

        /// <summary>
        /// Compares two I18N-strings for equality.
        /// </summary>
        /// <param name="I18NText1">A I18N-string.</param>
        /// <param name="I18NText2">Another I18N-string.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (I18NText I18NText1,
                                           I18NText I18NText2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(I18NText1, I18NText2))
                return true;

            // If one is null, but not both, return false.
            if (I18NText1 is null || I18NText2 is null)
                return false;

            return I18NText1.Equals(I18NText2);

        }

        #endregion

        #region Operator != (I18NText1, I18NText2)

        /// <summary>
        /// Compares two I18N-strings for inequality.
        /// </summary>
        /// <param name="I18NText1">A I18N-string.</param>
        /// <param name="I18NText2">Another I18N-string.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (I18NText I18NText1,
                                           I18NText I18NText2)

            => !(I18NText1 == I18NText2);

        #endregion

        #endregion

        #region IEquatable<I18NText> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is I18NText i18NText &&
                   Equals(i18NText);

        #endregion

        #region Equals(I18NText)

        /// <summary>
        /// Compares two I18NText for equality.
        /// </summary>
        /// <param name="I18NText">An I18NText to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(I18NText I18NText)
        {

            if (I18NText is null)
                return false;

            if (I18NTexts.Count != I18NText.Count())
                return false;

            foreach (var I18N in I18NTexts)
            {
                if (I18N.Value != I18NText[I18N.Key])
                    return false;
            }

            return true;

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {

            Int32 ReturnValue = 0;

            foreach (var Value in I18NTexts.
                                      Select(I18N => I18N.Key.GetHashCode() ^ I18N.Value.GetHashCode()))
            {
                ReturnValue ^= Value;
            }

            return ReturnValue;

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (I18NTexts.Count == 0)
                return String.Empty;

            return I18NTexts.
                       Select(I18N => I18N.Key.ToString() + ": " + I18N.Value).
                       AggregateWith("; ");

        }

        #endregion

    }

}
