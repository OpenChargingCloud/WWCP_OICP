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
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
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
        public Provider_Id         ProviderId         { get; }

        /// <summary>
        /// An EVSE identification.
        /// </summary>
        public EVSE_Id             EVSEId             { get; }

        /// <summary>
        /// An electric vehicle contract identification.
        /// </summary>
        public EVCO_Id             EVCOId             { get; }

        /// <summary>
        /// An optional charging session identification.
        /// </summary>
        public Session_Id?         SessionId          { get; }

        /// <summary>
        /// An optional partner session identification.
        /// </summary>
        public PartnerSession_Id?  PartnerSessionId   { get; }

        /// <summary>
        /// An optional partner product identification.
        /// </summary>
        public PartnerProduct_Id?  PartnerProductId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeRemoteReservationStart XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId">An electric vehicle contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        public AuthorizeRemoteReservationStartRequest(Provider_Id         ProviderId,
                                                      EVSE_Id             EVSEId,
                                                      EVCO_Id             EVCOId,
                                                      Session_Id?         SessionId           = null,
                                                      PartnerSession_Id?  PartnerSessionId    = null,
                                                      PartnerProduct_Id?  PartnerProductId    = null,

                                                      DateTime?           Timestamp           = null,
                                                      CancellationToken?  CancellationToken   = null,
                                                      EventTracking_Id    EventTrackingId     = null,
                                                      TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId        = ProviderId;
            this.EVSEId            = EVSEId;
            this.EVCOId            = EVCOId;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;
            this.PartnerProductId  = PartnerProductId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.0"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0">
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
        //          <Reservation:PartnerSessionID>?</Reservation:PartnerSessionID>
        //
        //          <Reservation:ProviderID>?</Reservation:ProviderID>
        //          <Reservation:EVSEID>?</Reservation:EVSEID>
        //
        //          <Reservation:Identification>
        //
        //             <!--You have a CHOICE of the next 4 items at this level-->
        //             <CommonTypes:RFIDmifarefamilyIdentification>
        //                <CommonTypes:UID>?</CommonTypes:UID>
        //             </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //             <CommonTypes:QRCodeIdentification>
        //
        //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //
        //                <!--You have a CHOICE of the next 2 items at this level-->
        //                <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //                <CommonTypes:HashedPIN>
        //                   <CommonTypes:Value>?</CommonTypes:Value>
        //                   <CommonTypes:Function>?</CommonTypes:Function>
        //                   <CommonTypes:Salt>?</CommonTypes:Salt>
        //                </CommonTypes:HashedPIN>
        //
        //             </CommonTypes:QRCodeIdentification>
        //
        //             <CommonTypes:PlugAndChargeIdentification>
        //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //             </CommonTypes:PlugAndChargeIdentification>
        //
        //             <CommonTypes:RemoteIdentification>
        //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //             </CommonTypes:RemoteIdentification>
        //
        //          </Reservation:Identification>
        //
        //          <!--Optional:-->
        //          <Reservation:PartnerProductID>?</Reservation:PartnerProductID>
        //
        //       </Reservation:eRoamingAuthorizeRemoteReservationStart>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AuthorizeRemoteReservationStartXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeRemoteReservationStartRequest Parse(XElement             AuthorizeRemoteReservationStartXML,
                                             OnExceptionDelegate  OnException = null)
        {

            AuthorizeRemoteReservationStartRequest _AuthorizeRemoteReservationStart;

            if (TryParse(AuthorizeRemoteReservationStartXML, out _AuthorizeRemoteReservationStart, OnException))
                return _AuthorizeRemoteReservationStart;

            return null;

        }

        #endregion

        #region (static) Parse(AuthorizeRemoteReservationStartText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeRemoteReservationStartRequest Parse(String               AuthorizeRemoteReservationStartText,
                                             OnExceptionDelegate  OnException = null)
        {

            AuthorizeRemoteReservationStartRequest _AuthorizeRemoteReservationStart;

            if (TryParse(AuthorizeRemoteReservationStartText, out _AuthorizeRemoteReservationStart, OnException))
                return _AuthorizeRemoteReservationStart;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStartXML,  out AuthorizeRemoteReservationStart, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteReservationStart">The parsed authorize remote reservation start request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                    AuthorizeRemoteReservationStartXML,
                                       out AuthorizeRemoteReservationStartRequest  AuthorizeRemoteReservationStart,
                                       OnExceptionDelegate                         OnException  = null)
        {

            try
            {

                EVCO_Id? EVCOID = null;

                var IdentificationXML = AuthorizeRemoteReservationStartXML.Element(OICPNS.Reservation + "Identification");
                if (IdentificationXML != null)
                {

                    foreach (var XML in new XElement[] {
                        IdentificationXML.Element(OICPNS.CommonTypes + "QRCodeIdentification"),
                        IdentificationXML.Element(OICPNS.CommonTypes + "PlugAndChargeIdentification"),
                        IdentificationXML.Element(OICPNS.CommonTypes + "RemoteIdentification")
                    })
                    {

                        if (XML == null)
                            continue;

                        var EVCOIDXML = XML.Element(OICPNS.CommonTypes + "EVCOID");

                        if (EVCOIDXML == null)
                            continue;

                        EVCOID = EVCO_Id.Parse(EVCOIDXML.Value);

                    }

                }
                else
                    throw new Exception("Missing 'Identification'-XML tag!");

                if (!EVCOID.HasValue)
                    throw new Exception("Missing 'Identification/EVCOID'-XML tag!");


                AuthorizeRemoteReservationStart = new AuthorizeRemoteReservationStartRequest(

                                                      AuthorizeRemoteReservationStartXML.MapValueOrFail    (OICPNS.Reservation + "ProviderID",
                                                                                                            Provider_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                            EVSE_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                            EVCO_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "SessionID",
                                                                                                            Session_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "PartnerSessionID",
                                                                                                            PartnerSession_Id.Parse),

                                                      AuthorizeRemoteReservationStartXML.MapValueOrNullable(OICPNS.Reservation + "PartnerProductID",
                                                                                                            PartnerProduct_Id.Parse)

                                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AuthorizeRemoteReservationStartXML, e);

                AuthorizeRemoteReservationStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteReservationStartText, out AuthorizeRemoteReservationStart, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize remote reservation start request.
        /// </summary>
        /// <param name="AuthorizeRemoteReservationStartText">The text to parse.</param>
        /// <param name="AuthorizeRemoteReservationStart">The parsed authorize remote reservation start request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                AuthorizeRemoteReservationStartText,
                                       out AuthorizeRemoteReservationStartRequest  AuthorizeRemoteReservationStart,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteReservationStartText).Root,
                             out AuthorizeRemoteReservationStart,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AuthorizeRemoteReservationStartText, e);
            }

            AuthorizeRemoteReservationStart = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart",

                                SessionId.HasValue
                                    ? new XElement(OICPNS.Reservation + "SessionID",         SessionId.       ToString())
                                    : null,

                                PartnerSessionId.HasValue
                                    ? new XElement(OICPNS.Reservation + "PartnerSessionID",  PartnerSessionId.ToString())
                                    : null,

                                new XElement(OICPNS.Reservation + "ProviderID",              ProviderId.      ToString()),
                                new XElement(OICPNS.Reservation + "EVSEID",                  EVSEId.          ToString()),

                                new XElement(OICPNS.Reservation + "Identification",
                                    new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                        new XElement(OICPNS.CommonTypes + "EVCOID",
                                            EVCOId.ToString()
                                        )
                                    )
                                ),

                                PartnerProductId.HasValue
                                    ? new XElement(OICPNS.Reservation + "PartnerProductID",  PartnerProductId.ToString())
                                    : null

                            );

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
            if (Object.ReferenceEquals(AuthorizeRemoteReservationStart1, AuthorizeRemoteReservationStart2))
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

            if (Object == null)
                return false;

            var AuthorizeRemoteReservationStart = Object as AuthorizeRemoteReservationStartRequest;
            if ((Object) AuthorizeRemoteReservationStart == null)
                return false;

            return Equals(AuthorizeRemoteReservationStart);

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

            if ((Object) AuthorizeRemoteReservationStart == null)
                return false;

            return ProviderId.Equals(AuthorizeRemoteReservationStart.ProviderId) &&
                   EVSEId.    Equals(AuthorizeRemoteReservationStart.EVSEId)     &&
                   EVCOId.    Equals(AuthorizeRemoteReservationStart.EVCOId)     &&

                   ((!SessionId.       HasValue && !AuthorizeRemoteReservationStart.SessionId.       HasValue) ||
                     (SessionId.       HasValue &&  AuthorizeRemoteReservationStart.SessionId.       HasValue && SessionId.       Value.Equals(AuthorizeRemoteReservationStart.SessionId.       Value))) &&

                   ((!PartnerSessionId.HasValue && !AuthorizeRemoteReservationStart.PartnerSessionId.HasValue) ||
                     (PartnerSessionId.HasValue &&  AuthorizeRemoteReservationStart.PartnerSessionId.HasValue && PartnerSessionId.Value.Equals(AuthorizeRemoteReservationStart.PartnerSessionId.Value))) &&

                   ((!PartnerProductId.HasValue && !AuthorizeRemoteReservationStart.PartnerProductId.HasValue) ||
                     (PartnerProductId.HasValue &&  AuthorizeRemoteReservationStart.PartnerProductId.HasValue && PartnerProductId.Value.Equals(AuthorizeRemoteReservationStart.PartnerProductId.Value)));

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

                return ProviderId.GetHashCode() * 17 ^
                       EVSEId.    GetHashCode() * 11 ^
                       EVCOId.    GetHashCode() *  7 ^

                       (SessionId.        HasValue
                            ? SessionId.       GetHashCode() * 5
                            : 0) ^

                       (PartnerSessionId. HasValue
                            ? PartnerSessionId.GetHashCode() * 3
                            : 0) ^

                       (!PartnerProductId.HasValue
                            ? PartnerProductId.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId,
                             " for ", EVCOId,
                             " (", ProviderId, ")");

        #endregion


    }

}
