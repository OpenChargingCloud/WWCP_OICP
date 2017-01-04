/*
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
        /// The name of the street.
        /// </summary>
        public String      Street           { get; }

        /// <summary>
        /// The house number.
        /// </summary>
        public String      HouseNumber      { get; }

        /// <summary>
        /// The floor level.
        /// </summary>
        public String      FloorLevel       { get; }

        /// <summary>
        /// The postal code.
        /// </summary>
        public String      PostalCode       { get; }

        /// <summary>
        /// The postal code sub.
        /// </summary>
        public String      PostalCodeSub    { get; }

        /// <summary>
        /// The city.
        /// </summary>
        public I18NString  City             { get; }

        /// <summary>
        /// The city.
        /// </summary>
        public Country     Country          { get; }

        /// <summary>
        /// An optional text/comment to describe the address.
        /// </summary>
        public I18NString  Comment          { get; }

        #endregion

        #region Constructor(s)

        #region Address()

        /// <summary>
        /// Create a new address.
        /// </summary>
        public Address()
        {

            this.FloorLevel     = "";
            this.HouseNumber    = "";
            this.Street         = "";
            this.PostalCode     = "";
            this.PostalCodeSub  = "";
            this.City           = new I18NString();
            this.Country        = Country.unknown;
            this.Comment        = new I18NString();

        }

        #endregion

        #region Address(Country, PostalCode, City, Street, HouseNumber)

        /// <summary>
        /// Create a new minimal address.
        /// </summary>
        /// <param name="Country">The country.</param>
        /// <param name="PostalCode">The postal code</param>
        /// <param name="City">The city.</param>
        /// <param name="Street">The name of the street.</param>
        /// <param name="HouseNumber">The house number.</param>
        public Address(Country     Country,
                       String      PostalCode,
                       I18NString  City,
                       String      Street,
                       String      HouseNumber)
        {

            this.FloorLevel     = "";
            this.HouseNumber    = HouseNumber;
            this.Street         = Street;
            this.PostalCode     = PostalCode;
            this.PostalCodeSub  = "";
            this.City           = City;
            this.Country        = Country;
            this.Comment        = new I18NString();

        }

        #endregion

        #region Address(Street, HouseNumber, FloorLevel, PostalCode, PostalCodeSub, City, Country, FreeText = null)

        /// <summary>
        /// Create a new address.
        /// </summary>
        /// <param name="Street">The name of the street.</param>
        /// <param name="HouseNumber">The house number.</param>
        /// <param name="FloorLevel">The floor level.</param>
        /// <param name="PostalCode">The postal code</param>
        /// <param name="PostalCodeSub">The postal code sub</param>
        /// <param name="City">The city.</param>
        /// <param name="Country">The country.</param>
        /// <param name="Comment">An optional text/comment to describe the address.</param>
        public Address(String      Street,
                       String      HouseNumber,
                       String      FloorLevel,
                       String      PostalCode,
                       String      PostalCodeSub,
                       I18NString  City,
                       Country     Country,
                       I18NString  Comment = null)

        {

            this.Street         = Street;
            this.HouseNumber    = HouseNumber;
            this.FloorLevel     = FloorLevel;
            this.PostalCode     = PostalCode;
            this.PostalCodeSub  = PostalCodeSub;
            this.City           = City;
            this.Country        = Country;
            this.Comment        = Comment != null ? Comment : new I18NString();

        }

        #endregion

        #endregion


        #region (static) Create(Country, PostalCode, City, Street, HouseNumber)

        /// <summary>
        /// Create a new minimal address.
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
                                     String      HouseNumber)

            => new Address(Country,
                           PostalCode,
                           City,
                           Street,
                           HouseNumber);

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

            // Check if the given object is an Address.
            var Address = Object as Address;
            if ((Object) Address == null)
                return false;

            return this.Equals(Address);

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

            return Street.        Equals(Address.Street) &&
                   HouseNumber.   Equals(Address.HouseNumber) &&
                   FloorLevel.    Equals(Address.FloorLevel) &&
                   PostalCode.    Equals(Address.PostalCode) &&
                   PostalCodeSub. Equals(Address.PostalCodeSub) &&
                   City.          Equals(Address.City) &&
                   Country.       Equals(Address.Country);

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

                return Street.        GetHashCode() * 41 ^
                       HouseNumber.   GetHashCode() * 37 ^
                       FloorLevel.    GetHashCode() * 31 ^
                       PostalCode.    GetHashCode() * 23 ^
                       PostalCodeSub. GetHashCode() * 17 ^
                       City.          GetHashCode() * 11 ^
                       Country.       GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => Street                        + " " +
               HouseNumber                   + " " +
               FloorLevel                    + ", " +
               PostalCode                    + " " +
               PostalCodeSub                 + " " +
               City                          + ", " +
               Country.CountryName.FirstText + " / " +
               Comment;

        #endregion

    }

}
