﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Diagnostics;
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
    /// An OICP CPO Client.
    /// </summary>
    public class CPOClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined OICP " + Version.Number + " CPO Client";

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnEVSEDataPushDelegate    OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler   OnEVSEDataPushRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler  OnEVSEDataPushResponse;

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnEVSEDataPushedDelegate  OnEVSEDataPushed;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnEVSEStatusPushDelegate   OnEVSEStatusPush;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnEVSEStatusPushRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnEVSEStatusPushResponse;

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnEVSEStatusPushedDelegate OnEVSEStatusPushed;

        #endregion

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartHandler    OnAuthorizeStart;

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnAuthorizeStartResponse;

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartedHandler  OnAuthorizeStarted;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize stop' request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestHandler   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnAuthorizeStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnAuthorizeStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' request had been received.
        /// </summary>
        public event OnAuthorizeStopResponseHandler  OnAuthorizeStopResponse;

        #endregion

        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a 'charge detail record' will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestHandler   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a 'charge detail record' will be send via SOAP.
        /// </summary>
        public event ClientRequestLogHandler                  OnSendChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response to a sent 'charge detail record' had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSendChargeDetailRecordSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a sent 'charge detail record' had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler  OnSendChargeDetailRecordResponse;

        #endregion

        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull authentication data' request will be send.
        /// </summary>
        public event OnPullAuthenticationDataRequestHandler   OnPullAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a 'pull authentication data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnPullAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull authentication data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnPullAuthenticationDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull authentication data' request had been received.
        /// </summary>
        public event OnPullAuthenticationDataResponseHandler  OnPullAuthenticationDataResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP CPO Client.
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

        { }

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

            var NumberOfEVSEDataRecords = GroupedEVSEDataRecords.
                                              Select(group => group.Count()).
                                              Sum   ();

            #endregion


            if (NumberOfEVSEDataRecords > 0)
            {

                #region Send OnEVSEDataPush event

                var Runtime = Stopwatch.StartNew();

                try
                {

                    OnEVSEDataPush?.Invoke(DateTime.Now,
                                           this,
                                           ClientId,
                                           OICPAction,
                                           GroupedEVSEDataRecords,
                                           (UInt32) NumberOfEVSEDataRecords);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnEVSEDataPush));
                }

                #endregion

                using (var OICPClient = new SOAPClient(_Hostname,
                                                       _TCPPort,
                                                       _HTTPVirtualHost,
                                                       "/ibis/ws/eRoamingEvseData_V2.0",
                                                       UserAgent,
                                                       _RemoteCertificateValidator,
                                                       DNSClient))
                {

                    var result = await OICPClient.Query(CPOClientXMLMethods.PushEVSEDataXML(GroupedEVSEDataRecords,
                                                                                            OICPAction,
                                                                                            OperatorId,
                                                                                            OperatorName),
                                                        "eRoamingPushEvseData",
                                                        RequestLogDelegate:   OnEVSEDataPushRequest,
                                                        ResponseLogDelegate:  OnEVSEDataPushResponse,
                                                        QueryTimeout:         QueryTimeout != null ? QueryTimeout.Value : this.RequestTimeout,

                                                        #region OnSuccess

                                                        OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                        #endregion

                                                        #region OnSOAPFault

                                                        OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                            SendSOAPError(timestamp, this, httpresponse.Content);

                                                            return new HTTPResponse<eRoamingAcknowledgement>(
                                                                httpresponse,
                                                                new eRoamingAcknowledgement(StatusCodes.SystemError),
                                                                IsFault: true);

                                                        },

                                                        #endregion

                                                        #region OnHTTPError

                                                        OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                            SendHTTPError(timestamp, this, httpresponse);

                                                            return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                             new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                                                                                         httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                         httpresponse.HTTPBody.      ToUTF8String()),
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


                    #region Send OnEVSEDataPushed event

                    Runtime.Stop();

                    try
                    {

                        OnEVSEDataPushed?.Invoke(DateTime.Now,
                                                 this,
                                                 ClientId,
                                                 OICPAction,
                                                 GroupedEVSEDataRecords,
                                                 (UInt32)NumberOfEVSEDataRecords,
                                                 result.Content,
                                                 Runtime.Elapsed);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOClient) + "." + nameof(OnEVSEDataPushed));
                    }

                    #endregion

                    return result;

                }

            }

            return HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success));

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


            return HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success));

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


        #region PushEVSEStatus(...EVSEStatusRecords,  OICPAction = update, OperatorId = null, OperatorName = null, ...)

        /// <summary>
        /// Upload the given lookup of EVSE status records grouped by their EVSE operator identification.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                           ActionType                     OICPAction    = ActionType.update,
                           EVSEOperator_Id                OperatorId    = null,
                           String                         OperatorName  = null,
                           TimeSpan?                      QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null!");

            #endregion

            #region Get effective number of EVSE data records to upload

            var _EVSEStatusRecords         = EVSEStatusRecords.ToArray();
            var NumberOfEVSEStatusRecords  = _EVSEStatusRecords.Count();

            #endregion


            if (NumberOfEVSEStatusRecords > 0)
            {

                #region Send OnEVSEStatusPush event

                var Runtime = Stopwatch.StartNew();

                try
                {

                    OnEVSEStatusPush?.Invoke(DateTime.Now,
                                             this,
                                             ClientId,
                                             OICPAction,
                                             _EVSEStatusRecords,
                                             (UInt32) NumberOfEVSEStatusRecords);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnEVSEStatusPush));
                }

                #endregion

                using (var _OICPClient = new SOAPClient(_Hostname,
                                                       _TCPPort,
                                                       _HTTPVirtualHost,
                                                       "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                       UserAgent,
                                                       _RemoteCertificateValidator,
                                                       DNSClient))
                {

                     var result = await _OICPClient.Query(CPOClientXMLMethods.PushEVSEStatusXML(_EVSEStatusRecords,
                                                                                               OICPAction,
                                                                                               OperatorId,
                                                                                               OperatorName),
                                                         "eRoamingPushEvseStatus",
                                                         RequestLogDelegate:   OnEVSEStatusPushRequest,
                                                         ResponseLogDelegate:  OnEVSEStatusPushResponse,
                                                         QueryTimeout:         QueryTimeout != null ? QueryTimeout.Value : this.RequestTimeout,

                                                         #region OnSuccess

                                                         OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                         #endregion

                                                         #region OnSOAPFault

                                                         OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                             SendSOAPError(timestamp, this, httpresponse.Content);

                                                             return new HTTPResponse<eRoamingAcknowledgement>(
                                                                 httpresponse,
                                                                 new eRoamingAcknowledgement(StatusCodes.SystemError),
                                                                 IsFault: true);

                                                         },

                                                         #endregion

                                                         #region OnHTTPError

                                                         OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                             SendHTTPError(timestamp, this, httpresponse);

                                                             return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                              new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                                                                                          httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                          httpresponse.HTTPBody.      ToUTF8String()),
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


                    #region Send OnEVSEStatusPushed event

                    Runtime.Stop();

                    try
                    {

                        OnEVSEStatusPushed?.Invoke(DateTime.Now,
                                                   this,
                                                   ClientId,
                                                   OICPAction,
                                                   _EVSEStatusRecords,
                                                   (UInt32) NumberOfEVSEStatusRecords,
                                                   result.Content,
                                                   Runtime.Elapsed);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOClient) + "." + nameof(OnEVSEDataPushed));
                    }

                    #endregion

                    return result;

                }

            }

            return HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success));

        }

        #endregion

        #region PushEVSEStatus(...KeyValuePairs<...>, OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEStatusRecords = null, ...)

        /// <summary>
        /// Create a new task pushing EVSE status key-value-pairs onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE identification and status key-value-pairs.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator identification to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEStatus,
                           ActionType                                          OICPAction                = ActionType.update,
                           EVSEOperator_Id                                     OperatorId                = null,
                           String                                              OperatorName              = null,
                           Func<EVSEStatusRecord, Boolean>                     IncludeEVSEStatusRecords  = null,
                           TimeSpan?                                           QueryTimeout              = null)

        {

            return await PushEVSEStatus(EVSEStatus.
                                            Select(kvp => new EVSEStatusRecord(kvp.Key, kvp.Value)).
                                            Where (IncludeEVSEStatusRecords),
                                        OICPAction,
                                        OperatorId,
                                        OperatorName,
                                        QueryTimeout);

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, ...)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAuthorizationStart>>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId             = null,
                           ChargingSession_Id  SessionId          = null,
                           ChargingProduct_Id  PartnerProductId   = null,
                           ChargingSession_Id  PartnerSessionId   = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)

        {

            #region Send OnAuthorizeStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnAuthorizeStart?.Invoke(DateTime.Now,
                                         Timestamp ?? DateTime.Now,
                                         this,
                                         ClientId,
                                         OperatorId,
                                         AuthToken,
                                         EVSEId,
                                         SessionId,
                                         PartnerProductId,
                                         PartnerSessionId,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStart));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                   TCPPort,
                                                   HTTPVirtualHost,
                                                   "/ibis/ws/eRoamingAuthorization_V2.0",
                                                   UserAgent,
                                                   _RemoteCertificateValidator,
                                                   DNSClient))
            {

                var result = await _OICPClient.Query(CPOClientXMLMethods.AuthorizeStartXML(OperatorId,
                                                                                           AuthToken,
                                                                                           EVSEId,
                                                                                           PartnerProductId,
                                                                                           SessionId,
                                                                                           PartnerSessionId),
                                                     "eRoamingAuthorizeStart",
                                                     RequestLogDelegate:   OnAuthorizeStartRequest,
                                                     ResponseLogDelegate:  OnAuthorizeStartResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAuthorizationStart.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                             new eRoamingAuthorizationStart(StatusCodes.DataError,
                                                                                                                                            httpresponse.Content.ToString()),
                                                                                                             IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         return new HTTPResponse<eRoamingAuthorizationStart>(httpresponse,
                                                                                                             new eRoamingAuthorizationStart(StatusCodes.DataError,
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

                #region Send OnAuthorizeStart event

                Runtime.Stop();

                try
                {

                    OnAuthorizeStarted?.Invoke(DateTime.Now,
                                               this,
                                               ClientId,
                                               OperatorId,
                                               AuthToken,
                                               EVSEId,
                                               SessionId,
                                               PartnerProductId,
                                               PartnerSessionId,
                                               RequestTimeout,
                                               result.Content,
                                               Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStarted));
                }

                #endregion

                return result;

            }

        }

        #endregion

        #region AuthorizeStop (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, ...)

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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAuthorizationStop>>

            AuthorizeStop(EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          EVSE_Id             EVSEId             = null,
                          ChargingSession_Id  PartnerSessionId   = null,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given EVSE operator identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),   "The given charging session identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given auth token must not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnAuthorizeStopRequest?.Invoke(DateTime.Now,
                                               Timestamp ?? DateTime.Now,
                                               this,
                                               ClientId,
                                               OperatorId,
                                               SessionId,
                                               AuthToken,
                                               EVSEId,
                                               PartnerSessionId,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthorization_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(CPOClientXMLMethods.AuthorizeStopXML(OperatorId,
                                                                                         SessionId,
                                                                                         AuthToken,
                                                                                         EVSEId,
                                                                                         PartnerSessionId),
                                                     "eRoamingAuthorizeStop",
                                                     RequestLogDelegate:   OnAuthorizeStopSOAPRequest,
                                                     ResponseLogDelegate:  OnAuthorizeStopSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAuthorizationStop.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                            new eRoamingAuthorizationStop(StatusCodes.DataError,
                                                                                                                                          httpresponse.Content.ToString()),
                                                                                                            IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         return new HTTPResponse<eRoamingAuthorizationStop>(httpresponse,
                                                                                                            new eRoamingAuthorizationStop(StatusCodes.DataError,
                                                                                                                                          httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                          httpresponse.HTTPBody.      ToUTF8String()),
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

                #region Send OnAuthorizeStopResponse event

                Runtime.Stop();

                try
                {

                    OnAuthorizeStopResponse?.Invoke(DateTime.Now,
                                                    this,
                                                    ClientId,
                                                    OperatorId,
                                                    SessionId,
                                                    AuthToken,
                                                    EVSEId,
                                                    PartnerSessionId,
                                                    RequestTimeout,
                                                    result.Content,
                                                    Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion

        #region SendChargeDetailRecord(...ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord),  "The given charge detail record must not be null!");

            #endregion

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <Authorization:eRoamingChargeDetailRecord>
            // 
            //          [...]
            // 
            //       </Authorization:eRoamingChargeDetailRecord>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Send OnSendChargeDetailRecord event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnSendChargeDetailRecordRequest?.Invoke(DateTime.Now,
                                                        Timestamp ?? DateTime.Now,
                                                        this,
                                                        ClientId,
                                                        ChargeDetailRecord,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                   TCPPort,
                                                   HTTPVirtualHost,
                                                   "/ibis/ws/eRoamingAuthorization_V2.0",
                                                   UserAgent,
                                                   _RemoteCertificateValidator,
                                                   DNSClient))
            {

                var result = await _OICPClient.Query(SOAP.Encapsulation(ChargeDetailRecord.ToXML()),
                                                     "eRoamingChargeDetailRecord",
                                                     RequestLogDelegate:   OnSendChargeDetailRecordSOAPRequest,
                                                     ResponseLogDelegate:  OnSendChargeDetailRecordSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                    #region OnSuccess

                                                    OnSuccess: XMLResponse => XMLResponse.Parse(eRoamingAcknowledgement.Parse),

                                                    #endregion

                                                    #region OnSOAPFault

                                                    OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                        DebugX.Log("e:" + httpresponse.EntirePDU);

                                                        SendSOAPError(timestamp, this, httpresponse.Content);

                                                        return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                         new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                                                                                     httpresponse.Content.ToString()),
                                                                                                         IsFault: true);

                                                    },

                                                    #endregion

                                                    #region OnHTTPError

                                                    OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                        DebugX.Log("e:" + httpresponse.EntirePDU);

                                                        SendHTTPError(timestamp, this, httpresponse);

                                                        return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
                                                                                                         new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                                                                                     httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                     httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                         IsFault: true);

                                                    },

                                                    #endregion

                                                    #region OnException

                                                    OnException: (timestamp, sender, exception) => {

                                                        DebugX.Log("e:" + exception.Message);

                                                        SendException(timestamp, sender, exception);

                                                        return null;

                                                    }

                                                    #endregion

                                                   );

                #region Send OnChargeDetailRecordSent event

                Runtime.Stop();

                try
                {

                    OnSendChargeDetailRecordResponse?.Invoke(DateTime.Now,
                                                             this,
                                                             ClientId,
                                                             ChargeDetailRecord,
                                                             RequestTimeout,
                                                             result.Content,
                                                             Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


        #region PullAuthenticationData(OperatorId, ...)

        /// <summary>
        /// Pull authentication data from the OICP server.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAuthenticationData>>

            PullAuthenticationData(EVSEOperator_Id     OperatorId,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given EVSE operator identification msut not be null!");

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnPullAuthenticationData event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnPullAuthenticationDataRequest?.Invoke(DateTime.Now,
                                                        Timestamp ?? DateTime.Now,
                                                        this,
                                                        ClientId,
                                                        OperatorId,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    TCPPort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                    UserAgent,
                                                    _RemoteCertificateValidator,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(CPOClientXMLMethods.PullAuthenticationDataXML(OperatorId),
                                                     "eRoamingPullAuthenticationData",
                                                     RequestLogDelegate:   OnPullAuthenticationDataSOAPRequest,
                                                     ResponseLogDelegate:  OnPullAuthenticationDataSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

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

                #region Send OnAuthenticationDataPulled event

                Runtime.Stop();

                try
                {

                    OnPullAuthenticationDataResponse?.Invoke(DateTime.Now,
                                                             this,
                                                             ClientId,
                                                             OperatorId,
                                                             RequestTimeout,
                                                             result.Content,
                                                             Runtime.Elapsed);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


    }

}
