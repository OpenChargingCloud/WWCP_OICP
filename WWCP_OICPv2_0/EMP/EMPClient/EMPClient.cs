/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Threading;
using System.Diagnostics;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An OICP EMP client.
    /// </summary>
    public class EMPClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined OICP " + Version.Number + " EMPClient";

        #endregion

        #region Events

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestHandler   OnPullEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnPullEVSEDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnPullEVSEDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseHandler  OnPullEVSEDataResponse;

        #endregion

        #region OnSearchEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a 'search EVSE' request will be send.
        /// </summary>
        public event OnSearchEVSERequestHandler   OnSearchEVSERequest;

        /// <summary>
        /// An event fired whenever a 'search EVSE' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler      OnSearchEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'search EVSE' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler     OnSearchEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'search EVSE' request had been received.
        /// </summary>
        public event OnSearchEVSEResponseHandler  OnSearchEVSEResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestHandler   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnPullEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnPullEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseHandler  OnPullEVSEStatusResponse;

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestHandler   OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnPullEVSEStatusByIdSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnPullEVSEStatusByIdSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseHandler  OnPullEVSEStatusByIdResponse;

        #endregion

        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'push authentication data' request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestHandler   OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a 'push authentication data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnPushAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnPushAuthenticationDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseHandler  OnPushAuthenticationDataResponse;

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestHandler   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAuthorizeRemoteStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAuthorizeRemoteStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseHandler  OnAuthorizeRemoteStartResponse;

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestHandler   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnAuthorizeRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseHandler  OnAuthorizeRemoteStopResponse;

        #endregion

        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a 'get charge detail records' request will be send.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestHandler   OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event fired whenever a 'get charge detail records' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnGetChargeDetailRecordsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'get charge detail records' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnGetChargeDetailRecordsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a 'get charge detail records' request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseHandler  OnGetChargeDetailRecordsResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP EMP client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">An optional OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               TCPPort                     = null,
                         String                               HTTPVirtualHost             = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   TCPPort,
                   HTTPVirtualHost,
                   RemoteCertificateValidator,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        { }

        #endregion


        #region PullEVSEData(ProviderId, SearchCenter = null, DistanceKM = 0.0, LastCall = null, ...)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingEVSEData>>

            PullEVSEData(EVSP_Id             ProviderId,
                         GeoCoordinate       SearchCenter       = null,
                         Double              DistanceKM         = 0.0,
                         DateTime?           LastCall           = null,

                         DateTime?           Timestamp          = null,
                         CancellationToken?  CancellationToken  = null,
                         EventTracking_Id    EventTrackingId    = null,
                         TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnPullEVSEDataRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnPullEVSEDataRequest?.Invoke(DateTime.Now,
                                              Timestamp ?? DateTime.Now,
                                              this,
                                              ClientId,
                                              EventTrackingId,
                                              ProviderId,
                                              SearchCenter,
                                              DistanceKM,
                                              LastCall,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEDataRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseData_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.PullEVSEDataRequestXML(ProviderId,
                                                                                                SearchCenter,
                                                                                                DistanceKM,
                                                                                                LastCall),
                                                     "eRoamingPullEVSEData",
                                                     RequestLogDelegate:   OnPullEVSEDataSOAPRequest,
                                                     ResponseLogDelegate:  OnPullEVSEDataSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingEVSEData.Parse, base.SendException),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         DebugX.Log("'PullEVSEDataRequest' lead to a SOAP fault!");

                                                         return new HTTPResponse<eRoamingEVSEData>(httpresponse,
                                                                                                   IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingEVSEData>(httpresponse,
                                                                                                   new eRoamingEVSEData(StatusCode: new StatusCode(-1,
                                                                                                                                                   Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                   AdditionalInfo: httpresponse.HTTPBody.ToUTF8String())),
                                                                                                   IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnPullEVSEDataResponse event

                Runtime.Stop();

                try
                {

                    OnPullEVSEDataResponse?.Invoke(DateTime.Now,
                                                   this,
                                                   ClientId,
                                                   EventTrackingId,
                                                   ProviderId,
                                                   SearchCenter,
                                                   DistanceKM,
                                                   LastCall,
                                                   RequestTimeout,
                                                   result.Content,
                                                   Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEDataResponse));
                }

                #endregion

                return result;

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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingEvseSearchResult>>

            SearchEVSE(EVSP_Id              ProviderId,
                       GeoCoordinate        SearchCenter       = null,
                       Double               DistanceKM         = 0.0,
                       Address              Address            = null,
                       PlugTypes?           Plug               = null,
                       ChargingFacilities?  ChargingFacility   = null,

                       DateTime?            Timestamp          = null,
                       CancellationToken?   CancellationToken  = null,
                       EventTracking_Id     EventTrackingId    = null,
                       TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnSearchEVSERequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnSearchEVSERequest?.Invoke(DateTime.Now,
                                            Timestamp ?? DateTime.Now,
                                            this,
                                            ClientId,
                                            EventTrackingId,
                                            ProviderId,
                                            SearchCenter,
                                            DistanceKM,
                                            Address,
                                            Plug,
                                            ChargingFacility,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnSearchEVSERequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseSearch_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.SearchEvseRequestXML(ProviderId,
                                                                                              SearchCenter,
                                                                                              DistanceKM,
                                                                                              Address,
                                                                                              Plug,
                                                                                              ChargingFacility),
                                                     "eRoamingSearchEvse",
                                                     RequestLogDelegate:   OnSearchEVSESOAPRequest,
                                                     ResponseLogDelegate:  OnSearchEVSESOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSOAPFault

                                                     OnSuccess: XMLResponse => {

                                                         OICPException _OICPException = null;
                                                         if (OICPClientHelper.IsHubjectError(XMLResponse.Content, out _OICPException, SendException))
                                                             return new HTTPResponse<eRoamingEvseSearchResult>(XMLResponse.HTTPRequest, _OICPException);

                                                         return XMLResponse.Parse(eRoamingEvseSearchResult.Parse);

                                                     },

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                         return new HTTPResponse<eRoamingEvseSearchResult>(httpresponse,
                                                                                                           IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingEvseSearchResult>(httpresponse,
                                                                                                           IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnSearchEVSEResponse event

                Runtime.Stop();

                try
                {

                    OnSearchEVSEResponse?.Invoke(DateTime.Now,
                                                 this,
                                                 ClientId,
                                                 EventTrackingId,
                                                 ProviderId,
                                                 SearchCenter,
                                                 DistanceKM,
                                                 Address,
                                                 Plug,
                                                 ChargingFacility,
                                                 RequestTimeout,
                                                 result.Content,
                                                 Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnSearchEVSEResponse));
                }

                #endregion

                return result;

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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingEVSEStatus>>

            PullEVSEStatus(EVSP_Id             ProviderId,
                           GeoCoordinate       SearchCenter       = null,
                           Double              DistanceKM         = 0.0,
                           EVSEStatusType?     EVSEStatusFilter   = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnPullEVSEStatusRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnPullEVSEStatusRequest?.Invoke(DateTime.Now,
                                                Timestamp ?? DateTime.Now,
                                                this,
                                                ClientId,
                                                EventTrackingId,
                                                ProviderId,
                                                SearchCenter,
                                                DistanceKM,
                                                EVSEStatusFilter,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.PullEVSEStatusRequestXML(ProviderId,
                                                                                                  SearchCenter,
                                                                                                  DistanceKM,
                                                                                                  EVSEStatusFilter),
                                                     "eRoamingPullEVSEStatus",
                                                     RequestLogDelegate:   OnPullEVSEStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnPullEVSEStatusSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingEVSEStatus.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                         return new HTTPResponse<eRoamingEVSEStatus>(httpresponse,
                                                                                                     IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingEVSEStatus>(httpresponse,
                                                                                                     new eRoamingEVSEStatus(new StatusCode(-1,
                                                                                                                                           httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                           httpresponse.HTTPBody.ToUTF8String())),
                                                                                                     IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnPullEVSEStatusResponse event

                Runtime.Stop();

                try
                {

                    OnPullEVSEStatusResponse?.Invoke(DateTime.Now,
                                                     this,
                                                     ClientId,
                                                     EventTrackingId,
                                                     ProviderId,
                                                     SearchCenter,
                                                     DistanceKM,
                                                     EVSEStatusFilter,
                                                     RequestTimeout,
                                                     result.Content,
                                                     Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion

        #region PullEVSEStatusById(ProviderId, EVSEIds, ...)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingEVSEStatusById>>

            PullEVSEStatusById(EVSP_Id               ProviderId,
                               IEnumerable<EVSE_Id>  EVSEIds,

                               DateTime?             Timestamp          = null,
                               CancellationToken?    CancellationToken  = null,
                               EventTracking_Id      EventTrackingId    = null,
                               TimeSpan?             RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (EVSEIds == null)
                throw new ArgumentNullException(nameof(EVSEIds),     "The given enumeration of EVSE identifications must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnPullEVSEStatusByIdRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnPullEVSEStatusByIdRequest?.Invoke(DateTime.Now,
                                                    Timestamp ?? DateTime.Now,
                                                    this,
                                                    ClientId,
                                                    EventTrackingId,
                                                    ProviderId,
                                                    EVSEIds,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.PullEVSEStatusByIdRequestXML(ProviderId,
                                                                                                      EVSEIds),
                                                     "eRoamingPullEvseStatusById",
                                                     RequestLogDelegate:   OnPullEVSEStatusByIdSOAPRequest,
                                                     ResponseLogDelegate:  OnPullEVSEStatusByIdSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingEVSEStatusById.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                         return new HTTPResponse<eRoamingEVSEStatusById>(httpresponse,
                                                                                                         IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingEVSEStatusById>(httpresponse,
                                                                                                         new eRoamingEVSEStatusById(new StatusCode(-1,
                                                                                                                                                   httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                   httpresponse.HTTPBody.ToUTF8String())),
                                                                                                         IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnPullEVSEStatusByIdResponse event

                Runtime.Stop();

                try
                {

                    OnPullEVSEStatusByIdResponse?.Invoke(DateTime.Now,
                                                         this,
                                                         ClientId,
                                                         EventTrackingId,
                                                         ProviderId,
                                                         EVSEIds,
                                                         RequestTimeout,
                                                         result.Content,
                                                         Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


        #region PushAuthenticationData(ProviderAuthenticationDataRecords, OICPAction = fullLoad, ...)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushAuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                   ActionType                               OICPAction         = ActionType.fullLoad,

                                   DateTime?                                Timestamp          = null,
                                   CancellationToken?                       CancellationToken  = null,
                                   EventTracking_Id                         EventTrackingId    = null,
                                   TimeSpan?                                RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderAuthenticationDataRecords == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationDataRecords), "The given provider authentication data records must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnPushAuthenticationDataRequest?.Invoke(DateTime.Now,
                                                        Timestamp ?? DateTime.Now,
                                                        this,
                                                        ClientId,
                                                        EventTrackingId,
                                                        ProviderAuthenticationDataRecords,
                                                        OICPAction,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.PushAuthenticationData(ProviderAuthenticationDataRecords,
                                                                                                OICPAction),
                                                     "eRoamingPushAuthenticationData",
                                                     RequestLogDelegate:   OnPushAuthenticationDataSOAPRequest,
                                                     ResponseLogDelegate:  OnPushAuthenticationDataSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                          new eRoamingAcknowledgement(false,
                                                                                                                                      -1,
                                                                                                                                      Description: httpresponse.Content.ToString()),
                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                          new eRoamingAcknowledgement(false,
                                                                                                                                      -1,
                                                                                                                                      Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                      AdditionalInfo: httpresponse.HTTPBody.ToUTF8String()),
                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnPushAuthenticationDataResponse event

                Runtime.Stop();

                try
                {

                    OnPushAuthenticationDataResponse?.Invoke(DateTime.Now,
                                                             this,
                                                             ClientId,
                                                             EventTrackingId,
                                                             ProviderAuthenticationDataRecords,
                                                             OICPAction,
                                                             RequestTimeout,
                                                             result.Content,
                                                             Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion

        #region PushAuthenticationData(...AuthorizationIdentifications, ProviderId, OICPAction = fullLoad, ...)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushAuthenticationData(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                   EVSP_Id                                   ProviderId,
                                   ActionType                                OICPAction         = ActionType.fullLoad,

                                   DateTime?                                 Timestamp          = null,
                                   CancellationToken?                        CancellationToken  = null,
                                   EventTracking_Id                          EventTrackingId    = null,
                                   TimeSpan?                                 RequestTimeout     = null)

            => await PushAuthenticationData(new ProviderAuthenticationData[] {
                                                new ProviderAuthenticationData(ProviderId, AuthorizationIdentifications)
                                            },
                                            OICPAction,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

        #endregion


        #region RemoteStart(ProviderId, EVSEId, eMAId, SessionId = null, PartnerSessionId = null, PartnerProductId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">The unique identification of the choosen charging product.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            RemoteStart(EVSP_Id                 ProviderId,
                        EVSE_Id                 EVSEId,
                        eMA_Id                  eMAId,
                        ChargingSession_Id      SessionId          = null,
                        ChargingSession_Id      PartnerSessionId   = null,
                        ChargingProduct_Id      PartnerProductId   = null,

                        DateTime?               Timestamp          = null,
                        CancellationToken?      CancellationToken  = null,
                        EventTracking_Id        EventTrackingId    = null,
                        TimeSpan?               RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),      "The given EVSE identification must not be null!");

            if (eMAId == null)
                throw new ArgumentNullException(nameof(eMAId),       "The given e-mobility account identification must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeRemoteStartRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnAuthorizeRemoteStartRequest?.Invoke(DateTime.Now,
                                                      Timestamp ?? DateTime.Now,
                                                      this,
                                                      ClientId,
                                                      EventTrackingId,
                                                      ProviderId,
                                                      EVSEId,
                                                      eMAId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      PartnerProductId,
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStartRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.AuthorizeRemoteStartXML(ProviderId,
                                                                                                 EVSEId,
                                                                                                 eMAId,
                                                                                                 SessionId,
                                                                                                 PartnerSessionId,
                                                                                                 PartnerProductId),
                                                     "eRoamingAuthorizeRemoteStart",
                                                     RequestLogDelegate:   OnAuthorizeRemoteStartSOAPRequest,
                                                     ResponseLogDelegate:  OnAuthorizeRemoteStartSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                          new eRoamingAcknowledgement(false,
                                                                                                                                      -1,
                                                                                                                                      Description: httpresponse.Content.ToString()),
                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                          new eRoamingAcknowledgement(false,
                                                                                                                                      -1,
                                                                                                                                      Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                      AdditionalInfo: httpresponse.HTTPBody.ToUTF8String()),
                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnAuthorizeRemoteStartResponse event

                Runtime.Stop();

                try
                {

                    OnAuthorizeRemoteStartResponse?.Invoke(DateTime.Now,
                                                           this,
                                                           ClientId,
                                                           EventTrackingId,
                                                           ProviderId,
                                                           EVSEId,
                                                           eMAId,
                                                           SessionId,
                                                           PartnerSessionId,
                                                           PartnerProductId,
                                                           RequestTimeout,
                                                           result.Content,
                                                           Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStartResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion

        #region RemoteStop(EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="PartnerSessionId">The unique identification for the partner charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            RemoteStop(ChargingSession_Id   SessionId,
                       EVSP_Id              ProviderId,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   PartnerSessionId   = null,

                       DateTime?            Timestamp          = null,
                       CancellationToken?   CancellationToken  = null,
                       EventTracking_Id     EventTrackingId    = null,
                       TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),   "The given charging session identification must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),      "The given EVSE identification must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeRemoteStopRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnAuthorizeRemoteStopRequest?.Invoke(DateTime.Now,
                                                     Timestamp ?? DateTime.Now,
                                                     this,
                                                     ClientId,
                                                     EventTrackingId,
                                                     SessionId,
                                                     ProviderId,
                                                     EVSEId,
                                                     PartnerSessionId,
                                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.AuthorizeRemoteStopXML(SessionId,
                                                                                                ProviderId,
                                                                                                EVSEId,
                                                                                                PartnerSessionId),
                                                     "eRoamingAuthorizeRemoteStop",
                                                     RequestLogDelegate:   OnAuthorizeRemoteStopSOAPRequest,
                                                     ResponseLogDelegate:  OnAuthorizeRemoteStopSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                          new eRoamingAcknowledgement(false,
                                                                                                                                      -1,
                                                                                                                                      Description: httpresponse.Content.ToString()),
                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                          new eRoamingAcknowledgement(false,
                                                                                                                                      -1,
                                                                                                                                      Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                      AdditionalInfo: httpresponse.HTTPBody.ToUTF8String()),
                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnAuthorizeRemoteStopResponse event

                Runtime.Stop();

                try
                {

                    OnAuthorizeRemoteStopResponse?.Invoke(DateTime.Now,
                                                          this,
                                                          ClientId,
                                                          EventTrackingId,
                                                          SessionId,
                                                          ProviderId,
                                                          EVSEId,
                                                          PartnerSessionId,
                                                          RequestTimeout,
                                                          result.Content,
                                                          Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


        #region GetChargeDetailRecords(ProviderId, From, To, ...)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">An optional end time. [default: current time].</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<IEnumerable<ChargeDetailRecord>>>

            GetChargeDetailRecords(EVSP_Id             ProviderId,
                                   DateTime            From,
                                   DateTime?           To                 = null,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given e-mobility provider identification must not be null!");

            if (!To.HasValue)
                To = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnGetChargeDetailRecordsRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnGetChargeDetailRecordsRequest?.Invoke(DateTime.Now,
                                                        Timestamp ?? DateTime.Now,
                                                        this,
                                                        ClientId,
                                                        EventTrackingId,
                                                        ProviderId,
                                                        From,
                                                        To.Value,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnGetChargeDetailRecordsRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.GetChargeDetailRecords(ProviderId,
                                                                                                From,
                                                                                                To.Value),
                                                     "eRoamingGetChargeDetailRecords",
                                                     RequestLogDelegate:   OnGetChargeDetailRecordsSOAPRequest,
                                                     ResponseLogDelegate:  OnGetChargeDetailRecordsSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingChargeDetailRecords.ParseXML),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<IEnumerable<ChargeDetailRecord>>(httpresponse,
                                                                                                                          new ChargeDetailRecord[0],
                                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<IEnumerable<ChargeDetailRecord>>(httpresponse,
                                                                                                                          new ChargeDetailRecord[0],
                                                                                                                          IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return null;

                                                     }

                                                     #endregion

                                                    );

                #region Send OnGetChargeDetailRecordsResponse event

                Runtime.Stop();

                try
                {

                    OnGetChargeDetailRecordsResponse?.Invoke(DateTime.Now,
                                                             this,
                                                             ClientId,
                                                             EventTrackingId,
                                                             ProviderId,
                                                             From,
                                                             To.Value,
                                                             RequestTimeout,
                                                             result.Content,
                                                             Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnGetChargeDetailRecordsResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


    }

}
