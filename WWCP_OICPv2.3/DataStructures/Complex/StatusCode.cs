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
    /// A result status code.
    /// </summary>
    public class StatusCode
    {

        #region Properties

        /// <summary>
        /// The result code of the operation.
        /// </summary>
        [Mandatory]
        public StatusCodes  Code              { get; }

        /// <summary>
        /// Whether the operation was successful and returned a valid result.
        /// </summary>
        public Boolean      HasResult
            => Code == 0;

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        [Optional]
        public String?      Description       { get; }

        /// <summary>
        /// Optional additional information.
        /// </summary>
        [Optional]
        public String?      AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new result status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">Optional additional information.</param>
        public StatusCode(StatusCodes  Code,
                          String?      Description      = null,
                          String?      AdditionalInfo   = null)
        {

            this.Code            = Code;
            this.Description     = Description;
            this.AdditionalInfo  = AdditionalInfo;


            unchecked
            {

                hashCode = this.Code.           GetHashCode()       * 5 ^
                          (this.Description?.   GetHashCode() ?? 0) * 3 ^
                          (this.AdditionalInfo?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/03_CPO_Data_Types.asciidoc#StatusCodeType

        // {
        //   "AdditionalInfo":  "string",
        //   "Code":            "000",
        //   "Description":     "string"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomStatusCodeParser = null)

        /// <summary>
        /// Parse the given JSON representation of a status code.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom status codes JSON objects.</param>
        public static StatusCode Parse(JObject                                   JSON,
                                       CustomJObjectParserDelegate<StatusCode>?  CustomStatusCodeParser   = null)
        {

            if (TryParse(JSON,
                         out var statusCode,
                         out var errorResponse,
                         CustomStatusCodeParser))
            {
                return statusCode;
            }

            throw new ArgumentException("The given JSON representation of a status code is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out StatusCode, out ErrorResponse, CustomStatusCodeParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a status code.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="StatusCode">The parsed status code.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                               JSON,
                                       [NotNullWhen(true)]  out StatusCode?  StatusCode,
                                       [NotNullWhen(false)] out String?      ErrorResponse)

            => TryParse(JSON,
                        out StatusCode,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a status code.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="StatusCode">The parsed status code.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom status codes JSON objects.</param>
        public static Boolean TryParse(JObject                                   JSON,
                                       [NotNullWhen(true)]  out StatusCode?      StatusCode,
                                       [NotNullWhen(false)] out String?          ErrorResponse,
                                       CustomJObjectParserDelegate<StatusCode>?  CustomStatusCodeParser)
        {

            try
            {

                StatusCode = default;

                #region Parse StatusCode        [mandatory]

                if (!JSON.ParseMandatory("Code",
                                         "status code",
                                         StatusCodesExtensions.TryParse,
                                         out StatusCodes StatusCodeValue,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Description       [optional]

                var Description     = JSON.GetString("Description");

                #endregion

                #region Parse AdditionalInfo    [optional]

                var AdditionalInfo  = JSON.GetString("AdditionalInfo");

                #endregion


                StatusCode  = new StatusCode(
                                  StatusCodeValue,
                                  Description,
                                  AdditionalInfo
                              );

                if (CustomStatusCodeParser is not null)
                    StatusCode = CustomStatusCodeParser(JSON,
                                                        StatusCode);

                return true;

            }
            catch (Exception e)
            {
                StatusCode     = default;
                ErrorResponse  = "The given JSON representation of a status code is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<StatusCode>?  CustomStatusCodeSerializer   = null)

        {

            var json = JSONObject.Create(

                                 new JProperty("Code",             ((Int32) Code).ToString("D3")),

                           Description.   IsNotNullOrEmpty()
                               ? new JProperty("Description",      Description)
                               : null,

                           AdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("AdditionalInfo",   AdditionalInfo)
                               : null

                       );

            return CustomStatusCodeSerializer is not null
                       ? CustomStatusCodeSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public StatusCode Clone

            => new (Code,
                    Description    != null ? new String(Description.   ToCharArray()) : null,
                    AdditionalInfo != null ? new String(AdditionalInfo.ToCharArray()) : null);

        #endregion


        #region Operator overloading

        #region Operator == (StatusCode1, StatusCode2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="StatusCode1">A status code.</param>
        /// <param name="StatusCode2">Another status code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (StatusCode? StatusCode1,
                                           StatusCode? StatusCode2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(StatusCode1, StatusCode2))
                return true;

            // If one is null, but not both, return false.
            if (StatusCode1 is null || StatusCode2 is null)
                return false;

            return StatusCode1.Equals(StatusCode2);

        }

        #endregion

        #region Operator != (StatusCode1, StatusCode2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="StatusCode1">A status code.</param>
        /// <param name="StatusCode2">Another status code.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (StatusCode? StatusCode1,
                                           StatusCode? StatusCode2)

            => !(StatusCode1 == StatusCode2);

        #endregion

        #endregion

        #region IEquatable<StatusCode> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two status codes for equality.
        /// </summary>
        /// <param name="Object">A status code to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is StatusCode statusCode &&
                   Equals(statusCode);

        #endregion

        #region Equals(StatusCode)

        /// <summary>
        /// Compares two status codes for equality.
        /// </summary>
        /// <param name="StatusCode">A status code to compare with.</param>
        public Boolean Equals(StatusCode StatusCode)

            => StatusCode is not null &&

               Code.Equals(StatusCode.Code) &&

             ((Description    is     null && StatusCode.Description    is     null) ||
              (Description    is not null && StatusCode.Description    is not null && Description.   Equals(StatusCode.Description))) &&

             ((AdditionalInfo is     null && StatusCode.AdditionalInfo is     null) ||
              (AdditionalInfo is not null && StatusCode.AdditionalInfo is not null && AdditionalInfo.Equals(StatusCode.AdditionalInfo)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Code.ToString(), " (", ((Int32) Code).ToString("D3"), ")",

                             Description.IsNotNullOrEmpty()
                                 ? ", "             + Description
                                 : "",

                             AdditionalInfo.IsNotNullOrEmpty()
                                 ? ", additional: " + AdditionalInfo
                                 : "");

        #endregion


        #region ToBuilder()

        /// <summary>
        /// Return a status code builder.
        /// </summary>
        public Builder ToBuilder()

            => new (Code,
                    Description,
                    AdditionalInfo);

        #endregion

        #region (class) Builder

        /// <summary>
        /// A status code builder.
        /// </summary>
        public class Builder
        {

            #region Properties

            /// <summary>
            /// The result code of the operation.
            /// </summary>
            [Mandatory]
            public StatusCodes?  Code              { get; set; }

            /// <summary>
            /// Whether the operation was successful and returned a valid result.
            /// </summary>
            public Boolean       HasResult
                => Code == 0;

            /// <summary>
            /// An optional description of the result code.
            /// </summary>
            [Optional]
            public String?       Description       { get; set; }

            /// <summary>
            /// Optional additional information.
            /// </summary>
            [Optional]
            public String?       AdditionalInfo    { get; set; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

            /// <summary>
            /// Create a new status code builder.
            /// </summary>
            /// <param name="Code">The result code of the operation.</param>
            /// <param name="Description">An optional description of the result code.</param>
            /// <param name="AdditionalInfo">Optional additional information.</param>
            public Builder(StatusCodes?  Code             = null,
                           String?       Description      = null,
                           String?       AdditionalInfo   = null)
            {

                this.Code            = Code;
                this.Description     = Description;
                this.AdditionalInfo  = AdditionalInfo;

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the status code.
            /// </summary>
            /// <param name="Builder">A status code builder.</param>
            public static implicit operator StatusCode(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the status code.
            /// </summary>
            public StatusCode ToImmutable()
            {

                #region Check mandatory parameters

                if (!Code.HasValue)
                    throw new ArgumentException("The given result code must not be null!", nameof(Code));

                #endregion

                return new (Code.Value,
                            Description,
                            AdditionalInfo);

            }

            #endregion

        }

        #endregion

    }

}