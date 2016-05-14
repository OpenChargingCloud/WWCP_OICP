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
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 CPO client.
    /// </summary>
    public class CentralClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined OICP v2.0 Central Client";

        private readonly Random _Random;

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data record will be send upstream.
        /// </summary>
        public event OnEVSEDataPushDelegate   OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever new EVSE data record had been sent upstream.
        /// </summary>
        public event OnEVSEDataPushedDelegate OnEVSEDataPushed;

        #endregion

        #endregion

        #region Properties

        #region URIPrefix

        private readonly String _URIPrefix;

        public String URIPrefix
        {
            get
            {
                return _URIPrefix;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 Central Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="TCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CentralClient(String                               ClientId,
                             String                               Hostname,
                             IPPort                               TCPPort                     = null,
                             String                               HTTPVirtualHost             = null,
                             RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                             String                               URIPrefix                   = "/ibis/ws/eRoamingAuthorization_V2.0",
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

        {

            this._Random     = new Random(DateTime.Now.Millisecond);

            this._URIPrefix  = URIPrefix.IsNotNullOrEmpty() ? URIPrefix : "/ibis/ws/eRoamingAuthorization_V2.0";

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAuthorizationStart>>

            AuthorizeRemoteStart(ChargingSession_Id  SessionId,
                                 EVSP_Id             ProviderId,
                                 EVSE_Id             EVSEId,
                                 eMAIdWithPIN        eMAIdWithPIN,
                                 ChargingProduct_Id  ProductId         = null,
                                 ChargingSession_Id  PartnerSessionId  = null,
                                 TimeSpan?           QueryTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    _URIPrefix,
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient: _DNSClient))
            {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <Authorization:eRoamingAuthorizeRemoteStart>
            // 
            //          <!--Optional:-->
            //          <Authorization:SessionID>?</Authorization:SessionID>
            // 
            //          <!--Optional:-->
            //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
            // 
            //          <Authorization:ProviderID>?</Authorization:ProviderID>
            //          <Authorization:EVSEID>?</Authorization:EVSEID>
            // 
            //          <Authorization:Identification>
            //             <!--You have a CHOICE of the next 4 items at this level-->
            //
            //             <CommonTypes:RFIDmifarefamilyIdentification>
            //                <CommonTypes:UID>?</CommonTypes:UID>
            //             </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //             <CommonTypes:QRCodeIdentification>
            // 
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            // 
            //                <!--You have a CHOICE of the next 2 items at this level-->
            //                <CommonTypes:PIN>?</CommonTypes:PIN>
            // 
            //                <CommonTypes:HashedPIN>
            //                   <CommonTypes:Value>?</CommonTypes:Value>
            //                   <CommonTypes:Function>?</CommonTypes:Function>
            //                   <CommonTypes:Salt>?</CommonTypes:Salt>
            //                </CommonTypes:HashedPIN>
            // 
            //             </CommonTypes:QRCodeIdentification>
            // 
            //             <CommonTypes:PlugAndChargeIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:PlugAndChargeIdentification>
            // 
            //             <CommonTypes:RemoteIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:RemoteIdentification>
            // 
            //          </Authorization:Identification>
            // 
            //          <!--Optional:-->
            //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
            // 
            //       </Authorization:eRoamingAuthorizeRemoteStart>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            var XML = SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart",

                                             new XElement(OICPNS.Authorization + "SessionID", SessionId.ToString()),

                                             PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,

                                             new XElement(OICPNS.Authorization + "ProviderID", ProviderId.ToString()),

                                             EVSEId != null
                                                 ? new XElement(OICPNS.Authorization + "EVSEID", EVSEId.OriginId)
                                                 : null,

                                             new XElement(OICPNS.Authorization + "Identification",
                                                 new XElement(OICPNS.CommonTypes + "QRCodeIdentification",
                                                     new XElement(OICPNS.CommonTypes + "EVCOID", eMAIdWithPIN.eMAId.ToString())
                                                 )
                                             )

                                         ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteStart",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => {
                                                   return XMLResponse.Parse(eRoamingAuthorizationStart.Parse);
                                               },

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                       new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                                                      StatusCode: new StatusCode(-1,
                                                                                                                                                                 Description: httpresponse.Content.ToString())),
                                                                                                       IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                       new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                                                      StatusCode: new StatusCode(-1,
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

        #region AuthorizeStop (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, QueryTimeout = null)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP v2.0 authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAuthorizationStop>>

            AuthorizeStop(EVSEOperator_Id      OperatorId,
                          ChargingSession_Id   SessionId,
                          Auth_Token           AuthToken,
                          EVSE_Id              EVSEId            = null,
                          ChargingSession_Id   PartnerSessionId  = null,   // [maxlength: 50]
                          TimeSpan?            QueryTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient: _DNSClient))
            {

                return await _OICPClient.Query(CPOClientXMLMethods.AuthorizeStopXML(OperatorId,
                                                                                     SessionId,
                                                                                     AuthToken,
                                                                                     EVSEId,
                                                                                     PartnerSessionId),
                                               "eRoamingAuthorizeStop",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAuthorizationStop.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                      new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                                                    StatusCode: new StatusCode(-1,
                                                                                                                                                               Description: httpresponse.Content.ToString())),
                                                                                                      IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                      new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                                                    StatusCode: new StatusCode(-1,
                                                                                                                                                               Description: httpresponse.HTTPStatusCode.ToString(),
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

        public Task AuthorizeRemoteStart(object p)
        {
            throw new NotImplementedException();
        }

        #endregion


    }

}
