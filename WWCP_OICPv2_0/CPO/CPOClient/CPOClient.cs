/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 CPO client.
    /// </summary>
    public class CPOClient : AOICPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined OICP v2.0 CPOClient";

        private readonly Random _Random;

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data record will be send upstream.
        /// </summary>
        public event OnEVSEDataPushDelegate   OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever new EVSE data record had been sent upstream.
        /// </summary>
        public event OnEVSEDataPushedDelegate OnEVSEDataPushed;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnEVSEStatusPushDelegate OnEVSEStatusPush;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public event OnEVSEStatusPushedDelegate OnEVSEStatusPushed;

        #endregion


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
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="TCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               TCPPort                     = null,
                         String                               HTTPVirtualHost             = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   TCPPort,
                   HTTPVirtualHost,
                   RemoteCertificateValidator,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        {

            this._Random = new Random(DateTime.Now.Millisecond);

        }

        #endregion


        #region PushEVSEData(GroupedEVSEDataRecords, OICPAction = fullLoad, OperatorId = null, OperatorName = null,                                QueryTimeout = null)

        /// <summary>
        /// Upload the given lookup of EVSE data records grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEDataRecords">A lookup of EVSE data records grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(ILookup<EVSEOperator, EVSEDataRecord>  GroupedEVSEDataRecords,
                         ActionType                             OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                        OperatorId    = null,
                         String                                 OperatorName  = null,
                         TimeSpan?                              QueryTimeout  = null)

        {

            #region Initial checks

            if (GroupedEVSEDataRecords == null)
                throw new ArgumentNullException("GroupedEVSEDataRecords", "The given lookup of EVSE data records must not be null!");

            #endregion

            #region Get effective number of EVSE data records to upload

            HTTPResponse<eRoamingAcknowledgement> _result = null;

            var NumberOfEVSEDataRecords = GroupedEVSEDataRecords.
                                              Select(group => group.Count()).
                                              Sum   ();

            var StartTime = DateTime.Now;

            #endregion


            if (NumberOfEVSEDataRecords > 0)
            {

                #region Send OnEVSEDataPush event

                var OnEVSEDataPushLocal = OnEVSEDataPush;
                if (OnEVSEDataPushLocal != null)
                    OnEVSEDataPushLocal(StartTime, this, ClientId, OICPAction, GroupedEVSEDataRecords, (UInt32) NumberOfEVSEDataRecords);

                #endregion

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseData_V2.0",
                                                        _UserAgent,
                                                        _RemoteCertificateValidator,
                                                        _DNSClient))
                {

                    _result = await _OICPClient.Query(CPOClient_XMLMethods.PushEVSEDataXML(GroupedEVSEDataRecords,
                                                                                           OICPAction,
                                                                                           OperatorId,
                                                                                           OperatorName),
                                                      "eRoamingPushEvseData",
                                                      QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                      #region OnSuccess

                                                      OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                      #endregion

                                                      #region OnSOAPFault

                                                      OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                          SendSOAPError(timestamp, this, httpresponse.Content);

                                                          return new HTTPResponse<eRoamingAcknowledgement>(
                                                              httpresponse,
                                                              new eRoamingAcknowledgement(false, 0, "", ""),
                                                              IsFault: true);

                                                      },

                                                      #endregion

                                                      #region OnHTTPError

                                                      OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                          SendHTTPError(timestamp, this, httpresponse);

                                                          return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                           new eRoamingAcknowledgement(false,
                                                                                                                                       -1,
                                                                                                                                       httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                       httpresponse.HTTPBody.ToUTF8String()),
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

            else
                _result = HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(true, 0));


            #region Send OnEVSEDataPushed event

            var EndTime = DateTime.Now;

            var OnEVSEDataPushedLocal = OnEVSEDataPushed;
            if (OnEVSEDataPushedLocal != null)
                OnEVSEDataPushedLocal(EndTime, this, ClientId, OICPAction, GroupedEVSEDataRecords, (UInt32) NumberOfEVSEDataRecords, _result.Content, EndTime - StartTime);

            #endregion

            return _result;

        }

        #endregion

        #region PushEVSEData(EVSEDataRecord,         OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEDataRecords = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data record.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
                         ActionType                     OICPAction    = ActionType.insert,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEDataRecords  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            return await PushEVSEData(new EVSEDataRecord[] { EVSEDataRecord },
                                      OICPAction,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEDataRecords,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEDataRecords,        OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEDataRecords = null, QueryTimeout = null)

        /// <summary>
        /// Upload the given enumeration of EVSE data records.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction              = ActionType.fullLoad,
                         EVSEOperator_Id                OperatorId              = null,
                         String                         OperatorName            = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEDataRecords  = null,
                         TimeSpan?                      QueryTimeout            = null)

        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException("EVSEDataRecords", "The given enumeration of EVSE data records must not be null!");

            if (IncludeEVSEDataRecords == null)
                IncludeEVSEDataRecords = EVSEDataRecord => true;

            var _EVSEDataRecords = EVSEDataRecords.
                                       Where(evsedatarecord => IncludeEVSEDataRecords(evsedatarecord)).
                                       ToArray();

            #endregion

            if (_EVSEDataRecords.Any())
                return await PushEVSEData(_EVSEDataRecords.ToLookup(evsedatarecord => evsedatarecord.EVSEOperator),
                                          OICPAction,
                                          OperatorId,
                                          OperatorName,
                                          QueryTimeout);


            return HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(true, 0));

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

            return await PushEVSEData(EVSEDataRecords,
                                      OICPAction);

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

            return await PushEVSEData(EVSEDataRecords,
                                      OICPAction,
                                      OperatorId);

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

            return await PushEVSEData(EVSEDataRecords,
                                      OICPAction,
                                      OperatorId,
                                      OperatorName);

        }

        #endregion


        #region PushEVSEStatus(GroupedEVSEStatusRecords, OICPAction = update, OperatorId = null, OperatorName = null,                                  QueryTimeout = null)

        /// <summary>
        /// Upload the given lookup of EVSE status records grouped by their EVSE operator identification.
        /// </summary>
        /// <param name="GroupedEVSEStatusRecords">A lookup of EVSE status records grouped by their EVSE operator identification.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(ILookup<EVSEOperator_Id, EVSEStatusRecord>  GroupedEVSEStatusRecords,
                           ActionType                                  OICPAction    = ActionType.update,
                           EVSEOperator_Id                             OperatorId    = null,
                           String                                      OperatorName  = null,
                           TimeSpan?                                   QueryTimeout  = null)

        {

            #region Initial checks

            if (GroupedEVSEStatusRecords == null)
                throw new ArgumentNullException("GroupedEVSEStatusRecords", "The given lookup of EVSE status records must not be null!");

            #endregion

            #region Get effective number of EVSE data records to upload

            HTTPResponse<eRoamingAcknowledgement> _result = null;

            var NumberOfEVSEStatusRecords = GroupedEVSEStatusRecords.
                                                Select(group => group.Count()).
                                                Sum   ();

            var StartTime = DateTime.Now;

            #endregion


            // Wait a random number of milliseconds, as Hubject
            // does not allow parallel requests.
            Thread.Sleep(_Random.Next(5000));

            if (NumberOfEVSEStatusRecords > 0)
            {

                #region Send OnEVSEStatusPush event

                var OnEVSEStatusPushLocal = OnEVSEStatusPush;
                if (OnEVSEStatusPushLocal != null)
                    OnEVSEStatusPushLocal(StartTime, this, ClientId, OICPAction, GroupedEVSEStatusRecords, (UInt32) NumberOfEVSEStatusRecords);

                #endregion

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                        _TCPPort,
                                                        _HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                        _UserAgent,
                                                        _RemoteCertificateValidator,
                                                        _DNSClient))
                {

                    _result = await _OICPClient.Query(CPOClient_XMLMethods.PushEVSEStatusXML(GroupedEVSEStatusRecords,
                                                                                             OICPAction,
                                                                                             OperatorId,
                                                                                             OperatorName),
                                                      "eRoamingPushEvseStatus",
                                                      QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                                      #region OnSuccess

                                                      OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                      #endregion

                                                      #region OnSOAPFault

                                                      OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                          SendSOAPError(timestamp, this, httpresponse.Content);

                                                          return new HTTPResponse<eRoamingAcknowledgement>(
                                                              httpresponse,
                                                              new eRoamingAcknowledgement(false, 0, "", ""),
                                                              IsFault: true);

                                                      },

                                                      #endregion

                                                      #region OnHTTPError

                                                      OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                          SendHTTPError(timestamp, this, httpresponse);

                                                          return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                           new eRoamingAcknowledgement(false,
                                                                                                                                       -1,
                                                                                                                                       httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                       httpresponse.HTTPBody.ToUTF8String()),
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

            else
                _result = HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(true, 0));


            #region Send OnEVSEStatusPushed event

            var EndTime = DateTime.Now;

            var OnEVSEStatusPushedLocal = OnEVSEStatusPushed;
            if (OnEVSEStatusPushedLocal != null)
                OnEVSEStatusPushedLocal(EndTime, this, ClientId, OICPAction, GroupedEVSEStatusRecords, (UInt32) NumberOfEVSEStatusRecords, _result.Content, EndTime - StartTime);

            #endregion

            return _result;

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus,               OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEStatusRecords = null, QueryTimeout = null)

        /// <summary>
        /// Upload the given enumeration of EVSE status records.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<EVSEStatusRecord>    EVSEStatusRecords,
                           ActionType                       OICPAction                = ActionType.update,
                           EVSEOperator_Id                  OperatorId                = null,
                           String                           OperatorName              = null,
                           Func<EVSEStatusRecord, Boolean>  IncludeEVSEStatusRecords  = null,
                           TimeSpan?                        QueryTimeout              = null)

        {

            #region Initial checks

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException("EVSEStatusRecords", "The given enumeration of EVSE status records must not be null!");

            if (IncludeEVSEStatusRecords == null)
                IncludeEVSEStatusRecords = EVSEStatusRecord => true;

            var _EVSEStatusRecords = EVSEStatusRecords.
                                         Where(evsestatusrecord => IncludeEVSEStatusRecords(evsestatusrecord)).
                                         ToArray();

            #endregion

            if (_EVSEStatusRecords.Any())
                return await PushEVSEStatus(_EVSEStatusRecords.ToLookup(evsestatusrecord => evsestatusrecord.Id.OperatorId),
                                            OICPAction,
                                            OperatorId,
                                            OperatorName,
                                            QueryTimeout);


            return HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(true, 0));

        }

        #endregion

        #region PushEVSEStatus(KeyValuePairs<...>,       OICPAction = update, OperatorId = null, OperatorName = null,                                  QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE status key-value-pairs onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE identification and status key-value-pairs.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator identification to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEStatus,
                           ActionType                                          OICPAction    = ActionType.update,
                           EVSEOperator_Id                                     OperatorId    = null,
                           String                                              OperatorName  = null,
                           TimeSpan?                                           QueryTimeout  = null)

        {

            return await PushEVSEStatus(EVSEStatus.Select(kvp => new EVSEStatusRecord(kvp.Key, kvp.Value)),
                                        OICPAction,
                                        OperatorId,
                                        OperatorName,
                                        null,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEStatusDiff, QueryTimeout = null)

        /// <summary>
        /// Send EVSE status updates upstream.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task PushEVSEStatus(EVSEStatusDiff  EVSEStatusDiff,
                                         TimeSpan?       QueryTimeout  = null)

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

                var result = await PushEVSEStatus(NewEVSEStatus,
                                                  ActionType.insert,
                                                  EVSEStatusDiff.EVSEOperatorId,
                                                  null,
                                                  null,
                                                  QueryTimeout);

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

                var result = await PushEVSEStatus(ChangedEVSEStatus,
                                                  ActionType.update,
                                                  EVSEStatusDiff.EVSEOperatorId,
                                                  null,
                                                  null,
                                                  QueryTimeout);

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

                var result = await PushEVSEStatus(RemovedEVSEStatus.Select(EVSEId => new EVSEStatusRecord(EVSEId, EVSEStatusType.OutOfService)),
                                                  ActionType.delete,
                                                  EVSEStatusDiff.EVSEOperatorId,
                                                  null,
                                                  null,
                                                  QueryTimeout);

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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
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

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAuthorizationStart.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                       new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                                                      StatusCode: new StatusCode(-1,
                                                                                                                                                                 Description: httpresponse.Content.ToString())),
                                                                                                       IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                       new eRoamingAuthorizationStart(AuthorizationStatusType.NotAuthorized,
                                                                                                                                      StatusCode: new StatusCode(-1,
                                                                                                                                                                 Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                                 AdditionalInfo: httpresponse.HTTPBody.ToUTF8String())),
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

        #region AuthorizeStop (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, QueryTimeout = null)

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
        public async Task<HTTPResponse<eRoamingAuthorizationStop>>

            AuthorizeStop(EVSEOperator_Id      OperatorId,
                          ChargingSession_Id   SessionId,
                          Auth_Token           AuthToken,
                          EVSE_Id              EVSEId            = null,
                          ChargingSession_Id   PartnerSessionId  = null,   // [maxlength: 50]
                          TimeSpan?            QueryTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient: _DNSClient))
            {

                return await _OICPClient.Query(CPOClient_XMLMethods.AuthorizeStopXML(OperatorId,
                                                                                     SessionId,
                                                                                     AuthToken,
                                                                                     EVSEId,
                                                                                     PartnerSessionId),
                                               "eRoamingAuthorizeStop",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAuthorizationStop.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                      new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                                                    StatusCode: new StatusCode(-1,
                                                                                                                                                               Description: httpresponse.Content.ToString())),
                                                                                                      IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                      new eRoamingAuthorizationStop(AuthorizationStatusType.NotAuthorized,
                                                                                                                                    StatusCode: new StatusCode(-1,
                                                                                                                                                               Description: httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                               AdditionalInfo: httpresponse.HTTPBody.ToUTF8String())),
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


        #region PullAuthenticationData(OperatorId, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 PullAuthenticationData request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAuthenticationData>> PullAuthenticationData(EVSEOperator_Id  OperatorId,
                                                                                           TimeSpan?        QueryTimeout = null)
        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient: _DNSClient))
            {

                return await _OICPClient.Query(CPOClient_XMLMethods.PullAuthenticationDataXML(OperatorId),
                                               "eRoamingPullAuthenticationData",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAuthenticationData.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAuthenticationData>(httpresponse,
                                                                                                       IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAuthenticationData>(httpresponse,
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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient: _DNSClient))
            {

                return await _OICPClient.Query(CPOClient_XMLMethods.SendChargeDetailRecordXML(ChargeDetailRecord),
                                               "eRoamingChargeDetailRecord",
                                               QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                    new eRoamingAcknowledgement(false,
                                                                                                                                -1,
                                                                                                                                httpresponse.Content.ToString()),
                                                                                                    IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                    new eRoamingAcknowledgement(false,
                                                                                                                                -1,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.ToUTF8String()),
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

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    _UserAgent,
                                                    _RemoteCertificateValidator,
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

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse), 

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                    new eRoamingAcknowledgement(false,
                                                                                                                                -1,
                                                                                                                                httpresponse.Content.ToString()),
                                                                                                    IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                    new eRoamingAcknowledgement(false,
                                                                                                                                -1,
                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                httpresponse.HTTPBody.ToUTF8String()),
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
