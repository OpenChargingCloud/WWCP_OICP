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
    /// A time period.
    /// </summary>
    public readonly struct Period : IEquatable<Period>,
                                    IComparable<Period>,
                                    IComparable
    {

        #region Properties

        /// <summary>
        /// The begin of the period.
        /// </summary>
        [Mandatory]
        public readonly HourMinute  Begin    { get; }

        /// <summary>
        /// The end of the period.
        /// </summary>
        [Mandatory]
        public readonly HourMinute  End      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new time period.
        /// </summary>
        /// <param name="Begin">The begin of the period.</param>
        /// <param name="End">The end of the period.</param>
        public Period(HourMinute Begin,
                      HourMinute End)
        {

            this.Begin  = Begin;
            this.End    = End;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#PeriodType

        // {
        //     "begin": "08:00",
        //     "end":   "19:30"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomPeriodParser = null)

        /// <summary>
        /// Parse the given JSON representation of a time period.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPeriodParser">A delegate to parse custom time periods JSON objects.</param>
        public static Period Parse(JObject                               JSON,
                                   CustomJObjectParserDelegate<Period>?  CustomPeriodParser   = null)
        {

            if (TryParse(JSON,
                         out var period,
                         out var errorResponse,
                         CustomPeriodParser))
            {
                return period;
            }

            throw new ArgumentException("The given JSON representation of a time period is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomPeriodParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a time period.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPeriodParser">A delegate to parse custom time periods JSON objects.</param>
        public static Period? TryParse(JObject                               JSON,
                                       CustomJObjectParserDelegate<Period>?  CustomPeriodParser   = null)
        {

            if (TryParse(JSON,
                         out var period,
                         out _,
                         CustomPeriodParser))
            {
                return period;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out Period, out ErrorResponse, CustomPeriodParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a time period.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Period">The parsed time period.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject      JSON,
                                       out Period   Period,
                                       out String?  ErrorResponse)

            => TryParse(JSON,
                        out Period,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a time period.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Period">The parsed time period.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPeriodParser">A delegate to parse custom time periods JSON objects.</param>
        public static Boolean TryParse(JObject                               JSON,
                                       out Period                            Period,
                                       out String?                           ErrorResponse,
                                       CustomJObjectParserDelegate<Period>?  CustomPeriodParser)
        {

            try
            {

                Period = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Begin     [mandatory]

                if (!JSON.ParseMandatory("begin",
                                         "period begin",
                                         HourMinute.TryParse,
                                         out HourMinute Begin,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse End       [mandatory]

                if (!JSON.ParseMandatory("end",
                                         "period end",
                                         HourMinute.TryParse,
                                         out HourMinute End,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                Period = new Period(Begin,
                                    End);

                if (CustomPeriodParser is not null)
                    Period = CustomPeriodParser(JSON,
                                                Period);

                return true;

            }
            catch (Exception e)
            {
                Period         = default;
                ErrorResponse  = "The given JSON representation of a time period is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPeriodSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPeriodSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<Period>?  CustomPeriodSerializer   = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("begin",  Begin.ToString()),
                           new JProperty("end",    End.  ToString())
                       );

            return CustomPeriodSerializer is not null
                       ? CustomPeriodSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public Period Clone

            => new (Begin,
                    End);

        #endregion


        #region Operator overloading

        #region Operator == (Period1, Period2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Period1">A time period.</param>
        /// <param name="Period2">Another time period.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Period Period1,
                                           Period Period2)

            => Period1.Equals(Period2);

        #endregion

        #region Operator != (Period1, Period2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Period1">A time period.</param>
        /// <param name="Period2">Another time period.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Period Period1,
                                           Period Period2)

            => !Period1.Equals(Period2);

        #endregion

        #region Operator <  (Period1, Period2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Period1">A time period.</param>
        /// <param name="Period2">Another time period.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Period Period1,
                                          Period Period2)

            => Period1.CompareTo(Period2) < 0;

        #endregion

        #region Operator <= (Period1, Period2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Period1">A time period.</param>
        /// <param name="Period2">Another time period.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Period Period1,
                                           Period Period2)

            => Period1.CompareTo(Period2) <= 0;

        #endregion

        #region Operator >  (Period1, Period2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Period1">A time period.</param>
        /// <param name="Period2">Another time period.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Period Period1,
                                          Period Period2)

            => Period1.CompareTo(Period2) > 0;

        #endregion

        #region Operator >= (Period1, Period2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Period1">A time period.</param>
        /// <param name="Period2">Another time period.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Period Period1,
                                           Period Period2)

            => Period1.CompareTo(Period2) >= 0;

        #endregion

        #endregion

        #region IComparable<Period> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two time periods.
        /// </summary>
        /// <param name="Object">A time period to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Period period
                   ? CompareTo(period)
                   : throw new ArgumentException("The given object is not a time period!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Period)

        /// <summary>
        /// Compares two time periods.
        /// </summary>
        /// <param name="Period">A time period to compare with.</param>
        public Int32 CompareTo(Period Period)
        {

            var c =    Begin.CompareTo(Period.Begin);

            if (c == 0)
                return End.  CompareTo(Period.End);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<Period> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two time periods for equality.
        /// </summary>
        /// <param name="Object">A time period to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Period period &&
                   Equals(period);

        #endregion

        #region Equals(Period)

        /// <summary>
        /// Compares two time periods for equality.
        /// </summary>
        /// <param name="Period">A time period to compare with.</param>
        public Boolean Equals(Period Period)

            => Begin.Equals(Period.Begin) &&
               End.  Equals(Period.End);

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
                return Begin.GetHashCode() * 3 ^
                       End.  GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Begin,
                             " -> ",
                             End);

        #endregion

    }

}
