/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Aegir;

using org.GraphDefined.eMI3.LocalService;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.eMI3.IO.OICP_1_2
{

    /// <summary>
    /// OICP EMP Upstream Service(s).
    /// </summary>
    public class OICP_EMP_UpstreamService : AOICPUpstreamService, IRoamingProviderProvided_EVSPServices
    {

        #region Data

        private const String UserAgent = "GraphDefined OICP-Client";

        #endregion

        #region Events

        #region OnException

        /// <summary>
        /// A delegate called whenever an exception occured.
        /// </summary>
        public delegate void OnExceptionDelegate(DateTime Timestamp, Object Sender, Exception Exception);

        /// <summary>
        /// An event fired whenever an exception occured.
        /// </summary>
        public event OnExceptionDelegate OnException;

        #endregion

        #region OnHTTPError

        /// <summary>
        /// A delegate called whenever a HTTP error occured.
        /// </summary>
        public delegate void OnHTTPErrorDelegate(DateTime Timestamp, Object Sender, HTTPResponse HttpResponse);

        /// <summary>
        /// An event fired whenever a HTTP error occured.
        /// </summary>
        public event OnHTTPErrorDelegate OnHTTPError;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP EMP Upstream Service(s).
        /// </summary>
        /// <param name="OICPHost"></param>
        /// <param name="OICPPort"></param>
        /// <param name="HTTPVirtualHost"></param>
        /// <param name="AuthorizatorId"></param>
        /// <param name="DNSClient"></param>
        public OICP_EMP_UpstreamService(String           OICPHost,
                                        IPPort           OICPPort,
                                        String           HTTPVirtualHost = null,
                                        Authorizator_Id  AuthorizatorId  = null,
                                        DNSClient        DNSClient       = null)

            : base(OICPHost,
                   OICPPort,
                   HTTPVirtualHost,
                   "/ibis/ws/HubjectMobileAuthorization_V1",
                   AuthorizatorId,
                   DNSClient)

        { }

        #endregion


        #region GetEVSEByIdRequest(...)

        public Task<HTTPResponse<XElement>> GetEVSEByIdRequest(EVSE_Id EVSEId)
        {

            try
            {

                using (var _OICPClient = new SOAPClient(OICPHost,
                                                        OICPPort,
                                                        "service-qa.hubject.com",
                                                        "/ibis/ws/eRoamingEvseData_V1.2",
                                                        UserAgent,
                                                        DNSClient))
                {

                    return _OICPClient.Query(EMPMethods.GetEVSEByIdRequestXML(EVSEId),
                                             "eRoamingEvseById",
                                             Timeout: TimeSpan.FromSeconds(180),

                                             OnSuccess: XMLData =>
                                                 new HTTPResponse<XElement>(
                                                     XMLData.HttpResponse,
                                                     XMLData.Content),

                                             OnSOAPFault: Fault =>
                                                 new HTTPResponse<XElement>(
                                                     Fault.HttpResponse,
                                                     Fault.Content,
                                                     IsFault: true),

                                             OnHTTPError: (t, s, e) => {

                                                 var OnHTTPErrorLocal = OnHTTPError;
                                                 if (OnHTTPErrorLocal != null)
                                                     OnHTTPErrorLocal(t, s, e);

                                             },

                                             OnException: (t, s, e) => {

                                                 var OnExceptionLocal = OnException;
                                                 if (OnExceptionLocal != null)
                                                     OnExceptionLocal(t, s, e);

                                             }

                                            );

                }

            }

            catch (Exception e)
            {

                var OnExceptionLocal = OnException;
                if (OnExceptionLocal != null)
                    OnExceptionLocal(DateTime.Now, this, e);

                return new Task<HTTPResponse<XElement>>(
                    () => new HTTPResponse<XElement>(e));

            }

        }

        #endregion

        #region PullEVSEDataRequest(...)

        /// <summary>
        /// Create a new task requesting all EVSE data.
        /// The request might either have none, a 'LastCall' or 'GeoCoordinate+DistanceKM' parameter.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="GeoCoordinate">An optional geo coordinate as search center.</param>
        /// <param name="DistanceKM">An optional geo coordinate as search radius.</param>
        /// <returns>A list of EVSE datasets.</returns>
        public Task<HTTPResponse<IEnumerable<XElement>>>

            PullEVSEDataRequest(EVSP_Id        ProviderId,
                                DateTime?      LastCall       = null,
                                GeoCoordinate  GeoCoordinate  = null,
                                UInt64         DistanceKM     = 0)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(OICPHost,
                                                        OICPPort,
                                                        "service-qa.hubject.com",
                                                        "/ibis/ws/eRoamingEvseData_V1.2",
                                                        UserAgent,
                                                        DNSClient))
                {

                    return _OICPClient.Query(EMPMethods.PullEVSEDataRequestXML(ProviderId, LastCall, GeoCoordinate, DistanceKM),
                                             "eRoamingPullEVSEData",
                                             Timeout: TimeSpan.FromSeconds(180),

                                             OnSuccess: XMLData =>

                                                 #region Documentation

                                                 // <tns:eRoamingEvseData xmlns:tns="http://www.hubject.com/b2b/services/evsedata/v1.2">
                                                 //   <tns:EvseData>
                                                 //
                                                 //     <tns:OperatorEvseData>
                                                 //       <tns:OperatorID>+45*045</tns:OperatorID>
                                                 //       <tns:OperatorName>CleanCharge</tns:OperatorName>
                                                 //
                                                 //       <tns:EvseDataRecord lastUpdate="2015-03-22T15:46:02.000Z">
                                                 //
                                                 //         <tns:EvseId>+45*045*010*096296</tns:EvseId>
                                                 //         <tns:ChargingStationName>Ladestation Stadt Kopenhagen</tns:ChargingStationName>
                                                 //         <tns:EnChargingStationName>Charging Station Copenhagen</tns:EnChargingStationName>
                                                 //
                                                 //         <tns:Address>
                                                 //           <cmn:Country    xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">DNK</cmn:Country>
                                                 //           <cmn:City       xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">København</cmn:City>
                                                 //           <cmn:Street     xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">Islands Brygge</cmn:Street>
                                                 //           <cmn:PostalCode xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">2300</cmn:PostalCode>
                                                 //           <cmn:HouseNum   xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">37</cmn:HouseNum>
                                                 //         </tns:Address>
                                                 //
                                                 //         <tns:GeoCoordinates>
                                                 //           <cmn:DecimalDegree xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                 //             <cmn:Longitude>12.574990</cmn:Longitude>
                                                 //             <cmn:Latitude>55.664520</cmn:Latitude>
                                                 //           </cmn:DecimalDegree>
                                                 //         </tns:GeoCoordinates>
                                                 //
                                                 //         <tns:Plugs>
                                                 //           <tns:Plug>Type 2 Outlet</tns:Plug>
                                                 //         </tns:Plugs>
                                                 //
                                                 //         <tns:ChargingFacilities>
                                                 //           <tns:ChargingFacility>Unspecified</tns:ChargingFacility>
                                                 //         </tns:ChargingFacilities>
                                                 //
                                                 //         <tns:AuthenticationModes>
                                                 //           <tns:AuthenticationMode>REMOTE</tns:AuthenticationMode>
                                                 //         </tns:AuthenticationModes>
                                                 //
                                                 //         <tns:Accessibility>Restricted access</tns:Accessibility>
                                                 //         <tns:HotlinePhoneNum>+00000</tns:HotlinePhoneNum>
                                                 //         <tns:IsOpen24Hours>true</tns:IsOpen24Hours>
                                                 //         <tns:IsHubjectCompatible>true</tns:IsHubjectCompatible>
                                                 //         <tns:DynamicInfoAvailable>true</tns:DynamicInfoAvailable>
                                                 //
                                                 //       </tns:EvseDataRecord>
                                                 //
                                                 //     </tns:OperatorEvseData>
                                                 //
                                                 //   </tns:EvseData>
                                                 // </tns:eRoamingEvseData>

                                                 #endregion

                                                 new HTTPResponse<IEnumerable<XElement>>(XMLData.HttpResponse,
                                                                                         XMLData.Content.
                                                                                                 Element (OICP_1_2.NS.OICPv1_2EVSEData + "EvseData").
                                                                                                 Elements(OICP_1_2.NS.OICPv1_2EVSEData + "OperatorEvseData").
                                                                                                 Elements(OICP_1_2.NS.OICPv1_2EVSEData + "EvseDataRecord")),

                                             OnSOAPFault: Fault => {

                                                 Debug.WriteLine("[" + DateTime.Now + "] 'PullEVSEDataRequest' lead to a fault!");

                                                 return new HTTPResponse<IEnumerable<XElement>>(
                                                     Fault.HttpResponse,
                                                     new XElement[1] { Fault.Content },
                                                     IsFault: true);

                                             },

                                             OnHTTPError: (t, s, e) => {

                                                 var OnHTTPErrorLocal = OnHTTPError;
                                                 if (OnHTTPErrorLocal != null)
                                                     OnHTTPErrorLocal(t, s, e);

                                             },

                                             OnException: (t, s, e) => {

                                                 var OnExceptionLocal = OnException;
                                                 if (OnExceptionLocal != null)
                                                     OnExceptionLocal(t, s, e);

                                             }

                                            );

                }

            }

            catch (Exception e)
            {

                var OnExceptionLocal = OnException;
                if (OnExceptionLocal != null)
                    OnExceptionLocal(DateTime.Now, this, e);

                return new Task<HTTPResponse<IEnumerable<XElement>>>(
                    () => new HTTPResponse<IEnumerable<XElement>>(e));

            }

        }

        #endregion

        #region PullEVSEStatusByIdRequest(...)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// <returns>An enumeration of EVSE Ids and their current status.</returns>
        public Task<HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>>

            PullEVSEStatusByIdRequest(EVSP_Id               ProviderId,
                                      IEnumerable<EVSE_Id>  EVSEIds)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(OICPHost,
                                                        OICPPort,
                                                        "service-qa.hubject.com",
                                                        "/ibis/ws/eRoamingEvseStatus_V1.2",
                                                        UserAgent,
                                                        DNSClient))

                {

                    return _OICPClient.Query(EMPMethods.PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds),
                                             "eRoamingPullEvseStatusById",
                                             Timeout: TimeSpan.FromSeconds(180),

                                             OnSuccess: XMLData =>

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

                                                 new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(
                                                     XMLData.HttpResponse,
                                                     XMLData.Content.
                                                             Element (OICP_1_2.NS.OICPv1_2EVSEStatus + "EvseStatusRecords").
                                                             Elements(OICP_1_2.NS.OICPv1_2EVSEStatus + "EvseStatusRecord").
                                                             Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(EVSE_Id.Parse(v.Element(OICP_1_2.NS.OICPv1_2EVSEStatus + "EvseId").Value),
                                                                                                                     (HubjectEVSEState) Enum.Parse(typeof(HubjectEVSEState), v.Element(OICP_1_2.NS.OICPv1_2EVSEStatus + "EvseStatus").Value))).
                                                             ToArray() as IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>),


                                             OnSOAPFault: Fault => {

                                                 Debug.WriteLine("[" + DateTime.Now + "] 'PullEVSEStatusByIdRequest' lead to a fault!");

                                                 return new HTTPResponse<IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>>(
                                                     Fault.HttpResponse,
                                                     new KeyValuePair<EVSE_Id, HubjectEVSEState>[0],
                                                     IsFault: true);

                                             },

                                             OnHTTPError: (t, s, e) => {

                                                 var OnHTTPErrorLocal = OnHTTPError;
                                                 if (OnHTTPErrorLocal != null)
                                                     OnHTTPErrorLocal(t, s, e);

                                             },

                                             OnException: (t, s, e) => {

                                                 var OnExceptionLocal = OnException;
                                                 if (OnExceptionLocal != null)
                                                     OnExceptionLocal(t, s, e);

                                             }

                                            );

                }

            }

            catch (Exception e)
            {

                var OnExceptionLocal = OnException;
                if (OnExceptionLocal != null)
                    OnExceptionLocal(DateTime.Now, this, e);

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

                using (var OICPClient = new SOAPClient(OICPHost, OICPPort, HTTPVirtualHost, URLPrefix))
                {

                    var HttpResponse = OICPClient.Query(EMPMethods.MobileAuthorizeStartXML(EVSEId,
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

                using (var _OICPClient = new SOAPClient(OICPHost, OICPPort, HTTPVirtualHost, URLPrefix))
                {

                    var HttpResponse = _OICPClient.Query(EMPMethods.MobileRemoteStartXML(SessionId),
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

                using (var _OICPClient = new SOAPClient(OICPHost, OICPPort, HTTPVirtualHost, URLPrefix))
                {

                    var HttpResponse = _OICPClient.Query(EMPMethods.MobileRemoteStopXML(SessionId),
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
