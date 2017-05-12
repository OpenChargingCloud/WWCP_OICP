﻿/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;

using org.GraphDefined.Vanaheimr.Illias;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A WWCP address.
    /// </summary>
    public class Address : IEquatable<Address>
    {

        #region Properties

        /// <summary>
        /// The city.
        /// </summary>
        public Country     Country          { get; }

        /// <summary>
        /// The city.
        /// </summary>
        public I18NString  City             { get; }

        /// <summary>
        /// The name of the street.
        /// </summary>
        public String      Street           { get; }

        /// <summary>
        /// The optional postal code.
        /// </summary>
        public String      PostalCode       { get; }

        /// <summary>
        /// The optional house number.
        /// </summary>
        public String      HouseNumber      { get; }

        /// <summary>
        /// The optional floor level.
        /// </summary>
        public String      FloorLevel       { get; }

        /// <summary>
        /// The optional region.
        /// </summary>
        public String      Region           { get; }

        /// <summary>
        /// The optional timezone.
        /// </summary>
        public String      Timezone         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new address.
        /// </summary>
        /// <param name="Country">The country.</param>
        /// <param name="City">The city.</param>
        /// <param name="Street">The name of the street.</param>
        /// <param name="PostalCode">An optional postal code</param>
        /// <param name="HouseNumber">An optional house number.</param>
        /// <param name="FloorLevel">An optional floor level.</param>
        /// <param name="Region">An optional region.</param>
        /// <param name="Timezone">An optional timezone.</param>
        public Address(Country     Country,
                       I18NString  City,
                       String      Street,
                       String      PostalCode   = null,
                       String      HouseNumber  = null,
                       String      FloorLevel   = null,
                       String      Region       = null,
                       String      Timezone     = null)

        {

            this.Country        = Country;
            this.City           = City;
            this.Street         = Street;
            this.PostalCode     = PostalCode;
            this.HouseNumber    = HouseNumber;
            this.FloorLevel     = FloorLevel;
            this.Region         = Region;
            this.Timezone       = Timezone;

        }

        #endregion


        #region (static) Create(Country, City, Street, ...)

        /// <summary>
        /// Create a new address.
        /// </summary>
        /// <param name="Country">The country.</param>
        /// <param name="PostalCode">The postal code</param>
        /// <param name="City">The city.</param>
        /// <param name="Street">The name of the street.</param>
        /// <param name="HouseNumber">The house number.</param>
        public static Address Create(Country     Country,
                                     String      PostalCode,
                                     I18NString  City,
                                     String      Street,
                                     String      HouseNumber  = null,
                                     String      FloorLevel   = null,
                                     String      Region       = null,
                                     String      Timezone     = null)

            => new Address(Country,
                           City,
                           Street,
                           PostalCode,
                           HouseNumber,
                           FloorLevel,
                           Region,
                           Timezone);

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/EVSEData/v2.1"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/EVSEData/v2.0">
        //
        // [...]
        //
        //    <EVSEData:Address>
        //
        //       <CommonTypes:Country>?</CommonTypes:Country>
        //       <CommonTypes:City>?</CommonTypes:City>
        //       <CommonTypes:Street>?</CommonTypes:Street>
        //
        //       <!--Optional:-->
        //       <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
        //
        //       <!--Optional:-->
        //       <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
        //
        //       <!--Optional:-->
        //       <CommonTypes:Floor>?</CommonTypes:Floor>
        //
        //       <!--Optional:-->
        //       <CommonTypes:Region>?</CommonTypes:Region>
        //
        //       <!--Optional:-->
        //       <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
        //
        //    </EVSEData:Address>
        //
        // [...]
        //
        // </soapenv:Envelope>

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AddressXML"></param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="OnException"></param>
        /// <returns></returns>
        public static Address Parse(XElement                          AddressXML,
                                    CustomXMLParserDelegate<Address>  CustomAddressParser   = null,
                                    OnExceptionDelegate               OnException           = null)
        {

            var _CountryTXT = AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Country", "Missing 'Country'-XML tag!").Trim();

                Country _Country;
                if (!Country.TryParse(_CountryTXT, out _Country))
                {

                    if (_CountryTXT.ToUpper() == "UNKNOWN")
                        _Country = Country.unknown;

                    else
                        throw new Exception("'" + _CountryTXT + "' is an unknown country name!");

                }

            var _Address = new Address(_Country,

                                       I18NString.Create(Languages.unknown,
                                                         AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "City").Trim()),

                                       AddressXML.ElementValueOrFail   (OICPNS.CommonTypes + "Street").        Trim(),

                                       AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode", "").Trim(),
                                       AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum",   "").Trim(),
                                       AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor",      "").Trim(),
                                       AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Region",     "").Trim(),
                                       AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "TimeZone",   "").Trim()
                                       );

            if (CustomAddressParser != null)
                _Address = CustomAddressParser(AddressXML,
                                               _Address);

            return _Address;

        }

        #region ToXML(XName = null, CustomAddressSerializer = null)

        /// <summary>
        /// Return an XML representation of this EVSE data record.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom Address XML elements.</param>
        public XElement ToXML(XName                                 XName                     = null,
                              CustomXMLSerializerDelegate<Address>  CustomAddressSerializer   = null)
        {

            var XML = new XElement(XName ?? OICPNS.EVSEData + "Address",

                          new XElement(OICPNS.CommonTypes + "Country",        Country.Alpha3Code),
                          new XElement(OICPNS.CommonTypes + "City",           City.FirstText()),
                          new XElement(OICPNS.CommonTypes + "Street",         Street), // OICP v2.1 requires at least 5 characters!

                          PostalCode. IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "PostalCode", PostalCode)
                              : null,

                          HouseNumber.IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "HouseNum",   HouseNumber)
                              : null,

                          FloorLevel. IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "Floor",      FloorLevel)
                              : null,

                          Region. IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "Region",     Region)
                              : null,

                          Timezone. IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "TimeZone",   Timezone)
                              : null

                      );

            return CustomAddressSerializer != null
                       ? CustomAddressSerializer(this, XML)
                       : XML;

        }

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

            // If one is null, but not both, return false.
            if (((Object) Address1 == null) || ((Object) Address2 == null))
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

        #endregion

        #region IEquatable<Address> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            var Address = Object as Address;
            if ((Object) Address == null)
                return false;

            return Equals(Address);

        }

        #endregion

        #region Equals(Address)

        /// <summary>
        /// Compares two addresses for equality.
        /// </summary>
        /// <param name="Address">An address to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Address Address)
        {

            if ((Object) Address == null)
                return false;

            return Country.       Equals(Address.Country) &&
                   City.          Equals(Address.City)    &&
                   Street.        Equals(Address.Street)  &&

                   ((PostalCode  == null && Address.PostalCode  == null) ||
                    (PostalCode  != null && Address.PostalCode  != null && PostalCode. Equals(Address.PostalCode))) &&

                   ((HouseNumber == null && Address.HouseNumber == null) ||
                    (HouseNumber != null && Address.HouseNumber != null && HouseNumber.Equals(Address.HouseNumber))) &&

                   ((FloorLevel  == null && Address.FloorLevel  == null) ||
                    (FloorLevel  != null && Address.FloorLevel  != null && FloorLevel. Equals(Address.FloorLevel))) &&

                   ((Region      == null && Address.Region      == null) ||
                    (Region      != null && Address.Region      != null && Region.     Equals(Address.Region))) &&

                   ((Timezone    == null && Address.Timezone    == null) ||
                    (Timezone    != null && Address.Timezone    != null && Timezone.   Equals(Address.Timezone)));

        }

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

                return Country.       GetHashCode() * 19 ^
                       City.          GetHashCode() * 17 ^
                       Street.        GetHashCode() * 13 ^

                       (PostalCode.IsNotNullOrEmpty()
                            ? PostalCode. GetHashCode() * 11
                            : 0) ^

                       (HouseNumber.IsNotNullOrEmpty()
                            ? HouseNumber.GetHashCode() * 7
                            : 0) ^

                       (FloorLevel.IsNotNullOrEmpty()
                            ? FloorLevel. GetHashCode() * 5
                            : 0) ^

                       (Region.IsNotNullOrEmpty()
                            ? Region.     GetHashCode() * 3
                            : 0) ^

                       (Timezone.IsNotNullOrEmpty()
                            ? Timezone.   GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Country.CountryName.FirstText(), ", ",
                             City, ", ",
                             Street,

                             PostalCode.IsNotNullOrEmpty()
                                 ? ", " + PostalCode
                                 : "",

                             HouseNumber.IsNotNullOrEmpty()
                                 ? ", " + HouseNumber
                                 : "",

                             FloorLevel.IsNotNullOrEmpty()
                                 ? ", " + FloorLevel
                                 : "",

                             Region.IsNotNullOrEmpty()
                                 ? ", " + Region
                                 : "",

                             Timezone.IsNotNullOrEmpty()
                                 ? ", " + Timezone
                                 : "");

        #endregion


    }

}