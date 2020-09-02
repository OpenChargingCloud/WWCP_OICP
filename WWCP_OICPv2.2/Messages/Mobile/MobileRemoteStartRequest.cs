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

namespace org.GraphDefined.WWCP.OICPv2_2.Mobile
{

    /// <summary>
    /// An OICP mobile authorize start request.
    /// </summary>
    public class MobileRemoteStartRequest : ARequest<MobileRemoteStartRequest>
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public Session_Id  SessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new MobileRemoteStart request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public MobileRemoteStartRequest(Session_Id          SessionId,

                                        DateTime?           Timestamp           = null,
                                        CancellationToken?  CancellationToken   = null,
                                        EventTracking_Id    EventTrackingId     = null,
                                        TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.SessionId  = SessionId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv              = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:MobileAuthorization  = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <MobileAuthorization:eRoamingMobileRemoteStart>
        //
        //          <MobileAuthorization:SessionID>de164e08-1c88-1293-537b-be355041070e</MobileAuthorization:SessionID>
        //
        //       </MobileAuthorization:eRoamingMobileRemoteStart>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (MobileRemoteStartXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileRemoteStartXML">The XML to parse.</param>
        /// <param name="CustomMobileRemoteStartRequestParser">A delegate to parse custom MobileRemoteStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static MobileRemoteStartRequest

            Parse(XElement                                           MobileRemoteStartXML,
                  CustomXMLParserDelegate<MobileRemoteStartRequest>  CustomMobileRemoteStartRequestParser   = null,
                  OnExceptionDelegate                                OnException                            = null,

                  DateTime?                                          Timestamp                              = null,
                  CancellationToken?                                 CancellationToken                      = null,
                  EventTracking_Id                                   EventTrackingId                        = null,
                  TimeSpan?                                          RequestTimeout                         = null)

        {

            if (TryParse(MobileRemoteStartXML,
                         out MobileRemoteStartRequest _MobileRemoteStart,
                         CustomMobileRemoteStartRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _MobileRemoteStart;

            return null;

        }

        #endregion

        #region (static) Parse   (MobileRemoteStartText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text-representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileRemoteStartText">The text to parse.</param>
        /// <param name="CustomMobileRemoteStartRequestParser">A delegate to parse custom MobileRemoteStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static MobileRemoteStartRequest

            Parse(String                                             MobileRemoteStartText,
                  CustomXMLParserDelegate<MobileRemoteStartRequest>  CustomMobileRemoteStartRequestParser   = null,
                  OnExceptionDelegate                                OnException                            = null,

                  DateTime?                                          Timestamp                              = null,
                  CancellationToken?                                 CancellationToken                      = null,
                  EventTracking_Id                                   EventTrackingId                        = null,
                  TimeSpan?                                          RequestTimeout                         = null)

        {

            if (TryParse(MobileRemoteStartText,
                         out MobileRemoteStartRequest _MobileRemoteStart,
                         CustomMobileRemoteStartRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _MobileRemoteStart;

            return null;

        }

        #endregion

        #region (static) TryParse(MobileRemoteStartXML,  out MobileRemoteStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileRemoteStartXML">The XML to parse.</param>
        /// <param name="MobileRemoteStart">The parsed mobile authorize start request.</param>
        /// <param name="CustomMobileRemoteStartRequestParser">A delegate to parse custom MobileRemoteStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                           MobileRemoteStartXML,
                                       out MobileRemoteStartRequest                       MobileRemoteStart,
                                       CustomXMLParserDelegate<MobileRemoteStartRequest>  CustomMobileRemoteStartRequestParser   = null,
                                       OnExceptionDelegate                                OnException                            = null,

                                       DateTime?                                          Timestamp                              = null,
                                       CancellationToken?                                 CancellationToken                      = null,
                                       EventTracking_Id                                   EventTrackingId                        = null,
                                       TimeSpan?                                          RequestTimeout                         = null)

        {

            try
            {

                if (MobileRemoteStartXML.Name != OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart")
                {
                    MobileRemoteStart = null;
                    return false;
                }

                MobileRemoteStart = new MobileRemoteStartRequest(

                                        MobileRemoteStartXML.MapValueOrFail(OICPNS.MobileAuthorization + "SessionID",
                                                                            Session_Id.Parse),

                                        Timestamp,
                                        CancellationToken,
                                        EventTrackingId,
                                        RequestTimeout);


                if (CustomMobileRemoteStartRequestParser != null)
                    MobileRemoteStart = CustomMobileRemoteStartRequestParser(MobileRemoteStartXML,
                                                                             MobileRemoteStart);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, MobileRemoteStartXML, e);

                MobileRemoteStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(MobileRemoteStartText, out MobileRemoteStart, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text-representation of an OICP mobile authorize start request.
        /// </summary>
        /// <param name="MobileRemoteStartText">The text to parse.</param>
        /// <param name="MobileRemoteStart">The parsed mobile authorize start request.</param>
        /// <param name="CustomMobileRemoteStartRequestParser">A delegate to parse custom MobileRemoteStart requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                             MobileRemoteStartText,
                                       out MobileRemoteStartRequest                       MobileRemoteStart,
                                       CustomXMLParserDelegate<MobileRemoteStartRequest>  CustomMobileRemoteStartRequestParser   = null,
                                       OnExceptionDelegate                                OnException                            = null,

                                       DateTime?                                          Timestamp                              = null,
                                       CancellationToken?                                 CancellationToken                      = null,
                                       EventTracking_Id                                   EventTrackingId                        = null,
                                       TimeSpan?                                          RequestTimeout                         = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(MobileRemoteStartText).Root,
                             out MobileRemoteStart,
                             CustomMobileRemoteStartRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, MobileRemoteStartText, e);
            }

            MobileRemoteStart = null;
            return false;

        }

        #endregion

        #region ToXML(CustomMobileRemoteStartRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomMobileRemoteStartRequestSerializer">A delegate to serialize custom MobileRemoteStart XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<MobileRemoteStartRequest>  CustomMobileRemoteStartRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart",

                                      new XElement(OICPNS.MobileAuthorization + "SessionID", SessionId.ToString())

                                  );

            return CustomMobileRemoteStartRequestSerializer != null
                       ? CustomMobileRemoteStartRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (MobileRemoteStartRequest1, MobileRemoteStartRequest2)

        /// <summary>
        /// Compares two mobile authorize start requests for equality.
        /// </summary>
        /// <param name="MobileRemoteStartRequest1">An mobile authorize start request.</param>
        /// <param name="MobileRemoteStartRequest2">Another mobile authorize start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (MobileRemoteStartRequest MobileRemoteStartRequest1, MobileRemoteStartRequest MobileRemoteStartRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(MobileRemoteStartRequest1, MobileRemoteStartRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) MobileRemoteStartRequest1 == null) || ((Object) MobileRemoteStartRequest2 == null))
                return false;

            return MobileRemoteStartRequest1.Equals(MobileRemoteStartRequest2);

        }

        #endregion

        #region Operator != (MobileRemoteStartRequest1, MobileRemoteStartRequest2)

        /// <summary>
        /// Compares two mobile authorize start requests for inequality.
        /// </summary>
        /// <param name="MobileRemoteStartRequest1">An mobile authorize start request.</param>
        /// <param name="MobileRemoteStartRequest2">Another mobile authorize start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (MobileRemoteStartRequest MobileRemoteStartRequest1, MobileRemoteStartRequest MobileRemoteStartRequest2)

            => !(MobileRemoteStartRequest1 == MobileRemoteStartRequest2);

        #endregion

        #endregion

        #region IEquatable<MobileRemoteStartRequest> Members

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

            var MobileRemoteStart = Object as MobileRemoteStartRequest;
            if ((Object) MobileRemoteStart == null)
                return false;

            return Equals(MobileRemoteStart);

        }

        #endregion

        #region Equals(MobileRemoteStartRequest)

        /// <summary>
        /// Compares two mobile authorize start requests for equality.
        /// </summary>
        /// <param name="MobileRemoteStartRequest">A mobile authorize start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(MobileRemoteStartRequest MobileRemoteStartRequest)
        {

            if ((Object) MobileRemoteStartRequest == null)
                return false;

            return SessionId.Equals(MobileRemoteStartRequest.SessionId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => SessionId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => SessionId.ToString();

        #endregion


    }

}
