/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP server API client, aka. the Hubject side of the API.
    /// </summary>
    public partial class EMPServerAPIClient : AHTTPClient//,
                                      //        IEMPServerAPIClient
    {

        #region (class) Counters

        public class Counters
        {

            public CounterValues  PullEVSEData                  { get; }
            public CounterValues  PullEVSEStatus                { get; }
            public CounterValues  PullEVSEStatusById            { get; }
            public CounterValues  PullEVSEStatusByOperatorId    { get; }
            public CounterValues  PushAuthenticationData        { get; }
            public CounterValues  RemoteReservationStart        { get; }
            public CounterValues  RemoteReservationStop         { get; }
            public CounterValues  RemoteStart                   { get; }
            public CounterValues  RemoteStop                    { get; }
            public CounterValues  GetChargeDetailRecords        { get; }

            public Counters(CounterValues? PullEVSEData                 = null,
                            CounterValues? PullEVSEStatus               = null,
                            CounterValues? PullEVSEStatusById           = null,
                            CounterValues? PullEVSEStatusByOperatorId   = null,
                            CounterValues? PushAuthenticationData       = null,
                            CounterValues? RemoteReservationStart       = null,
                            CounterValues? RemoteReservationStop        = null,
                            CounterValues? RemoteStart                  = null,
                            CounterValues? RemoteStop                   = null,
                            CounterValues? GetChargeDetailRecords       = null)
            {

                this.PullEVSEData                = PullEVSEData               ?? new CounterValues();
                this.PullEVSEStatus              = PullEVSEStatus             ?? new CounterValues();
                this.PullEVSEStatusById          = PullEVSEStatusById         ?? new CounterValues();
                this.PullEVSEStatusByOperatorId  = PullEVSEStatusByOperatorId ?? new CounterValues();
                this.PushAuthenticationData      = PushAuthenticationData     ?? new CounterValues();
                this.RemoteReservationStart      = RemoteReservationStart     ?? new CounterValues();
                this.RemoteReservationStop       = RemoteReservationStop      ?? new CounterValues();
                this.RemoteStart                 = RemoteStart                ?? new CounterValues();
                this.RemoteStop                  = RemoteStop                 ?? new CounterValues();
                this.GetChargeDetailRecords      = GetChargeDetailRecords     ?? new CounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PullEVSEData",                PullEVSEData.              ToJSON()),
                       new JProperty("PullEVSEStatus",              PullEVSEStatus.            ToJSON()),
                       new JProperty("PullEVSEStatusById",          PullEVSEStatusById.        ToJSON()),
                       new JProperty("PullEVSEStatusByOperatorId",  PullEVSEStatusByOperatorId.ToJSON()),
                       new JProperty("PushAuthenticationData",      PushAuthenticationData.    ToJSON()),
                       new JProperty("RemoteReservationStart",      RemoteReservationStart.    ToJSON()),
                       new JProperty("RemoteReservationStop",       RemoteReservationStop.     ToJSON()),
                       new JProperty("RemoteStart",                 RemoteStart.               ToJSON()),
                       new JProperty("RemoteStop",                  RemoteStop.                ToJSON()),
                       new JProperty("GetChargeDetailRecords",      GetChargeDetailRecords.    ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.Number + " EMP Server API Client";

        /// <summary>
        /// The default timeout for HTTP requests.
        /// </summary>
        public new readonly     TimeSpan  DefaultRequestTimeout       = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The default maximum number of transmission retries for HTTP request.
        /// </summary>
        public new const        UInt16    DefaultMaxNumberOfRetries   = 3;

        /// <summary>
        /// The default remote HTTP URL.
        /// </summary>
        public static readonly  URL       DefaultRemoteURL            = URL.Parse("https://service.hubject-qa.com");

        #endregion

        #region Properties

        public Counters  Counter    { get; }

        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new Logger HTTPLogger
        {
            get
            {
                return base.HTTPLogger as Logger;
            }
            set
            {
                base.HTTPLogger = value;
            }
        }

        public Newtonsoft.Json.Formatting  JSONFormat    { get; set; }

        #endregion

        #region Events

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStart request will be send.
        /// </summary>
        public event OnAuthorizeStartClientRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                 OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                OnAuthorizeStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStart request had been received.
        /// </summary>
        public event OnAuthorizeStartClientResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStop request will be send.
        /// </summary>
        public event OnAuthorizeStopClientRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAuthorizeStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStop request had been received.
        /// </summary>
        public event OnAuthorizeStopClientResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public EMPServerAPIClient(URL?                                  RemoteURL                    = null,
                                  HTTPHostname?                         VirtualHostname              = null,
                                  String?                               Description                  = null,
                                  RemoteCertificateValidationCallback?  RemoteCertificateValidator   = null,
                                  LocalCertificateSelectionCallback?    ClientCertificateSelector    = null,
                                  X509Certificate?                      ClientCert                   = null,
                                  SslProtocols?                         TLSProtocol                  = null,
                                  Boolean?                              PreferIPv4                   = null,
                                  String                                HTTPUserAgent                = DefaultHTTPUserAgent,
                                  TimeSpan?                             RequestTimeout               = null,
                                  TransmissionRetryDelayDelegate?       TransmissionRetryDelay       = null,
                                  UInt16?                               MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                                  Boolean                               DisableLogging               = false,
                                  String?                               LoggingPath                  = null,
                                  String                                LoggingContext               = Logger.DefaultContext,
                                  LogfileCreatorDelegate?               LogfileCreator               = null,
                                  DNSClient?                            DNSClient                    = null)

            : base(RemoteURL           ?? DefaultRemoteURL,
                   VirtualHostname,
                   Description,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   PreferIPv4,
                   HTTPUserAgent       ?? DefaultHTTPUserAgent,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries  ?? DefaultMaxNumberOfRetries,
                   false,
                   null,
                   DNSClient)

        {

            this.Counter     = new Counters();

            this.JSONFormat  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingPath,
                                                LoggingContext,
                                                LogfileCreator)
                                   : null;

        }

        #endregion


        #region AuthorizeStart               (Request)

        /// <summary>
        /// Send an authorize start request.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(AuthorizeStartRequest Request)

        {

            #region Initial checks

            if (Request is null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStart request must not be null!");

            //Request = _CustomAuthorizeStartRequestMapper(Request);

            if (Request is null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStart request must not be null!");


            Byte                                     TransmissionRetry   = 0;
            OICPResult<AuthorizationStartResponse>?  result              = null;

            #endregion

            #region Send OnAuthorizeStartClientRequest event

            var StartTime = Timestamp.Now;

            Counter.RemoteStart.IncRequests();

            try
            {

                if (OnAuthorizeStartRequest != null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartClientRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            try
            {

                do
                {

                    var processId = Process_Id.NewRandom;

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      false,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/authorize/start"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                       requestbuilder.Set("Process-ID", processId.ToString());
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (AuthorizationStartResponse.TryParse(Request,
                                                                        JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                        out AuthorizationStartResponse?  AuthorizeStartResponse,
                                                                        out String?                      ErrorResponse,
                                                                        HTTPResponse.Timestamp,
                                                                        HTTPResponse.EventTrackingId,
                                                                        HTTPResponse.Runtime,
                                                                        processId,
                                                                        HTTPResponse))
                                {

                                    result = OICPResult<AuthorizationStartResponse>.Success(Request,
                                                                                            AuthorizeStartResponse,
                                                                                            processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 null, // ProviderId
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList?  validationErrorList,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<AuthorizationStartResponse>.BadRequest(Request,
                                                                                                             validationErrorList,
                                                                                                             processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<AuthorizationStartResponse>.Failed(Request,
                                                                                           AuthorizationStartResponse.DataError(
                                                                                               Request,
                                                                                               statusCode!.Description,
                                                                                               statusCode!.AdditionalInfo,
                                                                                               Request.SessionId,
                                                                                               Request.CPOPartnerSessionId,
                                                                                               Request.EMPPartnerSessionId,
                                                                                               null, // ProviderId
                                                                                               HTTPResponse.Timestamp,
                                                                                               HTTPResponse.EventTrackingId,
                                                                                               HTTPResponse.Runtime,
                                                                                               processId,
                                                                                               HTTPResponse,
                                                                                               Request.CustomData
                                                                                           ),
                                                                                           processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 null, // ProviderId
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<AuthorizationStartResponse>.Failed(
                             Request,
                             AuthorizationStartResponse.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 null, // ProviderId
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 null, // ProcessId
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<AuthorizationStartResponse>.Failed(
                           Request,
                           AuthorizationStartResponse.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               null, // ProviderId
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               null, // ProcessId
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );


            #region Send OnAuthorizeStartClientResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStartResponse != null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartClientResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                    // Description,
                                                     result,
                                                     result.Runtime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
