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
    /// EVSE pricing information.
    /// </summary>
    public class EVSEPricing : IEquatable<EVSEPricing>,
                               IComparable<EVSEPricing>,
                               IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE for which the defined pricing products are applicable.
        /// </summary>
        [Mandatory]
        public EVSE_Id                         EVSEId               { get; }

        /// <summary>
        /// The EMP for whom the pricing data is applicable. In case the data is to be made available for all EMPs
        /// (e.g. for Offer-to-All prices), the asterix character (*) can be set as the value in this field.
        /// </summary>
        [Mandatory]
        public Provider_Id?                    ProviderId           { get; }

        /// <summary>
        /// A list of pricing products applicable per EVSE.
        /// </summary>
        [Mandatory]
        public IEnumerable<PartnerProduct_Id>  EVSEIdProductList    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                        CustomData           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE pricing information object.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE data records.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public EVSEPricing(EVSE_Id                         EVSEId,
                           Provider_Id?                    ProviderId,
                           IEnumerable<PartnerProduct_Id>  EVSEIdProductList,

                           JObject?                        CustomData   = null)
        {

            if (EVSEIdProductList is null || !EVSEIdProductList.Any())
                throw new ArgumentNullException(nameof(EVSEIdProductList), "The given enumeration of charging product identifications must not be null or empty!");

            this.EVSEId             = EVSEId;
            this.ProviderId         = ProviderId;
            this.EVSEIdProductList  = EVSEIdProductList.Distinct();

            this.CustomData         = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#125-evsepricingtype

        // {
        // }

        #endregion

        #region (static) Parse   (JSON, ..., CustomEVSEPricingParser = null)

        /// <summary>
        /// Parse the given JSON representation of EVSE pricing information.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEVSEPricingParser">A delegate to parse custom EVSE pricing information JSON objects.</param>
        public static EVSEPricing Parse(JObject                                    JSON,
                                        CustomJObjectParserDelegate<EVSEPricing>?  CustomEVSEPricingParser   = null)
        {

            if (TryParse(JSON,
                         out EVSEPricing?  evsePricing,
                         out String?       errorResponse,
                         CustomEVSEPricingParser))
            {
                return evsePricing!;
            }

            throw new ArgumentException("The given JSON representation of EVSE pricing information is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, ..., CustomEVSEPricingParser = null)

        /// <summary>
        /// Parse the given text representation of EVSE pricing information.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomEVSEPricingParser">A delegate to parse custom EVSE pricing information JSON objects.</param>
        public static EVSEPricing Parse(String                                     Text,
                                        CustomJObjectParserDelegate<EVSEPricing>?  CustomEVSEPricingParser   = null)
        {

            if (TryParse(Text,
                         out EVSEPricing?  evsePricing,
                         out String?       errorResponse,
                         CustomEVSEPricingParser))
            {
                return evsePricing!;
            }

            throw new ArgumentException("The given text representation of EVSE pricing information is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out EVSEPricing, out ErrorResponse, CustomEVSEPricingParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of EVSE pricing information.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEPricing">The parsed EVSE pricing information.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject           JSON,
                                       out EVSEPricing?  EVSEPricing,
                                       out String?       ErrorResponse)

            => TryParse(JSON,
                        out EVSEPricing,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of EVSE pricing information.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEPricing">The parsed EVSE pricing information.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEVSEPricingParser">A delegate to parse custom EVSE pricing information JSON objects.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       out EVSEPricing?                           EVSEPricing,
                                       out String?                                ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEPricing>?  CustomEVSEPricingParser)
        {

            try
            {

                EVSEPricing = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EVSEId               [mandatory]

                if (!JSON.ParseMandatory("EvseID",
                                         "EVSE identification",
                                         EVSE_Id.TryParse,
                                         out EVSE_Id EVSEId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ProviderId           [mandatory]

                Provider_Id? ProviderId = default;
                var providerIDString = JSON["ProviderID"]?.Value<String>();

                if (providerIDString is not null && providerIDString != "*")
                {

                    if (!JSON.ParseMandatory("ProviderID",
                                             "e-mobility provider identification",
                                             Provider_Id.TryParse,
                                             out Provider_Id providerId,
                                             out ErrorResponse))
                    {
                        return false;
                    }

                    ProviderId = providerId;

                }

                #endregion

                #region Parse EVSEIdProductList    [mandatory]

                if (!JSON.ParseMandatory("EvseIDProductList",
                                         "EVSE identification product list",
                                         PartnerProduct_Id.TryParse,
                                         out IEnumerable<PartnerProduct_Id> EVSEIdProductList,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse CustomData           [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                EVSEPricing = new EVSEPricing(EVSEId,
                                              ProviderId,
                                              EVSEIdProductList,

                                              customData);


                if (CustomEVSEPricingParser is not null)
                    EVSEPricing = CustomEVSEPricingParser(JSON,
                                                          EVSEPricing);

                return true;

            }
            catch (Exception e)
            {
                EVSEPricing    = default;
                ErrorResponse  = "The given JSON representation of EVSE pricing information is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out EVSEPricing, out ErrorResponse, CustomEVSEPricingParser = null)

        /// <summary>
        /// Try to parse the given text representation of EVSE pricing information.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="EVSEPricing">The parsed EVSE pricing information.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEVSEPricingParser">A delegate to parse custom EVSE pricing information JSON objects.</param>
        public static Boolean TryParse(String                                     Text,
                                       out EVSEPricing?                           EVSEPricing,
                                       out String?                                ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEPricing>?  CustomEVSEPricingParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out EVSEPricing,
                                out ErrorResponse,
                                CustomEVSEPricingParser);

            }
            catch (Exception e)
            {
                EVSEPricing    = default;
                ErrorResponse  = "The given text representation of EVSE pricing information is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEVSEPricingSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEVSEPricingSerializer">A delegate to serialize custom EVSE pricing information JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EVSEPricing>?  CustomEVSEPricingSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseID",              EVSEId.ToString()),

                           ProviderId.HasValue
                               ? new JProperty("ProviderID",    ProviderId.Value.ToString())
                               : new JProperty("ProviderID",    "*"),

                           new JProperty("EvseIDProductList",   new JArray(EVSEIdProductList.Select(productId => productId.ToString()))),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomEVSEPricingSerializer is not null
                       ? CustomEVSEPricingSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public EVSEPricing Clone

            => new (EVSEId.Clone,

                    ProviderId.HasValue
                        ? ProviderId.Value.Clone
                        : null,

                    EVSEIdProductList.SafeSelect(productId => productId.Clone).ToArray(),

                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEPricing1, EVSEPricing2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="EVSEPricing1">An EVSE pricing information.</param>
        /// <param name="EVSEPricing2">Another EVSE pricing information.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEPricing EVSEPricing1,
                                           EVSEPricing EVSEPricing2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEPricing1, EVSEPricing2))
                return true;

            // If one is null, but not both, return false.
            if (EVSEPricing1 is null || EVSEPricing2 is null)
                return false;

            return EVSEPricing1.Equals(EVSEPricing2);

        }

        #endregion

        #region Operator != (EVSEPricing1, EVSEPricing2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="EVSEPricing1">An EVSE pricing information.</param>
        /// <param name="EVSEPricing2">Another EVSE pricing information.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEPricing EVSEPricing1,
                                           EVSEPricing EVSEPricing2)

            => !(EVSEPricing1 == EVSEPricing2);

        #endregion

        #region Operator <  (EVSEPricing1, EVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEPricing1">An EVSE pricing information.</param>
        /// <param name="EVSEPricing2">Another EVSE pricing information.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEPricing EVSEPricing1,
                                          EVSEPricing EVSEPricing2)
        {

            if (EVSEPricing1 is null)
                throw new ArgumentNullException(nameof(EVSEPricing1), "The given EVSEPricing1 must not be null!");

            return EVSEPricing1.CompareTo(EVSEPricing2) < 0;

        }

        #endregion

        #region Operator <= (EVSEPricing1, EVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEPricing1">An EVSE pricing information.</param>
        /// <param name="EVSEPricing2">Another EVSE pricing information.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEPricing EVSEPricing1,
                                           EVSEPricing EVSEPricing2)

            => !(EVSEPricing1 > EVSEPricing2);

        #endregion

        #region Operator >  (EVSEPricing1, EVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEPricing1">An EVSE pricing information.</param>
        /// <param name="EVSEPricing2">Another EVSE pricing information.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEPricing EVSEPricing1,
                                          EVSEPricing EVSEPricing2)
        {

            if (EVSEPricing1 is null)
                throw new ArgumentNullException(nameof(EVSEPricing1), "The given EVSEPricing1 must not be null!");

            return EVSEPricing1.CompareTo(EVSEPricing2) > 0;

        }

        #endregion

        #region Operator >= (EVSEPricing1, EVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEPricing1">An EVSE pricing information.</param>
        /// <param name="EVSEPricing2">Another EVSE pricing information.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEPricing EVSEPricing1,
                                           EVSEPricing EVSEPricing2)

            => !(EVSEPricing1 < EVSEPricing2);

        #endregion

        #endregion

        #region IComparable<EVSEPricing> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEPricing evsePricing
                   ? CompareTo(evsePricing)
                   : throw new ArgumentException("The given object is not an EVSE pricing information!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEPricing)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEPricing">An object to compare with.</param>
        public Int32 CompareTo(EVSEPricing? EVSEPricing)
        {

            if (EVSEPricing is null)
                throw new ArgumentNullException(nameof(EVSEPricing), "The given EVSE pricing information must not be null!");

            var c = EVSEId.CompareTo(EVSEPricing.EVSEId);

            if (c == 0)
            {
                if (!ProviderId.HasValue && !EVSEPricing.ProviderId.HasValue)
                    c = 0;
                else if (ProviderId.HasValue && EVSEPricing.ProviderId.HasValue)
                    c = ProviderId.Value.CompareTo(EVSEPricing.ProviderId.Value);
                else
                    c = -1;
            }

            if (c == 0)
                c = EVSEIdProductList.Count().CompareTo(EVSEPricing.EVSEIdProductList.Count());

            if (c == 0)
                c = EVSEIdProductList.OrderBy      (productId => productId).
                                      Select       (productId => productId.ToString()).
                                      AggregateWith("-").
                                      CompareTo    (EVSEPricing.EVSEIdProductList.OrderBy(productId => productId).
                                                                                  Select (productId => productId.ToString()).
                                                                                  AggregateWith("-"));

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEPricing> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is EVSEPricing evsePricing &&
                   Equals(evsePricing);

        #endregion

        #region Equals(EVSEPricing)

        /// <summary>
        /// Compares two EVSE pricing informations for equality.
        /// </summary>
        /// <param name="EVSEPricing">A EVSE pricing information to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEPricing? EVSEPricing)

            => EVSEPricing is not null &&

               EVSEId.  Equals(EVSEPricing.EVSEId) &&

            ((!ProviderId.HasValue && !EVSEPricing.ProviderId.HasValue) ||
              (ProviderId.HasValue &&  EVSEPricing.ProviderId.HasValue && ProviderId.Value.Equals(EVSEPricing.ProviderId.Value))) &&

               EVSEIdProductList.Count().Equals(EVSEPricing.EVSEIdProductList.Count()) &&
               EVSEIdProductList.All(productId => EVSEPricing.EVSEIdProductList.Contains(productId));

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

                return EVSEId.      GetHashCode()       * 5 ^

                       (ProviderId?.GetHashCode() ?? 0) * 3 ^

                       (EVSEIdProductList.Any()
                            ? EVSEIdProductList.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId,
                             ProviderId.HasValue ? " for provider " + ProviderId.Value.ToString() : " for all providers",
                             ", ", EVSEIdProductList.Count(), " product id(s)");

        #endregion

    }

}
