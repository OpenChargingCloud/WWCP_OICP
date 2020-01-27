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
    /// An OICP get charge detail records request.
    /// </summary>
    public class GetChargeDetailRecordsRequest : ARequest<GetChargeDetailRecordsRequest>
    {

        #region Properties

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id   ProviderId   { get; }

        /// <summary>
        /// The starting time.
        /// </summary>
        public DateTime      From         { get; }

        /// <summary>
        /// The end time.
        /// </summary>
        public DateTime      To           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP GetChargeDetailRecords XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">The end time.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public GetChargeDetailRecordsRequest(Provider_Id         ProviderId,
                                             DateTime            From,
                                             DateTime            To,

                                             DateTime?           Timestamp           = null,
                                             CancellationToken?  CancellationToken   = null,
                                             EventTracking_Id    EventTrackingId     = null,
                                             TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.ProviderId  = ProviderId;
            this.From        = From;
            this.To          = To;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingGetChargeDetailRecords>
        //          <Authorization:ProviderID>?</Authorization:ProviderID>
        //          <Authorization:From>?</Authorization:From>
        //          <Authorization:To>?</Authorization:To>
        //       </Authorization:eRoamingGetChargeDetailRecords>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetChargeDetailRecordsRequestXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP get charge detail records request.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequestXML">The XML to parse.</param>
        /// <param name="CustomGetChargeDetailRecordsRequestParser">A delegate to parse custom GetChargeDetailRecords requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static GetChargeDetailRecordsRequest Parse(XElement                                                GetChargeDetailRecordsRequestXML,
                                                          CustomXMLParserDelegate<GetChargeDetailRecordsRequest>  CustomGetChargeDetailRecordsRequestParser   = null,
                                                          OnExceptionDelegate                                     OnException                                 = null,

                                                          DateTime?                                               Timestamp                                   = null,
                                                          CancellationToken?                                      CancellationToken                           = null,
                                                          EventTracking_Id                                        EventTrackingId                             = null,
                                                          TimeSpan?                                               RequestTimeout                              = null)

        {

            if (TryParse(GetChargeDetailRecordsRequestXML,
                         out GetChargeDetailRecordsRequest _GetChargeDetailRecordsRequest,
                         CustomGetChargeDetailRecordsRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _GetChargeDetailRecordsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetChargeDetailRecordsRequestText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP get charge detail records request.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequestText">The text to parse.</param>
        /// <param name="CustomGetChargeDetailRecordsRequestParser">A delegate to parse custom GetChargeDetailRecords requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static GetChargeDetailRecordsRequest Parse(String                                                  GetChargeDetailRecordsRequestText,
                                                          CustomXMLParserDelegate<GetChargeDetailRecordsRequest>  CustomGetChargeDetailRecordsRequestParser   = null,
                                                          OnExceptionDelegate                                     OnException                                 = null,

                                                          DateTime?                                               Timestamp                                   = null,
                                                          CancellationToken?                                      CancellationToken                           = null,
                                                          EventTracking_Id                                        EventTrackingId                             = null,
                                                          TimeSpan?                                               RequestTimeout                              = null)

        {

            if (TryParse(GetChargeDetailRecordsRequestText,
                         out GetChargeDetailRecordsRequest _GetChargeDetailRecordsRequest,
                         CustomGetChargeDetailRecordsRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _GetChargeDetailRecordsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetChargeDetailRecordsRequestXML,  out GetChargeDetailRecordsRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP get charge detail records request.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequestXML">The XML to parse.</param>
        /// <param name="GetChargeDetailRecordsRequest">The parsed get charge detail records request.</param>
        /// <param name="CustomGetChargeDetailRecordsRequestParser">A delegate to parse custom GetChargeDetailRecords requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                GetChargeDetailRecordsRequestXML,
                                       out GetChargeDetailRecordsRequest                       GetChargeDetailRecordsRequest,
                                       CustomXMLParserDelegate<GetChargeDetailRecordsRequest>  CustomGetChargeDetailRecordsRequestParser   = null,
                                       OnExceptionDelegate                                     OnException                                 = null,

                                       DateTime?                                               Timestamp                                   = null,
                                       CancellationToken?                                      CancellationToken                           = null,
                                       EventTracking_Id                                        EventTrackingId                             = null,
                                       TimeSpan?                                               RequestTimeout                              = null)

        {

            try
            {

                GetChargeDetailRecordsRequest = new GetChargeDetailRecordsRequest(

                                                    GetChargeDetailRecordsRequestXML.MapValueOrFail(OICPNS.Authorization + "ProviderID",
                                                                                                    Provider_Id.Parse),

                                                    GetChargeDetailRecordsRequestXML.MapValueOrFail(OICPNS.Authorization + "From",
                                                                                                    DateTime.Parse),

                                                    GetChargeDetailRecordsRequestXML.MapValueOrFail(OICPNS.Authorization + "To",
                                                                                                    DateTime.Parse),

                                                    Timestamp,
                                                    CancellationToken,
                                                    EventTrackingId,
                                                    RequestTimeout);


                if (CustomGetChargeDetailRecordsRequestParser != null)
                    GetChargeDetailRecordsRequest = CustomGetChargeDetailRecordsRequestParser(GetChargeDetailRecordsRequestXML,
                                                                                              GetChargeDetailRecordsRequest);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetChargeDetailRecordsRequestXML, e);

                GetChargeDetailRecordsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetChargeDetailRecordsRequestText, out GetChargeDetailRecordsRequest, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP get charge detail records request.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequestText">The text to parse.</param>
        /// <param name="GetChargeDetailRecordsRequest">The parsed get charge detail records request.</param>
        /// <param name="CustomGetChargeDetailRecordsRequestParser">A delegate to parse custom GetChargeDetailRecords requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                  GetChargeDetailRecordsRequestText,
                                       out GetChargeDetailRecordsRequest                       GetChargeDetailRecordsRequest,
                                       CustomXMLParserDelegate<GetChargeDetailRecordsRequest>  CustomGetChargeDetailRecordsRequestParser   = null,
                                       OnExceptionDelegate                                     OnException                                 = null,

                                       DateTime?                                               Timestamp                                   = null,
                                       CancellationToken?                                      CancellationToken                           = null,
                                       EventTracking_Id                                        EventTrackingId                             = null,
                                       TimeSpan?                                               RequestTimeout                              = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(GetChargeDetailRecordsRequestText).Root,
                             out GetChargeDetailRecordsRequest,
                             CustomGetChargeDetailRecordsRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetChargeDetailRecordsRequestText, e);
            }

            GetChargeDetailRecordsRequest = null;
            return false;

        }

        #endregion

        #region ToXML(CustomGetChargeDetailRecordsRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomGetChargeDetailRecordsRequestSerializer">A delegate to customize the serialization of GetChargeDetailRecords requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<GetChargeDetailRecordsRequest>  CustomGetChargeDetailRecordsRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingGetChargeDetailRecords",
                   new XElement(OICPNS.Authorization + "ProviderID",  ProviderId.ToString()),
                   new XElement(OICPNS.Authorization + "From",        From.      ToIso8601()),
                   new XElement(OICPNS.Authorization + "To",          To.        ToIso8601())
               );

            return CustomGetChargeDetailRecordsRequestSerializer != null
                       ? CustomGetChargeDetailRecordsRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetChargeDetailRecordsRequest1, GetChargeDetailRecordsRequest2)

        /// <summary>
        /// Compares two get charge detail records requests for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequest1">A get charge detail records request.</param>
        /// <param name="GetChargeDetailRecordsRequest2">Another get charge detail records request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargeDetailRecordsRequest GetChargeDetailRecordsRequest1, GetChargeDetailRecordsRequest GetChargeDetailRecordsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetChargeDetailRecordsRequest1, GetChargeDetailRecordsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargeDetailRecordsRequest1 == null) || ((Object) GetChargeDetailRecordsRequest2 == null))
                return false;

            return GetChargeDetailRecordsRequest1.Equals(GetChargeDetailRecordsRequest2);

        }

        #endregion

        #region Operator != (GetChargeDetailRecordsRequest1, GetChargeDetailRecordsRequest2)

        /// <summary>
        /// Compares two get charge detail records requests for inequality.
        /// </summary>
        /// <param name="GetChargeDetailRecordsRequest1">A get charge detail records request.</param>
        /// <param name="GetChargeDetailRecordsRequest2">Another get charge detail records request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargeDetailRecordsRequest GetChargeDetailRecordsRequest1, GetChargeDetailRecordsRequest GetChargeDetailRecordsRequest2)

            => !(GetChargeDetailRecordsRequest1 == GetChargeDetailRecordsRequest2);

        #endregion

        #endregion

        #region IEquatable<GetChargeDetailRecordsRequest> Members

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

            var GetChargeDetailRecordsRequest = Object as GetChargeDetailRecordsRequest;
            if ((Object) GetChargeDetailRecordsRequest == null)
                return false;

            return Equals(GetChargeDetailRecordsRequest);

        }

        #endregion

        #region Equals(GetChargeDetailRecords)

        /// <summary>
        /// Compares two get charge detail records requests for equality.
        /// </summary>
        /// <param name="GetChargeDetailRecords">A get charge detail records request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargeDetailRecordsRequest GetChargeDetailRecords)
        {

            if ((Object) GetChargeDetailRecords == null)
                return false;

            return ProviderId.Equals(GetChargeDetailRecords.ProviderId) &&
                   From.      Equals(GetChargeDetailRecords.From)       &&
                   To.        Equals(GetChargeDetailRecords.To);

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

                return ProviderId.GetHashCode() * 7 ^
                       From.      GetHashCode() * 5 ^
                       To.        GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(From.ToIso8601(), " -> ", To.ToIso8601(),
                             " (", ProviderId, ")");

        #endregion


    }

}
