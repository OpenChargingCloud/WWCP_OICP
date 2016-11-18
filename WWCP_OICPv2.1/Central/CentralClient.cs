/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Xml.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// An OICP Central client.
    /// </summary>
    public class CentralClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OICP " + Version.Number + " Central Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort     = IPPort.Parse(443);

        private readonly Random _Random;

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data record will be send upstream.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate   OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever new EVSE data record had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate OnEVSEDataPushed;

        #endregion
        #endregion

        #region Properties

        public String URIPrefix { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP Central client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CentralClient(String                               ClientId,
                             String                               Hostname,
                             IPPort                               RemotePort                  = null,
                             String                               HTTPVirtualHost             = null,
                             RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                             X509Certificate                      ClientCert                  = null,
                             String                               URIPrefix                   = "/ibis/ws/eRoamingAuthorization_V2.0",
                             String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                             TimeSpan?                            RequestTimeout                = null,
                             DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   "",
                   HTTPUserAgent,
                   RequestTimeout,
                   DNSClient)

        {

            this._Random     = new Random(DateTime.Now.Millisecond);

            this.URIPrefix  = URIPrefix.IsNotNullOrEmpty() ? URIPrefix : "/ibis/ws/eRoamingAuthorization_V2.0";

        }

        #endregion




        // Towards CPOs

        #region AuthorizeRemoteReservationStart(SessionId, ProviderId, EVSEId, eMAId, ChargingProductId = null, PartnerSessionId = null, RequestTimeout = null)

        /// <summary>
        /// Create an OICP authorize remote start request.
        /// </summary>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="EVCOId">An e-mobility account indentification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            AuthorizeRemoteReservationStart(Session_Id          SessionId,
                                            Provider_Id         ProviderId,
                                            EVSE_Id             EVSEId,
                                            EVCO_Id             EVCOId,
                                            PartnerProduct_Id?  ChargingProductId   = null,
                                            PartnerSession_Id?  PartnerSessionId    = null,
                                            TimeSpan?           RequestTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/Reservation",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.0"
                //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Reservation:eRoamingAuthorizeRemoteReservationStart>
                //
                //          <!--Optional:-->
                //          <Reservation:SessionID>?</Reservation:SessionID>
                //
                //          <!--Optional:-->
                //          <Reservation:PartnerSessionID>?</Reservation:PartnerSessionID>
                //
                //          <Reservation:ProviderID>?</Reservation:ProviderID>
                //          <Reservation:EVSEID>?</Reservation:EVSEID>
                //
                //          <Reservation:Identification>
                //
                //             <!--You have a CHOICE of the next 4 items at this level-->
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
                //          </Reservation:Identification>
                //
                //          <!--Optional:-->
                //          <Reservation:PartnerProductID>?</Reservation:PartnerProductID>
                //
                //       </Reservation:eRoamingAuthorizeRemoteReservationStart>
                //    </soapenv:Body>
                // </soapenv:Envelope>

                #endregion

                var XML = SOAP.Encapsulation(new XElement(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart",

                                                 new XElement(OICPNS.Reservation + "SessionID",   SessionId. ToString()),

                                                 PartnerSessionId != null
                                                     ? new XElement(OICPNS.Reservation + "PartnerSessionID", PartnerSessionId.ToString())
                                                     : null,

                                                 new XElement(OICPNS.Reservation + "ProviderID",  ProviderId.ToString()),
                                                 new XElement(OICPNS.Reservation + "EVSEID",      EVSEId.    ToString()),

                                                 new XElement(OICPNS.Reservation + "Identification",
                                                     new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                                         new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString())
                                                     )
                                                 ),

                                                 ChargingProductId != null
                                                     ? new XElement(OICPNS.Reservation + "PartnerProductID", ChargingProductId.ToString())
                                                     : null

                                             ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteReservationStart",
                                               QueryTimeout: RequestTimeout.HasValue ? RequestTimeout.Value : this.RequestTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => {
                                                   return XMLResponse.ConvertContent(Acknowledgement.Parse);
                                               },

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                    IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
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

        #region AuthorizeRemoteReservationStop(SessionId, ProviderId, EVSEId, PartnerSessionId = null, RequestTimeout = null)

        /// <summary>
        /// Create an OICP remote authorize stop request.
        /// </summary>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            AuthorizeRemoteReservationStop(Session_Id          SessionId,
                                           Provider_Id         ProviderId,
                                           EVSE_Id             EVSEId,
                                           PartnerSession_Id?  PartnerSessionId   = null,
                                           TimeSpan?           RequestTimeout     = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/Reservation",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Reservation:eRoamingAuthorizeRemoteReservationStop>
                //
                //          <Reservation:SessionID>?</Authorization:SessionID>
                //
                //          <!--Optional:-->
                //          <Reservation:PartnerSessionID>?</Authorization:PartnerSessionID>
                //
                //          <Reservation:ProviderID>?</Authorization:ProviderID>
                //          <Reservation:EVSEID>?</Authorization:EVSEID>
                //
                //       </Reservation:eRoamingAuthorizeRemoteReservationStop>
                //    </soapenv:Body>
                //
                // </soapenv:Envelope>

                #endregion

                var XML = SOAP.Encapsulation(new XElement(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop",

                                                 new XElement(OICPNS.Reservation + "SessionID",   SessionId. ToString()),

                                                 PartnerSessionId != null
                                                     ? new XElement(OICPNS.Reservation + "PartnerSessionID", PartnerSessionId.ToString())
                                                     : null,

                                                 new XElement(OICPNS.Reservation + "ProviderID",  ProviderId.ToString()),
                                                 new XElement(OICPNS.Reservation + "EVSEID",      EVSEId.    ToString())

                                             ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteReservationStop",
                                               QueryTimeout: RequestTimeout.HasValue ? RequestTimeout.Value : this.RequestTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => {
                                                   return XMLResponse.ConvertContent(Acknowledgement.Parse);
                                               },

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                    IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
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


        #region AuthorizeRemoteStart(SessionId, ProviderId, EVSEId, eMAId, ChargingProductId = null, PartnerSessionId = null, RequestTimeout = null)

        /// <summary>
        /// Create an OICP authorize remote start request.
        /// </summary>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="eMAId">An e-mobility account indentification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            AuthorizeRemoteStart(Session_Id          SessionId,
                                 Provider_Id         ProviderId,
                                 EVSE_Id             EVSEId,
                                 EVCO_Id             EVCOId,
                                 PartnerProduct_Id?  ChargingProductId   = null,
                                 PartnerSession_Id?  PartnerSessionId    = null,
                                 TimeSpan?           RequestTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/Authorization",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
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

                                                 new XElement(OICPNS.Authorization + "SessionID",  SessionId. ToString()),

                                                 PartnerSessionId != null
                                                     ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString())
                                                     : null,

                                                 new XElement(OICPNS.Authorization + "ProviderID", ProviderId.ToString()),
                                                 new XElement(OICPNS.Authorization + "EVSEID",     EVSEId.    ToString()),

                                                 new XElement(OICPNS.Authorization + "Identification",
                                                     new XElement(OICPNS.CommonTypes + "QRCodeIdentification",
                                                         new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString())
                                                     )
                                                 ),

                                                 ChargingProductId != null
                                                     ? new XElement(OICPNS.Authorization + "PartnerProductID", ChargingProductId.ToString())
                                                     : null

                                             ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteStart",
                                               QueryTimeout: RequestTimeout.HasValue ? RequestTimeout.Value : this.RequestTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => {
                                                   return XMLResponse.ConvertContent(Acknowledgement.Parse);
                                               },

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                    IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
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

        #region AuthorizeRemoteStop(SessionId, ProviderId, EVSEId, PartnerSessionId = null, RequestTimeout = null)

        /// <summary>
        /// Create an OICP remote authorize stop request.
        /// </summary>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            AuthorizeRemoteStop(Session_Id          SessionId,
                                Provider_Id         ProviderId,
                                EVSE_Id             EVSEId,
                                PartnerSession_Id?  PartnerSessionId   = null,
                                TimeSpan?           RequestTimeout     = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/Authorization",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Authorization:eRoamingAuthorizeRemoteStop>
                // 
                //          <Authorization:SessionID>?</Authorization:SessionID>
                // 
                //          <!--Optional:-->
                //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
                // 
                //          <Authorization:ProviderID>?</Authorization:ProviderID>
                // 
                //          <Authorization:EVSEID>?</Authorization:EVSEID>
                // 
                //       </Authorization:eRoamingAuthorizeRemoteStop>
                //    </soapenv:Body>
                //
                // </soapenv:Envelope>

                #endregion

                var XML = SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop",

                                                 new XElement(OICPNS.Authorization + "SessionID",   SessionId. ToString()),

                                                 PartnerSessionId.HasValue
                                                     ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString())
                                                     : null,

                                                 new XElement(OICPNS.Authorization + "ProviderID",  ProviderId.ToString()),
                                                 new XElement(OICPNS.Authorization + "EVSEID",      EVSEId.    ToString())

                                             ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteStop",
                                               QueryTimeout: RequestTimeout.HasValue ? RequestTimeout.Value : this.RequestTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => {
                                                   return XMLResponse.ConvertContent(Acknowledgement.Parse);
                                               },

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                    IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                    new Acknowledgement(StatusCodes.SystemError,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
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
