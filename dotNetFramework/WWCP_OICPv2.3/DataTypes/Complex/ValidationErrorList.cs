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

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// JSON schema validation errors.
    /// </summary>
    public class ValidationErrorList : IEquatable<ValidationErrorList>,
                                       IComparable<ValidationErrorList>,
                                       IComparable
    {

        #region Properties

        /// <summary>
        /// The error message.
        /// </summary>
        public String                        Message             { get; }

        /// <summary>
        /// The enumeration of validation errors.
        /// </summary>
        public IEnumerable<ValidationError>  ValidationErrors    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new list of JSON schema validation errors.
        /// </summary>
        /// <param name="Message">An error message.</param>
        /// <param name="ValidationErrors">An enumeration of validation errors.</param>
        public ValidationErrorList(String                        Message,
                                   IEnumerable<ValidationError>  ValidationErrors)
        {

            if (Message?.Trim().IsNullOrEmpty() == true)
                throw new ArgumentException("The given message is invalid!", nameof(Message));

            this.Message           = Message;
            this.ValidationErrors  = ValidationErrors ?? new ValidationError[0];

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a validation error.
        /// </summary>
        /// <param name="Text">A text representation of a validation error.</param>
        public static ValidationErrorList Parse(String Text)
        {

            if (TryParse(Text, out ValidationErrorList validationError))
                return validationError;

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a validation error must not be null or empty!");

            throw new ArgumentException("The given text representation of a validation error îs invalid!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a validation error.
        /// </summary>
        /// <param name="Text">A text representation of a validation error.</param>
        public static ValidationErrorList? TryParse(String Text)
        {

            if (TryParse(Text, out ValidationErrorList validationError))
                return validationError;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ValidationErrorList)

        public static Boolean TryParse(String Text, out ValidationErrorList ValidationErrorList)
        {

            ValidationErrorList = default;

            if (Text?.Trim().IsNotNullOrEmpty() == true)
            {
                try
                {

                    if (TryParse(JObject.Parse(Text), out ValidationErrorList))
                        return true;

                }
                catch (Exception)
                { }
            }

            return false;

        }

        #endregion

        #region (static) TryParse(JSON, out ValidationErrorList)

        public static Boolean TryParse(JObject JSON, out ValidationErrorList ValidationErrorList)
        {

            ValidationErrorList = default;

            if (JSON != null)
            {
                try
                {

                    #region Parse Message             [mandatory]

                    if (!JSON.ParseMandatoryText("message",
                                                 "message",
                                                 out String Message,
                                                 out String ErrorResponse))
                    {
                        return false;
                    }

                    #endregion

                    #region Parse ValidationErrors    [mandatory]

                    if (!JSON.ParseMandatoryJSON("validationErrors",
                                                 "validation errors",
                                                 ValidationError.TryParse,
                                                 out IEnumerable<ValidationError>  ValidationErrors,
                                                 out                               ErrorResponse))
                    {
                        return false;
                    }

                    #endregion


                    ValidationErrorList = new ValidationErrorList(Message,
                                                                  ValidationErrors);

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
        public ValidationErrorList Clone

            => new ValidationErrorList(Message,
                                       ValidationErrors);

        #endregion


        #region Operator overloading

        #region Operator == (ValidationErrorList1, ValidationErrorList2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList1">An enumeration of validation errors.</param>
        /// <param name="ValidationErrorList2">Another enumeration of validation errors.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ValidationErrorList ValidationErrorList1,
                                           ValidationErrorList ValidationErrorList2)

            => ValidationErrorList1.Equals(ValidationErrorList2);

        #endregion

        #region Operator != (ValidationErrorList1, ValidationErrorList2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList1">An enumeration of validation errors.</param>
        /// <param name="ValidationErrorList2">Another enumeration of validation errors.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ValidationErrorList ValidationErrorList1,
                                           ValidationErrorList ValidationErrorList2)

            => !(ValidationErrorList1 == ValidationErrorList2);

        #endregion

        #region Operator <  (ValidationErrorList1, ValidationErrorList2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList1">An enumeration of validation errors.</param>
        /// <param name="ValidationErrorList2">Another enumeration of validation errors.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ValidationErrorList ValidationErrorList1,
                                          ValidationErrorList ValidationErrorList2)

            => ValidationErrorList1.CompareTo(ValidationErrorList2) < 0;

        #endregion

        #region Operator <= (ValidationErrorList1, ValidationErrorList2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList1">An enumeration of validation errors.</param>
        /// <param name="ValidationErrorList2">Another enumeration of validation errors.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ValidationErrorList ValidationErrorList1,
                                           ValidationErrorList ValidationErrorList2)

            => !(ValidationErrorList1 > ValidationErrorList2);

        #endregion

        #region Operator >  (ValidationErrorList1, ValidationErrorList2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList1">An enumeration of validation errors.</param>
        /// <param name="ValidationErrorList2">Another enumeration of validation errors.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ValidationErrorList ValidationErrorList1,
                                          ValidationErrorList ValidationErrorList2)

            => ValidationErrorList1.CompareTo(ValidationErrorList2) > 0;

        #endregion

        #region Operator >= (ValidationErrorList1, ValidationErrorList2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList1">An enumeration of validation errors.</param>
        /// <param name="ValidationErrorList2">Another enumeration of validation errors.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ValidationErrorList ValidationErrorList1,
                                           ValidationErrorList ValidationErrorList2)

            => !(ValidationErrorList1 < ValidationErrorList2);

        #endregion

        #endregion

        #region IComparable<ValidationErrorList> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is ValidationErrorList validationErrorList
                   ? CompareTo(validationErrorList)
                   : throw new ArgumentException("The given object is not an enumeration of validation errors!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ValidationErrorList)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ValidationErrorList">An object to compare with.</param>
        public Int32 CompareTo(ValidationErrorList ValidationErrorList)
        {

            var c = Message.CompareTo(ValidationErrorList.Message);

            if (c == 0 && ValidationErrors.Count() != ValidationErrorList.ValidationErrors.Count())
                c = ValidationErrors.Count() > ValidationErrorList.ValidationErrors.Count() ? 1 : -1;

            if (c == 0)
            {
                for (var i = 0; i < ValidationErrors.Count(); i++)
                {
                    if (c == 0)
                        c = ValidationErrors.ElementAt(i).CompareTo(ValidationErrorList.ValidationErrors.ElementAt(i));
                    else
                        break;
                }
            }

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ValidationErrorList> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is ValidationErrorList validationErrorList &&
                   Equals(validationErrorList);

        #endregion

        #region Equals(ValidationErrorList)

        /// <summary>
        /// Compares two ValidationErrorLists for equality.
        /// </summary>
        /// <param name="ValidationErrorList">A ValidationErrorList to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ValidationErrorList ValidationErrorList)
        {

            if (!Message.Equals(ValidationErrorList.Message))
                return false;

            if (ValidationErrors.Count() != ValidationErrorList.ValidationErrors.Count())
                return false;

            for (var i = 0; i < ValidationErrors.Count(); i++)
            {
                if (!ValidationErrors.ElementAt(i).Equals(ValidationErrorList.ValidationErrors.ElementAt(i)))
                    return false;
            }

            return true;

        }

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
                return (Message?.GetHashCode() ?? 0) * 3 ^
                       ValidationErrors.Select(error => error.GetHashCode()).Aggregate((a,b) => a ^ b);
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Message, ": ", Environment.NewLine,
                             ValidationErrors.AggregateWith(Environment.NewLine));

        #endregion

    }

}
