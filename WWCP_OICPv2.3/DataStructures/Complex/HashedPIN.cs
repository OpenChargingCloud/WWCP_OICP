/*
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
        public Hash_Value     Value       { get; }

        /// <summary>
        /// Function that was used to generate the hash value.
        /// </summary>
        [Mandatory]
        public HashFunctions  Function    { get; }

        #endregion

        #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

        /// <summary>
        /// Create a new hashed PIN.
        /// </summary>
        /// <param name="Value">Hash value created by partner.</param>
        /// <param name="Function">Function that was used to generate the hash value.</param>
        public HashedPIN(Hash_Value     Value,
                         HashFunctions  Function)
        {

            this.Value     = Value;
            this.Function  = Function;

        }

#pragma warning restore IDE0290 // Use primary constructor

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#HashedPINType

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
        public static HashedPIN Parse(JObject                                  JSON,
                                      CustomJObjectParserDelegate<HashedPIN>?  CustomHashedPINParser   = null)
        {

            if (TryParse(JSON,
                         out var hashedPIN,
                         out var errorResponse,
                         CustomHashedPINParser))
            {
                return hashedPIN;
            }

            throw new ArgumentException("The given JSON representation of a hashed PIN is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomHashedPINParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a hashed PIN.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomHashedPINParser">A delegate to parse custom hashed PIN JSON objects.</param>
        public static HashedPIN? TryParse(JObject                                  JSON,
                                          CustomJObjectParserDelegate<HashedPIN>?  CustomHashedPINParser   = null)
        {

            if (TryParse(JSON,
                         out var hashedPIN,
                         out _,
                         CustomHashedPINParser))
            {
                return hashedPIN;
            }

            return null;

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
        public static Boolean TryParse(JObject                             JSON,
                                       [NotNullWhen(true)]  out HashedPIN  HashedPIN,
                                       [NotNullWhen(false)] out String?    ErrorResponse)

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
        public static Boolean TryParse(JObject                                  JSON,
                                       [NotNullWhen(true)]  out HashedPIN       HashedPIN,
                                       [NotNullWhen(false)] out String?         ErrorResponse,
                                       CustomJObjectParserDelegate<HashedPIN>?  CustomHashedPINParser)
        {

            try
            {

                HashedPIN = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Value       [mandatory]

                if (!JSON.ParseMandatory("Value",
                                         "hash value",
                                         Hash_Value.TryParse,
                                         out Hash_Value Value,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Function    [mandatory]

                if (!JSON.ParseMandatory("Function",
                                         "hash function",
                                         HashFunctionExtensions.TryParse,
                                         out HashFunctions Function,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                HashedPIN = new HashedPIN(
                                Value,
                                Function
                            );


                if (CustomHashedPINParser is not null)
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

        #region ToJSON(CustomHashedPINSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomHashedPINSerializer">A delegate to serialize custom hashed PIN JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<HashedPIN>?  CustomHashedPINSerializer   = null)
        {

            var json = JSONObject.Create(
                           new JProperty("Function",  Function.ToString()),
                           new JProperty("Value",     Value.   ToString())
                       );

            return CustomHashedPINSerializer is not null
                       ? CustomHashedPINSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this hashed PIN.
        /// </summary>
        public HashedPIN Clone()

            => new (
                   Value.Clone(),
                   Function
               );

        #endregion


        #region Operator overloading

        #region Operator == (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => !HashedPIN1.Equals(HashedPIN2);

        #endregion

        #region Operator <  (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => HashedPIN1.CompareTo(HashedPIN2) <= 0;

        #endregion

        #region Operator >  (HashedPIN1, HashedPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HashedPIN1">A hashed PIN.</param>
        /// <param name="HashedPIN2">Another hashed PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (HashedPIN HashedPIN1,
                                           HashedPIN HashedPIN2)

            => HashedPIN1.CompareTo(HashedPIN2) >= 0;

        #endregion

        #endregion

        #region IComparable<HashedPIN> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two hashed PINs.
        /// </summary>
        /// <param name="Object">A hashed PIN to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is HashedPIN hashedPIN
                   ? CompareTo(hashedPIN)
                   : throw new ArgumentException("The given object is not a hashed PIN!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(HashedPIN)

        /// <summary>
        /// Compares two hashed PINs.
        /// </summary>
        /// <param name="HashedPIN">A hashed PIN to compare with.</param>
        public Int32 CompareTo(HashedPIN HashedPIN)
        {

            var c = Value.CompareTo(HashedPIN.Value);

            if (c == 0)
                c = Function.CompareTo(HashedPIN.Function);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<HashedPIN> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two hashed PINs for equality.
        /// </summary>
        /// <param name="Object">A hashed PIN to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is HashedPIN hashedPIN &&
                   Equals(hashedPIN);

        #endregion

        #region Equals(HashedPIN)

        /// <summary>
        /// Compares two hashed PINs for equality.
        /// </summary>
        /// <param name="HashedPIN">A hashed PIN to compare with.</param>
        public Boolean Equals(HashedPIN HashedPIN)

            => Value.   Equals(HashedPIN.Value) &&
               Function.Equals(HashedPIN.Function);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hash code of this object.
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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Value.ToString()[15..]} ({Function})";

        #endregion

    }

}
