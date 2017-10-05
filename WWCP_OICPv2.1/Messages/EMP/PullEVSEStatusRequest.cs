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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP pull EVSE status request.
    /// </summary>
    public class PullEVSEStatusRequest : ARequest<PullEVSEStatusRequest>
    {

        #region Properties

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id       ProviderId          { get; }

        /// <summary>
        /// The optional geo coordinate of the search center.
        /// </summary>
        public GeoCoordinate?    SearchCenter        { get; }

        /// <summary>
        /// The optional search distance relative to the search center.
        /// </summary>
        public Single            DistanceKM          { get; }

        /// <summary>
        /// The optional EVSE status as filter criteria.
        /// </summary>
        public EVSEStatusTypes?  EVSEStatusFilter    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PullEVSEStatus XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PullEVSEStatusRequest(Provider_Id         ProviderId,
                                     GeoCoordinate?      SearchCenter        = null,
                                     Single              DistanceKM          = 0f,
                                     EVSEStatusTypes?    EVSEStatusFilter    = null,

                                     DateTime?           Timestamp           = null,
                                     CancellationToken?  CancellationToken   = null,
                                     EventTracking_Id    EventTrackingId     = null,
                                     TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            #endregion

            this.ProviderId        = ProviderId;
            this.SearchCenter      = SearchCenter;
            this.DistanceKM        = DistanceKM;
            this.EVSEStatusFilter  = EVSEStatusFilter;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
        //                   xmlns:CommonTypes         = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <EVSEStatus:eRoamingPullEvseStatus>
        //
        //          <EVSEStatus:ProviderID>DE*GDF</EVSEStatus:ProviderID>
        //
        //          <!--Optional:-->
        //          <EVSEStatus:SearchCenter>
        //
        //             <CommonTypes:GeoCoordinates>
        //                <!--You have a CHOICE of the next 3 items at this level-->
        //
        //                <CommonTypes:Google>
        //                   <!-- latitude longitude: -?1?\d{1,2}\.\d{1,6}\s*\,?\s*-?1?\d{1,2}\.\d{1,6} -->
        //                   <CommonTypes:Coordinates>50.931844 11.625214</CommonTypes:Coordinates>
        //                </CommonTypes:Google>
        //
        //                <CommonTypes:DecimalDegree>
        //                   <!-- -?1?\d{1,2}\.\d{1,6} -->
        //                   <CommonTypes:Longitude>11.625214</CommonTypes:Longitude>
        //                   <CommonTypes:Latitude>50.931844</CommonTypes:Latitude>
        //                </CommonTypes:DecimalDegree>
        //
        //                <CommonTypes:DegreeMinuteSeconds>
        //                   <!-- -?1?\d{1,2}°[ ]?\d{1,2}'[ ]?\d{1,2}\.\d+'' -->
        //                   <CommonTypes:Longitude>?</CommonTypes:Longitude>
        //                   <CommonTypes:Latitude>?</CommonTypes:Latitude>
        //                </CommonTypes:DegreeMinuteSeconds>
        //
        //             </CommonTypes:GeoCoordinates>
        //
        //             <!-- km ####.# is defined, but only #### seems to be accepted -->
        //             <CommonTypes:Radius>100</CommonTypes:Radius>
        //
        //          </EVSEStatus:SearchCenter>
        //
        //          <!--Optional:-->
        //          <EVSEStatus:EvseStatus>Available</EVSEStatus:EvseStatus>
        //
        //       </EVSEStatus:eRoamingPullEvseStatus>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PullEVSEStatusXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP pull EVSE status request.
        /// </summary>
        /// <param name="PullEVSEStatusXML">The XML to parse.</param>
        /// <param name="CustomPullEVSEStatusRequestParser">A delegate to parse custom PullEVSEStatus requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullEVSEStatusRequest Parse(XElement                                        PullEVSEStatusXML,
                                                  CustomXMLParserDelegate<PullEVSEStatusRequest>  CustomPullEVSEStatusRequestParser   = null,
                                                  OnExceptionDelegate                             OnException                         = null,

                                                  DateTime?                                       Timestamp                           = null,
                                                  CancellationToken?                              CancellationToken                   = null,
                                                  EventTracking_Id                                EventTrackingId                     = null,
                                                  TimeSpan?                                       RequestTimeout                      = null)

        {

            if (TryParse(PullEVSEStatusXML,
                         out PullEVSEStatusRequest _PullEVSEStatus,
                         CustomPullEVSEStatusRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PullEVSEStatus;

            return null;

        }

        #endregion

        #region (static) Parse(PullEVSEStatusText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text representation of an OICP pull EVSE status request.
        /// </summary>
        /// <param name="PullEVSEStatusText">The text to parse.</param>
        /// <param name="CustomPullEVSEStatusRequestParser">A delegate to parse custom PullEVSEStatus requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullEVSEStatusRequest Parse(String                                          PullEVSEStatusText,
                                                  CustomXMLParserDelegate<PullEVSEStatusRequest>  CustomPullEVSEStatusRequestParser   = null,
                                                  OnExceptionDelegate                             OnException                         = null,

                                                  DateTime?                                       Timestamp                           = null,
                                                  CancellationToken?                              CancellationToken                   = null,
                                                  EventTracking_Id                                EventTrackingId                     = null,
                                                  TimeSpan?                                       RequestTimeout                      = null)

        {

            if (TryParse(PullEVSEStatusText,
                         out PullEVSEStatusRequest _PullEVSEStatus,
                         CustomPullEVSEStatusRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PullEVSEStatus;

            return null;

        }

        #endregion

        #region (static) TryParse(PullEVSEStatusXML,  out PullEVSEStatus, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP pull EVSE status request.
        /// </summary>
        /// <param name="PullEVSEStatusXML">The XML to parse.</param>
        /// <param name="PullEVSEStatus">The parsed pull EVSE status request.</param>
        /// <param name="CustomPullEVSEStatusRequestParser">A delegate to parse custom PullEVSEStatus requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                        PullEVSEStatusXML,
                                       out PullEVSEStatusRequest                       PullEVSEStatus,
                                       CustomXMLParserDelegate<PullEVSEStatusRequest>  CustomPullEVSEStatusRequestParser   = null,
                                       OnExceptionDelegate                             OnException                         = null,

                                       DateTime?                                       Timestamp                           = null,
                                       CancellationToken?                              CancellationToken                   = null,
                                       EventTracking_Id                                EventTrackingId                     = null,
                                       TimeSpan?                                       RequestTimeout                      = null)

        {

            try
            {

                if (PullEVSEStatusXML.Name != OICPNS.EVSEStatus + "eRoamingPullEvseStatus")
                {
                    PullEVSEStatus = null;
                    return false;
                }

                #region Parse the optional search center

                GeoCoordinate? GeoCoordinates  = null;
                Single         Radius          = 0f;

                var SearchCenterXML = PullEVSEStatusXML.Element(OICPNS.EVSEData + "SearchCenter");
                if (SearchCenterXML != null)
                {

                    #region Parse Google format

                    var GoogleXML = SearchCenterXML.Element(OICPNS.EVSEData + "Google");
                    if (GoogleXML != null)
                    {

                        var GeoArray    = GoogleXML.ElementValueOrFail(OICPNS.CommonTypes + "Coordinates").
                                                    Split(new String[] { " " }, StringSplitOptions.None);

                        GeoCoordinates  = new GeoCoordinate(Latitude. Parse(GeoArray[0]),
                                                            Longitude.Parse(GeoArray[1]));

                    }

                    #endregion

                    #region Parse DecimalDegree format

                    var DecimalDegreeXML = SearchCenterXML.Element(OICPNS.EVSEData + "DecimalDegree");
                    if (DecimalDegreeXML != null)
                    {

                        GeoCoordinates  = new GeoCoordinate(DecimalDegreeXML.MapValueOrFail(OICPNS.CommonTypes + "Latitude",
                                                                                            Latitude.Parse),
                                                            DecimalDegreeXML.MapValueOrFail(OICPNS.CommonTypes + "Longitude",
                                                                                            Longitude.Parse));


                    }

                    #endregion

                    Radius  = SearchCenterXML.MapValueOrFail(OICPNS.CommonTypes + "Radius",
                                                             Single.Parse);

                }

                #endregion

                PullEVSEStatus = new PullEVSEStatusRequest(PullEVSEStatusXML.MapValueOrFail(OICPNS.EVSEStatus + "ProviderID",
                                                                                            Provider_Id.Parse),
                                                           GeoCoordinates,
                                                           Radius,

                                                           PullEVSEStatusXML.MapValueOrFail(OICPNS.EVSEStatus + "EvseStatus",
                                                                                            s => (EVSEStatusTypes)Enum.Parse(typeof(EVSEStatusTypes), s)),

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


                if (CustomPullEVSEStatusRequestParser != null)
                    PullEVSEStatus = CustomPullEVSEStatusRequestParser(PullEVSEStatusXML,
                                                                       PullEVSEStatus);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PullEVSEStatusXML, e);

                PullEVSEStatus = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PullEVSEStatusText, out PullEVSEStatus, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an OICP pull EVSE status request.
        /// </summary>
        /// <param name="PullEVSEStatusText">The text to parse.</param>
        /// <param name="PullEVSEStatus">The parsed pull EVSE status request.</param>
        /// <param name="CustomPullEVSEStatusRequestParser">A delegate to parse custom PullEVSEStatus requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                          PullEVSEStatusText,
                                       out PullEVSEStatusRequest                       PullEVSEStatus,
                                       CustomXMLParserDelegate<PullEVSEStatusRequest>  CustomPullEVSEStatusRequestParser   = null,
                                       OnExceptionDelegate                             OnException                         = null,

                                       DateTime?                                       Timestamp                           = null,
                                       CancellationToken?                              CancellationToken                   = null,
                                       EventTracking_Id                                EventTrackingId                     = null,
                                       TimeSpan?                                       RequestTimeout                      = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(PullEVSEStatusText).Root,
                             out PullEVSEStatus,
                             CustomPullEVSEStatusRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PullEVSEStatusText, e);
            }

            PullEVSEStatus = null;
            return false;

        }

        #endregion

        #region ToXML(CustomPullEVSEStatusRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEStatusRequestSerializer">A delegate to serialize custom eRoamingPullEvseStatus XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEStatusRequest>  CustomPullEVSEStatusRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatus",

                                   new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                   SearchCenter.HasValue && DistanceKM > 0
                                       ? new XElement(OICPNS.EVSEStatus + "SearchCenter",

                                             new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                 new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                    new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Value.Longitude.ToString("{0:0.######}").Replace(",", ".")),
                                                    new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Value.Latitude. ToString("{0:0.######}").Replace(",", "."))
                                                 )
                                             ),

                                             new XElement(OICPNS.CommonTypes + "Radius", String.Format("{0:0.}", DistanceKM).Replace(",", "."))

                                         )
                                       : null,

                                   EVSEStatusFilter.HasValue
                                       ? new XElement(OICPNS.EVSEStatus + "EvseStatus",  EVSEStatusFilter.Value)
                                       : null

                                  );

            return CustomPullEVSEStatusRequestSerializer != null
                       ? CustomPullEVSEStatusRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEStatus1, PullEVSEStatus2)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatus1">An pull EVSE status request.</param>
        /// <param name="PullEVSEStatus2">Another pull EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEStatusRequest PullEVSEStatus1, PullEVSEStatusRequest PullEVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PullEVSEStatus1, PullEVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PullEVSEStatus1 == null) || ((Object) PullEVSEStatus2 == null))
                return false;

            return PullEVSEStatus1.Equals(PullEVSEStatus2);

        }

        #endregion

        #region Operator != (PullEVSEStatus1, PullEVSEStatus2)

        /// <summary>
        /// Compares two pull EVSE status requests for inequality.
        /// </summary>
        /// <param name="PullEVSEStatus1">An pull EVSE status request.</param>
        /// <param name="PullEVSEStatus2">Another pull EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEStatusRequest PullEVSEStatus1, PullEVSEStatusRequest PullEVSEStatus2)

            => !(PullEVSEStatus1 == PullEVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEStatus> Members

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

            var PullEVSEStatus = Object as PullEVSEStatusRequest;
            if ((Object) PullEVSEStatus == null)
                return false;

            return Equals(PullEVSEStatus);

        }

        #endregion

        #region Equals(PullEVSEStatus)

        /// <summary>
        /// Compares two pull EVSE status requests for equality.
        /// </summary>
        /// <param name="PullEVSEStatus">An pull EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEStatusRequest PullEVSEStatus)
        {

            if ((Object) PullEVSEStatus == null)
                return false;

            return ProviderId.Equals(PullEVSEStatus.ProviderId) &&
                   DistanceKM.Equals(PullEVSEStatus.DistanceKM) &&

                   ((!SearchCenter.    HasValue && !PullEVSEStatus.SearchCenter.    HasValue) ||
                     (SearchCenter.    HasValue &&  PullEVSEStatus.SearchCenter.    HasValue && SearchCenter.    Value.Equals(PullEVSEStatus.SearchCenter.    Value))) &&

                   ((!EVSEStatusFilter.HasValue && !PullEVSEStatus.EVSEStatusFilter.HasValue) ||
                     (EVSEStatusFilter.HasValue &&  PullEVSEStatus.EVSEStatusFilter.HasValue && EVSEStatusFilter.Value.Equals(PullEVSEStatus.EVSEStatusFilter.Value)));

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
                       DistanceKM.GetHashCode() * 11 ^

                       (SearchCenter.     HasValue
                            ? SearchCenter.    GetHashCode() * 5
                            : 0) ^

                       (!EVSEStatusFilter.HasValue
                            ? EVSEStatusFilter.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(SearchCenter.HasValue
                                 ? SearchCenter.ToString() + " / " + DistanceKM + "km"
                                 : "",
                             EVSEStatusFilter.HasValue
                                 ? EVSEStatusFilter.Value.ToString()
                                 : "",
                             " (", ProviderId, ")");

        #endregion


    }

}
