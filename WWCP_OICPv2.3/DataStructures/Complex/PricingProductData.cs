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
    /// Pricing product data.
    /// </summary>
    public class PricingProductData : AInternalData,
                                      IEquatable<PricingProductData>,
                                      IComparable<PricingProductData>,
                                      IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the operator whose data records are listed below.
        /// </summary>
        [Mandatory]
        public Operator_Id                            OperatorId                        { get; }

        /// <summary>
        /// The optional name of the operator whose data records are listed below.
        /// </summary>
        [Optional]
        public String?                                OperatorName                      { get; }

        /// <summary>
        /// The EMP for whom the pricing data is applicable. In case the data is to be made
        /// available for all EMPs (e.g. for Offer-to-All prices), the asterix character (*)
        /// can be set as the value in this field.
        /// </summary>
        [Mandatory]
        public Provider_Id?                           ProviderId                        { get; }

        /// <summary>
        /// A default price for pricing sessions at undefined EVSEs.
        /// </summary>
        [Mandatory]
        public Decimal                                PricingDefaultPrice               { get; }

        /// <summary>
        /// The currency of the default prices.
        /// </summary>
        [Mandatory]
        public Currency_Id                            PricingDefaultPriceCurrency       { get; }

        /// <summary>
        /// Default Reference Unit in time or kWh.
        /// </summary>
        [Mandatory]
        public Reference_Unit                         PricingDefaultReferenceUnit       { get; }

        /// <summary>
        /// The multi-language name of the charging station hosting the EVSE.
        /// </summary>
        [Mandatory]
        public IEnumerable<PricingProductDataRecord>  PricingProductDataRecords         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new pricing product data object.
        /// </summary>
        /// <param name="OperatorId">The unique identification of the operator whose data records are listed below.</param>
        /// <param name="ProviderId">The EMP for whom the pricing data is applicable. In case the data is to be made available for all EMPs (e.g. for Offer-to-All prices), the asterix character (*) can be set as the value in this field.</param>
        /// <param name="PricingDefaultPrice">A default price for pricing sessions at undefined EVSEs.</param>
        /// <param name="PricingDefaultPriceCurrency">The currency of the default prices.</param>
        /// <param name="PricingDefaultReferenceUnit">Default Reference Unit in time or kWh.</param>
        /// <param name="PricingProductDataRecords">The multi-language name of the charging station hosting the EVSE.</param>
        /// 
        /// <param name="OperatorName">The name of the operator whose data records are listed below.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public PricingProductData(Operator_Id                            OperatorId,
                                  Provider_Id?                           ProviderId,
                                  Decimal                                PricingDefaultPrice,
                                  Currency_Id                            PricingDefaultPriceCurrency,
                                  Reference_Unit                         PricingDefaultReferenceUnit,
                                  IEnumerable<PricingProductDataRecord>  PricingProductDataRecords,

                                  String?                                OperatorName   = null,

                                  JObject?                               CustomData     = null,
                                  UserDefinedDictionary?                 InternalData   = null)

            : base(CustomData,
                   InternalData,
                   Timestamp.Now)

        {

            this.OperatorId                   = OperatorId;
            this.ProviderId                   = ProviderId;
            this.PricingDefaultPrice          = PricingDefaultPrice;
            this.PricingDefaultPriceCurrency  = PricingDefaultPriceCurrency;
            this.PricingDefaultReferenceUnit  = PricingDefaultReferenceUnit;
            this.PricingProductDataRecords    = PricingProductDataRecords.Distinct();

            this.OperatorName                 = OperatorName;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#123-pricingproductdatatype

        // {
        //     "OperatorID":                    "DE*ABC",
        //     "OperatorName":                  "ABC technologies",
        //     "PricingDefaultPrice":            0,
        //     "PricingDefaultPriceCurrency":   "EUR",
        //     "PricingDefaultReferenceUnit":   "HOUR",
        //     "PricingProductDataRecords": [
        //         {
        //             "AdditionalReferences": [
        //                 {
        //                     "AdditionalReference":               "PARKING FEE",
        //                     "AdditionalReferenceUnit":           "HOUR",
        //                     "PricePerAdditionalReferenceUnit":    2
        //                 }
        //             ],
        //             "IsValid24hours":                false,
        //             "MaximumProductChargingPower":   22,
        //             "PricePerReferenceUnit":         1,
        //             "ProductAvailabilityTimes": [
        //                 {
        //                     "Periods": [
        //                         {
        //                             "begin":  "09:00",
        //                             "end":    "18:00"
        //                         }
        //                     ],
        //                     "on": "Everyday"
        //                 }
        //             ],
        //             "ProductID":             "AC 1",
        //             "ProductPriceCurrency":  "EUR",
        //             "ReferenceUnit":         "HOUR"
        //         }
        //     ],
        //     "ProviderID": "*"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPricingProductDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of pricing product data object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPricingProductDataParser">A delegate to parse custom pricing product data objects JSON objects.</param>
        public static PricingProductData Parse(JObject                                           JSON,
                                               CustomJObjectParserDelegate<PricingProductData>?  CustomPricingProductDataParser   = null)
        {

            if (TryParse(JSON,
                         out var pricingProductData,
                         out var errorResponse,
                         CustomPricingProductDataParser))
            {
                return pricingProductData!;
            }

            throw new ArgumentException("The given JSON representation of pricing product data object is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out PricingProductData, out ErrorResponse, CustomPricingProductDataParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of pricing product data object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PricingProductData">The parsed pricing product data object.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                  JSON,
                                       out PricingProductData?  PricingProductData,
                                       out String?              ErrorResponse)

            => TryParse(JSON,
                        out PricingProductData,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of pricing product data object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="PricingProductData">The parsed pricing product data object.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPricingProductDataParser">A delegate to parse custom pricing product data objects JSON objects.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       out PricingProductData?                           PricingProductData,
                                       out String?                                       ErrorResponse,
                                       CustomJObjectParserDelegate<PricingProductData>?  CustomPricingProductDataParser)
        {

            try
            {

                PricingProductData = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorId                     [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "operator identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorName                   [optional]

                var OperatorName = JSON.GetString("OperatorName");

                #endregion

                #region Parse ProviderId                     [mandatory]

                if (!JSON.ParseMandatory("ProviderID",
                                         "provider identification",
                                         Provider_Id.TryParse,
                                         out Provider_Id ProviderId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PricingDefaultPrice            [mandatory]

                if (!JSON.ParseMandatory("PricingDefaultPrice",
                                         "pricing default price",
                                         out Decimal PricingDefaultPrice,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PricingDefaultPriceCurrency    [mandatory]

                if (!JSON.ParseMandatory("PricingDefaultPriceCurrency",
                                         "pricing default price currency",
                                         Currency_Id.TryParse,
                                         out Currency_Id PricingDefaultPriceCurrency,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PricingDefaultReferenceUnit    [mandatory]

                if (!JSON.ParseMandatory("PricingDefaultReferenceUnit",
                                         "pricing default reference unit",
                                         Reference_Unit.TryParse,
                                         out Reference_Unit PricingDefaultReferenceUnit,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PricingProductDataRecords      [mandatory]

                if (!JSON.ParseMandatoryJSON("PricingProductDataRecords",
                                             "pricing product data records",
                                             PricingProductDataRecord.TryParse,
                                             out IEnumerable<PricingProductDataRecord> PricingProductDataRecords,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse CustomData                     [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                PricingProductData = new PricingProductData(OperatorId,
                                                            ProviderId,
                                                            PricingDefaultPrice,
                                                            PricingDefaultPriceCurrency,
                                                            PricingDefaultReferenceUnit,
                                                            PricingProductDataRecords,

                                                            OperatorName,

                                                            customData);


                if (CustomPricingProductDataParser is not null)
                    PricingProductData = CustomPricingProductDataParser(JSON,
                                                                        PricingProductData);

                return true;

            }
            catch (Exception e)
            {
                PricingProductData  = default;
                ErrorResponse       = "The given JSON representation of pricing product data object is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPricingProductDataSerializer = null, CustomPricingProductDataRecordSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPricingProductDataSerializer">A delegate to serialize custom pricing product data JSON objects.</param>
        /// <param name="CustomPricingProductDataRecordSerializer">A delegate to serialize custom pricing product data record JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<PricingProductData>?        CustomPricingProductDataSerializer         = null,
                              CustomJObjectSerializerDelegate<PricingProductDataRecord>?  CustomPricingProductDataRecordSerializer   = null)
        {

            var json = JSONObject.Create(

                           new JProperty("OperatorID",                   OperatorId.ToString()),

                           OperatorName is not null && OperatorName.IsNotNullOrEmpty()
                               ? new JProperty("OperatorName",           OperatorName)
                               : null,

                           ProviderId.HasValue
                               ? new JProperty("ProviderID",             ProviderId.Value.ToString())
                               : new JProperty("ProviderID",             "*"),

                           new JProperty("PricingDefaultPrice",          PricingDefaultPrice),
                           new JProperty("PricingDefaultPriceCurrency",  PricingDefaultPriceCurrency.ToString()),
                           new JProperty("PricingDefaultReferenceUnit",  PricingDefaultReferenceUnit.ToString()),

                           new JProperty("PricingProductDataRecords",    new JArray(PricingProductDataRecords.Select(pricingProductDataRecord => pricingProductDataRecord.ToJSON(CustomPricingProductDataRecordSerializer)))),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",             CustomData)
                               : null

                       );

            return CustomPricingProductDataSerializer is not null
                       ? CustomPricingProductDataSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public PricingProductData Clone

            => new (OperatorId.                 Clone,
                    ProviderId?.                Clone,
                    PricingDefaultPrice,
                    PricingDefaultPriceCurrency.Clone,
                    PricingDefaultReferenceUnit.Clone,
                    PricingProductDataRecords.SafeSelect(pricingProductDataRecord => pricingProductDataRecord.Clone).ToArray(),

                    OperatorName is not null
                        ? new String(OperatorName.ToCharArray())
                        : null,

                    CustomData   is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (PricingProductData1, PricingProductData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PricingProductData1">An pricing product data object.</param>
        /// <param name="PricingProductData2">Another pricing product data object.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PricingProductData? PricingProductData1,
                                           PricingProductData? PricingProductData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PricingProductData1, PricingProductData2))
                return true;

            // If one is null, but not both, return false.
            if (PricingProductData1 is null || PricingProductData2 is null)
                return false;

            return PricingProductData1.Equals(PricingProductData2);

        }

        #endregion

        #region Operator != (PricingProductData1, PricingProductData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PricingProductData1">An pricing product data object.</param>
        /// <param name="PricingProductData2">Another pricing product data object.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PricingProductData? PricingProductData1,
                                           PricingProductData? PricingProductData2)

            => !(PricingProductData1 == PricingProductData2);

        #endregion

        #endregion

        #region IComparable<PricingProductData> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two pricing product data for equality.
        /// </summary>
        /// <param name="Object">Pricing product data to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is PricingProductData pricingProductData
                   ? CompareTo(pricingProductData)
                   : throw new ArgumentException("The given object is not pricing product data object!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(PricingProductData)

        /// <summary>
        /// Compares two pricing product data.
        /// </summary>
        /// <param name="PricingProductData">Pricing product data to compare with.</param>
        public Int32 CompareTo(PricingProductData? PricingProductData)
        {

            if (PricingProductData is null)
                throw new ArgumentNullException(nameof(PricingProductData), "The given pricing product data must not be null!");

            var c = OperatorId.CompareTo(PricingProductData.OperatorId);

            if (c == 0)
            {

                if      (!ProviderId.HasValue && !PricingProductData.ProviderId.HasValue)
                    c = 0;

                else if ( ProviderId.HasValue &&  PricingProductData.ProviderId.HasValue)
                    c = ProviderId.Value.CompareTo(PricingProductData.ProviderId.Value);

                else
                    c = -1;

            }

            if (c == 0)
                c = PricingDefaultPrice.        CompareTo(PricingProductData.PricingDefaultPrice);

            if (c == 0)
                c = PricingDefaultPriceCurrency.CompareTo(PricingProductData.PricingDefaultPriceCurrency);

            if (c == 0)
                c = PricingDefaultReferenceUnit.CompareTo(PricingProductData.PricingDefaultReferenceUnit);

            if (c == 0)
                c = PricingProductDataRecords.Count().CompareTo(PricingProductData.PricingProductDataRecords.Count());

            if (c == 0)
                c = PricingProductDataRecords.OrderBy      (pricingProductDataRecord => pricingProductDataRecord.ProductId).
                                              Select       (pricingProductDataRecord => pricingProductDataRecord.ProductId.ToString()).
                                              AggregateWith("-").
                                              CompareTo    (PricingProductData.PricingProductDataRecords.OrderBy(pricingProductDataRecord => pricingProductDataRecord.ProductId).
                                                                                                         Select (pricingProductDataRecord => pricingProductDataRecord.ProductId.ToString()).
                                                                                                         AggregateWith("-"));

            if (c == 0)
            {

                if (OperatorName is null && PricingProductData.OperatorName is null)
                    c = 0;

                else if (OperatorName is not null && PricingProductData.OperatorName is not null)
                    c = OperatorName.CompareTo(PricingProductData.OperatorName);

                else
                    c = -1;

            }

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<PricingProductData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two pricing product data for equality.
        /// </summary>
        /// <param name="Object">Pricing product data to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PricingProductData pricingProductData &&
                   Equals(pricingProductData);

        #endregion

        #region Equals(PricingProductData)

        /// <summary>
        /// Compares two pricing product data for equality.
        /// </summary>
        /// <param name="PricingProductData">Pricing product data to compare with.</param>
        public Boolean Equals(PricingProductData? PricingProductData)

            => PricingProductData is not null &&

               OperatorId.Equals(PricingProductData.OperatorId) &&

               String.Equals(OperatorName, PricingProductData.OperatorName) &&

            ((!ProviderId.HasValue && !PricingProductData.ProviderId.HasValue) ||
              (ProviderId.HasValue &&  PricingProductData.ProviderId.HasValue && ProviderId.Value.Equals(PricingProductData.ProviderId.Value))) &&

               PricingDefaultPrice.        Equals(PricingProductData.PricingDefaultPrice)         &&
               PricingDefaultPriceCurrency.Equals(PricingProductData.PricingDefaultPriceCurrency) &&
               PricingDefaultReferenceUnit.Equals(PricingProductData.PricingDefaultReferenceUnit) &&

               PricingProductDataRecords.Count().Equals(PricingProductData.PricingProductDataRecords.Count()) &&
               PricingProductDataRecords.All(pricingProductDataRecord => PricingProductData.PricingProductDataRecords.Contains(pricingProductDataRecord));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return OperatorId.                 GetHashCode()       * 17 ^
                      (OperatorName?.              GetHashCode() ?? 0) * 13 ^
                      (ProviderId?.                GetHashCode() ?? 0) * 11 ^
                       PricingDefaultPrice.        GetHashCode()       *  7 ^
                       PricingDefaultPriceCurrency.GetHashCode()       *  5 ^
                       PricingDefaultReferenceUnit.GetHashCode()       *  3 ^

                       (PricingProductDataRecords.Any()
                            ? PricingProductDataRecords.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             OperatorName.IsNotNullOrEmpty() ? " (" + OperatorName+ ")" : "",
                             ProviderId.HasValue ? " for provider " + ProviderId.Value.ToString() : " for all providers",
                             ": ", PricingDefaultPrice, " ", PricingDefaultPriceCurrency, " per ", PricingDefaultReferenceUnit,
                             " => ", PricingProductDataRecords.Count(), " pricing product data record(s)");

        #endregion

    }

}
