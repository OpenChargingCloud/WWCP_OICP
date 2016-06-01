/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Illias;

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
        public Boolean Result => _Result;

        #endregion

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        public StatusCode StatusCode => _StatusCode;

        #endregion

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        /// <summary>
        /// An optional charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public ChargingSession_Id SessionId => _SessionId;

        #endregion

        #region PartnerSessionId

        private readonly ChargingSession_Id _PartnerSessionId;

        /// <summary>
        /// An optional partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public ChargingSession_Id PartnerSessionId => _PartnerSessionId;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">The description of the result code.</param>
        /// <param name="AdditionalInfo">Additional information.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public eRoamingAcknowledgement(Boolean             Result,
                                       Int32               Code,
                                       String              Description       = null,
                                       String              AdditionalInfo    = null,
                                       ChargingSession_Id  SessionId         = null,
                                       ChargingSession_Id  PartnerSessionId  = null)
        {

            this._Result            = Result;
            this._StatusCode        = new StatusCode(Code,
                                                     Description    ?? String.Empty,
                                                     AdditionalInfo ?? String.Empty);
            this._SessionId         = SessionId;
            this._PartnerSessionId  = PartnerSessionId;

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
            //
            //       <CommonTypes:Result>true</CommonTypes:Result>
            //
            //       <CommonTypes:StatusCode>
            //         <CommonTypes:Code>000</CommonTypes:Code>
            //         <CommonTypes:Description>Success</CommonTypes:Description>
            //         <CommonTypes:AdditionalInfo />
            //       </CommonTypes:StatusCode>
            //
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
            //
            //       <CommonTypes:Result>true</CommonTypes:Result>
            //
            //       <CommonTypes:StatusCode>
            //         <CommonTypes:Code>009</CommonTypes:Code>
            //         <CommonTypes:Description>Data transaction error</CommonTypes:Description>
            //         <CommonTypes:AdditionalInfo>The Push of data is already in progress.</CommonTypes:AdditionalInfo>
            //       </CommonTypes:StatusCode>
            //
            //     </CommonTypes:eRoamingAcknowledgement>
            //   </soapenv:Body>
            //
            // </soapenv:Envelope>

            // HTTP/1.1 200 OK
            // Date: Tue, 31 May 2016 03:15:36 GMT
            // Content-Type: text/xml;charset=utf-8
            // Connection: close
            // Transfer-Encoding: chunked
            // 
            // <?xml version='1.0' encoding='UTF-8'?>
            // <soapenv:Envelope xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0"
            //                   xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/">
            //
            //   <soapenv:Body>
            //     <CommonTypes:eRoamingAcknowledgement>
            //
            //       <CommonTypes:Result>true</CommonTypes:Result>
            //
            //       <CommonTypes:StatusCode>
            //         <CommonTypes:Code>000</CommonTypes:Code>
            //         <CommonTypes:Description>Success</CommonTypes:Description>
            //       </CommonTypes:StatusCode>
            //
            //       <CommonTypes:SessionID>04cf39ad-0a88-1295-27dc-d593d1a076ac</CommonTypes:SessionID>
            //
            //     </CommonTypes:eRoamingAcknowledgement>
            //   </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            Acknowledgement = null;

            try
            {

                var AcknowledgementXML  = XML.Descendants(OICPNS.CommonTypes + "eRoamingAcknowledgement").
                                              FirstOrDefault();

                if (AcknowledgementXML == null && XML.Name == OICPNS.CommonTypes + "eRoamingAcknowledgement")
                    AcknowledgementXML = XML;

                if (AcknowledgementXML == null)
                    return false;

                var Result          = (AcknowledgementXML.ElementValueOrFail(OICPNS.CommonTypes + "Result", "Missing 'Result'-XML-tag!") == "true")
                                           ? true
                                           : false;

                var SessionId         = AcknowledgementXML.MapValueOrDefault(OICPNS.CommonTypes + "SessionID",        ChargingSession_Id.Parse);
                var PartnerSessionId  = AcknowledgementXML.MapValueOrDefault(OICPNS.CommonTypes + "PartnerSessionID", ChargingSession_Id.Parse);


                var StatusCodeXML     = AcknowledgementXML.ElementOrFail    (OICPNS.CommonTypes + "StatusCode",
                                                                             "Missing 'StatusCode'-XML-tag!");

                var Code              = StatusCodeXML.MapValueOrFail        (OICPNS.CommonTypes + "Code",             Int16.Parse,
                                                                             "Invalid or missing 'StatusCode/Code' XML-tag!");

                var Description       = StatusCodeXML.ElementValueOrDefault (OICPNS.CommonTypes + "Description");

                var AdditionalInfo    = StatusCodeXML.ElementValueOrDefault (OICPNS.CommonTypes + "AdditionalInfo");

                Acknowledgement = new eRoamingAcknowledgement(Result,
                                                              Code,
                                                              Description    ?? "",
                                                              AdditionalInfo ?? "",
                                                              SessionId,
                                                              PartnerSessionId);

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

        #endregion


        #region ToXML

        /// <summary>
        /// Return a XML-representation of this object.
        /// </summary>
        public XElement ToXML => new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                     new XElement(OICPNS.CommonTypes + "Result", _Result),

                                     _StatusCode.ToXML,

                                     _SessionId != null
                                         ? new XElement(OICPNS.CommonTypes + "SessionID",         _SessionId.ToString())
                                         : null,

                                     _PartnerSessionId != null
                                         ? new XElement(OICPNS.CommonTypes + "PartnerSessionID",  _PartnerSessionId.ToString())
                                         : null

                               );

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
