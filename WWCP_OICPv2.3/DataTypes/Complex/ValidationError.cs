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

            if (FieldReference?.Trim().IsNullOrEmpty() == true)
                throw new ArgumentException("The given field reference is invalid!",  nameof(FieldReference));

            if (ErrorMessage?.  Trim().IsNullOrEmpty() == true)
                throw new ArgumentException("The given error message is invalid!",    nameof(ErrorMessage));

            this.FieldReference  = FieldReference;
            this.ErrorMessage    = ErrorMessage;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a validation error.
        /// </summary>
        /// <param name="Text">A text representation of a validation error.</param>
        public static ValidationError Parse(String Text)
        {

            if (TryParse(Text, out ValidationError validationError))
                return validationError;

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a validation error must not be null or empty!");

            throw new ArgumentException("The given text representation of a validation error îs invalid!", nameof(Text));

        }

        #endregion

        #region (static) Parse   (JSON)

        /// <summary>
        /// Parse the given JSON as a validation error.
        /// </summary>
        /// <param name="JSON">A JSON representation of a validation error.</param>
        public static ValidationError Parse(JObject JSON)
        {

            if (TryParse(JSON, out ValidationError validationError))
                return validationError;

            throw new ArgumentException("The given JSON representation of a validation error îs invalid!", nameof(JSON));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a validation error.
        /// </summary>
        /// <param name="Text">A text representation of a validation error.</param>
        public static ValidationError? TryParse(String Text)
        {

            if (TryParse(Text, out ValidationError validationError))
                return validationError;

            return null;

        }

        #endregion

        #region (static) TryParse(JSON)

        /// <summary>
        /// Try to parse the given JSON as a validation error.
        /// </summary>
        /// <param name="JSON">A JSON representation of a validation error.</param>
        public static ValidationError? TryParse(JObject JSON)
        {

            if (TryParse(JSON, out ValidationError validationError))
                return validationError;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ValidationError)

        public static Boolean TryParse(String Text, out ValidationError ValidationError)
        {

            ValidationError = default;

            if (Text?.Trim().IsNotNullOrEmpty() == true)
            {
                try
                {

                    if (TryParse(JObject.Parse(Text), out ValidationError))
                        return true;

                }
                catch (Exception)
                { }
            }

            return false;

        }

        #endregion

        #region (static) TryParse(JSON, out ValidationError)

        public static Boolean TryParse(JObject JSON, out ValidationError ValidationError)
        {

            ValidationError = default;

            if (JSON != null)
            {
                try
                {

                    #region Parse FieldReference    [mandatory]

                    if (!JSON.ParseMandatoryText("fieldReference",
                                                 "field reference",
                                                 out String FieldReference,
                                                 out String ErrorResponse))
                    {
                        return false;
                    }

                    #endregion

                    #region Parse ErrorMessage      [mandatory]

                    if (!JSON.ParseMandatoryText("errorMessage",
                                                 "error message",
                                                 out String ErrorMessage,
                                                 out        ErrorResponse))
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

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public ValidationError Clone

            => new ValidationError(FieldReference,
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

            => !(ValidationError1 == ValidationError2);

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

            => !(ValidationError1 > ValidationError2);

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

            => !(ValidationError1 < ValidationError2);

        #endregion

        #endregion

        #region IComparable<ValidationError> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is ValidationError validationError
                   ? CompareTo(validationError)
                   : throw new ArgumentException("The given object is not a validation error!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ValidationError)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationError">An object to compare with.</param>
        public Int32 CompareTo(ValidationError ValidationError)
        {

            var result = FieldReference.CompareTo(ValidationError.FieldReference);

            return result == 0
                       ? ErrorMessage.CompareTo(ValidationError.ErrorMessage)
                       : result;

        }

        #endregion

        #endregion

        #region IEquatable<ValidationError> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ValidationError validationError &&
                   Equals(validationError);

        #endregion

        #region Equals(ValidationError)

        /// <summary>
        /// Compares two ValidationErrors for equality.
        /// </summary>
        /// <param name="ValidationError">A ValidationError to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
