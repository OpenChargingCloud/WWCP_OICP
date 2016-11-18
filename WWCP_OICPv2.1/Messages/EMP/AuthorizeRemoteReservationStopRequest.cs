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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP authorize remote reservation stop request.
    /// </summary>
    public class AuthorizeRemoteReservationStopRequest : ARequest<AuthorizeRemoteReservationStopRequest>
    {

        #region Properties


        public Session_Id          SessionId          { get; }

        public Provider_Id         ProviderId         { get; }

        public EVSE_Id             EVSEId             { get; }

        public PartnerSession_Id?  PartnerSessionId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeRemoteReservationStop XML/SOAP request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public AuthorizeRemoteReservationStopRequest(Session_Id          SessionId,
                                                     Provider_Id         ProviderId,
                                                     EVSE_Id             EVSEId,
                                                     PartnerSession_Id?  PartnerSessionId  = null)
        {

            this.SessionId         = SessionId;
            this.ProviderId        = ProviderId;
            this.EVSEId            = EVSEId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Reservation:eRoamingAuthorizeRemoteReservationStop>
        //
        //          <Reservation:SessionID>?</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Reservation:PartnerSessionID>?</Authorization:PartnerSessionID>
        //
        //          <Reservation:ProviderID>?</Authorization:ProviderID>
        //          <Reservation:EVSEID>?</Authorization:EVSEID>
        //
        //       </Reservation:eRoamingAuthorizeRemoteReservationStop>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AuthorizeRemoteReservationStopXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeRemoteReservationStopRequest Parse(XElement             AuthorizeRemoteReservationStopXML,
                                             OnExceptionDelegate  OnException = null)
        {

            AuthorizeRemoteReservationStopRequest _AuthorizeRemoteReservationStop;

            if (TryParse(AuthorizeRemoteReservationStopXML, out _AuthorizeRemoteReservationStop, OnException))
                return _AuthorizeRemoteReservationStop;

            return null;

        }

        #endregion

        #region (static) Parse(AuthorizeRemoteReservationStopText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeRemoteReservationStopRequest Parse(String               AuthorizeRemoteReservationStopText,
                                             OnExceptionDelegate  OnException = null)
        {

            AuthorizeRemoteReservationStopRequest _AuthorizeRemoteReservationStop;

            if (TryParse(AuthorizeRemoteReservationStopText, out _AuthorizeRemoteReservationStop, OnException))
                return _AuthorizeRemoteReservationStop;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStopXML,  out AuthorizeRemoteReservationStop, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteReservationStop">The parsed authorize remote reservation stop request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                            AuthorizeRemoteReservationStopXML,
                                       out AuthorizeRemoteReservationStopRequest  AuthorizeRemoteReservationStop,
                                       OnExceptionDelegate                 OnException  = null)
        {

            try
            {

                AuthorizeRemoteReservationStop = new AuthorizeRemoteReservationStopRequest(

                                                     AuthorizeRemoteReservationStopXML.MapValueOrFail    (OICPNS.Reservation + "SessionID",
                                                                                                          Session_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrFail    (OICPNS.Reservation + "ProviderID",
                                                                                                          Provider_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                          EVSE_Id.Parse),

                                                     AuthorizeRemoteReservationStopXML.MapValueOrNullable(OICPNS.Reservation + "PartnerSessionID",
                                                                                                          PartnerSession_Id.Parse)

                                                 );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AuthorizeRemoteReservationStopXML, e);

                AuthorizeRemoteReservationStop = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStopText, out AuthorizeRemoteReservationStop, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize remote reservation stop request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStopText">The text to parse.</param>
        /// <param name="AuthorizeRemoteReservationStop">The parsed authorize remote reservation stop request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                AuthorizeRemoteReservationStopText,
                                       out AuthorizeRemoteReservationStopRequest  AuthorizeRemoteReservationStop,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteReservationStopText).Root,
                             out AuthorizeRemoteReservationStop,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AuthorizeRemoteReservationStopText, e);
            }

            AuthorizeRemoteReservationStop = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop",

                                new XElement(OICPNS.Reservation + "SessionID",               SessionId.       ToString()),

                                PartnerSessionId.HasValue
                                    ? new XElement(OICPNS.Reservation + "PartnerSessionID",  PartnerSessionId.ToString())
                                    : null,

                                new XElement(OICPNS.Reservation + "ProviderID",              ProviderId.      ToString()),
                                new XElement(OICPNS.Reservation + "EVSEID",                  EVSEId.          ToString())

                           );

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
            if (Object.ReferenceEquals(AuthorizeRemoteReservationStop1, AuthorizeRemoteReservationStop2))
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

            if (Object == null)
                return false;

            var AuthorizeRemoteReservationStop = Object as AuthorizeRemoteReservationStopRequest;
            if ((Object) AuthorizeRemoteReservationStop == null)
                return false;

            return this.Equals(AuthorizeRemoteReservationStop);

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

            if ((Object) AuthorizeRemoteReservationStop == null)
                return false;

            return SessionId. Equals(AuthorizeRemoteReservationStop.SessionId)  &&
                   ProviderId.Equals(AuthorizeRemoteReservationStop.ProviderId) &&
                   EVSEId.    Equals(AuthorizeRemoteReservationStop.EVSEId)     &&

                   ((!PartnerSessionId.HasValue && !AuthorizeRemoteReservationStop.PartnerSessionId.HasValue) ||
                     (PartnerSessionId.HasValue &&  AuthorizeRemoteReservationStop.PartnerSessionId.HasValue && PartnerSessionId.Value.Equals(AuthorizeRemoteReservationStop.PartnerSessionId.Value)));

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

                return SessionId. GetHashCode() * 7 ^
                       ProviderId.GetHashCode() * 5 ^
                       EVSEId.    GetHashCode() * 3 ^

                       (PartnerSessionId.HasValue
                            ? PartnerSessionId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(SessionId,
                             " (", ProviderId, ") ",
                             " at ", EVSEId);

        #endregion


    }

}
