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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// An OICP acknowledgement.
    /// </summary>
    /// <typeparam name="TRequest">The type of the OICP request.</typeparam>
    public class Acknowledgement<TRequest> : AResponse<TRequest,
                                                       Acknowledgement<TRequest>>

        where TRequest : class, IRequest

    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public Boolean                Result                 { get; }

        /// <summary>
        /// The status code of the operation.
        /// </summary>
        public StatusCode             StatusCode             { get; }

        /// <summary>
        /// An optional charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public Session_Id?            SessionId              { get; }

        /// <summary>
        /// An optional EMP partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional CPO partner charging session identification for
        /// RemoteReservationStart and RemoteStart requests.
        /// </summary>
        public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; }

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
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        private Acknowledgement(TRequest                             Request,
                                Boolean                              Result,
                                StatusCode                           StatusCode,
                                Session_Id?                          SessionId             = null,
                                CPOPartnerSession_Id?                CPOPartnerSessionId   = null,
                                EMPPartnerSession_Id?                EMPPartnerSessionId   = null,
                                IReadOnlyDictionary<String, Object>  CustomData            = null)

            : base(Request,
                   CustomData)

        {

            this.Result               = Result;
            this.StatusCode           = StatusCode;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;

        }

        #endregion

        #region Acknowledgement(Request, SessionId = null, ...)

        /// <summary>
        /// Create a new OICP 'positive' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public Acknowledgement(TRequest                             Request,
                               Session_Id?                          SessionId                  = null,
                               CPOPartnerSession_Id?                CPOPartnerSessionId        = null,
                               EMPPartnerSession_Id?                EMPPartnerSessionId        = null,
                               String                               StatusCodeDescription      = null,
                               String                               StatusCodeAdditionalInfo   = null,
                               IReadOnlyDictionary<String, Object>  CustomData                 = null)

            : this(Request,
                   true,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   CPOPartnerSessionId,
                   EMPPartnerSessionId,
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
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="CustomData">Optional additional customer-specific data.</param>
        public Acknowledgement(TRequest                             Request,
                               StatusCodes                          StatusCode,
                               String                               StatusCodeDescription      = null,
                               String                               StatusCodeAdditionalInfo   = null,
                               Session_Id?                          SessionId                  = null,
                               CPOPartnerSession_Id?                CPOPartnerSessionId        = null,
                               EMPPartnerSession_Id?                EMPPartnerSessionId        = null,
                               IReadOnlyDictionary<String, Object>  CustomData                 = null)

            : this(Request,
                   false,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   CPOPartnerSessionId,
                   EMPPartnerSessionId,
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
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public static Acknowledgement<TRequest>

            Success(TRequest               Request,
                    Session_Id?            SessionId                  = null,
                    CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                    EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                    String                 StatusCodeDescription      = null,
                    String                 StatusCodeAdditionalInfo   = null)

                => new Acknowledgement<TRequest>(Request,
                                                 true,
                                                 new StatusCode(
                                                     StatusCodes.Success,
                                                     StatusCodeDescription,
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion


        #region (static) DataError                  (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'DataError' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            DataError(TRequest               Request,
                      String                 StatusCodeDescription      = null,
                      String                 StatusCodeAdditionalInfo   = null,
                      Session_Id?            SessionId                  = null,
                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.DataError,
                                                     StatusCodeDescription ?? "Data Error!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) SystemError                (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            SystemError(TRequest               Request,
                        String                 StatusCodeDescription      = null,
                        String                 StatusCodeAdditionalInfo   = null,
                        Session_Id?            SessionId                  = null,
                        CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                        EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     StatusCodeDescription ?? "System Error!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) ServiceNotAvailable        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'ServiceNotAvailable' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            ServiceNotAvailable(TRequest               Request,
                                String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.ServiceNotAvailable,
                                                     StatusCodeDescription ?? "Service not available!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) SessionIsInvalid           (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'SessionIsInvalid' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            SessionIsInvalid(TRequest               Request,
                             String                 StatusCodeDescription      = null,
                             String                 StatusCodeAdditionalInfo   = null,
                             Session_Id?            SessionId                  = null,
                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.SessionIsInvalid,
                                                     StatusCodeDescription ?? "Session is invalid!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) CommunicationToEVSEFailed  (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'CommunicationToEVSEFailed' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            CommunicationToEVSEFailed(TRequest               Request,
                                      String                 StatusCodeDescription      = null,
                                      String                 StatusCodeAdditionalInfo   = null,
                                      Session_Id?            SessionId                  = null,
                                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.CommunicationToEVSEFailed,
                                                     StatusCodeDescription ?? "Communication to EVSE failed!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) EVSEAlreadyReserved        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyReserved' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            EVSEAlreadyReserved(TRequest               Request,
                                String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.EVSEAlreadyReserved,
                                                     StatusCodeDescription ?? "EVSE already reserved!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) EVSEAlreadyInUse_WrongToken(Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyInUse_WrongToken' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            EVSEAlreadyInUse_WrongToken(TRequest               Request,
                                        String                 StatusCodeDescription      = null,
                                        String                 StatusCodeAdditionalInfo   = null,
                                        Session_Id?            SessionId                  = null,
                                        CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                        EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.EVSEAlreadyInUse_WrongToken,
                                                     StatusCodeDescription ?? "EVSE is already in use!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) UnknownEVSEID              (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'UnknownEVSEID' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            UnknownEVSEID(TRequest               Request,
                          String                 StatusCodeDescription      = null,
                          String                 StatusCodeAdditionalInfo   = null,
                          Session_Id?            SessionId                  = null,
                          CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                          EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.UnknownEVSEID,
                                                     StatusCodeDescription ?? "Unknown EVSE identification!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) EVSEOutOfService           (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEOutOfService' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            EVSEOutOfService(TRequest               Request,
                             String                 StatusCodeDescription      = null,
                             String                 StatusCodeAdditionalInfo   = null,
                             Session_Id?            SessionId                  = null,
                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.EVSEOutOfService,
                                                     StatusCodeDescription ?? "EVSE out of service!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) NoValidContract            (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'NoValidContract' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            NoValidContract(TRequest               Request,
                            String                 StatusCodeDescription      = null,
                            String                 StatusCodeAdditionalInfo   = null,
                            Session_Id?            SessionId                  = null,
                            CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                            EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.NoValidContract,
                                                     StatusCodeDescription ?? "No valid contract!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

        #endregion

        #region (static) NoEVConnectedToEVSE        (Request, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'NoEVConnectedToEVSE' acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        public static Acknowledgement<TRequest>

            NoEVConnectedToEVSE(TRequest               Request,
                                String                 StatusCodeDescription      = null,
                                String                 StatusCodeAdditionalInfo   = null,
                                Session_Id?            SessionId                  = null,
                                CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId        = null)

                => new Acknowledgement<TRequest>(Request,
                                                 false,
                                                 new StatusCode(
                                                     StatusCodes.NoEVConnectedToEVSE,
                                                     StatusCodeDescription ?? "No electric vehicle connected to EVSE!",
                                                     StatusCodeAdditionalInfo
                                                 ),
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId);

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
        //          <CommonTypes:CPOPartnerSessionID>?</Authorization:CPOPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <CommonTypes:EMPPartnerSessionID>?</Authorization:EMPPartnerSessionID>
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
        /// <param name="AcknowledgementXML">The XML to parse.</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom Acknowledgement XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">A delegate whenever exceptions occur.</param>
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
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="AcknowledgementXML">The XML to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        /// <param name="CustomAcknowledgementParser">A delegate to parse custom Acknowledgement XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">A delegate whenever exceptions occur.</param>
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

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "CPOPartnerSessionID",
                                                                            CPOPartnerSession_Id.Parse),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "EMPPartnerSessionID",
                                                                            EMPPartnerSession_Id.Parse)

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
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<Acknowledgement<TRequest>>  CustomAcknowledgementSerializer   = null,
                              CustomXMLSerializerDelegate<StatusCode>                 CustomStatusCodeSerializer        = null)
        {

            var XML = new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                          new XElement(OICPNS.CommonTypes + "Result", Result),

                          StatusCode.ToXML(CustomStatusCodeSerializer: CustomStatusCodeSerializer),

                          SessionId != null
                              ? new XElement(OICPNS.CommonTypes + "SessionID",            SessionId.          ToString())
                              : null,

                          CPOPartnerSessionId != null
                              ? new XElement(OICPNS.CommonTypes + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                              : null,

                          EMPPartnerSessionId != null
                              ? new XElement(OICPNS.CommonTypes + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                              : null

                      );

            return CustomAcknowledgementSerializer != null
                       ? CustomAcknowledgementSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (Acknowledgement1, Acknowledgement2)

        /// <summary>
        /// Compares two acknowledgements for equality.
        /// </summary>
        /// <param name="Acknowledgement1">An acknowledgement.</param>
        /// <param name="Acknowledgement2">Another acknowledgement.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Acknowledgement<TRequest> Acknowledgement1, Acknowledgement<TRequest> Acknowledgement2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Acknowledgement1, Acknowledgement2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Acknowledgement1 == null) || ((Object) Acknowledgement2 == null))
                return false;

            return Acknowledgement1.Equals(Acknowledgement2);

        }

        #endregion

        #region Operator != (Acknowledgement1, Acknowledgement2)

        /// <summary>
        /// Compares two acknowledgements for inequality.
        /// </summary>
        /// <param name="Acknowledgement1">An acknowledgement.</param>
        /// <param name="Acknowledgement2">Another acknowledgement.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Acknowledgement<TRequest> Acknowledgement1, Acknowledgement<TRequest> Acknowledgement2)
            => !(Acknowledgement1 == Acknowledgement2);

        #endregion

        #endregion

        #region IEquatable<Acknowledgement> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is Acknowledgement<TRequest> Acknowledgement))
                return false;

            return Equals(Acknowledgement);

        }

        #endregion

        #region Equals(Acknowledgement)

        /// <summary>
        /// Compares two acknowledgements for equality.
        /// </summary>
        /// <param name="Acknowledgement">An acknowledgement to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Acknowledgement<TRequest> Acknowledgement)
        {

            if (Acknowledgement is null)
                return false;

            return Result.    Equals(Acknowledgement.Result)     &&
                   StatusCode.Equals(Acknowledgement.StatusCode) &&

                   ((!SessionId.          HasValue && !Acknowledgement.SessionId.          HasValue) ||
                     (SessionId.          HasValue &&  Acknowledgement.SessionId.          HasValue && SessionId.          Value.Equals(Acknowledgement.SessionId.          Value))) &&

                   ((!CPOPartnerSessionId.HasValue && !Acknowledgement.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  Acknowledgement.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(Acknowledgement.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !Acknowledgement.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  Acknowledgement.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(Acknowledgement.EMPPartnerSessionId.Value)));

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

                return Result.    GetHashCode() * 11 ^
                       StatusCode.GetHashCode() *  7 ^

                       (SessionId.          HasValue
                            ? SessionId.          GetHashCode() * 5
                            : 0) ^

                       (CPOPartnerSessionId.HasValue
                            ? CPOPartnerSessionId.GetHashCode() * 3
                            : 0) ^

                       (EMPPartnerSessionId.HasValue
                            ? EMPPartnerSessionId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + StatusCode.Code, " / ", StatusCode.Description, " / ", StatusCode.AdditionalInfo);

        #endregion



        #region ToBuilder

        /// <summary>
        /// Return an acknowledgement builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An acknowledgement builder.
        /// </summary>
        public class Builder : AResponseBuilder<TRequest,
                                                Acknowledgement<TRequest>>
        {

            #region Properties

            /// <summary>
            /// The result of the operation.
            /// </summary>
            public Boolean                     Result                 { get; set; }

            /// <summary>
            /// The status code of the operation.
            /// </summary>
            public StatusCode                  StatusCode             { get; set; }

            /// <summary>
            /// An optional charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public Session_Id?                 SessionId              { get; set; }

            /// <summary>
            /// An optional EMP partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public CPOPartnerSession_Id?       CPOPartnerSessionId    { get; set; }

            /// <summary>
            /// An optional CPO partner charging session identification for
            /// RemoteReservationStart and RemoteStart requests.
            /// </summary>
            public EMPPartnerSession_Id?       EMPPartnerSessionId    { get; set; }

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

                    this.Result               = Acknowledgement.Result;
                    this.StatusCode           = Acknowledgement.StatusCode;
                    this.SessionId            = Acknowledgement.SessionId;
                    this.CPOPartnerSessionId  = Acknowledgement.CPOPartnerSessionId;
                    this.EMPPartnerSessionId  = Acknowledgement.EMPPartnerSessionId;

                }

            }


            #region Operator overloading

            #region Operator == (Acknowledgement1, Acknowledgement2)

            /// <summary>
            /// Compares two acknowledgements for equality.
            /// </summary>
            /// <param name="Acknowledgement1">An acknowledgement.</param>
            /// <param name="Acknowledgement2">Another acknowledgement.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public static Boolean operator == (Builder Acknowledgement1, Builder Acknowledgement2)
            {

                // If both are null, or both are same instance, return true.
                if (Object.ReferenceEquals(Acknowledgement1, Acknowledgement2))
                    return true;

                // If one is null, but not both, return false.
                if (((Object) Acknowledgement1 == null) || ((Object) Acknowledgement2 == null))
                    return false;

                return Acknowledgement1.Equals(Acknowledgement2);

            }

            #endregion

            #region Operator != (Acknowledgement1, Acknowledgement2)

            /// <summary>
            /// Compares two acknowledgements for inequality.
            /// </summary>
            /// <param name="Acknowledgement1">An acknowledgement.</param>
            /// <param name="Acknowledgement2">Another acknowledgement.</param>
            /// <returns>False if both match; True otherwise.</returns>
            public static Boolean operator != (Builder Acknowledgement1, Builder Acknowledgement2)
                => !(Acknowledgement1 == Acknowledgement2);

            #endregion

            #endregion

            #region IEquatable<Acknowledgement> Members

            #region Equals(Object)

            /// <summary>
            /// Compares two instances of this object.
            /// </summary>
            /// <param name="Object">An object to compare with.</param>
            /// <returns>true|false</returns>
            public override Boolean Equals(Object Object)
            {

                if (Object == null)
                    return false;

                if (!(Object is Acknowledgement<TRequest> Acknowledgement))
                    return false;

                return Equals(Acknowledgement);

            }

            #endregion

            #region Equals(Acknowledgement)

            /// <summary>
            /// Compares two acknowledgements for equality.
            /// </summary>
            /// <param name="Acknowledgement">An acknowledgement to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(Acknowledgement<TRequest> Acknowledgement)
            {

                if (Acknowledgement is null)
                    return false;

                return Result.    Equals(Acknowledgement.Result)     &&
                       StatusCode.Equals(Acknowledgement.StatusCode) &&

                       ((!SessionId.          HasValue && !Acknowledgement.SessionId.          HasValue) ||
                         (SessionId.          HasValue &&  Acknowledgement.SessionId.          HasValue && SessionId.          Value.Equals(Acknowledgement.SessionId.          Value))) &&

                       ((!CPOPartnerSessionId.HasValue && !Acknowledgement.CPOPartnerSessionId.HasValue) ||
                         (CPOPartnerSessionId.HasValue &&  Acknowledgement.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(Acknowledgement.CPOPartnerSessionId.Value))) &&

                       ((!EMPPartnerSessionId.HasValue && !Acknowledgement.EMPPartnerSessionId.HasValue) ||
                         (EMPPartnerSessionId.HasValue &&  Acknowledgement.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(Acknowledgement.EMPPartnerSessionId.Value)));

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

                    return Result.    GetHashCode() * 11 ^
                           StatusCode.GetHashCode() *  7 ^

                           (SessionId.          HasValue
                                ? SessionId.          GetHashCode() * 5
                                : 0) ^

                           (CPOPartnerSessionId.HasValue
                                ? CPOPartnerSessionId.GetHashCode() * 3
                                : 0) ^

                           (EMPPartnerSessionId.HasValue
                                ? EMPPartnerSessionId.GetHashCode()
                                : 0);

                }
            }

            #endregion

            public override Acknowledgement<TRequest> ToImmutable

                => new Acknowledgement<TRequest>(Request,
                                                 Result,
                                                 StatusCode,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 EMPPartnerSessionId,
                                                 CustomData);

        }

        #endregion

    }

}
