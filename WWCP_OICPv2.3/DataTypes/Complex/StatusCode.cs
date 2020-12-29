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
        public String       Description       { get; }

        /// <summary>
        /// Optional additional information.
        /// </summary>
        [Optional]
        public String       AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new result status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">Optional additional information.</param>
        public StatusCode(StatusCodes  Code,
                          String       Description      = null,
                          String       AdditionalInfo   = null)
        {

            this.Code            = Code;
            this.Description     = Description;
            this.AdditionalInfo  = AdditionalInfo;

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
        public static StatusCode Parse(JObject                                  JSON,
                                       CustomJObjectParserDelegate<StatusCode>  CustomStatusCodeParser   = null)
        {

            if (TryParse(JSON,
                         out StatusCode statusCode,
                         out String     ErrorResponse,
                         CustomStatusCodeParser))
            {
                return statusCode;
            }

            throw new ArgumentException("The given JSON representation of a status code is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomStatusCodeParser = null)

        /// <summary>
        /// Parse the given text representation of a status code.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom status codes JSON objects.</param>
        public static StatusCode Parse(String                                   Text,
                                       CustomJObjectParserDelegate<StatusCode>  CustomStatusCodeParser   = null)
        {

            if (TryParse(Text,
                         out StatusCode statusCode,
                         out String     ErrorResponse,
                         CustomStatusCodeParser))
            {
                return statusCode;
            }

            throw new ArgumentException("The given text representation of a status code is invalid: " + ErrorResponse, nameof(Text));

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
        public static Boolean TryParse(JObject         JSON,
                                       out StatusCode  StatusCode,
                                       out String      ErrorResponse)

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
        public static Boolean TryParse(JObject                                  JSON,
                                       out StatusCode                           StatusCode,
                                       out String                               ErrorResponse,
                                       CustomJObjectParserDelegate<StatusCode>  CustomStatusCodeParser)
        {

            try
            {

                StatusCode = default;

                #region Parse StatusCode        [mandatory]

                if (!JSON.ParseMandatory("Code",
                                         "status code",
                                         StatusCodesExtentions.TryParse,
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


                StatusCode  = new StatusCode(StatusCodeValue,
                                             Description,
                                             AdditionalInfo);

                if (CustomStatusCodeParser != null)
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

        #region (static) TryParse(Text, out StatusCode, out ErrorResponse, CustomStatusCodeParser = null)

        /// <summary>
        /// Try to parse the given text representation of a status code.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="StatusCode">The parsed status code.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom status codes JSON objects.</param>
        public static Boolean TryParse(String                                   Text,
                                       out StatusCode                           StatusCode,
                                       out String                               ErrorResponse,
                                       CustomJObjectParserDelegate<StatusCode>  CustomStatusCodeParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out StatusCode,
                                out ErrorResponse,
                                CustomStatusCodeParser);

            }
            catch (Exception e)
            {
                StatusCode     = default;
                ErrorResponse  = "The given text representation of a status code is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<StatusCode>  CustomStatusCodeSerializer   = null)

        {

            var JSON = JSONObject.Create(

                           new JProperty("Code",  Code),

                           Description.   IsNotNullOrEmpty()
                               ? new JProperty("Description",     Description)
                               : null,

                           AdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("AdditionalInfo",  AdditionalInfo)
                               : null

                       );

            return CustomStatusCodeSerializer != null
                       ? CustomStatusCodeSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public StatusCode Clone

            => new StatusCode(Code,
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
        public static Boolean operator == (StatusCode StatusCode1,
                                           StatusCode StatusCode2)

            => StatusCode1.Equals(StatusCode2);

        #endregion

        #region Operator != (StatusCode1, StatusCode2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="StatusCode1">A status code.</param>
        /// <param name="StatusCode2">Another status code.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (StatusCode StatusCode1,
                                           StatusCode StatusCode2)

            => !(StatusCode1.Equals(StatusCode2));

        #endregion

        #endregion

        #region IEquatable<StatusCode> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is StatusCode statusCode &&
                   Equals(statusCode);

        #endregion

        #region Equals(StatusCode)

        /// <summary>
        /// Compares two status codes for equality.
        /// </summary>
        /// <param name="StatusCode">A status code to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(StatusCode StatusCode)

            => Code.          Equals(StatusCode.Code)        &&

             ((Description    == null && StatusCode.Description    == null) ||
              (Description    != null && StatusCode.Description    != null && Description.   Equals(StatusCode.Description))) &&

             ((AdditionalInfo == null && StatusCode.AdditionalInfo == null) ||
              (AdditionalInfo != null && StatusCode.AdditionalInfo != null && AdditionalInfo.Equals(StatusCode.AdditionalInfo)));

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

                return  Code.           GetHashCode()       * 5 ^
                       (Description?.   GetHashCode() ?? 0) * 3 ^
                       (AdditionalInfo?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("StatusCode: ", (Int32) Code,

                             Description.IsNotNullOrEmpty()
                                 ? ", description: " + Description
                                 : "",

                             AdditionalInfo.IsNotNullOrEmpty()
                                 ? ", additional Info: " + AdditionalInfo
                                 : "");

        #endregion


        #region ToBuilder()

        /// <summary>
        /// Return a status code builder.
        /// </summary>
        public Builder ToBuilder()

            => new Builder(Code,
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
            public String        Description       { get; set; }

            /// <summary>
            /// Optional additional information.
            /// </summary>
            [Optional]
            public String        AdditionalInfo    { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new status code builder.
            /// </summary>
            /// <param name="Code">The result code of the operation.</param>
            /// <param name="Description">An optional description of the result code.</param>
            /// <param name="AdditionalInfo">Optional additional information.</param>
            public Builder(StatusCodes?  Code             = null,
                           String        Description      = null,
                           String        AdditionalInfo   = null)
            {

                this.Code            = Code;
                this.Description     = Description;
                this.AdditionalInfo  = AdditionalInfo;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the status code.
            /// </summary>
            /// <param name="Builder">A status code builder.</param>
            public static implicit operator StatusCode(Builder Builder)

                => Builder?.ToImmutable();


            /// <summary>
            /// Return an immutable version of the status code.
            /// </summary>
            public StatusCode ToImmutable()
            {

                #region Check mandatory parameters

                if (!Code.HasValue)
                    throw new ArgumentException("The given result code must not be null!", nameof(Code));

                #endregion

                return new StatusCode(Code.Value,
                                      Description,
                                      AdditionalInfo);

            }

            #endregion

        }

        #endregion

    }

}