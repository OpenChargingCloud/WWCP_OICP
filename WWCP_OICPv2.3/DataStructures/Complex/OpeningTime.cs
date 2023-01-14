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
    /// An opening time.
    /// </summary>
    public class OpeningTime : IEquatable<OpeningTime>
    {

        #region Properties

        /// <summary>
        /// The periods of the opening time.
        /// </summary>
        [Mandatory]
        public IEnumerable<Period>  Periods                    { get; }

        /// <summary>
        /// The day of the weeks of the opening time.
        /// </summary>
        [Mandatory]
        public DaysOfWeek           On                         { get; }

        /// <summary>
        /// Optional unstructured information about the opening time.
        /// </summary>
        [Optional]
        public String?              UnstructuredOpeningTime    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?             CustomData                 { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new opening time.
        /// </summary>
        /// <param name="Periods">The periods of the opening time.</param>
        /// <param name="On">The day of the weeks of the opening time.</param>
        /// <param name="UnstructuredText">Optional unstructured information about the opening time.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public OpeningTime(IEnumerable<Period>  Periods,
                           DaysOfWeek           On,
                           String?              UnstructuredText   = null,
                           JObject?             CustomData         = null)
        {

            if (Periods.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Periods), "The given enumeration of periods must not be null or empty!");

            this.Periods                  = Periods.Distinct();
            this.On                       = On;
            this.UnstructuredOpeningTime  = UnstructuredText;
            this.CustomData               = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#OpeningTimesType

        // {
        //   "Period": [
        //     {
        //       "begin": "07:00",
        //       "end":   "11:30"
        //     }
        //   ],
        //   "on":                       "Monday",
        //   "unstructuredOpeningTime":  "Our monday morning opening: 07:00 - 11:30!"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomOpeningTimesParser = null)

        /// <summary>
        /// Parse the given JSON representation of an opening time.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomOpeningTimesParser">A delegate to parse custom opening times JSON objects.</param>
        public static OpeningTime Parse(JObject                                    JSON,
                                        CustomJObjectParserDelegate<OpeningTime>?  CustomOpeningTimesParser   = null)
        {

            if (TryParse(JSON,
                         out var openingTime,
                         out var errorResponse,
                         CustomOpeningTimesParser))
            {
                return openingTime!;
            }

            throw new ArgumentException("The given JSON representation of an opening time is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomOpeningTimesParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of an opening time.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomOpeningTimesParser">A delegate to parse custom opening times JSON objects.</param>
        public static OpeningTime? TryParse(JObject                                    JSON,
                                            CustomJObjectParserDelegate<OpeningTime>?  CustomOpeningTimesParser   = null)
        {

            if (TryParse(JSON,
                         out var openingTime,
                         out _,
                         CustomOpeningTimesParser))
            {
                return openingTime;
            }

            return default;

        }

        #endregion

        #region (static) TryParse(JSON, out OpeningTimes, out ErrorResponse, CustomOpeningTimesParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an opening time.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OpeningTimes">The parsed opening time.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject           JSON,
                                       out OpeningTime?  OpeningTimes,
                                       out String?       ErrorResponse)

            => TryParse(JSON,
                        out OpeningTimes,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an opening time.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OpeningTimes">The parsed opening time.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOpeningTimesParser">A delegate to parse custom opening times JSON objects.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       out OpeningTime?                           OpeningTimes,
                                       out String?                                ErrorResponse,
                                       CustomJObjectParserDelegate<OpeningTime>?  CustomOpeningTimesParser)
        {

            try
            {

                OpeningTimes = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Periods               [mandatory]

                if (!JSON.ParseMandatoryJSON("Period",
                                             "periods",
                                             Period.TryParse,
                                             out IEnumerable<Period> Periods,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse On                    [mandatory]

                if (!JSON.ParseMandatoryEnum("on",
                                             "on days of the week",
                                             out DaysOfWeek On,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Unstructured Text     [optional]

                var UnstructuredText  = JSON["unstructuredOpeningTime"]?.Value<String>();

                #endregion#

                #region Parse CustomData            [optional]

                var CustomData        = JSON["CustomData"] as JObject;

                #endregion


                OpeningTimes = new OpeningTime(Periods,
                                               On,
                                               UnstructuredText,
                                               CustomData);


                if (CustomOpeningTimesParser is not null)
                    OpeningTimes = CustomOpeningTimesParser(JSON,
                                                            OpeningTimes);

                return true;

            }
            catch (Exception e)
            {
                OpeningTimes   = default;
                ErrorResponse  = "The given JSON representation of an opening time is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomOpeningTimesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<OpeningTime>?  CustomOpeningTimesSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           Periods.SafeAny()
                               ? new JProperty("Period", new JArray(Periods.Select(period => period.ToJSON())))
                               : null,

                           new JProperty("on",  On.AsString()),

                           UnstructuredOpeningTime.IsNotNullOrEmpty()
                               ? new JProperty("unstructuredOpeningTime",  UnstructuredOpeningTime)
                               : null

                       );

            return CustomOpeningTimesSerializer is not null
                       ? CustomOpeningTimesSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public OpeningTime Clone

            => new (Periods.SafeSelect(period => period.Clone).ToArray(),
                    On,
                    UnstructuredOpeningTime is not null
                        ? new String(UnstructuredOpeningTime.ToCharArray())
                        : null,
                    CustomData is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (OpeningTimes1, OpeningTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OpeningTimes1">An opening time.</param>
        /// <param name="OpeningTimes2">Another opening time.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (OpeningTime? OpeningTimes1,
                                           OpeningTime? OpeningTimes2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(OpeningTimes1, OpeningTimes2))
                return true;

            if (OpeningTimes1 is null || OpeningTimes2 is null)
                return false;

            return OpeningTimes1.Equals(OpeningTimes2);

        }

        #endregion

        #region Operator != (OpeningTimes1, OpeningTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OpeningTimes1">An opening time.</param>
        /// <param name="OpeningTimes2">Another opening time.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (OpeningTime? OpeningTimes1,
                                           OpeningTime? OpeningTimes2)

            => !(OpeningTimes1 == OpeningTimes2);

        #endregion

        #endregion

        #region IEquatable<OpeningTimes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two opening times for equality.
        /// </summary>
        /// <param name="Object">An opening time to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is OpeningTime openingTime &&
                   Equals(openingTime);

        #endregion

        #region Equals(OpeningTimes)

        /// <summary>
        /// Compares two opening times for equality.
        /// </summary>
        /// <param name="OpeningTimes">An opening time to compare with.</param>
        public Boolean Equals(OpeningTime? OpeningTimes)

            => OpeningTimes is not null &&

               Periods.Count().Equals(OpeningTimes.Periods.Count()) &&
               Periods.All(period => OpeningTimes.Periods.Contains(period)) &&

               On.Equals(OpeningTimes.On) &&

             ((UnstructuredOpeningTime is     null && OpeningTimes.UnstructuredOpeningTime is     null) ||
              (UnstructuredOpeningTime is not null && OpeningTimes.UnstructuredOpeningTime is not null && UnstructuredOpeningTime.Equals(OpeningTimes.UnstructuredOpeningTime)));

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

                return Periods.Aggregate(0, (hashCode, period) => hashCode ^ period.GetHashCode()) ^
                       On.     GetHashCode() * 3 ^

                       (UnstructuredOpeningTime is not null && UnstructuredOpeningTime.IsNullOrEmpty()
                           ? UnstructuredOpeningTime.GetHashCode()
                           : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Periods.AggregateWith(", "),
                             " -> ",
                             On.AsString(),
                             UnstructuredOpeningTime.IsNotNullOrEmpty()
                                 ? "; " + UnstructuredOpeningTime
                                 : "");

        #endregion

    }

}
