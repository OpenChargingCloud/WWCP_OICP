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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// An OICP pull EVSE data request.
    /// </summary>
    public class PullEVSEDataRequest : ARequest<PullEVSEDataRequest>
    {

        #region Properties

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id                    ProviderId                      { get; }

        /// <summary>
        /// The optional geo coordinate of the search center.
        /// </summary>
        public GeoCoordinate?                 SearchCenter                    { get; }

        /// <summary>
        /// The optional search distance relative to the search center.
        /// </summary>
        public Single                         DistanceKM                      { get; }

        /// <summary>
        /// The optional timestamp of the last call.
        /// </summary>
        public DateTime?                      LastCall                        { get; }

        private HashSet<Operator_Id>          _OperatorIdFilter;

        /// <summary>
        /// Only return EVSEs belonging to the given optional enumeration of EVSE operators.
        /// </summary>
        public IEnumerable<Operator_Id>       OperatorIdFilter
            => _OperatorIdFilter;

        private HashSet<Country>              _CountryCodeFilter;

        /// <summary>
        /// An optional enumeration of countries whose EVSE's a provider wants to retrieve.
        /// </summary>
        public IEnumerable<Country>           CountryCodeFilter
            => _CountryCodeFilter;

        /// <summary>
        /// The optional response format for representing geo coordinates.
        /// </summary>
        public GeoCoordinatesResponseFormats  GeoCoordinatesResponseFormat    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP PullEVSEData XML/SOAP request.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call. Cannot be combined with 'SearchCenter'.</param>
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for representing geo coordinates.</param>
        /// <param name="OperatorIdFilter">Only return EVSEs belonging to the given optional enumeration of EVSE operators.</param>
        /// <param name="CountryCodeFilter">An optional enumeration of countries whose EVSE's a provider wants to retrieve.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public PullEVSEDataRequest(Provider_Id                     ProviderId,
                                   GeoCoordinate?                  SearchCenter                   = null,
                                   Single                          DistanceKM                     = 0f,
                                   DateTime?                       LastCall                       = null,
                                   GeoCoordinatesResponseFormats?  GeoCoordinatesResponseFormat   = GeoCoordinatesResponseFormats.DecimalDegree,
                                   IEnumerable<Operator_Id>        OperatorIdFilter               = null,
                                   IEnumerable<Country>            CountryCodeFilter              = null,

                                   DateTime?                       Timestamp                      = null,
                                   CancellationToken?              CancellationToken              = null,
                                   EventTracking_Id                EventTrackingId                = null,
                                   TimeSpan?                       RequestTimeout                 = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            #endregion

            this.ProviderId                    = ProviderId;
            this.SearchCenter                  = SearchCenter;
            this.DistanceKM                    = DistanceKM;
            this.LastCall                      = LastCall;
            this._OperatorIdFilter             = OperatorIdFilter  != null ? new HashSet<Operator_Id>(OperatorIdFilter)  : new HashSet<Operator_Id>();
            this._CountryCodeFilter            = CountryCodeFilter != null ? new HashSet<Country>    (CountryCodeFilter) : new HashSet<Country>();
            this.GeoCoordinatesResponseFormat  = GeoCoordinatesResponseFormat ?? GeoCoordinatesResponseFormats.DecimalDegree;

        }

        #endregion

        //ToDo: Add OperatorIdFilter and CountryCodeFilter
        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/v2.0"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <EVSEData:eRoamingPullEvseData>
        //
        //          <EVSEData:ProviderID>DE*GDF</EVSEData:ProviderID>
        // 
        //          <!--You have a CHOICE of the next 4 items at this level-->
        //
        //          <!--Optional:-->
        //          <EVSEData:SearchCenter>
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
        //          </EVSEData:SearchCenter>
        //
        //          <!--Optional:-->
        //          <EVSEData:LastCall>?</EVSEData:LastCall>
        //
        //          <!--Optional:-->
        //          <v2:OperatorIDs>
        //             <!--1 or more repetitions:-->
        //             <v2:OperatorID>?</v2:OperatorID>
        //          </v2:OperatorIDs>
        //
        //          <!--Optional:-->
        //          <v2:CountryCodes>
        //             <!--1 or more repetitions:-->
        //             <v2:CountryCode>?</v2:CountryCode>
        //          </v2:CountryCodes>
        //
        //          <EVSEData:GeoCoordinatesResponseFormat>DecimalDegree</EVSEData:GeoCoordinatesResponseFormat>
        //
        //       </EVSEData:eRoamingPullEvseData>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(PullEVSEDataXML,  ..., OnException = null, ...)

        /// <summary>
        /// Parse the given XML representation of an OICP pull EVSE data request.
        /// </summary>
        /// <param name="PullEVSEDataXML">The XML to parse.</param>
        /// <param name="CustomPullEVSEDataRequestParser">A delegate to parse custom PullEVSEData requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullEVSEDataRequest Parse(XElement                                      PullEVSEDataXML,
                                                CustomXMLParserDelegate<PullEVSEDataRequest>  CustomPullEVSEDataRequestParser   = null,
                                                OnExceptionDelegate                           OnException                       = null,

                                                DateTime?                                     Timestamp                         = null,
                                                CancellationToken?                            CancellationToken                 = null,
                                                EventTracking_Id                              EventTrackingId                   = null,
                                                TimeSpan?                                     RequestTimeout                    = null)

        {

            if (TryParse(PullEVSEDataXML,
                         out PullEVSEDataRequest _PullEVSEData,
                         CustomPullEVSEDataRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PullEVSEData;

            return null;

        }

        #endregion

        #region (static) Parse(PullEVSEDataText, ..., OnException = null, ...)

        /// <summary>
        /// Parse the given text-representation of an OICP pull EVSE data request.
        /// </summary>
        /// <param name="PullEVSEDataText">The text to parse.</param>
        /// <param name="CustomPullEVSEDataRequestParser">A delegate to parse custom PullEVSEData requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static PullEVSEDataRequest Parse(String                                        PullEVSEDataText,
                                                CustomXMLParserDelegate<PullEVSEDataRequest>  CustomPullEVSEDataRequestParser   = null,
                                                OnExceptionDelegate                           OnException                       = null,

                                                DateTime?                                     Timestamp                         = null,
                                                CancellationToken?                            CancellationToken                 = null,
                                                EventTracking_Id                              EventTrackingId                   = null,
                                                TimeSpan?                                     RequestTimeout                    = null)

        {

            if (TryParse(PullEVSEDataText,
                         out PullEVSEDataRequest _PullEVSEData,
                         CustomPullEVSEDataRequestParser,
                         OnException,

                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout))

                return _PullEVSEData;

            return null;

        }

        #endregion

        //ToDo: Add OperatorIdFilter and CountryCodeFilter
        #region (static) TryParse(PullEVSEDataXML,  out PullEVSEData, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given XML representation of an OICP pull EVSE data request.
        /// </summary>
        /// <param name="PullEVSEDataXML">The XML to parse.</param>
        /// <param name="PullEVSEData">The parsed pull EVSE data request.</param>
        /// <param name="CustomPullEVSEDataRequestParser">A delegate to parse custom PullEVSEData requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(XElement                                      PullEVSEDataXML,
                                       out PullEVSEDataRequest                       PullEVSEData,
                                       CustomXMLParserDelegate<PullEVSEDataRequest>  CustomPullEVSEDataRequestParser   = null,
                                       OnExceptionDelegate                           OnException                       = null,

                                       DateTime?                                     Timestamp                         = null,
                                       CancellationToken?                            CancellationToken                 = null,
                                       EventTracking_Id                              EventTrackingId                   = null,
                                       TimeSpan?                                     RequestTimeout                    = null)

        {

            try
            {

                if (PullEVSEDataXML.Name != OICPNS.EVSEData + "eRoamingPullEvseData")
                {
                    PullEVSEData = null;
                    return false;
                }

                #region Parse the optional search center

                GeoCoordinate? GeoCoordinates  = null;
                Single         Radius          = 0f;

                var SearchCenterXML = PullEVSEDataXML.Element(OICPNS.EVSEData + "SearchCenter");
                if (SearchCenterXML != null)
                {

                    #region Parse Google format

                    var GoogleXML = SearchCenterXML.Element(OICPNS.CommonTypes + "Google");
                    if (GoogleXML != null)
                    {

                        var GeoArray    = GoogleXML.ElementValueOrFail(OICPNS.CommonTypes + "Coordinates").
                                                    Split(new String[] { " " }, StringSplitOptions.None);

                        GeoCoordinates  = new GeoCoordinate(Latitude. Parse(GeoArray[0]),
                                                            Longitude.Parse(GeoArray[1]));

                    }

                    #endregion

                    #region Parse DecimalDegree format

                    var DecimalDegreeXML = SearchCenterXML.Element(OICPNS.CommonTypes + "DecimalDegree");
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

                PullEVSEData = new PullEVSEDataRequest(PullEVSEDataXML.MapValueOrFail    (OICPNS.EVSEData + "ProviderID",
                                                                                          Provider_Id.Parse),
                                                       GeoCoordinates,
                                                       Radius,

                                                       PullEVSEDataXML.MapValueOrNullable(OICPNS.EVSEData + "LastCall",
                                                                                          DateTime.Parse),

                                                       PullEVSEDataXML.MapValueOrFail    (OICPNS.EVSEData + "GeoCoordinatesResponseFormat",
                                                                                          s => (GeoCoordinatesResponseFormats) Enum.Parse(typeof(GeoCoordinatesResponseFormats), s)),

                                                       null,
                                                       null,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout

                                                  );


                if (CustomPullEVSEDataRequestParser != null)
                    PullEVSEData = CustomPullEVSEDataRequestParser(PullEVSEDataXML,
                                                                   PullEVSEData);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PullEVSEDataXML, e);

                PullEVSEData = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PullEVSEDataText, out PullEVSEData, ..., OnException = null, ...)

        /// <summary>
        /// Try to parse the given text-representation of an OICP pull EVSE data request.
        /// </summary>
        /// <param name="PullEVSEDataText">The text to parse.</param>
        /// <param name="PullEVSEData">The parsed pull EVSE data request.</param>
        /// <param name="CustomPullEVSEDataRequestParser">A delegate to parse custom PullEVSEData requests.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Boolean TryParse(String                                        PullEVSEDataText,
                                       out PullEVSEDataRequest                       PullEVSEData,
                                       CustomXMLParserDelegate<PullEVSEDataRequest>  CustomPullEVSEDataRequestParser   = null,
                                       OnExceptionDelegate                           OnException                       = null,

                                       DateTime?                                     Timestamp                         = null,
                                       CancellationToken?                            CancellationToken                 = null,
                                       EventTracking_Id                              EventTrackingId                   = null,
                                       TimeSpan?                                     RequestTimeout                    = null)

        {

            try
            {

                if (TryParse(XDocument.Parse(PullEVSEDataText).Root,
                             out PullEVSEData,
                             CustomPullEVSEDataRequestParser,
                             OnException,

                             Timestamp,
                             CancellationToken,
                             EventTrackingId,
                             RequestTimeout))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PullEVSEDataText, e);
            }

            PullEVSEData = null;
            return false;

        }

        #endregion

        //ToDo: Add OperatorIdFilter and CountryCodeFilter
        #region ToXML(CustomPullEVSEDataRequestSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomPullEVSEDataRequestSerializer">A delegate to customize the serialization of PullEVSEData requests.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<PullEVSEDataRequest>  CustomPullEVSEDataRequestSerializer = null)
        {

            var XML = new XElement(OICPNS.EVSEData + "eRoamingPullEvseData",

                                   new XElement(OICPNS.EVSEData + "ProviderID", ProviderId.ToString()),

                                   SearchCenter != null && DistanceKM > 0
                                       ? new XElement(OICPNS.EVSEData + "SearchCenter",
                                           new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                               new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                   new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Value.Longitude.ToString("{0:0.######}").Replace(",", ".")),
                                                   new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Value.Latitude. ToString("{0:0.######}").Replace(",", "."))
                                               )
                                           ),
                                           new XElement(OICPNS.CommonTypes + "Radius", String.Format("{0:0.}", DistanceKM).Replace(",", "."))
                                         )
                                       : null,

                                   LastCall.HasValue
                                       ? new XElement(OICPNS.EVSEData + "LastCall",  LastCall.Value.ToIso8601())
                                       : null,

                                   new XElement(OICPNS.EVSEData + "GeoCoordinatesResponseFormat",  "DecimalDegree")

                              );

            return CustomPullEVSEDataRequestSerializer != null
                       ? CustomPullEVSEDataRequestSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (PullEVSEData1, PullEVSEData2)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullEVSEData1">An pull EVSE data request.</param>
        /// <param name="PullEVSEData2">Another pull EVSE data request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PullEVSEDataRequest PullEVSEData1, PullEVSEDataRequest PullEVSEData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PullEVSEData1, PullEVSEData2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PullEVSEData1 == null) || ((Object) PullEVSEData2 == null))
                return false;

            return PullEVSEData1.Equals(PullEVSEData2);

        }

        #endregion

        #region Operator != (PullEVSEData1, PullEVSEData2)

        /// <summary>
        /// Compares two pull EVSE data requests for inequality.
        /// </summary>
        /// <param name="PullEVSEData1">An pull EVSE data request.</param>
        /// <param name="PullEVSEData2">Another pull EVSE data request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PullEVSEDataRequest PullEVSEData1, PullEVSEDataRequest PullEVSEData2)

            => !(PullEVSEData1 == PullEVSEData2);

        #endregion

        #endregion

        #region IEquatable<PullEVSEData> Members

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

            if (!(Object is PullEVSEDataRequest PullEVSEData))
                return false;

            return Equals(PullEVSEData);

        }

        #endregion

        #region Equals(PullEVSEData)

        /// <summary>
        /// Compares two pull EVSE data requests for equality.
        /// </summary>
        /// <param name="PullEVSEData">An pull EVSE data request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(PullEVSEDataRequest PullEVSEData)
        {

            if ((Object) PullEVSEData == null)
                return false;

            return ProviderId.                  Equals(PullEVSEData.ProviderId)                   &&
                   DistanceKM.                  Equals(PullEVSEData.DistanceKM)                   &&
                   GeoCoordinatesResponseFormat.Equals(PullEVSEData.GeoCoordinatesResponseFormat) &&

                   ((!SearchCenter.    HasValue && !PullEVSEData.SearchCenter.    HasValue) ||
                     (SearchCenter.    HasValue &&  PullEVSEData.SearchCenter.    HasValue && SearchCenter.    Value.Equals(PullEVSEData.SearchCenter.Value))) &&

                   ((!LastCall.        HasValue && !PullEVSEData.LastCall.        HasValue) ||
                     (LastCall.        HasValue &&  PullEVSEData.LastCall.        HasValue && LastCall.        Value.Equals(PullEVSEData.LastCall.    Value))) &&

                   _OperatorIdFilter. SetEquals(PullEVSEData._OperatorIdFilter) &&
                   _CountryCodeFilter.SetEquals(PullEVSEData._CountryCodeFilter);

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

                return ProviderId.                  GetHashCode() * 17 ^
                       DistanceKM.                  GetHashCode() * 13 ^
                       GeoCoordinatesResponseFormat.GetHashCode() * 11 ^

                       (SearchCenter.        HasValue
                            ? SearchCenter.       GetHashCode()   *  7
                            : 0) ^

                       (!LastCall.HasValue
                            ? LastCall.GetHashCode()              *  5
                            : 0) ^

                       _OperatorIdFilter. GetHashCode()           *  3 ^

                       _CountryCodeFilter.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(SearchCenter.HasValue
                                 ? SearchCenter.ToString() + " / " + DistanceKM + "km"
                                 : "",
                             LastCall.HasValue
                                 ? LastCall.Value.ToIso8601()
                                 : "",
                             " (", ProviderId, ")");

        #endregion


    }

}
