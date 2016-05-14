/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Net.Security;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 Mobile client.
    /// </summary>
    public class MobileClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined OICP v2.0 MobileClient";

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 Mobile client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">An optional OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public MobileClient(String                               ClientId,
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


        #region MobileAuthorizeStart(EVSEId, eMAId, PIN, PartnerProductId = null, GetNewSession = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task sending a mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="eMAId">The eMA identification.</param>
        /// <param name="PIN">The PIN of the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingMobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id    EVSEId,
                                 eMA_Id     eMAId,
                                 String     PIN,
                                 String     PartnerProductId  = null,
                                 Boolean?   GetNewSession     = null,
                                 TimeSpan?  QueryTimeout      = null)

        {

            return await MobileAuthorizeStart(EVSEId,
                                              new eMAIdWithPIN(eMAId, PIN),
                                              PartnerProductId,
                                              GetNewSession,
                                              QueryTimeout);

        }

        #endregion

        #region MobileAuthorizeStart(EVSEId, eMAId, HashedPIN, Function, Salt, PartnerProductId = null, GetNewSession = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task sending a mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="eMAId">The eMA identification.</param>
        /// <param name="HashedPIN">The PIN of the eMA identification.</param>
        /// <param name="Function">The crypto hash function of the eMA identification.</param>
        /// <param name="Salt">The Salt of the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingMobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id    EVSEId,
                                 eMA_Id     eMAId,
                                 String     HashedPIN,
                                 PINCrypto  Function,
                                 String     Salt,
                                 String     PartnerProductId  = null,
                                 Boolean?   GetNewSession     = null,
                                 TimeSpan?  QueryTimeout      = null)

        {

            return await MobileAuthorizeStart(EVSEId,
                                              new eMAIdWithPIN(eMAId, HashedPIN, Function, Salt),
                                              PartnerProductId,
                                              GetNewSession,
                                              QueryTimeout);

        }

        #endregion

        #region MobileAuthorizeStart(EVSEId, eMAIdWithPIN, PartnerProductId = null, GetNewSession = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task sending a mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="eMAIdWithPIN">The eMA identification with its PIN.</param>
        /// <param name="ProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingMobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id       EVSEId,
                                 eMAIdWithPIN  eMAIdWithPIN,
                                 String        ProductId      = null,
                                 Boolean?      GetNewSession  = null,
                                 TimeSpan?     QueryTimeout   = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingMobileAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(MobileClient_XMLMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                               eMAIdWithPIN,
                                                                                               ProductId,
                                                                                               GetNewSession),
                                               "eRoamingMobileAuthorizeStart",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingMobileAuthorizationStart.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingMobileAuthorizationStart>(httpresponse,
                                                                                                             new eRoamingMobileAuthorizationStart(-1,
                                                                                                                                                  Description: httpresponse.Content.ToString()),
                                                                                                             IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, soapclient, httpresponse);

                                                   return new HTTPResponse<eRoamingMobileAuthorizationStart>(httpresponse,
                                                                                                             new eRoamingMobileAuthorizationStart(-1,
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

        #region MobileRemoteStart(SessionId, QueryTimeout = null)

        /// <summary>
        /// Create a new task starting a remote charging session.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            MobileRemoteStart(ChargingSession_Id  SessionId,
                              TimeSpan?           QueryTimeout  = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingMobileAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(MobileClient_XMLMethods.MobileRemoteStartXML(SessionId),
                                               "eRoamingMobileRemoteStart",
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

        #region MobileRemoteStop(SessionId, QueryTimeout = null)

        /// <summary>
        /// Create a new task stopping a remote charging session.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            MobileRemoteStop(ChargingSession_Id  SessionId,
                             TimeSpan?           QueryTimeout  = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/HubjectMobileAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))

            {

                return await _OICPClient.Query(MobileClient_XMLMethods.MobileRemoteStopXML(SessionId),
                                               "eRoamingMobileRemoteStop",
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


    }

}
