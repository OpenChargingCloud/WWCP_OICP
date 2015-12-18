/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 CPO Upstream Service(s).
    /// </summary>
    public class WWCP_CPOClient : IAuthServices
    {

        #region Data

        private readonly CPOClient _CPOClient;

        #endregion

        #region Properties

        #region AuthorizatorId

        protected readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

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
        /// Create a new OICP v2.0 CPO Upstream Service.
        /// </summary>
        /// <param name="Hostname">The hostname of the OICP service.</param>
        /// <param name="TCPPort">The TCP port of the OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the OICP service.</param>
        /// <param name="AuthorizatorId">An optional unique authorizator identification.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCP_CPOClient(String           Hostname,
                              IPPort           TCPPort,
                              String           HTTPVirtualHost  = null,
                              Authorizator_Id  AuthorizatorId   = null,
                              String           HTTPUserAgent    = "GraphDefined OICP v2.0 Gateway CPO Upstream Services",
                              TimeSpan?        QueryTimeout     = null,
                              DNSClient        DNSClient        = null)
        {

            this._CPOClient       = new CPOClient(Hostname,
                                                  TCPPort != null ? TCPPort : IPPort.Parse(443),
                                                  HTTPVirtualHost,
                                                  HTTPUserAgent,
                                                  QueryTimeout,
                                                  DNSClient);

            this._AuthorizatorId  = AuthorizatorId;

        }

        #endregion


        #region Tokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];
            }
        }

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens
        {
            get
            {
                return new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];
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
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

                    DebugX.Log(OICPAction + " of " + NumberOfEVSEs + " EVSE static data sets at " + _CPOClient.HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_CPOClient.Hostname,
                                                            _CPOClient.TCPPort,
                                                            _CPOClient.HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseData_V2.0",
                                                            _CPOClient.UserAgent,
                                                            false,
                                                            _CPOClient.DNSClient))
                    {

                        return await _OICPClient.Query(WWCP_CPOClient_XMLMethods.PushEVSEDataXML(GroupedData,
                                                                                                OICPAction,
                                                                                                OperatorId,
                                                                                                OperatorName),

                                                       "eRoamingPushEvseData",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : _CPOClient.QueryTimeout,

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

                                                           var ack = eRoamingAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE data: " + ack.Result + " / " + ack.StatusCode.Description + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                           DebugX.Log(OICPAction + " of EVSE data led to a fault!" + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(
                                                               soapfault.HttpResponse,
                                                               new eRoamingAcknowledgement(false, 0, "", ""),
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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE static data sets at " + _CPOClient.HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<eRoamingAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                if (e.InnerException != null)
                    e = e.InnerException;

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(), new eRoamingAcknowledgement(false, 0, e.Message), e);

            }

        }

        #endregion

        #region PushEVSEData(EVSEOperator,     OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

                    DebugX.Log(OICPAction + " of " + _EVSEs.Count() + " EVSE static data sets at " + _CPOClient.HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_CPOClient.Hostname,
                                                            _CPOClient.TCPPort,
                                                            _CPOClient.HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseData_V2.0",
                                                            _CPOClient.UserAgent,
                                                            false,
                                                            _CPOClient.DNSClient))
                    {

                        return await _OICPClient.Query(WWCP_CPOClient_XMLMethods.PushEVSEDataXML(_EVSEs,
                                                                                                OICPAction,
                                                                                                OperatorId,
                                                                                                OperatorName),

                                                       "eRoamingPushEvseData",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : _CPOClient.QueryTimeout,

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

                                                           var ack = eRoamingAcknowledgement.Parse(XMLData.Content);
                                                           DebugX.Log(OICPAction + " of EVSE data: " + ack.Result + " / " + ack.StatusCode.Description + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(XMLData.HttpResponse, ack, false);

                                                       },


                                                       OnSOAPFault: (timestamp, soapclient, soapfault) => {

                                                           DebugX.Log(OICPAction + " of EVSE data led to a fault!" + Environment.NewLine);

                                                           return new HTTPResponse<eRoamingAcknowledgement>(
                                                               soapfault.HttpResponse,
                                                               new eRoamingAcknowledgement(false, 0, "", ""),
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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE static data sets at " + _CPOClient.HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<eRoamingAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(), e);

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
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

        #region PushEVSEData(OperatorId, OperatorName, OICPAction, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OperatorId">The EVSE operator Id to use.</param>
        /// <param name="OperatorName">The EVSE operator name.</param>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(EVSEOperator_Id          OperatorId,
                         String                   OperatorName,
                         ActionType               OICPAction,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await PushEVSEData(EVSEDataRecords, OICPAction, OperatorId, OperatorName);

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
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            return null;

        }

        #endregion


        #region PushEVSEStatus(EVSEOperator, OICPAction = update, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

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

                    DebugX.Log(OICPAction + " of " + AllEVSEs.Length + " EVSE states at " + _CPOClient.HTTPVirtualHost + "...");

                    using (var _OICPClient = new SOAPClient(_CPOClient.Hostname,
                                                            _CPOClient.TCPPort,
                                                            _CPOClient.HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                            _CPOClient.UserAgent,
                                                            false,
                                                            _CPOClient.DNSClient))
                    {

                        return await _OICPClient.Query(AllEVSEs.
                                                           PushEVSEStatusXML(OICPAction,
                                                                             EVSEOperator.Id,
                                                                             EVSEOperator.Name[Languages.de]),
                                                       "eRoamingPushEvseStatus",
                                                       QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : _CPOClient.QueryTimeout,

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
                else
                {

                    DebugX.Log(OICPAction + " of EVSE states at " + _CPOClient.HTTPVirtualHost + " skipped!");

                    return new HTTPResponse<eRoamingAcknowledgement>();

                }

            }

            // Note: Will only catch SOAPClient init and query init exceptions!
            catch (Exception e)
            {

                SendOnException(DateTime.Now, this, e);

                if (e.InnerException != null)
                    e = e.InnerException;

                return new HTTPResponse<eRoamingAcknowledgement>(new HTTPResponse(), new eRoamingAcknowledgement(false, 0, e.Message), e);

            }

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEStatus,
                           ActionType                                          OICPAction    = ActionType.update,
                           EVSEOperator_Id                                     OperatorId    = null,
                           String                                              OperatorName  = null,
                           TimeSpan?                                           QueryTimeout  = null)

        {

            return null;

        }

        #endregion

        #region PushEVSEStatusUpdates(EVSEStatusDiff)

        /// <summary>
        /// Send EVSE status updates upstream.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        public async Task PushEVSEStatusUpdates(EVSEStatusDiff EVSEStatusDiff)

        {

            await _CPOClient.PushEVSEStatusUpdates(EVSEStatusDiff);

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, ChargingProductId = null, SessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStart request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId             = null,
                           ChargingProduct_Id  ChargingProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            var AuthStartTask = await _CPOClient.AuthorizeStart(OperatorId,
                                                                  AuthToken,
                                                                  EVSEId,
                                                                  SessionId,
                                                                  ChargingProductId,
                                                                  null,
                                                                  QueryTimeout);

            if (AuthStartTask.HttpResponse.HTTPStatusCode == HTTPStatusCode.OK)
            {

                if (AuthStartTask.Content.AuthorizationStatus == AuthorizationStatusType.Authorized)
                    return AuthStartEVSEResult.Authorized(AuthorizatorId,
                                                          AuthStartTask.Content.SessionId,
                                                          AuthStartTask.Content.ProviderId,
                                                          AuthStartTask.Content.StatusCode.Description,
                                                          AuthStartTask.Content.StatusCode.AdditionalInfo);

                return AuthStartEVSEResult.NotAuthorized(AuthorizatorId,
                                                         AuthStartTask.Content.ProviderId,
                                                         AuthStartTask.Content.StatusCode.Description,
                                                         AuthStartTask.Content.StatusCode.AdditionalInfo);

            }

            return AuthStartEVSEResult.Error(AuthorizatorId,
                                             "HTTP error: " + AuthStartTask.HttpResponse.HTTPStatusCode.ToString());

        }

        #endregion

        #region AuthorizeStart(OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStart request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingStation_Id  ChargingStationId,
                           ChargingProduct_Id  ChargingProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            #region Initial checks

            if (OperatorId        == null)
                throw new ArgumentNullException("OperatorId",         "The given parameter must not be null!");

            if (AuthToken         == null)
                throw new ArgumentNullException("AuthToken",          "The given parameter must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException("ChargingStationId",  "The given parameter must not be null!");

            #endregion

            //ToDo: Implement AuthorizeStart(...ChargingStationId...)
            return AuthStartChargingStationResult.Error(AuthorizatorId, "Not implemented!");

        }

        #endregion

        #region AuthorizeStop(OperatorId, SessionId, AuthToken, EVSEId = null, QueryTimeout = null)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopEVSEResult> AuthorizeStop(EVSEOperator_Id      OperatorId,
                                                        ChargingSession_Id   SessionId,
                                                        Auth_Token           AuthToken,
                                                        EVSE_Id              EVSEId            = null,
                                                        TimeSpan?            QueryTimeout      = null)
        {

            var AuthorizationStopResult = await _CPOClient.AuthorizeStop(OperatorId,
                                                                         SessionId,
                                                                         AuthToken,
                                                                         EVSEId,
                                                                         null,
                                                                         QueryTimeout);

            // Authorized
            if (AuthorizationStopResult.Content.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return new AuthStopEVSEResult(AuthorizatorId) {
                           AuthorizationResult  = AuthStopEVSEResultType.Success,
                           SessionId            = AuthorizationStopResult.Content.SessionId,
                           ProviderId           = AuthorizationStopResult.Content.ProviderId,
                           Description          = AuthorizationStopResult.Content.StatusCode.Description,
                           AdditionalInfo       = AuthorizationStopResult.Content.StatusCode.AdditionalInfo
                       };

            // NotAuthorized
            else
                return new AuthStopEVSEResult(AuthorizatorId) {
                           AuthorizationResult  = AuthStopEVSEResultType.Error,
                           SessionId            = AuthorizationStopResult.Content.SessionId,
                           ProviderId           = AuthorizationStopResult.Content.ProviderId,
                           Description          = AuthorizationStopResult.Content.StatusCode.Description,
                           AdditionalInfo       = AuthorizationStopResult.Content.StatusCode.AdditionalInfo
                       };

        }

        #endregion

        #region AuthorizeStop(OperatorId, SessionId, AuthToken, ChargingStationId, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopChargingStationResult> AuthorizeStop(EVSEOperator_Id      OperatorId,
                                                                       ChargingSession_Id   SessionId,
                                                                       Auth_Token           AuthToken,
                                                                       ChargingStation_Id   ChargingStationId,
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

            return new AuthStopChargingStationResult(AuthorizatorId) {
                       AuthorizationResult  = AuthStopChargingStationResultType.Error
                   };

        }

        #endregion



        #region PullAuthenticationData(OperatorId, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 PullAuthenticationData request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAuthenticationData>>

            PullAuthenticationData(EVSEOperator_Id  OperatorId,
                                   TimeSpan?        QueryTimeout = null)

        {

            return await _CPOClient.PullAuthenticationData(OperatorId,
                                                           QueryTimeout);

        }

        #endregion


        #region SendChargeDetailRecord(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            var result = await _CPOClient.SendChargeDetailRecord(EVSEId:                ChargeDetailRecord.EVSEId,
                                                                 SessionId:             ChargeDetailRecord.SessionId,
                                                                 PartnerProductId:      ChargeDetailRecord.PartnerProductId,
                                                                 SessionStart:          ChargeDetailRecord.SessionTime.Value.StartTime,
                                                                 SessionEnd:            ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                                                                 Identification:        new AuthorizationIdentification(ChargeDetailRecord.Identification),
                                                                 PartnerSessionId:      ChargeDetailRecord.PartnerSessionId,
                                                                 ChargingStart:         ChargeDetailRecord.SessionTime.HasValue ? new Nullable<DateTime>(ChargeDetailRecord.SessionTime.Value.StartTime) : null,
                                                                 ChargingEnd:           ChargeDetailRecord.SessionTime.HasValue ?                        ChargeDetailRecord.SessionTime.Value.EndTime    : null,
                                                                 MeterValueStart:       ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? new Double?(ChargeDetailRecord.MeterValues.First().Value) : null,
                                                                 MeterValueEnd:         ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? new Double?(ChargeDetailRecord.MeterValues.Last(). Value) : null,
                                                                 MeterValuesInBetween:  ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? ChargeDetailRecord.MeterValues.Select(v => v.Value)       : null,
                                                                 ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy,
                                                                 MeteringSignature:     ChargeDetailRecord.MeteringSignature,
                                                                 QueryTimeout:          QueryTimeout);

            return result.Content;

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, Identification, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">The ev charging product identification.</param>
        /// <param name="SessionStart">The session start timestamp.</param>
        /// <param name="SessionEnd">The session end timestamp.</param>
        /// <param name="Identification">An identification.</param>
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
        public new async Task<SendCDRResult>

            SendChargeDetailRecord(EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   PartnerProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   AuthInfo             Identification,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   HubOperator_Id       HubOperatorId         = null,
                                   EVSP_Id              HubProviderId         = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            var Ack = await _CPOClient.SendChargeDetailRecord(EVSEId,
                                                              SessionId,
                                                              PartnerProductId,
                                                              SessionStart,
                                                              SessionEnd,
                                                              new AuthorizationIdentification(Identification),
                                                              null,
                                                              ChargingStart,
                                                              ChargingEnd,
                                                              MeterValueStart,
                                                              MeterValueEnd,
                                                              MeterValuesInBetween,
                                                              ConsumedEnergy,
                                                              MeteringSignature,
                                                              HubOperatorId,
                                                              HubProviderId,
                                                              QueryTimeout);



            // true
            if (Ack.Content.Result)
                return SendCDRResult.Forwarded(_AuthorizatorId);

            // false
            else
                return SendCDRResult.False(_AuthorizatorId);

        }

        #endregion



        #region (protected) SendOnHTTPError(Timestamp, Sender, HttpResponse)

        /// <summary>
        /// Notify that an HTTP error occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the error received.</param>
        /// <param name="Sender">The sender of this error message.</param>
        /// <param name="HttpResponse">The HTTP response related to this error message.</param>
        protected void SendOnHTTPError(DateTime      Timestamp,
                                       Object        Sender,
                                       HTTPResponse  HttpResponse)
        {

            DebugX.Log("AOICPUpstreamService => HTTP Status Code: " + HttpResponse != null ? HttpResponse.HTTPStatusCode.ToString() : "<null>");

            var OnHTTPErrorLocal = OnHTTPError;
            if (OnHTTPErrorLocal != null)
                OnHTTPErrorLocal(Timestamp, Sender, HttpResponse);

        }

        #endregion

        #region (protected) SendOnException(Timestamp, Sender, Exception)

        /// <summary>
        /// Notify that an exception occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the exception.</param>
        /// <param name="Sender">The sender of this exception.</param>
        /// <param name="Exception">The exception itself.</param>
        protected void SendOnException(DateTime   Timestamp,
                                       Object     Sender,
                                       Exception  Exception)
        {

            DebugX.Log("AOICPUpstreamService => Exception: " + Exception.Message);

            var OnExceptionLocal = OnException;
            if (OnExceptionLocal != null)
                OnExceptionLocal(Timestamp, Sender, Exception);

        }

        #endregion


    }

}
