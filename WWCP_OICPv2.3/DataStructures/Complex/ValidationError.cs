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
    /// A JSON schema validation error.
    /// </summary>
    public readonly struct ValidationError : IEquatable<ValidationError>,
                                             IComparable<ValidationError>,
                                             IComparable
    {

        #region Properties

        /// <summary>
        /// The field reference.
        /// </summary>
        public readonly String  FieldReference    { get; }

        /// <summary>
        /// The error message.
        /// </summary>
        public readonly String  ErrorMessage      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new JSON schema validation error.
        /// </summary>
        /// <param name="FieldReference">The field reference.</param>
        /// <param name="ErrorMessage">The error message.</param>
        public ValidationError(String  FieldReference,
                               String  ErrorMessage)
        {

            if (FieldReference is null)
                throw new ArgumentException(nameof(FieldReference), "The given field reference must not be null!");

            if (FieldReference.Trim().IsNullOrEmpty())
                throw new ArgumentException("The given field reference is invalid!", nameof(FieldReference));


            if (ErrorMessage   is null)
                throw new ArgumentNullException(nameof(ErrorMessage), "The given error message must not be null!");

            if (ErrorMessage.  Trim().IsNullOrEmpty())
                throw new ArgumentException    ("The given error message is invalid!", nameof(ErrorMessage));


            this.FieldReference  = FieldReference;
            this.ErrorMessage    = ErrorMessage;

        }

        #endregion


        #region (static) Parse   (JSON)

        /// <summary>
        /// Parse the given JSON as a validation error.
        /// </summary>
        /// <param name="JSON">A JSON representation of a validation error.</param>
        public static ValidationError Parse(JObject JSON)
        {

            if (TryParse(JSON,
                         out var validationError,
                         out var errorResponse))
            {
                return validationError;
            }

            throw new ArgumentException("The given JSON representation of a validation error is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON)

        /// <summary>
        /// Try to parse the given JSON as a validation error.
        /// </summary>
        /// <param name="JSON">A JSON representation of a validation error.</param>
        public static ValidationError? TryParse(JObject JSON)
        {

            if (TryParse(JSON,
                         out var validationError,
                         out _))
            {
                return validationError;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(JSON, out ValidationError, out ErrorResponse)

        public static Boolean TryParse(JObject              JSON,
                                       out ValidationError  ValidationError,
                                       out String?          ErrorResponse)
        {

            ErrorResponse    = default;
            ValidationError  = default;

            if (JSON is not null)
            {
                try
                {

                    #region Parse FieldReference    [mandatory]

                    if (!JSON.ParseMandatoryText("fieldReference",
                                                 "field reference",
                                                 out String FieldReference,
                                                 out ErrorResponse))
                    {
                        return false;
                    }

                    #endregion

                    #region Parse ErrorMessage      [mandatory]

                    if (!JSON.ParseMandatoryText("errorMessage",
                                                 "error message",
                                                 out String ErrorMessage,
                                                 out ErrorResponse))
                    {
                        return false;
                    }

                    #endregion


                    ValidationError = new ValidationError(FieldReference,
                                                          ErrorMessage);

                    return true;

                }
                catch (Exception)
                { }
            }

            return false;

        }

        #endregion

        #region ToJSON()

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public JObject ToJSON()

            => JSONObject.Create(
                   new JProperty("fieldReference", FieldReference),
                   new JProperty("errorMessage",   ErrorMessage)
               );

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public ValidationError Clone

            => new (FieldReference,
                    ErrorMessage);

        #endregion


        #region Operator overloading

        #region Operator == (ValidationError1, ValidationError2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError1">A validation error.</param>
        /// <param name="ValidationError2">Another validation error.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ValidationError ValidationError1,
                                           ValidationError ValidationError2)

            => ValidationError1.Equals(ValidationError2);

        #endregion

        #region Operator != (ValidationError1, ValidationError2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError1">A validation error.</param>
        /// <param name="ValidationError2">Another validation error.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ValidationError ValidationError1,
                                           ValidationError ValidationError2)

            => !ValidationError1.Equals(ValidationError2);

        #endregion

        #region Operator <  (ValidationError1, ValidationError2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError1">A validation error.</param>
        /// <param name="ValidationError2">Another validation error.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ValidationError ValidationError1,
                                          ValidationError ValidationError2)

            => ValidationError1.CompareTo(ValidationError2) < 0;

        #endregion

        #region Operator <= (ValidationError1, ValidationError2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError1">A validation error.</param>
        /// <param name="ValidationError2">Another validation error.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ValidationError ValidationError1,
                                           ValidationError ValidationError2)

            => ValidationError1.CompareTo(ValidationError2) <= 0;

        #endregion

        #region Operator >  (ValidationError1, ValidationError2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError1">A validation error.</param>
        /// <param name="ValidationError2">Another validation error.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ValidationError ValidationError1,
                                          ValidationError ValidationError2)

            => ValidationError1.CompareTo(ValidationError2) > 0;

        #endregion

        #region Operator >= (ValidationError1, ValidationError2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError1">A validation error.</param>
        /// <param name="ValidationError2">Another validation error.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ValidationError ValidationError1,
                                           ValidationError ValidationError2)

            => ValidationError1.CompareTo(ValidationError2) >= 0;

        #endregion

        #endregion

        #region IComparable<ValidationError> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two validation errors.
        /// </summary>
        /// <param name="Object">A validation error to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ValidationError validationError
                   ? CompareTo(validationError)
                   : throw new ArgumentException("The given object is not a validation error!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ValidationError)

        /// <summary>
        /// Compares two validation errors.
        /// </summary>
        /// <param name="ValidationError">A validation error to compare with.</param>
        public Int32 CompareTo(ValidationError ValidationError)
        {

            var c = FieldReference.CompareTo(ValidationError.FieldReference);

            if (c == 0)
                return ErrorMessage.CompareTo(ValidationError.ErrorMessage);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ValidationError> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two validation errors for equality.
        /// </summary>
        /// <param name="Object">A validation error to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ValidationError validationError &&
                   Equals(validationError);

        #endregion

        #region Equals(ValidationError)

        /// <summary>
        /// Compares two validation errors for equality.
        /// </summary>
        /// <param name="ValidationError">A validation error to compare with.</param>
        public Boolean Equals(ValidationError ValidationError)

            => FieldReference.Equals(ValidationError.FieldReference) &&
               ErrorMessage.  Equals(ValidationError.ErrorMessage);

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
                return (FieldReference?.GetHashCode() ?? 0) * 3 ^
                       (ErrorMessage?.  GetHashCode() ?? 0);
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Field '",
                             FieldReference, "' ",
                             ErrorMessage, "!");

        #endregion

    }

}
