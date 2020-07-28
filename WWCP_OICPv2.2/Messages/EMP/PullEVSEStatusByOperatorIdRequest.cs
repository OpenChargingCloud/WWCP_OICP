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
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// An OICP pull EVSE status by operator id request.
    /// </summary>
    public class PullEVSEStatusByOperatorIdRequest : ARequest<PullEVSEStatusByOperatorIdRequest>
    {

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id               ProviderId     { get; }

        /// <summary>
        /// The enumeration of operator identifications to query.
        /// </summary>
        public IEnumerable<Operator_Id>  OperatorIds    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PullEVSEStatusByOperatorId XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OperatorIds">An enumeration of up to 100 operator identifications to query.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PullEVSEStatusByOperatorIdRequest(Provider_Id               ProviderId,
                                                 IEnumerable<Operator_Id>  OperatorIds,

                                                 DateTime?                 Timestamp           = null,
                                                 CancellationToken?        CancellationToken   = null,
                                                 EventTracking_Id          EventTrackingId     = null,
                                                 TimeSpan?                 RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),   "The given e-mobility provider identification must not be null!");

            if (OperatorIds.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(OperatorIds),  "The given enumeration of operator identifications must not be null!");

            #endregion

            this.ProviderId   = ProviderId;
            this.OperatorIds  = OperatorIds;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/v2.1">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <EVSEStatus:eRoamingPullEvseStatusByOperatorID>
        //
        //          <EVSEStatus:ProviderID>?</EVSEStatus:ProviderID>
        //
        //          <!--1 to 100 repetitions:-->
        //          <EVSEStatus:OperatorID>?</EVSEStatus:OperatorID>
        //
        //       </EVSEStatus:eRoamingPullEvseStatusByOperatorId>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PullEVSEStatusByOperatorIdXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP pull EVSE status by id request.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorIdXML">The XML to parse.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullEVSEStatusByOperatorIdRequest Parse(XElement                                                    PullEVSEStatusByOperatorIdXML,
                                                              CustomXMLParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null,
                                                              OnExceptionDelegate                                         OnException                                     = null,

                                                              DateTime?                                                   Timestamp                                       = null,
                                                              CancellationToken?                                          CancellationToken                               = null,
                                                              EventTracking_Id                                            EventTrackingId                                 = null,
                                                              TimeSpan?                                                   RequestTimeout                                  = null)

        {

            if (TryParse(PullEVSEStatusByOperatorIdXML,
                         out PullEVSEStatusByOperatorIdRequest pullEVSEStatusByOperatorId,
                         CustomPullEVSEStatusByOperatorIdRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return pullEVSEStatusByOperatorId;
            }

            return null;

        }

        #endregion

        #region (static) Parse(PullEVSEStatusByOperatorIdText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP pull EVSE status by id request.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorIdText">The text to parse.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullEVSEStatusByOperatorIdRequest Parse(String                                                      PullEVSEStatusByOperatorIdText,
                                                              CustomXMLParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null,
                                                              OnExceptionDelegate                                         OnException                                     = null,

                                                              DateTime?                                                   Timestamp                                       = null,
                                                              CancellationToken?                                          CancellationToken                               = null,
                                                              EventTracking_Id                                            EventTrackingId                                 = null,
                                                              TimeSpan?                                                   RequestTimeout                                  = null)

        {

            if (TryParse(PullEVSEStatusByOperatorIdText,
                         out PullEVSEStatusByOperatorIdRequest pullEVSEStatusByOperatorId,
                         CustomPullEVSEStatusByOperatorIdRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))
            {
                return pullEVSEStatusByOperatorId;
            }

            return null;

        }

        #endregion

        #region (static) TryParse(PullEVSEStatusByOperatorIdXML,  out PullEVSEStatusByOperatorId, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP pull EVSE status by id request.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorIdXML">The XML to parse.</param>
        /// <param name="PullEVSEStatusByOperatorId">The parsed pull EVSE status by id request.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                                    PullEVSEStatusByOperatorIdXML,
                                       out PullEVSEStatusByOperatorIdRequest                       PullEVSEStatusByOperatorId,
                                       CustomXMLParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null,
                                       OnExceptionDelegate                                         OnException                                     = null,

                                       DateTime?                                                   Timestamp                                       = null,
                                       CancellationToken?                                          CancellationToken                               = null,
                                       EventTracking_Id                                            EventTrackingId                                 = null,
                                       TimeSpan?                                                   RequestTimeout                                  = null)

        {

            try
            {

                if (PullEVSEStatusByOperatorIdXML.Name != OICPNS.EVSEStatus + "eRoamingPullEvseStatusByOperatorId")
                {
                    PullEVSEStatusByOperatorId = null;
                    return false;
                }

                PullEVSEStatusByOperatorId = new PullEVSEStatusByOperatorIdRequest(PullEVSEStatusByOperatorIdXML.MapValueOrFail (OICPNS.EVSEStatus + "ProviderID",
                                                                                                                                 Provider_Id.Parse),

                                                                                   PullEVSEStatusByOperatorIdXML.MapValuesOrFail(OICPNS.EVSEStatus + "OperatorID",
                                                                                                                                 Operator_Id.Parse),

                                                                                   Timestamp,
                                                                                   CancellationToken,
                                                                                   EventTrackingId,
                                                                                   RequestTimeout);


                if (CustomPullEVSEStatusByOperatorIdRequestParser != null)
                    PullEVSEStatusByOperatorId = CustomPullEVSEStatusByOperatorIdRequestParser(PullEVSEStatusByOperatorIdXML,
                                                                               PullEVSEStatusByOperatorId);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PullEVSEStatusByOperatorIdXML, e);

                PullEVSEStatusByOperatorId = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PullEVSEStatusByOperatorIdText, out PullEVSEStatusByOperatorId, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP pull EVSE status by id request.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorIdText">The text to parse.</param>
        /// <param name="PullEVSEStatusByOperatorId">The parsed pull EVSE status by id request.</param>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestParser">A delegate to parse custom PullEVSEStatusByOperatorId requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                                      PullEVSEStatusByOperatorIdText,
                                       out PullEVSEStatusByOperatorIdRequest                       PullEVSEStatusByOperatorId,
                                       CustomXMLParserDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestParser   = null,
                                       OnExceptionDelegate                                         OnException                                     = null,

                                       DateTime?                                                   Timestamp                                       = null,
                                       CancellationToken?                                          CancellationToken                               = null,
                                       EventTracking_Id                                            EventTrackingId                                 = null,
                                       TimeSpan?                                                   RequestTimeout                                  = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(PullEVSEStatusByOperatorIdText).Root,
                             out PullEVSEStatusByOperatorId,
                             CustomPullEVSEStatusByOperatorIdRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PullEVSEStatusByOperatorIdText, e);
            }

            PullEVSEStatusByOperatorId = null;
            return false;

        }

        #endregion

        #region ToXML(CustomPullEVSEStatusByOperatorIdRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusByOperatorIdRequestSerializer">A delegate to serialize custom eRoamingPullEvseStatusByOperatorId XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEStatusByOperatorIdRequest>  CustomPullEVSEStatusByOperatorIdRequestSerializer  = null)
        {

            var XML = new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatusByOperatorID",

                                   new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                   OperatorIds.SafeSelect(evseid => new XElement(OICPNS.EVSEStatus + "OperatorID", evseid.ToString()))

                                  );

            return CustomPullEVSEStatusByOperatorIdRequestSerializer != null
                       ? CustomPullEVSEStatusByOperatorIdRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorId2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorId1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByOperatorId2">Another pull EVSE status by id request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorId2))
                return true;

            // If one is null, but not both, return false.
            if ((PullEVSEStatusByOperatorId1 is null) || (PullEVSEStatusByOperatorId2 is null))
                return false;

            return PullEVSEStatusByOperatorId1.Equals(PullEVSEStatusByOperatorId2);

        }

        #endregion

        #region Operator != (PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorId2)

        /// <summary>
        /// Compares two pull EVSE status by id requests for inequality.
        /// </summary>
        /// <param name="PullEVSEStatusByOperatorId1">An pull EVSE status by id request.</param>
        /// <param name="PullEVSEStatusByOperatorId2">Another pull EVSE status by id request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId1, PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorId2)

            => !(PullEVSEStatusByOperatorId1 == PullEVSEStatusByOperatorId2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatusByOperatorId> Members

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

            if (!(Object is PullEVSEStatusByOperatorIdRequest PullEVSEStatusByOperatorIdRequest))
                return false;

            return Equals(PullEVSEStatusByOperatorIdRequest);

        }

        #endregion

        #region Equals(PullEVSEStatus)

        /// <summary>
        /// Compares two pull EVSE status by id requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatus">An pull EVSE status by id request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusByOperatorIdRequest PullEVSEStatus)
        {

            if (PullEVSEStatus is null)
                return false;

            return ProviderId.         Equals(PullEVSEStatus.ProviderId) &&
                   OperatorIds.Count().Equals(PullEVSEStatus.OperatorIds.Count());

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

                return ProviderId.GetHashCode() * 5 ^
                       OperatorIds.   GetHashCode() * 3;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorIds.Count(),
                             " (", ProviderId, ")");

        #endregion


    }

}
