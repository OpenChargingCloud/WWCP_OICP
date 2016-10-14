/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP status code.
    /// </summary>
    public class StatusCode
    {

        #region Properties

        /// <summary>
        /// The result code of the operation.
        /// </summary>
        public StatusCodes Code              { get; }

        /// <summary>
        /// Whether the operation was successful and returned a valid result.
        /// </summary>
        public Boolean     HasResult => Code == 0;

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
                          String       Description     = null,
                          String       AdditionalInfo  = null)
        {

            this.Code            = Code;
            this.Description     = Description.   IsNotNullOrEmpty() ? Description    : String.Empty;
            this.AdditionalInfo  = AdditionalInfo.IsNotNullOrEmpty() ? AdditionalInfo : String.Empty;

        }

        #endregion


        #region Documentation

        // <?xml version='1.0' encoding='UTF-8'?>
        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
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

        #region (static) Parse(StatusCodeXML)

        /// <summary>
        /// Parse the given XML representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeXML">The XML to parse.</param>
        public static StatusCode Parse(XElement  StatusCodeXML)
        {

            StatusCode _StatusCode;

            if (TryParse(StatusCodeXML, out _StatusCode))
                return _StatusCode;

            return null;

        }

        #endregion

        #region (static) TryParse(StatusCodeXML, out StatusCode, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP status code.
        /// </summary>
        /// <param name="StatusCodeXML">The XML to parse.</param>
        /// <param name="StatusCode">The parsed status code</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             StatusCodeXML,
                                       out StatusCode       StatusCode,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                // Sometimes CommonTypes:StatusCode, sometimes Authorization:StatusCode!
                if (StatusCodeXML.Name.LocalName != "StatusCode")
                {
                    StatusCode = null;
                    return false;
                }

                StatusCode = new StatusCode(StatusCodeXML.MapValueOrFail       (OICPNS.CommonTypes + "Code",
                                                                                str => (StatusCodes) Int16.Parse(str),
                                                                                "Invalid or missing 'Code' XML tag!"),

                                            StatusCodeXML.ElementValueOrDefault(OICPNS.CommonTypes + "Description",
                                                                                String.Empty),

                                            StatusCodeXML.ElementValueOrDefault(OICPNS.CommonTypes + "AdditionalInfo",
                                                                                String.Empty));

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, StatusCodeXML, e);

                StatusCode = null;
                return false;

            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.CommonTypes + "StatusCode",

                   new XElement(OICPNS.CommonTypes + "Code",  ((Int32) Code).ToString("D3")),

                   Description.IsNotNullOrEmpty()
                       ? new XElement(OICPNS.CommonTypes + "Description",     Description)
                       : null,

                   AdditionalInfo.IsNotNullOrEmpty()
                       ? new XElement(OICPNS.CommonTypes + "AdditionalInfo",  AdditionalInfo)
                       : null

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("StatusCode: ", ((Int32) Code), ", Description: ", Description, ", Additional Info: ", AdditionalInfo);

        #endregion

    }

}