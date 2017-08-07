/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class Acknowledgement<TRequest> : AResponse<TRequest,
                                                       Acknowledgement<TRequest>>

        where TRequest : class, IRequest

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

        #region (private) Acknowledgement(Request, Result, StatusCode, ...)

        /// <summary>
        /// Create a new OICP acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        private Acknowledgement(TRequest                             Request,
                                Boolean                              Result,
                                StatusCode                           StatusCode,
                                Session_Id?                          SessionId          = null,
                                PartnerSession_Id?                   PartnerSessionId   = null,
                                IReadOnlyDictionary<String, Object>  CustomData         = null)

            : base(Request,
                   CustomData)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given request object must not be null!");

            #endregion

            this.Result            = Result;
            this.StatusCode        = StatusCode;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion

        #region Acknowledgement(Request, SessionId = null, ...)

        /// <summary>
        /// Create a new OICP 'positive' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public Acknowledgement(TRequest                             Request,
                               Session_Id?                          SessionId                  = null,
                               PartnerSession_Id?                   PartnerSessionId           = null,
                               String                               StatusCodeDescription      = null,
                               String                               StatusCodeAdditionalInfo   = null,
                               IReadOnlyDictionary<String, Object>  CustomData                 = null)

            : this(Request,
                   true,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId,
                   CustomData)

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
        public Acknowledgement(TRequest                             Request,
                               StatusCodes                          StatusCode,
                               String                               StatusCodeDescription      = null,
                               String                               StatusCodeAdditionalInfo   = null,
                               Session_Id?                          SessionId                  = null,
                               PartnerSession_Id?                   PartnerSessionId           = null,
                               IReadOnlyDictionary<String, Object>  CustomData                 = null)

            : this(Request,
                   false,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId,
                   CustomData)

        { }

        #endregion

        #endregion


        #region (static) Success(Request, SessionId = null, ...)

        /// <summary>
        /// Create a new OICP 'positive' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public static Acknowledgement<TRequest>

            Success(TRequest            Request,
                    Session_Id?         SessionId                  = null,
                    PartnerSession_Id?  PartnerSessionId           = null,
                    String              StatusCodeDescription      = null,
                    String              StatusCodeAdditionalInfo   = null)

                => new Acknowledgement<TRequest>(Request,
                                                 true,
                                                 new StatusCode(
                                                     StatusCodes.Success,
                                                     StatusCodeDescription,
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion


        #region (static) DataError                  (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'DataError' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            DataError(TRequest            Request,
                      String              StatusCodeDescription      = null,
                      String              StatusCodeAdditionalInfo   = null,
                      Session_Id?         SessionId                  = null,
                      PartnerSession_Id?  PartnerSessionId           = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.DataError,
                                                     StatusCodeDescription ?? "Data Error!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) SystemError                (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            SystemError(TRequest            Request,
                        String              StatusCodeDescription      = null,
                        String              StatusCodeAdditionalInfo   = null,
                        Session_Id?         SessionId                  = null,
                        PartnerSession_Id?  PartnerSessionId           = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     StatusCodeDescription ?? "System Error!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) ServiceNotAvailable        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'ServiceNotAvailable' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            ServiceNotAvailable(TRequest            Request,
                                String              StatusCodeDescription      = null,
                                String              StatusCodeAdditionalInfo   = null,
                                Session_Id?         SessionId                  = null,
                                PartnerSession_Id?  PartnerSessionId           = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.ServiceNotAvailable,
                                                     StatusCodeDescription ?? "Service not available!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) SessionIsInvalid           (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'SessionIsInvalid' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            SessionIsInvalid(TRequest            Request,
                             String              StatusCodeDescription      = null,
                             String              StatusCodeAdditionalInfo   = null,
                             Session_Id?         SessionId                  = null,
                             PartnerSession_Id?  PartnerSessionId           = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.SessionIsInvalid,
                                                     StatusCodeDescription ?? "Session is invalid",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) CommunicationToEVSEFailed  (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'CommunicationToEVSEFailed' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            CommunicationToEVSEFailed(TRequest            Request,
                                      String              StatusCodeDescription      = null,
                                      String              StatusCodeAdditionalInfo   = null,
                                      Session_Id?         SessionId                  = null,
                                      PartnerSession_Id?  PartnerSessionId           = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.CommunicationToEVSEFailed,
                                                     StatusCodeDescription ?? "Communication to EVSE failed!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) EVSEAlreadyReserved        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyReserved' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            EVSEAlreadyReserved(TRequest            Request,
                                String              StatusCodeDescription     = null,
                                String              StatusCodeAdditionalInfo  = null,
                                Session_Id?         SessionId                 = null,
                                PartnerSession_Id?  PartnerSessionId          = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.EVSEAlreadyReserved,
                                                     StatusCodeDescription ?? "EVSE already reserved!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) EVSEAlreadyInUse_WrongToken(Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyInUse_WrongToken' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            EVSEAlreadyInUse_WrongToken(TRequest            Request,
                                        String              StatusCodeDescription     = null,
                                        String              StatusCodeAdditionalInfo  = null,
                                        Session_Id?         SessionId                 = null,
                                        PartnerSession_Id?  PartnerSessionId          = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.EVSEAlreadyInUse_WrongToken,
                                                     StatusCodeDescription ?? "EVSE is already in use!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) UnknownEVSEID              (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'UnknownEVSEID' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            UnknownEVSEID(TRequest            Request,
                          String              StatusCodeDescription     = null,
                          String              StatusCodeAdditionalInfo  = null,
                          Session_Id?         SessionId                 = null,
                          PartnerSession_Id?  PartnerSessionId          = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.UnknownEVSEID,
                                                     StatusCodeDescription ?? "Unknown EVSE ID!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) EVSEOutOfService           (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEOutOfService' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            EVSEOutOfService(TRequest            Request,
                             String              StatusCodeDescription     = null,
                             String              StatusCodeAdditionalInfo  = null,
                             Session_Id?         SessionId                 = null,
                             PartnerSession_Id?  PartnerSessionId          = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.EVSEOutOfService,
                                                     StatusCodeDescription ?? "EVSE out of service!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion

        #region (static) NoValidContract            (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'NoValidContract' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            NoValidContract(TRequest            Request,
                            String              StatusCodeDescription     = null,
                            String              StatusCodeAdditionalInfo  = null,
                            Session_Id?         SessionId                 = null,
                            PartnerSession_Id?  PartnerSessionId          = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.NoValidContract,
                                                     StatusCodeDescription ?? "No valid contract!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 PartnerSessionId);

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <CommonTypes:eRoamingAcknowledgement>
        //
        //          <CommonTypes:Result>?</CommonTypes:Result>
        //
        //          <CommonTypes:StatusCode>
        //
        //             <CommonTypes:Code>?</CommonTypes:Code>
        //
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        //
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //          </CommonTypes:StatusCode>
        //
        //          <!--Optional:-->
        //          <CommonTypes:SessionID>?</CommonTypes:SessionID>
        //
        //          <!--Optional:-->
        //          <CommonTypes:PartnerSessionID>?</CommonTypes:PartnerSessionID>
        //
        //       </CommonTypes:eRoamingAcknowledgement>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static)    Parse(Request, AcknowledgementXML, CustomAcknowledgementParser = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="XML">The XML to parse.</param>
        public static Acknowledgement<TRequest> Parse(TRequest                                            Request,
                                                      XElement                                            AcknowledgementXML,
                                                      CustomXMLParserDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementParser   = null,
                                                      CustomXMLParserDelegate<StatusCode>                 CustomStatusCodeParser        = null,
                                                      OnExceptionDelegate                                 OnException                   = null)
        {

            if (TryParse(Request,
                         AcknowledgementXML,
                         out Acknowledgement<TRequest> _Acknowledgement,
                         CustomAcknowledgementParser,
                         CustomStatusCodeParser,
                         OnException))

                return _Acknowledgement;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AcknowledgementXML, out Acknowledgement, CustomAcknowledgementParser = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom Acknowledgement XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        public static Boolean TryParse(TRequest                                            Request,
                                       XElement                                            AcknowledgementXML,
                                       out Acknowledgement<TRequest>                       Acknowledgement,
                                       CustomXMLParserDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementParser   = null,
                                       CustomXMLParserDelegate<StatusCode>                 CustomStatusCodeParser        = null,
                                       OnExceptionDelegate                                 OnException                   = null)
        {

            try
            {

                if (AcknowledgementXML.Name != OICPNS.CommonTypes + "eRoamingAcknowledgement")
                {
                    Acknowledgement = null;
                    return false;
                }

                Acknowledgement = new Acknowledgement<TRequest>(

                                      Request,

                                      AcknowledgementXML.ElementValueOrFail(OICPNS.CommonTypes + "Result") == "true",

                                      AcknowledgementXML.MapElementOrFail  (OICPNS.CommonTypes + "StatusCode",
                                                                            (xml, e) => StatusCode.Parse(xml,
                                                                                                         CustomStatusCodeParser,
                                                                                                         e),
                                                                            OnException),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "SessionID",
                                                                            Session_Id.Parse),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "PartnerSessionID",
                                                                            PartnerSession_Id.Parse)

                                  );


                if (CustomAcknowledgementParser != null)
                    Acknowledgement = CustomAcknowledgementParser(AcknowledgementXML, Acknowledgement);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AcknowledgementXML, e);

                Acknowledgement = null;
                return false;

            }

        }

        #endregion

        #region ToXML(CustomAcknowledgementSerializer = null, CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a XML-representation of this object.
        /// </summary>
        /// <param name="CustomAcknowledgementSerializer">A delegate to customize the serialization of Acknowledgement respones.</param>
        /// <param name="CustomOperatorEVSEStatusSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementSerializer   = null,
                              CustomXMLSerializerDelegate<StatusCode>                 CustomStatusCodeSerializer        = null)
        {

            var XML = new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                          new XElement(OICPNS.CommonTypes + "Result", Result),

                          StatusCode.ToXML(CustomStatusCodeSerializer: CustomStatusCodeSerializer),

                          SessionId != null
                              ? new XElement(OICPNS.CommonTypes + "SessionID",         SessionId.ToString())
                              : null,

                          PartnerSessionId != null
                              ? new XElement(OICPNS.CommonTypes + "PartnerSessionID",  PartnerSessionId.ToString())
                              : null

                      );

            return CustomAcknowledgementSerializer != null
                       ? CustomAcknowledgementSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + StatusCode.Code, " / ", StatusCode.Description, " / ", StatusCode.AdditionalInfo);

        #endregion


        public override bool Equals(Acknowledgement<TRequest> ARequest)
        {
            throw new NotImplementedException();
        }


        public class Builder : AResponseBuilder<TRequest,
                                                Acknowledgement<TRequest>>
        {

            #region Properties

            /// <summary>
            /// The result of the operation.
            /// </summary>
            public Boolean                     Result             { get; set; }

            /// <summary>
            /// The status code of the operation.
            /// </summary>
            public StatusCode                  StatusCode         { get; set; }

            /// <summary>
            /// An optional charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public Session_Id?                 SessionId          { get; set; }

            /// <summary>
            /// An optional partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public PartnerSession_Id?          PartnerSessionId   { get; set; }

            #endregion

            public Builder(Acknowledgement<TRequest>            Acknowledgement,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Acknowledgement?.Request,
                       Acknowledgement.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? Acknowledgement.CustomData.Concat(CustomData)
                                 : Acknowledgement.CustomData
                           : CustomData)

            {

                if (Acknowledgement != null)
                {

                    this.Result            = Acknowledgement.Result;
                    this.StatusCode        = Acknowledgement.StatusCode;
                    this.SessionId         = Acknowledgement.SessionId;
                    this.PartnerSessionId  = Acknowledgement.PartnerSessionId;

                }

            }


            public override bool Equals(Acknowledgement<TRequest> ARequest)
            {
                throw new NotImplementedException();
            }

            public override Acknowledgement<TRequest> ToImmutable

                => new Acknowledgement<TRequest>(Request,
                                                 Result,
                                                 StatusCode,
                                                 SessionId,
                                                 PartnerSessionId,
                                                 CustomData);

        }

    }

}
