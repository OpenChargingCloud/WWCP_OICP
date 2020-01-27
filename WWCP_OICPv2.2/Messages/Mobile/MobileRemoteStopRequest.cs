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
    /// An OICP mobile remote stop request.
    /// </summary>
    public class MobileRemoteStopRequest : ARequest<MobileRemoteStopRequest>
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public Session_Id  SessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new MobileRemoteStop request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public MobileRemoteStopRequest(Session_Id          SessionId,

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
        //       <MobileAuthorization:eRoamingMobileRemoteStop>
        //
        //          <MobileAuthorization:SessionID>de164e08-1c88-1293-537b-be355041070e</MobileAuthorization:SessionID>
        //
        //       </MobileAuthorization:eRoamingMobileRemoteStop>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(MobileRemoteStopXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP mobile remote stop request.
        /// </summary>
        /// <param name="MobileRemoteStopXML">The XML to parse.</param>
        /// <param name="CustomMobileRemoteStopRequestParser">A delegate to parse custom MobileRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static MobileRemoteStopRequest

            Parse(XElement                                          MobileRemoteStopXML,
                  CustomXMLParserDelegate<MobileRemoteStopRequest>  CustomMobileRemoteStopRequestParser   = null,
                  OnExceptionDelegate                               OnException                           = null,

                  DateTime?                                         Timestamp                             = null,
                  CancellationToken?                                CancellationToken                     = null,
                  EventTracking_Id                                  EventTrackingId                       = null,
                  TimeSpan?                                         RequestTimeout                        = null)

        {

            if (TryParse(MobileRemoteStopXML,
                         out MobileRemoteStopRequest _MobileRemoteStop,
                         CustomMobileRemoteStopRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _MobileRemoteStop;

            return null;

        }

        #endregion

        #region (static) Parse(MobileRemoteStopText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP mobile remote stop request.
        /// </summary>
        /// <param name="MobileRemoteStopText">The text to parse.</param>
        /// <param name="CustomMobileRemoteStopRequestParser">A delegate to parse custom MobileRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static MobileRemoteStopRequest

            Parse(String                                            MobileRemoteStopText,
                  CustomXMLParserDelegate<MobileRemoteStopRequest>  CustomMobileRemoteStopRequestParser   = null,
                  OnExceptionDelegate                               OnException                           = null,

                  DateTime?                                         Timestamp                             = null,
                  CancellationToken?                                CancellationToken                     = null,
                  EventTracking_Id                                  EventTrackingId                       = null,
                  TimeSpan?                                         RequestTimeout                        = null)

        {

            if (TryParse(MobileRemoteStopText,
                         out MobileRemoteStopRequest _MobileRemoteStop,
                         CustomMobileRemoteStopRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _MobileRemoteStop;

            return null;

        }

        #endregion

        #region (static) TryParse(MobileRemoteStopXML,  out MobileRemoteStop, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP mobile remote stop request.
        /// </summary>
        /// <param name="MobileRemoteStopXML">The XML to parse.</param>
        /// <param name="MobileRemoteStop">The parsed mobile remote stop request.</param>
        /// <param name="CustomMobileRemoteStopRequestParser">A delegate to parse custom MobileRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                          MobileRemoteStopXML,
                                       out MobileRemoteStopRequest                       MobileRemoteStop,
                                       CustomXMLParserDelegate<MobileRemoteStopRequest>  CustomMobileRemoteStopRequestParser   = null,
                                       OnExceptionDelegate                               OnException                           = null,

                                       DateTime?                                         Timestamp                             = null,
                                       CancellationToken?                                CancellationToken                     = null,
                                       EventTracking_Id                                  EventTrackingId                       = null,
                                       TimeSpan?                                         RequestTimeout                        = null)
        {

            try
            {

                if (MobileRemoteStopXML.Name != OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop")
                {
                    MobileRemoteStop = null;
                    return false;
                }

                MobileRemoteStop = new MobileRemoteStopRequest(

                                       MobileRemoteStopXML.MapValueOrFail(OICPNS.MobileAuthorization + "SessionID",
                                                                           Session_Id.Parse),

                                       Timestamp,
                                       CancellationToken,
                                       EventTrackingId,
                                       RequestTimeout);


                if (CustomMobileRemoteStopRequestParser != null)
                    MobileRemoteStop = CustomMobileRemoteStopRequestParser(MobileRemoteStopXML,
                                                                             MobileRemoteStop);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, MobileRemoteStopXML, e);

                MobileRemoteStop = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(MobileRemoteStopText, out MobileRemoteStop, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP mobile remote stop request.
        /// </summary>
        /// <param name="MobileRemoteStopText">The text to parse.</param>
        /// <param name="MobileRemoteStop">The parsed mobile remote stop request.</param>
        /// <param name="CustomMobileRemoteStopRequestParser">A delegate to parse custom MobileRemoteStop requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                            MobileRemoteStopText,
                                       out MobileRemoteStopRequest                       MobileRemoteStop,
                                       CustomXMLParserDelegate<MobileRemoteStopRequest>  CustomMobileRemoteStopRequestParser   = null,
                                       OnExceptionDelegate                               OnException                           = null,

                                       DateTime?                                         Timestamp                             = null,
                                       CancellationToken?                                CancellationToken                     = null,
                                       EventTracking_Id                                  EventTrackingId                       = null,
                                       TimeSpan?                                         RequestTimeout                        = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(MobileRemoteStopText).Root,
                             out MobileRemoteStop,
                             CustomMobileRemoteStopRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, MobileRemoteStopText, e);
            }

            MobileRemoteStop = null;
            return false;

        }

        #endregion

        #region ToXML(CustomMobileRemoteStopRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomMobileRemoteStopRequestSerializer">A delegate to serialize custom MobileRemoteStop XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<MobileRemoteStopRequest>  CustomMobileRemoteStopRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop",

                                      new XElement(OICPNS.MobileAuthorization + "SessionID", SessionId.ToString())

                                  );

            return CustomMobileRemoteStopRequestSerializer != null
                       ? CustomMobileRemoteStopRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (MobileRemoteStopRequest1, MobileRemoteStopRequest2)

        /// <summary>
        /// Compares two mobile remote stop requests for equality.
        /// </summary>
        /// <param name="MobileRemoteStopRequest1">An mobile remote stop request.</param>
        /// <param name="MobileRemoteStopRequest2">Another mobile remote stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (MobileRemoteStopRequest MobileRemoteStopRequest1, MobileRemoteStopRequest MobileRemoteStopRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(MobileRemoteStopRequest1, MobileRemoteStopRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) MobileRemoteStopRequest1 == null) || ((Object) MobileRemoteStopRequest2 == null))
                return false;

            return MobileRemoteStopRequest1.Equals(MobileRemoteStopRequest2);

        }

        #endregion

        #region Operator != (MobileRemoteStopRequest1, MobileRemoteStopRequest2)

        /// <summary>
        /// Compares two mobile remote stop requests for inequality.
        /// </summary>
        /// <param name="MobileRemoteStopRequest1">An mobile remote stop request.</param>
        /// <param name="MobileRemoteStopRequest2">Another mobile remote stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (MobileRemoteStopRequest MobileRemoteStopRequest1, MobileRemoteStopRequest MobileRemoteStopRequest2)

            => !(MobileRemoteStopRequest1 == MobileRemoteStopRequest2);

        #endregion

        #endregion

        #region IEquatable<MobileRemoteStopRequest> Members

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

            var MobileRemoteStop = Object as MobileRemoteStopRequest;
            if ((Object) MobileRemoteStop == null)
                return false;

            return Equals(MobileRemoteStop);

        }

        #endregion

        #region Equals(MobileRemoteStopRequest)

        /// <summary>
        /// Compares two mobile remote stop requests for equality.
        /// </summary>
        /// <param name="MobileRemoteStopRequest">A mobile remote stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(MobileRemoteStopRequest MobileRemoteStopRequest)
        {

            if ((Object) MobileRemoteStopRequest == null)
                return false;

            return SessionId.Equals(MobileRemoteStopRequest.SessionId);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => SessionId.ToString();

        #endregion


    }

}
