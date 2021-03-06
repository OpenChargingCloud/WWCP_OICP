﻿/*
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
    /// A hashed PIN.
    /// </summary>
    public readonly struct HashedPIN : IEquatable<HashedPIN>,
                                       IComparable<HashedPIN>,
                                       IComparable
    {

        #region Properties

        /// <summary>
        /// Hash value created by partner.
        /// </summary>
        [Mandatory]
        public Hash_Value    Value       { get; }

        /// <summary>
        /// Function that was used to generate the hash value.
        /// </summary>
        [Mandatory]
        public HashFunctions  Function    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new hashed PIN.
        /// </summary>
        /// <param name="Value">Hash value created by partner.</param>
        /// <param name="Function">Function that was used to generate the hash value.</param>
        public HashedPIN(Hash_Value    Value,
                         HashFunctions  Function)
        {

            this.Value     = Value;
            this.Function  = Function;

        }

        #endregion


        #region Documentation

        // https://github.com/ahzf/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#HashedPINType

        // {
        //   "Function":  "Bcrypt",
        //   "LegacyHashData": {
        //     "Function":  "MD5",
        //     "Salt":      "string",
        //     "Value":     "string"
        //   },
        //   "Value":     "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomHashedPINParser = null)

        /// <summary>
        /// Parse the given JSON representation of a hashed PIN.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomHashedPINParser">A delegate to parse custom hashed PIN JSON objects.</param>
        public static HashedPIN Parse(JObject                                 JSON,
                                      CustomJObjectParserDelegate<HashedPIN>  CustomHashedPINParser   = null)
        {

            if (TryParse(JSON,
                         out HashedPIN hashedPIN,
                         out String    ErrorResponse,
                         CustomHashedPINParser))
            {
                return hashedPIN;
            }

            throw new ArgumentException("The given JSON representation of a hashed PIN is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomHashedPINParser = null)

        /// <summary>
        /// Parse the given text representation of a hashed PIN.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomHashedPINParser">A delegate to parse custom hashed PIN JSON objects.</param>
        public static HashedPIN Parse(String                                  Text,
                                      CustomJObjectParserDelegate<HashedPIN>  CustomHashedPINParser   = null)
        {

            if (TryParse(Text,
                         out HashedPIN hashedPIN,
                         out String    ErrorResponse,
                         CustomHashedPINParser))
            {
                return hashedPIN;
            }

            throw new ArgumentException("The given text representation of a hashed PIN is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out HashedPIN, out ErrorResponse, CustomHashedPINParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a hashed PIN.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="HashedPIN">The parsed hashed PIN.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject        JSON,
                                       out HashedPIN  HashedPIN,
                                       out String     ErrorResponse)

            => TryParse(JSON,
                        out HashedPIN,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a hashed PIN.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="HashedPIN">The parsed hashed PIN.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomHashedPINParser">A delegate to parse custom hashed PIN JSON objects.</param>
        public static Boolean TryParse(JObject                                 JSON,
                                       out HashedPIN                           HashedPIN,
                                       out String                              ErrorResponse,
                                       CustomJObjectParserDelegate<HashedPIN>  CustomHashedPINParser)
        {

            try
            {

                HashedPIN = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Value         [mandatory]

                if (!JSON.ParseMandatory("Value",
                                         "hash value",
                                         Hash_Value.TryParse,
                                         out Hash_Value Value,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Function      [mandatory]

                if (!JSON.ParseMandatoryEnum("Function",
                                             "hash function",
                                             out HashFunctions Function,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion


                HashedPIN = new HashedPIN(Value,
                                          Function);


                if (CustomHashedPINParser != null)
                    HashedPIN = CustomHashedPINParser(JSON,
                                                      HashedPIN);

                return true;

            }
            catch (Exception e)
            {
                HashedPIN      = default;
                ErrorResponse  = "The given JSON representation of a hashed PIN is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out HashedPIN, out ErrorResponse, CustomHashedPINParser = null)

        /// <summary>
        /// Try to parse the given text representation of a hashed PIN.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="HashedPIN">The parsed hashed PIN.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomHashedPINParser">A delegate to parse custom hashed PIN JSON objects.</param>
        public static Boolean TryParse(String                                  Text,
                                       out HashedPIN                           HashedPIN,
                                       out String                              ErrorResponse,
                                       CustomJObjectParserDelegate<HashedPIN>  CustomHashedPINParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out HashedPIN,
                                out ErrorResponse,
                                CustomHashedPINParser);

            }
            catch (Exception e)
            {
                HashedPIN      = default;
                ErrorResponse  = "The given text representation of a hashed PIN is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomHashedPINSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomHashedPINSerializer">A delegate to serialize custom hashed PIN JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<HashedPIN>  CustomHashedPINSerializer   = null)
        {

            var JSON = JSONObject.Create(
                           new JProperty("Function",  Function.ToString()),
                           new JProperty("Value",     Value.   ToString())
                       );

            return CustomHashedPINSerializer != null
                       ? CustomHashedPINSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public HashedPIN Clone

            => new HashedPIN(Value.Clone,
                             Function);

        #endregion


        #region Operator overloading

        #region Operator == (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => HashedPIN1.Equals(HashedPIN2);

        #endregion

        #region Operator != (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => !(HashedPIN1 == HashedPIN2);

        #endregion

        #region Operator <  (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (HashedPIN HashedPIN1,
                                          HashedPIN HashedPIN2)

            => HashedPIN1.CompareTo(HashedPIN2) < 0;

        #endregion

        #region Operator <= (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => !(HashedPIN1 > HashedPIN2);

        #endregion

        #region Operator >  (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (HashedPIN HashedPIN1,
                                          HashedPIN HashedPIN2)

            => HashedPIN1.CompareTo(HashedPIN2) > 0;

        #endregion

        #region Operator >= (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => !(HashedPIN1 < HashedPIN2);

        #endregion

        #endregion

        #region IComparable<HashedPIN> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is HashedPIN hashedPIN
                   ? CompareTo(hashedPIN)
                   : throw new ArgumentException("The given object is not a hashed PIN!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(HashedPIN)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN">An  with (hashed) pin object to compare with.</param>
        public Int32 CompareTo(HashedPIN HashedPIN)
        {

            var result = Value.CompareTo(HashedPIN.Value);

            if (result == 0)
                result = Function.CompareTo(HashedPIN.Function);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<HashedPIN> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is HashedPIN hashedPIN &&
                   Equals(hashedPIN);

        #endregion

        #region Equals(HashedPIN)

        /// <summary>
        /// Compares two s with (hashed) pins for equality.
        /// </summary>
        /// <param name="HashedPIN">An  with (hashed) pin to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(HashedPIN HashedPIN)

            => Value.   Equals(HashedPIN.Value) &&
               Function.Equals(HashedPIN.Function);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Value.   GetHashCode() * 3 ^
                       Function.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Value.ToString().Substring(15),
                             " (", Function.ToString(), ")");

        #endregion

    }

}
