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

using System;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An acknowledgement result.
    /// </summary>
    public class eRoamingAcknowledgement
    {

        #region Properties

        #region Result

        private readonly Boolean _Result;

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public Boolean Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">The description of the result code.</param>
        /// <param name="AdditionalInfo">Additional information.</param>
        public eRoamingAcknowledgement(Boolean  Result,
                                       Int16    Code,
                                       String   Description     = null,
                                       String   AdditionalInfo  = null)
        {

            this._Result      = Result;
            this._StatusCode  = new StatusCode(Code,
                                               Description    != null ? Description    : String.Empty,
                                               AdditionalInfo != null ? AdditionalInfo : String.Empty);

        }

        #endregion


        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static eRoamingAcknowledgement Parse(XElement XML)
        {

            eRoamingAcknowledgement _Acknowledgement;

            if (TryParse(XML, out _Acknowledgement))
                return _Acknowledgement;

            return null;

        }

        #endregion

        #region (static) TryParse(XML, out Acknowledgement)

        /// <summary>
        /// Create a new Hubject Acknowledgement result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        public static Boolean TryParse(XElement XML, out eRoamingAcknowledgement Acknowledgement)
        {

            #region Documentation

            // <?xml version='1.0' encoding='UTF-8'?>
            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //   <soapenv:Body>
            //     <CommonTypes:eRoamingAcknowledgement>
            //       <CommonTypes:Result>true</CommonTypes:Result>
            //       <CommonTypes:StatusCode>
            //         <CommonTypes:Code>000</CommonTypes:Code>
            //         <CommonTypes:Description>Success</CommonTypes:Description>
            //         <CommonTypes:AdditionalInfo />
            //       </CommonTypes:StatusCode>
            //     </CommonTypes:eRoamingAcknowledgement>
            //   </soapenv:Body>
            //
            // </soapenv:Envelope>

            // <?xml version='1.0' encoding='UTF-8'?>
            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //   <soapenv:Body>
            //     <CommonTypes:eRoamingAcknowledgement>
            //       <CommonTypes:Result>true</CommonTypes:Result>
            //       <CommonTypes:StatusCode>
            //         <CommonTypes:Code>009</CommonTypes:Code>
            //         <CommonTypes:Description>Data transaction error</CommonTypes:Description>
            //         <CommonTypes:AdditionalInfo>The Push of data is already in progress.</CommonTypes:AdditionalInfo>
            //       </CommonTypes:StatusCode>
            //     </CommonTypes:eRoamingAcknowledgement>
            //   </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            Acknowledgement = null;

            try
            {

                var ack              = XML.Descendants(OICPNS.CommonTypes + "eRoamingAcknowledgement").
                                           FirstOrDefault();

                if (ack == null && XML.Name == OICPNS.CommonTypes + "eRoamingAcknowledgement")
                    ack = XML;

                if (ack == null)
                    return false;

                var _Result          = (ack.Element(OICPNS.CommonTypes + "Result").Value == "true")
                                           ? true
                                           : false;

                var StatusCode       = ack.Element(OICPNS.CommonTypes + "StatusCode");

                Int16 _Code;
                if (!Int16.TryParse(StatusCode.Element(OICPNS.CommonTypes + "Code").Value, out _Code))
                    return false;

                var _Description     = StatusCode.Element(OICPNS.CommonTypes + "Description").Value;

                var _AdditionalInfo  = (StatusCode.Element(OICPNS.CommonTypes + "AdditionalInfo") != null)
                                           ? StatusCode.Element(OICPNS.CommonTypes + "AdditionalInfo").Value
                                           : String.Empty;

                Acknowledgement = new eRoamingAcknowledgement(_Result, _Code, _Description, _AdditionalInfo);

                return true;

            }
            catch (Exception e)
            {
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
            return String.Concat("Result: " + _Result + "; " + _StatusCode.Code, " / ", _StatusCode.Description, " / ", _StatusCode.AdditionalInfo);
        }

        #endregion

    }

}
