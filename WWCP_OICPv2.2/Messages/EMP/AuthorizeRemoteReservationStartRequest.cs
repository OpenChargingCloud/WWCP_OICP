﻿/*
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
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// An OICP authorize remote reservation start request.
    /// </summary>
    public class AuthorizeRemoteReservationStartRequest : ARequest<AuthorizeRemoteReservationStartRequest>
    {

        #region Properties

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id            ProviderId             { get; }

        /// <summary>
        /// An EVSE identification.
        /// </summary>
        public EVSE_Id                EVSEId                 { get; }

        /// <summary>
        /// An identification, e.g. an electric vehicle contract identification.
        /// </summary>
        public Identification         Identification         { get; }

        /// <summary>
        /// An optional charging session identification.
        /// </summary>
        public Session_Id?            SessionId              { get; }

        /// <summary>
        /// An optional CPO partner session identification.
        /// </summary>
        public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional EMP partner session identification.
        /// </summary>
        public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; }

        /// <summary>
        /// An optional partner product identification.
        /// </summary>
        public PartnerProduct_Id?     PartnerProductId       { get; }

        /// <summary>
        /// The optional duration of reservation.
        /// </summary>
        public TimeSpan?              Duration               { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeRemoteReservationStart XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="Identification">An identification, e.g. an electric vehicle contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public AuthorizeRemoteReservationStartRequest(Provider_Id            ProviderId,
                                                      EVSE_Id                EVSEId,
                                                      Identification         Identification,
                                                      Session_Id?            SessionId             = null,
                                                      CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                      EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                      PartnerProduct_Id?     PartnerProductId      = null,
                                                      TimeSpan?              Duration              = null,

                                                      DateTime?              Timestamp             = null,
                                                      CancellationToken?     CancellationToken     = null,
                                                      EventTracking_Id       EventTrackingId       = null,
                                                      TimeSpan?              RequestTimeout        = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId           = ProviderId;
            this.EVSEId               = EVSEId;
            this.Identification       = Identification;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.PartnerProductId     = PartnerProductId;
            this.Duration             = Duration;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.1"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Reservation:eRoamingAuthorizeRemoteReservationStart>
        //
        //          <!--Optional:-->
        //          <Reservation:SessionID>?</Reservation:SessionID>
        //
        //          <!--Optional:-->
        //          <Reservation:CPOPartnerSessionID>?</Reservation:CPOPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Reservation:EMPPartnerSessionID>?</Reservation:EMPPartnerSessionID>
        //
        //          <Reservation:ProviderID>?</Reservation:ProviderID>
        //          <Reservation:EvseID>?</Reservation:EvseID>
        //
        //       <Authorization:Identification>
        //         <!--You have a CHOICE of the next 5 items at this level-->
        //
        //         <CommonTypes:RFIDMifareFamilyIdentification>
        //            <CommonTypes:UID>?</CommonTypes:UID>
        //         </CommonTypes:RFIDMifareFamilyIdentification>
        //
        //
        //         <CommonTypes:RFIDIdentification>
        //
        //            <CommonTypes:UID>?</CommonTypes:UID>
        //
        //            <!--Optional:-->
        //            <CommonTypes:EvcoID>?</CommonTypes:EvcoID>
        //
        //            <CommonTypes:RFIDType>?</CommonTypes:RFIDType>
        //
        //            <!--Optional:-->
        //            <CommonTypes:PrintedNumber>?</CommonTypes:PrintedNumber>
        //
        //            <!--Optional:-->
        //            <CommonTypes:ExpiryDate>?</CommonTypes:ExpiryDate>
        //
        //         </CommonTypes:RFIDIdentification>
        //
        //
        //         <CommonTypes:QRCodeIdentification>
        //
        //            <CommonTypes:EvcoID>?</CommonTypes:EvcoID>
        //
        //            <!--You have a CHOICE of the next 2 items at this level-->
        //            <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //            <CommonTypes:HashedPIN>
        //               <CommonTypes:Value>?</CommonTypes:Value>
        //               <CommonTypes:Function>?</CommonTypes:Function>
        //            </CommonTypes:HashedPIN>
        //
        //         </CommonTypes:QRCodeIdentification>
        //
        //
        //         <CommonTypes:PlugAndChargeIdentification>
        //            <CommonTypes:EvcoID>DE-MEG-C10145984-1</CommonTypes:EvcoID>
        //         </CommonTypes:PlugAndChargeIdentification>
        //
        //
        //         <CommonTypes:RemoteIdentification>
        //           <CommonTypes:EvcoID>DE-MEG-C10145984-1</CommonTypes:EvcoID>
        //         </CommonTypes:RemoteIdentification>
        //
        //       </Authorization:Identification>
        //
        //          <!--Optional:-->
        //          <Reservation:PartnerProductID>?</Reservation:PartnerProductID>
        //
        //          <!--Optional:-->
        //          <Reservation:Duration>?</Reservation:Duration>
        //
        //       </Reservation:eRoamingAuthorizeRemoteReservationStart>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (AuthorizeRemoteReservationStartXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartXML">The XML to parse.</param>
        /// <param name="CustomAuthorizeRemoteReservationStartRequestParser">A delegate to parse custom AuthorizeRemoteReservationStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteReservationStartRequest

            Parse(XElement                                                         AuthorizeRemoteReservationStartXML,
                  CustomXMLParserDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestParser   = null,
                  CustomXMLParserDelegate<Identification>                          CustomIdentificationParser                           = null,
                  CustomXMLParserDelegate<RFIDIdentification>                      CustomRFIDIdentificationParser                       = null,
                  OnExceptionDelegate                                              OnException                                          = null,

                  DateTime?                                                        Timestamp                                            = null,
                  CancellationToken?                                               CancellationToken                                    = null,
                  EventTracking_Id                                                 EventTrackingId                                      = null,
                  TimeSpan?                                                        RequestTimeout                                       = null)

        {

            if (TryParse(AuthorizeRemoteReservationStartXML,
                         out AuthorizeRemoteReservationStartRequest _AuthorizeRemoteReservationStart,
                         CustomAuthorizeRemoteReservationStartRequestParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteReservationStart;

            return null;

        }

        #endregion

        #region (static) Parse   (AuthorizeRemoteReservationStartText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text-representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartText">The text to parse.</param>
        /// <param name="CustomAuthorizeRemoteReservationStartRequestParser">A delegate to parse custom AuthorizeRemoteReservationStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteReservationStartRequest

            Parse(String                                                           AuthorizeRemoteReservationStartText,
                  CustomXMLParserDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestParser   = null,
                  CustomXMLParserDelegate<Identification>                          CustomIdentificationParser                           = null,
                  CustomXMLParserDelegate<RFIDIdentification>                      CustomRFIDIdentificationParser                       = null,
                  OnExceptionDelegate                                              OnException                                          = null,

                  DateTime?                                                        Timestamp                                            = null,
                  CancellationToken?                                               CancellationToken                                    = null,
                  EventTracking_Id                                                 EventTrackingId                                      = null,
                  TimeSpan?                                                        RequestTimeout                                       = null)

        {

            if (TryParse(AuthorizeRemoteReservationStartText,
                         out AuthorizeRemoteReservationStartRequest _AuthorizeRemoteReservationStart,
                         CustomAuthorizeRemoteReservationStartRequestParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteReservationStart;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStartXML,  out AuthorizeRemoteReservationStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteReservationStart">The parsed authorize remote reservation start request.</param>
        /// <param name="CustomAuthorizeRemoteReservationStartRequestParser">A delegate to parse custom AuthorizeRemoteReservationStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                         AuthorizeRemoteReservationStartXML,
                                       out AuthorizeRemoteReservationStartRequest                       AuthorizeRemoteReservationStart,
                                       CustomXMLParserDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>                          CustomIdentificationParser                           = null,
                                       CustomXMLParserDelegate<RFIDIdentification>                      CustomRFIDIdentificationParser                       = null,
                                       OnExceptionDelegate                                              OnException                                          = null,

                                       DateTime?                                                        Timestamp                                            = null,
                                       CancellationToken?                                               CancellationToken                                    = null,
                                       EventTracking_Id                                                 EventTrackingId                                      = null,
                                       TimeSpan?                                                        RequestTimeout                                       = null)
        {

            try
            {

                if (AuthorizeRemoteReservationStartXML.Name != OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart")
                {
                    AuthorizeRemoteReservationStart = null;
                    return false;
                }

                AuthorizeRemoteReservationStart = new AuthorizeRemoteReservationStartRequest(

                                                      AuthorizeRemoteReservationStartXML.MapValueOrFail    (OICPNS.Reservation + "ProviderID",
                                                                                                            Provider_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                            EVSE_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapElementOrFail  (OICPNS.Reservation + "Identification",
                                                                                                            (xml, e) => Identification.Parse(xml,
                                                                                                                                             CustomIdentificationParser,
                                                                                                                                             CustomRFIDIdentificationParser,
                                                                                                                                             e),
                                                                                                            OnException),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "SessionID",
                                                                                                            Session_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "CPOPartnerSessionID",
                                                                                                            CPOPartnerSession_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "EMPPartnerSessionID",
                                                                                                            EMPPartnerSession_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "PartnerProductID",
                                                                                                            PartnerProduct_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "Duration",
                                                                                                            v => TimeSpan.FromMinutes(Double.Parse(v))),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout

                                                  );


                if (CustomAuthorizeRemoteReservationStartRequestParser != null)
                    AuthorizeRemoteReservationStart = CustomAuthorizeRemoteReservationStartRequestParser(AuthorizeRemoteReservationStartXML,
                                                                                                         AuthorizeRemoteReservationStart);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteReservationStartXML, e);

                AuthorizeRemoteReservationStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStartText, out AuthorizeRemoteReservationStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text-representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartText">The text to parse.</param>
        /// <param name="AuthorizeRemoteReservationStart">The parsed authorize remote reservation start request.</param>
        /// <param name="CustomAuthorizeRemoteReservationStartRequestParser">A delegate to parse custom AuthorizeRemoteReservationStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                           AuthorizeRemoteReservationStartText,
                                       out AuthorizeRemoteReservationStartRequest                       AuthorizeRemoteReservationStart,
                                       CustomXMLParserDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>                          CustomIdentificationParser                           = null,
                                       CustomXMLParserDelegate<RFIDIdentification>                      CustomRFIDIdentificationParser                       = null,
                                       OnExceptionDelegate                                              OnException                                          = null,

                                       DateTime?                                                        Timestamp                                            = null,
                                       CancellationToken?                                               CancellationToken                                    = null,
                                       EventTracking_Id                                                 EventTrackingId                                      = null,
                                       TimeSpan?                                                        RequestTimeout                                       = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteReservationStartText).Root,
                             out AuthorizeRemoteReservationStart,
                             CustomAuthorizeRemoteReservationStartRequestParser,
                             CustomIdentificationParser,
                             CustomRFIDIdentificationParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteReservationStartText, e);
            }

            AuthorizeRemoteReservationStart = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizeRemoteReservationStartRequestSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteReservationStartRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteReservationStart requests.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestSerializer   = null,
                              CustomXMLSerializerDelegate<Identification>                          CustomIdentificationSerializer                           = null)

        {

            var XML = new XElement(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart",

                                       SessionId.HasValue
                                           ? new XElement(OICPNS.Reservation + "SessionID",            SessionId.          ToString())
                                           : null,

                                       CPOPartnerSessionId.HasValue
                                           ? new XElement(OICPNS.Reservation + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                                           : null,

                                       EMPPartnerSessionId.HasValue
                                           ? new XElement(OICPNS.Reservation + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                                           : null,

                                       new XElement(OICPNS.Reservation + "ProviderID",                 ProviderId.         ToString()),
                                       new XElement(OICPNS.Reservation + "EVSEID",                     EVSEId.             ToString()),

                                       Identification.ToXML(OICPNS.Reservation + "Identification",
                                                            CustomIdentificationSerializer),

                                       PartnerProductId.HasValue
                                           ? new XElement(OICPNS.Reservation + "PartnerProductID",     PartnerProductId.   ToString())
                                           : null,

                                       Duration.HasValue
                                           ? new XElement(OICPNS.Reservation + "Duration",             Convert.ToInt32(Math.Round(Duration.Value.TotalMinutes, 0)))
                                           : null

                                   );

            return CustomAuthorizeRemoteReservationStartRequestSerializer != null
                       ? CustomAuthorizeRemoteReservationStartRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteReservationStart1, AuthorizeRemoteReservationStart2)

        /// <summary>
        /// Compares two authorize remote reservation start requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStart1">An authorize remote reservation start request.</param>
        /// <param name="AuthorizeRemoteReservationStart2">Another authorize remote reservation start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteReservationStartRequest AuthorizeRemoteReservationStart1, AuthorizeRemoteReservationStartRequest AuthorizeRemoteReservationStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeRemoteReservationStart1, AuthorizeRemoteReservationStart2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizeRemoteReservationStart1 == null) || ((Object) AuthorizeRemoteReservationStart2 == null))
                return false;

            return AuthorizeRemoteReservationStart1.Equals(AuthorizeRemoteReservationStart2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteReservationStart1, AuthorizeRemoteReservationStart2)

        /// <summary>
        /// Compares two authorize remote reservation start requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStart1">An authorize remote reservation start request.</param>
        /// <param name="AuthorizeRemoteReservationStart2">Another authorize remote reservation start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteReservationStartRequest AuthorizeRemoteReservationStart1, AuthorizeRemoteReservationStartRequest AuthorizeRemoteReservationStart2)
            => !(AuthorizeRemoteReservationStart1 == AuthorizeRemoteReservationStart2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteReservationStart> Members

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

            if (!(Object is AuthorizeRemoteReservationStartRequest AuthorizeRemoteReservationStartRequest))
                return false;

            return Equals(AuthorizeRemoteReservationStartRequest);

        }

        #endregion

        #region Equals(AuthorizeRemoteReservationStart)

        /// <summary>
        /// Compares two authorize remote reservation start requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStart">An authorize remote reservation start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteReservationStartRequest AuthorizeRemoteReservationStart)
        {

            if (AuthorizeRemoteReservationStart is null)
                return false;

            return ProviderId.    Equals(AuthorizeRemoteReservationStart.ProviderId)     &&
                   EVSEId.        Equals(AuthorizeRemoteReservationStart.EVSEId)         &&
                   Identification.Equals(AuthorizeRemoteReservationStart.Identification) &&

                   ((!SessionId.          HasValue && !AuthorizeRemoteReservationStart.SessionId.          HasValue) ||
                     (SessionId.          HasValue &&  AuthorizeRemoteReservationStart.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizeRemoteReservationStart.SessionId.          Value))) &&

                   ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteReservationStart.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteReservationStart.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteReservationStart.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteReservationStart.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteReservationStart.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteReservationStart.EMPPartnerSessionId.Value))) &&

                   ((!PartnerProductId.   HasValue && !AuthorizeRemoteReservationStart.PartnerProductId.   HasValue) ||
                     (PartnerProductId.   HasValue &&  AuthorizeRemoteReservationStart.PartnerProductId.   HasValue && PartnerProductId.   Value.Equals(AuthorizeRemoteReservationStart.PartnerProductId.   Value))) &&

                   ((!Duration.           HasValue && !AuthorizeRemoteReservationStart.Duration.           HasValue) ||
                     (Duration.           HasValue &&  AuthorizeRemoteReservationStart.Duration.           HasValue && Duration.           Value.Equals(AuthorizeRemoteReservationStart.Duration.           Value)));

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

                return ProviderId.    GetHashCode() * 17 ^
                       EVSEId.        GetHashCode() * 13 ^
                       Identification.GetHashCode() * 11 ^

                       (SessionId.        HasValue
                            ? SessionId.          GetHashCode() * 9
                            : 0) ^

                       (CPOPartnerSessionId.HasValue
                            ? CPOPartnerSessionId.GetHashCode() * 7
                            : 0) ^

                       (EMPPartnerSessionId.HasValue
                            ? EMPPartnerSessionId.GetHashCode() * 5
                            : 0) ^

                       (PartnerProductId.HasValue
                            ? PartnerProductId.   GetHashCode() * 3
                            : 0) ^

                       (Duration.HasValue
                            ? Duration.           GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId,
                             " for user '", Identification, "' ",
                             Duration.HasValue ? " for " + Duration.Value.TotalMinutes + " minutes " : "",
                             " (", ProviderId, ")");

        #endregion


    }

}
