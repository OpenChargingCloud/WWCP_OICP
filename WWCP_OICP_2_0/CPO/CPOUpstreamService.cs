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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 CPO Upstream Service(s).
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
        /// Create a new OICP v2.0 CPO Upstream Service.
        /// </summary>
        /// <param name="OICPHost">The hostname of the OICP service.</param>
        /// <param name="TCPPort">The TCP port of the OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the OICP service.</param>
        /// <param name="AuthorizatorId">An optional unique authorizator identification.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOUpstreamService(String           OICPHost,
                                  IPPort           TCPPort,
                                  String           HTTPVirtualHost  = null,
                                  Authorizator_Id  AuthorizatorId   = null,
                                  String           HTTPUserAgent    = "GraphDefined OICP v2.0 Gateway CPO Upstream Services",
                                  TimeSpan?        QueryTimeout     = null,
                                  DNSClient        DNSClient        = null)

            : base(OICPHost,
                   TCPPort,
                   HTTPVirtualHost,
                   AuthorizatorId,
                   HTTPUserAgent,
                   QueryTimeout,
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


        #region PushEVSEData(GroupedData,      OICPAction = fullLoad, OperatorId = null, OperatorName = null, QueryTimeout = null)

        /// <summary>
        /// Push EVSE data onto the remote OICP server.
        /// </summary>
        /// <param name="GroupedData">A list of EVSEs grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The optional OICP data management action on the remote server.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(ILookup<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                         ActionType                                OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                           OperatorId    = null,
                         String                                    OperatorName  = null,
                         TimeSpan?                                 QueryTimeout  = null)

        {

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            try
            {

                var NumberOfEVSEs = GroupedData.Select(v => v.Select(w => w.Count())).SelectMany(x => x).Sum();

                if (NumberOfEVSEs > 0)
                {

                    DebugX.Log(OICPAction + " of " + NumberOfEVSEs + " EVSE static data sets at " + _HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseData_V2.0",
                                                            _UserAgent,
                                                            _DNSClient))
                    {

                        return await _OICPClient.Query(CPO_XMLMethods.PushEVSEDataXML(GroupedData,
                                                                                      OICPAction,
                                                                                      OperatorId,
                                                                                      OperatorName),

                                                       "eRoamingPushEvseData",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                       OnSuccess: XMLData =>
                                                       {

                                                           #region Documentation

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

                                                           #endregion

                                                           var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE data: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                           return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: Fault =>
                                                       {

                                                           DebugX.Log(OICPAction + " of EVSE data led to a fault!" + Environment.NewLine);

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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE static data sets at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<HubjectAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<HubjectAcknowledgement>(new HTTPResponse(), e);

            }

        }

        #endregion

        #region PushEVSEData(EVSEOperator,     OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(EVSEOperator         EVSEOperator,
                         ActionType           OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            #endregion

            return await PushEVSEData(new EVSEOperator[] { EVSEOperator },
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEOperators,    OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(IEnumerable<EVSEOperator>  EVSEOperators,
                         ActionType                 OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException("EVSEOperators", "The given parameter must not be null!");

            var _EVSEOperators = EVSEOperators.ToArray();

            if (_EVSEOperators.Length == 0)
                throw new ArgumentNullException("EVSEOperators", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return await PushEVSEData(_EVSEOperators.ToLookup(evseoperator => evseoperator,
                                                              evseoperator => evseoperator.SelectMany(pool    => pool.ChargingStations).
                                                                                           SelectMany(station => station.EVSEs).
                                                                                           Where     (evse    => IncludeEVSEs(evse))),
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingPool,     OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(ChargingPool         ChargingPool,
                         ActionType           OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given parameter must not be null!");

            #endregion

            return await PushEVSEData(new ChargingPool[] { ChargingPool },
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingPools,    OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(IEnumerable<ChargingPool>  ChargingPools,
                         ActionType                 OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException("ChargingPools", "The given parameter must not be null!");

            var _ChargingPools = ChargingPools.ToArray();

            if (_ChargingPools.Length == 0)
                throw new ArgumentNullException("ChargingPools", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return await PushEVSEData(_ChargingPools.ToLookup(pool => pool.EVSEOperator,
                                                              pool => pool.SelectMany(station => station.EVSEs).
                                                                           Where     (evse    => IncludeEVSEs(evse))),
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingStation,  OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(ChargingStation      ChargingStation,
                         ActionType           OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given parameter must not be null!");

            #endregion

            return await PushEVSEData(new ChargingStation[] { ChargingStation },
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingStations, OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(IEnumerable<ChargingStation>  ChargingStations,
                         ActionType                    OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id               OperatorId    = null,
                         String                        OperatorName  = null,
                         Func<EVSE, Boolean>           IncludeEVSEs  = null,
                         TimeSpan?                     QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException("ChargingStations", "The given parameter must not be null!");

            var _ChargingStations = ChargingStations.ToArray();

            if (_ChargingStations.Length == 0)
                throw new ArgumentNullException("ChargingStations", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return await PushEVSEData(_ChargingStations.ToLookup(station => station.ChargingPool.EVSEOperator,
                                                                 station => station.Where(evse => IncludeEVSEs(evse))),
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSE,             OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(EVSE                 EVSE,
                         ActionType           OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given parameter must not be null!");

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEs,            OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,
                         ActionType           OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            var _EVSEs = EVSEs.
                             Where(evse => IncludeEVSEs(evse)).
                             ToArray();

            #endregion

            try
            {

                if (_EVSEs.Any())
                {

                    DebugX.Log(OICPAction + " of " + _EVSEs.Count() + " EVSE static data sets at " + _HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseData_V2.0",
                                                            _UserAgent,
                                                            _DNSClient))
                    {

                        return await _OICPClient.Query(CPO_XMLMethods.PushEVSEDataXML(_EVSEs,
                                                                                      OICPAction,
                                                                                      OperatorId,
                                                                                      OperatorName),

                                                       "eRoamingPushEvseData",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                       OnSuccess: XMLData =>
                                                       {

                                                           #region Documentation

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

                                                           #endregion

                                                           var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE data: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                           return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: Fault =>
                                                       {

                                                           DebugX.Log(OICPAction + " of EVSE data led to a fault!" + Environment.NewLine);

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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE static data sets at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<HubjectAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<HubjectAcknowledgement>(new HTTPResponse(), e);

            }

        }

        #endregion

        #region PushEVSEData(EVSEDataRecord,   OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

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

            #region Initial checks

            if (EVSEDataRecord == null)
                throw new ArgumentNullException("EVSEDataRecord", "The given parameter must not be null!");

            #endregion

            return await PushEVSEData(new EVSEDataRecord[] { EVSEDataRecord },
                                      OICPAction,
                                      OperatorId   != null ? OperatorId : EVSEDataRecord.EVSEId.OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEDataRecords,  OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

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
                                                            _DNSClient))
                    {

                        return await _OICPClient.Query(CPO_XMLMethods.PushEVSEDataXML(_EVSEDataRecords,
                                                                                      OICPAction,
                                                                                      OperatorId,
                                                                                      OperatorName),

                                                       "eRoamingPushEvseData",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                       OnSuccess: XMLData =>
                                                       {

                                                           #region Documentation

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

                                                           #endregion

                                                           var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE data: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                           return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: Fault =>
                                                       {

                                                           DebugX.Log(OICPAction + " of EVSE data led to a fault!" + Environment.NewLine);

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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE static data records at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<HubjectAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<HubjectAcknowledgement>(new HTTPResponse(), e);

            }

        }

        #endregion


        #region PushEVSEStatus(EVSEOperator, OICPAction = update, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEStatus(EVSEOperator         EVSEOperator,
                           ActionType           OICPAction    = ActionType.update,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            try
            {

                var AllEVSEs = EVSEOperator.
                                   AllEVSEs.
                                   Where(IncludeEVSEs).
                                   //Where(evse => !EVSEOperator.InvalidEVSEIds.Contains(evse.Id)).
                                   ToArray();

                if (AllEVSEs.Any())
                {

                    DebugX.Log(OICPAction + " of " + AllEVSEs.Length + " EVSE states at " + _HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_Hostname,
                                                            _TCPPort,
                                                            _HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                            _UserAgent,
                                                            _DNSClient))
                    {

                        return await _OICPClient.Query(AllEVSEs.
                                                           PushEVSEStatusXML(OICPAction,
                                                                             EVSEOperator.Id,
                                                                             EVSEOperator.Name[Languages.de]),
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

                                                           var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE states: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                           return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: Fault =>
                                                       {

                                                           DebugX.Log(OICPAction + " of EVSE states led to a fault!" + Environment.NewLine);

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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE states at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<HubjectAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<HubjectAcknowledgement>(new HTTPResponse(), e);

            }

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<HubjectAcknowledgement>>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatus>>  EVSEStatus,
                           ActionType                                          OICPAction    = ActionType.update,
                           EVSEOperator_Id                                     OperatorId    = null,
                           String                                              OperatorName  = null,
                           TimeSpan?                                           QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException("EVSEStatus", "The given parameter must not be null!");

            if (OperatorId == null)
                OperatorId = EVSEStatus.First().Key.OperatorId;

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

                                                           var ack = HubjectAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE states: " + ack.Result + " / " + ack.Description + Environment.NewLine);

                                                           return new HTTPResponse<HubjectAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: Fault =>
                                                       {

                                                           DebugX.Log(OICPAction + " of EVSE states led to a fault!" + Environment.NewLine);

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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE states at " + _HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<HubjectAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<HubjectAcknowledgement>(new HTTPResponse(), e);

            }

        }

        #endregion

        #region PushEVSEStatusUpdates(EVSEStatusDiff)

        /// <summary>
        /// Send EVSE status updates upstream.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        public async Task PushEVSEStatusUpdates(EVSEStatusDiff EVSEStatusDiff)

        {

            if (EVSEStatusDiff == null)
                return;

            if (EVSEStatusDiff.NewEVSEStatus.Count()     == 0 &&
                EVSEStatusDiff.ChangedEVSEStatus.Count() == 0 &&
                EVSEStatusDiff.RemovedEVSEIds.Count()    == 0)
                return;

            var TrackingId = Guid.NewGuid().ToString();

            #region Insert new EVSEs...

            var NewEVSEStatus  = EVSEStatusDiff.
                                     NewEVSEStatus.
                                     Select(v => new KeyValuePair<EVSE_Id, EVSEStatusType>(v.Key, v.Value));

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

                var NewEVSEStatusSendingTask = await

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

            #endregion

            #region Upload EVSE changes...

            var ChangedEVSEStatus = EVSEStatusDiff.
                                        ChangedEVSEStatus.
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
                                                    UserAgent,
                                                    DNSClient))

            {

                var ChangedEVSEStatusSendingTask = await

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

            #endregion

            #region Remove outdated EVSEs...

            var RemovedEVSEStatus = EVSEStatusDiff.
                                        RemovedEVSEIds.
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
                                                    UserAgent,
                                                    DNSClient))

            {

                var RemovedEVSEIdsSendingTask = await

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

            #endregion

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP authorize start request.
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
                           EVSE_Id             EVSEId            = null,   // OICP v2.0: Optional
                           ChargingSession_Id  SessionId         = null,   // OICP v2.0: Optional
                           String              PartnerProductId  = null,   // OICP v2.0: Optional [100]
                           ChargingSession_Id  PartnerSessionId  = null,   // OICP v2.0: Optional [50]
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

                    return await _OICPClient.Query(CPO_XMLMethods.AuthorizeStartXML(OperatorId,
                                                                                    AuthToken,
                                                                                    EVSEId,
                                                                                    PartnerProductId,
                                                                                    SessionId,
                                                                                    PartnerSessionId),
                                                   "eRoamingAuthorizeStart",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData =>
                                                   {

                                                       var AuthStartResult = HubjectAuthorizationStart.Parse(XMLData.Content);

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

                                                           return new HTTPResponse<AUTHSTARTResult>(XMLData.HttpResponse,
                                                                                                    new AUTHSTARTResult(AuthorizatorId) {
                                                                                                        AuthorizationResult  = AuthorizationResult.Authorized,
                                                                                                        SessionId            = AuthStartResult.SessionID,
                                                                                                        PartnerSessionId     = PartnerSessionId,
                                                                                                        ProviderId           = EVSP_Id.Parse(AuthStartResult.ProviderID),
                                                                                                        Description          = AuthStartResult.Description,
                                                                                                        AdditionalInfo       = AuthStartResult.AdditionalInfo
                                                                                                    });

                                                       #endregion

                                                       #region NotAuthorized

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
                                                       //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Thueringen/L=Jena/O=Hubject/OU=GraphDefined GmbH/CN=GraphDefined Software Development/emailAddress=achim.friedland@graphdefined.com" from the certificate.</v1:AdditionalInfo>
                                                       //       </wsc:StatusCode>
                                                       //     </wsc:HubjectAuthorizationStop>
                                                       //   </isns:Body>
                                                       // </isns:Envelope>

                                                       if (AuthStartResult.Code == 017)
                                                           return new HTTPResponse<AUTHSTARTResult>(XMLData.HttpResponse,
                                                                                                    new AUTHSTARTResult(AuthorizatorId) {
                                                                                                        AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                                                                        PartnerSessionId     = PartnerSessionId,
                                                                                                        Description          = AuthStartResult.Description,
                                                                                                        AdditionalInfo       = AuthStartResult.AdditionalInfo
                                                                                                    });


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

                                                       return new HTTPResponse<AUTHSTARTResult>(XMLData.HttpResponse,
                                                                                                new AUTHSTARTResult(AuthorizatorId) {
                                                                                                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                                                                    PartnerSessionId     = PartnerSessionId,
                                                                                                    Description          = AuthStartResult.Description,
                                                                                                    AdditionalInfo       = AuthStartResult.AdditionalInfo
                                                                                                });

                                                       #endregion

                                                   },

                                                   OnSOAPFault: Fault =>
                                                   {

                                                       DebugX.Log("AUTHSTART of EVSE data led to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<AUTHSTARTResult>(Fault.HttpResponse,
                                                                                                new AUTHSTARTResult(AuthorizatorId) {
                                                                                                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                                                                    PartnerSessionId     = PartnerSessionId,
                                                                                                    Description          = Fault.ToString()
                                                                                                },
                                                                                                IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                                                   OnException: (t, s, e) => SendOnException(t, s, e)

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<AUTHSTARTResult>(new HTTPResponse(),
                                                         new AUTHSTARTResult(AuthorizatorId) {
                                                             AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                             PartnerSessionId     = PartnerSessionId,
                                                             Description          = "An exception occured: " + e.Message
                                                         });

            }

        }

        #endregion

        #region AuthorizeStop(OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, QueryTimeout = null)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP authorize stop request.
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
                                                                      EVSE_Id              EVSEId            = null,   // OICP v2.0: Optional
                                                                      ChargingSession_Id   PartnerSessionId  = null,   // OICP v2.0: Optional [50]
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

                    return await _OICPClient.Query(CPO_XMLMethods.AuthorizeStopXML(OperatorId,
                                                                                   SessionId,
                                                                                   AuthToken,
                                                                                   EVSEId,
                                                                                   PartnerSessionId),
                                                   "eRoamingAuthorizeStop",
                                                   QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                   OnSuccess: XMLData =>
                                                   {

                                                       var AuthStopResult = HubjectAuthorizationStop.Parse(XMLData.Content);

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

                                                           return new HTTPResponse<AUTHSTOPResult>(XMLData.HttpResponse,
                                                                                                   new AUTHSTOPResult(AuthorizatorId) {
                                                                                                       AuthorizationResult  = AuthorizationResult.Authorized,
                                                                                                       SessionId            = AuthStopResult.SessionID,
                                                                                                       PartnerSessionId     = PartnerSessionId,
                                                                                                       ProviderId           = EVSP_Id.Parse(AuthStopResult.ProviderID),
                                                                                                       Description          = AuthStopResult.Description
                                                                                                   });

                                                       #endregion

                                                       #region NotAuthorized

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
                                                       //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Thueringen/L=Jena/O=Hubject/OU=GraphDefined GmbH/CN=GraphDefined Software Development/emailAddress=achim.friedland@graphdefined.com" from the certificate.</v1:AdditionalInfo>
                                                       //       </wsc:StatusCode>
                                                       //     </wsc:HubjectAuthorizationStop>
                                                       //   </isns:Body>
                                                       // </isns:Envelope>

                                                       if (AuthStopResult.Code == 017)
                                                           return new HTTPResponse<AUTHSTOPResult>(XMLData.HttpResponse,
                                                                                                   new AUTHSTOPResult(AuthorizatorId) {
                                                                                                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                                                                       PartnerSessionId     = PartnerSessionId,
                                                                                                       Description          = AuthStopResult.Description + " - " + AuthStopResult.AdditionalInfo
                                                                                                   });


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

                                                       return new HTTPResponse<AUTHSTOPResult>(XMLData.HttpResponse,
                                                                                               new AUTHSTOPResult(AuthorizatorId) {
                                                                                                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                                                                   PartnerSessionId     = PartnerSessionId,
                                                                                                   Description          = AuthStopResult.Description
                                                                                               });

                                                       #endregion

                                                   },

                                                   OnSOAPFault: Fault =>
                                                   {

                                                       DebugX.Log("AUTHSTOP of EVSE data led to a fault!" + Environment.NewLine);

                                                       return new HTTPResponse<AUTHSTOPResult>(Fault.HttpResponse,
                                                                                                new AUTHSTOPResult(AuthorizatorId) {
                                                                                                    AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                                                                    PartnerSessionId     = PartnerSessionId,
                                                                                                    Description          = Fault.ToString()
                                                                                                },
                                                                                                IsFault: true);

                                                   },

                                                   OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                                                   OnException: (t, s, e) => SendOnException(t, s, e)

                                                  );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<AUTHSTOPResult>(new HTTPResponse(),
                                                        new AUTHSTOPResult(AuthorizatorId) {
                                                            AuthorizationResult  = AuthorizationResult.NotAuthorized,
                                                            PartnerSessionId     = PartnerSessionId,
                                                            Description          = "An exception occured: " + e.Message
                                                        });

            }

        }

        #endregion

        #region SendCDR(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendCDR(ChargeDetailRecord  ChargeDetailRecord,
                    TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            if ( ChargeDetailRecord.SessionTime == null || !ChargeDetailRecord.SessionTime.HasValue)
                throw new ArgumentNullException("SessionTime", "The given parameter must not be null or empty!");

            #endregion

            return await SendCDR(EVSEId:                ChargeDetailRecord.EVSEId,
                                 SessionId:             ChargeDetailRecord.SessionId,
                                 PartnerProductId:      ChargeDetailRecord.PartnerProductId,
                                 SessionStart:          ChargeDetailRecord.SessionTime.Value.StartTime,
                                 SessionEnd:            ChargeDetailRecord.SessionTime.Value.EndTime,
                                 AuthToken:             ChargeDetailRecord.AuthToken,
                                 eMAId:                 ChargeDetailRecord.eMAId,
                                 PartnerSessionId:      ChargeDetailRecord.PartnerSessionId,
                                 ChargingStart:         ChargeDetailRecord.SessionTime.HasValue ? ChargeDetailRecord.SessionTime.Value.StartTimeOpt : null,
                                 ChargingEnd:           ChargeDetailRecord.SessionTime.HasValue ? ChargeDetailRecord.SessionTime.Value.EndTimeOpt   : null,
                                 MeterValueStart:       ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? new Double?(ChargeDetailRecord.MeterValues.First().Value) : null,
                                 MeterValueEnd:         ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? new Double?(ChargeDetailRecord.MeterValues.Last(). Value) : null,
                                 MeterValuesInBetween:  ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? ChargeDetailRecord.MeterValues.Select(v => v.Value)       : null,
                                 ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy,
                                 MeteringSignature:     ChargeDetailRecord.MeteringSignature,

                                 QueryTimeout:          QueryTimeout);

        }

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendCDR(OICPChargeDetailRecord  ChargeDetailRecord,
                    TimeSpan?               QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            return await SendCDR(EVSEId:                ChargeDetailRecord.EVSEId,
                                 SessionId:             ChargeDetailRecord.SessionId,
                                 PartnerProductId:      ChargeDetailRecord.PartnerProductId,
                                 SessionStart:          ChargeDetailRecord.SessionStart,
                                 SessionEnd:            ChargeDetailRecord.SessionEnd,
                                 AuthToken:             ChargeDetailRecord.AuthToken,
                                 eMAId:                 ChargeDetailRecord.eMAId,
                                 PartnerSessionId:      ChargeDetailRecord.PartnerSessionId,
                                 ChargingStart:         ChargeDetailRecord.ChargingStart,
                                 ChargingEnd:           ChargeDetailRecord.ChargingEnd,
                                 MeterValueStart:       ChargeDetailRecord.MeterValueStart,
                                 MeterValueEnd:         ChargeDetailRecord.MeterValueEnd,
                                 MeterValuesInBetween:  ChargeDetailRecord.MeterValuesInBetween,
                                 ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy,
                                 MeteringSignature:     ChargeDetailRecord.MeteringSignature,
                                 HubOperatorId:         ChargeDetailRecord.HubOperatorId,
                                 HubProviderId:         ChargeDetailRecord.HubProviderId,

                                 QueryTimeout:          QueryTimeout);

        }

        #endregion

        #region SendCDR(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, PartnerSessionId = null, ..., QueryTimeout = null)

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
        /// <param name="ChargingTime">Optional timestamps of the charging start/stop.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<SENDCDRResult>>

            SendCDR(EVSE_Id              EVSEId,
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

            if (AuthToken        == null &&
                eMAId            == null)
                throw new ArgumentNullException("AuthToken / eMAId", "At least one of the given parameters must not be null!");

            #endregion

            try
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        TCPPort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        DNSClient: _DNSClient))
                {

                    return await _OICPClient.Query(CPO_XMLMethods.SendChargeDetailRecordXML(EVSEId,
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
                                                                                            HubProviderId),
                                                          "eRoamingChargeDetailRecord",
                                                          QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                          OnSuccess: XMLData =>
                                                          {

                                                              var ack = HubjectAcknowledgement.Parse(XMLData.Content);

                                                              #region Ok

                                                             if (ack.Result)
                                                                 return new HTTPResponse<SENDCDRResult>(XMLData.HttpResponse,
                                                                                                        new SENDCDRResult(AuthorizatorId) {
                                                                                                            State             = SENDCDRState.Forwarded,
                                                                                                            PartnerSessionId  = PartnerSessionId,
                                                                                                            Description       = ack.Description
                                                                                                        });

                                                             #endregion

                                                              #region Error

                                                             return new HTTPResponse<SENDCDRResult>(XMLData.HttpResponse,
                                                                                                    new SENDCDRResult(AuthorizatorId) {
                                                                                                        State             = SENDCDRState.False,
                                                                                                        PartnerSessionId  = PartnerSessionId,
                                                                                                        Description       = ack.Description
                                                                                                    });

                                                             #endregion

                                                          },

                                                          OnSOAPFault: Fault =>
                                                          {

                                                              DebugX.Log("SendCDR led to a fault!" + Environment.NewLine);

                                                              return new HTTPResponse<SENDCDRResult>(Fault.HttpResponse,
                                                                                                     new SENDCDRResult(AuthorizatorId) {
                                                                                                         State             = SENDCDRState.False,
                                                                                                         PartnerSessionId  = PartnerSessionId,
                                                                                                         Description       = Fault.ToString()
                                                                                                     },
                                                                                                     IsFault: true);

                                                          },

                                                          OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                                                          OnException: (t, s, e) => SendOnException(t, s, e)

                                                         );

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<SENDCDRResult>(new HTTPResponse(),
                                                       new SENDCDRResult(AuthorizatorId) {
                                                           State             = SENDCDRState.False,
                                                           PartnerSessionId  = PartnerSessionId,
                                                           Description       = "An exception occured: " + e.Message
                                                       });

            }

        }

        #endregion


    }

}
