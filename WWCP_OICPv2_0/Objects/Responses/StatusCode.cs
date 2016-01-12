/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A Hubject status code.
    /// </summary>
    public class StatusCode
    {

        #region Properties

        #region Code

        private readonly Int16 _Code;

        /// <summary>
        /// The result code of the operation.
        /// </summary>
        public Int16 Code
        {
            get
            {
                return _Code;
            }
        }

        #endregion

        #region HasResult

        /// <summary>
        /// Whether the operation was successful and returned a valid result.
        /// </summary>
        public Boolean HasResult
        {
            get
            {
                return _Code == 0;
            }
        }

        #endregion

        #region Description

        private readonly String _Description;

        /// <summary>
        /// The description of the result code.
        /// </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #region AdditionalInfo

        private readonly String _AdditionalInfo;

        /// <summary>
        /// Additional information.
        /// </summary>
        public String AdditionalInfo
        {
            get
            {
                return _AdditionalInfo;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Hubject status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">The description of the result code.</param>
        /// <param name="AdditionalInfo">Additional information.</param>
        public StatusCode(Int16   Code,
                          String  Description     = null,
                          String  AdditionalInfo  = null)
        {

            this._Code            = Code;
            this._Description     = Description    != null ? Description    : String.Empty;
            this._AdditionalInfo  = AdditionalInfo != null ? AdditionalInfo : String.Empty;

        }

        #endregion


        #region (static) Parse(StatusCodeXML)

        /// <summary>
        /// Create a new Hubject status code.
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
        /// Create a new Hubject status code.
        /// </summary>
        /// <param name="StatusCodeXML">The XML to parse.</param>
        /// <param name="StatusCode">The parsed status code</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             StatusCodeXML,
                                       out StatusCode       StatusCode,
                                       OnExceptionDelegate  OnException  = null)
        {

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

            StatusCode = null;

            try
            {

                // Sometimes CommonTypes:StatusCode, sometimes Authorization:StatusCode!
                if (StatusCodeXML.Name.LocalName != "StatusCode")
                    return false;

                var _Code            = StatusCodeXML.Element(OICPNS.CommonTypes + "Code");
                var _Description     = StatusCodeXML.Element(OICPNS.CommonTypes + "Description");
                var _AdditionalInfo  = StatusCodeXML.Element(OICPNS.CommonTypes + "AdditionalInfo");

                if (_Code == null)
                    return false;

                Int16 __Code;
                if (!Int16.TryParse(_Code.Value, out __Code))
                    return false;

                StatusCode = new StatusCode(__Code,
                                            _Description    != null ? _Description.   Value : String.Empty,
                                            _AdditionalInfo != null ? _AdditionalInfo.Value : String.Empty);

                return true;

            }
            catch (Exception e)
            {

                if (OnException != null)
                    OnException(DateTime.Now, StatusCodeXML, e);

                return false;

            }

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat("StatusCode: ", _Code, ", Description: ", _Description, ", Additional Info: ", _AdditionalInfo);
        }

        #endregion

    }

}
