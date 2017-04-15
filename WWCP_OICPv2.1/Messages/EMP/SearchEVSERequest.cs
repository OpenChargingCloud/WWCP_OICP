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
using System.Linq;
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP search EVSE request.
    /// </summary>
    public class SearchEVSERequest : ARequest<SearchEVSERequest>
    {

        #region Properties

        /// <summary>
        /// An e-mobility provider identification.
        /// </summary>
        public Provider_Id          ProviderId          { get; }

        /// <summary>
        /// The optional geo coordinate of the search center.
        /// </summary>
        public GeoCoordinate?       SearchCenter        { get; }

        /// <summary>
        /// The optional search distance relative to the search center.
        /// </summary>
        public Single               DistanceKM          { get; }

        /// <summary>
        /// The optional address of the charging stations as filter criteria.
        /// </summary>
        public Address              Address             { get; }

        /// <summary>
        /// The optional plugs of the charging station as filter criteria.
        /// </summary>
        public PlugTypes?           Plug                { get; }

        /// <summary>
        /// The optional charging facilities of the charging station as filter criteria.
        /// </summary>
        public ChargingFacilities?  ChargingFacility    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OICP SearchEVSE XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations as filter criteria.</param>
        /// <param name="Plug">Optional plugs of the charging station as filter criteria.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station as filter criteria.</param>
        public SearchEVSERequest(Provider_Id          ProviderId,
                                 GeoCoordinate?       SearchCenter        = null,
                                 Single               DistanceKM          = 0f,
                                 Address              Address             = null,
                                 PlugTypes?           Plug                = null,
                                 ChargingFacilities?  ChargingFacility    = null,

                                 DateTime?            Timestamp           = null,
                                 CancellationToken?   CancellationToken   = null,
                                 EventTracking_Id     EventTrackingId     = null,
                                 TimeSpan?            RequestTimeout      = null)

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
            this.Address           = Address;
            this.Plug              = Plug;
            this.ChargingFacility  = ChargingFacility;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSESearch  = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <EVSESearch:eRoamingSearchEvse>
        //          <!--You have a CHOICE of the next 2 items at this level-->
        // 
        //          <!--Optional:-->
        //          <EVSESearch:GeoCoordinates>
        //             <!--You have a CHOICE of the next 3 items at this level-->
        // 
        //             <CommonTypes:Google>
        //                <!-- latitude longitude: -?1?\d{1,2}\.\d{1,6}\s*\,?\s*-?1?\d{1,2}\.\d{1,6} -->
        //                <CommonTypes:Coordinates>50.931844 11.625214</CommonTypes:Coordinates>
        //             </CommonTypes:Google>
        // 
        //             <CommonTypes:DecimalDegree>
        //                <!-- -?1?\d{1,2}\.\d{1,6} -->
        //                <CommonTypes:Longitude>11.625214</CommonTypes:Longitude>
        //                <CommonTypes:Latitude >50.931844</CommonTypes:Latitude>
        //             </CommonTypes:DecimalDegree>
        // 
        //             <CommonTypes:DegreeMinuteSeconds>
        //                <!-- -?1?\d{1,2}°[ ]?\d{1,2}'[ ]?\d{1,2}\.\d+'' -->
        //                <CommonTypes:Longitude>11° 37' 30.7704''</CommonTypes:Longitude>
        //                <CommonTypes:Latitude >50° 55' 54.6384''</CommonTypes:Latitude>
        //             </CommonTypes:DegreeMinuteSeconds>
        // 
        //          </EVSESearch:GeoCoordinates>
        // 
        //          <!--Optional:-->
        //          <EVSESearch:Address>
        //             <CommonTypes:Country>DE</CommonTypes:Country>
        //             <CommonTypes:City>Jena</CommonTypes:City>
        //             <CommonTypes:Street>Biberweg</CommonTypes:Street>
        //             <!--Optional:-->
        //             <CommonTypes:PostalCode>07749</CommonTypes:PostalCode>
        //             <!--Optional:-->
        //             <CommonTypes:HouseNum>18</CommonTypes:HouseNum>
        //             <!--Optional:-->
        //             <CommonTypes:Floor>2</CommonTypes:Floor>
        //             <!--Optional:-->
        //             <CommonTypes:Region>?</CommonTypes:Region>
        //             <!--Optional:-->
        //             <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
        //          </EVSESearch:Address>
        // 
        //          <EVSESearch:ProviderID>DE*GDF</EVSESearch:ProviderID>
        // 
        //          <EVSESearch:Range>100</EVSESearch:Range>
        // 
        //          <!--Optional:-->
        //          <EVSESearch:Plug>Type 2 Outlet</EVSESearch:Plug>
        // 
        //          <!--Optional:-->
        //          <EVSESearch:ChargingFacility>380 - 480V, 3-Phase ≤ 16A</EVSESearch:ChargingFacility>
        // 
        //       </EVSESearch:eRoamingSearchEvse>
        //    </soapenv:Body>
        // 
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(SearchEVSEXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP search EVSE request.
        /// </summary>
        /// <param name="SearchEVSEXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SearchEVSERequest Parse(XElement             SearchEVSEXML,
                                              OnExceptionDelegate  OnException = null)
        {

            SearchEVSERequest _SearchEVSE;

            if (TryParse(SearchEVSEXML, out _SearchEVSE, OnException))
                return _SearchEVSE;

            return null;

        }

        #endregion

        #region (static) Parse(SearchEVSEText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP search EVSE request.
        /// </summary>
        /// <param name="SearchEVSEText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SearchEVSERequest Parse(String               SearchEVSEText,
                                              OnExceptionDelegate  OnException = null)
        {

            SearchEVSERequest _SearchEVSE;

            if (TryParse(SearchEVSEText, out _SearchEVSE, OnException))
                return _SearchEVSE;

            return null;

        }

        #endregion

        #region (static) TryParse(SearchEVSEXML,  out SearchEVSE, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP search EVSE request.
        /// </summary>
        /// <param name="SearchEVSEXML">The XML to parse.</param>
        /// <param name="SearchEVSE">The parsed search EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement               SearchEVSEXML,
                                       out SearchEVSERequest  SearchEVSE,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                #region Parse the optional search center

                GeoCoordinate? GeoCoordinates  = null;
                Single         Radius          = 0f;

                var GeoCoordinatesXML = SearchEVSEXML.Element(OICPNS.EVSESearch + "GeoCoordinates");
                if (GeoCoordinatesXML != null)
                {

                    #region Parse Google format

                    var GoogleXML = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "Google");
                    if (GoogleXML != null)
                    {

                        var GeoArray    = GoogleXML.ElementValueOrFail(OICPNS.CommonTypes + "Coordinates").
                                                    Split(new String[] { " " }, StringSplitOptions.None);

                        GeoCoordinates  = new GeoCoordinate(Latitude. Parse(GeoArray[0]),
                                                            Longitude.Parse(GeoArray[1]));

                    }

                    #endregion

                    #region Parse DecimalDegree format

                    var DecimalDegreeXML = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DecimalDegree");
                    if (DecimalDegreeXML != null)
                    {

                        GeoCoordinates  = new GeoCoordinate(DecimalDegreeXML.MapValueOrFail(OICPNS.CommonTypes + "Latitude",
                                                                                            Latitude.Parse),
                                                            DecimalDegreeXML.MapValueOrFail(OICPNS.CommonTypes + "Longitude",
                                                                                            Longitude.Parse));


                    }

                    #endregion

                    Radius  = GeoCoordinatesXML.MapValueOrFail(OICPNS.CommonTypes + "Radius",
                                                             Single.Parse);

                }

                #endregion

                SearchEVSE = new SearchEVSERequest(SearchEVSEXML.MapValueOrFail(OICPNS.EVSESearch + "ProviderID",
                                                                                Provider_Id.Parse),
                                                   GeoCoordinates,
                                                   Radius,
                                                   SearchEVSEXML.MapElement(OICPNS.EVSESearch + "Address",
                                                                            XML_IO.ParseAddressXML),

                                                   SearchEVSEXML.MapValueOrNullable(OICPNS.EVSESearch + "Plug",
                                                                                    XML_IO.AsPlugType),
                                                   SearchEVSEXML.MapValueOrNullable(OICPNS.EVSESearch + "ChargingFacility",
                                                                                    XML_IO.AsChargingFacility)
                                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, SearchEVSEXML, e);

                SearchEVSE = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SearchEVSEText, out SearchEVSE, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP search EVSE request.
        /// </summary>
        /// <param name="SearchEVSEText">The text to parse.</param>
        /// <param name="SearchEVSE">The parsed search EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                 SearchEVSEText,
                                       out SearchEVSERequest  SearchEVSE,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SearchEVSEText).Root,
                             out SearchEVSE,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, SearchEVSEText, e);
            }

            SearchEVSE = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.EVSESearch + "eRoamingSearchEvse",

                                          SearchCenter.HasValue && DistanceKM > 0
                                              ? new XElement(OICPNS.EVSESearch + "GeoCoordinates",
                                                    new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                        new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Value.Longitude.ToString("{0:0.######}").Replace(",", ".")),
                                                        new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Value.Latitude. ToString("{0:0.######}").Replace(",", "."))
                                                    )
                                                )
                                              : null,

                                          (Address         != null            &&
                                           Address.Country != Country.unknown &&
                                          !Address.City.   Any()  &&
                                           Address.Street.IsNotNullOrEmpty())
                                              ? new XElement(OICPNS.EVSESearch + "Address",

                                                    new XElement(OICPNS.CommonTypes + "Country" + Address.Country.ToString()),
                                                    new XElement(OICPNS.CommonTypes + "City"    + Address.City.FirstText),
                                                    new XElement(OICPNS.CommonTypes + "Street"  + Address.Street),

                                                    Address.PostalCode.IsNotNullOrEmpty()
                                                        ? new XElement(OICPNS.CommonTypes + "PostalCode" + Address.PostalCode)
                                                        : null,

                                                    Address.HouseNumber.IsNotNullOrEmpty()
                                                        ? new XElement(OICPNS.CommonTypes + "HouseNum"   + Address.HouseNumber)
                                                        : null,

                                                    Address.FloorLevel.IsNotNullOrEmpty()
                                                        ? new XElement(OICPNS.CommonTypes + "Floor"      + Address.FloorLevel)
                                                        : null

                                                    // <!--Optional:-->
                                                    // <v21:Region>?</v21:Region>

                                                    // <!--Optional:-->
                                                    // <v21:TimeZone>?</v21:TimeZone>

                                                    )
                                              : null,

                                          new XElement(OICPNS.EVSESearch + "ProviderID", ProviderId),

                                          SearchCenter.HasValue && DistanceKM > 0
                                              ? new XElement(OICPNS.EVSESearch + "Range", String.Format("{0:0.}", DistanceKM).Replace(",", "."))
                                              : null,

                                          Plug.HasValue
                                              ? new XElement(OICPNS.EVSESearch + "Plug", XML_IO.AsString(Plug.Value))
                                              : null,

                                          ChargingFacility.HasValue
                                              ? new XElement(OICPNS.EVSESearch + "ChargingFacility", XML_IO.AsString(ChargingFacility.Value))
                                              : null

                                     );

        #endregion


        #region Operator overloading

        #region Operator == (SearchEVSE1, SearchEVSE2)

        /// <summary>
        /// Compares two search EVSE requests for equality.
        /// </summary>
        /// <param name="SearchEVSE1">An search EVSE request.</param>
        /// <param name="SearchEVSE2">Another search EVSE request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SearchEVSERequest SearchEVSE1, SearchEVSERequest SearchEVSE2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SearchEVSE1, SearchEVSE2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SearchEVSE1 == null) || ((Object) SearchEVSE2 == null))
                return false;

            return SearchEVSE1.Equals(SearchEVSE2);

        }

        #endregion

        #region Operator != (SearchEVSE1, SearchEVSE2)

        /// <summary>
        /// Compares two search EVSE requests for inequality.
        /// </summary>
        /// <param name="SearchEVSE1">An search EVSE request.</param>
        /// <param name="SearchEVSE2">Another search EVSE request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SearchEVSERequest SearchEVSE1, SearchEVSERequest SearchEVSE2)

            => !(SearchEVSE1 == SearchEVSE2);

        #endregion

        #endregion

        #region IEquatable<SearchEVSE> Members

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

            var SearchEVSE = Object as SearchEVSERequest;
            if ((Object) SearchEVSE == null)
                return false;

            return Equals(SearchEVSE);

        }

        #endregion

        #region Equals(SearchEVSE)

        /// <summary>
        /// Compares two search EVSE requests for equality.
        /// </summary>
        /// <param name="SearchEVSE">An search EVSE request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SearchEVSERequest SearchEVSE)
        {

            if ((Object) SearchEVSE == null)
                return false;

            return ProviderId.Equals(SearchEVSE.ProviderId) &&
                   DistanceKM.Equals(SearchEVSE.DistanceKM) &&

                   (( Address          != null  && SearchEVSE. Address          != null) ||
                     (Address          != null  && SearchEVSE. Address          != null  && Address.               Equals(SearchEVSE.Address))) &&

                   ((!SearchCenter.    HasValue && !SearchEVSE.SearchCenter.    HasValue) ||
                     (SearchCenter.    HasValue &&  SearchEVSE.SearchCenter.    HasValue && SearchCenter.    Value.Equals(SearchEVSE.SearchCenter.    Value))) &&

                   ((!Plug.            HasValue && !SearchEVSE.Plug.            HasValue) ||
                     (Plug.            HasValue &&  SearchEVSE.Plug.            HasValue && Plug.            Value.Equals(SearchEVSE.Plug.            Value))) &&

                   ((!ChargingFacility.HasValue && !SearchEVSE.ChargingFacility.HasValue) ||
                     (ChargingFacility.HasValue &&  SearchEVSE.ChargingFacility.HasValue && ChargingFacility.Value.Equals(SearchEVSE.ChargingFacility.Value)));

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

                       (Address != null
                            ? Address.         GetHashCode() * 7
                            : 0) ^

                       (SearchCenter.HasValue
                            ? SearchCenter.    GetHashCode() * 5
                            : 0) ^

                       (Plug.HasValue
                            ? Plug.            GetHashCode() * 3
                            : 0) ^

                       (ChargingFacility.HasValue
                            ? ChargingFacility.GetHashCode()
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
                             //EVSEStatusFilter.HasValue
                             //    ? EVSEStatusFilter.Value.ToString()
                             //    : "",
                             " (", ProviderId, ")");

        #endregion


    }

}
