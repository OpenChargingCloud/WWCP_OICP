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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using System.Xml.Linq;
using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A simple OICP v2.0 EMP client.
    /// </summary>
    public class EMPClient : AOICPUpstreamService
    {

        private Authorizator_Id AuthorizatorId = Authorizator_Id.Parse("");


        #region Data

        private readonly EMPUpstreamService _EMPUpstreamService;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 EMP client.
        /// </summary>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">An optional OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPClient(String     Hostname,
                         IPPort     TCPPort          = null,
                         String     HTTPVirtualHost  = null,
                         String     HTTPUserAgent    = "GraphDefined OICP v2.0 EMPClient",
                         TimeSpan?  QueryTimeout     = null,
                         DNSClient  DNSClient        = null)

            : base(Hostname,
                   TCPPort,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        { }

        #endregion


        #region GetEVSEByIdRequest(EVSEId, QueryTimeout = null) // <- Note!

        // Note: It's confusing, but this request does not belong here!
        //       It must be omplemented on the CPO client side!

        ///// <summary>
        ///// Create a new task requesting the static EVSE data
        ///// for the given EVSE identification.
        ///// </summary>
        ///// <param name="EVSEId">The unique identification of the EVSE.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public Task<HTTPResponse<EVSEDataRecord>>

        //    GetEVSEByIdRequest(EVSE_Id    EVSEId,
        //                       TimeSpan?  QueryTimeout  = null)

        //{

        //    try
        //    {

        //        using (var _OICPClient = new SOAPClient(Hostname,
        //                                                TCPPort,
        //                                                HTTPVirtualHost,
        //                                                "/ibis/ws/eRoamingEvseData_V2.0",
        //                                                UserAgent,
        //                                                DNSClient))
        //        {

        //            return _OICPClient.Query(EMP_XMLMethods.GetEVSEByIdRequestXML(EVSEId),
        //                                     "eRoamingEvseById",
        //                                     QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

        //                                     OnSuccess: XMLData =>

        //                                         #region Documentation

        //                                         // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                                         //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
        //                                         //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //                                         //   <soapenv:Header/>
        //                                         //   <soapenv:Body>
        //                                         //      <EVSEData:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
        //                                         //          [...]
        //                                         //      </EVSEData:eRoamingEvseDataRecord>
        //                                         //    </soapenv:Body>
        //                                         // </soapenv:Envelope>

        //                                         #endregion

        //                                         new HTTPResponse<EVSEDataRecord>(XMLData.HttpResponse,
        //                                                                          XMLMethods.ParseEVSEDataRecordXML(XMLData.Content)),

        //                                     OnSOAPFault: Fault =>
        //                                         new HTTPResponse<EVSEDataRecord>(
        //                                             Fault.HttpResponse,
        //                                             new Exception(Fault.Content.ToString())),

        //                                     OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

        //                                     OnException: (t, s, e) => SendOnException(t, s, e)

        //                                    );

        //        }

        //    }

        //    catch (Exception e)
        //    {

        //        SendOnException(DateTime.Now, this, e);

        //        return new Task<HTTPResponse<EVSEDataRecord>>(
        //            () => new HTTPResponse<EVSEDataRecord>(e));

        //    }

        //}

        #endregion

        #region PullEVSEData(ProviderId, SearchCenter = null, DistanceKM = 0.0, LastCall = null, QueryTimeout = null, ExceptionHandler = null)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        /// <param name="ExceptionHandler">An optional delegate called whenever an exception occured.</param>
        public async Task<HTTPResponse<eRoamingEVSEData>>

            PullEVSEData(EVSP_Id           ProviderId,
                         GeoCoordinate     SearchCenter      = null,
                         Double            DistanceKM        = 0.0,
                         DateTime?         LastCall          = null,
                         TimeSpan?         QueryTimeout      = null,
                         Action<Exception> ExceptionHandler  = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseData_V2.0",
                                                        UserAgent,
                                                        DNSClient))
                {

                    return await _OICPClient.Query(EMP_XMLMethods.PullEVSEDataRequestXML(ProviderId,
                                                                                         SearchCenter,
                                                                                         DistanceKM,
                                                                                         LastCall),
                                                   "eRoamingPullEVSEData",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingEVSEData>(XMLData.HttpResponse,
                                                                                                 eRoamingEVSEData.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("'PullEVSEDataRequest' lead to a SOAP fault!");

                                                       return new HTTPResponse<eRoamingEVSEData>(
                                                           soapfault.HttpResponse,
                                                           new Exception(soapfault.Content.ToString()));

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendOnHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEVSEData>(httpresponse,
                                                                                                 new eRoamingEVSEData(StatusCode: new StatusCode(-1,
                                                                                                                                                 Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                 AdditionalInfo: httpresponse.Content.ToUTF8String())),
                                                                                                 IsFault: true);

                                                   },

                                                   OnException: (t, s, e) => {

                                                       //var OnExceptionLocal = OnException;
                                                       //if (OnExceptionLocal != null)
                                                       //    OnExceptionLocal(t, s, e);

                                                       return null;

                                                   }

                                                  );

                }

            }

            catch (Exception e)
            {

                DebugX.Log("'PullEVSEDataRequest' led to an exception: " + e.Message);

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingEVSEData>(new HTTPResponse(), e);

            }

        }

        #endregion


        #region PullEVSEStatus(ProviderId, SearchCenter = null, DistanceKM = 0.0, EVSEStatusFilter = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingEVSEStatus>>

            PullEVSEStatus(EVSP_Id          ProviderId,
                           GeoCoordinate    SearchCenter      = null,
                           Double           DistanceKM        = 0.0,
                           OICPEVSEStatusType?  EVSEStatusFilter  = null,
                           TimeSpan?        QueryTimeout      = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    _OICPClient.ClientCert                 = this.ClientCert;
                    _OICPClient.RemoteCertificateValidator = this.RemoteCertificateValidator;
                    _OICPClient.ClientCertificateSelector  = this.ClientCertificateSelector;
                    _OICPClient.UseTLS                     = this.UseTLS;

                    return await _OICPClient.Query(EMP_XMLMethods.PullEVSEStatusRequestXML(ProviderId, SearchCenter, DistanceKM, EVSEStatusFilter),
                                                   "eRoamingPullEVSEStatus",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingEVSEStatus>(XMLData.HttpResponse,
                                                                                                   eRoamingEVSEStatus.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                       return new HTTPResponse<eRoamingEVSEStatus>(
                                                           soapfault.HttpResponse,
                                                           null,
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendOnHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEVSEStatus>(httpresponse,
                                                                                                   new eRoamingEVSEStatus(new StatusCode(-1,
                                                                                                                                         httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                         httpresponse.Content.ToUTF8String())),
                                                                                                   IsFault: true);

                                                   },

                                                   OnException: (t, s, e) => {

                                                       //var OnExceptionLocal = OnException;
                                                       //if (OnExceptionLocal != null)
                                                       //    OnExceptionLocal(t, s, e);

                                                       return null;

                                                   }

                                                  );

                }

            }

            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingEVSEStatus>(e);

            }

        }

        #endregion

        #region PullEVSEStatusById(ProviderId, EVSEIds, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingEVSEStatusById>>

            PullEVSEStatusById(EVSP_Id               ProviderId,
                               IEnumerable<EVSE_Id>  EVSEIds,
                               TimeSpan?             QueryTimeout = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds),
                                                   "eRoamingPullEvseStatusById",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingEVSEStatusById>(XMLData.HttpResponse,
                                                                                                       eRoamingEVSEStatusById.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                       return new HTTPResponse<eRoamingEVSEStatusById>(
                                                           soapfault.HttpResponse,
                                                           null,
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendOnHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEVSEStatusById>(httpresponse,
                                                                                                       new eRoamingEVSEStatusById(new StatusCode(-1,
                                                                                                                                                 httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                 httpresponse.Content.ToUTF8String())),
                                                                                                       IsFault: true);

                                                   },

                                                   OnException: (t, s, e) => {

                                                       //var OnExceptionLocal = OnException;
                                                       //if (OnExceptionLocal != null)
                                                       //    OnExceptionLocal(t, s, e);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingEVSEStatusById>(e);

            }

        }

        #endregion


        #region SearchEVSE(ProviderId, SearchCenter = null, DistanceKM = 0.0, Address = null, Plug = null, ChargingFacility = null, QueryTimeout = null)

        /// <summary>
        /// Create a new Search EVSE request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geocoordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatusType>>>>

            SearchEVSE(EVSP_Id              ProviderId,
                       GeoCoordinate        SearchCenter      = null,
                       Double               DistanceKM        = 0.0,
                       Address              Address           = null,
                       PlugTypes?           Plug              = null,
                       ChargingFacilities?  ChargingFacility  = null,
                       TimeSpan?            QueryTimeout      = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingSearchEvse_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.SearchEvseRequestXML(ProviderId,
                                                                                       SearchCenter,
                                                                                       DistanceKM,
                                                                                       Address,
                                                                                       Plug,
                                                                                       ChargingFacility),
                                                   "eRoamingSearchEvse",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       #region Documentation

                                                       // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
                                                       //                   xmlns:EVSESearch  = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
                                                       //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
                                                       //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                                                       //
                                                       //    <soapenv:Header/>
                                                       //    <soapenv:Body>
                                                       //       <EVSESearch:eRoamingEvseSearchResult>
                                                       //          <EVSESearch:EvseMatches>
                                                       //
                                                       //             <!--Zero or more repetitions:-->
                                                       //             <EVSESearch:EvseMatch>
                                                       //
                                                       //                <EVSESearch:Distance>?</EVSESearch:Distance>
                                                       //
                                                       //                <EVSESearch:EVSE deltaType="?" lastUpdate="?">
                                                       //
                                                       //                   <EVSEData:EvseId>?</EVSEData:EvseId>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:ChargingStationId>?</EVSEData:ChargingStationId>
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:ChargingStationName>?</EVSEData:ChargingStationName>
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:EnChargingStationName>?</EVSEData:EnChargingStationName>
                                                       //
                                                       //                   <EVSEData:Address>
                                                       //                      <CommonTypes:Country>?</CommonTypes:Country>
                                                       //                      <CommonTypes:City>?</CommonTypes:City>
                                                       //                      <CommonTypes:Street>?</CommonTypes:Street>
                                                       //                      <!--Optional:-->
                                                       //                      <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
                                                       //                      <!--Optional:-->
                                                       //                      <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
                                                       //                      <!--Optional:-->
                                                       //                      <CommonTypes:Floor>?</CommonTypes:Floor>
                                                       //                      <!--Optional:-->
                                                       //                      <CommonTypes:Region>?</CommonTypes:Region>
                                                       //                      <!--Optional:-->
                                                       //                      <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
                                                       //                   </EVSEData:Address>
                                                       //
                                                       //                   <EVSEData:GeoCoordinates>
                                                       //                      <!--You have a CHOICE of the next 3 items at this level-->
                                                       //
                                                       //                      <CommonTypes:Google>
                                                       //                         <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
                                                       //                      </CommonTypes:Google>
                                                       //
                                                       //                      <CommonTypes:DecimalDegree>
                                                       //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
                                                       //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
                                                       //                      </CommonTypes:DecimalDegree>
                                                       //
                                                       //                      <CommonTypes:DegreeMinuteSeconds>
                                                       //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
                                                       //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
                                                       //                      </CommonTypes:DegreeMinuteSeconds>
                                                       //
                                                       //                   </EVSEData:GeoCoordinates>
                                                       //
                                                       //                   <EVSEData:Plugs>
                                                       //                      <!--1 or more repetitions:-->
                                                       //                      <EVSEData:Plug>?</EVSEData:Plug>
                                                       //                   </EVSEData:Plugs>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:ChargingFacilities>
                                                       //                      <!--1 or more repetitions:-->
                                                       //                      <EVSEData:ChargingFacility>?</EVSEData:ChargingFacility>
                                                       //                   </EVSEData:ChargingFacilities>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:ChargingModes>
                                                       //                      <!--1 or more repetitions:-->
                                                       //                      <EVSEData:ChargingMode>?</EVSEData:ChargingMode>
                                                       //                   </EVSEData:ChargingModes>
                                                       //
                                                       //                   <EVSEData:AuthenticationModes>
                                                       //                      <!--1 or more repetitions:-->
                                                       //                      <EVSEData:AuthenticationMode>?</EVSEData:AuthenticationMode>
                                                       //                   </EVSEData:AuthenticationModes>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:MaxCapacity>?</EVSEData:MaxCapacity>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:PaymentOptions>
                                                       //                      <!--1 or more repetitions:-->
                                                       //                      <EVSEData:PaymentOption>?</EVSEData:PaymentOption>
                                                       //                   </EVSEData:PaymentOptions>
                                                       //
                                                       //                   <EVSEData:Accessibility>?</EVSEData:Accessibility>
                                                       //                   <EVSEData:HotlinePhoneNum>?</EVSEData:HotlinePhoneNum>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:AdditionalInfo>?</EVSEData:AdditionalInfo>
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:EnAdditionalInfo>?</EVSEData:EnAdditionalInfo>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:GeoChargingPointEntrance>
                                                       //                      <!--You have a CHOICE of the next 3 items at this level-->
                                                       //                      <CommonTypes:Google>
                                                       //                         <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
                                                       //                      </CommonTypes:Google>
                                                       //                      <CommonTypes:DecimalDegree>
                                                       //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
                                                       //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
                                                       //                      </CommonTypes:DecimalDegree>
                                                       //                      <CommonTypes:DegreeMinuteSeconds>
                                                       //                         <CommonTypes:Longitude>?</CommonTypes:Longitude>
                                                       //                         <CommonTypes:Latitude>?</CommonTypes:Latitude>
                                                       //                      </CommonTypes:DegreeMinuteSeconds>
                                                       //                   </EVSEData:GeoChargingPointEntrance>
                                                       //
                                                       //                   <EVSEData:IsOpen24Hours>?</EVSEData:IsOpen24Hours>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:OpeningTime>?</EVSEData:OpeningTime>
                                                       //
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:HubOperatorID>?</EVSEData:HubOperatorID>
                                                       //                   <!--Optional:-->
                                                       //                   <EVSEData:ClearinghouseID>?</EVSEData:ClearinghouseID>
                                                       //
                                                       //                   <EVSEData:IsHubjectCompatible>?</EVSEData:IsHubjectCompatible>
                                                       //                   <EVSEData:DynamicInfoAvailable>?</EVSEData:DynamicInfoAvailable>
                                                       //
                                                       //                </EVSESearch:EVSE>
                                                       //             </EVSESearch:EvseMatch>
                                                       //          </EVSESearch:EvseMatches>
                                                       //       </EVSESearch:eRoamingEvseSearchResult>
                                                       //    </soapenv:Body>
                                                       // </soapenv:Envelope>

                                                       #endregion

                                                       #region Hubject error?

                                                       var HubjectError = XMLData.
                                                                              Content.
                                                                              Element(OICPNS.EVSEStatus + "StatusCode");

                                                       if (HubjectError != null)
                                                       {

                                                           // <tns:eRoamingEvseStatusById xmlns:tns="http://www.hubject.com/b2b/services/evsestatus/v1.2">
                                                           //   <tns:StatusCode>
                                                           //     <cmn:Code        xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">002</cmn:Code>
                                                           //     <cmn:Description xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">Hubject database error</cmn:Description>
                                                           //   </tns:StatusCode>
                                                           // </tns:eRoamingEvseStatusById>

                                                           var Code         = HubjectError.Element(OICPNS.CommonTypes + "Code").       Value;
                                                           var Description  = HubjectError.Element(OICPNS.CommonTypes + "Description").Value;
                                                           var Exception    = new ApplicationException(Code + " - " + Description);

                                                           SendOnException(DateTime.Now, this, Exception);

                                                           return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatusType>>>(Exception);

                                                       }

                                                       #endregion

                                                       return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatusType>>>(
                                                                  XMLData.HttpResponse,
                                                                  XMLData.Content.
                                                                          Element (OICPNS.EVSEStatus + "EvseStatusRecords").
                                                                          Elements(OICPNS.EVSEStatus + "EvseStatusRecord").
                                                                          Select(v => new KeyValuePair<EVSE_Id, OICPEVSEStatusType>(EVSE_Id.Parse(v.Element(OICPNS.EVSEStatus + "EvseId").Value),
                                                                                                                               (OICPEVSEStatusType) Enum.Parse(typeof(OICPEVSEStatusType), v.Element(OICPNS.EVSEStatus + "EvseStatus").Value))));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                       return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatusType>>>(
                                                           soapfault.HttpResponse,
                                                           new KeyValuePair<EVSE_Id, OICPEVSEStatusType>[0],
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => {

                                                       //var OnHTTPErrorLocal = OnHTTPError;
                                                       //if (OnHTTPErrorLocal != null)
                                                       //    OnHTTPErrorLocal(t, s, e);

                                                       return null;

                                                   },

                                                   OnException: (t, s, e) => {

                                                       //var OnExceptionLocal = OnException;
                                                       //if (OnExceptionLocal != null)
                                                       //    OnExceptionLocal(t, s, e);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatusType>>>(e);

            }

        }

        #endregion


        // ToDo's:

        #region PushAuthenticationData

        #endregion

        #region GetChargeDetailRecords

        #endregion


        #region MobileAuthorizeStart(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public HubjectMobileAuthorizationStart MobileAuthorizeStart(EVSE_Id  EVSEId,
                                                                    eMA_Id   EVCOId,
                                                                    String   PIN,
                                                                    String   PartnerProductId = null)
        {

            try
            {

                using (var OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/HubjectMobileAuthorization_V1"))
                {

                    var HttpResponse = OICPClient.Query(EMP_XMLMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                           EVCOId,
                                                                                           PIN,
                                                                                           PartnerProductId),
                                                        "eRoamingMobileAuthorizeStart");

                    var XML = XDocument.Parse(HttpResponse.Content.ToUTF8String());

                    //ToDo: In case of errors this will not parse!
                    var MobileAuthorizationStartResult = HubjectMobileAuthorizationStart.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    if (MobileAuthorizationStartResult.AuthorizationStatus == AuthorizationStatusType.Authorized)
                    {

                    //    // <?xml version='1.0' encoding='UTF-8'?>
                    //    // <isns:Envelope xmlns:cmn  = "http://www.inubit.com/eMobility/SBP/CommonTypes"
                    //    //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                xmlns:ns   = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                xmlns:sbp  = "http://www.inubit.com/eMobility/SBP"
                    //    //                xmlns:tns  = "http://www.hubject.com/b2b/services/evsedata/v1"
                    //    //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                xmlns:wsc  = "http://www.hubject.com/b2b/services/mobileauthorization/v1">
                    //    //
                    //    //   <isns:Body>
                    //    //     <wsc:HubjectMobileAuthorizationStart>
                    //    //
                    //    //       <wsc:SessionID>2cfc3548-0a88-1296-7141-df2c5e1864d3</wsc:SessionID>
                    //    //       <wsc:AuthorizationStatus>Authorized</wsc:AuthorizationStatus>
                    //    //
                    //    //       <wsc:StatusCode>
                    //    //         <v1:Code>000</v1:Code>
                    //    //         <v1:Description>Success</v1:Description>
                    //    //       </wsc:StatusCode>
                    //    //
                    //    //       <wsc:GeoCoordinates>
                    //    //         <v1:DecimalDegree>
                    //    //           <v1:Longitude>10.144537</v1:Longitude>
                    //    //           <v1:Latitude>49.729122</v1:Latitude>
                    //    //         </v1:DecimalDegree>
                    //    //       </wsc:GeoCoordinates>
                    //    //
                    //    //       <wsc:Address>
                    //    //         <v1:Country>DEU</v1:Country>
                    //    //         <v1:City>Kitzingen</v1:City>
                    //    //         <v1:Street>Steigweg</v1:Street>
                    //    //         <v1:PostalCode>97318</v1:PostalCode>
                    //    //         <v1:HouseNum>24</v1:HouseNum>
                    //    //       </wsc:Address>
                    //    //
                    //    //       <wsc:ChargingStationName>Innopark Kitzingen</wsc:ChargingStationName>
                    //    //       <wsc:EnChargingStationName>Innopark Kitzingen</wsc:EnChargingStationName>
                    //    //
                    //    //     </wsc:HubjectMobileAuthorizationStart>
                    //    //   </isns:Body>
                    //    // </isns:Envelope>

                    //    return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.Authorized,
                    //                   SessionId            = AuthStartResult.SessionID,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   ProviderId           = EVServiceProvider_Id.Parse(AuthStartResult.ProviderID),
                    //                   Description          = AuthStartResult.Description
                    //               };

                    }

                    #endregion

                    #region NotAuthorized

                    else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    {

                    //    //- Invalid OperatorId ----------------------------------------------------------------------

                    //    // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                    //    //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <isns:Body>
                    //    //     <wsc:HubjectAuthorizationStop>
                    //    //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                    //    //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                    //    //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                    //    //       <wsc:StatusCode>
                    //    //         <v1:Code>017</v1:Code>
                    //    //         <v1:Description>Unauthorized Access</v1:Description>
                    //    //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Thueringen/L=Jena/O=Hubject/OU=GraphDefined GmbH/CN=GraphDefined Software Development/emailAddress=achim.friedland@graphdefined.com" from the certificate.</v1:AdditionalInfo>
                    //    //       </wsc:StatusCode>
                    //    //     </wsc:HubjectAuthorizationStop>
                    //    //   </isns:Body>
                    //    // </isns:Envelope>

                    //    if (AuthStartResult.Code == 017)
                    //        return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   Description          = AuthStartResult.Description + " - " + AuthStartResult.AdditionalInfo
                    //               };


                    //    //- Invalid UID -----------------------------------------------------------------------------

                    //    // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <soapenv:Body>
                    //    //     <tns:HubjectAuthorizationStart>
                    //    //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                    //    //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                    //    //       <tns:StatusCode>
                    //    //         <cmn:Code>320</cmn:Code>
                    //    //         <cmn:Description>Service not available</cmn:Description>
                    //    //       </tns:StatusCode>
                    //    //     </tns:HubjectAuthorizationStart>
                    //    //   </soapenv:Body>
                    //    // </soapenv:Envelope>

                    //    else
                    //        return new AUTHSTARTResult(AuthorizatorId) {
                    //                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    //                       PartnerSessionId     = PartnerSessionId,
                    //                       Description          = AuthStartResult.Description
                    //                   };

                    }

                    #endregion

                    return MobileAuthorizationStartResult;

                }

            }

            catch (Exception e)
            {

                //return new AUTHSTARTResult(AuthorizatorId) {
                //               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                //               PartnerSessionId     = PartnerSessionId,
                //               Description          = "An exception occured: " + e.Message
                //           };

            }

            return null;

        }

        #endregion

        #region MobileRemoteStart(SessionId = null)

        public MobileRemoteStartResult MobileRemoteStart(ChargingSession_Id SessionId = null)
        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/HubjectMobileAuthorization_V1"))
                {

                    var HttpResponse = _OICPClient.Query(EMP_XMLMethods.MobileRemoteStartXML(SessionId),
                                                         "eRoamingMobileRemoteStart");

                    //ToDo: In case of errors this will not parse!
                    var ack = eRoamingAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new MobileRemoteStartResult(AuthorizatorId) {
                            State             = true,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.StatusCode.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new MobileRemoteStartResult(AuthorizatorId) {
                            State             = false,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.StatusCode.Description
                        };

                    #endregion

                }

            }

            catch (Exception e)
            {

                return
                    new MobileRemoteStartResult(AuthorizatorId) {
                        State             = false,
                        //PartnerSessionId  = PartnerSessionId,
                        Description       = "An exception occured: " + e.Message
                    };

            }

        }

        #endregion

        #region MobileRemoteStop(SessionId = null)

        public MobileRemoteStopResult MobileRemoteStop(ChargingSession_Id SessionId = null)
        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/HubjectMobileAuthorization_V1"))
                {

                    var HttpResponse = _OICPClient.Query(EMP_XMLMethods.MobileRemoteStopXML(SessionId),
                                                         "eRoamingMobileRemoteStop");

                    //ToDo: In case of errors this will not parse!
                    var ack = eRoamingAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new MobileRemoteStopResult(AuthorizatorId) {
                            State             = true,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.StatusCode.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new MobileRemoteStopResult(AuthorizatorId) {
                            State             = false,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.StatusCode.Description
                        };

                    #endregion

                }

            }

            catch (Exception e)
            {

                return
                    new MobileRemoteStopResult(AuthorizatorId) {
                        State             = false,
                        //PartnerSessionId  = PartnerSessionId,
                        Description       = "An exception occured: " + e.Message
                    };

            }

        }

        #endregion


        #region MobileAuthorizeStart

        #region Documentation

        // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:MobileAuthorization = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0"
        //                   xmlns:CommonTypes         = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <MobileAuthorization:eRoamingMobileAuthorizationStart>
        // 
        //          <!--Optional:-->
        //          <MobileAuthorization:SessionID>?</MobileAuthorization:SessionID>
        // 
        //          <MobileAuthorization:AuthorizationStatus>?</MobileAuthorization:AuthorizationStatus>
        // 
        //          <!--Optional:-->
        //          <MobileAuthorization:StatusCode>
        //             <CommonTypes:Code>?</CommonTypes:Code>
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //          </MobileAuthorization:StatusCode>
        // 
        //          <!--Optional:-->
        //          <MobileAuthorization:TermsOfUse>?</MobileAuthorization:TermsOfUse>
        // 
        //          <MobileAuthorization:GeoCoordinates>
        // 
        //             <!--You have a CHOICE of the next 3 items at this level-->
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
        //          </MobileAuthorization:GeoCoordinates>
        // 
        //          <!--Optional:-->
        //          <MobileAuthorization:Address>
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
        //          </MobileAuthorization:Address>
        // 
        //          <!--Optional:-->
        //          <MobileAuthorization:AdditionalInfo>?</MobileAuthorization:AdditionalInfo>
        //          <!--Optional:-->
        //          <MobileAuthorization:EnAdditionalInfo>?</MobileAuthorization:EnAdditionalInfo>
        //          <!--Optional:-->
        //          <MobileAuthorization:ChargingStationName>?</MobileAuthorization:ChargingStationName>
        //          <!--Optional:-->
        //          <MobileAuthorization:EnChargingStationName>?</MobileAuthorization:EnChargingStationName>
        // 
        //       </MobileAuthorization:eRoamingMobileAuthorizationStart>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #endregion

        #region MobileRemoteStart

        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <CommonTypes:eRoamingAcknowledgement>
        // 
        //          <CommonTypes:Result>?</CommonTypes:Result>
        // 
        //          <CommonTypes:StatusCode>
        // 
        //             <CommonTypes:Code>?</CommonTypes:Code>
        // 
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        // 
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        // 
        //          </CommonTypes:StatusCode>
        // 
        //          <!--Optional:-->
        //          <CommonTypes:SessionID>?</CommonTypes:SessionID>
        // 
        //          <!--Optional:-->
        //          <CommonTypes:PartnerSessionID>?</CommonTypes:PartnerSessionID>
        // 
        //       </CommonTypes:eRoamingAcknowledgement>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #endregion

        #region MobileRemoteStop

        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <CommonTypes:eRoamingAcknowledgement>
        // 
        //          <CommonTypes:Result>?</CommonTypes:Result>
        // 
        //          <CommonTypes:StatusCode>
        // 
        //             <CommonTypes:Code>?</CommonTypes:Code>
        // 
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        // 
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        // 
        //          </CommonTypes:StatusCode>
        // 
        //          <!--Optional:-->
        //          <CommonTypes:SessionID>?</CommonTypes:SessionID>
        // 
        //          <!--Optional:-->
        //          <CommonTypes:PartnerSessionID>?</CommonTypes:PartnerSessionID>
        // 
        //       </CommonTypes:eRoamingAcknowledgement>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #endregion



    }

}
