/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A signed metering value.
    /// </summary>
    public class SignedMeteringValue : IEquatable<SignedMeteringValue>,
                                       IComparable<SignedMeteringValue>,
                                       IComparable
    {

        #region Properties

        /// <summary>
        /// The optional signed metering value for a transparency software.
        /// </summary>
        [Optional]
        public String?              Value             { get; }

        /// <summary>
        /// The optional status of the given signed metering value.
        /// </summary>
        [Optional]
        public MeteringStatusType?  MeteringStatus    { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?             CustomData        { get; }


        /// <summary>
        /// Whether all sub-datastructures are empty/null.
        /// </summary>
        public Boolean IsEmpty

            => Value is null &&
               MeteringStatus.HasValue;

        /// <summary>
        /// Whether NOT all sub-datastructures are empty/null.
        /// </summary>
        public Boolean IsNotEmpty
            => !IsEmpty;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new signed metering value.
        /// </summary>
        /// <param name="Value">An optional signed metering value for a transparency software.</param>
        /// <param name="MeteringStatus">An optional status of the given signed metering value.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public SignedMeteringValue(String?              Value,
                                   MeteringStatusType?  MeteringStatus   = null,
                                   JObject?             CustomData       = null)

        {

            this.Value           = Value?.TrimToNull();
            this.MeteringStatus  = MeteringStatus;
            this.CustomData      = CustomData;

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#SignedMeteringValueType

        // {
        //   "SignedMeteringValue":  "string",
        //   "MeteringStatus":       "Start"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomSignedMeteringValueParser = null)

        /// <summary>
        /// Parse the given JSON representation of a signed metering value.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomSignedMeteringValueParser">A delegate to parse custom signed metering values JSON objects.</param>
        public static SignedMeteringValue Parse(JObject                                            JSON,
                                                CustomJObjectParserDelegate<SignedMeteringValue>?  CustomSignedMeteringValueParser   = null)
        {

            if (TryParse(JSON,
                         out var signedMeteringValue,
                         out var errorResponse,
                         CustomSignedMeteringValueParser))
            {
                return signedMeteringValue;
            }

            throw new ArgumentException("The given JSON representation of a signed metering value is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out SignedMeteringValue, out ErrorResponse, CustomSignedMeteringValueParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a signed metering value.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="SignedMeteringValue">The parsed signed metering value.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out SignedMeteringValue?  SignedMeteringValue,
                                       [NotNullWhen(false)] out String?               ErrorResponse)

            => TryParse(JSON,
                        out SignedMeteringValue,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a signed metering value.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="SignedMeteringValue">The parsed signed metering value.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomSignedMeteringValueParser">A delegate to parse custom signed metering values JSON objects.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       [NotNullWhen(true)]  out SignedMeteringValue?      SignedMeteringValue,
                                       [NotNullWhen(false)] out String?                   ErrorResponse,
                                       CustomJObjectParserDelegate<SignedMeteringValue>?  CustomSignedMeteringValueParser)
        {

            try
            {

                SignedMeteringValue = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Value             [optional]

                var Value = JSON["SignedMeteringValue"]?.Value<String>();

                #endregion

                #region Parse MeteringStatus    [optional]

                if (JSON.ParseOptional("MeteringStatus",
                                       "metering status",
                                       MeteringStatusType.TryParse,
                                       out MeteringStatusType? MeteringStatus,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData        [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                SignedMeteringValue = new SignedMeteringValue(
                                          Value,
                                          MeteringStatus,
                                          customData
                                      );

                if (CustomSignedMeteringValueParser is not null)
                    SignedMeteringValue = CustomSignedMeteringValueParser(JSON,
                                                                          SignedMeteringValue);

                return true;

            }
            catch (Exception e)
            {
                SignedMeteringValue  = default;
                ErrorResponse        = "The given JSON representation of a signed metering value is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSignedMeteringValueSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomSignedMeteringValueSerializer">A delegate to serialize custom time period JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<SignedMeteringValue>?  CustomSignedMeteringValueSerializer   = null)
        {

            var json = JSONObject.Create(

                           Value is not null
                               ? new JProperty("SignedMeteringValue",   Value)
                               : null,

                           MeteringStatus.HasValue
                               ? new JProperty("MeteringStatus",        MeteringStatus.ToString())
                               : null,

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",            CustomData)
                               : null

                       );

            return CustomSignedMeteringValueSerializer is not null
                       ? CustomSignedMeteringValueSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this signed metering value.
        /// </summary>
        public SignedMeteringValue Clone

            => new (

                   Value is not null
                       ? new String(Value.ToCharArray())
                       : null,

                   MeteringStatus,

                   CustomData is not null
                       ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (SignedMeteringValue1, SignedMeteringValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue1">A signed metering value.</param>
        /// <param name="SignedMeteringValue2">Another signed metering value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SignedMeteringValue? SignedMeteringValue1,
                                           SignedMeteringValue? SignedMeteringValue2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SignedMeteringValue1, SignedMeteringValue2))
                return true;

            // If one is null, but not both, return false.
            if (SignedMeteringValue1 is null || SignedMeteringValue2 is null)
                return false;

            return SignedMeteringValue1.Equals(SignedMeteringValue2);

        }

        #endregion

        #region Operator != (SignedMeteringValue1, SignedMeteringValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue1">A signed metering value.</param>
        /// <param name="SignedMeteringValue2">Another signed metering value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SignedMeteringValue? SignedMeteringValue1,
                                           SignedMeteringValue? SignedMeteringValue2)

            => !(SignedMeteringValue1 == SignedMeteringValue2);

        #endregion

        #region Operator <  (SignedMeteringValue1, SignedMeteringValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue1">A signed metering value.</param>
        /// <param name="SignedMeteringValue2">Another signed metering value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SignedMeteringValue? SignedMeteringValue1,
                                          SignedMeteringValue? SignedMeteringValue2)
        {

            if (SignedMeteringValue1 is null)
                throw new ArgumentNullException(nameof(SignedMeteringValue1), "The given signed metering value must not be null!");

            return SignedMeteringValue1.CompareTo(SignedMeteringValue2) < 0;

        }

        #endregion

        #region Operator <= (SignedMeteringValue1, SignedMeteringValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue1">A signed metering value.</param>
        /// <param name="SignedMeteringValue2">Another signed metering value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SignedMeteringValue? SignedMeteringValue1,
                                           SignedMeteringValue? SignedMeteringValue2)

            => !(SignedMeteringValue1 > SignedMeteringValue2);

        #endregion

        #region Operator >  (SignedMeteringValue1, SignedMeteringValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue1">A signed metering value.</param>
        /// <param name="SignedMeteringValue2">Another signed metering value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SignedMeteringValue? SignedMeteringValue1,
                                          SignedMeteringValue? SignedMeteringValue2)
        {

            if (SignedMeteringValue1 is null)
                throw new ArgumentNullException(nameof(SignedMeteringValue1), "The given signed metering value must not be null!");

            return SignedMeteringValue1.CompareTo(SignedMeteringValue2) > 0;

        }

        #endregion

        #region Operator >= (SignedMeteringValue1, SignedMeteringValue2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue1">A signed metering value.</param>
        /// <param name="SignedMeteringValue2">Another signed metering value.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SignedMeteringValue? SignedMeteringValue1,
                                           SignedMeteringValue? SignedMeteringValue2)

            => !(SignedMeteringValue1 < SignedMeteringValue2);

        #endregion

        #endregion

        #region IComparable<SignedMeteringValue> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SignedMeteringValue signedMeteringValue
                   ? CompareTo(signedMeteringValue)
                   : throw new ArgumentException("The given object is not a signed metering value!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SignedMeteringValue)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignedMeteringValue">An object to compare with.</param>
        public Int32 CompareTo(SignedMeteringValue? SignedMeteringValue)
        {

            if (SignedMeteringValue is null)
                throw new ArgumentNullException(nameof(SignedMeteringValue), "The given SignedMeteringValue must not be null!");

            var c = 0;

            if (         Value           is not null && SignedMeteringValue.Value is not null)
                c = Value.               CompareTo(SignedMeteringValue.Value);

            if (c == 0 && MeteringStatus.HasValue    && SignedMeteringValue.MeteringStatus.HasValue)
                c = MeteringStatus.Value.CompareTo(SignedMeteringValue.MeteringStatus.Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<SignedMeteringValue> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two signed metering values for equality.
        /// </summary>
        /// <param name="Object">A signed metering value to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SignedMeteringValue signedMeteringValue &&
                   Equals(signedMeteringValue);

        #endregion

        #region Equals(SignedMeteringValue)

        /// <summary>
        /// Compares two signed metering values for equality.
        /// </summary>
        /// <param name="SignedMeteringValue">A signed metering value to compare with.</param>
        public Boolean Equals(SignedMeteringValue? SignedMeteringValue)

            => SignedMeteringValue is not null &&

               Value?.         Equals(SignedMeteringValue.Value)          == true &&
               MeteringStatus?.Equals(SignedMeteringValue.MeteringStatus) == true;

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (Value?.         GetHashCode() ?? 0) * 3 ^
                       (MeteringStatus?.GetHashCode() ?? 0);
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Value?.SubstringMax(20) ?? ""}, {MeteringStatus}";

        #endregion

    }

}
