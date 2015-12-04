/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
    /// OICP v2.0 EMP client XML methods.
    /// </summary>
    public static class EMPClient_XMLMethods
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

        #region PullEVSEDataRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0.0, LastCall = null, GeoCoordinatesResponseFormat = DecimalDegree)

        /// <summary>
        /// Create a new PullEVSEData request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for the geo coordinates [default: DecimalDegree]</param>
        public static XElement PullEVSEDataRequestXML(EVSP_Id                            ProviderId,
                                                      GeoCoordinate                      SearchCenter                  = null,
                                                      Double                             DistanceKM                    = 0.0,
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
            //       <EVSEData:ProviderID>DE*GDF</EVSEData:ProviderID>
            // 
            //       <!--You have a CHOICE of the next 2 items at this level-->
            //       <!--Optional:-->
            //       <EVSEData:SearchCenter>
            // 
            //          <CommonTypes:GeoCoordinates>
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
            //                <CommonTypes:Latitude>50.931844</CommonTypes:Latitude>
            //             </CommonTypes:DecimalDegree>
            // 
            //             <CommonTypes:DegreeMinuteSeconds>
            //                <!-- -?1?\d{1,2}°[ ]?\d{1,2}'[ ]?\d{1,2}\.\d+'' -->
            //                <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //                <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //             </CommonTypes:DegreeMinuteSeconds>
            // 
            //          </CommonTypes:GeoCoordinates>
            //
            //             <!-- km ####.# is defined, but only #### seems to be accepted -->
            //             <CommonTypes:Radius>100</CommonTypes:Radius>
            //
            //          </EVSEData:SearchCenter>
            //
            //          <!--Optional:-->
            //          <EVSEData:LastCall>?</EVSEData:LastCall>
            //
            //          <EVSEData:GeoCoordinatesResponseFormat>DecimalDegree</EVSEData:GeoCoordinatesResponseFormat>
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
                                                          new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString("{0:0.######}").Replace(",", ".")),
                                                          new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString("{0:0.######}").Replace(",", "."))
                                                      )
                                                  ),
                                                  new XElement(OICPNS.CommonTypes + "Radius", String.Format("{0:0.}", DistanceKM).Replace(",", "."))
                                                )
                                              : null,

                                          (LastCall != null && LastCall.HasValue)
                                              ? new XElement(OICPNS.EVSEData + "LastCall",  LastCall.Value.ToIso8601())
                                              : null,

                                          new XElement(OICPNS.EVSEData + "GeoCoordinatesResponseFormat", GeoCoordinatesResponseFormat.ToString())

                                     ));

        }

        #endregion


        #region PullEVSEStatusRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0.0, EVSEStatus = null)

        /// <summary>
        /// Create a new PullEVSEStatus request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatus">An optional EVSE status as filter criteria.</param>
        public static XElement PullEVSEStatusRequestXML(EVSP_Id          ProviderId,
                                                        GeoCoordinate    SearchCenter  = null,
                                                        Double           DistanceKM    = 0.0,
                                                        EVSEStatusType?  EVSEStatus    = null)
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
                                                           new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString("{0:0.######}").Replace(",", ".")),
                                                           new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString("{0:0.######}").Replace(",", "."))
                                                        )
                                                    ),

                                                    new XElement(OICPNS.CommonTypes + "Radius", String.Format("{0:0.}", DistanceKM).Replace(",", "."))

                                                )
                                              : null,

                                          (EVSEStatus != null && EVSEStatus.HasValue)
                                              ? new XElement(OICPNS.EVSEStatus + "EvseStatus",  EVSEStatus.Value)
                                              : null

                                     ));

        }

        #endregion

        #region PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds)

        /// <summary>
        /// Create a new PullEVSEStatusById request.
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


        #region SearchEvseRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0.0, Address = null, Plug = null, ChargingFacility = null)

        /// <summary>
        /// Create a new SearchEVSE request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        public static XElement SearchEvseRequestXML(EVSP_Id              ProviderId,
                                                    GeoCoordinate        SearchCenter      = null,
                                                    Double               DistanceKM        = 0.0,
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

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSESearch + "eRoamingSearchEvse",

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSESearch + "GeoCoordinates",
                                                    new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                        new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString("{0:0.######}").Replace(",", ".")),
                                                        new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString("{0:0.######}").Replace(",", "."))
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

                                          (SearchCenter     != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSESearch + "Range", String.Format("{0:0.}", DistanceKM).Replace(",", "."))
                                              : null,

                                          (Plug             != null && Plug.HasValue)
                                              ? new XElement(OICPNS.EVSESearch + "Plug", OICPMapper.AsString(Plug.Value))
                                              : null,

                                          (ChargingFacility != null && ChargingFacility.HasValue)
                                              ? new XElement(OICPNS.EVSESearch + "ChargingFacility", OICPMapper.AsString(ChargingFacility.Value))
                                              : null

                                     ));

        }

        #endregion


        #region PushAuthenticationData(ProviderAuthenticationDataRecords, OICPAction = fullLoad)

        /// <summary>
        /// Create a new PushAuthenticationData records request.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        public static XElement PushAuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                                      ActionType                               OICPAction  = ActionType.fullLoad)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
            //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <AuthenticationData:eRoamingPushAuthenticationData>
            // 
            //          <AuthenticationData:ActionType>fullLoad|update|insert|delete</AuthenticationData:ActionType>
            // 
            //          <AuthenticationData:ProviderAuthenticationData>
            // 
            //             <AuthenticationData:ProviderID>?</AuthenticationData:ProviderID>
            // 
            //             <!--Zero or more repetitions:-->
            //             <AuthenticationData:AuthenticationDataRecord>
            //                <AuthenticationData:Identification>
            // 
            //                   <!--You have a CHOICE of the next 4 items at this level-->
            //                   <CommonTypes:RFIDmifarefamilyIdentification>
            //                      <CommonTypes:UID>?</CommonTypes:UID>
            //                   </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //                   <CommonTypes:QRCodeIdentification>
            // 
            //                      <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            // 
            //                      <!--You have a CHOICE of the next 2 items at this level-->
            //                      <CommonTypes:PIN>?</CommonTypes:PIN>
            // 
            //                      <CommonTypes:HashedPIN>
            //                         <CommonTypes:Value>?</CommonTypes:Value>
            //                         <CommonTypes:Function>?</CommonTypes:Function>
            //                         <CommonTypes:Salt>?</CommonTypes:Salt>
            //                      </CommonTypes:HashedPIN>
            // 
            //                   </CommonTypes:QRCodeIdentification>
            // 
            //                   <CommonTypes:PlugAndChargeIdentification>
            //                      <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //                   </CommonTypes:PlugAndChargeIdentification>
            // 
            //                   <CommonTypes:RemoteIdentification>
            //                      <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //                   </CommonTypes:RemoteIdentification>
            // 
            //                </AuthenticationData:Identification>
            //             </AuthenticationData:AuthenticationDataRecord>
            // 
            //          </AuthenticationData:ProviderAuthenticationData>
            // 
            //       </AuthenticationData:eRoamingPushAuthenticationData>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderAuthenticationDataRecords == null)
                throw new ArgumentNullException("ProviderAuthenticationDataRecords", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.AuthenticationData + "eRoamingPushAuthenticationData",

                                      new XElement(OICPNS.AuthenticationData + "ActionType", OICPAction.ToString()),

                                      ProviderAuthenticationDataRecords.Select(ProviderAuthenticationData =>

                                          new XElement(OICPNS.AuthenticationData + "ProviderAuthenticationData",

                                              new XElement(OICPNS.AuthenticationData + "ProviderID", ProviderAuthenticationData.ProviderId.ToString()),

                                              ProviderAuthenticationData.
                                                  AuthorizationIdentifications.
                                                  Select(AuthorizationIdentification => new XElement(OICPNS.AuthenticationData + "AuthenticationDataRecord",
                                                                                                     AuthorizationIdentification.ToXML(OICPNS.AuthenticationData))).
                                                  ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushAuthenticationData(AuthorizationIdentifications, ProviderId, OICPAction = fullLoad)

        /// <summary>
        /// Create a new PushAuthenticationData request.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        public static XElement PushAuthenticationData(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                                      EVSP_Id                                   ProviderId,
                                                      ActionType                                OICPAction  = ActionType.fullLoad)
        {

            return PushAuthenticationData(new ProviderAuthenticationData[] {
                                              new ProviderAuthenticationData(ProviderId, AuthorizationIdentifications)
                                          },
                                          OICPAction);

        }

        #endregion


        #region GetChargeDetailRecords(ProviderId, From, To)

        /// <summary>
        /// Create a new GetChargeDetailRecords request.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">The end time.</param>
        public static XElement GetChargeDetailRecords(EVSP_Id   ProviderId,
                                                      DateTime  From,
                                                      DateTime  To)
        {

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

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingGetChargeDetailRecords",
                                          new XElement(OICPNS.Authorization + "ProviderID", ProviderId.ToString()),
                                          new XElement(OICPNS.Authorization + "From",       From.ToIso8601()),
                                          new XElement(OICPNS.Authorization + "To",         To.ToIso8601())
                                      ));

        }

        #endregion


    }

}
