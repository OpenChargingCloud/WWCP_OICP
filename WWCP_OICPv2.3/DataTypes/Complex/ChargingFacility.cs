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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A charging facility.
    /// </summary>
    public class ChargingFacility : IEquatable<ChargingFacility>,
                                    IComparable<ChargingFacility>,
                                    IComparable
    {

        #region Properties

        /// <summary>
        /// The country.
        /// </summary>
        [Mandatory]
        public Country     Country            { get; }

        /// <summary>
        /// The city.
        /// </summary>
        [Mandatory]
        public String      City               { get; }

        /// <summary>
        /// The name of the street.
        /// </summary>
        [Mandatory]
        public String      Street             { get; }

        /// <summary>
        /// The postal code.
        /// </summary>
        [Mandatory]
        public String      PostalCode         { get; }

        /// <summary>
        /// The house number.
        /// </summary>
        [Mandatory]
        public String      HouseNumber        { get; }

        /// <summary>
        /// The optional floor level.
        /// </summary>
        [Optional]
        public String      Floor              { get; }

        /// <summary>
        /// The optional region.
        /// </summary>
        [Optional]
        public String      Region             { get; }

        /// <summary>
        /// Whether a parking facility exists.
        /// </summary>
        [Optional]
        public Boolean?    ParkingFacility    { get; }

        /// <summary>
        /// The optional parking spot.
        /// </summary>
        [Optional]
        public String      ParkingSpot        { get; }

        /// <summary>
        /// The optional time zone.
        /// </summary>
        [Optional]
        public Time_Zone?  TimeZone           { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject     CustomData         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging facility.
        /// </summary>
        /// <param name="Country">The country.</param>
        /// <param name="City">The city.</param>
        /// <param name="Street">The name of the street.</param>
        /// <param name="PostalCode">The postal code.</param>
        /// <param name="HouseNumber">The house number.</param>
        /// <param name="Floor">The optional floor level.</param>
        /// <param name="Region">The optional region.</param>
        /// <param name="ParkingFacility">Whether a parking facility exists.</param>
        /// <param name="ParkingSpot">The optional parking spot.</param>
        /// <param name="TimeZone">The optional time zone.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public ChargingFacility(Country     Country,
                       String      City,
                       String      Street,
                       String      PostalCode,
                       String      HouseNumber,
                       String      Floor             = null,
                       String      Region            = null,
                       Boolean?    ParkingFacility   = null,
                       String      ParkingSpot       = null,
                       Time_Zone?  TimeZone          = null,
                       JObject     CustomData        = null)

        {

            this.Country          = Country;
            this.City             = City;
            this.Street           = Street;
            this.PostalCode       = PostalCode;
            this.HouseNumber      = HouseNumber;
            this.Floor            = Floor;
            this.Region           = Region;
            this.ParkingFacility  = ParkingFacility;
            this.ParkingSpot      = ParkingSpot;
            this.TimeZone         = TimeZone;
            this.CustomData       = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#ChargingFacilityIso19773Type

        // {
        //   "City":            "string",
        //   "Country":         "string",
        //   "Floor":           "string",
        //   "HouseNum":        "string",
        //   "ParkingFacility":  false,
        //   "ParkingSpot":     "string",
        //   "PostalCode":      "string",
        //   "Region":          "string",
        //   "Street":          "string",
        //   "TimeZone":        "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomChargingFacilityParser = null)

        /// <summary>
        /// Parse the given JSON representation of an charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facility JSON objects.</param>
        public static ChargingFacility Parse(JObject                               JSON,
                                    CustomJObjectParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null)
        {

            if (TryParse(JSON,
                         out ChargingFacility chargingFacility,
                         out String  ErrorResponse,
                         CustomChargingFacilityParser))
            {
                return chargingFacility;
            }

            throw new ArgumentException("The given JSON representation of an charging facility is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomChargingFacilityParser = null)

        /// <summary>
        /// Parse the given text representation of an charging facility.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facility JSON objects.</param>
        public static ChargingFacility Parse(String                                Text,
                                    CustomJObjectParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null)
        {

            if (TryParse(Text,
                         out ChargingFacility chargingFacility,
                         out String  ErrorResponse,
                         CustomChargingFacilityParser))
            {
                return chargingFacility;
            }

            throw new ArgumentException("The given text representation of an charging facility is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParseJSON(JSONObject, ..., out ChargingFacility, out ErrorResponse, CustomChargingFacilityParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingFacility">The parsed charging facility.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject      JSON,
                                       out ChargingFacility  ChargingFacility,
                                       out String   ErrorResponse)

            => TryParse(JSON,
                        out ChargingFacility,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an charging facility.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingFacility">The parsed charging facility.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facilitys JSON objects.</param>
        public static Boolean TryParse(JObject                               JSON,
                                       out ChargingFacility                           ChargingFacility,
                                       out String                            ErrorResponse,
                                       CustomJObjectParserDelegate<ChargingFacility>  CustomChargingFacilityParser)
        {

            try
            {

                ChargingFacility = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Country           [mandatory]

                if (!JSON.ParseMandatory("Country",
                                         "country",
                                         org.GraphDefined.Vanaheimr.Illias.Country.TryParse,
                                         out Country Country,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse City              [mandatory]

                if (!JSON.ParseMandatoryText("City",
                                             "city",
                                             out String City,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Street            [mandatory]

                if (!JSON.ParseMandatoryText("Street",
                                             "street",
                                             out String Street,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PostalCode        [mandatory]

                if (!JSON.ParseMandatoryText("PostalCode",
                                             "postal code",
                                             out String PostalCode,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse HouseNumber       [mandatory]

                if (!JSON.ParseMandatoryText("HouseNum",
                                             "house number",
                                             out String HouseNumber,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Floor             [optional]

                if (JSON.ParseOptional("Floor",
                                       "floor",
                                       out String Floor,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse Region            [optional]

                if (JSON.ParseOptional("Region",
                                       "region",
                                       out String Region,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse ParkingFacility   [optional]

                if (JSON.ParseOptional("ParkingFacility",
                                       "parking facility",
                                       out Boolean? ParkingFacility,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse ParkingSpot       [optional]

                if (JSON.ParseOptional("ParkingSpot",
                                       "parking spot",
                                       out String ParkingSpot,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse TimeZone          [optional]

                if (JSON.ParseOptional("TimeZone",
                                       "time zone",
                                       Time_Zone.TryParse,
                                       out Time_Zone TimeZone,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse Custom Data       [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                ChargingFacility = new ChargingFacility(Country,
                                      City,
                                      Street,
                                      PostalCode,
                                      HouseNumber,
                                      Floor,
                                      Region,
                                      ParkingFacility,
                                      ParkingSpot,
                                      TimeZone,
                                      CustomData);

                if (CustomChargingFacilityParser != null)
                    ChargingFacility = CustomChargingFacilityParser(JSON,
                                                  ChargingFacility);

                return true;

            }
            catch (Exception e)
            {
                ChargingFacility        = default;
                ErrorResponse  = "The given JSON representation of an charging facility is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out ChargingFacility, out ErrorResponse, CustomChargingFacilityParser = null)

        /// <summary>
        /// Try to parse the given text representation of a charging facility.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ChargingFacility">The parsed charging facility.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom charging facilitys JSON objects.</param>
        public static Boolean TryParse(String                                Text,
                                       out ChargingFacility                           ChargingFacility,
                                       out String                            ErrorResponse,
                                       CustomJObjectParserDelegate<ChargingFacility>  CustomChargingFacilityParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out ChargingFacility,
                                out ErrorResponse,
                                CustomChargingFacilityParser);

            }
            catch (Exception e)
            {
                ChargingFacility        = default;
                ErrorResponse  = "The given text representation of an charging facility is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomChargingFacilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingFacility> CustomChargingFacilitySerializer = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("Country",                Country.Alpha3Code),
                           new JProperty("City",                   City),
                           new JProperty("Street",                 Street),
                           new JProperty("PostalCode",             PostalCode),
                           new JProperty("HouseNum",               HouseNumber),

                           Floor.IsNotNullOrEmpty()
                               ? new JProperty("Floor",            Floor)
                               : null,

                           Region.IsNotNullOrEmpty()
                               ? new JProperty("Region",           Region)
                               : null,

                           ParkingFacility.HasValue
                               ? new JProperty("ParkingFacility",  ParkingFacility.Value)
                               : null,

                           ParkingSpot.IsNotNullOrEmpty()
                               ? new JProperty("ParkingSpot",      ParkingSpot)
                               : null,

                           TimeZone.HasValue
                               ? new JProperty("TimeZone",         TimeZone.ToString())
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",       CustomData)
                               : null

                );

            return CustomChargingFacilitySerializer != null
                       ? CustomChargingFacilitySerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two charging facilityes for equality.
        /// </summary>
        /// <param name="ChargingFacility1">An charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingFacility1, ChargingFacility2))
                return true;

            if (ChargingFacility1 is null || ChargingFacility2 is null)
                return false;

            return ChargingFacility1.Equals(ChargingFacility2);

        }

        #endregion

        #region Operator != (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two charging facilityes for inequality.
        /// </summary>
        /// <param name="ChargingFacility1">An charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
            => !(ChargingFacility1 == ChargingFacility2);

        #endregion

        #region Operator <  (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">An charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
        {

            if (ChargingFacility1 is null)
                throw new ArgumentNullException(nameof(ChargingFacility1), "The given charging facility must not be null!");

            return ChargingFacility1.CompareTo(ChargingFacility2) < 0;

        }

        #endregion

        #region Operator <= (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">An charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
            => !(ChargingFacility1 > ChargingFacility2);

        #endregion

        #region Operator >  (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">An charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
        {

            if (ChargingFacility1 is null)
                throw new ArgumentNullException(nameof(ChargingFacility1), "The given charging facility must not be null!");

            return ChargingFacility1.CompareTo(ChargingFacility2) > 0;

        }

        #endregion

        #region Operator >= (ChargingFacility1, ChargingFacility2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility1">An charging facility.</param>
        /// <param name="ChargingFacility2">Another charging facility.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingFacility ChargingFacility1, ChargingFacility ChargingFacility2)
            => !(ChargingFacility1 < ChargingFacility2);

        #endregion

        #endregion

        #region IComparable<ChargingFacility> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is ChargingFacility chargingFacility
                   ? CompareTo(chargingFacility)
                   : throw new ArgumentException("The given object is not an charging facility!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingFacility)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingFacility">An object to compare with.</param>
        public Int32 CompareTo(ChargingFacility ChargingFacility)
        {

            if (ChargingFacility is null)
                throw new ArgumentNullException(nameof(ChargingFacility), "The given charging facility must not be null!");

            var c = Country.     CompareTo(ChargingFacility.Country);
            if (c != 0)
                return c;

            c = City.CompareTo(ChargingFacility.City);
            if (c != 0)
                return c;

            c = Street.CompareTo(ChargingFacility.Street);
            if (c != 0)
                return c;

            c = PostalCode.CompareTo(ChargingFacility.PostalCode);
            if (c != 0)
                return c;

            c = HouseNumber.CompareTo(ChargingFacility.HouseNumber);
            if (c != 0)
                return c;

            c = (Floor ?? "").CompareTo(ChargingFacility.Floor ?? "");
            if (c != 0)
                return c;

            c = (Floor ?? "").CompareTo(ChargingFacility.Floor ?? "");
            if (c != 0)
                return c;

            c = (Region ?? "").CompareTo(ChargingFacility.Region ?? "");
            if (c != 0)
                return c;

            //c = (ParkingFacility ?? "").CompareTo(ChargingFacility.ParkingFacility ?? "");
            //if (c != 0)
            //    return c;

            return (TimeZone?.ToString() ?? "").CompareTo(ChargingFacility.TimeZone?.ToString() ?? "");

        }

        #endregion

        #endregion

        #region IEquatable<ChargingFacility> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ChargingFacility chargingFacility &&
                   Equals(chargingFacility);

        #endregion

        #region Equals(ChargingFacility)

        /// <summary>
        /// Compares two charging facilityes for equality.
        /// </summary>
        /// <param name="ChargingFacility">An charging facility to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingFacility ChargingFacility)

            => !(ChargingFacility is null) &&
                 Country         == ChargingFacility.Country         &&
                 City            == ChargingFacility.City            &&
                 Street          == ChargingFacility.Street          &&
                 PostalCode      == ChargingFacility.PostalCode      &&
                 HouseNumber     == ChargingFacility.HouseNumber     &&
                 Floor           == ChargingFacility.Floor           &&
                 Region          == ChargingFacility.Region          &&
                 ParkingFacility == ChargingFacility.ParkingFacility &&
                 ParkingSpot     == ChargingFacility.ParkingSpot     &&
                 TimeZone        == ChargingFacility.TimeZone;

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

                return Country.         GetHashCode()       * 27 ^
                       City.            GetHashCode()       * 23 ^
                       Street.          GetHashCode()       * 19 ^
                       PostalCode.      GetHashCode()       * 17^
                       HouseNumber.     GetHashCode()       * 13 ^

                      (Floor?.          GetHashCode() ?? 0) * 11 ^
                      (Region?.         GetHashCode() ?? 0) *  7 ^
                      (ParkingFacility?.GetHashCode() ?? 0) *  5 ^
                      (ParkingSpot?.    GetHashCode() ?? 0) *  3 ^
                      (TimeZone?.       GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Country, " ",
                             City,    " ",
                             Street,  " ",
                             HouseNumber);

        #endregion

    }

}
