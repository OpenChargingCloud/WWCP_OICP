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
        private Acknowledgement(TRequest                               Request,
                                Boolean                         Result,
                                StatusCode                      StatusCode,
                                Session_Id?                     SessionId         = null,
                                PartnerSession_Id?              PartnerSessionId  = null,
                                //IReadOnlyDictionary<String, Object> CustomData = null,
                                CustomMapper2Delegate<Builder>  CustomMapper      = null)

            : base(Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given request object must not be null!");

            #endregion

            this.Result            = Result;
            this.StatusCode        = StatusCode;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;

            if (CustomMapper != null)
            {

                var Builder = CustomMapper.Invoke(new Builder(this));

                this.Result            = Builder.Result;
                this.StatusCode        = Builder.StatusCode;
                this.SessionId         = Builder.SessionId;
                this.PartnerSessionId  = Builder.PartnerSessionId;
                this.CustomData        = Builder.CustomData;

            }

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
        public Acknowledgement(TRequest                        Request,
                               Session_Id                      SessionId,
                               PartnerSession_Id?              PartnerSessionId          = null,
                               String                          StatusCodeDescription     = null,
                               String                          StatusCodeAdditionalInfo  = null,
                               CustomMapper2Delegate<Builder>  CustomMapper              = null)

            : this(Request,
                   true,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId,
                   CustomMapper)

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
        public Acknowledgement(TRequest                        Request,
                               StatusCodes                     StatusCode,
                               String                          StatusCodeDescription     = null,
                               String                          StatusCodeAdditionalInfo  = null,
                               Session_Id?                     SessionId                 = null,
                               PartnerSession_Id?              PartnerSessionId          = null,
                               CustomMapper2Delegate<Builder>  CustomMapper              = null)

            : this(Request,
                   false,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   SessionId,
                   PartnerSessionId,
                   CustomMapper)

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

        #region (static)    Parse(Request, XML, CustomMapper = null, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="XML">The XML to parse.</param>
        public static Acknowledgement<TRequest> Parse(TRequest                                                  Request,
                                                      XElement                                                  XML,
                                                      CustomMapperDelegate<Acknowledgement<TRequest>, Builder>  CustomMapper  = null,
                                                      OnExceptionDelegate                                       OnException   = null)
        {

            Acknowledgement<TRequest> _Acknowledgement;

            if (TryParse(Request, XML, out _Acknowledgement, CustomMapper, OnException))
                return _Acknowledgement;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, XML, out Acknowledgement, CustomMapper = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP acknowledgement.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        /// <param name="Acknowledgement">The parsed acknowledgement</param>
        public static Boolean TryParse(TRequest                                                  Request,
                                       XElement                                                  XML,
                                       out Acknowledgement<TRequest>                             Acknowledgement,
                                       CustomMapperDelegate<Acknowledgement<TRequest>, Builder>  CustomMapper  = null,
                                       OnExceptionDelegate                                       OnException   = null)
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

                Acknowledgement = new Acknowledgement<TRequest>(

                                      Request,

                                      AcknowledgementXML.ElementValueOrFail(OICPNS.CommonTypes + "Result") == "true",

                                      AcknowledgementXML.MapElementOrFail  (OICPNS.CommonTypes + "StatusCode",
                                                                            StatusCode.Parse,
                                                                            OnException),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "SessionID",
                                                                            Session_Id.Parse),

                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "PartnerSessionID",
                                                                            PartnerSession_Id.Parse),

                                      responsebuilder => CustomMapper != null
                                                             ? CustomMapper(AcknowledgementXML, responsebuilder)
                                                             : responsebuilder

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


        public override bool Equals(Acknowledgement<TRequest> ARequest)
        {
            throw new NotImplementedException();
        }


        public class Builder : ABuilder
        {

            #region Properties

            public TRequest                    Request            { get; set; }

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

            public Dictionary<String, Object>  CustomData         { get; set; }

            #endregion

            public Builder(Acknowledgement<TRequest> Acknowledgement = null)
            {

                if (Acknowledgement != null)
                {

                    this.Request           = Acknowledgement.Request;
                    this.Result            = Acknowledgement.Result;
                    this.StatusCode        = Acknowledgement.StatusCode;
                    this.SessionId         = Acknowledgement.SessionId;
                    this.PartnerSessionId  = Acknowledgement.PartnerSessionId;
                    this.CustomData        = new Dictionary<String, Object>();

                    if (Acknowledgement.CustomData != null)
                        foreach (var item in Acknowledgement.CustomData)
                            CustomData.Add(item.Key, item.Value);

                }

            }


            //public Acknowledgement<T> ToImmutable()

            //    => new Acknowledgement<T>(Request,
            //                              Result,
            //                              StatusCode,
            //                              SessionId,
            //                              PartnerSessionId,
            //                              CustomData);

        }

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


        #region (static) Success(SessionId, ...)

        /// <summary>
        /// Create a new OICP 'positive' acknowledgement.
        /// </summary>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public static Acknowledgement Success(Session_Id?         SessionId                 = null,
                                              PartnerSession_Id?  PartnerSessionId          = null,
                                              String              StatusCodeDescription     = null,
                                              String              StatusCodeAdditionalInfo  = null)

            => new Acknowledgement(
                   true,
                   new StatusCode(
                       StatusCodes.Success,
                       StatusCodeDescription,
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion


        #region (static) DataError(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'DataError' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement DataError(String              StatusCodeDescription     = null,
                                                String              StatusCodeAdditionalInfo  = null,
                                                Session_Id?         SessionId                 = null,
                                                PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.DataError,
                       StatusCodeDescription ?? "Data Error!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) SystemError(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement SystemError(String              StatusCodeDescription     = null,
                                                  String              StatusCodeAdditionalInfo  = null,
                                                  Session_Id?         SessionId                 = null,
                                                  PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.SystemError,
                       StatusCodeDescription ?? "System Error!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) ServiceNotAvailable(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'ServiceNotAvailable' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement ServiceNotAvailable(String              StatusCodeDescription     = null,
                                                          String              StatusCodeAdditionalInfo  = null,
                                                          Session_Id?         SessionId                 = null,
                                                          PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.ServiceNotAvailable,
                       StatusCodeDescription ?? "Service not available!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) SessionIsInvalid(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'SessionIsInvalid' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement SessionIsInvalid(String              StatusCodeDescription     = null,
                                                       String              StatusCodeAdditionalInfo  = null,
                                                       Session_Id?         SessionId                 = null,
                                                       PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.SessionIsInvalid,
                       StatusCodeDescription ?? "Session is invalid",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) CommunicationToEVSEFailed(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'CommunicationToEVSEFailed' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement CommunicationToEVSEFailed(String              StatusCodeDescription     = null,
                                                                String              StatusCodeAdditionalInfo  = null,
                                                                Session_Id?         SessionId                 = null,
                                                                PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.CommunicationToEVSEFailed,
                       StatusCodeDescription ?? "Communication to EVSE failed!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) EVSEAlreadyReserved(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyReserved' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement EVSEAlreadyReserved(String              StatusCodeDescription     = null,
                                                          String              StatusCodeAdditionalInfo  = null,
                                                          Session_Id?         SessionId                 = null,
                                                          PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.EVSEAlreadyReserved,
                       StatusCodeDescription ?? "EVSE already reserved!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) EVSEAlreadyInUse_WrongToken(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyInUse_WrongToken' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement EVSEAlreadyInUse_WrongToken(String              StatusCodeDescription     = null,
                                                                  String              StatusCodeAdditionalInfo  = null,
                                                                  Session_Id?         SessionId                 = null,
                                                                  PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.EVSEAlreadyInUse_WrongToken,
                       StatusCodeDescription ?? "EVSE is already in use!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) UnknownEVSEID(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'UnknownEVSEID' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement UnknownEVSEID(String              StatusCodeDescription     = null,
                                                    String              StatusCodeAdditionalInfo  = null,
                                                    Session_Id?         SessionId                 = null,
                                                    PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.UnknownEVSEID,
                       StatusCodeDescription ?? "Unknown EVSE ID!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) EVSEOutOfService(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'EVSEOutOfService' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement EVSEOutOfService(String              StatusCodeDescription     = null,
                                                       String              StatusCodeAdditionalInfo  = null,
                                                       Session_Id?         SessionId                 = null,
                                                       PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.EVSEOutOfService,
                       StatusCodeDescription ?? "EVSE out of service!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion

        #region (static) NoValidContract(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'NoValidContract' acknowledgement.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        public static Acknowledgement NoValidContract(String              StatusCodeDescription     = null,
                                                      String              StatusCodeAdditionalInfo  = null,
                                                      Session_Id?         SessionId                 = null,
                                                      PartnerSession_Id?  PartnerSessionId          = null)

            => new Acknowledgement(
                   false,
                   new StatusCode(
                       StatusCodes.NoValidContract,
                       StatusCodeDescription ?? "No valid contract!",
                       StatusCodeAdditionalInfo
                   ),
                   SessionId,
                   PartnerSessionId
               );

        #endregion



        #region Documentation

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
        //       <!--Optional:-->
        //       <CommonTypes:SessionID>04cf39ad-0a88-1295-27dc-d593d1a076ac</CommonTypes:SessionID>
        //
        //       <!--Optional:-->
        //       <CommonTypes:PartnerSessionID>04cf39ad0a88129527dcd593d1a076ac</CommonTypes:PartnerSessionID>
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

                                                      AcknowledgementXML.MapElementOrFail  (OICPNS.CommonTypes + "StatusCode",
                                                                                            StatusCode.Parse),

                                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "SessionID",
                                                                                            Session_Id.Parse),

                                                      AcknowledgementXML.MapValueOrNullable(OICPNS.CommonTypes + "PartnerSessionID",
                                                                                            PartnerSession_Id.Parse)

                                                     );

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

                   SessionId.HasValue
                       ? new XElement(OICPNS.CommonTypes + "SessionID",         SessionId.ToString())
                       : null,

                   PartnerSessionId.HasValue
                       ? new XElement(OICPNS.CommonTypes + "PartnerSessionID",  PartnerSessionId.ToString().SubstringMax(50))
                       : null

             );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " +
                             StatusCode.Code,        " / ",
                             StatusCode.Description, " / ",
                             StatusCode.AdditionalInfo);

        #endregion

    }

}
