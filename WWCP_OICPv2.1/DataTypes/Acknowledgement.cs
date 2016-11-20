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
using System.Linq;
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class Acknowledgement<T>
        where T : class
    {

        #region Properties

        public T                   Request             { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public Boolean             Result              { get; }

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        public StatusCode          StatusCode          { get; }

        /// <summary>
        /// An optional charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public Session_Id?         SessionId           { get; }

        /// <summary>
        /// An optional partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public PartnerSession_Id?  PartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        #region (private) Acknowledgement(Request, Result, StatusCode, ...)

        /// <summary>
        /// Create a new OICP acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        private Acknowledgement(T                   Request,
                                Boolean             Result,
                                StatusCode          StatusCode,
                                Session_Id?         SessionId         = null,
                                PartnerSession_Id?  PartnerSessionId  = null)
        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given request object must not be null!");

            #endregion

            this.Request           = Request;
            this.Result            = Result;
            this.StatusCode        = StatusCode;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion

        #region Acknowledgement(Request, SessionId, ...)

        /// <summary>
        /// Create a new OICP 'positive' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public Acknowledgement(T                   Request,
                               Session_Id          SessionId,
                               PartnerSession_Id?  PartnerSessionId          = null,
                               String              StatusCodeDescription     = null,
                               String              StatusCodeAdditionalInfo  = null)

            : this(Request,
                   true,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId)

        { }

        #endregion

        #region Acknowledgement(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'negative' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public Acknowledgement(T                   Request,
                               StatusCodes         StatusCode,
                               String              StatusCodeDescription     = null,
                               String              StatusCodeAdditionalInfo  = null,
                               Session_Id?         SessionId                 = null,
                               PartnerSession_Id?  PartnerSessionId          = null)

            : this(Request,
                   false,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId)

        { }

        #endregion

        #endregion


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

        #region (static) Parse(XML)

        /// <summary>
        /// Try to parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static Acknowledgement<T> Parse(T                    Request,
                                               XElement             XML,
                                               OnExceptionDelegate  OnException = null)
        {

            Acknowledgement<T> _Acknowledgement;

            if (TryParse(Request, XML, out _Acknowledgement, OnException))
                return _Acknowledgement;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, XML, out Acknowledgement, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        public static Boolean TryParse(T                       Request,
                                       XElement                XML,
                                       out Acknowledgement<T>  Acknowledgement,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                var AcknowledgementXML  = XML.Descendants(OICPNS.CommonTypes + "eRoamingAcknowledgement").
                                              FirstOrDefault();

                if (AcknowledgementXML == null && XML.Name == OICPNS.CommonTypes + "eRoamingAcknowledgement")
                    AcknowledgementXML = XML;

                if (AcknowledgementXML == null)
                {
                    Acknowledgement = null;
                    return false;
                }

                Acknowledgement = new Acknowledgement<T>(

                                      Request,

                                      AcknowledgementXML.ElementValueOrFail(OICPNS.CommonTypes + "Result") == "true",

                                      AcknowledgementXML.MapElementOrFail  (OICPNS.CommonTypes + "StatusCode",
                                                                            StatusCode.Parse,
                                                                            OnException),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "SessionID",
                                                                            Session_Id.Parse),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "PartnerSessionID",
                                                                            PartnerSession_Id.Parse)

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                Acknowledgement = null;
                return false;

            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML-representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                   new XElement(OICPNS.CommonTypes + "Result", Result),

                   StatusCode.ToXML(),

                   SessionId != null
                       ? new XElement(OICPNS.CommonTypes + "SessionID",         SessionId.ToString())
                       : null,

                   PartnerSessionId != null
                       ? new XElement(OICPNS.CommonTypes + "PartnerSessionID",  PartnerSessionId.ToString())
                       : null

             );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + StatusCode.Code, " / ", StatusCode.Description, " / ", StatusCode.AdditionalInfo);

        #endregion


    }


    /// <summary>
    /// An OICP Acknowledgement.
    /// </summary>
    public class Acknowledgement
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public Boolean             Result              { get; }

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        public StatusCode          StatusCode          { get; }

        /// <summary>
        /// An optional charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public Session_Id?         SessionId           { get; }

        /// <summary>
        /// An optional partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public PartnerSession_Id?  PartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        #region (private) Acknowledgement(Result, StatusCode, ...)

        /// <summary>
        /// Create a new OICP acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        private Acknowledgement(Boolean             Result,
                                StatusCode          StatusCode,
                                Session_Id?         SessionId         = null,
                                PartnerSession_Id?  PartnerSessionId  = null)
        {

            this.Result            = Result;
            this.StatusCode        = StatusCode;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion

        #region Acknowledgement(SessionId, ...)

        /// <summary>
        /// Create a new OICP 'positive' acknowledgement.
        /// </summary>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public Acknowledgement(Session_Id          SessionId,
                               PartnerSession_Id?  PartnerSessionId          = null,
                               String              StatusCodeDescription     = null,
                               String              StatusCodeAdditionalInfo  = null)

            : this(true,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId)

        { }

        #endregion

        #region Acknowledgement(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'negative' acknowledgement.
        /// </summary>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public Acknowledgement(StatusCodes         StatusCode,
                               String              StatusCodeDescription     = null,
                               String              StatusCodeAdditionalInfo  = null,
                               Session_Id?         SessionId                 = null,
                               PartnerSession_Id?  PartnerSessionId          = null)

            : this(false,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId)

        { }

        #endregion

        #endregion


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

        #region (static) Parse(XML)

        /// <summary>
        /// Try to parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static Acknowledgement Parse(XElement XML)
        {

            Acknowledgement _Acknowledgement;

            if (TryParse(XML, out _Acknowledgement))
                return _Acknowledgement;

            return null;

        }

        #endregion

        #region (static) TryParse(XML, out Acknowledgement)

        /// <summary>
        /// Parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        public static Boolean TryParse(XElement XML, out Acknowledgement Acknowledgement)
        {

            Acknowledgement = null;

            try
            {

                var AcknowledgementXML  = XML.Descendants(OICPNS.CommonTypes + "eRoamingAcknowledgement").
                                              FirstOrDefault();

                if (AcknowledgementXML == null && XML.Name == OICPNS.CommonTypes + "eRoamingAcknowledgement")
                    AcknowledgementXML = XML;

                if (AcknowledgementXML == null)
                    return false;

                Acknowledgement = new Acknowledgement((AcknowledgementXML.ElementValueOrFail(OICPNS.CommonTypes + "Result", "Missing 'Result'-XML-tag!") == "true")
                                                          ? true
                                                          : false,
                                                      StatusCode.Parse(AcknowledgementXML.ElementOrFail(OICPNS.CommonTypes + "StatusCode",
                                                                                                        "Missing 'StatusCode'-XML-tag!")),
                                                      AcknowledgementXML.MapValueOrDefault(OICPNS.CommonTypes + "SessionID",        Session_Id.Parse),
                                                      AcknowledgementXML.MapValueOrDefault(OICPNS.CommonTypes + "PartnerSessionID", PartnerSession_Id.Parse));

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML-representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                   new XElement(OICPNS.CommonTypes + "Result", Result),

                   StatusCode.ToXML(),

                   SessionId != null
                       ? new XElement(OICPNS.CommonTypes + "SessionID",         SessionId.ToString())
                       : null,

                   PartnerSessionId != null
                       ? new XElement(OICPNS.CommonTypes + "PartnerSessionID",  PartnerSessionId.ToString())
                       : null

             );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + StatusCode.Code, " / ", StatusCode.Description, " / ", StatusCode.AdditionalInfo);

        #endregion

    }

}
