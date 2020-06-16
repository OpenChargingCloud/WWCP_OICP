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

using org.GraphDefined.Vanaheimr.Illias;
using System.Threading;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// An OICP authorize remote reservation stop request.
    /// </summary>
    public class AuthorizeRemoteReservationStopRequest : ARequest<AuthorizeRemoteReservationStopRequest>
    {

        #region Properties

        /// <summary>
        /// A charging session identification.
        /// </summary>
        public Session_Id             SessionId              { get; }

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id            ProviderId             { get; }

        /// <summary>
        /// An EVSE identification.
        /// </summary>
        public EVSE_Id                EVSEId                 { get; }

        /// <summary>
        /// An optional CPO partner session identification.
        /// </summary>
        public CPOPartnerSession_Id?  CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional EMP partner session identification.
        /// </summary>
        public EMPPartnerSession_Id?  EMPPartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeRemoteReservationStop XML/SOAP request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public AuthorizeRemoteReservationStopRequest(Session_Id             SessionId,
                                                     Provider_Id            ProviderId,
                                                     EVSE_Id                EVSEId,
                                                     CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                     EMPPartnerSession_Id?  EMPPartnerSessionId   = null,

                                                     DateTime?              Timestamp             = null,
                                                     CancellationToken?     CancellationToken     = null,
                                                     EventTracking_Id       EventTrackingId       = null,
                                                     TimeSpan?              RequestTimeout        = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.SessionId            = SessionId;
            this.ProviderId           = ProviderId;
            this.EVSEId               = EVSEId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.1">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Reservation:eRoamingAuthorizeRemoteReservationStop>
        //
        //          <Reservation:SessionID>?</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Reservation:CPOPartnerSessionID>?</Authorization:CPOPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Reservation:EMPPartnerSessionID>?</Authorization:EMPPartnerSessionID>
        //
        //          <Reservation:ProviderID>?</Authorization:ProviderID>
        //          <Reservation:EvseID>?</Authorization:EvseID>
        //
        //       </Reservation:eRoamingAuthorizeRemoteReservationStop>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (AuthorizeRemoteReservationStopXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopXML">The XML to parse.</param>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestParser">A delegate to parse custom AuthorizeRemoteReservationStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteReservationStopRequest

            Parse(XElement                                                        AuthorizeRemoteReservationStopXML,
                  CustomXMLParserDelegate<AuthorizeRemoteReservationStopRequest>  CustomAuthorizeRemoteReservationStopRequestParser   = null,
                  OnExceptionDelegate                                             OnException                                         = null,

                  DateTime?                                                       Timestamp                                           = null,
                  CancellationToken?                                              CancellationToken                                   = null,
                  EventTracking_Id                                                EventTrackingId                                     = null,
                  TimeSpan?                                                       RequestTimeout                                      = null)

        {

            if (TryParse(AuthorizeRemoteReservationStopXML,
                         out AuthorizeRemoteReservationStopRequest _AuthorizeRemoteReservationStop,
                         CustomAuthorizeRemoteReservationStopRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteReservationStop;

            return null;

        }

        #endregion

        #region (static) Parse   (AuthorizeRemoteReservationStopText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopText">The text to parse.</param>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestParser">A delegate to parse custom AuthorizeRemoteReservationStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteReservationStopRequest

            Parse(String                                                          AuthorizeRemoteReservationStopText,
                  CustomXMLParserDelegate<AuthorizeRemoteReservationStopRequest>  CustomAuthorizeRemoteReservationStopRequestParser   = null,
                  OnExceptionDelegate                                             OnException                                         = null,

                  DateTime?                                                       Timestamp                                           = null,
                  CancellationToken?                                              CancellationToken                                   = null,
                  EventTracking_Id                                                EventTrackingId                                     = null,
                  TimeSpan?                                                       RequestTimeout                                      = null)

        {

            if (TryParse(AuthorizeRemoteReservationStopText,
                         out AuthorizeRemoteReservationStopRequest _AuthorizeRemoteReservationStop,
                         CustomAuthorizeRemoteReservationStopRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteReservationStop;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStopXML,  out AuthorizeRemoteReservationStop, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteReservationStop">The parsed authorize remote reservation stop request.</param>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestParser">A delegate to parse custom AuthorizeRemoteReservationStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                        AuthorizeRemoteReservationStopXML,
                                       out AuthorizeRemoteReservationStopRequest                       AuthorizeRemoteReservationStop,
                                       CustomXMLParserDelegate<AuthorizeRemoteReservationStopRequest>  CustomAuthorizeRemoteReservationStopRequestParser   = null,
                                       OnExceptionDelegate                                             OnException                                         = null,

                                       DateTime?                                                       Timestamp                                           = null,
                                       CancellationToken?                                              CancellationToken                                   = null,
                                       EventTracking_Id                                                EventTrackingId                                     = null,
                                       TimeSpan?                                                       RequestTimeout                                      = null)
        {

            try
            {

                if (AuthorizeRemoteReservationStopXML.Name != OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop")
                {
                    AuthorizeRemoteReservationStop = null;
                    return false;
                }

                AuthorizeRemoteReservationStop = new AuthorizeRemoteReservationStopRequest(

                                                     AuthorizeRemoteReservationStopXML.MapValueOrFail    (OICPNS.Reservation + "SessionID",
                                                                                                          Session_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrFail    (OICPNS.Reservation + "ProviderID",
                                                                                                          Provider_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                          EVSE_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrNullable(OICPNS.Reservation + "CPOPartnerSessionID",
                                                                                                          CPOPartnerSession_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrNullable(OICPNS.Reservation + "EMPPartnerSessionID",
                                                                                                          EMPPartnerSession_Id.Parse),

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout

                                                 );


                if (CustomAuthorizeRemoteReservationStopRequestParser != null)
                    AuthorizeRemoteReservationStop = CustomAuthorizeRemoteReservationStopRequestParser(AuthorizeRemoteReservationStopXML,
                                                                                                       AuthorizeRemoteReservationStop);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteReservationStopXML, e);

                AuthorizeRemoteReservationStop = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStopText, out AuthorizeRemoteReservationStop, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopText">The text to parse.</param>
        /// <param name="AuthorizeRemoteReservationStop">The parsed authorize remote reservation stop request.</param>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestParser">A delegate to parse custom AuthorizeRemoteReservationStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                          AuthorizeRemoteReservationStopText,
                                       out AuthorizeRemoteReservationStopRequest                       AuthorizeRemoteReservationStop,
                                       CustomXMLParserDelegate<AuthorizeRemoteReservationStopRequest>  CustomAuthorizeRemoteReservationStopRequestParser   = null,
                                       OnExceptionDelegate                                             OnException                                         = null,

                                       DateTime?                                                       Timestamp                                           = null,
                                       CancellationToken?                                              CancellationToken                                   = null,
                                       EventTracking_Id                                                EventTrackingId                                     = null,
                                       TimeSpan?                                                       RequestTimeout                                      = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteReservationStopText).Root,
                             out AuthorizeRemoteReservationStop,
                             CustomAuthorizeRemoteReservationStopRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteReservationStopText, e);
            }

            AuthorizeRemoteReservationStop = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizeRemoteReservationStopRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteReservationStopRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteReservationStop requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizeRemoteReservationStopRequest>  CustomAuthorizeRemoteReservationStopRequestSerializer   = null)
        {

            var XML = new XElement(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop",

                                       new XElement(OICPNS.Reservation + "SessionID",                  SessionId.          ToString()),

                                       CPOPartnerSessionId.HasValue
                                           ? new XElement(OICPNS.Reservation + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                                           : null,

                                       EMPPartnerSessionId.HasValue
                                           ? new XElement(OICPNS.Reservation + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                                           : null,

                                       new XElement(OICPNS.Reservation + "ProviderID",                 ProviderId.         ToString()),
                                       new XElement(OICPNS.Reservation + "EVSEID",                     EVSEId.             ToString())

                                  );

            return CustomAuthorizeRemoteReservationStopRequestSerializer != null
                       ? CustomAuthorizeRemoteReservationStopRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteReservationStop1, AuthorizeRemoteReservationStop2)

        /// <summary>
        /// Compares two authorize remote reservation stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStop1">An authorize remote reservation stop request.</param>
        /// <param name="AuthorizeRemoteReservationStop2">Another authorize remote reservation stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStop1, AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStop2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeRemoteReservationStop1, AuthorizeRemoteReservationStop2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizeRemoteReservationStop1 == null) || ((Object) AuthorizeRemoteReservationStop2 == null))
                return false;

            return AuthorizeRemoteReservationStop1.Equals(AuthorizeRemoteReservationStop2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteReservationStop1, AuthorizeRemoteReservationStop2)

        /// <summary>
        /// Compares two authorize remote reservation stop requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStop1">An authorize remote reservation stop request.</param>
        /// <param name="AuthorizeRemoteReservationStop2">Another authorize remote reservation stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStop1, AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStop2)
            => !(AuthorizeRemoteReservationStop1 == AuthorizeRemoteReservationStop2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteReservationStop> Members

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

            if (!(Object is AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStopRequest))
                return false;

            return Equals(AuthorizeRemoteReservationStopRequest);

        }

        #endregion

        #region Equals(AuthorizeRemoteReservationStop)

        /// <summary>
        /// Compares two authorize remote reservation stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStop">An authorize remote reservation stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteReservationStopRequest AuthorizeRemoteReservationStop)
        {

            if (AuthorizeRemoteReservationStop is null)
                return false;

            return SessionId. Equals(AuthorizeRemoteReservationStop.SessionId)  &&
                   ProviderId.Equals(AuthorizeRemoteReservationStop.ProviderId) &&
                   EVSEId.    Equals(AuthorizeRemoteReservationStop.EVSEId)     &&

                   ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteReservationStop.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteReservationStop.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteReservationStop.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteReservationStop.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteReservationStop.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteReservationStop.EMPPartnerSessionId.Value)));

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

                return SessionId. GetHashCode() * 9 ^
                       ProviderId.GetHashCode() * 7 ^
                       EVSEId.    GetHashCode() * 5 ^

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

            => String.Concat(SessionId,
                             " (", ProviderId, ") ",
                             " at ", EVSEId);

        #endregion


    }

}
