/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
    /// An opening time.
    /// </summary>
    public readonly struct OpeningTime : IEquatable<OpeningTime>
    {

        #region Properties

        /// <summary>
        /// The periods of the opening time.
        /// </summary>
        [Mandatory]
        public IEnumerable<Period>  Periods             { get; }

        /// <summary>
        /// The day of the weeks of the opening time.
        /// </summary>
        [Mandatory]
        public DaysOfWeek           On                  { get; }

        /// <summary>
        /// Optional unstructured information about the opening time.
        /// </summary>
        public String               UnstructuredText    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject              CustomData          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new opening time.
        /// </summary>
        /// <param name="Periods">The periods of the opening time.</param>
        /// <param name="On">The day of the weeks of the opening time.</param>
        /// <param name="UnstructuredText">Optional unstructured information about the opening time.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public OpeningTime(IEnumerable<Period>  Periods,
                           DaysOfWeek           On,
                           String               UnstructuredText,
                           JObject              CustomData  = null)
        {

            if (Periods.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Periods), "The given enumeration of periods must not be null or empty!");

            this.Periods           = Periods;
            this.On                = On;
            this.UnstructuredText  = UnstructuredText;
            this.CustomData        = CustomData;

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
        public static OpeningTime Parse(JObject                                        JSON,
                                             CustomJObjectParserDelegate<OpeningTime>  CustomOpeningTimesParser   = null)
        {

            if (TryParse(JSON,
                         out OpeningTime openingTime,
                         out String      ErrorResponse,
                         CustomOpeningTimesParser))
            {
                return openingTime;
            }

            throw new ArgumentException("The given JSON representation of an opening time is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomOpeningTimesParser = null)

        /// <summary>
        /// Parse the given text representation of an opening time.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomOpeningTimesParser">A delegate to parse custom opening times JSON objects.</param>
        public static OpeningTime Parse(String                                         Text,
                                             CustomJObjectParserDelegate<OpeningTime>  CustomOpeningTimesParser   = null)
        {

            if (TryParse(Text,
                         out OpeningTime openingTime,
                         out String      ErrorResponse,
                         CustomOpeningTimesParser))
            {
                return openingTime;
            }

            throw new ArgumentException("The given text representation of an opening time is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out OpeningTimes, out ErrorResponse, CustomOpeningTimesParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an opening time.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OpeningTimes">The parsed connector.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject          JSON,
                                       out OpeningTime  OpeningTimes,
                                       out String       ErrorResponse)

            => TryParse(JSON,
                        out OpeningTimes,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an opening time.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OpeningTimes">The parsed connector.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOpeningTimesParser">A delegate to parse custom opening times JSON objects.</param>
        public static Boolean TryParse(JObject                                   JSON,
                                       out OpeningTime                           OpeningTimes,
                                       out String                                ErrorResponse,
                                       CustomJObjectParserDelegate<OpeningTime>  CustomOpeningTimesParser)
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

                if (!JSON.ParseMandatoryJSON("EvseID",
                                             "EVSE identification",
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

                #region Parse Custom Data           [optional]

                var CustomData        = JSON["CustomData"] as JObject;

                #endregion


                OpeningTimes = new OpeningTime(Periods,
                                               On,
                                               UnstructuredText,
                                               CustomData);


                if (CustomOpeningTimesParser != null)
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

        #region (static) TryParse(Text, out OpeningTimes, out ErrorResponse, CustomOpeningTimesParser = null)

        /// <summary>
        /// Try to parse the given text representation of an opening time.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="OpeningTimes">The parsed connector.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomOpeningTimesParser">A delegate to parse custom opening times JSON objects.</param>
        public static Boolean TryParse(String                                    Text,
                                       out OpeningTime                           OpeningTimes,
                                       out String                                ErrorResponse,
                                       CustomJObjectParserDelegate<OpeningTime>  CustomOpeningTimesParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out OpeningTimes,
                                out ErrorResponse,
                                CustomOpeningTimesParser);

            }
            catch (Exception e)
            {
                OpeningTimes   = default;
                ErrorResponse  = "The given text representation of an opening time is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomOpeningTimesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<OpeningTime> CustomOpeningTimesSerializer = null)
        {

            var JSON = JSONObject.Create(

                           Periods.SafeAny()
                               ? new JProperty("Period", new JArray(Periods.Select(period => period.ToJSON())))
                               : null,

                           new JProperty("on",  On.AsString()),

                           UnstructuredText.IsNotNullOrEmpty()
                               ? new JProperty("unstructuredOpeningTime",  UnstructuredText)
                               : null

                       );

            return CustomOpeningTimesSerializer != null
                       ? CustomOpeningTimesSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (OpeningTimes1, OpeningTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OpeningTimes1">An opening time.</param>
        /// <param name="OpeningTimes2">Another opening time.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (OpeningTime OpeningTimes1, OpeningTime OpeningTimes2)
            => OpeningTimes1.Equals(OpeningTimes2);

        #endregion

        #region Operator != (OpeningTimes1, OpeningTimes2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OpeningTimes1">An opening time.</param>
        /// <param name="OpeningTimes2">Another opening time.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (OpeningTime OpeningTimes1, OpeningTime OpeningTimes2)
            => !(OpeningTimes1 == OpeningTimes2);

        #endregion

        #endregion

        #region IEquatable<OpeningTimes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is OpeningTime openingTime &&
                   Equals(openingTime);

        #endregion

        #region Equals(OpeningTimes)

        /// <summary>
        /// Compares two opening times for equality.
        /// </summary>
        /// <param name="OpeningTimes">An opening time to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OpeningTime OpeningTimes)

            => Periods.Count().Equals(OpeningTimes.Periods.Count()) &&
               Periods.All(period => OpeningTimes.Periods.Contains(period)) &&

               On.Equals(OpeningTimes.On) &&

            ((!UnstructuredText.IsNullOrEmpty() && !OpeningTimes.UnstructuredText.IsNullOrEmpty()) ||
              (UnstructuredText.IsNullOrEmpty() &&  OpeningTimes.UnstructuredText.IsNullOrEmpty() && UnstructuredText.Equals(OpeningTimes.UnstructuredText)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Periods.Aggregate(0, (hashCode, price) => hashCode ^ price.GetHashCode()) ^
                       On.     GetHashCode() * 3 ^

                       (UnstructuredText.IsNullOrEmpty()
                           ? UnstructuredText.GetHashCode()
                           : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Periods.AggregateWith(", "),
                             " -> ",
                             On.AsString(),
                             UnstructuredText.IsNotNullOrEmpty()
                                 ? "; " + UnstructuredText
                                 : "");

        #endregion

    }

}
