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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Product availability times.
    /// </summary>
    public readonly struct ProductAvailabilityTimes : IEquatable<ProductAvailabilityTimes>,
                                                      IComparable<ProductAvailabilityTimes>,
                                                      IComparable
    {

        #region Properties

        /// <summary>
        /// The starting and end time for pricing product applicability in the specified period.
        /// </summary>
        [Mandatory]
        public readonly Period   Period    { get; }

        /// <summary>
        /// Day values to be used in specifying periods on which the product is available.
        /// </summary>
        [Optional]
        public readonly WeekDay  On        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy source.
        /// </summary>
        /// <param name="EnergyType">The energy type.</param>
        /// <param name="Percentage">Percentage of EnergyType being used by the charging stations.</param>
        public ProductAvailabilityTimes(Period   Period,
                                        WeekDay  On)
        {
            this.Period  = Period;
            this.On      = On;
        }

        #endregion


        //ToDo: Unclear data structure?!

        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#ProductAvailabilityTimesType

        // {
        //     "Period":  {
        //        "begin":  "01:00",
        //        "end":    "06:00",
        //     },
        //     "on":      "Monday"
        // }

        //ToDo: Unclear which one is correct?!

        //         {
        //             "Periods": [
        //                 {
        //                     "begin":  "09:00",
        //                     "end":    "18:00"
        //                 }
        //             ],
        //             "on": "Everyday"
        //         }

        #endregion

        #region (static) Parse   (JSON, CustomProductAvailabilityTimesParser = null)

        /// <summary>
        /// Parse the given JSON representation of a product availability times object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomProductAvailabilityTimesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static ProductAvailabilityTimes Parse(JObject                                                 JSON,
                                                     CustomJObjectParserDelegate<ProductAvailabilityTimes>?  CustomProductAvailabilityTimesParser   = null)
        {

            if (TryParse(JSON,
                         out var productAvailabilityTimes,
                         out var errorResponse,
                         CustomProductAvailabilityTimesParser))
            {
                return productAvailabilityTimes;
            }

            throw new ArgumentException("The given JSON representation of a product availability times object is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomProductAvailabilityTimesParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a product availability times object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomProductAvailabilityTimesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static ProductAvailabilityTimes? TryParse(JObject                                                 JSON,
                                                         CustomJObjectParserDelegate<ProductAvailabilityTimes>?  CustomProductAvailabilityTimesParser   = null)
        {

            if (TryParse(JSON,
                         out ProductAvailabilityTimes productAvailabilityTimes,
                         out _,
                         CustomProductAvailabilityTimesParser))
            {
                return productAvailabilityTimes;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out ProductAvailabilityTimes, out ErrorResponse, CustomProductAvailabilityTimesParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a product availability times object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProductAvailabilityTimes">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                       JSON,
                                       out ProductAvailabilityTimes  ProductAvailabilityTimes,
                                       out String?                   ErrorResponse)

            => TryParse(JSON,
                        out ProductAvailabilityTimes,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a product availability times object.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ProductAvailabilityTimes">The parsed energy source.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomProductAvailabilityTimesParser">A delegate to parse custom energy sources JSON objects.</param>
        public static Boolean TryParse(JObject                                                 JSON,
                                       out ProductAvailabilityTimes                            ProductAvailabilityTimes,
                                       out String?                                             ErrorResponse,
                                       CustomJObjectParserDelegate<ProductAvailabilityTimes>?  CustomProductAvailabilityTimesParser)
        {

            try
            {

                ProductAvailabilityTimes = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Period    [mandatory]

                if (!JSON.ParseMandatoryJSON("Period",
                                             "period of time",
                                             OICPv2_3.Period.TryParse,
                                             out Period Period,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse On        [mandatory]

                if (!JSON.ParseMandatory("on",
                                         "on week day(s)",
                                         OICPv2_3.WeekDay.TryParse,
                                         out WeekDay WeekDay,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                ProductAvailabilityTimes = new ProductAvailabilityTimes(Period,
                                                                        WeekDay);


                if (CustomProductAvailabilityTimesParser is not null)
                    ProductAvailabilityTimes = CustomProductAvailabilityTimesParser(JSON,
                                                                                    ProductAvailabilityTimes);

                return true;

            }
            catch (Exception e)
            {
                ProductAvailabilityTimes  = default;
                ErrorResponse             = "The given JSON representation of a product availability times object is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomProductAvailabilityTimesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomProductAvailabilityTimesSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ProductAvailabilityTimes>? CustomProductAvailabilityTimesSerializer = null)
        {

            var json = JSONObject.Create(
                           new JProperty("Period",  Period.ToJSON()),
                           new JProperty("on",      On.    ToString())
                       );

            return CustomProductAvailabilityTimesSerializer is not null
                       ? CustomProductAvailabilityTimesSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public ProductAvailabilityTimes Clone

            => new (Period.Clone,
                    On);

        #endregion


        #region Operator overloading

        #region Operator == (ProductAvailabilityTimes1, ProductAvailabilityTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes1">An energy source.</param>
        /// <param name="ProductAvailabilityTimes2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ProductAvailabilityTimes ProductAvailabilityTimes1,
                                           ProductAvailabilityTimes ProductAvailabilityTimes2)

            => ProductAvailabilityTimes1.Equals(ProductAvailabilityTimes2);

        #endregion

        #region Operator != (ProductAvailabilityTimes1, ProductAvailabilityTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes1">An energy source.</param>
        /// <param name="ProductAvailabilityTimes2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ProductAvailabilityTimes ProductAvailabilityTimes1,
                                           ProductAvailabilityTimes ProductAvailabilityTimes2)

            => !ProductAvailabilityTimes1.Equals(ProductAvailabilityTimes2);

        #endregion

        #region Operator <  (ProductAvailabilityTimes1, ProductAvailabilityTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes1">An energy source.</param>
        /// <param name="ProductAvailabilityTimes2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ProductAvailabilityTimes ProductAvailabilityTimes1,
                                          ProductAvailabilityTimes ProductAvailabilityTimes2)

            => ProductAvailabilityTimes1.CompareTo(ProductAvailabilityTimes2) < 0;

        #endregion

        #region Operator <= (ProductAvailabilityTimes1, ProductAvailabilityTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes1">An energy source.</param>
        /// <param name="ProductAvailabilityTimes2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ProductAvailabilityTimes ProductAvailabilityTimes1,
                                           ProductAvailabilityTimes ProductAvailabilityTimes2)

            => ProductAvailabilityTimes1.CompareTo(ProductAvailabilityTimes2) <= 0;

        #endregion

        #region Operator >  (ProductAvailabilityTimes1, ProductAvailabilityTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes1">An energy source.</param>
        /// <param name="ProductAvailabilityTimes2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ProductAvailabilityTimes ProductAvailabilityTimes1,
                                          ProductAvailabilityTimes ProductAvailabilityTimes2)

            => ProductAvailabilityTimes1.CompareTo(ProductAvailabilityTimes2) > 0;

        #endregion

        #region Operator >= (ProductAvailabilityTimes1, ProductAvailabilityTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes1">An energy source.</param>
        /// <param name="ProductAvailabilityTimes2">Another energy source.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ProductAvailabilityTimes ProductAvailabilityTimes1,
                                           ProductAvailabilityTimes ProductAvailabilityTimes2)

            => ProductAvailabilityTimes1.CompareTo(ProductAvailabilityTimes2) >= 0;

        #endregion

        #endregion

        #region IComparable<ProductAvailabilityTimes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ProductAvailabilityTimes productAvailabilityTimes
                   ? CompareTo(productAvailabilityTimes)
                   : throw new ArgumentException("The given object is not a product availability times object!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ProductAvailabilityTimes)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProductAvailabilityTimes">An object to compare with.</param>
        public Int32 CompareTo(ProductAvailabilityTimes ProductAvailabilityTimes)
        {

            var c = On.CompareTo(ProductAvailabilityTimes.On);

            if (c == 0)
                c = Period.CompareTo(ProductAvailabilityTimes.Period);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ProductAvailabilityTimes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ProductAvailabilityTimes productAvailabilityTimes &&
                   Equals(productAvailabilityTimes);

        #endregion

        #region Equals(ProductAvailabilityTimes)

        /// <summary>
        /// Compares two ProductAvailabilityTimess for equality.
        /// </summary>
        /// <param name="ProductAvailabilityTimes">An energy source to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ProductAvailabilityTimes ProductAvailabilityTimes)

            => On.    Equals(ProductAvailabilityTimes.On) &&
               Period.Equals(ProductAvailabilityTimes.Period);

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

                return On.    GetHashCode() * 3 ^
                       Period.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(On, ": ", Period);

        #endregion

    }

}
