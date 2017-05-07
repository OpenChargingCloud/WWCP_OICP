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

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP authorize stop request.
    /// </summary>
    public class AuthorizeStopRequest : ARequest<AuthorizeStopRequest>
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the charging station operator.
        /// </summary>
        public Operator_Id         OperatorId          { get; }

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public Session_Id          SessionId           { get; }

        /// <summary>
        /// A (RFID) user identification.
        /// </summary>
        public UID                 UID                 { get; }

        /// <summary>
        /// An optional EVSE identification.
        /// </summary>
        public EVSE_Id?            EVSEId              { get; }

        /// <summary>
        /// An optional partner session identification.
        /// </summary>
        public PartnerSession_Id?  PartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP AuthorizeStop XML/SOAP request.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="UID">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public AuthorizeStopRequest(Operator_Id         OperatorId,
                                    Session_Id          SessionId,
                                    UID                 UID,
                                    EVSE_Id?            EVSEId              = null,
                                    PartnerSession_Id?  PartnerSessionId    = null,

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
            this.SessionId         = SessionId;
            this.UID               = UID;
            this.EVSEId            = EVSEId;
            this.PartnerSessionId  = PartnerSessionId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingAuthorizeStop>
        // 
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
        //       </Authorization:eRoamingAuthorizeStop>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (AuthorizeStopXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorize stop request.
        /// </summary>
        /// <param name="AuthorizeStopXML">The XML to parse.</param>
        /// <param name="CustomAuthorizeStopRequestParser">A delegate to customize the deserialization of AuthorizeStop requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeStopRequest Parse(XElement                                    AuthorizeStopXML,
                                                 CustomXMLParserDelegate<Identification>        CustomIdentificationParser         = null,
                                                 CustomXMLParserDelegate<AuthorizeStopRequest>  CustomAuthorizeStopRequestParser   = null,
                                                 OnExceptionDelegate                         OnException                        = null)
        {

            AuthorizeStopRequest _AuthorizeStop;

            if (TryParse(AuthorizeStopXML,
                         out _AuthorizeStop,
                         CustomAuthorizeStopRequestParser,
                         CustomIdentificationParser,
                         OnException))

                return _AuthorizeStop;

            return null;

        }

        #endregion

        #region (static) Parse   (AuthorizeStopText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP authorize stop request.
        /// </summary>
        /// <param name="AuthorizeStopText">The text to parse.</param>
        /// <param name="CustomAuthorizeStopRequestParser">A delegate to customize the deserialization of AuthorizeStop requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizeStopRequest Parse(String                                      AuthorizeStopText,
                                                 CustomXMLParserDelegate<AuthorizeStopRequest>  CustomAuthorizeStopRequestParser   = null,
                                                 CustomXMLParserDelegate<Identification>        CustomIdentificationParser         = null,
                                                 OnExceptionDelegate                         OnException                        = null)
        {

            AuthorizeStopRequest _AuthorizeStop;

            if (TryParse(AuthorizeStopText,
                         out _AuthorizeStop,
                         CustomAuthorizeStopRequestParser,
                         CustomIdentificationParser,
                         OnException))

                return _AuthorizeStop;

            return null;

        }

        #endregion

        #region (static) TryParse(AuthorizeStopXML,  out AuthorizeStop, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorize stop request.
        /// </summary>
        /// <param name="AuthorizeStopXML">The XML to parse.</param>
        /// <param name="AuthorizeStop">The parsed authorize stop request.</param>
        /// <param name="CustomAuthorizeStopRequestParser">A delegate to customize the deserialization of AuthorizeStop requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                    AuthorizeStopXML,
                                       out AuthorizeStopRequest                    AuthorizeStop,
                                       CustomXMLParserDelegate<AuthorizeStopRequest>  CustomAuthorizeStopRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>        CustomIdentificationParser         = null,
                                       OnExceptionDelegate                         OnException                        = null,

                                       DateTime?                                   Timestamp                          = null,
                                       CancellationToken?                          CancellationToken                  = null,
                                       EventTracking_Id                            EventTrackingId                    = null,
                                       TimeSpan?                                   RequestTimeout                     = null)
        {

            try
            {

                if (AuthorizeStopXML.Name != OICPNS.Authorization + "eRoamingAuthorizeStop")
                {
                    AuthorizeStop = null;
                    return false;
                }

                var UID = AuthorizeStopXML.MapElementOrFail(OICPNS.Authorization + "Identification",
                                                            (xml, e) => Identification.Parse(xml,
                                                                                             CustomIdentificationParser,
                                                                                             e),
                                                            OnException).RFIDId;

                if (!UID.HasValue)
                    throw new Exception("No UID/RFID identification found in request!");


                AuthorizeStop = new AuthorizeStopRequest(

                                     AuthorizeStopXML.MapValueOrFail    (OICPNS.Authorization + "OperatorID",
                                                                         Operator_Id.Parse),

                                     AuthorizeStopXML.MapValueOrFail    (OICPNS.Authorization + "SessionID",
                                                                         Session_Id.Parse),

                                     UID.Value,

                                     AuthorizeStopXML.MapValueOrNullable(OICPNS.Authorization + "EVSEID",
                                                                         EVSE_Id.Parse),

                                     AuthorizeStopXML.MapValueOrNullable(OICPNS.Authorization + "PartnerSessionID",
                                                                         PartnerSession_Id.Parse),

                                     Timestamp,
                                     CancellationToken,
                                     EventTrackingId,
                                     RequestTimeout

                                 );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AuthorizeStopXML, e);

                AuthorizeStop = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AuthorizeStopText, out AuthorizeStop, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorize stop request.
        /// </summary>
        /// <param name="AuthorizeStopText">The text to parse.</param>
        /// <param name="AuthorizeStop">The parsed authorize stop request.</param>
        /// <param name="CustomAuthorizeStopRequestParser">A delegate to customize the deserialization of AuthorizeStop requests.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                      AuthorizeStopText,
                                       out AuthorizeStopRequest                    AuthorizeStop,
                                       CustomXMLParserDelegate<AuthorizeStopRequest>  CustomAuthorizeStopRequestParser   = null,
                                       CustomXMLParserDelegate<Identification>        CustomIdentificationParser         = null,
                                       OnExceptionDelegate                         OnException                        = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AuthorizeStopText).Root,
                             out AuthorizeStop,
                             CustomAuthorizeStopRequestParser,
                             CustomIdentificationParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AuthorizeStopText, e);
            }

            AuthorizeStop = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizeStopRequestSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizeStopRequestSerializer">A delegate to customize the serialization of AuthorizeStop requests.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizeStopRequest> CustomAuthorizeStopRequestSerializer  = null,
                              CustomXMLSerializerDelegate<Identification>       CustomIdentificationSerializer        = null)

        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingAuthorizeStop",

                                      new XElement(OICPNS.Authorization + "SessionID",               SessionId.       ToString()),

                                      PartnerSessionId.HasValue
                                          ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                                          : null,

                                      new XElement(OICPNS.Authorization + "OperatorID",              OperatorId.      ToString()),

                                      EVSEId.HasValue
                                          ? new XElement(OICPNS.Authorization + "EVSEID",            EVSEId.          ToString())
                                          : null,

                                      Identification.FromRFIDId(UID).ToXML(CustomIdentificationSerializer: CustomIdentificationSerializer)

                                  );

            return CustomAuthorizeStopRequestSerializer != null
                       ? CustomAuthorizeStopRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizeStop1, AuthorizeStop2)

        /// <summary>
        /// Compares two authorize stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeStop1">An authorize stop request.</param>
        /// <param name="AuthorizeStop2">Another authorize stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizeStopRequest AuthorizeStop1, AuthorizeStopRequest AuthorizeStop2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthorizeStop1, AuthorizeStop2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizeStop1 == null) || ((Object) AuthorizeStop2 == null))
                return false;

            return AuthorizeStop1.Equals(AuthorizeStop2);

        }

        #endregion

        #region Operator != (AuthorizeStop1, AuthorizeStop2)

        /// <summary>
        /// Compares two authorize stop requests for inequality.
        /// </summary>
        /// <param name="AuthorizeStop1">An authorize stop request.</param>
        /// <param name="AuthorizeStop2">Another authorize stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizeStopRequest AuthorizeStop1, AuthorizeStopRequest AuthorizeStop2)

            => !(AuthorizeStop1 == AuthorizeStop2);

        #endregion

        #endregion

        #region IEquatable<AuthorizeStop> Members

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

            var AuthorizeStop = Object as AuthorizeStopRequest;
            if ((Object) AuthorizeStop == null)
                return false;

            return this.Equals(AuthorizeStop);

        }

        #endregion

        #region Equals(AuthorizeStop)

        /// <summary>
        /// Compares two authorize stop requests for equality.
        /// </summary>
        /// <param name="AuthorizeStop">An authorize stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizeStopRequest AuthorizeStop)
        {

            if ((Object) AuthorizeStop == null)
                return false;

            return OperatorId.Equals(AuthorizeStop.OperatorId) &&
                   SessionId. Equals(AuthorizeStop.SessionId)  &&
                   UID.       Equals(AuthorizeStop.UID)        &&

                   ((!EVSEId.          HasValue && !AuthorizeStop.EVSEId.          HasValue) ||
                     (EVSEId.          HasValue &&  AuthorizeStop.EVSEId.          HasValue && EVSEId.          Value.Equals(AuthorizeStop.EVSEId.          Value))) &&

                   ((!PartnerSessionId.HasValue && !AuthorizeStop.PartnerSessionId.HasValue) ||
                     (PartnerSessionId.HasValue &&  AuthorizeStop.PartnerSessionId.HasValue && PartnerSessionId.Value.Equals(AuthorizeStop.PartnerSessionId.Value)));

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

                return OperatorId.GetHashCode() * 19 ^
                       SessionId. GetHashCode() * 17 ^
                       UID.       GetHashCode() * 11 ^

                       (EVSEId           != null
                            ? EVSEId.          GetHashCode() * 7
                            : 0) ^

                       (PartnerSessionId != null
                            ? PartnerSessionId.GetHashCode() * 5
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(UID,

                             EVSEId.HasValue
                                 ? " at " + EVSEId
                                 : "",

                             " (", OperatorId, ") ");

        #endregion


    }

}
