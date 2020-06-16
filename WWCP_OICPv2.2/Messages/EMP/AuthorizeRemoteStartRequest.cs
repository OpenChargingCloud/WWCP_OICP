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
    /// An OICP authorize remote start request.
    /// </summary>
    public class AuthorizeRemoteStartRequest : ARequest<AuthorizeRemoteStartRequest>
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
        /// The user or contract identification.
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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeRemoteStartRequest XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="Identification">The user or contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public AuthorizeRemoteStartRequest(Provider_Id            ProviderId,
                                           EVSE_Id                EVSEId,
                                           Identification         Identification,
                                           Session_Id?            SessionId             = null,
                                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                           PartnerProduct_Id?     PartnerProductId      = null,

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

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/">
        //   <soapenv:Body>
        //     <Authorization:eRoamingAuthorizeRemoteStart xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.1"
        //                                                 xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.1">
        //
        //       <!--Optional:-->
        //       <Authorization:SessionID>389f4491-2835-4dc9-9eae-1e93eb26f7f5</Authorization:SessionID>
        //
        //       <Authorization:ProviderID>DE-MEG</Authorization:ProviderID>
        //       <Authorization:EvseID>DE*BDO*E028578852*2</Authorization:EvseID>
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
        //     </Authorization:eRoamingAuthorizeRemoteStart>
        //
        //   </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (AuthorizeRemoteStartRequestXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestXML">The XML to parse.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteStartRequest

            Parse(XElement                                              AuthorizeRemoteStartRequestXML,
                  CustomXMLParserDelegate<AuthorizeRemoteStartRequest>  CustomAuthorizeRemoteStartRequestParser   = null,
                  CustomXMLParserDelegate<Identification>               CustomIdentificationParser                = null,
                  CustomXMLParserDelegate<RFIDIdentification>           CustomRFIDIdentificationParser            = null,
                  OnExceptionDelegate                                   OnException                               = null,

                  DateTime?                                             Timestamp                                 = null,
                  CancellationToken?                                    CancellationToken                         = null,
                  EventTracking_Id                                      EventTrackingId                           = null,
                  TimeSpan?                                             RequestTimeout                            = null)

        {

            if (TryParse(AuthorizeRemoteStartRequestXML,
                         out AuthorizeRemoteStartRequest _AuthorizeRemoteStartRequest,
                         CustomAuthorizeRemoteStartRequestParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteStartRequest;

            return null;

        }

        #endregion

        #region (static) Parse   (AuthorizeRemoteStartRequestText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestText">The text to parse.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeRemoteStartRequest

            Parse(String                                                AuthorizeRemoteStartRequestText,
                  CustomXMLParserDelegate<AuthorizeRemoteStartRequest>  CustomAuthorizeRemoteStartRequestParser   = null,
                  CustomXMLParserDelegate<Identification>               CustomIdentificationParser                = null,
                  CustomXMLParserDelegate<RFIDIdentification>           CustomRFIDIdentificationParser            = null,
                  OnExceptionDelegate                                   OnException                               = null,

                  DateTime?                                             Timestamp                                 = null,
                  CancellationToken?                                    CancellationToken                         = null,
                  EventTracking_Id                                      EventTrackingId                           = null,
                  TimeSpan?                                             RequestTimeout                            = null)

        {

            if (TryParse(AuthorizeRemoteStartRequestText,
                         out AuthorizeRemoteStartRequest _AuthorizeRemoteStartRequest,
                         CustomAuthorizeRemoteStartRequestParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeRemoteStartRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteStartRequestXML,  out AuthorizeRemoteStartRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestXML">The XML to parse.</param>
        /// <param name="AuthorizeRemoteStartRequest">The parsed authorize remote start request.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                              AuthorizeRemoteStartRequestXML,
                                       out AuthorizeRemoteStartRequest                       AuthorizeRemoteStartRequest,
                                       CustomXMLParserDelegate<AuthorizeRemoteStartRequest>  CustomAuthorizeRemoteStartRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>               CustomIdentificationParser                = null,
                                       CustomXMLParserDelegate<RFIDIdentification>           CustomRFIDIdentificationParser            = null,
                                       OnExceptionDelegate                                   OnException                               = null,

                                       DateTime?                                             Timestamp                                 = null,
                                       CancellationToken?                                    CancellationToken                         = null,
                                       EventTracking_Id                                      EventTrackingId                           = null,
                                       TimeSpan?                                             RequestTimeout                            = null)

        {

            try
            {

                if (AuthorizeRemoteStartRequestXML.Name != OICPNS.Authorization + "eRoamingAuthorizeRemoteStart")
                {
                    AuthorizeRemoteStartRequest = null;
                    return false;
                }

                AuthorizeRemoteStartRequest = new AuthorizeRemoteStartRequest(

                                                  AuthorizeRemoteStartRequestXML.MapValueOrFail    (OICPNS.Authorization + "ProviderID",
                                                                                                    Provider_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrFail    (OICPNS.Authorization + "EvseID",
                                                                                                    EVSE_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapElementOrFail  (OICPNS.Authorization + "Identification",
                                                                                                    (xml, e) => OICPv2_2.Identification.Parse(xml,
                                                                                                                                              CustomIdentificationParser,
                                                                                                                                              CustomRFIDIdentificationParser,
                                                                                                                                              e),
                                                                                                    OnException),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Authorization + "SessionID",
                                                                                                    Session_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Authorization + "CPOPartnerSessionID",
                                                                                                    CPOPartnerSession_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Authorization + "EMPPartnerSessionID",
                                                                                                    EMPPartnerSession_Id.Parse),

                                                  AuthorizeRemoteStartRequestXML.MapValueOrNullable(OICPNS.Authorization + "PartnerProductID",
                                                                                                    PartnerProduct_Id.Parse),

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout

                                              );


                if (CustomAuthorizeRemoteStartRequestParser != null)
                    AuthorizeRemoteStartRequest = CustomAuthorizeRemoteStartRequestParser(AuthorizeRemoteStartRequestXML,
                                                                                          AuthorizeRemoteStartRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteStartRequestXML, e);

                AuthorizeRemoteStartRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeRemoteStartRequestText, out AuthorizeRemoteStartRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize remote start request.
        /// </summary>
        /// <param name="AuthorizeRemoteStartRequestText">The text to parse.</param>
        /// <param name="AuthorizeRemoteStartRequest">The parsed authorize remote start request.</param>
        /// <param name="CustomAuthorizeRemoteStartRequestParser">A delegate to parse custom AuthorizeRemoteStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                AuthorizeRemoteStartRequestText,
                                       out AuthorizeRemoteStartRequest                       AuthorizeRemoteStartRequest,
                                       CustomXMLParserDelegate<AuthorizeRemoteStartRequest>  CustomAuthorizeRemoteStartRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>               CustomIdentificationParser                = null,
                                       CustomXMLParserDelegate<RFIDIdentification>           CustomRFIDIdentificationParser            = null,
                                       OnExceptionDelegate                                   OnException                               = null,

                                       DateTime?                                             Timestamp                                 = null,
                                       CancellationToken?                                    CancellationToken                         = null,
                                       EventTracking_Id                                      EventTrackingId                           = null,
                                       TimeSpan?                                             RequestTimeout                            = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeRemoteStartRequestText).Root,
                             out AuthorizeRemoteStartRequest,
                             CustomAuthorizeRemoteStartRequestParser,
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
                OnException?.Invoke(DateTime.UtcNow, AuthorizeRemoteStartRequestText, e);
            }

            AuthorizeRemoteStartRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizeRemoteStartRequestSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeRemoteStartRequestSerializer">A delegate to customize the serialization of AuthorizeRemoteStart requests.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizeRemoteStartRequest>  CustomAuthorizeRemoteStartRequestSerializer   = null,
                              CustomXMLSerializerDelegate<Identification>               CustomIdentificationSerializer                = null)
        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart",

                                       SessionId.HasValue
                                           ? new XElement(OICPNS.Authorization + "SessionID",            SessionId.          ToString())
                                           : null,

                                       CPOPartnerSessionId.HasValue
                                           ? new XElement(OICPNS.Authorization + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                                           : null,

                                       EMPPartnerSessionId.HasValue
                                           ? new XElement(OICPNS.Authorization + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                                           : null,

                                       new XElement(OICPNS.Authorization + "ProviderID",                 ProviderId.         ToString()),
                                       new XElement(OICPNS.Authorization + "EvseID",                     EVSEId.             ToString()),

                                       Identification.ToXML(CustomIdentificationSerializer: CustomIdentificationSerializer),

                                       PartnerProductId.HasValue
                                           ? new XElement(OICPNS.Authorization + "PartnerProductID", PartnerProductId.   ToString())
                                           : null

                                   );

            return CustomAuthorizeRemoteStartRequestSerializer != null
                       ? CustomAuthorizeRemoteStartRequestSerializer(this, XML)
                       : XML;

        }

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
            if (ReferenceEquals(AuthorizeRemoteStartRequest1, AuthorizeRemoteStartRequest2))
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

            if (Object is null)
                return false;

            if (!(Object is AuthorizeRemoteStartRequest AuthorizeRemoteStartRequest))
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

            if (AuthorizeRemoteStartRequest is null)
                return false;

            return ProviderId.    Equals(AuthorizeRemoteStartRequest.ProviderId)     &&
                   EVSEId.        Equals(AuthorizeRemoteStartRequest.EVSEId)         &&
                   Identification.Equals(AuthorizeRemoteStartRequest.Identification) &&

                   ((!SessionId.          HasValue && !AuthorizeRemoteStartRequest.SessionId.          HasValue) ||
                     (SessionId.          HasValue &&  AuthorizeRemoteStartRequest.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizeRemoteStartRequest.SessionId.          Value))) &&

                   ((!CPOPartnerSessionId.HasValue && !AuthorizeRemoteStartRequest.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  AuthorizeRemoteStartRequest.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizeRemoteStartRequest.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !AuthorizeRemoteStartRequest.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  AuthorizeRemoteStartRequest.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizeRemoteStartRequest.EMPPartnerSessionId.Value))) &&

                   ((!PartnerProductId.   HasValue && !AuthorizeRemoteStartRequest.PartnerProductId.   HasValue) ||
                     (PartnerProductId.   HasValue &&  AuthorizeRemoteStartRequest.PartnerProductId.   HasValue && PartnerProductId.   Value.Equals(AuthorizeRemoteStartRequest.PartnerProductId.   Value)));

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

                return ProviderId.    GetHashCode() * 19 ^
                       EVSEId.        GetHashCode() * 17 ^
                       Identification.GetHashCode() * 11 ^

                       (SessionId.          HasValue
                            ? SessionId.          GetHashCode() * 7
                            : 0) ^

                       (CPOPartnerSessionId.HasValue
                            ? CPOPartnerSessionId.GetHashCode() * 5
                            : 0) ^

                       (EMPPartnerSessionId.HasValue
                            ? EMPPartnerSessionId.GetHashCode() * 3
                            : 0) ^

                       (!PartnerProductId.  HasValue
                            ? PartnerProductId.   GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId,
                             " for ", Identification,
                             " (", ProviderId, ")");

        #endregion


    }

}
