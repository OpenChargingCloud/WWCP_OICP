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

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// An OICP pull authentication data request.
    /// </summary>
    public class PullAuthenticationDataRequest : ARequest<PullAuthenticationDataRequest>
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the charging station operator.
        /// </summary>
        public Operator_Id  OperatorId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PullAuthenticationData XML/SOAP request.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PullAuthenticationDataRequest(Operator_Id         OperatorId,

                                             DateTime?           Timestamp           = null,
                                             CancellationToken?  CancellationToken   = null,
                                             EventTracking_Id    EventTrackingId     = null,
                                             TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.OperatorId  = OperatorId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/EVSEData.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <AuthenticationData:eRoamingPullAuthenticationData>
        //
        //          <AuthenticationData:OperatorID>DE*GEF</AuthenticationData:OperatorID>
        //
        //       </AuthenticationData:eRoamingPullAuthenticationData>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PullAuthenticationDataXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP pull authentication data request.
        /// </summary>
        /// <param name="PullAuthenticationDataXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullAuthenticationDataRequest

            Parse(XElement                                                PullAuthenticationDataXML,
                  CustomXMLParserDelegate<PullAuthenticationDataRequest>  CustomPullAuthenticationDataRequestParser   = null,
                  OnExceptionDelegate                                     OnException                                 = null,

                  DateTime?                                               Timestamp                                   = null,
                  CancellationToken?                                      CancellationToken                           = null,
                  EventTracking_Id                                        EventTrackingId                             = null,
                  TimeSpan?                                               RequestTimeout                              = null)

        {

            if (TryParse(PullAuthenticationDataXML,
                         out PullAuthenticationDataRequest _PullAuthenticationData,
                         CustomPullAuthenticationDataRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PullAuthenticationData;

            return null;

        }

        #endregion

        #region (static) Parse(PullAuthenticationDataText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP pull authentication data request.
        /// </summary>
        /// <param name="PullAuthenticationDataText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullAuthenticationDataRequest

            Parse(String                                                  PullAuthenticationDataText,
                  CustomXMLParserDelegate<PullAuthenticationDataRequest>  CustomPullAuthenticationDataRequestParser   = null,
                  OnExceptionDelegate                                     OnException                                 = null,

                  DateTime?                                               Timestamp                                   = null,
                  CancellationToken?                                      CancellationToken                           = null,
                  EventTracking_Id                                        EventTrackingId                             = null,
                  TimeSpan?                                               RequestTimeout                              = null)

        {

            if (TryParse(PullAuthenticationDataText,
                         out PullAuthenticationDataRequest _PullAuthenticationData,
                         CustomPullAuthenticationDataRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PullAuthenticationData;

            return null;

        }

        #endregion

        #region (static) TryParse(PullAuthenticationDataXML,  out PullAuthenticationData, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP pull authentication data request.
        /// </summary>
        /// <param name="PullAuthenticationDataXML">The XML to parse.</param>
        /// <param name="PullAuthenticationData">The parsed pull authentication data request.</param>
        /// <param name="CustomPullAuthenticationDataRequestParser">A delegate to customize the deserialization of PullAuthenticationDataRequest requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                PullAuthenticationDataXML,
                                       out PullAuthenticationDataRequest                       PullAuthenticationData,
                                       CustomXMLParserDelegate<PullAuthenticationDataRequest>  CustomPullAuthenticationDataRequestParser   = null,
                                       OnExceptionDelegate                                     OnException                                 = null,

                                       DateTime?                                               Timestamp                                   = null,
                                       CancellationToken?                                      CancellationToken                           = null,
                                       EventTracking_Id                                        EventTrackingId                             = null,
                                       TimeSpan?                                               RequestTimeout                              = null)

        {

            try
            {

                if (PullAuthenticationDataXML.Name != OICPNS.AuthenticationData + "eRoamingPullAuthenticationData")
                {
                    PullAuthenticationData = null;
                    return false;
                }

                PullAuthenticationData = new PullAuthenticationDataRequest(

                                             PullAuthenticationDataXML.MapValueOrFail(OICPNS.EVSEStatus + "OperatorEvseStatus",
                                                                                      Operator_Id.Parse),

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout

                                         );


                if (CustomPullAuthenticationDataRequestParser != null)
                    PullAuthenticationData = CustomPullAuthenticationDataRequestParser(PullAuthenticationDataXML,
                                                                                       PullAuthenticationData);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PullAuthenticationDataXML, e);

                PullAuthenticationData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PullAuthenticationDataText, out PullAuthenticationData, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP pull authentication data request.
        /// </summary>
        /// <param name="PullAuthenticationDataText">The text to parse.</param>
        /// <param name="PullAuthenticationData">The parsed pull authentication data request.</param>
        /// 
        /// 
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                  PullAuthenticationDataText,
                                       out PullAuthenticationDataRequest                       PullAuthenticationData,
                                       CustomXMLParserDelegate<PullAuthenticationDataRequest>  CustomPullAuthenticationDataRequestParser   = null,
                                       OnExceptionDelegate                                     OnException                                 = null,

                                       DateTime?                                               Timestamp                                   = null,
                                       CancellationToken?                                      CancellationToken                           = null,
                                       EventTracking_Id                                        EventTrackingId                             = null,
                                       TimeSpan?                                               RequestTimeout                              = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(PullAuthenticationDataText).Root,
                             out PullAuthenticationData,
                             CustomPullAuthenticationDataRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PullAuthenticationDataText, e);
            }

            PullAuthenticationData = null;
            return false;

        }

        #endregion

        #region ToXML(CustomPullAuthenticationDataRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPullAuthenticationDataRequestSerializer">A delegate to customize the serialization of PullAuthenticationData requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PullAuthenticationDataRequest> CustomPullAuthenticationDataRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.AuthenticationData + "eRoamingPullAuthenticationData",

                          new XElement(OICPNS.AuthenticationData + "OperatorID", OperatorId.ToString())

                      );

            return CustomPullAuthenticationDataRequestSerializer != null
                       ? CustomPullAuthenticationDataRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullAuthenticationData1, PullAuthenticationData2)

        /// <summary>
        /// Compares two pull authentication data requests for equality.
        /// </summary>
        /// <param name="PullAuthenticationData1">An pull authentication data request.</param>
        /// <param name="PullAuthenticationData2">Another pull authentication data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullAuthenticationDataRequest PullAuthenticationData1, PullAuthenticationDataRequest PullAuthenticationData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullAuthenticationData1, PullAuthenticationData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PullAuthenticationData1 == null) || ((Object) PullAuthenticationData2 == null))
                return false;

            return PullAuthenticationData1.Equals(PullAuthenticationData2);

        }

        #endregion

        #region Operator != (PullAuthenticationData1, PullAuthenticationData2)

        /// <summary>
        /// Compares two pull authentication data requests for inequality.
        /// </summary>
        /// <param name="PullAuthenticationData1">An pull authentication data request.</param>
        /// <param name="PullAuthenticationData2">Another pull authentication data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullAuthenticationDataRequest PullAuthenticationData1, PullAuthenticationDataRequest PullAuthenticationData2)

            => !(PullAuthenticationData1 == PullAuthenticationData2);

        #endregion

        #endregion

        #region IEquatable<PullAuthenticationData> Members

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

            var PullAuthenticationData = Object as PullAuthenticationDataRequest;
            if ((Object) PullAuthenticationData == null)
                return false;

            return this.Equals(PullAuthenticationData);

        }

        #endregion

        #region Equals(PullAuthenticationData)

        /// <summary>
        /// Compares two pull authentication data requests for equality.
        /// </summary>
        /// <param name="PullAuthenticationData">An pull authentication data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullAuthenticationDataRequest PullAuthenticationData)
        {

            if ((Object) PullAuthenticationData == null)
                return false;

            return OperatorId.Equals(PullAuthenticationData.OperatorId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => OperatorId.ToString();

        #endregion


    }

}
