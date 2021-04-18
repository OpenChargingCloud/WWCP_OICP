/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The OICP EMP client.
    /// </summary>
    public partial class EMPClient : AHTTPClient,
                                     IEMPClient
    {

        public class EMPCounters
        {

            public CounterValues GetTokens  { get; }
            public CounterValues PostTokens { get; }

            public EMPCounters(CounterValues? GetTokens  = null,
                               CounterValues? PostTokens = null)
            {

                this.GetTokens  = GetTokens  ?? new CounterValues();
                this.PostTokens = PostTokens ?? new CounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("GetTokens",  GetTokens. ToJSON()),
                       new JProperty("PostTokens", PostTokens.ToJSON())
                   );

        }


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.Number + " EMP Client";

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

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate   OnPullEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnPullEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPullEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate  OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnPullEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnPullEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate  OnPullEVSEStatusResponse;

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
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public EMPClient(URL?                                 RemoteURL                    = null,
                         HTTPHostname?                        VirtualHostname              = null,
                         String                               Description                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         X509Certificate                      ClientCert                   = null,
                         String                               HTTPUserAgent                = null,
                         TimeSpan?                            RequestTimeout               = null,
                         TransmissionRetryDelayDelegate       TransmissionRetryDelay       = null,
                         Byte?                                MaxNumberOfRetries           = null,
                         Boolean                              DisableLogging               = false,
                         String                               LoggingContext               = null,
                         LogfileCreatorDelegate               LogfileCreator               = null,
                         DNSClient                            DNSClient                    = null)

            : base(RemoteURL           ?? DefaultRemoteURL,
                   VirtualHostname,
                   Description,
                   RemoteCertificateValidator,
                   null,
                   ClientCert,
                   HTTPUserAgent       ?? DefaultHTTPUserAgent,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries  ?? DefaultMaxNumberOfRetries,
                   false,
                   null,
                   DNSClient)

        {

            this.JSONFormat  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingContext,
                                                LogfileCreator)
                                   : null;

        }

        #endregion

        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(EMPClient));


        #region PullEVSEData  (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        public async Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(PullEVSEDataRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PullEVSEData request must not be null!");

            //Request = _CustomPullEVSEDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PullEVSEData request must not be null!");


            Byte                              TransmissionRetry   = 0;
            OICPResult<PullEVSEDataResponse>  result              = null;

            #endregion

            #region Send OnPullEVSEDataRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnPullEVSEDataRequest != null)
            //        await Task.WhenAll(OnPullEVSEDataRequest.GetInvocationList().
            //                           Cast<OnPullEVSEDataRequestDelegate>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         //ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.Action,
            //                                         Request.EVSEDataRecords.ULongCount(),
            //                                         Request.EVSEDataRecords,
            //                                         Request.RequestTimeout ?? RequestTimeout))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEDataRequest));
            //}

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await new HTTPSClient(RemoteURL,
                                                             VirtualHostname,
                                                             Description,
                                                             RemoteCertificateValidator,
                                                             null,
                                                             ClientCert,
                                                             HTTPUserAgent,
                                                             RequestTimeout,
                                                             TransmissionRetryDelay,
                                                             MaxNumberOfRetries,
                                                             false,
                                                             null,
                                                             DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepull/v23/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/data-records"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEDataHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEDataHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if      (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEDataResponse.TryParse(Request,
                                                                  JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                  HTTPResponse.Timestamp,
                                                                  HTTPResponse.EventTrackingId,
                                                                  HTTPResponse.Runtime,
                                                                  out PullEVSEDataResponse  pullEVSEDataResponse,
                                                                  out String                ErrorResponse,
                                                                  processId,
                                                                  HTTPResponse))
                                {

                                    result = OICPResult<PullEVSEDataResponse>.Success(Request,
                                                                                      pullEVSEDataResponse,
                                                                                      processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEDataResponse>.Failed(
                                             Request,
                                             new PullEVSEDataResponse(
                                                 Request,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new OperatorEVSEData[0],
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 processId
                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400
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
                                                             out ValidationErrorList ValidationErrors))
                            {

                                result = OICPResult<PullEVSEDataResponse>.BadRequest(Request,
                                                                                     ValidationErrors,
                                                                                     processId);

                            }

                        }

                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {
                        // Hubject firewall problem!
                        // Only HTML response!
                        break;
                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401
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
                                                        out StatusCode  statusCode,
                                                        out String      ErrorResponse))
                                {

                                    result = OICPResult<PullEVSEDataResponse>.Failed(Request,
                                                                                     new PullEVSEDataResponse(
                                                                                         Request,
                                                                                         HTTPResponse.Timestamp,
                                                                                         HTTPResponse.EventTrackingId,
                                                                                         HTTPResponse.Runtime,
                                                                                         new OperatorEVSEData[0],
                                                                                         statusCode,
                                                                                         processId
                                                                                     ),
                                                                                     processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEDataResponse>.Failed(
                                             Request,
                                             new PullEVSEDataResponse(
                                                 Request,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new OperatorEVSEData[0],
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 processId
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<PullEVSEDataResponse>.Failed(
                             Request,
                             new PullEVSEDataResponse(
                                 Request,
                                 DateTime.UtcNow,
                                 Request.EventTrackingId,
                                 DateTime.UtcNow - Request.Timestamp,
                                 new OperatorEVSEData[0],
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            if (result == null)
                result = OICPResult<PullEVSEDataResponse>.Failed(
                             Request,
                             new PullEVSEDataResponse(
                                 Request,
                                 DateTime.UtcNow,
                                 Request.EventTrackingId,
                                 DateTime.UtcNow - Request.Timestamp,
                                 new OperatorEVSEData[0],
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             )
                         );


            #region Send OnPullEVSEDataResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnPullEVSEDataResponse != null)
            //        await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
            //                           Cast<OnPullEVSEDataResponseDelegate>().
            //                           Select(e => e(Endtime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         //ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.Action,
            //                                         Request.EVSEDataRecords.ULongCount(),
            //                                         Request.EVSEDataRecords,
            //                                         Request.RequestTimeout ?? RequestTimeout,
            //                                         result,
            //                                         Endtime - StartTime))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEDataResponse));
            //}

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatus(Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public async Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(PullEVSEStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PullEVSEStatus request must not be null!");

            //Request = _CustomPullEVSEStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PullEVSEStatus request must not be null!");


            Byte                                TransmissionRetry   = 0;
            OICPResult<PullEVSEStatusResponse>  result              = null;

            #endregion

            #region Send OnPullEVSEStatusRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnPullEVSEStatusRequest != null)
            //        await Task.WhenAll(OnPullEVSEStatusRequest.GetInvocationList().
            //                           Cast<OnPullEVSEStatusRequestDelegate>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         //ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.Action,
            //                                         Request.EVSEStatusRecords.ULongCount(),
            //                                         Request.EVSEStatusRecords,
            //                                         Request.RequestTimeout ?? RequestTimeout))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusRequest));
            //}

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await new HTTPSClient(RemoteURL,
                                                             VirtualHostname,
                                                             Description,
                                                             RemoteCertificateValidator,
                                                             null,
                                                             ClientCert,
                                                             HTTPUserAgent,
                                                             RequestTimeout,
                                                             TransmissionRetryDelay,
                                                             MaxNumberOfRetries,
                                                             false,
                                                             null,
                                                             DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepull/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/status-records"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEStatusHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEStatusHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if      (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEStatusResponse.TryParse(Request,
                                                                    JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                    HTTPResponse.Timestamp,
                                                                    HTTPResponse.EventTrackingId,
                                                                    HTTPResponse.Runtime,
                                                                    out PullEVSEStatusResponse  pullEVSEStatusResponse,
                                                                    out String                  ErrorResponse,
                                                                    processId,
                                                                    HTTPResponse))
                                {

                                    result = OICPResult<PullEVSEStatusResponse>.Success(Request,
                                                                                        pullEVSEStatusResponse,
                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusResponse(
                                                 Request,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new OperatorEVSEStatus[0],
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 processId
                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400
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
                                                             out ValidationErrorList ValidationErrors))
                            {

                                result = OICPResult<PullEVSEStatusResponse>.BadRequest(Request,
                                                                                       ValidationErrors,
                                                                                       processId);

                            }

                        }

                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401
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
                                                        out StatusCode  statusCode,
                                                        out String      ErrorResponse))
                                {

                                    result = OICPResult<PullEVSEStatusResponse>.Failed(Request,
                                                                                       new PullEVSEStatusResponse(
                                                                                           Request,
                                                                                           HTTPResponse.Timestamp,
                                                                                           HTTPResponse.EventTrackingId,
                                                                                           HTTPResponse.Runtime,
                                                                                           new OperatorEVSEStatus[0],
                                                                                           statusCode,
                                                                                           processId
                                                                                       ),
                                                                                       processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusResponse(
                                                 Request,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new OperatorEVSEStatus[0],
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 processId
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<PullEVSEStatusResponse>.Failed(
                             Request,
                             new PullEVSEStatusResponse(
                                 Request,
                                 DateTime.UtcNow,
                                 Request.EventTrackingId,
                                 DateTime.UtcNow - Request.Timestamp,
                                 new OperatorEVSEStatus[0],
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            if (result == null)
                result = OICPResult<PullEVSEStatusResponse>.Failed(
                             Request,
                             new PullEVSEStatusResponse(
                                 Request,
                                 DateTime.UtcNow,
                                 Request.EventTrackingId,
                                 DateTime.UtcNow - Request.Timestamp,
                                 new OperatorEVSEStatus[0],
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             )
                         );


            #region Send OnPullEVSEStatusResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnPullEVSEStatusResponse != null)
            //        await Task.WhenAll(OnPullEVSEStatusResponse.GetInvocationList().
            //                           Cast<OnPullEVSEStatusResponseDelegate>().
            //                           Select(e => e(Endtime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         //ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.Action,
            //                                         Request.EVSEStatusRecords.ULongCount(),
            //                                         Request.EVSEStatusRecords,
            //                                         Request.RequestTimeout ?? RequestTimeout,
            //                                         result,
            //                                         Endtime - StartTime))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusResponse));
            //}

            #endregion

            return result;

        }

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        { }

        #endregion

    }

}
