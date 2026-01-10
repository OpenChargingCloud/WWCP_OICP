/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

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
        /// Create a new additional reference.
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

            unchecked
            {

                hashCode = this.AdditionalReference.            GetHashCode() * 5 ^
                           this.AdditionalReferenceUnit.        GetHashCode() * 3 ^
                           this.PricePerAdditionalReferenceUnit.GetHashCode();

            }

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
        /// Parse the given JSON representation of an additional reference.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom additional references JSON objects.</param>
        public static AdditionalReferences Parse(JObject                                             JSON,
                                                 CustomJObjectParserDelegate<AdditionalReferences>?  CustomAdditionalReferencesParser   = null)
        {

            if (TryParse(JSON,
                         out var additionalReferences,
                         out var errorResponse,
                         CustomAdditionalReferencesParser))
            {
                return additionalReferences;
            }

            throw new ArgumentException("The given JSON representation of an additional reference is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out AdditionalReferences, out ErrorResponse, CustomAdditionalReferencesParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an additional reference.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AdditionalReferences">The parsed additional reference.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                         JSON,
                                       [NotNullWhen(true)]  out AdditionalReferences?  AdditionalReferences,
                                       [NotNullWhen(false)] out String?                ErrorResponse)

            => TryParse(JSON,
                        out AdditionalReferences,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an additional reference.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AdditionalReferences">The parsed additional reference.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAdditionalReferencesParser">A delegate to parse custom additional references JSON objects.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       [NotNullWhen(true)]  out AdditionalReferences?      AdditionalReferences,
                                       [NotNullWhen(false)] out String?                    ErrorResponse,
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


                AdditionalReferences = new AdditionalReferences(
                                           AdditionalReference,
                                           AdditionalReferenceUnit,
                                           PricePerAdditionalReferenceUnit
                                       );


                if (CustomAdditionalReferencesParser is not null)
                    AdditionalReferences = CustomAdditionalReferencesParser(JSON,
                                                                            AdditionalReferences);

                return true;

            }
            catch (Exception e)
            {
                AdditionalReferences  = default;
                ErrorResponse         = "The given JSON representation of an additional reference is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAdditionalReferencesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAdditionalReferencesSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AdditionalReferences>? CustomAdditionalReferencesSerializer = null)
        {

            var json = JSONObject.Create(
                           new JProperty("AdditionalReference",              AdditionalReference.    ToString()),
                           new JProperty("AdditionalReferenceUnit",          AdditionalReferenceUnit.ToString()),
                           new JProperty("PricePerAdditionalReferenceUnit",  PricePerAdditionalReferenceUnit)
                       );

            return CustomAdditionalReferencesSerializer is not null
                       ? CustomAdditionalReferencesSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone these additional references.
        /// </summary>
        public AdditionalReferences Clone()

            => new (
                   AdditionalReference,
                   AdditionalReferenceUnit,
                   PricePerAdditionalReferenceUnit
               );

        #endregion


        #region Operator overloading

        #region Operator == (AdditionalReferences1, AdditionalReferences2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReferences1">An additional reference.</param>
        /// <param name="AdditionalReferences2">Another additional reference.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AdditionalReferences? AdditionalReferences1,
                                           AdditionalReferences? AdditionalReferences2)
        {

            if (Object.ReferenceEquals(AdditionalReferences1, AdditionalReferences2))
                return true;

            if (AdditionalReferences1 is null || AdditionalReferences2 is null)
                return false;

            return AdditionalReferences1.Equals(AdditionalReferences2);

        }

        #endregion

        #region Operator != (AdditionalReferences1, AdditionalReferences2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AdditionalReferences1">An additional reference.</param>
        /// <param name="AdditionalReferences2">Another additional reference.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AdditionalReferences? AdditionalReferences1,
                                           AdditionalReferences? AdditionalReferences2)

            => !(AdditionalReferences1 == AdditionalReferences2);

        #endregion

        #endregion

        #region IEquatable<AdditionalReferences> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two additional references for equality.
        /// </summary>
        /// <param name="Object">Additional references to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AdditionalReferences additionalReferences &&
                   Equals(additionalReferences);

        #endregion

        #region Equals(AdditionalReferences)

        /// <summary>
        /// Compares two additional references for equality.
        /// </summary>
        /// <param name="AdditionalReferences">Additional references to compare with.</param>
        public Boolean Equals(AdditionalReferences? AdditionalReferences)

            => AdditionalReferences is not null &&

               AdditionalReferences.           Equals(AdditionalReferences.AdditionalReference)     &&
               AdditionalReferenceUnit.        Equals(AdditionalReferences.AdditionalReferenceUnit) &&
               PricePerAdditionalReferenceUnit.Equals(AdditionalReferences.PricePerAdditionalReferenceUnit);

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{AdditionalReference}, {AdditionalReferenceUnit}, {PricePerAdditionalReferenceUnit}";

        #endregion

    }

}
