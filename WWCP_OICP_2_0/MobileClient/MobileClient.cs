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
    /// A simple OICP v2.0 Mobile client.
    /// </summary>
    public class MobileClient : AOICPUpstreamService
    {

        private Authorizator_Id AuthorizatorId = Authorizator_Id.Parse("");

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 Mobile client.
        /// </summary>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">An optional OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public MobileClient(String              Hostname,
                            IPPort              TCPPort          = null,
                            String              HTTPVirtualHost  = null,
                            String              HTTPUserAgent    = "GraphDefined OICP v2.0 MobileClient",
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
        /// Create a new task starting a remote charging session.
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
        /// Create a new task stopping a remote charging session.
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
