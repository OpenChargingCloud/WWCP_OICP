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
using System.Xml.Linq;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.JSON;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Extention methods for OICP status codes.
    /// </summary>
    public static class StatusCodeExtentions
    {

        /// <summary>
        /// Whether the given status code is valid and has results.
        /// </summary>
        /// <param name="StatusCode">A status code.</param>
        public static Boolean HasResult(this StatusCode? StatusCode)
        {

            if (!StatusCode.HasValue)
                return false;

            return StatusCode.Value.HasResult;

        }

    }


    /// <summary>
    /// An OICP status code.
    /// </summary>
    public struct StatusCode
    {

        #region Properties

        /// <summary>
        /// The result code of the operation.
        /// </summary>
        public StatusCodes Code              { get; }

        /// <summary>
        /// Whether the operation was successful and returned a valid result.
        /// </summary>
        public Boolean     HasResult
            => Code == 0;

        /// <summary>
        /// The description of the result code.
        /// </summary>
        public String      Description       { get; }

        /// <summary>
        /// Additional information.
        /// </summary>
        public String      AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
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

        // <?xml version='1.0' encoding='UTF-8'?>
        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        //
        // [...]
        //
        //  <CommonTypes:StatusCode>
        //    <CommonTypes:Code>000</CommonTypes:Code>
        //    <CommonTypes:Description>Success</CommonTypes:Description>
        //    <CommonTypes:AdditionalInfo />
        //  </CommonTypes:StatusCode>
        //
        // [...]

        #endregion

        #region (static) Parse   (StatusCodeXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeXML">The XML to parse.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static StatusCode Parse(XElement                             StatusCodeXML,
                                       CustomXMLParserDelegate<StatusCode>  CustomStatusCodeParser   = null,
                                       OnExceptionDelegate                  OnException              = null)
        {

            if (TryParse(StatusCodeXML,
                         out StatusCode statusCode,
                         CustomStatusCodeParser,
                         OnException))

                return statusCode;

            return default;

        }

        #endregion

        #region (static) Parse   (StatusCodeText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeText">The text to parse.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static StatusCode Parse(String                               StatusCodeText,
                                       CustomXMLParserDelegate<StatusCode>  CustomStatusCodeParser   = null,
                                       OnExceptionDelegate                  OnException              = null)
        {

            if (TryParse(StatusCodeText,
                         out StatusCode statusCode,
                         CustomStatusCodeParser,
                         OnException))

                return statusCode;

            return default;

        }

        #endregion

        #region (static) TryParse(StatusCodeXML,                  ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeXML">The XML to parse.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static StatusCode? TryParse(XElement                             StatusCodeXML,
                                           CustomXMLParserDelegate<StatusCode>  CustomStatusCodeParser   = null,
                                           OnExceptionDelegate                  OnException              = null)
        {

            try
            {

                if (!(StatusCodeXML.Name == OICPNS.CommonTypes +   "StatusCode" ||
                      StatusCodeXML.Name == OICPNS.Authorization + "StatusCode"))
                {
                    return null;
                }

                var StatusCode = new StatusCode(StatusCodeXML.MapValueOrFail       (OICPNS.CommonTypes + "Code",
                                                                                    s => (StatusCodes) Int16.Parse(s),
                                                                                    "Invalid or missing 'Code' XML tag!"),

                                                StatusCodeXML.ElementValueOrDefault(OICPNS.CommonTypes + "Description",
                                                                                    String.Empty),

                                                StatusCodeXML.ElementValueOrDefault(OICPNS.CommonTypes + "AdditionalInfo",
                                                                                    String.Empty));

                if (CustomStatusCodeParser != null)
                    StatusCode = CustomStatusCodeParser(StatusCodeXML,
                                                        StatusCode);

                return StatusCode;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, StatusCodeXML, e);

                return null;

            }

        }

        #endregion

        #region (static) TryParse(StatusCodeXML,  out StatusCode, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeXML">The XML to parse.</param>
        /// <param name="StatusCode">The parsed status code</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                             StatusCodeXML,
                                       out StatusCode                       StatusCode,
                                       CustomXMLParserDelegate<StatusCode>  CustomStatusCodeParser   = null,
                                       OnExceptionDelegate                  OnException              = null)
        {

            var statusCode = TryParse(StatusCodeXML,
                                      CustomStatusCodeParser,
                                      OnException);

            if (statusCode.HasValue)
            {
                StatusCode = statusCode.Value;
                return true;
            }

            StatusCode = default;
            return false;

        }

        #endregion

        #region (static) TryParse(StatusCodeText,                 ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeText">The text to parse.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static StatusCode? TryParse(String                               StatusCodeText,
                                           CustomXMLParserDelegate<StatusCode>  CustomStatusCodeParser   = null,
                                           OnExceptionDelegate                  OnException              = null)
        {

            try
            {

                return TryParse(XDocument.Parse(StatusCodeText).Root,
                                CustomStatusCodeParser,
                                OnException);

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, StatusCodeText, e);
            }

            return null;

        }

        #endregion

        #region (static) TryParse(StatusCodeText, out StatusCode, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeText">The text to parse.</param>
        /// <param name="StatusCode">The parsed status code.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                               StatusCodeText,
                                       out StatusCode                       StatusCode,
                                       CustomXMLParserDelegate<StatusCode>  CustomStatusCodeParser   = null,
                                       OnExceptionDelegate                  OnException              = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(StatusCodeText).Root,
                             out StatusCode,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, StatusCodeText, e);
            }

            StatusCode = default;
            return false;

        }

        #endregion

        #region ToXML(XName = null, CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        public XElement ToXML(XName                                    XName                        = null,
                              CustomXMLSerializerDelegate<StatusCode>  CustomStatusCodeSerializer   = null)

        {

            var XML = new XElement(XName ?? OICPNS.CommonTypes + "StatusCode",

                          new XElement(OICPNS.CommonTypes + "Code",  ((Int32) Code).ToString("D3")),

                          Description.IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "Description",     Description)
                              : null,

                          AdditionalInfo.IsNotNullOrEmpty()
                              ? new XElement(OICPNS.CommonTypes + "AdditionalInfo",  AdditionalInfo)
                              : null

                      );

            return CustomStatusCodeSerializer != null
                       ? CustomStatusCodeSerializer(this, XML)
                       : XML;

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

                           new JProperty("code",  Code),

                           Description.IsNotNullOrEmpty()
                               ? new JProperty("description",     Description)
                               : null,

                           AdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("additionalInfo",  AdditionalInfo)
                               : null

                       );

            return CustomStatusCodeSerializer != null
                       ? CustomStatusCodeSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (StatusCode1, StatusCode2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="StatusCode1">A status code.</param>
        /// <param name="StatusCode2">Another status code.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (StatusCode StatusCode1, StatusCode StatusCode2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(StatusCode1, StatusCode2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) StatusCode1 == null) || ((Object) StatusCode2 == null))
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
        public static Boolean operator != (StatusCode StatusCode1, StatusCode StatusCode2)

            => !(StatusCode1 == StatusCode2);

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
        {

            if (Object is null)
                return false;

            if (!(Object is StatusCode StatusCode))
                return false;

            return Equals(StatusCode);

        }

        #endregion

        #region Equals(StatusCode)

        /// <summary>
        /// Compares two status codes for equality.
        /// </summary>
        /// <param name="StatusCode">A status code to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(StatusCode StatusCode)
        {

            if ((Object) StatusCode == null)
                return false;

            return Code.Equals(StatusCode.Code) &&

                   ((Description    == null && StatusCode.Description    == null) ||
                    (Description    != null && StatusCode.Description    != null && Description.   Equals(StatusCode.Description))) &&

                   ((AdditionalInfo == null && StatusCode.AdditionalInfo == null) ||
                    (AdditionalInfo != null && StatusCode.AdditionalInfo != null && AdditionalInfo.Equals(StatusCode.AdditionalInfo)));

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

                return Code.GetHashCode() * 5 ^

                       (Description != null
                            ? Description.   GetHashCode()
                            : 0) * 3 ^

                       (AdditionalInfo != null
                            ? AdditionalInfo.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("StatusCode: ", (Int32) Code,

                             Description.IsNotNullOrEmpty()
                                 ? ", Description: " + Description
                                 : "",

                             AdditionalInfo.IsNotNullOrEmpty()
                                 ? ", Additional Info: " + AdditionalInfo
                                 : "");

        #endregion

    }

}