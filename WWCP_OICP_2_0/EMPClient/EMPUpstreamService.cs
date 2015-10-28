/*
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using org.GraphDefined.Vanaheimr.Aegir;

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 EMP upstream services.
    /// </summary>
    public class EMPUpstreamService : IRoamingProviderProvided_EVSPServices
    {

        #region Data

        private readonly EMPClient _EMPClient;

        #endregion

        #region Properties

        #region AuthorizatorId

        protected readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnException

        /// <summary>
        /// A delegate called whenever an exception occured.
        /// </summary>
        public delegate void OnExceptionDelegate(DateTime Timestamp, Object Sender, Exception Exception);

        /// <summary>
        /// An event fired whenever an exception occured.
        /// </summary>
        public event OnExceptionDelegate OnException;

        #endregion

        #region OnHTTPError

        /// <summary>
        /// A delegate called whenever a HTTP error occured.
        /// </summary>
        public delegate void OnHTTPErrorDelegate(DateTime Timestamp, Object Sender, HTTPResponse HttpResponse);

        /// <summary>
        /// An event fired whenever a HTTP error occured.
        /// </summary>
        public event OnHTTPErrorDelegate OnHTTPError;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 EMP upstream service.
        /// </summary>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">The OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="AuthorizatorId">An optional authorizator identification to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPUpstreamService(String           Hostname,
                                  IPPort           TCPPort,
                                  String           HTTPVirtualHost  = null,
                                  Authorizator_Id  AuthorizatorId   = null,
                                  String           HTTPUserAgent    = "GraphDefined OICP v2.0 Gateway EMP Upstream Services",
                                  TimeSpan?        QueryTimeout     = null,
                                  DNSClient        DNSClient        = null)

        {

            this._EMPClient = new EMPClient(Hostname,
                                            TCPPort != null ? TCPPort : IPPort.Parse(443),
                                            HTTPVirtualHost,
                                            HTTPUserAgent,
                                            QueryTimeout,
                                            DNSClient);

        }

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

            return await _EMPClient.PullEVSEData(ProviderId,
                                                 SearchCenter,
                                                 DistanceKM,
                                                 LastCall,
                                                 QueryTimeout,
                                                 ExceptionHandler);

        }

        #endregion


        #region PullEVSEStatus(ProviderId, SearchCenter = null, DistanceKM = 0, EVSEStatusFilter = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingEVSEStatus>>

            PullEVSEStatus(EVSP_Id              ProviderId,
                           GeoCoordinate        SearchCenter      = null,
                           Double               DistanceKM        = 0.0,
                           OICPEVSEStatusType?  EVSEStatusFilter  = null,
                           TimeSpan?            QueryTimeout      = null)

        {

            return await _EMPClient.PullEVSEStatus(ProviderId,
                                                   SearchCenter,
                                                   DistanceKM,
                                                   EVSEStatusFilter,
                                                   QueryTimeout);

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

            return await _EMPClient.PullEVSEStatusById(ProviderId,
                                                       EVSEIds,
                                                       QueryTimeout);

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

            return await _EMPClient.SearchEVSE(ProviderId,
                                               SearchCenter,
                                               DistanceKM,
                                               Address,
                                               Plug,
                                               ChargingFacility);

        }

        #endregion



        //ToDo's:

        #region PushAuthenticationData

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

        #region GetChargeDetailRecords

        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingChargeDetailRecords>
        // 
        //          <!--Zero or more repetitions:-->
        //          <Authorization:eRoamingChargeDetailRecord>
        // 
        //             <Authorization:SessionID>?</Authorization:SessionID>
        // 
        //             <!--Optional:-->
        //             <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        // 
        //             <!--Optional:-->
        //             <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
        // 
        //             <Authorization:EvseID>?</Authorization:EvseID>
        // 
        //             <Authorization:Identification>
        // 
        //                <!--You have a CHOICE of the next 4 items at this level-->
        //                <CommonTypes:RFIDmifarefamilyIdentification>
        //                   <CommonTypes:UID>?</CommonTypes:UID>
        //                </CommonTypes:RFIDmifarefamilyIdentification>
        // 
        //                <CommonTypes:QRCodeIdentification>
        // 
        //                   <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        // 
        //                   <!--You have a CHOICE of the next 2 items at this level-->
        //                   <CommonTypes:PIN>?</CommonTypes:PIN>
        // 
        //                   <CommonTypes:HashedPIN>
        //                      <CommonTypes:Value>?</CommonTypes:Value>
        //                      <CommonTypes:Function>?</CommonTypes:Function>
        //                      <CommonTypes:Salt>?</CommonTypes:Salt>
        //                   </CommonTypes:HashedPIN>
        // 
        //                </CommonTypes:QRCodeIdentification>
        // 
        //                <CommonTypes:PlugAndChargeIdentification>
        //                   <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //                </CommonTypes:PlugAndChargeIdentification>
        // 
        //                <CommonTypes:RemoteIdentification>
        //                   <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //                </CommonTypes:RemoteIdentification>
        // 
        //             </Authorization:Identification>
        // 
        //             <!--Optional:-->
        //             <Authorization:ChargingStart>?</Authorization:ChargingStart>
        //             <!--Optional:-->
        //             <Authorization:ChargingEnd>?</Authorization:ChargingEnd>
        //             <Authorization:SessionStart>?</Authorization:SessionStart>
        //             <Authorization:SessionEnd>?</Authorization:SessionEnd>
        //             <!--Optional:-->
        //             <Authorization:MeterValueStart>?</Authorization:MeterValueStart>
        //             <!--Optional:-->
        //             <Authorization:MeterValueEnd>?</Authorization:MeterValueEnd>
        //             <!--Optional:-->
        //             <Authorization:MeterValueInBetween>
        //                <!--1 or more repetitions:-->
        //                <Authorization:MeterValue>?</Authorization:MeterValue>
        //             </Authorization:MeterValueInBetween>
        //             <!--Optional:-->
        //             <Authorization:ConsumedEnergy>?</Authorization:ConsumedEnergy>
        //             <!--Optional:-->
        //             <Authorization:MeteringSignature>?</Authorization:MeteringSignature>
        //             <!--Optional:-->
        //             <Authorization:HubOperatorID>?</Authorization:HubOperatorID>
        //             <!--Optional:-->
        //             <Authorization:HubProviderID>?</Authorization:HubProviderID>
        // 
        //          </Authorization:eRoamingChargeDetailRecord>
        // 
        //       </Authorization:eRoamingChargeDetailRecords>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #endregion


        #region MobileAuthorizeStart(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public async Task<HTTPResponse<HubjectMobileAuthorizationStart>> MobileAuthorizeStart(EVSE_Id  EVSEId,
                                                                                              eMA_Id   EVCOId,
                                                                                              String   PIN,
                                                                                              String   PartnerProductId = null)
        {

            return Task.FromResult<HTTPResponse<HubjectMobileAuthorizationStart>>(null).Result;

        }

        #endregion

        #region MobileRemoteStart(SessionId = null)

        public async Task<HTTPResponse<MobileRemoteStartResult>> MobileRemoteStart(ChargingSession_Id SessionId = null)
        {

            return Task.FromResult<HTTPResponse<MobileRemoteStartResult>>(null).Result;

        }

        #endregion

        #region MobileRemoteStop(SessionId = null)

        public async Task<HTTPResponse<MobileRemoteStopResult>> MobileRemoteStop(ChargingSession_Id SessionId = null)
        {

            return Task.FromResult<HTTPResponse<MobileRemoteStopResult>>(null).Result;

        }

        #endregion


    }

}
