/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod;
using eu.Vanaheimr.Hermod.HTTP;
using eu.Vanaheimr.Hermod.Services.DNS;

using org.emi3group.LocalService;

#endregion

namespace org.emi3group.IO.OICP
{

    public class OICPClient : HTTPClient
    {

        #region Properties

        #region HTTPVirtualHost

        private readonly String _HTTPVirtualHost;

        public String HTTPVirtualHost
        {
            get
            {
                return _HTTPVirtualHost;
            }
        }

        #endregion

        #endregion

        #region Constructor

        public OICPClient(IPv4Address  OICPHost,
                              IPPort       OICPPort,
                              String       HTTPVirtualHost)

            : base(OICPHost, OICPPort)

        {
            this._HTTPVirtualHost = HTTPVirtualHost;
        }

        #endregion

        #region Query

        public HTTPResponse Query(String Query, String SOAPAction)
        {

            var builder = this.POST("/ibis/ws/HubjectAuthorization_V1");
            builder.Host         = HTTPVirtualHost;
            builder.Content      = Query.ToUTF8Bytes();
            builder.ContentType  = HTTPContentType.XMLTEXT_UTF8;
            builder.Set("SOAPAction", SOAPAction);
            builder.UserAgent    = "Belectric Drive Hubject Gateway";

            return this.Execute_Synced(builder);

        }

        #endregion

    }


    public class OICPUpstreamService : IUpstreamEMobilityService
    {

        #region Data

        private DNSClient DNSClient;

        #endregion

        #region Properties

        #region AuthorizatorId

        private readonly AuthorizatorId _AuthorizatorId;

        public AuthorizatorId AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region OICPHost

        private readonly String _OICPHost;

        public String OICPHost
        {
            get
            {
                return _OICPHost;
            }
        }

        #endregion

        #region OICPPort

        private readonly IPPort _OICPPort;

        public IPPort OICPPort
        {
            get
            {
                return _OICPPort;
            }
        }

        #endregion

        #region HTTPVirtualHost

        private readonly String _HTTPVirtualHost;

        public String HTTPVirtualHost
        {
            get
            {
                return _HTTPVirtualHost;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public OICPUpstreamService(String          OICPHost,
                                   IPPort          OICPPort,
                                   String          HTTPVirtualHost = null,
                                   AuthorizatorId  AuthorizatorId  = null)
        {

            this._OICPHost         = OICPHost;
            this._OICPPort         = OICPPort;
            this._HTTPVirtualHost  = (HTTPVirtualHost != null) ? HTTPVirtualHost : OICPHost;
            this._AuthorizatorId   = (AuthorizatorId  == null) ? AuthorizatorId.Parse("OICP Gateway") : AuthorizatorId;
            this.DNSClient         = new DNSClient(SearchForIPv6Servers: false);

        }

        #endregion


        #region AuthorizeStart(OperatorId, EVSEId, PartnerSessionId, UID)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id  OperatorId,
                                              EVSE_Id          EVSEId,
                                              SessionId        PartnerSessionId,
                                              Token            UID)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.AuthorizeStartXML(OperatorId,
                                                                                      EVSEId,
                                                                                      PartnerSessionId,
                                                                                      UID).
                                                                                      ToString(),
                                                         "HubjectAuthorizeStart");

                    //ToDo: In case of errors this will not parse!
                    var AuthStartResult = HubjectAuthorizationStart.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    if (AuthStartResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStart>
                        //       <tns:SessionID>8fade8bd-0a88-1296-0f2f-41ae8a80af1b</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:ProviderID>BMW</tns:ProviderID>
                        //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>000</cmn:Code>
                        //         <cmn:Description>Success</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStart>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.Authorized,
                                       SessionId            = AuthStartResult.SessionID,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVServiceProvider_Id.Parse(AuthStartResult.ProviderID),
                                       Description          = AuthStartResult.Description
                                   };

                    #endregion

                    #region NotAuthorized

                    else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    {

                        //- Invalid OperatorId ----------------------------------------------------------------------

                        // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                        //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <isns:Body>
                        //     <wsc:HubjectAuthorizationStop>
                        //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                        //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                        //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                        //       <wsc:StatusCode>
                        //         <v1:Code>017</v1:Code>
                        //         <v1:Description>Unauthorized Access</v1:Description>
                        //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
                        //       </wsc:StatusCode>
                        //     </wsc:HubjectAuthorizationStop>
                        //   </isns:Body>
                        // </isns:Envelope>

                        if (AuthStartResult.Code == 017)
                            return new AUTHSTARTResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                       PartnerSessionId     = PartnerSessionId,
                                       Description          = AuthStartResult.Description + " - " + AuthStartResult.AdditionalInfo
                                   };


                        //- Invalid UID -----------------------------------------------------------------------------

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStart>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>320</cmn:Code>
                        //         <cmn:Description>Service not available</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStart>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        else
                            return new AUTHSTARTResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                           PartnerSessionId     = PartnerSessionId,
                                           Description          = AuthStartResult.Description
                                       };

                    }

                    #endregion

                }

            }

            catch (Exception e)
            {

                return new AUTHSTARTResult(AuthorizatorId) {
                               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                               PartnerSessionId     = PartnerSessionId,
                               Description          = "An exception occured: " + e.Message
                           };

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, EVSEId, SessionId, PartnerSessionId, UID)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public AUTHSTOPResult AuthorizeStop(EVSEOperator_Id  OperatorId,
                                            EVSE_Id          EVSEId,
                                            SessionId        SessionId,
                                            SessionId        PartnerSessionId,
                                            Token            UID)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.AuthorizeStopXML(OperatorId,
                                                                                     EVSEId,
                                                                                     SessionId,
                                                                                     PartnerSessionId,
                                                                                     UID).
                                                                                     ToString(),
                                                         "HubjectAuthorizeStop");

                    //ToDo: In case of errors this will not parse!
                    var AuthStopResult = HubjectAuthorizationStop.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    if (AuthStopResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStop>
                        //       <tns:SessionID>8f9cbd74-0a88-1296-2078-6e9cca762de2</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:ProviderID>BMW</tns:ProviderID>
                        //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>000</cmn:Code>
                        //         <cmn:Description>Success</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStop>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.Authorized,
                                       SessionId            = AuthStopResult.SessionID,
                                       PartnerSessionId     = PartnerSessionId,
                                       ProviderId           = EVServiceProvider_Id.Parse(AuthStopResult.ProviderID),
                                       Description          = AuthStopResult.Description
                                   };

                    #endregion

                    #region NotAuthorized

                    else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    {

                        //- Invalid OperatorId ----------------------------------------------------------------------

                        // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                        //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <isns:Body>
                        //     <wsc:HubjectAuthorizationStop>
                        //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                        //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                        //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                        //       <wsc:StatusCode>
                        //         <v1:Code>017</v1:Code>
                        //         <v1:Description>Unauthorized Access</v1:Description>
                        //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
                        //       </wsc:StatusCode>
                        //     </wsc:HubjectAuthorizationStop>
                        //   </isns:Body>
                        // </isns:Envelope>

                        if (AuthStopResult.Code == 017)
                            return new AUTHSTOPResult(AuthorizatorId) {
                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                       PartnerSessionId     = PartnerSessionId,
                                       Description          = AuthStopResult.Description + " - " + AuthStopResult.AdditionalInfo
                                   };


                        //- Invalid SessionId -----------------------------------------------------------------------

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStop>
                        //       <tns:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>400</cmn:Code>
                        //         <cmn:Description>Session is invalid</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStop>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>

                        //- Invalid UID -----------------------------------------------------------------------------

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                        //   <soapenv:Body>
                        //     <tns:HubjectAuthorizationStop>
                        //       <tns:SessionID>8f9cbd74-0a88-1296-2078-6e9cca762de2</tns:SessionID>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>102</cmn:Code>
                        //         <cmn:Description>RFID Authentication failed – invalid UID</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:HubjectAuthorizationStop>
                        //   </soapenv:Body>
                        // </soapenv:Envelope>


                        //- Invalid PartnerSessionId ----------------------------------------------------------------

                        // No checks!


                        //- EVSEID changed/is invalid! --------------------------------------------------------------

                        //   => Session is invalid

                        else

                            return new AUTHSTOPResult(AuthorizatorId) {
                                           AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                           PartnerSessionId     = PartnerSessionId,
                                           Description          = AuthStopResult.Description
                                       };

                    }

                    #endregion

                }

            }

            catch (Exception e)
            {

                return new AUTHSTOPResult(AuthorizatorId) {
                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    PartnerSessionId      = PartnerSessionId,
                    Description           = "An exception occured: " + e.Message
                };

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, UID, EVCOId, ChargeStart, ChargeEnd, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="UID">The optional RFID user identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="ChargeStart">The timestamp of the charging start.</param>
        /// <param name="ChargeEnd">The timestamp of the charging end.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="MeterValueStart">The initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">The final value of the energy meter.</param>
        public SENDCDRResult SendCDR(EVSE_Id     EVSEId,
                                     SessionId   SessionId,
                                     SessionId   PartnerSessionId,
                                     String      PartnerProductId,
                                     Token       UID,
                                     eMA_Id      EVCOId,
                                     DateTime    ChargeStart,
                                     DateTime    ChargeEnd,
                                     DateTime?   SessionStart    = null,
                                     DateTime?   SessionEnd      = null,
                                     Double?     MeterValueStart = null,
                                     Double?     MeterValueEnd   = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.SendChargeDetailRecordXML(EVSEId,
                                                                                              SessionId,
                                                                                              PartnerSessionId,
                                                                                              PartnerProductId,
                                                                                              UID,
                                                                                              EVCOId,
                                                                                              ChargeStart,
                                                                                              ChargeEnd,
                                                                                              SessionStart,
                                                                                              SessionEnd,
                                                                                              MeterValueStart,
                                                                                              MeterValueEnd).
                                                                                              ToString(),
                                                         "HubjectChargeDetailRecord");

                    //ToDo: In case of errors this will not parse!
                    var ack = HubjectAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new SENDCDRResult(AuthorizatorId) {
                            State             = true,
                            PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new SENDCDRResult(AuthorizatorId) {
                            State             = false,
                            PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                }

            }

            catch (Exception e)
            {

                return
                    new SENDCDRResult(AuthorizatorId) {
                        State             = false,
                        PartnerSessionId  = PartnerSessionId,
                        Description       = "An exception occured: " + e.Message
                    };

            }

        }

        #endregion


        #region HubjectMobileAuthorizeStart(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public AUTHSTARTResult HubjectMobileAuthorizeStart(EVSE_Id  EVSEId,
                                                           eMA_Id   EVCOId,
                                                           String   PIN,
                                                           String   PartnerProductId = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost))
                {

                    var HttpResponse = OICPClient.Query(EMPMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                           EVCOId,
                                                                                           PIN,
                                                                                           PartnerProductId).
                                                                                           ToString(),
                                                        "HubjectMobileAuthorizeStart");

                    //ToDo: In case of errors this will not parse!
                    var AuthStartResult = HubjectAuthorizationStart.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    //if (AuthStartResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                    //    // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <soapenv:Body>
                    //    //     <tns:HubjectAuthorizationStart>
                    //    //       <tns:SessionID>8fade8bd-0a88-1296-0f2f-41ae8a80af1b</tns:SessionID>
                    //    //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                    //    //       <tns:ProviderID>BMW</tns:ProviderID>
                    //    //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                    //    //       <tns:StatusCode>
                    //    //         <cmn:Code>000</cmn:Code>
                    //    //         <cmn:Description>Success</cmn:Description>
                    //    //       </tns:StatusCode>
                    //    //     </tns:HubjectAuthorizationStart>
                    //    //   </soapenv:Body>
                    //    // </soapenv:Envelope>

                    //    return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.Authorized,
                    //                   SessionId            = AuthStartResult.SessionID,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   ProviderId           = EVServiceProvider_Id.Parse(AuthStartResult.ProviderID),
                    //                   Description          = AuthStartResult.Description
                    //               };

                    #endregion

                    #region NotAuthorized

                    //else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    //{

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
                    //    //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
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

                    //}

                    #endregion

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

        // => HubjectMobileAuthorizationStart

        }

        #endregion

        #region MobileRemoteStartXML(SessionId = null)

        public MobileRemoteStartResult MobileRemoteStartXML(SessionId SessionId = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost))
                {

                    var HttpResponse = _OICPClient.Query(EMPMethods.MobileRemoteStartXML(SessionId).ToString(),
                                                         "HubjectMobileRemoteStart");

                    //ToDo: In case of errors this will not parse!
                    var ack = HubjectAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new MobileRemoteStartResult(AuthorizatorId) {
                            State             = true,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new MobileRemoteStartResult(AuthorizatorId) {
                            State             = false,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
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

        #region MobileRemoteStopXML(SessionId = null)

        public MobileRemoteStopResult MobileRemoteStopXML(SessionId SessionId = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost))
                {

                    var HttpResponse = _OICPClient.Query(EMPMethods.MobileRemoteStopXML(SessionId).ToString(),
                                                         "HubjectMobileRemoteStop");

                    //ToDo: In case of errors this will not parse!
                    var ack = HubjectAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new MobileRemoteStopResult(AuthorizatorId) {
                            State             = true,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new MobileRemoteStopResult(AuthorizatorId) {
                            State             = false,
                            //PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
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

    }

}
