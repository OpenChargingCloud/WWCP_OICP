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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A simple OICP v2.0 CPO Client.
    /// </summary>
    public class CPOClient
    {

        #region Data

        private readonly CPOUpstreamService _CPOUpstreamService;

        #endregion

        #region Events

        #region OnNewEVSEStatusSending

        /// <summary>
        /// A delegate called whenever new EVSE status will be send upstream.
        /// </summary>
        public delegate void OnNewEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> EVSEStatus, String Hostinfo, String TrackingId);

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
        public delegate void OnChangedEVSEStatusSendingDelegate(DateTime Timestamp, IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> EVSEStatus, String Hostinfo, String TrackingId);

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
        /// Create a new OICP v2.0 CPO Client.
        /// </summary>
        /// <param name="OICPHost">The hostname of the OICP service.</param>
        /// <param name="TCPPort">An optional TCP port of the OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String    OICPHost,
                         IPPort    TCPPort          = null,
                         String    HTTPVirtualHost  = null,
                         String    HTTPUserAgent    = "GraphDefined OICP v2.0 CPO Client",
                         TimeSpan? QueryTimeout     = null,
                         DNSClient DNSClient        = null)

        {

            this._CPOUpstreamService = new CPOUpstreamService(OICPHost,
                                                              TCPPort != null ? TCPPort : IPPort.Parse(443),
                                                              HTTPVirtualHost,
                                                              Authorizator_Id.Parse("GraphDefined OICP v2.0 CPO Client"),
                                                              HTTPUserAgent,
                                                              QueryTimeout,
                                                              DNSClient);

        }

        #endregion


        #region PushEVSEData(EVSEDataRecord,  OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data record.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
                         ActionType                     OICPAction    = ActionType.insert,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            return await _CPOUpstreamService.PushEVSEData(EVSEDataRecord,
                                                          OICPAction,
                                                          OperatorId,
                                                          OperatorName,
                                                          IncludeEVSEs,
                                                          QueryTimeout);

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
        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            return await _CPOUpstreamService.PushEVSEData(EVSEDataRecords,
                                                          OICPAction,
                                                          OperatorId,
                                                          OperatorName,
                                                          IncludeEVSEs,
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
        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatus>>  EVSEStatus,
                           ActionType                                          OICPAction    = ActionType.update,
                           EVSEOperator_Id                                     OperatorId    = null,
                           String                                              OperatorName  = null,
                           TimeSpan?                                           QueryTimeout  = null)

        {

            return await _CPOUpstreamService.PushEVSEStatus(EVSEStatus,
                                                            OICPAction,
                                                            OperatorId,
                                                            OperatorName,
                                                            QueryTimeout);

        }

        #endregion

        #region PushEVSEStatusUpdates(EVSEStatusDiff)

        /// <summary>
        /// Send EVSE status updates upstream.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        public async Task PushEVSEStatusUpdates(EVSEStatusDiff  EVSEStatusDiff)

        {

            await _CPOUpstreamService.PushEVSEStatusUpdates(EVSEStatusDiff);

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
        public async Task<HTTPResponse<AUTHSTARTResult>>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId            = null,
                           ChargingSession_Id  SessionId         = null,
                           ChargingProduct_Id  PartnerProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  PartnerSessionId  = null,   // [maxlength: 50]
                           TimeSpan?           QueryTimeout      = null)

        {

            return await _CPOUpstreamService.AuthorizeStart(OperatorId,
                                                            AuthToken,
                                                            EVSEId,
                                                            SessionId,
                                                            PartnerProductId,
                                                            PartnerSessionId,
                                                            QueryTimeout);

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
        public async Task<HTTPResponse<AUTHSTOPResult>> AuthorizeStop(EVSEOperator_Id      OperatorId,
                                                                      ChargingSession_Id   SessionId,
                                                                      Auth_Token           AuthToken,
                                                                      EVSE_Id              EVSEId            = null,
                                                                      ChargingSession_Id   PartnerSessionId  = null,   // [maxlength: 50]
                                                                      TimeSpan?            QueryTimeout      = null)
        {

            return await _CPOUpstreamService.AuthorizeStop(OperatorId,
                                                           SessionId,
                                                           AuthToken,
                                                           EVSEId,
                                                           PartnerSessionId,
                                                           QueryTimeout);

        }

        #endregion


        #region PullAuthenticationData

        /// <summary>
        /// Create an OICP v2.0 PullAuthenticationData request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<AuthenticationData>> PullAuthenticationData(EVSEOperator_Id  OperatorId,
                                                                                   TimeSpan?        QueryTimeout = null)
        {

            return await _CPOUpstreamService.PullAuthenticationData(OperatorId,
                                                                    QueryTimeout);

        }

        #endregion


        #region SendChargeDetailRecord(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendChargeDetailRecord(OICPChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?               QueryTimeout  = null)

        {

            return await _CPOUpstreamService.SendCDR(ChargeDetailRecord,
                                                     QueryTimeout);

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">Optional timestamp of the charging start.</param>
        /// <param name="ChargingEnd">Optional timestamp of the charging stop.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendChargeDetailRecord(EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   PartnerProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   Auth_Token           AuthToken             = null,
                                   eMA_Id               eMAId                 = null,
                                   ChargingSession_Id   PartnerSessionId      = null,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   EVSEOperator_Id      HubOperatorId         = null,
                                   EVSP_Id              HubProviderId         = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            return await _CPOUpstreamService.SendChargeDetailRecord(EVSEId,
                                                                    SessionId,
                                                                    PartnerProductId,
                                                                    SessionStart,
                                                                    SessionEnd,
                                                                    AuthToken,
                                                                    eMAId,
                                                                    PartnerSessionId,
                                                                    ChargingStart,
                                                                    ChargingEnd,
                                                                    MeterValueStart,
                                                                    MeterValueEnd,
                                                                    MeterValuesInBetween,
                                                                    ConsumedEnergy,
                                                                    MeteringSignature,
                                                                    HubOperatorId,
                                                                    HubProviderId);

        }

        #endregion


    }

}
