/*
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
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP CPO Client.
    /// </summary>
    public partial class CPOClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OICP " + Version.Number + " CPO Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort     = IPPort.Parse(443);

        #endregion

        #region Properties

        /// <summary>
        /// The attached OICP CPO client (HTTP/SOAP client) logger.
        /// </summary>
        public CPOClientLogger Logger { get; }

        public RoamingNetwork RoamingNetwork { get; }

        public EVSEOperatorNameSelectorDelegate EVSEOperatorNameSelector { get; }

        #endregion

        #region Events

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnPushEVSEDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPushEVSEDataSOAPResponse;

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate  OnPushEVSEDataResponse;

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnPushEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnPushEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        #endregion

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartHandler    OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnAuthorizeStartSOAPResponse;

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartedHandler  OnAuthorizeStartResponse;

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

        #region CPOClient(ClientId, Hostname, ..., LoggingContext = EMPClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OICP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null,
                         String                               LoggingContext              = CPOClientLogger.DefaultContext,
                         Func<String, String, String>         LogFileCreator              = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger = new CPOClientLogger(this,
                                              LoggingContext,
                                              LogFileCreator);

            this.EVSEOperatorNameSelector = I18N => I18N.FirstText;

        }

        #endregion

        #region CPOClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OICP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String                               ClientId,
                         CPOClientLogger                      Logger,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Logger == null)
                throw new ArgumentNullException(nameof(Logger),    "The given mobile client logger must not be null!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger                    = Logger;
            this.EVSEOperatorNameSelector  = I18N => I18N.FirstText;

        }

        #endregion

        #endregion


        #region PushEVSEData(GroupedEVSEDataRecords, OICPAction = fullLoad, ...)

        /// <summary>
        /// Upload the given lookup of EVSE data records grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEDataRecords">A lookup of EVSE data records grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEData(ILookup<EVSEOperator, EVSEDataRecord>  GroupedEVSEDataRecords,
                         ActionType                             OICPAction         = ActionType.fullLoad,

                         DateTime?                              Timestamp          = null,
                         CancellationToken?                     CancellationToken  = null,
                         EventTracking_Id                       EventTrackingId    = null,
                         TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (GroupedEVSEDataRecords == null)
                throw new ArgumentNullException(nameof(GroupedEVSEDataRecords),  "The given lookup of EVSE data records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Get effective number of EVSE data records to upload

            var NumberOfEVSEDataRecords = GroupedEVSEDataRecords.
                                              Where (group => group.Key != null).
                                              Select(group => group.Count()).
                                              Sum   ();

            var results = new List<HTTPResponse<eRoamingAcknowledgement>>();

            var _OICPAction = OICPAction;

            #endregion

            #region Send OnPushEVSEDataRequest event

            try
            {

                OnPushEVSEDataRequest?.Invoke(DateTime.Now,
                                              Timestamp.Value,
                                              this,
                                              ClientId,
                                              EventTrackingId,
                                              OICPAction,
                                              GroupedEVSEDataRecords,
                                              (UInt32) NumberOfEVSEDataRecords,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataRequest));
            }

            #endregion


            // Multiple 'OperatorEvseData'-sets must be send each within their own requests!
            foreach (var EVSEDataRecordGroup in GroupedEVSEDataRecords.Where(group => group.Key != null))
            {

                if (EVSEDataRecordGroup.Any())
                {

                    using (var _OICPClient = new SOAPClient(Hostname,
                                                            RemotePort,
                                                            HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseData_V2.1",
                                                            RemoteCertificateValidator,
                                                            ClientCert,
                                                            UserAgent,
                                                            DNSClient))
                    {

                        var result = await _OICPClient.Query(CPOClientXMLMethods.PushEVSEDataXML(EVSEDataRecordGroup,
                                                                                                 _OICPAction,
                                                                                                 EVSEOperator_Id.Parse("+49*822"),// EVSEDataRecordGroup.Key.Id,
                                                                                                 EVSEOperatorNameSelector(EVSEDataRecordGroup.Key.Name)),
                                                             "eRoamingPushEvseData",
                                                             RequestLogDelegate:   OnPushEVSEDataSOAPRequest,
                                                             ResponseLogDelegate:  OnPushEVSEDataSOAPResponse,
                                                             CancellationToken:    CancellationToken,
                                                             EventTrackingId:      EventTrackingId,
                                                             QueryTimeout:         RequestTimeout,

                                                             #region OnSuccess

                                                             OnSuccess: XMLResponse => XMLResponse.ConvertContent(eRoamingAcknowledgement.Parse),

                                                             #endregion

                                                             #region OnSOAPFault

                                                             OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                                 SendSOAPError(timestamp, this, httpresponse.Content);

                                                                 return new HTTPResponse<eRoamingAcknowledgement>(httpresponse,
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

                                                                 return HTTPResponse<eRoamingAcknowledgement>.ExceptionThrown(new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                                                                                                          exception.Message,
                                                                                                                                                          exception.StackTrace),
                                                                                                                              Exception:  exception);

                                                             }

                                                         #endregion

                                                            );

                        results.Add(result);

                        // On any error, break early!
                        if (result.HTTPStatusCode != HTTPStatusCode.OK ||
                            result.Content        == null &&
                            result.Content.Result == false)
                            break;

                    }

                    // After first 'fullLoad' switch to 'insert'-mode...
                    if (_OICPAction == ActionType.fullLoad)
                        _OICPAction = ActionType.insert;

                }

            }


            if (results.Count == 0)
                results.Add(HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success, "Nothing to upload!")));


            #region Send OnPushEVSEDataResponse event

            try
            {

                OnPushEVSEDataResponse?.Invoke(DateTime.Now,
                                               Timestamp.Value,
                                               this,
                                               ClientId,
                                               EventTrackingId,
                                               OICPAction,
                                               GroupedEVSEDataRecords,
                                               (UInt32) NumberOfEVSEDataRecords,
                                               RequestTimeout,
                                               results.Select(result => result.Content).First(),
                                               DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion


            return results;

        }

        #endregion

        #region PushEVSEData(EVSEDataRecord,         OICPAction = insert, IncludeEVSEDataRecords = null, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
                         ActionType                     OICPAction              = ActionType.insert,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEDataRecords  = null,

                         DateTime?                      Timestamp               = null,
                         CancellationToken?             CancellationToken       = null,
                         EventTracking_Id               EventTrackingId         = null,
                         TimeSpan?                      RequestTimeout          = null)


            => await PushEVSEData(new EVSEDataRecord[] { EVSEDataRecord },
                                  OICPAction,
                                  IncludeEVSEDataRecords,

                                  Timestamp,
                                  CancellationToken,
                                  EventTrackingId,
                                  RequestTimeout);

        #endregion

        #region PushEVSEData(EVSEDataRecords,        OICPAction = fullLoad, IncludeEVSEDataRecords = null, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE data records.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction              = ActionType.fullLoad,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEDataRecords  = null,

                         DateTime?                      Timestamp               = null,
                         CancellationToken?             CancellationToken       = null,
                         EventTracking_Id               EventTrackingId         = null,
                         TimeSpan?                      RequestTimeout          = null)

        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException(nameof(EVSEDataRecords),  "The given enumeration of EVSE data records must not be null!");

            if (IncludeEVSEDataRecords == null)
                IncludeEVSEDataRecords = EVSEDataRecord => true;

            var _EVSEDataRecords = EVSEDataRecords.
                                       Where(IncludeEVSEDataRecords).
                                       ToArray();

            #endregion

            if (_EVSEDataRecords.Length > 0)
                return await PushEVSEData(_EVSEDataRecords.ToLookup(evsedatarecord => evsedatarecord.EVSE.Operator),
                                          OICPAction,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);


            return new HTTPResponse<eRoamingAcknowledgement>[] {
                       HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success))
                   };

        }

        #endregion

        #region PushEVSEData(OICPAction, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEData(ActionType               OICPAction,
                         params EVSEDataRecord[]  EVSEDataRecords)


            => await PushEVSEData(EVSEDataRecords,
                                  OICPAction);

        #endregion


        #region PushEVSEStatus(GroupedEVSEStatusRecords,  OICPAction = update, ...)

        /// <summary>
        /// Upload the given lookup of EVSE status records grouped by their EVSE operator identification.
        /// </summary>
        /// <param name="GroupedEVSEStatusRecords">A lookup of EVSE status records grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEStatus(ILookup<EVSEOperator, EVSEStatusRecord>  GroupedEVSEStatusRecords,
                           ActionType                               OICPAction         = ActionType.update,

                           DateTime?                                Timestamp          = null,
                           CancellationToken?                       CancellationToken  = null,
                           EventTracking_Id                         EventTrackingId    = null,
                           TimeSpan?                                RequestTimeout     = null)

        {

            #region Initial checks

            if (GroupedEVSEStatusRecords == null)
                throw new ArgumentNullException(nameof(GroupedEVSEStatusRecords),  "The given lookup of EVSE status records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Get effective number of EVSE data records to upload

            var NumberOfEVSEStatusRecords = GroupedEVSEStatusRecords.
                                                Where (group => group.Key != null).
                                                Select(group => group.Count()).
                                                Sum();

            var results = new List<HTTPResponse<eRoamingAcknowledgement>>();

            var _OICPAction = OICPAction;

            #endregion

            #region Send OnPushEVSEStatusRequest event

            try
            {

                OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
                                                Timestamp.Value,
                                                this,
                                                ClientId,
                                                EventTrackingId,
                                                OICPAction,
                                                GroupedEVSEStatusRecords,
                                                (UInt32) NumberOfEVSEStatusRecords,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            // Multiple 'OperatorEvseStatus'-sets must be send each within their own requests!
            foreach (var EVSEStatusRecordGroup in GroupedEVSEStatusRecords.Where(group => group.Key != null))
            {

                if (EVSEStatusRecordGroup.Any())
                {

                    using (var _OICPClient = new SOAPClient(Hostname,
                                                            RemotePort,
                                                            HTTPVirtualHost,
                                                            "/ibis/ws/eRoamingEvseStatus_V2.0",
                                                            RemoteCertificateValidator,
                                                            ClientCert,
                                                            UserAgent,
                                                            DNSClient))
                    {

                         var result = await _OICPClient.Query(CPOClientXMLMethods.PushEVSEStatusXML(EVSEStatusRecordGroup,
                                                                                                    _OICPAction,
                                                                                                    EVSEOperator_Id.Parse("+49*822"),// EVSEStatusRecordGroup.Key.Id,
                                                                                                    EVSEOperatorNameSelector(EVSEStatusRecordGroup.Key.Name)),
                                                              "eRoamingPushEvseStatus",
                                                              RequestLogDelegate:   OnPushEVSEStatusSOAPRequest,
                                                              ResponseLogDelegate:  OnPushEVSEStatusSOAPResponse,
                                                              CancellationToken:    CancellationToken,
                                                              EventTrackingId:      EventTrackingId,
                                                              QueryTimeout:         RequestTimeout,

                                                              #region OnSuccess

                                                              OnSuccess: XMLResponse => XMLResponse.ConvertContent(eRoamingAcknowledgement.Parse),

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

                                                                  return HTTPResponse<eRoamingAcknowledgement>.ExceptionThrown(new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                                                                                                           exception.Message,
                                                                                                                                                           exception.StackTrace),
                                                                                                                               Exception: exception);

                                                              }

                                                              #endregion

                                                             );

                        results.Add(result);

                        // On any error, break early!
                        if (result.HTTPStatusCode != HTTPStatusCode.OK ||
                            result.Content        == null &&
                            result.Content.Result == false)
                            break;

                    }

                    // After first 'fullLoad' switch to 'insert'-mode...
                    if (_OICPAction == ActionType.fullLoad)
                        _OICPAction = ActionType.insert;

                }

            }


            if (results.Count == 0)
                results.Add(HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success, "Nothing to upload!")));


            #region Send OnPushEVSEDataResponse event

            try
            {

                OnPushEVSEStatusResponse?.Invoke(DateTime.Now,
                                                 Timestamp.Value,
                                                 this,
                                                 ClientId,
                                                 EventTrackingId,
                                                 OICPAction,
                                                 GroupedEVSEStatusRecords,
                                                 (UInt32) NumberOfEVSEStatusRecords,
                                                 RequestTimeout,
                                                 results.Select(result => result.Content).First(),
                                                 DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion

            return results;

        }

        #endregion

        #region PushEVSEStatus(EVSEStatusRecord,          OICPAction = insert, IncludeEVSEStatusRecords = null, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE status record onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatusRecord">An EVSE status record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(EVSEStatusRecord                  EVSEStatusRecord,
                           ActionType                        OICPAction                = ActionType.insert,
                           IncludeEVSEStatusRecordsDelegate  IncludeEVSEStatusRecords  = null,

                           DateTime?                         Timestamp                 = null,
                           CancellationToken?                CancellationToken         = null,
                           EventTracking_Id                  EventTrackingId           = null,
                           TimeSpan?                         RequestTimeout            = null)


            => (await PushEVSEStatus(new EVSEStatusRecord[] { EVSEStatusRecord },
                                     OICPAction,
                                     IncludeEVSEStatusRecords,

                                     Timestamp,
                                     CancellationToken,
                                     EventTrackingId,
                                     RequestTimeout)).FirstOrDefault();

        #endregion

        #region PushEVSEStatus(EVSEStatusRecords,         OICPAction = update, IncludeEVSEStatusRecords = null, ...)

        /// <summary>
        /// Create a new task pushing EVSE status key-value-pairs onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE identification and status key-value-pairs.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEStatus(IEnumerable<EVSEStatusRecord>     EVSEStatusRecords,
                           ActionType                        OICPAction                = ActionType.update,
                           IncludeEVSEStatusRecordsDelegate  IncludeEVSEStatusRecords  = null,

                           DateTime?                         Timestamp                 = null,
                           CancellationToken?                CancellationToken         = null,
                           EventTracking_Id                  EventTrackingId           = null,
                           TimeSpan?                         RequestTimeout            = null)

        {

            #region Initial checks

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException(nameof(EVSEStatusRecords), "The given enumeration of EVSE status records must not be null!");

            var _EVSEStatusRecords = IncludeEVSEStatusRecords != null
                                         ? EVSEStatusRecords.
                                               Where(evsestatus => IncludeEVSEStatusRecords(evsestatus)).
                                               ToArray()
                                         : EVSEStatusRecords.
                                               ToArray();

            #endregion

            if (_EVSEStatusRecords.Length > 0)
                return await PushEVSEStatus(_EVSEStatusRecords.ToLookup(evsestatusrecord => RoamingNetwork.GetEVSEOperatorbyId(evsestatusrecord.Id.OperatorId)),
                                            OICPAction,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

            return new HTTPResponse<eRoamingAcknowledgement>[] {
                       HTTPResponse<eRoamingAcknowledgement>.OK(new eRoamingAcknowledgement(StatusCodes.Success, "Nothing to upload!"))
                   };

        }

        #endregion

        #region PushEVSEStatus(OICPAction, params EVSEStatusRecords)

        /// <summary>
        /// Create a new task pushing EVSE status records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEStatusRecords">An array of EVSE status records.</param>
        public async Task<IEnumerable<HTTPResponse<eRoamingAcknowledgement>>>

            PushEVSEStatus(ActionType                 OICPAction,
                           params EVSEStatusRecord[]  EVSEStatusRecords)


            => await PushEVSEStatus(EVSEStatusRecords,
                                    OICPAction);

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

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given EVSE operator identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException(nameof(AuthToken),   "The given auth token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            try
            {

                OnAuthorizeStartRequest?.Invoke(DateTime.Now,
                                                Timestamp.Value,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion

            #region Verify length of AuthToken

            HTTPResponse<eRoamingAuthorizationStart> result = null;

            if (AuthToken.Length !=  8 &&
                AuthToken.Length != 14 &&
                AuthToken.Length != 20)
            {

                result = new HTTPResponse<eRoamingAuthorizationStart>(HTTPResponse.BadRequest,
                                                                      new eRoamingAuthorizationStart(StatusCodes.DataError,
                                                                                                     "OICP " + Version.Number + " only allows a 8, 14 or 20 hex-character authentication token!"),
                                                                      IsFault: true);

            }

            #endregion


            if (result == null)
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        RemotePort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        RemoteCertificateValidator,
                                                        ClientCert,
                                                        UserAgent,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(CPOClientXMLMethods.AuthorizeStartXML(OperatorId,
                                                                                           AuthToken,
                                                                                           EVSEId,
                                                                                           PartnerProductId,
                                                                                           SessionId,
                                                                                           PartnerSessionId),
                                                     "eRoamingAuthorizeStart",
                                                     RequestLogDelegate:   OnAuthorizeStartSOAPRequest,
                                                     ResponseLogDelegate:  OnAuthorizeStartSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(eRoamingAuthorizationStart.Parse),

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

                                                         return HTTPResponse<eRoamingAuthorizationStart>.ExceptionThrown(new eRoamingAuthorizationStart(StatusCodes.SystemError,
                                                                                                                                                        exception.Message,
                                                                                                                                                        exception.StackTrace),
                                                                                                                         Exception: exception);

                                                     }

                                                     #endregion

                                                    );

                }

            }

            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(DateTime.Now,
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
                                                 DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion


            return result;

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


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            try
            {

                OnAuthorizeStopRequest?.Invoke(DateTime.Now,
                                               Timestamp.Value,
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

            #region Verify length of AuthToken

            HTTPResponse<eRoamingAuthorizationStop> result = null;

            if (AuthToken.Length !=  8 &&
                AuthToken.Length != 14 &&
                AuthToken.Length != 20)
            {

                result = new HTTPResponse<eRoamingAuthorizationStop>(HTTPResponse.BadRequest,
                                                                     new eRoamingAuthorizationStop(StatusCodes.DataError,
                                                                                                   "OICP " + Version.Number + " only allows a 8, 14 or 20 hex-character authentication token!"),
                                                                     IsFault: true);

            }

            #endregion


            if (result != null)
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        RemotePort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        RemoteCertificateValidator,
                                                        ClientCert,
                                                        UserAgent,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(CPOClientXMLMethods.AuthorizeStopXML(OperatorId,
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

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(eRoamingAuthorizationStop.Parse),

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

                                                         return HTTPResponse<eRoamingAuthorizationStop>.ExceptionThrown(new eRoamingAuthorizationStop(StatusCodes.SystemError,
                                                                                                                                                      exception.Message,
                                                                                                                                                      exception.StackTrace),
                                                                                                                        Exception: exception);

                                                     }

                                                     #endregion

                                                    );

                }

            }

            #region Send OnAuthorizeStopResponse event

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
                                                DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion


            return result;

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


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

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

            try
            {

                OnSendChargeDetailRecordRequest?.Invoke(DateTime.Now,
                                                        Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        EventTrackingId,
                                                        ChargeDetailRecord,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion

            #region Verify length of AuthToken, if present...

            HTTPResponse<eRoamingAcknowledgement> result = null;

            if (ChargeDetailRecord?.Identification?.AuthToken         != null &&
                ChargeDetailRecord?.Identification?.AuthToken.Length  !=    8 &&
                ChargeDetailRecord?.Identification?.AuthToken.Length  !=   14 &&
                ChargeDetailRecord?.Identification?.AuthToken.Length  !=   20)
            {

                result = new HTTPResponse<eRoamingAcknowledgement>(HTTPResponse.BadRequest,
                                                                   new eRoamingAcknowledgement(StatusCodes.DataError,
                                                                                               "OICP " + Version.Number + " only allows a 8, 14 or 20 hex-character authentication token!"),
                                                                   IsFault: true);

            }

            #endregion


            if (result != null)
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        RemotePort,
                                                        HTTPVirtualHost,
                                                        "/ibis/ws/eRoamingAuthorization_V2.0",
                                                        RemoteCertificateValidator,
                                                        ClientCert,
                                                        UserAgent,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(SOAP.Encapsulation(ChargeDetailRecord.ToXML()),
                                                     "eRoamingChargeDetailRecord",
                                                     RequestLogDelegate:   OnSendChargeDetailRecordSOAPRequest,
                                                     ResponseLogDelegate:  OnSendChargeDetailRecordSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(eRoamingAcknowledgement.Parse),

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

                                                         return HTTPResponse<eRoamingAcknowledgement>.ExceptionThrown(new eRoamingAcknowledgement(StatusCodes.ServiceNotAvailable,
                                                                                                                                                  exception.Message,
                                                                                                                                                  exception.StackTrace,
                                                                                                                                                  ChargeDetailRecord.SessionId),
                                                                                                                      Exception:  exception);

                                                     }

                                                     #endregion

                                                    );

                }

            }

            #region Send OnChargeDetailRecordSent event

            try
            {

                OnSendChargeDetailRecordResponse?.Invoke(DateTime.Now,
                                                         Timestamp.Value,
                                                         this,
                                                         ClientId,
                                                         EventTrackingId,
                                                         ChargeDetailRecord,
                                                         RequestTimeout,
                                                         result.Content,
                                                         DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordResponse));
            }

            #endregion


            return result;

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


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnPullAuthenticationData event

            try
            {

                OnPullAuthenticationDataRequest?.Invoke(DateTime.Now,
                                                        Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        EventTrackingId,
                                                        OperatorId,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataRequest));
            }

            #endregion

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/ibis/ws/eRoamingAuthenticationData_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
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

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(eRoamingAuthenticationData.Parse),

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

                                                         return HTTPResponse<eRoamingAuthenticationData>.ExceptionThrown(new eRoamingAuthenticationData(StatusCodes.SystemError,
                                                                                                                                                        exception.Message,
                                                                                                                                                        exception.StackTrace),
                                                                                                                         Exception: exception);

                                                     }

                                                     #endregion

                                                    );

                #region Send OnAuthenticationDataPulled event

                try
                {

                    OnPullAuthenticationDataResponse?.Invoke(DateTime.Now,
                                                             this,
                                                             ClientId,
                                                             EventTrackingId,
                                                             OperatorId,
                                                             RequestTimeout,
                                                             result.Content,
                                                             DateTime.Now - Timestamp.Value);

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
