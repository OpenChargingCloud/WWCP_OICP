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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP authorize remote start request.
    /// </summary>
    public class AuthorizeRemoteStartRequest : ARequest<AuthorizeRemoteStartRequest>
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
        /// Create an OICP AuthorizeRemoteStartRequest XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId">An electric vehicle contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        public AuthorizeRemoteStartRequest(Provider_Id         ProviderId,
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

        // <soapenv:Envelope xmlns:soapenv        = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization  = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes    = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingAuthorizeRemoteStart>
        //
        //          <!--Optional:-->
        //          <Authorization:SessionID>?</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        //
        //          <Authorization:ProviderID>?</Authorization:ProviderID>
        //          <Authorization:EVSEID>?</Authorization:EVSEID>
        //
        //          <Authorization:Identification>
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
        //          </Authorization:Identification>
        //
        //          <!--Optional:-->
        //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
        //
        //       </Authorization:eRoamingAuthorizeRemoteStart>
        //
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AuthorizeRemoteStartRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeRemoteStartRequest Parse(XElement             AuthorizeRemoteStartRequestXML,
                                                        OnExceptionDelegate  OnException = null)
        {

            AuthorizeRemoteStartRequest _AuthorizeRemoteStartRequest;

            if (TryParse(AuthorizeRemoteStartRequestXML, out _AuthorizeRemoteStartRequest, OnException))
                return _AuthorizeRemoteStartRequest;

            return null;

        }

        #endregion

        #region (static) Parse(AuthorizeRemoteStartRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeRemoteStartRequest Parse(String               AuthorizeRemoteStartRequestText,
                                                        OnExceptionDelegate  OnException = null)
        {

            AuthorizeRemoteStartRequest _AuthorizeRemoteStartRequest;

            if (TryParse(AuthorizeRemoteStartRequestText, out _AuthorizeRemoteStartRequest, OnException))
                return _AuthorizeRemoteStartRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteStartRequestXML,  out AuthorizeRemoteStartRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteStartRequest">The parsed authorize remote start request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                         AuthorizeRemoteStartRequestXML,
                                       out AuthorizeRemoteStartRequest  AuthorizeRemoteStartRequest,
                                       OnExceptionDelegate              OnException         = null,

                                       DateTime?                        Timestamp           = null,
                                       CancellationToken?               CancellationToken   = null,
                                       EventTracking_Id                 EventTrackingId     = null,
                                       TimeSpan?                        RequestTimeout      = null)
        {

            try
            {

                EVCO_Id? EVCOID = null;

                var IdentificationXML = AuthorizeRemoteStartRequestXML.Element(OICPNS.Reservation + "Identification");
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


                AuthorizeRemoteStartRequest = new AuthorizeRemoteStartRequest(

                                                  AuthorizeRemoteStartRequestXML.MapValueOrFail    (OICPNS.Reservation + "ProviderID",
                                                                                                    Provider_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                    EVSE_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrFail    (OICPNS.Reservation + "EVSEID",
                                                                                                    EVCO_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Reservation + "SessionID",
                                                                                                    Session_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Reservation + "PartnerSessionID",
                                                                                                    PartnerSession_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Reservation + "PartnerProductID",
                                                                                                    PartnerProduct_Id.Parse),

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout

                                              );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AuthorizeRemoteStartRequestXML, e);

                AuthorizeRemoteStartRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteStartRequestText, out AuthorizeRemoteStartRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestText">The text to parse.</param>
        /// <param name="AuthorizeRemoteStartRequest">The parsed authorize remote start request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                           AuthorizeRemoteStartRequestText,
                                       out AuthorizeRemoteStartRequest  AuthorizeRemoteStartRequest,
                                       OnExceptionDelegate              OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteStartRequestText).Root,
                             out AuthorizeRemoteStartRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AuthorizeRemoteStartRequestText, e);
            }

            AuthorizeRemoteStartRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart",

                                SessionId.HasValue
                                    ? new XElement(OICPNS.Authorization + "SessionID",         SessionId.       ToString())
                                    : null,

                                PartnerSessionId.HasValue
                                    ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                                    : null,

                                new XElement(OICPNS.Authorization + "ProviderID",              ProviderId.      ToString()),
                                new XElement(OICPNS.Authorization + "EVSEID",                  EVSEId.          ToString()),

                                new XElement(OICPNS.Authorization + "Identification",
                                    new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                        new XElement(OICPNS.CommonTypes + "EVCOID",
                                            EVCOId.ToString()
                                        )
                                    )
                                ),

                                PartnerProductId.HasValue
                                    ? new XElement(OICPNS.Authorization + "PartnerProductID",  PartnerProductId.ToString())
                                    : null

                            );

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2)

        /// <summary>
        /// Compares two authorize remote start requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequest1">An authorize remote start request.</param>
        /// <param name="AuthorizeRemoteStartRequest2">Another authorize remote start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizeRemoteStartRequest1 == null) || ((Object) AuthorizeRemoteStartRequest2 == null))
                return false;

            return AuthorizeRemoteStartRequest1.Equals(AuthorizeRemoteStartRequest2);

        }

        #endregion

        #region Operator != (AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2)

        /// <summary>
        /// Compares two authorize remote start requests for inequality.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequest1">An authorize remote start request.</param>
        /// <param name="AuthorizeRemoteStartRequest2">Another authorize remote start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest2)

            => !(AuthorizeRemoteStartRequest1 == AuthorizeRemoteStartRequest2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeRemoteStartRequest> Members

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

            var AuthorizeRemoteStartRequest = Object as AuthorizeRemoteStartRequest;
            if ((Object) AuthorizeRemoteStartRequest == null)
                return false;

            return Equals(AuthorizeRemoteStartRequest);

        }

        #endregion

        #region Equals(AuthorizeRemoteStartRequest)

        /// <summary>
        /// Compares two authorize remote start requests for equality.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequest">An authorize remote start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest)
        {

            if ((Object) AuthorizeRemoteStartRequest == null)
                return false;

            return ProviderId.Equals(AuthorizeRemoteStartRequest.ProviderId) &&
                   EVSEId.    Equals(AuthorizeRemoteStartRequest.EVSEId)     &&
                   EVCOId.    Equals(AuthorizeRemoteStartRequest.EVCOId)     &&

                   ((!SessionId.       HasValue && !AuthorizeRemoteStartRequest.SessionId.       HasValue) ||
                     (SessionId.       HasValue &&  AuthorizeRemoteStartRequest.SessionId.       HasValue && SessionId.       Value.Equals(AuthorizeRemoteStartRequest.SessionId.       Value))) &&

                   ((!PartnerSessionId.HasValue && !AuthorizeRemoteStartRequest.PartnerSessionId.HasValue) ||
                     (PartnerSessionId.HasValue &&  AuthorizeRemoteStartRequest.PartnerSessionId.HasValue && PartnerSessionId.Value.Equals(AuthorizeRemoteStartRequest.PartnerSessionId.Value))) &&

                   ((!PartnerProductId.HasValue && !AuthorizeRemoteStartRequest.PartnerProductId.HasValue) ||
                     (PartnerProductId.HasValue &&  AuthorizeRemoteStartRequest.PartnerProductId.HasValue && PartnerProductId.Value.Equals(AuthorizeRemoteStartRequest.PartnerProductId.Value)));

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
