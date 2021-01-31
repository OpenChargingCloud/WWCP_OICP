/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
    /// An address.
    /// </summary>
    public class Address : IEquatable<Address>,
                           IComparable<Address>,
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
        /// Create a new address.
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
        public Address(Country     Country,
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

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#AddressIso19773Type

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

        #region (static) Parse   (JSON, CustomAddressParser = null)

        /// <summary>
        /// Parse the given JSON representation of an address.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom address JSON objects.</param>
        public static Address Parse(JObject                               JSON,
                                    CustomJObjectParserDelegate<Address>  CustomAddressParser   = null)
        {

            if (TryParse(JSON,
                         out Address address,
                         out String  ErrorResponse,
                         CustomAddressParser))
            {
                return address;
            }

            throw new ArgumentException("The given JSON representation of an address is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAddressParser = null)

        /// <summary>
        /// Parse the given text representation of an address.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom address JSON objects.</param>
        public static Address Parse(String                                Text,
                                    CustomJObjectParserDelegate<Address>  CustomAddressParser   = null)
        {

            if (TryParse(Text,
                         out Address address,
                         out String  ErrorResponse,
                         CustomAddressParser))
            {
                return address;
            }

            throw new ArgumentException("The given text representation of an address is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParseJSON(JSON, ..., out Address, out ErrorResponse, CustomAddressParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an address.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Address">The parsed address.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject      JSON,
                                       out Address  Address,
                                       out String   ErrorResponse)

            => TryParse(JSON,
                        out Address,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an address.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Address">The parsed address.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom addresss JSON objects.</param>
        public static Boolean TryParse(JObject                               JSON,
                                       out Address                           Address,
                                       out String                            ErrorResponse,
                                       CustomJObjectParserDelegate<Address>  CustomAddressParser)
        {

            try
            {

                Address = default;

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


                Address = new Address(Country,
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

                if (CustomAddressParser != null)
                    Address = CustomAddressParser(JSON,
                                                  Address);

                return true;

            }
            catch (Exception e)
            {
                Address        = default;
                ErrorResponse  = "The given JSON representation of an address is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out Address, out ErrorResponse, CustomAddressParser = null)

        /// <summary>
        /// Try to parse the given text representation of a address.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="Address">The parsed address.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom addresss JSON objects.</param>
        public static Boolean TryParse(String                                Text,
                                       out Address                           Address,
                                       out String                            ErrorResponse,
                                       CustomJObjectParserDelegate<Address>  CustomAddressParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out Address,
                                out ErrorResponse,
                                CustomAddressParser);

            }
            catch (Exception e)
            {
                Address        = default;
                ErrorResponse  = "The given text representation of an address is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAddressSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<Address> CustomAddressSerializer = null)
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

            return CustomAddressSerializer != null
                       ? CustomAddressSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public Address Clone

            => new Address(Country.Clone,
                           City        != null ? new String(City.       ToCharArray())                               : null,
                           Street      != null ? new String(Street.     ToCharArray())                               : null,
                           PostalCode  != null ? new String(PostalCode. ToCharArray())                               : null,
                           HouseNumber != null ? new String(HouseNumber.ToCharArray())                               : null,
                           Floor       != null ? new String(Floor.      ToCharArray())                               : null,
                           Region      != null ? new String(Region.     ToCharArray())                               : null,
                           ParkingFacility,
                           ParkingSpot != null ? new String(ParkingSpot.ToCharArray())                               : null,
                           TimeZone?.Clone,
                           CustomData  != null ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None)) : null);

        #endregion


        #region Operator overloading

        #region Operator == (Address1, Address2)

        /// <summary>
        /// Compares two addresses for equality.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Address Address1, Address Address2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Address1, Address2))
                return true;

            if (Address1 is null || Address2 is null)
                return false;

            return Address1.Equals(Address2);

        }

        #endregion

        #region Operator != (Address1, Address2)

        /// <summary>
        /// Compares two addresses for inequality.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Address Address1, Address Address2)
            => !(Address1 == Address2);

        #endregion

        #region Operator <  (Address1, Address2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Address Address1, Address Address2)
        {

            if (Address1 is null)
                throw new ArgumentNullException(nameof(Address1), "The given address must not be null!");

            return Address1.CompareTo(Address2) < 0;

        }

        #endregion

        #region Operator <= (Address1, Address2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Address Address1, Address Address2)
            => !(Address1 > Address2);

        #endregion

        #region Operator >  (Address1, Address2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Address Address1, Address Address2)
        {

            if (Address1 is null)
                throw new ArgumentNullException(nameof(Address1), "The given address must not be null!");

            return Address1.CompareTo(Address2) > 0;

        }

        #endregion

        #region Operator >= (Address1, Address2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Address Address1, Address Address2)
            => !(Address1 < Address2);

        #endregion

        #endregion

        #region IComparable<Address> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is Address address
                   ? CompareTo(address)
                   : throw new ArgumentException("The given object is not an address!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Address)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Address">An object to compare with.</param>
        public Int32 CompareTo(Address Address)
        {

            if (Address is null)
                throw new ArgumentNullException(nameof(Address), "The given address must not be null!");

            var c = Country.     CompareTo(Address.Country);
            if (c != 0)
                return c;

            c = City.CompareTo(Address.City);
            if (c != 0)
                return c;

            c = Street.CompareTo(Address.Street);
            if (c != 0)
                return c;

            c = PostalCode.CompareTo(Address.PostalCode);
            if (c != 0)
                return c;

            c = HouseNumber.CompareTo(Address.HouseNumber);
            if (c != 0)
                return c;

            c = (Floor ?? "").CompareTo(Address.Floor ?? "");
            if (c != 0)
                return c;

            c = (Floor ?? "").CompareTo(Address.Floor ?? "");
            if (c != 0)
                return c;

            c = (Region ?? "").CompareTo(Address.Region ?? "");
            if (c != 0)
                return c;

            //c = (ParkingFacility ?? "").CompareTo(Address.ParkingFacility ?? "");
            //if (c != 0)
            //    return c;

            return (TimeZone?.ToString() ?? "").CompareTo(Address.TimeZone?.ToString() ?? "");

        }

        #endregion

        #endregion

        #region IEquatable<Address> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is Address address &&
                   Equals(address);

        #endregion

        #region Equals(Address)

        /// <summary>
        /// Compares two addresses for equality.
        /// </summary>
        /// <param name="Address">An address to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Address Address)

            => !(Address is null) &&
                 Country         == Address.Country         &&
                 City            == Address.City            &&
                 Street          == Address.Street          &&
                 PostalCode      == Address.PostalCode      &&
                 HouseNumber     == Address.HouseNumber     &&
                 Floor           == Address.Floor           &&
                 Region          == Address.Region          &&
                 ParkingFacility == Address.ParkingFacility &&
                 ParkingSpot     == Address.ParkingSpot     &&
                 TimeZone        == Address.TimeZone;

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
