﻿/*
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

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartHandler    OnAuthorizeRemoteStart;

        /// <summary>
        /// An event fired whenever an authorize remote start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize remote start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnAuthorizeRemoteStartResponse;

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStartedHandler  OnAuthorizeRemoteStarted;

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize remote stop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopHandler     OnAuthorizeRemoteStop;

        /// <summary>
        /// An event fired whenever an authorize remote stop SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize remote stop SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnAuthorizeRemoteStopResponse;

        /// <summary>
        /// An event fired whenever an authorize remote stop request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStoppedHandler  OnAuthorizeRemoteStopped;

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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseData_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                return await _OICPClient.Query(EMPClientXMLMethods.PullEVSEDataRequestXML(ProviderId,
                                                                                           SearchCenter,
                                                                                           DistanceKM,
                                                                                           LastCall),
                                               "eRoamingPullEVSEData",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseSearch_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(EMPClientXMLMethods.SearchEvseRequestXML(ProviderId,
                                                                                        SearchCenter,
                                                                                        DistanceKM,
                                                                                        Address,
                                                                                        Plug,
                                                                                        ChargingFacility),
                                               "eRoamingSearchEvse",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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
                           EVSEStatusType?  EVSEStatusFilter  = null,
                           TimeSpan?        QueryTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

               // _OICPClient.ClientCert                 = this.ClientCert;
               // _OICPClient.RemoteCertificateValidator = this.RemoteCertificateValidator;
               // _OICPClient.ClientCertificateSelector  = this.ClientCertificateSelector;

                return await _OICPClient.Query(EMPClientXMLMethods.PullEVSEStatusRequestXML(ProviderId,
                                                                                             SearchCenter,
                                                                                             DistanceKM,
                                                                                             EVSEStatusFilter),
                                               "eRoamingPullEVSEStatus",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(EMPClientXMLMethods.PullEVSEStatusByIdRequestXML(ProviderId,
                                                                                                 EVSEIds),
                                               "eRoamingPullEvseStatusById",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(EMPClientXMLMethods.PushAuthenticationData(ProviderAuthenticationDataRecords,
                                                                                           OICPAction),
                                               "eRoamingPushAuthenticationData",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(EMPClientXMLMethods.PushAuthenticationData(AuthorizationIdentifications,
                                                                                           ProviderId,
                                                                                           OICPAction),
                                               "eRoamingPushAuthenticationData",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

            }

        }

        #endregion


        #region RemoteStart(...ProviderId, EVSEId, eMAId, SessionId = null, PartnerSessionId = null, PartnerProductId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">The unique identification of the choosen charging product.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        EVSP_Id                 ProviderId,
                        EVSE_Id                 EVSEId,
                        eMA_Id                  eMAId,
                        ChargingSession_Id      SessionId         = null,
                        ChargingSession_Id      PartnerSessionId  = null,
                        ChargingProduct_Id      PartnerProductId  = null,
                        TimeSpan?               QueryTimeout      = default(TimeSpan?))

        {

            #region Send OnAuthorizeRemoteStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnAuthorizeRemoteStart?.Invoke(DateTime.Now,
                                               this,
                                               ClientId,
                                               ProviderId,
                                               EVSEId,
                                               eMAId,
                                               SessionId,
                                               PartnerSessionId,
                                               PartnerProductId,
                                               QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStart));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
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
                                                     RequestLogDelegate:   OnAuthorizeRemoteStartRequest,
                                                     ResponseLogDelegate:  OnAuthorizeRemoteStartResponse,
                                                     QueryTimeout:         QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

                #region Send OnAuthorizeRemoteStarted event

                Runtime.Stop();

                try
                {

                    OnAuthorizeRemoteStarted?.Invoke(DateTime.Now,
                                                     this,
                                                     ClientId,
                                                     ProviderId,
                                                     EVSEId,
                                                     eMAId,
                                                     SessionId,
                                                     PartnerSessionId,
                                                     PartnerProductId,
                                                     QueryTimeout,
                                                     result.Content,
                                                     Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStarted));
                }

                #endregion

                return result;

            }

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="PartnerSessionId">The unique identification for the partner charging session.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       ChargingSession_Id   SessionId,
                       EVSP_Id              ProviderId,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   PartnerSessionId  = null,
                       TimeSpan?            QueryTimeout      = null)

        {

            #region Send OnAuthorizeRemoteStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnAuthorizeRemoteStop?.Invoke(DateTime.Now,
                                              this,
                                              ClientId,
                                              SessionId,
                                              ProviderId,
                                              EVSEId,
                                              PartnerSessionId,
                                              QueryTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStop));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                var result = await _OICPClient.Query(EMPClientXMLMethods.AuthorizeRemoteStopXML(SessionId,
                                                                                                ProviderId,
                                                                                                EVSEId,
                                                                                                PartnerSessionId),
                                                     "eRoamingAuthorizeRemoteStop",
                                                     RequestLogDelegate:   OnAuthorizeRemoteStopRequest,
                                                     ResponseLogDelegate:  OnAuthorizeRemoteStopResponse,
                                                     QueryTimeout:         QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

                #region Send OnAuthorizeRemoteStopped event

                Runtime.Stop();

                try
                {

                    OnAuthorizeRemoteStopped?.Invoke(DateTime.Now,
                                                     this,
                                                     ClientId,
                                                     SessionId,
                                                     ProviderId,
                                                     EVSEId,
                                                     PartnerSessionId,
                                                     QueryTimeout,
                                                     result.Content,
                                                     Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopped));
                }

                #endregion

                return result;

            }

        }

        #endregion


        #region GetChargeDetailRecords(ProviderId, From, To, QueryTimeout = null)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">The end time.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<IEnumerable<ChargeDetailRecord>>>

            GetChargeDetailRecords(DateTime           Timestamp,
                                   CancellationToken  CancellationToken,
                                   EventTracking_Id   EventTrackingId,
                                   EVSP_Id            ProviderId,
                                   DateTime           From,
                                   DateTime           To,
                                   TimeSpan?          QueryTimeout  = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(EMPClientXMLMethods.GetChargeDetailRecords(ProviderId,
                                                                                          From,
                                                                                          To),
                                               "eRoamingGetChargeDetailRecords",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

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

            }

        }

        #endregion


    }

}
