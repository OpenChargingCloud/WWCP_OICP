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

using org.GraphDefined.WWCP.LocalService;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 CPOClient.
    /// </summary>
    public class CPOClient : AOICPUpstreamService
    {

        private Authorizator_Id AuthorizatorId = Authorizator_Id.Parse("");


        #region Events

        #region OnNewEVSEStatusSending

        /// <summary>
        /// A delegate called whenever new EVSE status will be send upstream.
        /// </summary>
        public delegate void OnNewEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<EVSEStatusRecord> EVSEStatus, String Hostinfo, String TrackingId);

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnNewEVSEStatusSendingDelegate OnNewEVSEStatusSending;

        #endregion

        #region OnNewEVSEStatusSent

        /// <summary>
        /// A delegate called whenever new EVSE status had been send upstream.
        /// </summary>
        public delegate void OnNewEVSEStatusSentDelegate(DateTime Timestamp, String TrackingId, eRoamingAcknowledgement Result, String Description = null);

        /// <summary>
        /// An event fired whenever new EVSE status had been send upstream.
        /// </summary>
        public event OnNewEVSEStatusSentDelegate OnNewEVSEStatusSent;

        #endregion

        #region OnChangedEVSEStatusSending

        /// <summary>
        /// A delegate called whenever changed EVSE status will be send upstream.
        /// </summary>
        public delegate void OnChangedEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<EVSEStatusRecord> EVSEStatus, String Hostinfo, String TrackingId);

        /// <summary>
        /// An event fired whenever changed EVSE status will be send upstream.
        /// </summary>
        public event OnChangedEVSEStatusSendingDelegate OnChangedEVSEStatusSending;

        #endregion

        #region OnChangedEVSEStatusSent

        /// <summary>
        /// A delegate called whenever changed EVSE status had been send upstream.
        /// </summary>
        public delegate void OnChangedEVSEStatusSentDelegate(DateTime Timestamp, String TrackingId, eRoamingAcknowledgement Result, String Description = null);

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
        public delegate void OnRemovedEVSEStatusSentDelegate(DateTime Timestamp, String TrackingId, eRoamingAcknowledgement Result, String Description = null);

        /// <summary>
        /// An event fired whenever removed EVSE status had been send upstream.
        /// </summary>
        public event OnRemovedEVSEStatusSentDelegate OnRemovedEVSEStatusSent;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 CPOClient.
        /// </summary>
        /// <param name="Hostname">The hostname of the OICP service.</param>
        /// <param name="TCPPort">An optional TCP port of the OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String    Hostname,
                         IPPort    TCPPort          = null,
                         String    HTTPVirtualHost  = null,
                         String    HTTPUserAgent    = "GraphDefined OICP v2.0 CPOClient",
                         TimeSpan? QueryTimeout     = null,
                         DNSClient DNSClient        = null)

            : base(Hostname,
                   TCPPort,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        { }

        #endregion


        #region PushEVSEData(EVSEDataRecord, OICPAction = insert, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data record.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
                         ActionType                     OICPAction    = ActionType.insert,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            return await PushEVSEData(new EVSEDataRecord[1] { EVSEDataRecord },
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(OICPAction, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(ActionType               OICPAction,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await PushEVSEData(EVSEDataRecords, OICPAction);

        }

        #endregion

        #region PushEVSEData(OICPAction, OperatorId, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="OperatorId">The EVSE operator Id to use.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(ActionType               OICPAction,
                         EVSEOperator_Id          OperatorId,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await PushEVSEData(EVSEDataRecords, OICPAction, OperatorId);

        }

        #endregion

        #region PushEVSEData(OICPAction, OperatorId, OperatorName, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="OperatorId">The EVSE operator Id to use.</param>
        /// <param name="OperatorName">The EVSE operator name.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(ActionType               OICPAction,
                         EVSEOperator_Id          OperatorId,
                         String                   OperatorName,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await PushEVSEData(EVSEDataRecords, OICPAction, OperatorId, OperatorName);

        }

        #endregion

        #region PushEVSEData(EVSEDataRecords, OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException("EVSEDataRecords", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            var _EVSEDataRecords = EVSEDataRecords.
                                       Where(evse => IncludeEVSEs(evse)).
                                       ToArray();

            #endregion

            try
            {

                if (_EVSEDataRecords.Any())
                {

                    DebugX.Log(OICPAction + " of " + _EVSEDataRecords.Count() + " EVSE static data records at " + _HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseData_V2.0",
                                                            _UserAgent,
                                                            false,
                                                            _DNSClient))
                    {

                        return await _OICPClient.Query(CPOClient_XMLMethods.PushEVSEDataXML(_EVSEDataRecords,
                                                                                            OICPAction,
                                                                                            OperatorId,
                                                                                            OperatorName),

                                                       "eRoamingPushEvseData",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                       OnSuccess: XMLData => {

                                                           return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse,
                                                                                                           eRoamingAcknowledgement.Parse(XMLData.Content),
                                                                                                           false);

                                                       },


                                                       OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                           DebugX.Log(OICPAction + " of EVSE data led to a fault!" + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(
                                                               soapfault.HttpResponse,
                                                               new eRoamingAcknowledgement(false, 0, "", ""),
                                                               IsFault: true);

                                                       },

                                                       OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                           SendHTTPError(timestamp, soapclient, httpresponse);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                            new eRoamingAcknowledgement(false,
                                                                                                                                        -1,
                                                                                                                                        httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                        httpresponse.Content.ToUTF8String()),
                                                                                                            IsFault: true);

                                                       },

                                                       OnException: (timestamp, sender, exception) => {

                                                           SendException(timestamp, sender, exception);

                                                           return null;

                                                       }

                                                   );

                    }

                }
                else
                {

                    DebugX.Log(OICPAction + " of EVSE static data records at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<eRoamingAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                if (e.InnerException != null)
                    e = e.InnerException;

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(), new eRoamingAcknowledgement(false, 0, e.Message), e);

            }

        }

        #endregion


        #region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE status records onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE Id and status records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatusType>>  EVSEStatus,
                           ActionType                                              OICPAction    = ActionType.update,
                           EVSEOperator_Id                                         OperatorId    = null,
                           String                                                  OperatorName  = null,
                           TimeSpan?                                               QueryTimeout  = null)

        {

            return await PushEVSEStatus(EVSEStatus.Select(kvp => new EVSEStatusRecord(kvp.Key, kvp.Value)),
                                        OICPAction,
                                        OperatorId,
                                        OperatorName,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE status records onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE Id and status records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatus,
                           ActionType                     OICPAction    = ActionType.update,
                           EVSEOperator_Id                OperatorId    = null,
                           String                         OperatorName  = null,
                           TimeSpan?                      QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException("EVSEStatus", "The given parameter must not be null!");

            if (OperatorId == null)
                OperatorId = EVSEStatus.First().Id.OperatorId;

            #endregion

            try
            {

                if (EVSEStatus.Any())
                {

                    DebugX.Log(OICPAction + " of " + EVSEStatus.Count() + " EVSE states at " + _HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                            _UserAgent,
                                                            false,
                                                            _DNSClient))
                    {

                        return await _OICPClient.Query(EVSEStatus.
                                                           PushEVSEStatusXML(OperatorId,
                                                                             OperatorName,
                                                                             OICPAction),
                                                       "eRoamingPushEvseStatus",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                       OnSuccess: XMLData =>
                                                       {

                                                           #region Documentation

                                                           // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v2.0">
                                                           //   <cmn:Result>true</cmn:Result>
                                                           //   <cmn:StatusCode>
                                                           //     <cmn:Code>000</cmn:Code>
                                                           //     <cmn:Description>Success</cmn:Description>
                                                           //     <cmn:AdditionalInfo />
                                                           //   </cmn:StatusCode>
                                                           // </cmn:eRoamingAcknowledgement>

                                                           // <cmn:eRoamingAcknowledgement xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v2.0">
                                                           //   <cmn:Result>false</cmn:Result>
                                                           //   <cmn:StatusCode>
                                                           //     <cmn:Code>009</cmn:Code>
                                                           //     <cmn:Description>Data transaction error</cmn:Description>
                                                           //     <cmn:AdditionalInfo>The Push of data is already in progress.</cmn:AdditionalInfo>
                                                           //   </cmn:StatusCode>
                                                           // </cmn:eRoamingAcknowledgement>

                                                           #endregion

                                                           var ack = eRoamingAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE states: " + ack.Result + " / " + ack.StatusCode.Description + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                           DebugX.Log(OICPAction + " of EVSE states led to a fault!" + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(
                                                               soapfault.HttpResponse,
                                                               new eRoamingAcknowledgement(false, 0, "", ""),
                                                               IsFault: true);

                                                       },

                                                       OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                           SendHTTPError(timestamp, soapclient, httpresponse);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                            new eRoamingAcknowledgement(false,
                                                                                                                                        -1,
                                                                                                                                        httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                        httpresponse.Content.ToUTF8String()),
                                                                                                            IsFault: true);

                                                       },

                                                       OnException: (timestamp, sender, exception) => {

                                                           SendException(timestamp, sender, exception);

                                                           return null;

                                                       }

                                                      );

                    }

                }
                else
                {

                    DebugX.Log(OICPAction + " of EVSE states at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<eRoamingAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                if (e.InnerException != null)
                    e = e.InnerException;

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(), new eRoamingAcknowledgement(false, 0, e.Message), e);

            }

        }

        #endregion

        #region PushEVSEStatusUpdates(EVSEStatusDiff)

        /// <summary>
        /// Send EVSE status updates upstream.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        public async Task PushEVSEStatusUpdates(EVSEStatusDiff  EVSEStatusDiff)

        {

            if (EVSEStatusDiff == null)
                return;

            var TrackingId = Guid.NewGuid().ToString();

            #region Insert new EVSEs...

            if (EVSEStatusDiff.NewStatus.Count() > 0)
            {

                var NewEVSEStatus  = EVSEStatusDiff.
                                         NewStatus.
                                         Select(v => new EVSEStatusRecord(v.Key, v.Value.AsOICPEVSEStatus())).
                                         ToArray();

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
                                                        _UserAgent,
                                                        false,
                                                        _DNSClient))

                {

                    var NewEVSEStatusSendingTask = await

                        _OICPClient.
                             Query(EVSEStatesInsertXML,
                                   "eRoamingPushEvseStatus",
                                   QueryTimeout: TimeSpan.FromSeconds(180),

                                   OnSuccess: XMLData => {

                                       var ack = eRoamingAcknowledgement.Parse(XMLData.Content);

                                       var OnNewEVSEStatusSentLocal = OnNewEVSEStatusSent;
                                       if (OnNewEVSEStatusSentLocal != null)
                                           OnNewEVSEStatusSentLocal(DateTime.Now, TrackingId, ack);

                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse, ack, false);

                                   },


                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                       var nack = new eRoamingAcknowledgement(false, 0, "SOAPFault", "");

                                       var OnNewEVSEStatusSentLocal = OnNewEVSEStatusSent;
                                       if (OnNewEVSEStatusSentLocal != null)
                                           OnNewEVSEStatusSentLocal(DateTime.Now, TrackingId, nack);

                                       return new HTTPResponse<eRoamingAcknowledgement>(
                                           soapfault.HttpResponse,
                                           nack,
                                           IsFault: true);

                                   },

                                   OnHTTPError: (t, s, e) => {

                                       //var OnHTTPErrorLocal = OnHTTPError;
                                       //if (OnHTTPErrorLocal != null)
                                       //    OnHTTPErrorLocal(t, s, e);

                                       return null;

                                   },

                                   OnException: (t, s, e) => {

                                       //var OnExceptionLocal = OnException;
                                       //if (OnExceptionLocal != null)
                                       //    OnExceptionLocal(t, s, e);

                                       return null;

                                   }

                                  );

                }

            }

            #endregion

            #region Upload EVSE changes...

            if (EVSEStatusDiff.ChangedStatus.Count() > 0)
            {

                var ChangedEVSEStatus = EVSEStatusDiff.
                                            ChangedStatus.
                                            Select(v => new EVSEStatusRecord(v.Key, v.Value.AsOICPEVSEStatus())).
                                            ToArray();

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
                                                        _UserAgent,
                                                        false,
                                                        _DNSClient))

                {

                    var ChangedEVSEStatusSendingTask = await

                        _OICPClient.
                             Query(EVSEStatesUpdateXML,
                                   "eRoamingPushEvseStatus",
                                   QueryTimeout: TimeSpan.FromSeconds(180),

                                   OnSuccess: XMLData => {

                                       var ack = eRoamingAcknowledgement.Parse(XMLData.Content);

                                       DebugX.Log("eRoamingPushEvseStatus => " + ack.Result);

                                       var OnChangedEVSEStatusSentLocal = OnChangedEVSEStatusSent;
                                       if (OnChangedEVSEStatusSentLocal != null)
                                           OnChangedEVSEStatusSentLocal(DateTime.Now, TrackingId, ack);

                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse, ack, false);

                                   },


                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                       var nack = new eRoamingAcknowledgement(false, 0, "SOAPFault", "");

                                       var OnChangedEVSEStatusSentLocal = OnChangedEVSEStatusSent;
                                       if (OnChangedEVSEStatusSentLocal != null)
                                           OnChangedEVSEStatusSentLocal(DateTime.Now, TrackingId, nack);

                                       return new HTTPResponse<eRoamingAcknowledgement>(
                                           soapfault.HttpResponse,
                                           nack,
                                           IsFault: true);

                                   },

                                   OnHTTPError: (t, s, e) => {

                                       //var OnHTTPErrorLocal = OnHTTPError;
                                       //if (OnHTTPErrorLocal != null)
                                       //    OnHTTPErrorLocal(t, s, e);

                                       return null;

                                   },

                                   OnException: (t, s, e) => {

                                       //var OnExceptionLocal = OnException;
                                       //if (OnExceptionLocal != null)
                                       //    OnExceptionLocal(t, s, e);

                                       return null;

                                   }

                                  );

                }

            }

            #endregion

            #region Remove outdated EVSEs...

            if (EVSEStatusDiff.RemovedIds.Count() > 0)
            {

                var RemovedEVSEStatus = EVSEStatusDiff.
                                            RemovedIds.
                                            ToArray();

                var OnRemovedEVSEStatusSendingLocal = OnRemovedEVSEStatusSending;
                if (OnRemovedEVSEStatusSendingLocal != null)
                    OnRemovedEVSEStatusSendingLocal(DateTime.Now, RemovedEVSEStatus, _HTTPVirtualHost, TrackingId);


                var EVSEStatesRemoveXML = RemovedEVSEStatus.
                                              PushEVSEStatusXML(EVSEStatusType.Unavailable,
                                                                EVSEStatusDiff.EVSEOperatorId,
                                                                EVSEStatusDiff.EVSEOperatorName[Languages.de],
                                                                ActionType.delete);

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        _UserAgent,
                                                        false,
                                                        _DNSClient))

                {

                    var RemovedEVSEIdsSendingTask = await

                        _OICPClient.
                             Query(EVSEStatesRemoveXML,
                                   "eRoamingPushEvseStatus",
                                   QueryTimeout: TimeSpan.FromSeconds(180),

                                   OnSuccess: XMLData => {

                                       var ack = eRoamingAcknowledgement.Parse(XMLData.Content);

                                       var OnRemovedEVSEStatusSentLocal = OnRemovedEVSEStatusSent;
                                       if (OnRemovedEVSEStatusSentLocal != null)
                                           OnRemovedEVSEStatusSentLocal(DateTime.Now, TrackingId, ack);

                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse, ack, false);

                                   },


                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                       var nack = new eRoamingAcknowledgement(false, 0, "SOAPFault", "");

                                       var OnRemovedEVSEStatusSentLocal = OnRemovedEVSEStatusSent;
                                       if (OnRemovedEVSEStatusSentLocal != null)
                                           OnRemovedEVSEStatusSentLocal(DateTime.Now, TrackingId, nack);

                                       return new HTTPResponse<eRoamingAcknowledgement>(
                                           soapfault.HttpResponse,
                                           nack,
                                           IsFault: true);

                                   },

                                   OnHTTPError: (t, s, e) => {

                                       //var OnHTTPErrorLocal = OnHTTPError;
                                       //if (OnHTTPErrorLocal != null)
                                       //    OnHTTPErrorLocal(t, s, e);

                                       return null;

                                   },

                                   OnException: (t, s, e) => {

                                       //var OnExceptionLocal = OnException;
                                       //if (OnExceptionLocal != null)
                                       //    OnExceptionLocal(t, s, e);

                                       return null;

                                   }

                                  );

                }

            }

            #endregion

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

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId            = null,
                           ChargingSession_Id  SessionId         = null,
                           ChargingProduct_Id  PartnerProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  PartnerSessionId  = null,   // [maxlength: 50]
                           TimeSpan?           QueryTimeout      = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        DNSClient: _DNSClient))
                {

                    return await _OICPClient.Query(CPOClient_XMLMethods.AuthorizeStartXML(OperatorId,
                                                                                          AuthToken,
                                                                                          EVSEId,
                                                                                          PartnerProductId,
                                                                                          SessionId,
                                                                                          PartnerSessionId),
                                                   "eRoamingAuthorizeStart",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData =>
                                                   {

                                                       return new HTTPResponse<eRoamingAuthorizationStart>(XMLData.HttpResponse,
                                                                                                           eRoamingAuthorizationStart.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAuthorizationStart>(soapfault.HttpResponse,
                                                                                                           new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                                                          StatusCode: new StatusCode(-1,
                                                                                                                                                                     Description: soapfault.Content.ToString())),
                                                                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                           new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                                                          StatusCode: new StatusCode(-1,
                                                                                                                                                                     Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                                     AdditionalInfo: httpresponse.Content.ToUTF8String())),
                                                                                                           IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAuthorizationStart>(new HTTPResponse(),
                                                                    new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                   StatusCode: new StatusCode(-1,
                                                                                                                              Description:  "An exception occured: " + e.Message)
                                                                    ));

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, QueryTimeout = null)

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
        public async Task<HTTPResponse<eRoamingAuthorizationStop>> AuthorizeStop(EVSEOperator_Id      OperatorId,
                                                                                 ChargingSession_Id   SessionId,
                                                                                 Auth_Token           AuthToken,
                                                                                 EVSE_Id              EVSEId            = null,
                                                                                 ChargingSession_Id   PartnerSessionId  = null,   // [maxlength: 50]
                                                                                 TimeSpan?            QueryTimeout      = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        DNSClient: _DNSClient))
                {

                    return await _OICPClient.Query(CPOClient_XMLMethods.AuthorizeStopXML(OperatorId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         EVSEId,
                                                                                         PartnerSessionId),
                                                   "eRoamingAuthorizeStop",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData =>
                                                   {

                                                       return new HTTPResponse<eRoamingAuthorizationStop>(XMLData.HttpResponse,
                                                                                                          eRoamingAuthorizationStop.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAuthorizationStop>(soapfault.HttpResponse,
                                                                                                          new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                                                        StatusCode: new StatusCode(-1,
                                                                                                                                                                   Description: soapfault.Content.ToString())),
                                                                                                          IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                          new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                                                        StatusCode: new StatusCode(-1,
                                                                                                                                                                   Description: httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                                   AdditionalInfo: httpresponse.Content.ToUTF8String())),
                                                                                                          IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAuthorizationStop>(new HTTPResponse(),
                                                                    new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                  StatusCode: new StatusCode(-1,
                                                                                                                             Description:  "An exception occured: " + e.Message)
                                                                    ));

            }

        }

        #endregion


        #region PullAuthenticationData(OperatorId, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 PullAuthenticationData request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAuthenticationData>> PullAuthenticationData(EVSEOperator_Id  OperatorId,
                                                                                           TimeSpan?        QueryTimeout = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                        DNSClient: _DNSClient))
                {

                    return await _OICPClient.Query(CPOClient_XMLMethods.PullAuthenticationDataXML(OperatorId),
                                                   "eRoamingPullAuthenticationData",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData =>
                                                   {

                                                       #region Documentation

                                                       // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
                                                       //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
                                                       //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                                                       //
                                                       //    <soapenv:Header/>
                                                       //
                                                       //    <soapenv:Body>
                                                       //       <AuthenticationData:eRoamingAuthenticationData>
                                                       // 
                                                       //          <AuthenticationData:AuthenticationData>
                                                       // 
                                                       //             <!--Zero or more repetitions:-->
                                                       //             <AuthenticationData:ProviderAuthenticationData>
                                                       // 
                                                       //                <AuthenticationData:ProviderID>?</AuthenticationData:ProviderID>
                                                       // 
                                                       //                <!--Zero or more repetitions:-->
                                                       //                <AuthenticationData:AuthenticationDataRecord>
                                                       //                   <AuthenticationData:Identification>
                                                       // 
                                                       //                      <!--You have a CHOICE of the next 4 items at this level-->
                                                       //                      <CommonTypes:RFIDmifarefamilyIdentification>
                                                       //                         <CommonTypes:UID>?</CommonTypes:UID>
                                                       //                      </CommonTypes:RFIDmifarefamilyIdentification>
                                                       // 
                                                       //                      <CommonTypes:QRCodeIdentification>
                                                       // 
                                                       //                         <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
                                                       // 
                                                       //                         <!--You have a CHOICE of the next 2 items at this level-->
                                                       //                         <CommonTypes:PIN>?</CommonTypes:PIN>
                                                       // 
                                                       //                         <CommonTypes:HashedPIN>
                                                       //                            <CommonTypes:Value>?</CommonTypes:Value>
                                                       //                            <CommonTypes:Function>?</CommonTypes:Function>
                                                       //                            <CommonTypes:Salt>?</CommonTypes:Salt>
                                                       //                         </CommonTypes:HashedPIN>
                                                       // 
                                                       //                      </CommonTypes:QRCodeIdentification>
                                                       // 
                                                       //                      <CommonTypes:PlugAndChargeIdentification>
                                                       //                         <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
                                                       //                      </CommonTypes:PlugAndChargeIdentification>
                                                       // 
                                                       //                      <CommonTypes:RemoteIdentification>
                                                       //                         <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
                                                       //                      </CommonTypes:RemoteIdentification>
                                                       // 
                                                       //                   </AuthenticationData:Identification>
                                                       //                </AuthenticationData:AuthenticationDataRecord>
                                                       // 
                                                       //             </AuthenticationData:ProviderAuthenticationData>
                                                       //          </AuthenticationData:AuthenticationData>
                                                       // 
                                                       //          <!--Optional:-->
                                                       //          <AuthenticationData:StatusCode>
                                                       // 
                                                       //             <CommonTypes:Code>?</CommonTypes:Code>
                                                       // 
                                                       //             <!--Optional:-->
                                                       //             <CommonTypes:Description>?</CommonTypes:Description>
                                                       // 
                                                       //             <!--Optional:-->
                                                       //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
                                                       // 
                                                       //          </AuthenticationData:StatusCode>
                                                       // 
                                                       //       </AuthenticationData:eRoamingAuthenticationData>
                                                       //    </soapenv:Body>
                                                       //
                                                       // </soapenv:Envelope>

                                                       // <tns:eRoamingAuthenticationData xmlns:tns="http://www.hubject.com/b2b/services/authenticationdata/v2.0">
                                                       //  <tns:AuthenticationData>
                                                       //    <tns:ProviderAuthenticationData>
                                                       //
                                                       //      <tns:ProviderID>DE*ICE</tns:ProviderID>
                                                       //
                                                       //      <tns:AuthenticationDataRecord>
                                                       //        <tns:Identification>
                                                       //          <cmn:QRCodeIdentification xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v2.0">
                                                       //            <cmn:EVCOID>DE*ICE*I00800*4</cmn:EVCOID>
                                                       //            <cmn:HashedPIN>
                                                       //              <cmn:Value>e43357c592ff9525b0670c697b733534596fed57</cmn:Value>
                                                       //              <cmn:Function>SHA-1</cmn:Function>
                                                       //              <cmn:Salt>735400AC87A31</cmn:Salt>
                                                       //            </cmn:HashedPIN>
                                                       //          </cmn:QRCodeIdentification>
                                                       //        </tns:Identification>
                                                       //      </tns:AuthenticationDataRecord>
                                                       //
                                                       //      <tns:AuthenticationDataRecord>
                                                       //        <tns:Identification>
                                                       //          <cmn:RFIDmifarefamilyIdentification xmlns:cmn="http://www.hubject.com/b2b/services/commontypes/v2.0">
                                                       //            <cmn:UID>049314143A2AC0</cmn:UID>
                                                       //          </cmn:RFIDmifarefamilyIdentification>
                                                       //        </tns:Identification>
                                                       //      </tns:AuthenticationDataRecord>
                                                       //
                                                       //    </tns:ProviderAuthenticationData>
                                                       //  </tns:AuthenticationData>
                                                       //</tns:eRoamingAuthenticationData>

                                                       #endregion

                                                       return new HTTPResponse<eRoamingAuthenticationData>(XMLData.HttpResponse,
                                                                                                           eRoamingAuthenticationData.Parse(XMLData.Content));

                                                   },

                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       DebugX.Log("PullAuthenticationData led to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<eRoamingAuthenticationData>(soapfault.HttpResponse,
                                                                                                           null,
                                                                                                           IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAuthenticationData>(httpresponse,
                                                                                                           null,
                                                                                                           IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAuthenticationData>(new HTTPResponse(),
                                                                    null,
                                                                    e);

            }

        }

        #endregion


        #region SendChargeDetailRecord(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            SendChargeDetailRecord(eRoamingChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?                   QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        DNSClient: _DNSClient))
                {

                    return await _OICPClient.Query(CPOClient_XMLMethods.SendChargeDetailRecordXML(ChargeDetailRecord),
                                                   "eRoamingChargeDetailRecord",
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
                                                                                                                                    soapfault.Content.ToString()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                    httpresponse.Content.ToUTF8String()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(),
                                                                 new eRoamingAcknowledgement(false,
                                                                                             -1,
                                                                                             "An exception occured: " + e.Message));

            }

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, Identification, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="Identification">An ev customer identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            SendChargeDetailRecord(EVSE_Id                      EVSEId,
                                   ChargingSession_Id           SessionId,
                                   ChargingProduct_Id           PartnerProductId,
                                   DateTime                     SessionStart,
                                   DateTime                     SessionEnd,
                                   AuthorizationIdentification  Identification,
                                   ChargingSession_Id           PartnerSessionId      = null,
                                   DateTime?                    ChargingStart         = null,
                                   DateTime?                    ChargingEnd           = null,
                                   Double?                      MeterValueStart       = null,
                                   Double?                      MeterValueEnd         = null,
                                   IEnumerable<Double>          MeterValuesInBetween  = null,
                                   Double?                      ConsumedEnergy        = null,
                                   String                       MeteringSignature     = null,
                                   HubOperator_Id               HubOperatorId         = null,
                                   EVSP_Id                      HubProviderId         = null,
                                   TimeSpan?                    QueryTimeout          = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException("EVSEId",            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            if (Identification   == null)
                throw new ArgumentNullException("Identification",    "The given parameter must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        DNSClient: _DNSClient))
                {

                    return await _OICPClient.Query(CPOClient_XMLMethods.SendChargeDetailRecordXML(EVSEId,
                                                                                                  SessionId,
                                                                                                  PartnerProductId,
                                                                                                  SessionStart,
                                                                                                  SessionEnd,
                                                                                                  Identification,
                                                                                                  PartnerSessionId,
                                                                                                  ChargingStart,
                                                                                                  ChargingEnd,
                                                                                                  MeterValueStart,
                                                                                                  MeterValueEnd,
                                                                                                  MeterValuesInBetween,
                                                                                                  ConsumedEnergy,
                                                                                                  MeteringSignature,
                                                                                                  HubOperatorId,
                                                                                                  HubProviderId),
                                                   "eRoamingChargeDetailRecord",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData =>
                                                   {

                                                       return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse,
                                                                                                        eRoamingAcknowledgement.Parse(XMLData.Content));

                                                   },


                                                   OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                       SendSOAPError(timestamp, soapclient, soapfault.Content);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(soapfault.HttpResponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    soapfault.Content.ToString()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                       SendHTTPError(timestamp, soapclient, httpresponse);

                                                       return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                        new eRoamingAcknowledgement(false,
                                                                                                                                    -1,
                                                                                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                    httpresponse.Content.ToUTF8String()),
                                                                                                        IsFault: true);

                                                   },

                                                   OnException: (timestamp, sender, exception) => {

                                                       SendException(timestamp, sender, exception);

                                                       return null;

                                                   }

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(),
                                                                 new eRoamingAcknowledgement(false,
                                                                                             -1,
                                                                                             "An exception occured: " + e.Message));

            }

        }

        #endregion


    }

}
