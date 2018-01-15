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

namespace org.GraphDefined.WWCP.OICPv2_1.Mobile
{

    /// <summary>
    /// An OICP mobile authorize start request.
    /// </summary>
    public class MobileAuthorizeStartRequest : ARequest<MobileAuthorizeStartRequest>
    {

        #region Properties

        /// <summary>
        /// The EVSE identification.
        /// </summary>
        public EVSE_Id               EVSEId                  { get; }

        /// <summary>
        /// The EVCO identification with its PIN.
        /// </summary>
        public QRCodeIdentification  QRCodeIdentification    { get; }

        /// <summary>
        /// The optional charging product identification.
        /// </summary>
        public PartnerProduct_Id?    PartnerProductId        { get; }

        /// <summary>
        /// Whether to start a new charging session or not.
        /// </summary>
        public Boolean?              GetNewSession           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new MobileAuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOIdWithPIN">The EVCO identification with its PIN.</param>
        /// <param name="ProductId">An optional charging product identification.</param>
        /// <param name="GetNewSession">Whether to start a new charging session or not.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public MobileAuthorizeStartRequest(EVSE_Id               EVSEId,
                                           QRCodeIdentification  QRCodeIdentification,
                                           PartnerProduct_Id?    PartnerProductId    = null,
                                           Boolean?              GetNewSession       = null,

                                           DateTime?             Timestamp           = null,
                                           CancellationToken?    CancellationToken   = null,
                                           EventTracking_Id      EventTrackingId     = null,
                                           TimeSpan?             RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.EVSEId                = EVSEId;
            this.QRCodeIdentification  = QRCodeIdentification;
            this.PartnerProductId      = PartnerProductId;
            this.GetNewSession         = GetNewSession;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv              = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:MobileAuthorization  = "http://www.hubject.com/b2b/services/mobileauthorization/MobileAuthorization.0"
        //                   xmlns:CommonTypes          = "http://www.hubject.com/b2b/services/commontypes/MobileAuthorization.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <MobileAuthorization:eRoamingMobileAuthorizeStart>
        //
        //          <MobileAuthorization:EvseID>?</MobileAuthorization:EvseID>
        //
        //          <MobileAuthorization:QRCodeIdentification>
        //
        //             <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //
        //             <!--You have a CHOICE of the next 2 items at this level-->
        //             <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //             <CommonTypes:HashedPIN>
        //                <CommonTypes:Value>?</CommonTypes:Value>
        //                <CommonTypes:Function>?</CommonTypes:Function>
        //                <CommonTypes:Salt>?</CommonTypes:Salt>
        //             </CommonTypes:HashedPIN>
        //
        //          </MobileAuthorization:QRCodeIdentification>
        //
        //          <!--Optional:-->
        //          <MobileAuthorization:PartnerProductID>?</MobileAuthorization:PartnerProductID>
        //
        //          <!--Optional:-->
        //          <MobileAuthorization:GetNewSession>true|false</MobileAuthorization:GetNewSession>
        //
        //       </MobileAuthorization:eRoamingMobileAuthorizeStart>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(MobileAuthorizeStartXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileAuthorizeStartXML">The XML to parse.</param>
        /// <param name="CustomMobileAuthorizeStartRequestParser">A delegate to parse custom MobileAuthorizeStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static MobileAuthorizeStartRequest

            Parse(XElement                                              MobileAuthorizeStartXML,
                  CustomXMLParserDelegate<MobileAuthorizeStartRequest>  CustomMobileAuthorizeStartRequestParser   = null,
                  OnExceptionDelegate                                   OnException                               = null,

                  DateTime?                                             Timestamp                                 = null,
                  CancellationToken?                                    CancellationToken                         = null,
                  EventTracking_Id                                      EventTrackingId                           = null,
                  TimeSpan?                                             RequestTimeout                            = null)

        {

            if (TryParse(MobileAuthorizeStartXML,
                         out MobileAuthorizeStartRequest _MobileAuthorizeStart,
                         CustomMobileAuthorizeStartRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _MobileAuthorizeStart;

            return null;

        }

        #endregion

        #region (static) Parse(MobileAuthorizeStartText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileAuthorizeStartText">The text to parse.</param>
        /// <param name="CustomMobileAuthorizeStartRequestParser">A delegate to parse custom MobileAuthorizeStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static MobileAuthorizeStartRequest

            Parse(String                                                MobileAuthorizeStartText,
                  CustomXMLParserDelegate<MobileAuthorizeStartRequest>  CustomMobileAuthorizeStartRequestParser   = null,
                  OnExceptionDelegate                                   OnException                               = null,

                  DateTime?                                             Timestamp                                 = null,
                  CancellationToken?                                    CancellationToken                         = null,
                  EventTracking_Id                                      EventTrackingId                           = null,
                  TimeSpan?                                             RequestTimeout                            = null)

        {

            if (TryParse(MobileAuthorizeStartText,
                         out MobileAuthorizeStartRequest _MobileAuthorizeStart,
                         CustomMobileAuthorizeStartRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _MobileAuthorizeStart;

            return null;

        }

        #endregion

        #region (static) TryParse(MobileAuthorizeStartXML,  out MobileAuthorizeStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileAuthorizeStartXML">The XML to parse.</param>
        /// <param name="MobileAuthorizeStart">The parsed mobile authorize start request.</param>
        /// <param name="CustomMobileAuthorizeStartRequestParser">A delegate to parse custom MobileAuthorizeStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                              MobileAuthorizeStartXML,
                                       out MobileAuthorizeStartRequest                       MobileAuthorizeStart,
                                       CustomXMLParserDelegate<MobileAuthorizeStartRequest>  CustomMobileAuthorizeStartRequestParser   = null,
                                       OnExceptionDelegate                                   OnException                               = null,

                                       DateTime?                                             Timestamp                                 = null,
                                       CancellationToken?                                    CancellationToken                         = null,
                                       EventTracking_Id                                      EventTrackingId                           = null,
                                       TimeSpan?                                             RequestTimeout                            = null)

        {

            try
            {

                if (MobileAuthorizeStartXML.Name != OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart")
                {
                    MobileAuthorizeStart = null;
                    return false;
                }

                MobileAuthorizeStart = new MobileAuthorizeStartRequest(

                                           MobileAuthorizeStartXML.MapValueOrFail    (OICPNS.MobileAuthorization + "EvseID",
                                                                                      EVSE_Id.Parse),

                                           MobileAuthorizeStartXML.MapValueOrFail    (OICPNS.MobileAuthorization + "QRCodeIdentification",
                                                                                      QRCodeIdentification.Parse),

                                           MobileAuthorizeStartXML.MapValueOrNullable(OICPNS.MobileAuthorization + "PartnerProductID",
                                                                                      PartnerProduct_Id.Parse),

                                           MobileAuthorizeStartXML.MapValueOrNull    (OICPNS.MobileAuthorization + "GetNewSession",
                                                                                      s => s == "true"
                                                                                               ? new Boolean?(true)
                                                                                               : s == "false"
                                                                                                     ? new Boolean?(false)
                                                                                                     : null),

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);


                if (CustomMobileAuthorizeStartRequestParser != null)
                    MobileAuthorizeStart = CustomMobileAuthorizeStartRequestParser(MobileAuthorizeStartXML,
                                                                                   MobileAuthorizeStart);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, MobileAuthorizeStartXML, e);

                MobileAuthorizeStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(MobileAuthorizeStartText, out MobileAuthorizeStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileAuthorizeStartText">The text to parse.</param>
        /// <param name="MobileAuthorizeStart">The parsed mobile authorize start request.</param>
        /// <param name="CustomMobileAuthorizeStartRequestParser">A delegate to parse custom MobileAuthorizeStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                MobileAuthorizeStartText,
                                       out MobileAuthorizeStartRequest                       MobileAuthorizeStart,
                                       CustomXMLParserDelegate<MobileAuthorizeStartRequest>  CustomMobileAuthorizeStartRequestParser   = null,
                                       OnExceptionDelegate                                   OnException                               = null,

                                       DateTime?                                             Timestamp                                 = null,
                                       CancellationToken?                                    CancellationToken                         = null,
                                       EventTracking_Id                                      EventTrackingId                           = null,
                                       TimeSpan?                                             RequestTimeout                            = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(MobileAuthorizeStartText).Root,
                             out MobileAuthorizeStart,
                             CustomMobileAuthorizeStartRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, MobileAuthorizeStartText, e);
            }

            MobileAuthorizeStart = null;
            return false;

        }

        #endregion

        #region ToXML(CustomMobileAuthorizeStartRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomMobileAuthorizeStartRequestSerializer">A delegate to serialize custom MobileAuthorizeStart XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<MobileAuthorizeStartRequest>  CustomMobileAuthorizeStartRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart",

                                      new XElement(OICPNS.MobileAuthorization + "EvseID", EVSEId.ToString()),

                                      QRCodeIdentification.ToXML(OICPNS.MobileAuthorization + "QRCodeIdentification"),

                                      PartnerProductId.HasValue
                                          ? new XElement(OICPNS.MobileAuthorization + "PartnerProductID",  PartnerProductId.ToString())
                                          : null,

                                      (GetNewSession.HasValue)
                                          ? new XElement(OICPNS.MobileAuthorization + "GetNewSession",     GetNewSession.Value ? "true" : "false")
                                          : null

                                 );

            return CustomMobileAuthorizeStartRequestSerializer != null
                       ? CustomMobileAuthorizeStartRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (MobileAuthorizeStartRequest1, MobileAuthorizeStartRequest2)

        /// <summary>
        /// Compares two mobile authorize start requests for equality.
        /// </summary>
        /// <param name="MobileAuthorizeStartRequest1">An mobile authorize start request.</param>
        /// <param name="MobileAuthorizeStartRequest2">Another mobile authorize start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (MobileAuthorizeStartRequest MobileAuthorizeStartRequest1, MobileAuthorizeStartRequest MobileAuthorizeStartRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(MobileAuthorizeStartRequest1, MobileAuthorizeStartRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) MobileAuthorizeStartRequest1 == null) || ((Object) MobileAuthorizeStartRequest2 == null))
                return false;

            return MobileAuthorizeStartRequest1.Equals(MobileAuthorizeStartRequest2);

        }

        #endregion

        #region Operator != (MobileAuthorizeStartRequest1, MobileAuthorizeStartRequest2)

        /// <summary>
        /// Compares two mobile authorize start requests for inequality.
        /// </summary>
        /// <param name="MobileAuthorizeStartRequest1">An mobile authorize start request.</param>
        /// <param name="MobileAuthorizeStartRequest2">Another mobile authorize start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (MobileAuthorizeStartRequest MobileAuthorizeStartRequest1, MobileAuthorizeStartRequest MobileAuthorizeStartRequest2)

            => !(MobileAuthorizeStartRequest1 == MobileAuthorizeStartRequest2);

        #endregion

        #endregion

        #region IEquatable<MobileAuthorizeStartRequest> Members

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

            var MobileAuthorizeStart = Object as MobileAuthorizeStartRequest;
            if ((Object) MobileAuthorizeStart == null)
                return false;

            return Equals(MobileAuthorizeStart);

        }

        #endregion

        #region Equals(MobileAuthorizeStartRequest)

        /// <summary>
        /// Compares two mobile authorize start requests for equality.
        /// </summary>
        /// <param name="MobileAuthorizeStartRequest">A mobile authorize start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(MobileAuthorizeStartRequest MobileAuthorizeStartRequest)
        {

            if ((Object) MobileAuthorizeStartRequest == null)
                return false;

            return EVSEId.              Equals(MobileAuthorizeStartRequest.EVSEId)                       &&
                   QRCodeIdentification.Equals(MobileAuthorizeStartRequest.QRCodeIdentification)         &&

                   ((!PartnerProductId. HasValue && !MobileAuthorizeStartRequest.PartnerProductId.HasValue) ||
                     (PartnerProductId. HasValue &&  MobileAuthorizeStartRequest.PartnerProductId.HasValue && PartnerProductId.Value.Equals(MobileAuthorizeStartRequest.PartnerProductId.Value))) &&

                   ((!GetNewSession.    HasValue && !MobileAuthorizeStartRequest.GetNewSession.   HasValue) ||
                     (GetNewSession.    HasValue &&  MobileAuthorizeStartRequest.GetNewSession.   HasValue && GetNewSession.   Value.Equals(MobileAuthorizeStartRequest.GetNewSession.   Value)));

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

                return EVSEId.              GetHashCode() * 7 ^
                       QRCodeIdentification.GetHashCode() * 5 ^

                       (PartnerProductId.HasValue
                            ? PartnerProductId.GetHashCode() * 3
                            : 0) ^

                       (!GetNewSession.HasValue
                            ? GetNewSession.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(QRCodeIdentification,
                             " at ", EVSEId,

                             PartnerProductId.HasValue
                                 ? " consuming " + PartnerProductId
                                 : "",

                             GetNewSession.HasValue
                                 ? GetNewSession.Value ? " (NewSession)" : ""
                                 : "");

        #endregion


    }

}
