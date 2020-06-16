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
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// An OICP authorize remote stop request.
    /// </summary>
    public class AuthorizeRemoteStopRequest : ARequest<AuthorizeRemoteStopRequest>
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
        /// Create an OICP AuthorizeRemoteStopRequest XML/SOAP request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public AuthorizeRemoteStopRequest(Session_Id             SessionId,
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

        // <soapenv:Envelope xmlns:soapenv        = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization  = "http://www.hubject.com/b2b/services/authorization/v2.1">
        //                   xmlns:CommonTypes    = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingAuthorizeRemoteStop>
        //
        //          <Authorization:SessionID>?</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:CPOPartnerSessionID>?</Authorization:CPOPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:EMPPartnerSessionID>?</Authorization:EMPPartnerSessionID>
        //
        //          <Authorization:ProviderID>?</Authorization:ProviderID>
        //          <Authorization:EvseID>?</Authorization:EvseID>
        //
        //       </Authorization:eRoamingAuthorizeRemoteStop>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (AuthorizeRemoteStopRequestXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequestXML">The XML to parse.</param>
        /// <param name="CustomAuthorizeRemoteStopRequestParser">A delegate to parse custom AuthorizeRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteStopRequest

            Parse(XElement                                             AuthorizeRemoteStopRequestXML,
                  CustomXMLParserDelegate<AuthorizeRemoteStopRequest>  CustomAuthorizeRemoteStopRequestParser   = null,
                  OnExceptionDelegate                                  OnException                              = null,

                  DateTime?                                            Timestamp                                = null,
                  CancellationToken?                                   CancellationToken                        = null,
                  EventTracking_Id                                     EventTrackingId                          = null,
                  TimeSpan?                                            RequestTimeout                           = null)

        {

            if (TryParse(AuthorizeRemoteStopRequestXML,
                         out AuthorizeRemoteStopRequest _AuthorizeRemoteStopRequest,
                         CustomAuthorizeRemoteStopRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteStopRequest;

            return null;

        }

        #endregion

        #region (static) Parse   (AuthorizeRemoteStopRequestText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP authorize remote stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequestText">The text to parse.</param>
        /// <param name="CustomAuthorizeRemoteStopRequestParser">A delegate to parse custom AuthorizeRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteStopRequest

            Parse(String                                               AuthorizeRemoteStopRequestText,
                  CustomXMLParserDelegate<AuthorizeRemoteStopRequest>  CustomAuthorizeRemoteStopRequestParser   = null,
                  OnExceptionDelegate                                  OnException                              = null,

                  DateTime?                                            Timestamp                                = null,
                  CancellationToken?                                   CancellationToken                        = null,
                  EventTracking_Id                                     EventTrackingId                          = null,
                  TimeSpan?                                            RequestTimeout                           = null)

        {

            if (TryParse(AuthorizeRemoteStopRequestText,
                         out AuthorizeRemoteStopRequest _AuthorizeRemoteStopRequest,
                         CustomAuthorizeRemoteStopRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteStopRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteStopRequestXML,  out AuthorizeRemoteStopRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequestXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteStopRequest">The parsed authorize remote stop request.</param>
        /// <param name="CustomAuthorizeRemoteStopRequestParser">A delegate to parse custom AuthorizeRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                             AuthorizeRemoteStopRequestXML,
                                       out AuthorizeRemoteStopRequest                       AuthorizeRemoteStopRequest,
                                       CustomXMLParserDelegate<AuthorizeRemoteStopRequest>  CustomAuthorizeRemoteStopRequestParser   = null,
                                       OnExceptionDelegate                                  OnException                              = null,

                                       DateTime?                                            Timestamp                                = null,
                                       CancellationToken?                                   CancellationToken                        = null,
                                       EventTracking_Id                                     EventTrackingId                          = null,
                                       TimeSpan?                                            RequestTimeout                           = null)
        {

            try
            {

                if (AuthorizeRemoteStopRequestXML.Name != OICPNS.Authorization + "eRoamingAuthorizeRemoteStop")
                {
                    AuthorizeRemoteStopRequest = null;
                    return false;
                }

                AuthorizeRemoteStopRequest = new AuthorizeRemoteStopRequest(

                                                 AuthorizeRemoteStopRequestXML.MapValueOrFail    (OICPNS.Authorization + "SessionID",
                                                                                                  Session_Id.Parse),

                                                 AuthorizeRemoteStopRequestXML.MapValueOrFail    (OICPNS.Authorization + "ProviderID",
                                                                                                  Provider_Id.Parse),

                                                 AuthorizeRemoteStopRequestXML.MapValueOrFail    (OICPNS.Authorization + "EvseID",
                                                                                                  EVSE_Id.Parse),

                                                 AuthorizeRemoteStopRequestXML.MapValueOrNullable(OICPNS.Authorization + "CPOPartnerSessionID",
                                                                                                  CPOPartnerSession_Id.Parse),

                                                 AuthorizeRemoteStopRequestXML.MapValueOrNullable(OICPNS.Authorization + "EMPPartnerSessionID",
                                                                                                  EMPPartnerSession_Id.Parse),

                                                 Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout

                                             );


                if (CustomAuthorizeRemoteStopRequestParser != null)
                    AuthorizeRemoteStopRequest = CustomAuthorizeRemoteStopRequestParser(AuthorizeRemoteStopRequestXML,
                                                                                        AuthorizeRemoteStopRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteStopRequestXML, e);

                AuthorizeRemoteStopRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteStopRequestText, out AuthorizeRemoteStopRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize remote stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequestText">The text to parse.</param>
        /// <param name="AuthorizeRemoteStopRequest">The parsed authorize remote stop request.</param>
        /// <param name="CustomAuthorizeRemoteStopRequestParser">A delegate to parse custom AuthorizeRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                               AuthorizeRemoteStopRequestText,
                                       out AuthorizeRemoteStopRequest                       AuthorizeRemoteStopRequest,
                                       CustomXMLParserDelegate<AuthorizeRemoteStopRequest>  CustomAuthorizeRemoteStopRequestParser   = null,
                                       OnExceptionDelegate                                  OnException                              = null,

                                       DateTime?                                            Timestamp                                = null,
                                       CancellationToken?                                   CancellationToken                        = null,
                                       EventTracking_Id                                     EventTrackingId                          = null,
                                       TimeSpan?                                            RequestTimeout                           = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteStopRequestText).Root,
                             out AuthorizeRemoteStopRequest,
                             CustomAuthorizeRemoteStopRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteStopRequestText, e);
            }

            AuthorizeRemoteStopRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizeRemoteStopRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteStopRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteStop requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizeRemoteStopRequest>  CustomAuthorizeRemoteStopRequestSerializer   = null)
        {

            var XML= new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop",

                                      new XElement(OICPNS.Authorization + "SessionID",                  SessionId.          ToString()),

                                      CPOPartnerSessionId.HasValue
                                          ? new XElement(OICPNS.Authorization + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                                          : null,

                                      EMPPartnerSessionId.HasValue
                                          ? new XElement(OICPNS.Authorization + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                                          : null,

                                      new XElement(OICPNS.Authorization + "ProviderID",                 ProviderId.         ToString()),
                                      new XElement(OICPNS.Authorization + "EvseID",                     EVSEId.             ToString())

                                 );

            return CustomAuthorizeRemoteStopRequestSerializer != null
                       ? CustomAuthorizeRemoteStopRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest2)

        /// <summary>
        /// Compares two authorize remote stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequest1">An authorize remote stop request.</param>
        /// <param name="AuthorizeRemoteStopRequest2">Another authorize remote stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizeRemoteStopRequest1 == null) || ((Object) AuthorizeRemoteStopRequest2 == null))
                return false;

            return AuthorizeRemoteStopRequest1.Equals(AuthorizeRemoteStopRequest2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest2)

        /// <summary>
        /// Compares two authorize remote stop requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequest1">An authorize remote stop request.</param>
        /// <param name="AuthorizeRemoteStopRequest2">Another authorize remote stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest1, AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest2)
            => !(AuthorizeRemoteStopRequest1 == AuthorizeRemoteStopRequest2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteStopRequest> Members

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

            if (!(Object is AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest))
                return false;

            return Equals(AuthorizeRemoteStopRequest);

        }

        #endregion

        #region Equals(AuthorizeRemoteStopRequest)

        /// <summary>
        /// Compares two authorize remote stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStopRequest">An authorize remote stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteStopRequest AuthorizeRemoteStopRequest)
        {

            if (AuthorizeRemoteStopRequest is null)
                return false;

            return SessionId. Equals(AuthorizeRemoteStopRequest.SessionId)  &&
                   ProviderId.Equals(AuthorizeRemoteStopRequest.ProviderId) &&
                   EVSEId.    Equals(AuthorizeRemoteStopRequest.EVSEId)     &&

                   ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteStopRequest.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteStopRequest.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteStopRequest.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteStopRequest.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteStopRequest.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteStopRequest.EMPPartnerSessionId.Value)));

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

                return SessionId. GetHashCode() * 11 ^
                       ProviderId.GetHashCode() *  7 ^
                       EVSEId.    GetHashCode() *  5 ^

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
