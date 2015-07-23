﻿/*
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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

using org.GraphDefined.WWCP.LocalService;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    /// <summary>
    /// OICPv1.2 CPO Upstream Service(s).
    /// </summary>
    public class CPOUpstreamService : AOICPUpstreamService,
                                      IAuthServices,
                                      IDataServices
    {

        #region Events

        #region OnNewEVSEStatusSending

        /// <summary>
        /// A delegate called whenever new EVSE status will be send upstream.
        /// </summary>
        public delegate void OnNewEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>> EVSEStatus, String Hostinfo, String TrackingId);

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnNewEVSEStatusSendingDelegate OnNewEVSEStatusSending;

        #endregion

        #region OnNewEVSEStatusSent

        /// <summary>
        /// A delegate called whenever new EVSE status had been send upstream.
        /// </summary>
        public delegate void OnNewEVSEStatusSentDelegate(DateTime Timestamp, String TrackingId, HubjectAcknowledgement Result, String Description = null);

        /// <summary>
        /// An event fired whenever new EVSE status had been send upstream.
        /// </summary>
        public event OnNewEVSEStatusSentDelegate OnNewEVSEStatusSent;

        #endregion

        #region OnChangedEVSEStatusSending

        /// <summary>
        /// A delegate called whenever changed EVSE status will be send upstream.
        /// </summary>
        public delegate void OnChangedEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>> EVSEStatus, String Hostinfo, String TrackingId);

        /// <summary>
        /// An event fired whenever changed EVSE status will be send upstream.
        /// </summary>
        public event OnChangedEVSEStatusSendingDelegate OnChangedEVSEStatusSending;

        #endregion

        #region OnChangedEVSEStatusSent

        /// <summary>
        /// A delegate called whenever changed EVSE status had been send upstream.
        /// </summary>
        public delegate void OnChangedEVSEStatusSentDelegate(DateTime Timestamp, String TrackingId, HubjectAcknowledgement Result, String Description = null);

        /// <summary>
        /// An event fired whenever changed EVSE status had been send upstream.
        /// </summary>
        public event OnChangedEVSEStatusSentDelegate OnChangedEVSEStatusSent;

        #endregion

        #region OnRemovedEVSEStatusSending

        /// <summary>
        /// A delegate called whenever removed EVSE status will be send upstream.
        /// </summary>
        public delegate void OnRemovedEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<EVSE_Id> EVSEIds, String Hostinfo, String TrackingId);

        /// <summary>
        /// An event fired whenever removed EVSE status will be send upstream.
        /// </summary>
        public event OnRemovedEVSEStatusSendingDelegate OnRemovedEVSEStatusSending;

        #endregion

        #region OnRemovedEVSEStatusSent

        /// <summary>
        /// A delegate called whenever removed EVSE status had been send upstream.
        /// </summary>
        public delegate void OnRemovedEVSEStatusSentDelegate(DateTime Timestamp, String TrackingId, HubjectAcknowledgement Result, String Description = null);

        /// <summary>
        /// An event fired whenever removed EVSE status had been send upstream.
        /// </summary>
        public event OnRemovedEVSEStatusSentDelegate OnRemovedEVSEStatusSent;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICPv1.2 CPO Upstream Service.
        /// </summary>
        /// <param name="OICPHost">The hostname of the OICP service.</param>
        /// <param name="OICPPort">The IP port of the OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the OICP service.</param>
        /// <param name="AuthorizatorId">An optional unique authorizator identification.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOUpstreamService(String           OICPHost,
                                  IPPort           OICPPort,
                                  String           HTTPVirtualHost  = null,
                                  Authorizator_Id  AuthorizatorId   = null,
                                  String           HTTPUserAgent    = "GraphDefined OICPv1.2 Gateway CPO Upstream Services",
                                  DNSClient        DNSClient        = null)

            : base(OICPHost,
                   OICPPort,
                   HTTPVirtualHost,
                   AuthorizatorId,
                   HTTPUserAgent,
                   DNSClient)

        { }

        #endregion


        #region Tokens

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AllTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> AuthorizedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> NotAuthorizedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, AuthorizationResult>> BlockedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, AuthorizationResult>[0];
            }
        }

        #endregion


        #region EVSEDataFullload(EVSEOperator)

        public Task<HTTPResponse<HubjectAcknowledgement>> EVSEDataFullload(EVSEOperator  EVSEOperator)

        {

            Console.WriteLine("FullLoad of " + EVSEOperator.ChargingPools.
                                                            SelectMany(Pool    => Pool.ChargingStations).
                                                            SelectMany(Station => Station.EVSEs).
                                                            Where     (EVSE    => !EVSEOperator.InvalidEVSEIds.Contains(EVSE.Id)).
                                                            Count() + " EVSE static data sets at " + _HTTPVirtualHost + "...");

            try
            {

                var EVSEDataFullLoadXML = EVSEOperator.
                                              ChargingPools.
                                              PushEVSEDataXML(EVSEOperator.Id,
                                                              EVSEOperator.Name[Languages.de],
                                                              ActionType.fullLoad,
                                                              IncludeEVSEs: EVSEId => !EVSEOperator.InvalidEVSEIds.Contains(EVSEId));

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseData_V2.0",
                                                        _UserAgent,
                                                        _DNSClient))
                    {

                        return _OICPClient.Query(EVSEDataFullLoadXML,
                                                   "eRoamingPushEvseData",
                                                   QueryTimeout: TimeSpan.FromSeconds(60),

                                                   OnSuccess: XMLData =>
                                                   {

                                                       // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                       //   <cmn:Result>true</cmn:Result>
                                                       //   <cmn:StatusCode>
                                                       //     <cmn:Code>000</cmn:Code>
                                                       //     <cmn:Description>Success</cmn:Description>
                                                       //     <cmn:AdditionalInfo />
                                                       //   </cmn:StatusCode>
                                                       // </cmn:eRoamingAcknowledgement>

                                                       // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                       //   <cmn:Result>false</cmn:Result>
                                                       //   <cmn:StatusCode>
                                                       //     <cmn:Code>009</cmn:Code>
                                                       //     <cmn:Description>Data transaction error</cmn:Description>
                                                       //     <cmn:AdditionalInfo>The Push of data is already in progress.</cmn:AdditionalInfo>
                                                       //   </cmn:StatusCode>
                                                       // </cmn:eRoamingAcknowledgement>

                                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                       Console.WriteLine("EVSE data fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                   },


                                                   OnSOAPFault: Fault =>
                                                   {

                                                       Console.WriteLine("EVSE data fullload lead to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(
                                                           Fault.HttpResponse,
                                                           new HubjectAcknowledgement(false, 0, "", ""),
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),
    
                                                   OnException: (t, s, e) => SendOnException(t, s, e)

                                                  );

                    }

            } catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

            }

            return Task.FromResult<HTTPResponse<HubjectAcknowledgement>>(new HTTPResponse<HubjectAcknowledgement>(null, null, true));

        }

        #endregion

        #region EVSEStatusFullload(EVSEOperator)

        public Task<HTTPResponse<HubjectAcknowledgement>> EVSEStatusFullload(EVSEOperator  EVSEOperator)

        {

            try
            {

                var XML_EVSEStates = EVSEOperator.
                                         AllEVSEStatus.
                                         Where(v => !EVSEOperator.InvalidEVSEIds.Contains(v.Key)).
                                         ToArray();

                if (XML_EVSEStates.Any())
                {

                    Console.WriteLine("FullLoad of " + XML_EVSEStates.Length + " EVSE states at " + _HTTPVirtualHost + "...");

                    var EVSEStatesInsertXML = XML_EVSEStates.
                                                  Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState())).
                                                  PushEVSEStatusXML(EVSEOperator.Id,
                                                                    EVSEOperator.Name[Languages.de],
                                                                    ActionType.fullLoad);

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                            _UserAgent,
                                                            _DNSClient))
                    {

                        return _OICPClient.Query(EVSEStatesInsertXML,
                                                   "eRoamingPushEvseStatus",
                                                   QueryTimeout: TimeSpan.FromSeconds(60),

                                                   OnSuccess: XMLData =>
                                                   {

                                                       // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v1.2">
                                                       //   <cmn:Result>true</cmn:Result>
                                                       //   <cmn:StatusCode>
                                                       //     <cmn:Code>000</cmn:Code>
                                                       //     <cmn:Description>Success</cmn:Description>
                                                       //     <cmn:AdditionalInfo />
                                                       //   </cmn:StatusCode>
                                                       // </cmn:eRoamingAcknowledgement>

                                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                       Console.WriteLine("EVSE states fullload: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                   },


                                                   OnSOAPFault: Fault =>
                                                   {

                                                       Console.WriteLine("EVSE states fullload lead to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<HubjectAcknowledgement>(
                                                           Fault.HttpResponse,
                                                           new HubjectAcknowledgement(false, 0, "", ""),
                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),
    
                                                   OnException: (t, s, e) => SendOnException(t, s, e)

                                                  );

                    }

                }

            }
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

            }

            return Task.FromResult<HTTPResponse<HubjectAcknowledgement>>(new HTTPResponse<HubjectAcknowledgement>(null, null, true));

        }

        #endregion

        #region SendEVSEStatusUpdates(EVSEStatusDiff)

        /// <summary>
        /// Send EVSE status updates upstream.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        public void SendEVSEStatusUpdates(EVSEStatusDiff  EVSEStatusDiff)

        {

            var TrackingId = Guid.NewGuid().ToString();

            Task<HTTPResponse<HubjectAcknowledgement>> NewEVSEStatusSendingTask      = null;
            Task<HTTPResponse<HubjectAcknowledgement>> ChangedEVSEStatusSendingTask  = null;
            Task<HTTPResponse<HubjectAcknowledgement>> RemovedEVSEIdsSendingTask     = null;


            #region Insert new EVSEs...

            if (EVSEStatusDiff.NewEVSEStatus.Any())
            {

                var NewEVSEStatus  = EVSEStatusDiff.
                                         NewEVSEStatus.
                                         Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState()));

                var OnNewEVSEStatusSendingLocal = OnNewEVSEStatusSending;
                if (OnNewEVSEStatusSendingLocal != null)
                    OnNewEVSEStatusSendingLocal(DateTime.Now, NewEVSEStatus, _HTTPVirtualHost, TrackingId);


                var EVSEStatesInsertXML = NewEVSEStatus.
                                              PushEVSEStatusXML(EVSEStatusDiff.EVSEOperatorId,
                                                                EVSEStatusDiff.EVSEOperatorName[Languages.de],
                                                                ActionType.insert);

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    NewEVSEStatusSendingTask = 
                        _OICPClient.
                             Query(EVSEStatesInsertXML,
                                   "eRoamingPushEvseStatus",
                                   QueryTimeout: TimeSpan.FromSeconds(180),

                                   OnSuccess: XMLData => {

                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);

                                       var OnNewEVSEStatusSentLocal = OnNewEVSEStatusSent;
                                       if (OnNewEVSEStatusSentLocal != null)
                                           OnNewEVSEStatusSentLocal(DateTime.Now, TrackingId, ack);

                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                   },


                                   OnSOAPFault: Fault => {

                                       var nack = new HubjectAcknowledgement(false, 0, "SOAPFault", "");

                                       var OnNewEVSEStatusSentLocal = OnNewEVSEStatusSent;
                                       if (OnNewEVSEStatusSentLocal != null)
                                           OnNewEVSEStatusSentLocal(DateTime.Now, TrackingId, nack);

                                       return new HTTPResponse<HubjectAcknowledgement>(
                                           Fault.HttpResponse,
                                           nack,
                                           IsFault: true);

                                   },

                                   OnHTTPError: (t, s, e) => {

                                       //var OnHTTPErrorLocal = OnHTTPError;
                                       //if (OnHTTPErrorLocal != null)
                                       //    OnHTTPErrorLocal(t, s, e);

                                   },

                                   OnException: (t, s, e) => {

                                       //var OnExceptionLocal = OnException;
                                       //if (OnExceptionLocal != null)
                                       //    OnExceptionLocal(t, s, e);

                                   }

                                  );

                }

            }

            #endregion

            #region Upload EVSE changes...

            if (EVSEStatusDiff.ChangedEVSEStatus.Any())
            {


                var ChangedEVSEStatus = EVSEStatusDiff.
                                            ChangedEVSEStatus.
                                            Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState()));

                var OnChangedEVSEStatusSendingLocal = OnChangedEVSEStatusSending;
                if (OnChangedEVSEStatusSendingLocal != null)
                    OnChangedEVSEStatusSendingLocal(DateTime.Now, ChangedEVSEStatus, _HTTPVirtualHost, TrackingId);


                var EVSEStatesUpdateXML = ChangedEVSEStatus.
                                              PushEVSEStatusXML(EVSEStatusDiff.EVSEOperatorId,
                                                                EVSEStatusDiff.EVSEOperatorName[Languages.de],
                                                                ActionType.update);


                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    ChangedEVSEStatusSendingTask = 
                        _OICPClient.
                             Query(EVSEStatesUpdateXML,
                                   "eRoamingPushEvseStatus",
                                   QueryTimeout: TimeSpan.FromSeconds(180),

                                   OnSuccess: XMLData => {

                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);

                                       DebugX.Log("eRoamingPushEvseStatus => " + ack.Result);

                                       var OnChangedEVSEStatusSentLocal = OnChangedEVSEStatusSent;
                                       if (OnChangedEVSEStatusSentLocal != null)
                                           OnChangedEVSEStatusSentLocal(DateTime.Now, TrackingId, ack);

                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                   },


                                   OnSOAPFault: Fault => {

                                       var nack = new HubjectAcknowledgement(false, 0, "SOAPFault", "");

                                       var OnChangedEVSEStatusSentLocal = OnChangedEVSEStatusSent;
                                       if (OnChangedEVSEStatusSentLocal != null)
                                           OnChangedEVSEStatusSentLocal(DateTime.Now, TrackingId, nack);

                                       return new HTTPResponse<HubjectAcknowledgement>(
                                           Fault.HttpResponse,
                                           nack,
                                           IsFault: true);

                                   },

                                   OnHTTPError: (t, s, e) => {

                                       //var OnHTTPErrorLocal = OnHTTPError;
                                       //if (OnHTTPErrorLocal != null)
                                       //    OnHTTPErrorLocal(t, s, e);

                                   },

                                   OnException: (t, s, e) => {

                                       //var OnExceptionLocal = OnException;
                                       //if (OnExceptionLocal != null)
                                       //    OnExceptionLocal(t, s, e);

                                   }

                                  );

                }

            }

            #endregion

            #region Remove outdated EVSEs...

            if (EVSEStatusDiff.RemovedEVSEIds.Any())
            {

                var OnRemovedEVSEStatusSendingLocal = OnRemovedEVSEStatusSending;
                if (OnRemovedEVSEStatusSendingLocal != null)
                    OnRemovedEVSEStatusSendingLocal(DateTime.Now, EVSEStatusDiff.ChangedEVSEStatus.Select(v => v.Key), _HTTPVirtualHost, TrackingId);


                var RemovedEVSEStatus = EVSEStatusDiff.
                                            ChangedEVSEStatus.
                                            Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(v.Key, v.Value.AsHubjectEVSEState()));

                var EVSEStatesRemoveXML = RemovedEVSEStatus.
                                              PushEVSEStatusXML(EVSEStatusDiff.EVSEOperatorId,
                                                                EVSEStatusDiff.EVSEOperatorName[Languages.de],
                                                                ActionType.delete);

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        UserAgent,
                                                        DNSClient))

                {

                    RemovedEVSEIdsSendingTask = 
                        _OICPClient.
                             Query(EVSEStatesRemoveXML,
                                   "eRoamingPushEvseStatus",
                                   QueryTimeout: TimeSpan.FromSeconds(180),

                                   OnSuccess: XMLData => {

                                       var ack = HubjectAcknowledgement.Parse(XMLData.Content);

                                       var OnRemovedEVSEStatusSentLocal = OnRemovedEVSEStatusSent;
                                       if (OnRemovedEVSEStatusSentLocal != null)
                                           OnRemovedEVSEStatusSentLocal(DateTime.Now, TrackingId, ack);

                                       return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                   },


                                   OnSOAPFault: Fault => {

                                       var nack = new HubjectAcknowledgement(false, 0, "SOAPFault", "");

                                       var OnRemovedEVSEStatusSentLocal = OnRemovedEVSEStatusSent;
                                       if (OnRemovedEVSEStatusSentLocal != null)
                                           OnRemovedEVSEStatusSentLocal(DateTime.Now, TrackingId, nack);

                                       return new HTTPResponse<HubjectAcknowledgement>(
                                           Fault.HttpResponse,
                                           nack,
                                           IsFault: true);

                                   },

                                   OnHTTPError: (t, s, e) => {

                                       //var OnHTTPErrorLocal = OnHTTPError;
                                       //if (OnHTTPErrorLocal != null)
                                       //    OnHTTPErrorLocal(t, s, e);

                                   },

                                   OnException: (t, s, e) => {

                                       //var OnExceptionLocal = OnException;
                                       //if (OnExceptionLocal != null)
                                       //    OnExceptionLocal(t, s, e);

                                   }

                                  );

                }

            }

            #endregion


            if (NewEVSEStatusSendingTask != null)
                NewEVSEStatusSendingTask.Wait();

            if (ChangedEVSEStatusSendingTask != null)
                ChangedEVSEStatusSendingTask.Wait();

            if (RemovedEVSEIdsSendingTask != null)
                RemovedEVSEIdsSendingTask.Wait();

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, PartnerProductId = null, HubjectSessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="HubjectSessionId">An optional Hubject session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public AUTHSTARTResult AuthorizeStart(EVSEOperator_Id     OperatorId,
                                              Auth_Token          AuthToken,
                                              EVSE_Id             EVSEId            = null,   // OICP v2.0: Optional
                                              String              PartnerProductId  = null,   // OICP v2.0: Optional [100]
                                              ChargingSession_Id  HubjectSessionId  = null,   // OICP v2.0: Optional
                                              ChargingSession_Id  PartnerSessionId  = null)   // OICP v2.0: Optional [50]

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/eRoamingAuthorization_V2.0", DNSClient: _DNSClient))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.AuthorizeStartXML(OperatorId,
                                                                                      AuthToken,
                                                                                      EVSEId,
                                                                                      PartnerProductId,
                                                                                      HubjectSessionId,
                                                                                      PartnerSessionId),
                                                         "eRoamingAuthorizeStart");

                    Console.WriteLine(HttpResponse.Content.ToUTF8String());

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
                                       ProviderId           = EVSP_Id.Parse(AuthStartResult.ProviderID),
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

                        // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1.2"
                        //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                        //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1.2">
                        //   <soapenv:Body>
                        //     <tns:eRoamingAuthorizationStart>
                        //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                        //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                        //       <tns:StatusCode>
                        //         <cmn:Code>102</cmn:Code>
                        //         <cmn:Description>RFID Authentication failed – invalid UID</cmn:Description>
                        //       </tns:StatusCode>
                        //     </tns:eRoamingAuthorizationStart>
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

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public AUTHSTOPResult AuthorizeStop(EVSEOperator_Id     OperatorId,
                                            EVSE_Id             EVSEId,
                                            ChargingSession_Id  SessionId,
                                            ChargingSession_Id  PartnerSessionId,
                                            Auth_Token          UID)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/eRoamingAuthorization_V2.0", DNSClient: _DNSClient))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.AuthorizeStopXML(OperatorId,
                                                                                     EVSEId,
                                                                                     SessionId,
                                                                                     PartnerSessionId,
                                                                                     UID),
                                                         "eRoamingAuthorizeStop");

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
                                       ProviderId           = EVSP_Id.Parse(AuthStopResult.ProviderID),
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
                    PartnerSessionId     = PartnerSessionId,
                    Description          = "An exception occured: " + e.Message
                };

            }

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerSessionId, PartnerProductId, ChargeStart, ChargeEnd, UID = null, EVCOId = null, SessionStart = null, SessionEnd = null, MeterValueStart = null, MeterValueEnd = null)

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
        public SENDCDRResult SendCDR(EVSE_Id             EVSEId,
                                     ChargingSession_Id  SessionId,
                                     ChargingSession_Id  PartnerSessionId,
                                     String              PartnerProductId,
                                     DateTime            ChargeStart,
                                     DateTime            ChargeEnd,
                                     Auth_Token          UID             = null,
                                     eMA_Id              EVCOId          = null,
                                     DateTime?           SessionStart    = null,
                                     DateTime?           SessionEnd      = null,
                                     Double?             MeterValueStart = null,
                                     Double?             MeterValueEnd   = null)

        {

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname, TCPPort, HTTPVirtualHost, "/ibis/ws/eRoamingAuthorization_V2.0", DNSClient: _DNSClient))
                {

                    var HttpResponse = _OICPClient.Query(CPOMethods.SendChargeDetailRecordXML(EVSEId,
                                                                                              SessionId,
                                                                                              PartnerSessionId,
                                                                                              PartnerProductId,
                                                                                              ChargeStart,
                                                                                              ChargeEnd,
                                                                                              UID,
                                                                                              EVCOId,
                                                                                              SessionStart,
                                                                                              SessionEnd,
                                                                                              MeterValueStart,
                                                                                              MeterValueEnd),
                                                         "eRoamingChargeDetailRecord");

                    //ToDo: In case of errors this will not parse!
                    var ack = HubjectAcknowledgement.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Ok

                    if (ack.Result)
                        return new SENDCDRResult(AuthorizatorId) {
                            State             = SENDCDRState.Forwarded,
                            PartnerSessionId  = PartnerSessionId,
                            Description       = ack.Description
                        };

                    #endregion

                    #region Error

                    else
                        return new SENDCDRResult(AuthorizatorId) {
                            State             = SENDCDRState.False,
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
                        State             = SENDCDRState.False,
                        PartnerSessionId  = PartnerSessionId,
                        Description       = "An exception occured: " + e.Message
                    };

            }

        }

        #endregion

    }

}