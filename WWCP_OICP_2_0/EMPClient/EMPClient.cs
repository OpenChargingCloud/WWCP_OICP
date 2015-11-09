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
        public EMPClient(String              Hostname,
                         IPPort              TCPPort          = null,
                         String              HTTPVirtualHost  = null,
                         String              HTTPUserAgent    = "GraphDefined OICP v2.0 EMPClient",
                         TimeSpan?           QueryTimeout     = null,
                         DNSClient           DNSClient        = null)

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

        #region PullEVSEData(ProviderId, SearchCenter = null, DistanceKM = 0.0, LastCall = null, QueryTimeout = null, OnException = null)

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
        public async Task<HTTPResponse<eRoamingEVSEData>>

            PullEVSEData(EVSP_Id           ProviderId,
                         GeoCoordinate     SearchCenter  = null,
                         Double            DistanceKM    = 0.0,
                         DateTime?         LastCall      = null,
                         TimeSpan?         QueryTimeout  = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseData_V2.0",
                                                        UserAgent,
                                                        false,
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
                                                                                                 eRoamingEVSEData.Parse(XMLData.Content, base.SendException));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("'PullEVSEDataRequest' lead to a SOAP fault!");

                                                       return new HTTPResponse<eRoamingEVSEData>(
                                                           soapfault.HttpResponse,
                                                           new Exception(soapfault.Content.ToString()));

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEVSEData>(httpresponse,
                                                                                                 new eRoamingEVSEData(StatusCode: new StatusCode(-1,
                                                                                                                                                 Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                 AdditionalInfo: httpresponse.Content.ToUTF8String())),
                                                                                                 IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            catch (Exception e)
            {

                DebugX.Log("'PullEVSEDataRequest' led to an exception: " + e.Message);

                SendException(DateTime.Now, this, e);

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

            PullEVSEStatus(EVSP_Id              ProviderId,
                           GeoCoordinate        SearchCenter      = null,
                           Double               DistanceKM        = 0.0,
                           OICPEVSEStatusType?  EVSEStatusFilter  = null,
                           TimeSpan?            QueryTimeout      = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        false,
                                                        DNSClient))

                {

                   // _OICPClient.ClientCert                 = this.ClientCert;
                   // _OICPClient.RemoteCertificateValidator = this.RemoteCertificateValidator;
                   // _OICPClient.ClientCertificateSelector  = this.ClientCertificateSelector;

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

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEVSEStatus>(httpresponse,
                                                                                                   new eRoamingEVSEStatus(new StatusCode(-1,
                                                                                                                                         httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                         httpresponse.Content.ToUTF8String())),
                                                                                                   IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

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
                                                        false,
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

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEVSEStatusById>(httpresponse,
                                                                                                       new eRoamingEVSEStatusById(new StatusCode(-1,
                                                                                                                                                 httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                 httpresponse.Content.ToUTF8String())),
                                                                                                       IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

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
        public async Task<HTTPResponse<eRoamingEvseSearchResult>>

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
                                                        "/ibis/ws/eRoamingEvseSearch_V2.0",
                                                        UserAgent,
                                                        false,
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

                                                       OICPException _OICPException = null;
                                                       if (IsHubjectError(XMLData.Content, out _OICPException, SendException))
                                                           return new HTTPResponse<eRoamingEvseSearchResult>(_OICPException);

                                                       return new HTTPResponse<eRoamingEvseSearchResult>(XMLData.HttpResponse,
                                                                                                         eRoamingEvseSearchResult.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                       return new HTTPResponse<eRoamingEvseSearchResult>(soapfault.HttpResponse,
                                                                                                         null,
                                                                                                         IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingEvseSearchResult>(httpresponse,
                                                                                                         null,
                                                                                                         IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingEvseSearchResult>(e);

            }

        }

        #endregion


        #region PushAuthenticationData(ProviderAuthenticationDataRecords, OICPAction = fullLoad, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushAuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                   ActionType                               OICPAction    = ActionType.fullLoad,
                                   TimeSpan?                                QueryTimeout  = null)

        {

            #region Initial checks

            if (ProviderAuthenticationDataRecords == null)
                throw new ArgumentNullException("ProviderAuthenticationDataRecords", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                        UserAgent,
                                                        false,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.PushAuthenticationData(ProviderAuthenticationDataRecords,
                                                                                         OICPAction),
                                                   "eRoamingPushAuthenticationData",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse,
                                                                                                        eRoamingAcknowledgement.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(soapfault.HttpResponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description: soapfault.Content.ToString()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                    AdditionalInfo: httpresponse.Content.ToUTF8String()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(e);

            }

        }

        #endregion

        #region PushAuthenticationData(AuthorizationIdentifications, ProviderId, OICPAction = fullLoad, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushAuthenticationData(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                   EVSP_Id                                   ProviderId,
                                   ActionType                                OICPAction    = ActionType.fullLoad,
                                   TimeSpan?                                 QueryTimeout  = null)

        {

            #region Initial checks

            if (AuthorizationIdentifications == null)
                throw new ArgumentNullException("AuthorizationIdentifications", "The given parameter must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                        UserAgent,
                                                        false,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.PushAuthenticationData(AuthorizationIdentifications,
                                                                                         ProviderId,
                                                                                         OICPAction),
                                                   "eRoamingPushAuthenticationData",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse,
                                                                                                        eRoamingAcknowledgement.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(soapfault.HttpResponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description: soapfault.Content.ToString()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                    AdditionalInfo: httpresponse.Content.ToUTF8String()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(e);

            }

        }

        #endregion


        #region GetChargeDetailRecords

        #endregion



        #region MobileAuthorizeStart(EVSEId, EVCOId, PIN, PartnerProductId = null, GetNewSession = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task sending a mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOId">The eMA identification.</param>
        /// <param name="PIN">The PIN for the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingMobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id    EVSEId,
                                 eMA_Id     EVCOId,
                                 String     PIN,
                                 String     PartnerProductId  = null,
                                 Boolean?   GetNewSession     = null,
                                 TimeSpan?  QueryTimeout      = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            if (EVCOId == null)
                throw new ArgumentNullException("EVCOId", "The given parameter must not be null!");

            if (PIN == null)
                throw new ArgumentNullException("PIN", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingMobileAuthorization_V2.0",
                                                        UserAgent,
                                                        false,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                          EVCOId,
                                                                                          PIN,
                                                                                          PartnerProductId),
                                                   "eRoamingMobileAuthorizeStart",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

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

                                                       return new HTTPResponse<eRoamingMobileAuthorizationStart>(XMLData.HttpResponse,
                                                                                                                eRoamingMobileAuthorizationStart.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingMobileAuthorizationStart>(soapfault.HttpResponse,
                                                                                                                new eRoamingMobileAuthorizationStart(-1,
                                                                                                                                                     Description: soapfault.Content.ToString()),
                                                                                                                IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingMobileAuthorizationStart>(httpresponse,
                                                                                                                 new eRoamingMobileAuthorizationStart(-1,
                                                                                                                                                      Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                      AdditionalInfo: httpresponse.Content.ToUTF8String()),
                                                                                                                 IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingMobileAuthorizationStart>(e);

            }

        }

        #endregion

        #region MobileRemoteStart(SessionId, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            MobileRemoteStart(ChargingSession_Id  SessionId,
                              TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException("SessionId", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingMobileAuthorization_V2.0",
                                                        UserAgent,
                                                        false,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.MobileRemoteStartXML(SessionId),
                                                   "eRoamingMobileRemoteStart",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse,
                                                                                                        eRoamingAcknowledgement.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(soapfault.HttpResponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description: soapfault.Content.ToString()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                    AdditionalInfo: httpresponse.Content.ToUTF8String()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(e);

            }

        }

        #endregion

        #region MobileRemoteStop(SessionId, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            MobileRemoteStop(ChargingSession_Id  SessionId,
                             TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException("SessionId", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/HubjectMobileAuthorization_V2.0",
                                                        UserAgent,
                                                        false,
                                                        DNSClient))

                {

                    return await _OICPClient.Query(EMP_XMLMethods.MobileRemoteStopXML(SessionId),
                                                   "eRoamingMobileRemoteStop",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData => {

                                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse,
                                                                                                        eRoamingAcknowledgement.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(soapfault.HttpResponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description: soapfault.Content.ToString()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                    AdditionalInfo: httpresponse.Content.ToUTF8String()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                            );

                }

            }

            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(e);

            }

        }

        #endregion


    }

}
