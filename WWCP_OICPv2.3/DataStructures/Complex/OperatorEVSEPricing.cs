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
    /// Operator EVSE pricing.
    /// </summary>
    public class OperatorEVSEPricing : IEquatable<OperatorEVSEPricing>,
                                       IComparable<OperatorEVSEPricing>,
                                       IComparable
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE pricing records.
        /// </summary>
        [Mandatory]
        public IEnumerable<EVSEPricing>  EVSEPricings    { get; }

        /// <summary>
        /// The unqiue identification of the EVSE operator maintaining the given EVSE pricing records.
        /// </summary>
        [Mandatory]
        public Operator_Id               OperatorId      { get; }

        /// <summary>
        /// The name of the EVSE operator maintaining the given EVSE pricing records.
        /// </summary>
        [Optional]
        public String?                   OperatorName    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                  CustomData      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OperatorEVSEPricing object.
        /// </summary>
        /// <param name="EVSEPricings">An enumeration of EVSE pricing records.</param>
        /// <param name="OperatorId">The unqiue identification of the EVSE operator maintaining the given EVSE pricing records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE pricing records.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public OperatorEVSEPricing(IEnumerable<EVSEPricing>  EVSEPricings,
                                   Operator_Id               OperatorId,
                                   String?                   OperatorName   = null,

                                   JObject?                  CustomData     = null)
        {

            this.EVSEPricings  = EVSEPricings.Distinct();
            this.OperatorId    = OperatorId;
            this.OperatorName  = OperatorName?.Trim();

            this.CustomData    = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OperatorEvseDataType

        // {
        //   "OperatorID":     "string",
        //   "OperatorName":   "string",
        //   "EVSEPricing":  [
        //     {
        //       ...
        //     }
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomOperatorEVSEPricingParser = null)

        /// <summary>
        /// Parse the given JSON representation of operator EVSE pricing.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomOperatorEVSEPricingParser">A delegate to parse custom operator EVSE pricings JSON objects.</param>
        public static OperatorEVSEPricing Parse(JObject                                            JSON,
                                                CustomJObjectParserDelegate<OperatorEVSEPricing>?  CustomOperatorEVSEPricingParser   = null)
        {

            if (TryParse(JSON,
                         out OperatorEVSEPricing?  operatorEVSEPricing,
                         out String?               errorResponse,
                         CustomOperatorEVSEPricingParser))
            {
                return operatorEVSEPricing!;
            }

            throw new ArgumentException("The given JSON representation of operator EVSE pricing is invalid: " + errorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomOperatorEVSEPricingParser = null)

        /// <summary>
        /// Parse the given text representation of operator EVSE pricing.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomOperatorEVSEPricingParser">A delegate to parse custom operator EVSE pricings JSON objects.</param>
        public static OperatorEVSEPricing Parse(String                                             Text,
                                                CustomJObjectParserDelegate<OperatorEVSEPricing>?  CustomOperatorEVSEPricingParser   = null)
        {

            if (TryParse(Text,
                         out OperatorEVSEPricing?  operatorEVSEPricing,
                         out String?               errorResponse,
                         CustomOperatorEVSEPricingParser))
            {
                return operatorEVSEPricing!;
            }

            throw new ArgumentException("The given text representation of operator EVSE pricing is invalid: " + errorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out OperatorEVSEPricing, out ErrorResponse, CustomOperatorEVSEPricingParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of operator EVSE pricing.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorEVSEPricing">The parsed operator EVSE pricing.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                   JSON,
                                       out OperatorEVSEPricing?  OperatorEVSEPricing,
                                       out String?               ErrorResponse)

            => TryParse(JSON,
                        out OperatorEVSEPricing,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of operator EVSE pricing.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorEVSEPricing">The parsed operator EVSE pricing.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOperatorEVSEPricingParser">A delegate to parse custom operator EVSE pricings JSON objects.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       out OperatorEVSEPricing?                           OperatorEVSEPricing,
                                       out String?                                        ErrorResponse,
                                       CustomJObjectParserDelegate<OperatorEVSEPricing>?  CustomOperatorEVSEPricingParser)
        {

            try
            {

                OperatorEVSEPricing = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EvseDataRecords       [mandatory]

                if (!JSON.ParseMandatoryJSON("EvseDataRecord",
                                             "EVSE pricing records",
                                             EVSEPricing.TryParse,
                                             out IEnumerable<EVSEPricing> EvseDataRecords,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorId            [mandatory]

                if (!JSON.ParseMandatoryEnum("OperatorID",
                                             "operator identification",
                                             out Operator_Id OperatorId,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorName          [mandatory]

                if (!JSON.ParseMandatoryText("OperatorName",
                                             "operator name",
                                             out String OperatorName,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse CustomData            [optional]

#pragma warning disable CA1507 // Use nameof to express symbol names
                var CustomData = JSON["CustomData"] as JObject;
#pragma warning restore CA1507 // Use nameof to express symbol names

                #endregion


                OperatorEVSEPricing = new OperatorEVSEPricing(EvseDataRecords,
                                                              OperatorId,
                                                              OperatorName,

                                                              CustomData);


                if (CustomOperatorEVSEPricingParser is not null)
                    OperatorEVSEPricing = CustomOperatorEVSEPricingParser(JSON,
                                                                          OperatorEVSEPricing);

                return true;

            }
            catch (Exception e)
            {
                OperatorEVSEPricing  = default;
                ErrorResponse        = "The given JSON representation of operator EVSE pricing is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out OperatorEVSEPricing, out ErrorResponse, CustomOperatorEVSEPricingParser = null)

        /// <summary>
        /// Try to parse the given text representation of operator EVSE pricing.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OperatorEVSEPricing">The parsed operator EVSE pricing.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOperatorEVSEPricingParser">A delegate to parse custom operator EVSE pricings JSON objects.</param>
        public static Boolean TryParse(String                                             Text,
                                       out OperatorEVSEPricing?                           OperatorEVSEPricing,
                                       out String?                                        ErrorResponse,
                                       CustomJObjectParserDelegate<OperatorEVSEPricing>?  CustomOperatorEVSEPricingParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out OperatorEVSEPricing,
                                out ErrorResponse,
                                CustomOperatorEVSEPricingParser);

            }
            catch (Exception e)
            {
                OperatorEVSEPricing  = default;
                ErrorResponse        = "The given text representation of operator EVSE pricing is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomOperatorEVSEPricingSerializer = null, CustomEVSEPricingSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomOperatorEVSEPricingSerializer">A delegate to serialize custom operator EVSE pricing JSON objects.</param>
        /// <param name="CustomEVSEPricingSerializer">A delegate to serialize custom EVSE pricing record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<OperatorEVSEPricing>?  CustomOperatorEVSEPricingSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSEPricing>?          CustomEVSEPricingSerializer           = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseDataRecord",  new JArray(EVSEPricings.Select(evsePricing => evsePricing.ToJSON(CustomEVSEPricingSerializer)))),
                           new JProperty("OperatorID",      OperatorId.ToString()),
                           new JProperty("OperatorName",    OperatorName),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomOperatorEVSEPricingSerializer is not null
                       ? CustomOperatorEVSEPricingSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public OperatorEVSEPricing Clone

            => new (EVSEPricings.SafeSelect(evseDataRecord => evseDataRecord.Clone).ToArray(),
                    OperatorId.Clone,
                    OperatorName is not null
                        ? new String(OperatorName.ToCharArray())
                        : null,
                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OperatorEVSEPricing OperatorEVSEPricing1,
                                           OperatorEVSEPricing OperatorEVSEPricing2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(OperatorEVSEPricing1, OperatorEVSEPricing2))
                return true;

            // If one is null, but not both, return false.
            if (OperatorEVSEPricing1 is null || OperatorEVSEPricing2 is null)
                return false;

            return OperatorEVSEPricing1.Equals(OperatorEVSEPricing2);

        }

        #endregion

        #region Operator != (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OperatorEVSEPricing OperatorEVSEPricing1,
                                           OperatorEVSEPricing OperatorEVSEPricing2)

            => !(OperatorEVSEPricing1 == OperatorEVSEPricing2);

        #endregion

        #region Operator <  (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (OperatorEVSEPricing OperatorEVSEPricing1,
                                          OperatorEVSEPricing OperatorEVSEPricing2)
        {

            if (OperatorEVSEPricing1 is null)
                throw new ArgumentNullException(nameof(OperatorEVSEPricing1), "The given OperatorEVSEPricing1 must not be null!");

            return OperatorEVSEPricing1.CompareTo(OperatorEVSEPricing2) < 0;

        }

        #endregion

        #region Operator <= (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (OperatorEVSEPricing OperatorEVSEPricing1,
                                           OperatorEVSEPricing OperatorEVSEPricing2)

            => !(OperatorEVSEPricing1 > OperatorEVSEPricing2);

        #endregion

        #region Operator >  (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (OperatorEVSEPricing OperatorEVSEPricing1,
                                          OperatorEVSEPricing OperatorEVSEPricing2)
        {

            if (OperatorEVSEPricing1 is null)
                throw new ArgumentNullException(nameof(OperatorEVSEPricing1), "The given OperatorEVSEPricing1 must not be null!");

            return OperatorEVSEPricing1.CompareTo(OperatorEVSEPricing2) > 0;

        }

        #endregion

        #region Operator >= (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (OperatorEVSEPricing OperatorEVSEPricing1,
                                           OperatorEVSEPricing OperatorEVSEPricing2)

            => !(OperatorEVSEPricing1 < OperatorEVSEPricing2);

        #endregion

        #endregion

        #region IComparable<OperatorEVSEPricing> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is OperatorEVSEPricing operatorEVSEPricing
                   ? CompareTo(operatorEVSEPricing)
                   : throw new ArgumentException("The given object is not operator EVSE pricing!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(OperatorEVSEPricing)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing">An object to compare with.</param>
        public Int32 CompareTo(OperatorEVSEPricing? OperatorEVSEPricing)
        {

            if (OperatorEVSEPricing is null)
                throw new ArgumentNullException(nameof(OperatorEVSEPricing), "The given operator EVSE pricing must not be null!");

            var c = OperatorId.  CompareTo(OperatorEVSEPricing.OperatorId);

            if (c == 0 && OperatorName is not null && OperatorEVSEPricing.OperatorName is not null)
                c = OperatorName.CompareTo(OperatorEVSEPricing.OperatorName);

            if (c == 0)
                c = EVSEPricings.Count().CompareTo(OperatorEVSEPricing.EVSEPricings.Count());

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<OperatorEVSEPricing> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is OperatorEVSEPricing operatorEVSEPricing &&
                   Equals(operatorEVSEPricing);

        #endregion

        #region Equals(OperatorEVSEPricing)

        /// <summary>
        /// Compares two operator EVSE pricings for equality.
        /// </summary>
        /// <param name="OperatorEVSEPricing">A operator EVSE pricing to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OperatorEVSEPricing? OperatorEVSEPricing)

            => OperatorEVSEPricing is not null &&

               OperatorId.  Equals(OperatorEVSEPricing.OperatorId)   &&

             ((OperatorName is     null && OperatorEVSEPricing.OperatorName is     null) ||
              (OperatorName is not null && OperatorEVSEPricing.OperatorName is not null && OperatorName.Equals(OperatorEVSEPricing.OperatorName))) &&

               EVSEPricings.Count().Equals(OperatorEVSEPricing.EVSEPricings.Count()) &&
               EVSEPricings.All(evseDataRecord => OperatorEVSEPricing.EVSEPricings.Contains(evseDataRecord));

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

                return OperatorId.GetHashCode() * 5 ^

                       (OperatorName?.GetHashCode() ?? 0) * 3 ^

                       (EVSEPricings.Any()
                            ? EVSEPricings.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             OperatorName.IsNotNullOrEmpty() ? ", " + OperatorName : "",
                             ", ",  EVSEPricings.Count(), " EVSE pricing record(s)");

        #endregion

    }

}
