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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A pricing product data record.
    /// </summary>
    public class PricingProductDataRecord : AInternalData,
                                            IEquatable<PricingProductDataRecord>
    {

        #region Properties

        /// <summary>
        /// A pricing product name (for identifying a tariff) that MUST be unique.
        /// </summary>
        [Mandatory]
        public PartnerProduct_Id                      ProductId                              { get; }


        /// <summary>
        /// Reference unit in time or kWh.
        /// </summary>
        [Mandatory]
        public Reference_Unit                         ReferenceUnit                          { get; }

        /// <summary>
        /// Currency for default prices.
        /// </summary>
        [Mandatory]
        public Currency_Id                            ProductPriceCurrency                   { get; }

        /// <summary>
        /// A price per reference unit.
        /// </summary>
        [Mandatory]
        public Decimal                                PricePerReferenceUnit                  { get; }

        /// <summary>
        /// A value in kWh.
        /// </summary>
        [Mandatory]
        public Decimal                                MaximumProductChargingPower            { get; }

        /// <summary>
        /// Set to TRUE if the respective pricing product is applicable 24 hours a day.
        /// If FALSE, the respective applicability times SHOULD be provided in the field "ProductAvailabilityTimes".
        /// </summary>
        [Mandatory]
        public Boolean                                IsValid24hours                         { get; }

        /// <summary>
        /// An enumeration indicating when the pricing product is applicable.
        /// </summary>
        [Mandatory]
        public IEnumerable<ProductAvailabilityTimes>  ProductAvailabilityTimes               { get; }

        /// <summary>
        /// An optional enumeration of additional reference units and their respective prices.
        /// </summary>
        [Optional]
        public IEnumerable<AdditionalReferences>      AdditionalReferences                   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new pricing product data record.
        /// </summary>
        /// <param name="ProductId">A pricing product name (for identifying a tariff) that MUST be unique.</param>
        /// <param name="ReferenceUnit">Reference unit in time or kWh.</param>
        /// <param name="ProductPriceCurrency">Currency for default prices.</param>
        /// <param name="PricePerReferenceUnit">A price per reference unit.</param>
        /// <param name="MaximumProductChargingPower">A value in kWh.</param>
        /// <param name="IsValid24hours">Set to TRUE if the respective pricing product is applicable 24 hours a day. If FALSE, the respective applicability times SHOULD be provided in the field "ProductAvailabilityTimes".</param>
        /// <param name="ProductAvailabilityTimes">An enumeration indicating when the pricing product is applicable.</param>
        /// 
        /// <param name="AdditionalReferences">An optional enumeration of additional reference units and their respective prices.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public PricingProductDataRecord(PartnerProduct_Id                      ProductId,
                                        Reference_Unit                         ReferenceUnit,
                                        Currency_Id                            ProductPriceCurrency,
                                        Decimal                                PricePerReferenceUnit,
                                        Decimal                                MaximumProductChargingPower,
                                        Boolean                                IsValid24hours,
                                        IEnumerable<ProductAvailabilityTimes>  ProductAvailabilityTimes,

                                        IEnumerable<AdditionalReferences>?     AdditionalReferences   = null,

                                        JObject?                               CustomData             = null,
                                        UserDefinedDictionary?                 InternalData           = null)

            : base(CustomData,
                   InternalData)

        {

            this.ProductId                    = ProductId;
            this.ReferenceUnit                = ReferenceUnit;
            this.ProductPriceCurrency         = ProductPriceCurrency;
            this.PricePerReferenceUnit        = PricePerReferenceUnit;
            this.MaximumProductChargingPower  = MaximumProductChargingPower;
            this.IsValid24hours               = IsValid24hours;
            this.ProductAvailabilityTimes     = ProductAvailabilityTimes.Distinct();

            this.AdditionalReferences         = AdditionalReferences?.   Distinct() ?? Array.Empty<AdditionalReferences>();

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#124-pricingproductdatarecordtype

        // {
        //     "AdditionalReferences": [
        //         {
        //             "AdditionalReference":               "PARKING FEE",
        //             "AdditionalReferenceUnit":           "HOUR",
        //             "PricePerAdditionalReferenceUnit":    2
        //         }
        //     ],
        //     "IsValid24hours":                false,
        //     "MaximumProductChargingPower":   22,
        //     "PricePerReferenceUnit":         1,
        //     "ProductAvailabilityTimes": [
        //         {
        //             "Periods": [
        //                 {
        //                     "begin":  "09:00",
        //                     "end":    "18:00"
        //                 }
        //             ],
        //             "on": "Everyday"
        //         }
        //     ],
        //     "ProductID":             "AC 1",
        //     "ProductPriceCurrency":  "EUR",
        //     "ReferenceUnit":         "HOUR"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPricingProductDataRecordParser = null)

        /// <summary>
        /// Parse the given JSON representation of an EVSE data record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPricingProductDataRecordParser">A delegate to parse custom EVSE data records JSON objects.</param>
        public static PricingProductDataRecord Parse(JObject                                                 JSON,
                                                     CustomJObjectParserDelegate<PricingProductDataRecord>?  CustomPricingProductDataRecordParser   = null)
        {

            if (TryParse(JSON,
                         out PricingProductDataRecord?  pricingProductDataRecord,
                         out String?                    errorResponse,
                         CustomPricingProductDataRecordParser))
            {
                return pricingProductDataRecord!;
            }

            throw new ArgumentException("The given JSON representation of an EVSE data record is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PricingProductDataRecord, out ErrorResponse, CustomPricingProductDataRecordParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an EVSE data record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PricingProductDataRecord">The parsed EVSE data record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                        JSON,
                                       out PricingProductDataRecord?  PricingProductDataRecord,
                                       out String?                    ErrorResponse)

            => TryParse(JSON,
                        out PricingProductDataRecord,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an EVSE data record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PricingProductDataRecord">The parsed EVSE data record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPricingProductDataRecordParser">A delegate to parse custom EVSE data records JSON objects.</param>
        public static Boolean TryParse(JObject                                                 JSON,
                                       out PricingProductDataRecord?                           PricingProductDataRecord,
                                       out String?                                             ErrorResponse,
                                       CustomJObjectParserDelegate<PricingProductDataRecord>?  CustomPricingProductDataRecordParser)
        {

            try
            {

                PricingProductDataRecord = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ProductId                      [mandatory]

                if (!JSON.ParseMandatory("ProductID",
                                         "charging product identification",
                                         PartnerProduct_Id.TryParse,
                                         out PartnerProduct_Id ProductId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ReferenceUnit                  [mandatory]

                if (!JSON.ParseMandatory("ReferenceUnit",
                                         "reference unit",
                                         Reference_Unit.TryParse,
                                         out Reference_Unit ReferenceUnit,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ProductPriceCurrency           [mandatory]

                if (!JSON.ParseMandatory("ProductPriceCurrency",
                                         "product price currency",
                                         Currency_Id.TryParse,
                                         out Currency_Id ProductPriceCurrency,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PricePerReferenceUnit          [mandatory]

                if (!JSON.ParseMandatory("PricePerReferenceUnit",
                                         "price per reference unit",
                                         out Decimal PricePerReferenceUnit,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse MaximumProductChargingPower    [mandatory]

                if (!JSON.ParseMandatory("MaximumProductChargingPower",
                                         "maximum product charging power",
                                         out Decimal MaximumProductChargingPower,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse IsValid24hours                 [mandatory]

                if (!JSON.ParseMandatory("IsValid24hours",
                                         "is valid 24 hours",
                                         out Boolean IsValid24hours,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ProductAvailabilityTimes       [mandatory]

                if (!JSON.ParseMandatoryJSON("ProductAvailabilityTimes",
                                             "product availability times",
                                             OICPv2_3.ProductAvailabilityTimes.TryParse,
                                             out IEnumerable<ProductAvailabilityTimes> ProductAvailabilityTimes,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse AdditionalReferences           [optional]

                if (!JSON.ParseOptionalJSON("AdditionalReferences",
                                            "additional references",
                                            OICPv2_3.AdditionalReferences.TryParse,
                                            out IEnumerable<AdditionalReferences> AdditionalReferences,
                                            out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse CustomData                     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PricingProductDataRecord = new PricingProductDataRecord(ProductId,
                                                                        ReferenceUnit,
                                                                        ProductPriceCurrency,
                                                                        PricePerReferenceUnit,
                                                                        MaximumProductChargingPower,
                                                                        IsValid24hours,
                                                                        ProductAvailabilityTimes,

                                                                        AdditionalReferences,

                                                                        customData);


                if (CustomPricingProductDataRecordParser is not null)
                    PricingProductDataRecord = CustomPricingProductDataRecordParser(JSON,
                                                                                    PricingProductDataRecord);

                return true;

            }
            catch (Exception e)
            {
                PricingProductDataRecord  = default;
                ErrorResponse             = "The given JSON representation of an EVSE data record is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPricingProductDataRecordSerializer = null, CustomProductAvailabilityTimesSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPricingProductDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomProductAvailabilityTimesSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomAdditionalReferencesSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PricingProductDataRecord>?  CustomPricingProductDataRecordSerializer   = null,
                              CustomJObjectSerializerDelegate<ProductAvailabilityTimes>?  CustomProductAvailabilityTimesSerializer   = null,
                              CustomJObjectSerializerDelegate<AdditionalReferences>?      CustomAdditionalReferencesSerializer       = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("ProductID",                    ProductId.               ToString()),
                           new JProperty("ReferenceUnit",                ReferenceUnit.           ToString()),
                           new JProperty("ProductPriceCurrency",         ProductPriceCurrency.    ToString()),
                           new JProperty("PricePerReferenceUnit",        PricePerReferenceUnit),
                           new JProperty("MaximumProductChargingPower",  MaximumProductChargingPower),
                           new JProperty("IsValid24hours",               IsValid24hours),
                           new JProperty("ProductAvailabilityTimes",     new JArray(ProductAvailabilityTimes.Select(productAvailabilityTime => productAvailabilityTime.ToJSON(CustomProductAvailabilityTimesSerializer)))),

                           AdditionalReferences.Any()
                               ? new JProperty("AdditionalReferences",   new JArray(AdditionalReferences.    Select(additionalReference     => additionalReference.    ToJSON(CustomAdditionalReferencesSerializer))))
                               : null,

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",             CustomData)
                               : null

                       );

            return CustomPricingProductDataRecordSerializer is not null
                       ? CustomPricingProductDataRecordSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public PricingProductDataRecord Clone

            => new (ProductId.           Clone,
                    ReferenceUnit.       Clone,
                    ProductPriceCurrency.Clone,
                    PricePerReferenceUnit,
                    MaximumProductChargingPower,
                    IsValid24hours,
                    ProductAvailabilityTimes.Select(productAvailabilityTime => productAvailabilityTime.Clone).ToArray(),

                    AdditionalReferences is not null
                        ? AdditionalReferences.Select(additionalReference => additionalReference.Clone).ToArray()
                        : null,

                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (PricingProductDataRecord1, PricingProductDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PricingProductDataRecord1">An EVSE data record.</param>
        /// <param name="PricingProductDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PricingProductDataRecord PricingProductDataRecord1,
                                           PricingProductDataRecord PricingProductDataRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PricingProductDataRecord1, PricingProductDataRecord2))
                return true;

            // If one is null, but not both, return false.
            if (PricingProductDataRecord1 is null || PricingProductDataRecord2 is null)
                return false;

            return PricingProductDataRecord1.Equals(PricingProductDataRecord2);

        }

        #endregion

        #region Operator != (PricingProductDataRecord1, PricingProductDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PricingProductDataRecord1">An EVSE data record.</param>
        /// <param name="PricingProductDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PricingProductDataRecord PricingProductDataRecord1,
                                           PricingProductDataRecord PricingProductDataRecord2)

            => !(PricingProductDataRecord1 == PricingProductDataRecord2);

        #endregion

        #endregion

        #region IEquatable<PricingProductDataRecord> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two pricing product data data records for equality.
        /// </summary>
        /// <param name="Object">A pricing product data data record to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PricingProductDataRecord pricingProductDataRecord &&
                   Equals(pricingProductDataRecord);

        #endregion

        #region Equals(PricingProductDataRecord)

        /// <summary>
        /// Compares two pricing product data data records for equality.
        /// </summary>
        /// <param name="PricingProductDataRecord">A pricing product data data record to compare with.</param>
        public Boolean Equals(PricingProductDataRecord? PricingProductDataRecord)

            => PricingProductDataRecord is not null &&

               ProductId.                   Equals(PricingProductDataRecord.ProductId)                    &&
               ReferenceUnit.               Equals(PricingProductDataRecord.ReferenceUnit)                &&
               ProductPriceCurrency.        Equals(PricingProductDataRecord.ProductPriceCurrency)         &&
               PricePerReferenceUnit.       Equals(PricingProductDataRecord.PricePerReferenceUnit)        &&
               MaximumProductChargingPower. Equals(PricingProductDataRecord.MaximumProductChargingPower)  &&
               IsValid24hours.              Equals(PricingProductDataRecord.IsValid24hours)               &&
               ProductAvailabilityTimes.    Equals(PricingProductDataRecord.ProductAvailabilityTimes)     &&

               AdditionalReferences.Count().Equals(PricingProductDataRecord.AdditionalReferences.Count()) &&
               AdditionalReferences.All(additionalReference => PricingProductDataRecord.AdditionalReferences.Contains(additionalReference));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => ProductId.                  GetHashCode() * 29 ^
               ReferenceUnit.              GetHashCode() * 23 ^
               ProductPriceCurrency.       GetHashCode() * 17 ^
               PricePerReferenceUnit.      GetHashCode() * 13 ^
               MaximumProductChargingPower.GetHashCode() * 11 ^
               IsValid24hours.             GetHashCode() *  7 ^
               ProductAvailabilityTimes.   GetHashCode() *  3 ^

              (AdditionalReferences is not null && AdditionalReferences.Any()
                   ? AdditionalReferences.GetHashCode()
                   : 0);

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ProductId,             ", ",
                             ReferenceUnit,         ", ",
                             PricePerReferenceUnit, " ",
                             ProductPriceCurrency);

        #endregion


    }

}
