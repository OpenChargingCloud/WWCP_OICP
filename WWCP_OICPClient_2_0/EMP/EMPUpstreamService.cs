/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    /// <summary>
    /// OICP v2.0 EMP upstream services.
    /// </summary>
    public class EMPUpstreamService : AOICPUpstreamService,
                                      IRoamingProviderProvided_EVSPServices
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 EMP upstream service.
        /// </summary>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="TCPPort">The OICP TCP port to connect to.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="AuthorizatorId">An optional authorizator identification to use.</param>
        /// <param name="UserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPUpstreamService(String           Hostname,
                                  IPPort           TCPPort,
                                  String           HTTPVirtualHost  = null,
                                  Authorizator_Id  AuthorizatorId   = null,
                                  String           UserAgent        = "GraphDefined OICP Gateway",
                                  TimeSpan?        QueryTimeout     = null,
                                  DNSClient        DNSClient        = null)

            : base(Hostname,
                   TCPPort,
                   HTTPVirtualHost,
                   AuthorizatorId,
                   UserAgent,
                   QueryTimeout,
                   DNSClient)

        { }

        #endregion


        #region GetEVSEByIdRequest(EVSEId, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the static EVSE data
        /// for the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public Task<HTTPResponse<EVSEDataRecord>>

            GetEVSEByIdRequest(EVSE_Id    EVSEId,
                               TimeSpan?  QueryTimeout  = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        "service-qa.hubject.com",
                                                        "/ibis/ws/eRoamingEvseData_V2.0",
                                                        UserAgent,
                                                        DNSClient))
                {

                    return _OICPClient.Query(EMP_XMLMethods.GetEVSEByIdRequestXML(EVSEId),
                                             "eRoamingEvseById",
                                             QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                             OnSuccess: XMLData =>

                                             #region Documentation

                                             // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                                             //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
                                             //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                                             //   <soapenv:Header/>
                                             //   <soapenv:Body>
                                             //      <v2:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
                                             //          [...]
                                             //      </v2:eRoamingEvseDataRecord>
                                             //    </soapenv:Body>
                                             // </soapenv:Envelope>

                                             #endregion

                                             new HTTPResponse<EVSEDataRecord>(XMLData.HttpResponse,
                                                                              EMP_XMLMethods.ParseEVSEDataRecord(XMLData.Content)),

                                             OnSOAPFault: Fault =>
                                                 new HTTPResponse<EVSEDataRecord>(
                                                     Fault.HttpResponse,
                                                     new Exception(Fault.Content.ToString())),

                                             OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                                             OnException: (t, s, e) => SendOnException(t, s, e)

                                            );

                }

            }

            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new Task<HTTPResponse<EVSEDataRecord>>(
                    () => new HTTPResponse<EVSEDataRecord>(e));

            }

        }

        #endregion

        #region PullEVSEDataRequest(ProviderId, GeoCoordinate = null, DistanceKM = 0, LastCall = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting all EVSE data.
        /// The request might either have none, a 'LastCall' or 'GeoCoordinate+DistanceKM' parameter.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for the geo coordinates [default: DecimalDegree]</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public Task<HTTPResponse<IEnumerable<XElement>>>

            PullEVSEDataRequest(EVSP_Id        ProviderId,
                                GeoCoordinate  SearchCenter  = null,
                                UInt64         DistanceKM    = 0,
                                DateTime?      LastCall      = null,
                                TimeSpan?      QueryTimeout  = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        "service-qa.hubject.com",
                                                        "/ibis/ws/eRoamingEvseData_V2.0",
                                                        UserAgent,
                                                        DNSClient))
                {

                    return _OICPClient.Query(EMP_XMLMethods.PullEVSEDataRequestXML(ProviderId,
                                                                                   SearchCenter,
                                                                                   DistanceKM,
                                                                                   LastCall),
                                             "eRoamingPullEVSEData",
                                             QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                             OnSuccess: XMLData => {

                                                 #region Hubject error?

                                                 var HubjectError = XMLData.
                                                                        Content.
                                                                        Element(OICPNS.EVSEStatus + "StatusCode");

                                                 if (HubjectError != null)
                                                 {

                                                     // <tns:eRoamingEvseStatusById xmlns:tns="http://www.hubject.com/b2b/services/evsestatus/v1.2">
                                                     //   <tns:StatusCode>
                                                     //     <cmn:Code        xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">002</cmn:Code>
                                                     //     <cmn:Description xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">Hubject database error</cmn:Description>
                                                     //   </tns:StatusCode>
                                                     // </tns:eRoamingEvseStatusById>

                                                     var Code         = HubjectError.Element(OICPNS.CommonTypes + "Code").       Value;
                                                     var Description  = HubjectError.Element(OICPNS.CommonTypes + "Description").Value;
                                                     var Exception    = new ApplicationException(Code + " - " + Description);

                                                     SendOnException(DateTime.Now, this, Exception);

                                                     return new HTTPResponse<IEnumerable<XElement>>(Exception);

                                                 }

                                                 #endregion

                                                 try
                                                 {

                                                     return new HTTPResponse<IEnumerable<XElement>>(
                                                                XMLData.HttpResponse,
                                                                XMLData.Content.
                                                                    Element (OICPNS.EVSEData + "EvseData").
                                                                    Elements(OICPNS.EVSEData + "OperatorEvseData"));

                                                 }
                                                 catch (Exception e)
                                                 {
                                                     DebugX.Log("'PullEVSEDataRequest' led to an exception: " + e.Message);
                                                     SendOnException(DateTime.Now, this, e);
                                                     return new HTTPResponse<IEnumerable<XElement>>(e);
                                                 }

                                             },

                                             #region SOAP fault, HTTP error, exception...

                                             OnSOAPFault: Fault => {

                                                 DebugX.Log("'PullEVSEDataRequest' lead to a SOAP fault!");

                                                 return new HTTPResponse<IEnumerable<XElement>>(
                                                     Fault.HttpResponse,
                                                     new XElement[1] { Fault.Content },
                                                     IsFault: true);

                                             },

                                             OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                                             OnException: (t, s, e) => SendOnException(t, s, e)

                                             #endregion

                                            );

                }

            }

            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new Task<HTTPResponse<IEnumerable<XElement>>>(
                    () => new HTTPResponse<IEnumerable<XElement>>(e));

            }

        }

        #endregion

        #region PullEVSEStatusByIdRequest(ProviderId, EVSEIds, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public Task<HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>>

            PullEVSEStatusByIdRequest(EVSP_Id               ProviderId,
                                      IEnumerable<EVSE_Id>  EVSEIds,
                                      TimeSpan?             QueryTimeout = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        "service-qa.hubject.com",
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    return _OICPClient.Query(EMP_XMLMethods.PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds),
                                             "eRoamingPullEvseStatusById",
                                             QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                             OnSuccess: XMLData => {

                                                 #region Documentation

                                                 // <?xml version='1.0' encoding='UTF-8'?>
                                                 // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1.2"
                                                 //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                                                 //                   xmlns:tns     = "http://www.hubject.com/b2b/services/evsestatus/v1.2">
                                                 //
                                                 //   <soapenv:Body>
                                                 //     <tns:eRoamingEvseStatusById>
                                                 //       <tns:EvseStatusRecords>
                                                 //
                                                 //         <tns:EvseStatusRecord>
                                                 //           <tns:EvseId>DK*045*E010*096296</tns:EvseId>
                                                 //           <tns:EvseStatus>EvseNotFound</tns:EvseStatus>
                                                 //         </tns:EvseStatusRecord>
                                                 //
                                                 //         <tns:EvseStatusRecord>
                                                 //           <tns:EvseId>SE*899*E02423*01</tns:EvseId>
                                                 //           <tns:EvseStatus>EvseNotFound</tns:EvseStatus>
                                                 //         </tns:EvseStatusRecord>
                                                 //
                                                 //       </tns:EvseStatusRecords>
                                                 //     </tns:eRoamingEvseStatusById>
                                                 //
                                                 //   </soapenv:Body>
                                                 //
                                                 // </soapenv:Envelope>

                                                 #endregion

                                                 #region Hubject error?

                                                 var HubjectError = XMLData.
                                                                        Content.
                                                                        Element(OICPNS.EVSEStatus + "StatusCode");

                                                 if (HubjectError != null)
                                                 {

                                                     // <tns:eRoamingEvseStatusById xmlns:tns="http://www.hubject.com/b2b/services/evsestatus/v1.2">
                                                     //   <tns:StatusCode>
                                                     //     <cmn:Code        xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">002</cmn:Code>
                                                     //     <cmn:Description xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">Hubject database error</cmn:Description>
                                                     //   </tns:StatusCode>
                                                     // </tns:eRoamingEvseStatusById>

                                                     var Code         = HubjectError.Element(OICPNS.CommonTypes + "Code").       Value;
                                                     var Description  = HubjectError.Element(OICPNS.CommonTypes + "Description").Value;
                                                     var Exception    = new ApplicationException(Code + " - " + Description);

                                                     SendOnException(DateTime.Now, this, Exception);

                                                     return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(Exception);

                                                 }

                                                 #endregion

                                                 try
                                                 {

                                                     return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(
                                                                XMLData.HttpResponse,
                                                                XMLData.Content.
                                                                        Element (OICPNS.EVSEStatus + "EvseStatusRecords").
                                                                        Elements(OICPNS.EVSEStatus + "EvseStatusRecord").
                                                                        Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(EVSE_Id.Parse(v.Element(OICPNS.EVSEStatus + "EvseId").Value),
                                                                                                                                (HubjectEVSEState)Enum.Parse(typeof(HubjectEVSEState), v.Element(OICPNS.EVSEStatus + "EvseStatus").Value))).
                                                                        ToArray() as IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>);

                                                 }
                                                 catch (Exception e)
                                                 {
                                                     DebugX.Log("'PullEVSEStatusByIdRequest' led to an exception: " + e.Message);
                                                     SendOnException(DateTime.Now, this, e);
                                                     return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(e);
                                                 }

                                             },

                                             #region SOAP fault, HTTP error, exception...

                                             OnSOAPFault: Fault => {

                                                 DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                 return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(
                                                     Fault.HttpResponse,
                                                     new KeyValuePair<EVSE_Id, HubjectEVSEState>[0],
                                                     IsFault: true);

                                             },

                                             OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                                             OnException: (t, s, e) => SendOnException(t, s, e)

                                             #endregion

                                            );

                }

            }

            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new Task<HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>>(
                    () => new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(e));

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
        public HubjectMobileAuthorizationStart HubjectMobileAuthorizeStart(EVSE_Id  EVSEId,
                                                                           eMA_Id   EVCOId,
                                                                           String   PIN,
                                                                           String   PartnerProductId = null)
        {

            try
            {

                using (var OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/HubjectMobileAuthorization_V1"))
                {

                    var HttpResponse = OICPClient.Query(EMP_XMLMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                           EVCOId,
                                                                                           PIN,
                                                                                           PartnerProductId),
                                                        "eRoamingMobileAuthorizeStart");

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

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/HubjectMobileAuthorization_V1"))
                {

                    var HttpResponse = _OICPClient.Query(EMP_XMLMethods.MobileRemoteStartXML(SessionId),
                                                         "eRoamingMobileRemoteStart");

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

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/HubjectMobileAuthorization_V1"))
                {

                    var HttpResponse = _OICPClient.Query(EMP_XMLMethods.MobileRemoteStopXML(SessionId),
                                                         "eRoamingMobileRemoteStop");

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
