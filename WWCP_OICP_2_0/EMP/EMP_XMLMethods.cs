﻿/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 EMP management methods.
    /// </summary>
    public static class EMP_XMLMethods
    {

        #region GetEVSEByIdRequestXML(EVSEId) // <- Note!

        // Note: It's confusing, but this request does not belong here!
        //       It must be omplemented on the CPO client side!

        ///// <summary>
        ///// Create a new Get-EVSE-By-Id request.
        ///// In case that CPOs do not upload EVSE data to Hubject, Hubject requests specific EVSE data on demand.
        ///// </summary>
        ///// <param name="EVSEId">An unique EVSE identification.</param>
        //public static XElement GetEVSEByIdRequestXML(EVSE_Id  EVSEId)
        //{

        //    #region Documentation

        //    // <soapenv:Envelope xmlns:soapenv  = "http://schemas.xmlsoap.org/soap/envelope/"
        //    //                   xmlns:EVSEData = "http://www.hubject.com/b2b/services/evsedata/v2.0">
        //    //
        //    //    <soapenv:Header/>
        //    //    <soapenv:Body>
        //    //       <EVSEData:eRoamingGetEvseById>
        //    //
        //    //          <EVSEData:EvseId>+49*123*1234567*1</EVSEData:EvseId>
        //    //
        //    //       </EVSEData:eRoamingGetEvseById>
        //    //    </soapenv:Body>
        //    // </soapenv:Envelope>

        //    #endregion

        //    #region Initial checks

        //    if (EVSEId == null)
        //        throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

        //    #endregion

        //    return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingGetEvseById",
        //                                  new XElement(OICPNS.EVSEData + "EvseId", EVSEId.OriginId)
        //                             ));

        //}

        #endregion

        #region PullEVSEDataRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0, LastCall = null, GeoCoordinatesResponseFormat = DecimalDegree)

        /// <summary>
        /// Create a new Pull EVSE Data request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for the geo coordinates [default: DecimalDegree]</param>
        public static XElement PullEVSEDataRequestXML(EVSP_Id                            ProviderId,
                                                      GeoCoordinate                      SearchCenter                  = null,
                                                      UInt64                             DistanceKM                    = 0,
                                                      DateTime?                          LastCall                      = null,
                                                      GeoCoordinatesResponseFormatTypes  GeoCoordinatesResponseFormat  = GeoCoordinatesResponseFormatTypes.DecimalDegree)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEData:eRoamingPullEvseData>
            //
            //          <EVSEData:ProviderID>?</EVSEData:ProviderID>
            //
            //          <!--You have a CHOICE of the next 2 items at this level-->
            //          <!--Optional:-->
            //          <EVSEData:SearchCenter>
            //
            //             <CommonTypes:GeoCoordinates>
            //                <!--You have a CHOICE of the next 3 items at this level-->
            //
            //                <CommonTypes:Google>
            //                   <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //                </CommonTypes:Google>
            //
            //                <CommonTypes:DecimalDegree>
            //                   <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                   <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                </CommonTypes:DecimalDegree>
            //
            //                <CommonTypes:DegreeMinuteSeconds>
            //                   <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                   <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                </CommonTypes:DegreeMinuteSeconds>
            //
            //             </CommonTypes:GeoCoordinates>
            //
            //             <!-- km ####.# -->
            //             <CommonTypes:Radius>23.5</CommonTypes:Radius>
            //
            //          </EVSEData:SearchCenter>
            //
            //          <!--Optional:-->
            //          <EVSEData:LastCall>?</EVSEData:LastCall>
            //
            //          <EVSEData:GeoCoordinatesResponseFormat>?</EVSEData:GeoCoordinatesResponseFormat>
            //
            //       </EVSEData:eRoamingPullEvseData>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPullEvseData",

                                          new XElement(OICPNS.EVSEData + "ProviderID", ProviderId.ToString()),

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEData + "SearchCenter",
                                                  new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                      new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                          new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                          new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat)))
                                                  ),
                                                  new XElement(OICPNS.CommonTypes + "Radius", DistanceKM)
                                                )
                                              : null,

                                          (LastCall != null && LastCall.HasValue)
                                              ? new XElement(OICPNS.EVSEData + "LastCall",  LastCall.Value.ToIso8601())
                                              : null,

                                          new XElement(OICPNS.EVSEData + "GeoCoordinatesResponseFormat", GeoCoordinatesResponseFormat.ToString())

                                     ));

        }

        #endregion


        #region PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds)

        /// <summary>
        /// Create a new Pull EVSE Status-By-Id request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        public static XElement PullEVSEStatusByIdRequestXML(EVSP_Id               ProviderId,
                                                            IEnumerable<EVSE_Id>  EVSEIds)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEStatus:eRoamingPullEvseStatusById>
            //
            //          <EVSEStatus:ProviderID>?</EVSEStatus:ProviderID>
            //
            //          <!--1 to 100 repetitions:-->
            //          <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
            //
            //       </EVSEStatus:eRoamingPullEvseStatusById>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            if (EVSEIds == null)
                throw new ArgumentNullException("EVSEIds", "The given parameter must not be null!");

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                throw new ArgumentNullException("EVSEIds", "The given enumeration must not be empty!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById",

                                          new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                          EVSEIds.Select(EVSEId => new XElement(OICPNS.EVSEStatus + "EvseId", EVSEId.OriginId))

                                     ));

        }

        #endregion

        #region PullEVSEStatusRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0, EVSEStatus = null)

        /// <summary>
        /// Create a new Pull EVSE Status request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatus">An optional EVSE status as filter criteria.</param>
        public static XElement PullEVSEStatusRequestXML(EVSP_Id          ProviderId,
                                                        GeoCoordinate    SearchCenter  = null,
                                                        UInt64           DistanceKM    = 0,
                                                        OICPEVSEStatus?  EVSEStatus    = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus  = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEStatus:eRoamingPullEvseStatus>
            //
            //          <EVSEStatus:ProviderID>?</EVSEStatus:ProviderID>
            //
            //          <!--Optional:-->
            //          <EVSEStatus:SearchCenter>
            //
            //             <CommonTypes:GeoCoordinates>
            //
            //                <!--You have a CHOICE of the next 3 items at this level-->
            //                <CommonTypes:Google>
            //                   <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //                </CommonTypes:Google>
            //
            //                <CommonTypes:DecimalDegree>
            //                   <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                   <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                </CommonTypes:DecimalDegree>
            //
            //                <CommonTypes:DegreeMinuteSeconds>
            //                   <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                   <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //                </CommonTypes:DegreeMinuteSeconds>
            //
            //             </CommonTypes:GeoCoordinates>
            //
            //             <CommonTypes:Radius>?</CommonTypes:Radius>
            //
            //          </EVSEStatus:SearchCenter>
            //
            //          <!--Optional:-->
            //          <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
            //
            //       </EVSEStatus:eRoamingPullEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatus",

                                          new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEStatus + "SearchCenter",

                                                    new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                        new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                           new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                           new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                                        )
                                                    ),

                                                    new XElement(OICPNS.CommonTypes + "Radius", DistanceKM)

                                                )
                                              : null,

                                          (EVSEStatus != null && EVSEStatus.HasValue)
                                              ? new XElement(OICPNS.EVSEStatus + "EvseStatus",  EVSEStatus.Value)
                                              : null

                                     ));

        }

        #endregion


        #region SearchEvseRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0, Address = null, Plug = null, ChargingFacility = null)

        /// <summary>
        /// Create a new Search EVSE request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        public static XElement SearchEvseRequestXML(EVSP_Id              ProviderId,
                                                    GeoCoordinate        SearchCenter      = null,
                                                    UInt64               DistanceKM        = 0,
                                                    Address              Address           = null,
                                                    PlugTypes?           Plug              = null,
                                                    ChargingFacilities?  ChargingFacility  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSESearch  = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSESearch:eRoamingSearchEvse>
            //
            //          <!--Optional:-->
            //          <EVSESearch:GeoCoordinates>
            //             <!--You have a CHOICE of the next 3 items at this level-->
            //
            //             <CommonTypes:Google>
            //                <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //             </CommonTypes:Google>
            //
            //             <CommonTypes:DecimalDegree>
            //                <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //             </CommonTypes:DecimalDegree>
            //
            //             <CommonTypes:DegreeMinuteSeconds>
            //                <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //             </CommonTypes:DegreeMinuteSeconds>
            //
            //          </EVSESearch:GeoCoordinates>
            //
            //          <!--Optional:-->
            //          <EVSESearch:Address>
            //             <CommonTypes:Country>?</CommonTypes:Country>
            //             <CommonTypes:City>?</CommonTypes:City>
            //             <CommonTypes:Street>?</CommonTypes:Street>
            //             <!--Optional:-->
            //             <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
            //             <!--Optional:-->
            //             <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
            //             <!--Optional:-->
            //             <CommonTypes:Floor>?</CommonTypes:Floor>
            //             <!--Optional:-->
            //             <CommonTypes:Region>?</CommonTypes:Region>
            //             <!--Optional:-->
            //             <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
            //          </EVSESearch:Address>
            //
            //          <EVSESearch:ProviderID>?</EVSESearch:ProviderID>
            //
            //          <EVSESearch:Range>?</EVSESearch:Range>
            //
            //          <!--Optional:-->
            //          <EVSESearch:Plug>?</EVSESearch:Plug>
            //
            //          <!--Optional:-->
            //          <EVSESearch:ChargingFacility>?</EVSESearch:ChargingFacility>
            //
            //       </EVSESearch:eRoamingSearchEvse>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSESearch + "eRoamingSearchEvse",

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEData + "SearchCenter",
                                                  new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                      new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                          new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                          new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat)))
                                                  ))
                                              : null,

                                          (Address         != null            &&
                                           Address.Country != Country.unknown &&
                                           Address.City.  IsNotNullOrEmpty()  &&
                                           Address.Street.IsNotNullOrEmpty())
                                              ? new XElement(OICPNS.EVSEData + "Address",

                                                    new XElement(OICPNS.CommonTypes + "Country" + Address.Country.ToString()),
                                                    new XElement(OICPNS.CommonTypes + "City"    + Address.City),
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

                                          (SearchCenter     != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSESearch + "Range", DistanceKM)
                                              : null,

                                          (Plug             != null && Plug.HasValue)
                                              ? new XElement(OICPNS.EVSESearch + "Plug", Plug.ToString())
                                              : null,

                                          (ChargingFacility != null && ChargingFacility.HasValue)
                                              ? new XElement(OICPNS.EVSESearch + "ChargingFacility", ChargingFacility.ToString())
                                              : null

                                     ));

        }

        #endregion



        #region MobileAuthorizeStartXML(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public static XElement MobileAuthorizeStartXML(EVSE_Id  EVSEId,
                                                       eMA_Id   EVCOId,
                                                       String   PIN,
                                                       String   PartnerProductId = null)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart",

                                          new XElement(OICPNS.MobileAuthorization + "EvseID", EVSEId.ToString()),

                                          new XElement(OICPNS.MobileAuthorization + "QRCodeIdentification",
                                              new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString()),
                                              new XElement(OICPNS.CommonTypes + "PIN",    PIN)
                                          ),

                                          (PartnerProductId != null)
                                              ? new XElement(OICPNS.MobileAuthorization + "PartnerProductID", PartnerProductId)
                                              : null

                                     ));

        }

        #endregion

        #region MobileRemoteStartXML(SessionId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="SessionId">An OICP session identification from the MobileAuthorizationStart response.</param>
        public static XElement MobileRemoteStartXML(ChargingSession_Id  SessionId = null)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart",
                                          new XElement(OICPNS.EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

        #region MobileRemoteStopXML(SessionId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStop request.
        /// </summary>
        /// <param name="SessionId">The OICP session identification from the MobileAuthorizationStart response.</param>
        public static XElement MobileRemoteStopXML(ChargingSession_Id SessionId = null)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop",
                                          new XElement(OICPNS.EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

    }

}