/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP authorize start request.
    /// </summary>
    public class AuthorizeStartRequest : ARequest<AuthorizeStartRequest>
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the charging station operator.
        /// </summary>
        public Operator_Id         OperatorId          { get; }

        /// <summary>
        /// An user identification.
        /// </summary>
        public Identification      Identification      { get; }

        /// <summary>
        /// An optional EVSE identification.
        /// </summary>
        public EVSE_Id?            EVSEId              { get; }

        /// <summary>
        /// An optional partner product identification.
        /// </summary>
        public PartnerProduct_Id?  PartnerProductId    { get; }

        /// <summary>
        /// An optional charging session identification.
        /// </summary>
        public Session_Id?         SessionId           { get; }

        /// <summary>
        /// An optional partner session identification.
        /// </summary>
        public PartnerSession_Id?  PartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeStart XML/SOAP request.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// <param name="Identification">An user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public AuthorizeStartRequest(Operator_Id         OperatorId,
                                     Identification      Identification,
                                     EVSE_Id?            EVSEId              = null,
                                     PartnerProduct_Id?  PartnerProductId    = null,   // OICP v2.1: [max 100]
                                     Session_Id?         SessionId           = null,
                                     PartnerSession_Id?  PartnerSessionId    = null,   // OICP v2.1: [max 50]

                                     DateTime?           Timestamp           = null,
                                     CancellationToken?  CancellationToken   = null,
                                     EventTracking_Id    EventTrackingId     = null,
                                     TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorId        = OperatorId;
            this.Identification    = Identification;
            this.EVSEId            = EVSEId;
            this.PartnerProductId  = PartnerProductId;
            this.SessionId         = SessionId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv        = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization  = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
        //                   xmlns:CommonTypes    = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingAuthorizeStart>
        //
        //          <!--Optional:-->
        //          <Authorization:SessionID>?</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        //
        //          <Authorization:OperatorID>?</Authorization:OperatorID>
        //
        //          <!--Optional:-->
        //          <Authorization:EVSEID>?</Authorization:EVSEID>
        //
        //          <Authorization:Identification>
        //             <!--You have a CHOICE of the next 4 items at this level-->
        //
        //             <CommonTypes:RFIDmifarefamilyIdentification>
        //                <CommonTypes:UID>08152305</CommonTypes:UID>
        //             </CommonTypes:RFIDmifarefamilyIdentification>
        // 
        //             <CommonTypes:QRCodeIdentification>
        // 
        //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        // 
        //                <!--You have a CHOICE of the next 2 items at this level-->
        //                <CommonTypes:PIN>1234</CommonTypes:PIN>
        // 
        //                <CommonTypes:HashedPIN>
        //                   <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
        //                   <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
        //                   <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
        //                </CommonTypes:HashedPIN>
        // 
        //             </CommonTypes:QRCodeIdentification>
        // 
        //             <CommonTypes:PlugAndChargeIdentification>
        //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //             </CommonTypes:PlugAndChargeIdentification>
        // 
        //             <CommonTypes:RemoteIdentification>
        //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
        //             </CommonTypes:RemoteIdentification>
        //
        //          </Authorization:Identification>
        //
        //          <!--Optional:-->
        //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
        //
        //       </Authorization:eRoamingAuthorizeStart>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (AuthorizeStartXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize start request.
        /// </summary>
        /// <param name="AuthorizeStartXML">The XML to parse.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to customize the deserialization of AuthorizeStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeStartRequest Parse(XElement                                        AuthorizeStartXML,
                                                  CustomXMLParserDelegate<AuthorizeStartRequest>  CustomAuthorizeStartRequestParser   = null,
                                                  CustomXMLParserDelegate<Identification>         CustomIdentificationParser          = null,
                                                  OnExceptionDelegate                             OnException                         = null,

                                                  DateTime?                                       Timestamp                           = null,
                                                  CancellationToken?                              CancellationToken                   = null,
                                                  EventTracking_Id                                EventTrackingId                     = null,
                                                  TimeSpan?                                       RequestTimeout                      = null)

        {

            if (TryParse(AuthorizeStartXML,
                         out AuthorizeStartRequest _AuthorizeStart,
                         CustomAuthorizeStartRequestParser,
                         CustomIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeStart;

            return null;

        }

        #endregion

        #region (static) Parse   (AuthorizeStartText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP authorize start request.
        /// </summary>
        /// <param name="AuthorizeStartText">The text to parse.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to customize the deserialization of AuthorizeStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static AuthorizeStartRequest Parse(String                                          AuthorizeStartText,
                                                  CustomXMLParserDelegate<AuthorizeStartRequest>  CustomAuthorizeStartRequestParser   = null,
                                                  CustomXMLParserDelegate<Identification>         CustomIdentificationParser          = null,
                                                  OnExceptionDelegate                             OnException                         = null,

                                                  DateTime?                                       Timestamp                           = null,
                                                  CancellationToken?                              CancellationToken                   = null,
                                                  EventTracking_Id                                EventTrackingId                     = null,
                                                  TimeSpan?                                       RequestTimeout                      = null)

        {

            if (TryParse(AuthorizeStartText,
                         out AuthorizeStartRequest _AuthorizeStart,
                         CustomAuthorizeStartRequestParser,
                         CustomIdentificationParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _AuthorizeStart;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeStartXML,  out AuthorizeStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize start request.
        /// </summary>
        /// <param name="AuthorizeStartXML">The XML to parse.</param>
        /// <param name="AuthorizeStart">The parsed authorize start request.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to customize the deserialization of AuthorizeStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                        AuthorizeStartXML,
                                       out AuthorizeStartRequest                       AuthorizeStart,
                                       CustomXMLParserDelegate<AuthorizeStartRequest>  CustomAuthorizeStartRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>         CustomIdentificationParser          = null,
                                       OnExceptionDelegate                             OnException                         = null,

                                       DateTime?                                       Timestamp                           = null,
                                       CancellationToken?                              CancellationToken                   = null,
                                       EventTracking_Id                                EventTrackingId                     = null,
                                       TimeSpan?                                       RequestTimeout                      = null)
        {

            try
            {

                if (AuthorizeStartXML.Name != OICPNS.Authorization + "eRoamingAuthorizeStart")
                {
                    AuthorizeStart = null;
                    return false;
                }

                AuthorizeStart = new AuthorizeStartRequest(

                                     AuthorizeStartXML.MapValueOrFail    (OICPNS.Authorization + "OperatorID",
                                                                          Operator_Id.Parse),

                                     AuthorizeStartXML.MapElementOrFail  (OICPNS.Authorization + "Identification",
                                                                          (xml, e) => Identification.Parse(xml,
                                                                                                           CustomIdentificationParser,
                                                                                                           e),
                                                                          OnException),

                                     AuthorizeStartXML.MapValueOrNullable(OICPNS.Authorization + "EVSEID",
                                                                          EVSE_Id.Parse),

                                     AuthorizeStartXML.MapValueOrNullable(OICPNS.Authorization + "PartnerProductID",
                                                                          PartnerProduct_Id.Parse),

                                     AuthorizeStartXML.MapValueOrNullable(OICPNS.Authorization + "SessionID",
                                                                          Session_Id.Parse),

                                     AuthorizeStartXML.MapValueOrNullable(OICPNS.Authorization + "PartnerSessionID",
                                                                          PartnerSession_Id.Parse),

                                     Timestamp,
                                     CancellationToken,
                                     EventTrackingId,
                                     RequestTimeout

                                 );


                if (CustomAuthorizeStartRequestParser != null)
                    AuthorizeStart = CustomAuthorizeStartRequestParser(AuthorizeStartXML,
                                                                       AuthorizeStart);

                return true;

            }
            catch (Exception e)
            {

                DebugX.Log(e.Message + Environment.NewLine + e.StackTrace);

                OnException?.Invoke(DateTime.UtcNow, AuthorizeStartXML, e);

                AuthorizeStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeStartText, out AuthorizeStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize start request.
        /// </summary>
        /// <param name="AuthorizeStartText">The text to parse.</param>
        /// <param name="AuthorizeStart">The parsed authorize start request.</param>
        /// <param name="CustomAuthorizeStartRequestParser">A delegate to customize the deserialization of AuthorizeStart requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                          AuthorizeStartText,
                                       out AuthorizeStartRequest                       AuthorizeStart,
                                       CustomXMLParserDelegate<AuthorizeStartRequest>  CustomAuthorizeStartRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>         CustomIdentificationParser          = null,
                                       OnExceptionDelegate                             OnException                         = null,

                                       DateTime?                                       Timestamp                           = null,
                                       CancellationToken?                              CancellationToken                   = null,
                                       EventTracking_Id                                EventTrackingId                     = null,
                                       TimeSpan?                                       RequestTimeout                      = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeStartText).Root,
                             out AuthorizeStart,
                             CustomAuthorizeStartRequestParser,
                             CustomIdentificationParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthorizeStartText, e);
            }

            AuthorizeStart = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizeStartRequestSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeStartRequestSerializer">A delegate to customize the serialization of AuthorizeStart requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizeStartRequest> CustomAuthorizeStartRequestSerializer  = null,
                              CustomXMLSerializerDelegate<Identification>        CustomIdentificationSerializer         = null)

        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingAuthorizeStart",

                                       SessionId.       HasValue ? new XElement(OICPNS.Authorization + "SessionID",        SessionId.       ToString()) : null,
                                       PartnerSessionId.HasValue ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,

                                       new XElement(OICPNS.Authorization + "OperatorID",    OperatorId.ToString()),

                                       EVSEId.HasValue
                                           ? new XElement(OICPNS.Authorization + "EVSEID",  EVSEId.    ToString())
                                           : null,

                                       Identification.ToXML(CustomIdentificationSerializer: CustomIdentificationSerializer),
   
                                       PartnerProductId.HasValue
                                           ? new XElement(OICPNS.Authorization + "PartnerProductID", PartnerProductId.ToString())
                                           : null

                                  );

            return CustomAuthorizeStartRequestSerializer != null
                       ? CustomAuthorizeStartRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeStart1, AuthorizeStart2)

        /// <summary>
        /// Compares two authorize start requests for equality.
        /// </summary>
        /// <param name="AuthorizeStart1">An authorize start request.</param>
        /// <param name="AuthorizeStart2">Another authorize start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeStartRequest AuthorizeStart1, AuthorizeStartRequest AuthorizeStart2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthorizeStart1, AuthorizeStart2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizeStart1 == null) || ((Object) AuthorizeStart2 == null))
                return false;

            return AuthorizeStart1.Equals(AuthorizeStart2);

        }

        #endregion

        #region Operator != (AuthorizeStart1, AuthorizeStart2)

        /// <summary>
        /// Compares two authorize start requests for inequality.
        /// </summary>
        /// <param name="AuthorizeStart1">An authorize start request.</param>
        /// <param name="AuthorizeStart2">Another authorize start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeStartRequest AuthorizeStart1, AuthorizeStartRequest AuthorizeStart2)

            => !(AuthorizeStart1 == AuthorizeStart2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeStart> Members

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

            var AuthorizeStart = Object as AuthorizeStartRequest;
            if ((Object) AuthorizeStart == null)
                return false;

            return this.Equals(AuthorizeStart);

        }

        #endregion

        #region Equals(AuthorizeStart)

        /// <summary>
        /// Compares two authorize start requests for equality.
        /// </summary>
        /// <param name="AuthorizeStart">An authorize start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeStartRequest AuthorizeStart)
        {

            if ((Object) AuthorizeStart == null)
                return false;

            return OperatorId.    Equals(AuthorizeStart.OperatorId)     &&
                   Identification.Equals(AuthorizeStart.Identification) &&

                   ((!EVSEId.          HasValue && !AuthorizeStart.EVSEId.          HasValue) ||
                     (EVSEId.          HasValue &&  AuthorizeStart.EVSEId.          HasValue && EVSEId.          Value.Equals(AuthorizeStart.EVSEId.          Value))) &&

                   ((!PartnerProductId.HasValue && !AuthorizeStart.PartnerProductId.HasValue) ||
                     (PartnerProductId.HasValue &&  AuthorizeStart.PartnerProductId.HasValue && PartnerProductId.Value.Equals(AuthorizeStart.PartnerProductId.Value))) &&

                   ((!SessionId.       HasValue && !AuthorizeStart.SessionId.       HasValue) ||
                     (SessionId.       HasValue &&  AuthorizeStart.SessionId.       HasValue && SessionId.       Value.Equals(AuthorizeStart.SessionId.       Value))) &&

                   ((!PartnerSessionId.HasValue && !AuthorizeStart.PartnerSessionId.HasValue) ||
                     (PartnerSessionId.HasValue &&  AuthorizeStart.PartnerSessionId.HasValue && PartnerSessionId.Value.Equals(AuthorizeStart.PartnerSessionId.Value)));

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

                return OperatorId.    GetHashCode() * 19 ^
                       Identification.GetHashCode() * 17 ^

                       (EVSEId           != null
                            ? EVSEId.          GetHashCode() * 11
                            : 0) ^

                       (PartnerProductId != null
                            ? PartnerProductId.GetHashCode() * 7
                            : 0) ^

                       (SessionId        != null
                            ? SessionId.       GetHashCode() * 5
                            : 0) ^

                       (PartnerSessionId != null
                            ? PartnerSessionId.GetHashCode() * 3
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Identification,

                             EVSEId.HasValue
                                 ? " at " + EVSEId
                                 : "",

                             " (", OperatorId, ") ",

                             PartnerProductId.HasValue
                                 ? " using " + PartnerProductId
                                 : "");

        #endregion


    }

}
