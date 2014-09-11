/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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

using com.graphdefined.eMI3.LocalService;

#endregion

namespace com.graphdefined.eMI3.IO.OICP
{

    public class OICP_EMP_UpstreamService : AOICPUpstreamService, IRoamingProviderProvided_EVSPServices
    {

        #region Constructor(s)

        public OICP_EMP_UpstreamService(String           OICPHost,
                                        IPPort           OICPPort,
                                        String           HTTPVirtualHost  = null,
                                        Authorizator_Id  AuthorizatorId   = null)

            : base(OICPHost,
                   OICPPort,
                   HTTPVirtualHost,
                   "/ibis/ws/HubjectMobileAuthorization_V1",
                   AuthorizatorId)

        { }

        #endregion


        #region HubjectMobileAuthorizeStart(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public HubjectMobileAuthorizationStart HubjectMobileAuthorizeStart(EVSE_Id  EVSEId,
                                                                           eMA_Id   EVCOId,
                                                                           String   PIN,
                                                                           String   PartnerProductId = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost, URLPrefix))
                {

                    var HttpResponse = OICPClient.Query(EMPMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                           EVCOId,
                                                                                           PIN,
                                                                                           PartnerProductId).
                                                                                           ToString(),
                                                        "HubjectMobileAuthorizeStart");

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

        #region MobileRemoteStartXML(SessionId = null)

        public MobileRemoteStartResult MobileRemoteStartXML(ChargingSession_Id SessionId = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost, URLPrefix))
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

        public MobileRemoteStopResult MobileRemoteStopXML(ChargingSession_Id SessionId = null)
        {

            try
            {

                var IPv4Addresses = DNSClient.Query<A>(OICPHost).Select(a => a.IPv4Address).ToArray();

                using (var _OICPClient = new OICPClient(IPv4Addresses.First(), OICPPort, HTTPVirtualHost, URLPrefix))
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
