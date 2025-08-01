﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
        /// The unique identification of the EVSE operator maintaining the given EVSE pricing records.
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
        /// <param name="OperatorId">The unique identification of the EVSE operator maintaining the given EVSE pricing records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE pricing records.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public OperatorEVSEPricing(IEnumerable<EVSEPricing>  EVSEPricings,
                                   Operator_Id               OperatorId,
                                   String?                   OperatorName   = null,

                                   JObject?                  CustomData     = null)
        {

            var duplicateEVSEPricings = EVSEPricings.GroupBy(evsePricing => evsePricing.EVSEId).Where(group => group.Count() > 1).ToArray();
            if (duplicateEVSEPricings.SafeAny())
                throw new ArgumentException("The following EVSE Ids are not unique: " + duplicateEVSEPricings.AggregateWith(", "), nameof(EVSEPricings));

            this.EVSEPricings  = EVSEPricings.Distinct();
            this.OperatorId    = OperatorId;
            this.OperatorName  = OperatorName?.Trim();

            this.CustomData    = CustomData;


            unchecked
            {

                hashCode = this.OperatorId.   GetHashCode()       * 5 ^
                          (this.OperatorName?.GetHashCode() ?? 0) * 3 ^
                           this.EVSEPricings. GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OperatorEVSEPricingType

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
                         out var operatorEVSEPricing,
                         out var errorResponse,
                         CustomOperatorEVSEPricingParser))
            {
                return operatorEVSEPricing;
            }

            throw new ArgumentException("The given JSON representation of operator EVSE pricing is invalid: " + errorResponse,
                                        nameof(JSON));

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
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out OperatorEVSEPricing?  OperatorEVSEPricing,
                                       [NotNullWhen(false)] out String?               ErrorResponse)

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
                                       [NotNullWhen(true)]  out OperatorEVSEPricing?      OperatorEVSEPricing,
                                       [NotNullWhen(false)] out String?                   ErrorResponse,
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

                #region Parse EVSEPricings    [mandatory]

                if (!JSON.ParseMandatoryJSON("EVSEPricing",
                                             "EVSE pricings",
                                             EVSEPricing.TryParse,
                                             out IEnumerable<EVSEPricing> EVSEPricings,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorId      [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "operator identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorName    [optional]

                var OperatorName = JSON.GetString("OperatorName");

                #endregion


                #region Parse CustomData      [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                OperatorEVSEPricing = new OperatorEVSEPricing(

                                          EVSEPricings,
                                          OperatorId,
                                          OperatorName,

                                          customData

                                      );


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

        #region ToJSON(CustomOperatorEVSEPricingSerializer = null, CustomEVSEPricingSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomOperatorEVSEPricingSerializer">A delegate to serialize custom operator EVSE pricing JSON objects.</param>
        /// <param name="CustomEVSEPricingSerializer">A delegate to serialize custom EVSE pricing JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<OperatorEVSEPricing>?  CustomOperatorEVSEPricingSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSEPricing>?          CustomEVSEPricingSerializer           = null)
        {

            var json = JSONObject.Create(

                           new JProperty("EVSEPricing",   new JArray(EVSEPricings.Select(evsePricing => evsePricing.ToJSON(CustomEVSEPricingSerializer)))),
                           new JProperty("OperatorID",    OperatorId.ToString()),
                           new JProperty("OperatorName",  OperatorName),

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomOperatorEVSEPricingSerializer is not null
                       ? CustomOperatorEVSEPricingSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this operator EVSE pricing.
        /// </summary>
        public OperatorEVSEPricing Clone()

            => new (

                   EVSEPricings.Select(evseDataRecord => evseDataRecord.Clone()),
                   OperatorId.   Clone(),
                   OperatorName?.CloneString(),

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OperatorEVSEPricing? OperatorEVSEPricing1,
                                           OperatorEVSEPricing? OperatorEVSEPricing2)
        {

            if (ReferenceEquals(OperatorEVSEPricing1, OperatorEVSEPricing2))
                return true;

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
        public static Boolean operator != (OperatorEVSEPricing? OperatorEVSEPricing1,
                                           OperatorEVSEPricing? OperatorEVSEPricing2)

            => !(OperatorEVSEPricing1 == OperatorEVSEPricing2);

        #endregion

        #region Operator <  (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (OperatorEVSEPricing? OperatorEVSEPricing1,
                                          OperatorEVSEPricing? OperatorEVSEPricing2)
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (OperatorEVSEPricing? OperatorEVSEPricing1,
                                           OperatorEVSEPricing? OperatorEVSEPricing2)

            => !(OperatorEVSEPricing1 > OperatorEVSEPricing2);

        #endregion

        #region Operator >  (OperatorEVSEPricing1, OperatorEVSEPricing2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorEVSEPricing1">An operator EVSE pricing.</param>
        /// <param name="OperatorEVSEPricing2">Another operator EVSE pricing.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (OperatorEVSEPricing? OperatorEVSEPricing1,
                                          OperatorEVSEPricing? OperatorEVSEPricing2)
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (OperatorEVSEPricing? OperatorEVSEPricing1,
                                           OperatorEVSEPricing? OperatorEVSEPricing2)

            => !(OperatorEVSEPricing1 < OperatorEVSEPricing2);

        #endregion

        #endregion

        #region IComparable<OperatorEVSEPricing> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two operator EVSE pricings.
        /// </summary>
        /// <param name="Object">A operator EVSE pricing to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is OperatorEVSEPricing operatorEVSEPricing
                   ? CompareTo(operatorEVSEPricing)
                   : throw new ArgumentException("The given object is not operator EVSE pricing!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(OperatorEVSEPricing)

        /// <summary>
        /// Compares two operator EVSE pricings.
        /// </summary>
        /// <param name="OperatorEVSEPricing">A operator EVSE pricing to compare with.</param>
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
        /// Compares two operator EVSE pricings for equality.
        /// </summary>
        /// <param name="Object">A operator EVSE pricing to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is OperatorEVSEPricing operatorEVSEPricing &&
                   Equals(operatorEVSEPricing);

        #endregion

        #region Equals(OperatorEVSEPricing)

        /// <summary>
        /// Compares two operator EVSE pricings for equality.
        /// </summary>
        /// <param name="OperatorEVSEPricing">A operator EVSE pricing to compare with.</param>
        public Boolean Equals(OperatorEVSEPricing? OperatorEVSEPricing)

            => OperatorEVSEPricing is not null &&

               OperatorId.  Equals(OperatorEVSEPricing.OperatorId)   &&

             ((OperatorName is     null && OperatorEVSEPricing.OperatorName is     null) ||
              (OperatorName is not null && OperatorEVSEPricing.OperatorName is not null && OperatorName.Equals(OperatorEVSEPricing.OperatorName))) &&

               EVSEPricings.Count().Equals(OperatorEVSEPricing.EVSEPricings.Count()) &&
               EVSEPricings.All(evseDataRecord => OperatorEVSEPricing.EVSEPricings.Contains(evseDataRecord));

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

            => String.Concat(

                   OperatorName.IsNotNullOrEmpty()
                       ? $"'{OperatorName}' ({OperatorId})"
                       : OperatorId,

                   $": {EVSEPricings.Count()} EVSE pricing record(s)"

               );

        #endregion

    }

}
