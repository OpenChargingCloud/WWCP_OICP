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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An additional reference for pricing information.
    /// </summary>
    public class AdditionalReferences : IEquatable<AdditionalReferences>
    {

        #region Properties

        /// <summary>
        /// Additional pricing components to be considered in addition to the base pricing.
        /// </summary>
        [Mandatory]
        public Additional_Reference  AdditionalReference                { get; }

        /// <summary>
        /// Additional reference units that can be used in defining pricing products.
        /// </summary>
        [Mandatory]
        public Reference_Unit        AdditionalReferenceUnit            { get; }

        /// <summary>
        /// A price in the given currency.
        /// </summary>
        [Mandatory]
        public Decimal               PricePerAdditionalReferenceUnit    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy source.
        /// </summary>
        /// <param name="AdditionalReference">Additional pricing components to be considered in addition to the base pricing.</param>
        /// <param name="AdditionalReferenceUnit">Additional reference units that can be used in defining pricing products.</param>
        /// <param name="PricePerAdditionalReferenceUnit">A price in the given currency.</param>
        public AdditionalReferences(Additional_Reference  AdditionalReference,
                                    Reference_Unit        AdditionalReferenceUnit,
                                    Decimal               PricePerAdditionalReferenceUnit)
        {

            this.AdditionalReference              = AdditionalReference;
            this.AdditionalReferenceUnit          = AdditionalReferenceUnit;
            this.PricePerAdditionalReferenceUnit  = PricePerAdditionalReferenceUnit;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#128-additionalreferencestype

        // {
        //     "AdditionalReference":               "PARKING FEE",
        //     "AdditionalReferenceUnit":           "HOUR",
        //     "PricePerAdditionalReferenceUnit":    2
        // }

        #endregion

        #region (static) Parse   (JSON, CustomAdditionalReferencesParser = null)

        /// <summary>
        /// Parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static AdditionalReferences Parse(JObject                                             JSON,
                                                 CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser   = null)
        {

            if (TryParse(JSON,
                         out AdditionalReferences?  additionalReferences,
                         out String?                errorResponse,
                         CustomAdditionalReferencesParser))
            {
                return additionalReferences!;
            }

            throw new ArgumentException("The given JSON representation of an energy source is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAdditionalReferencesParser = null)

        /// <summary>
        /// Parse the given text representation of an energy source.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static AdditionalReferences Parse(String                                              Text,
                                                 CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser   = null)
        {

            if (TryParse(Text,
                         out AdditionalReferences?  additionalReferences,
                         out String?                errorResponse,
                         CustomAdditionalReferencesParser))
            {
                return additionalReferences!;
            }

            throw new ArgumentException("The given text representation of an energy source is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, CustomAdditionalReferencesParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static AdditionalReferences? TryParse(JObject                                             JSON,
                                                     CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser   = null)
        {

            if (TryParse(JSON,
                         out AdditionalReferences? additionalReferences,
                         out _,
                         CustomAdditionalReferencesParser))
            {
                return additionalReferences;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(Text, CustomAdditionalReferencesParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given text representation of an energy source.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static AdditionalReferences? TryParse(String                                              Text,
                                                     CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser   = null)
        {

            if (TryParse(Text,
                         out AdditionalReferences? additionalReferences,
                         out _,
                         CustomAdditionalReferencesParser))
            {
                return additionalReferences;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out AdditionalReferences, out ErrorResponse, CustomAdditionalReferencesParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AdditionalReferences">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                    JSON,
                                       out AdditionalReferences?  AdditionalReferences,
                                       out String?                ErrorResponse)

            => TryParse(JSON,
                        out AdditionalReferences,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an energy source.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AdditionalReferences">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       out AdditionalReferences?                           AdditionalReferences,
                                       out String?                                         ErrorResponse,
                                       CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser)
        {

            try
            {

                AdditionalReferences = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse AdditionalReference        [mandatory]

                if (!JSON.ParseMandatory("AdditionalReference",
                                         "additional reference",
                                         Additional_Reference.TryParse,
                                         out Additional_Reference AdditionalReference,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse AdditionalReferenceUnit    [mandatory]

                if (!JSON.ParseMandatory("AdditionalReferenceUnit",
                                         "additional reference unit",
                                         Reference_Unit.TryParse,
                                         out Reference_Unit AdditionalReferenceUnit,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Percentage                 [mandatory]

                if (JSON.ParseMandatory("PricePerAdditionalReferenceUnit",
                                        "price per additional referenceUnit",
                                        out Decimal PricePerAdditionalReferenceUnit,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                AdditionalReferences = new AdditionalReferences(AdditionalReference,
                                                                AdditionalReferenceUnit,
                                                                PricePerAdditionalReferenceUnit);


                if (CustomAdditionalReferencesParser is not null)
                    AdditionalReferences = CustomAdditionalReferencesParser(JSON,
                                                                            AdditionalReferences);

                return true;

            }
            catch (Exception e)
            {
                AdditionalReferences  = default;
                ErrorResponse         = "The given JSON representation of an energy source is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out AdditionalReferences, out ErrorResponse, CustomAdditionalReferencesParser = null)

        /// <summary>
        /// Try to parse the given text representation of an energy source.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="AdditionalReferences">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static Boolean TryParse(String                                              Text,
                                       out AdditionalReferences?                           AdditionalReferences,
                                       out String?                                         ErrorResponse,
                                       CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser = null)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out AdditionalReferences,
                                out ErrorResponse,
                                CustomAdditionalReferencesParser);

            }
            catch (Exception e)
            {
                AdditionalReferences  = default;
                ErrorResponse         = "The given text representation of an energy source is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAdditionalReferencesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAdditionalReferencesSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AdditionalReferences>?  CustomAdditionalReferencesSerializer   = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("AdditionalReference",              AdditionalReference.    ToString()),
                           new JProperty("AdditionalReferenceUnit",          AdditionalReferenceUnit.ToString()),
                           new JProperty("PricePerAdditionalReferenceUnit",  PricePerAdditionalReferenceUnit)
                       );

            return CustomAdditionalReferencesSerializer is not null
                       ? CustomAdditionalReferencesSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public AdditionalReferences Clone

            => new (AdditionalReference,
                    AdditionalReferenceUnit,
                    PricePerAdditionalReferenceUnit);

        #endregion


        #region Operator overloading

        #region Operator == (AdditionalReferences1, AdditionalReferences2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReferences1">An energy source.</param>
        /// <param name="AdditionalReferences2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (AdditionalReferences AdditionalReferences1,
                                           AdditionalReferences AdditionalReferences2)

            => AdditionalReferences1.Equals(AdditionalReferences2);

        #endregion

        #region Operator != (AdditionalReferences1, AdditionalReferences2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReferences1">An energy source.</param>
        /// <param name="AdditionalReferences2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (AdditionalReferences AdditionalReferences1,
                                           AdditionalReferences AdditionalReferences2)

            => !AdditionalReferences1.Equals(AdditionalReferences2);

        #endregion

        #endregion

        #region IEquatable<AdditionalReferences> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is AdditionalReferences additionalReferences &&
                   Equals(additionalReferences);

        #endregion

        #region Equals(AdditionalReferences)

        /// <summary>
        /// Compares two AdditionalReferencess for equality.
        /// </summary>
        /// <param name="AdditionalReferences">An energy source to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AdditionalReferences? AdditionalReferences)

            => AdditionalReferences is not null &&

               AdditionalReferences.           Equals(AdditionalReferences.AdditionalReference)     &&
               AdditionalReferenceUnit.        Equals(AdditionalReferences.AdditionalReferenceUnit) &&
               PricePerAdditionalReferenceUnit.Equals(AdditionalReferences.PricePerAdditionalReferenceUnit);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return AdditionalReference.            GetHashCode() * 7 ^
                       AdditionalReferenceUnit.        GetHashCode() * 5 ^
                       PricePerAdditionalReferenceUnit.GetHashCode() * 3;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(AdditionalReference,     ", ",
                             AdditionalReferenceUnit, ", ",
                             PricePerAdditionalReferenceUnit);

        #endregion

    }

}
