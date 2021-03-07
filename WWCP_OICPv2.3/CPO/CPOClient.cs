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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO client.
    /// </summary>
    public partial class CPOClient
    {

        public class CPOCounters
        {

            public CounterValues GetTokens  { get; }
            public CounterValues PostTokens { get; }

            public CPOCounters(CounterValues? GetTokens  = null,
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

        private String DefaultURL = "/api/oicp/evsepush/v23/operators/{operatorID}/data-records";

        #endregion

        #region Properties

        public URL                                  RemoteURL                     { get; }

        public RemoteCertificateValidationCallback  RemoteCertificateValidator    { get; }

        public X509Certificate                      ClientCert                    { get; }

        public TimeSpan                             RequestTimeout                { get; }

        public DNSClient                            DNSClient                     { get; }

        public Byte                                 MaxNumberOfRetries            { get; }


        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        public Logger                               HTTPLogger                    { get; }



        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEDataRequest>>            CustomPushEVSEDataAcknowledgementParser              { get; set; }
        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEStatusRequest>>          CustomPushEVSEStatusAcknowledgementParser            { get; set; }


        public CustomJObjectParserDelegate<AuthorizationStartResponse>                      CustomAuthorizationStartResponseParser               { get; set; }
        public CustomJObjectParserDelegate<AuthorizationStopResponse>                       CustomAuthorizationStopResponseParser                { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<SendChargeDetailRecordRequest>>  CustomSendChargeDetailRecordAcknowledgementParser    { get; set; }

        #endregion

        #region Events

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a HTTP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnPushEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPushEVSEDataHTTPResponse;

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
        /// An event fired whenever a HTTP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnPushEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnPushEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestHandler     OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler            OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize start HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler           OnAuthorizeStartHTTPResponse;

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartResponseHandler    OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize stop' request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestHandler   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize stop' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnAuthorizeStopHTTPResponse;

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
        /// An event fired whenever a 'charge detail record' will be send via HTTP.
        /// </summary>
        public event ClientRequestLogHandler                  OnSendChargeDetailRecordHTTPRequest;

        /// <summary>
        /// An event fired whenever a HTTP response to a sent 'charge detail record' had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSendChargeDetailRecordHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a sent 'charge detail record' had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler  OnSendChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMSP client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the endpoint to connect to.</param>
        /// <param name="Description">An optional description of this client.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="RemoteCertificateValidator">An optional remote SSL/TLS certificate validator.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(URL?                                 RemoteURL                    = null,
                         String                               Description                  = null,
                         HTTPHostname?                        VirtualHostname              = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         X509Certificate                      ClientCert                   = null,
                         TimeSpan?                            RequestTimeout               = null,
                         Byte?                                MaxNumberOfRetries           = null,
                         DNSClient                            DNSClient                    = null)

        {

            this.HTTPLogger                  = new Logger(this);

            this.RemoteURL                   = URL.Parse("https://service.hubject-qa.com");
            this.RemoteCertificateValidator  = (sender, certificate, chain, policyErrors) => true;
            this.ClientCert                  = ClientCert;

        }

        #endregion

        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(CPOClient));


        #region PushEVSEData          (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>> PushEVSEData(PushEVSEDataRequest Request)
        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PushEVSEData request must not be null!");

            //Request = _CustomPushEVSEDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PushEVSEData request must not be null!");


            Byte                                             TransmissionRetry  = 0;
            OICPResult<Acknowledgement<PushEVSEDataRequest>> result             = null;

            #endregion

            #region Send OnPushEVSEDataRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEDataRequest != null)
                    await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                       Cast<OnPushEVSEDataRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp,
                                                     this,
                                                     //ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEDataRecords.ULongCount(),
                                                     Request.EVSEDataRecords,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.EVSEDataRecords.Any())
            {

                result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(
                             Request,
                             Acknowledgement<PushEVSEDataRequest>.Success(Request,
                                                                          StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else
            {

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await (RemoteURL.Protocol == HTTPProtocols.http

                                                      ? new HTTPClient (RemoteURL.Hostname,
                                                                        RemotePort:  RemoteURL.Port ?? IPPort.HTTP,
                                                                        DNSClient:   DNSClient)

                                                      : new HTTPSClient(RemoteURL.Hostname,
                                                                        (sender, certificate, chain, policyErrors) => {
                                                                            return true;
                                                                        },
                                                                        (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => {
                                                                            return ClientCert;
                                                                        },
                                                                        ClientCert:  ClientCert,
                                                                        RemotePort:  RemoteURL.Port ?? IPPort.HTTPS,
                                                                        DNSClient:   DNSClient)).

                                                  Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                         RemoteURL.Path + ("/api/oicp/evsepush/v23/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/data-records"),
                                                                                         requestbuilder => {
                                                                                             requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                             requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                             requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                         }),

                                                          RequestLogDelegate:   OnPushEVSEDataHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEDataHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? this.RequestTimeout).

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

                                    // HTTP/1.1 200
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // 
                                    // {
                                    //     "Result":               true,
                                    //     "StatusCode": {
                                    //         "Code":             "000",
                                    //         "Description":      null,
                                    //         "AdditionalInfo":   null
                                    //     },
                                    //     "SessionID":            null,
                                    //     "CPOPartnerSessionID":  null,
                                    //     "EMPPartnerSessionID":  null
                                    // }

                                    if (Acknowledgement<PushEVSEDataRequest>.TryParse(Request,
                                                                                      JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                      out Acknowledgement<PushEVSEDataRequest>  acknowledgement,
                                                                                      out String                                ErrorResponse,
                                                                                      HTTPResponse.Timestamp,
                                                                                      HTTPResponse.EventTrackingId,
                                                                                      HTTPResponse.Runtime,
                                                                                      processId,
                                                                                      CustomPushEVSEDataAcknowledgementParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(Request,
                                                                                                          acknowledgement,
                                                                                                          processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     Request,
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     false,
                                                     ProcessId: processId
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

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.BadRequest(Request,
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

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(Request,
                                                                                                         new Acknowledgement<PushEVSEDataRequest>(
                                                                                                             Request,
                                                                                                             HTTPResponse.Timestamp,
                                                                                                             HTTPResponse.EventTrackingId,
                                                                                                             HTTPResponse.Runtime,
                                                                                                             statusCode,
                                                                                                             ProcessId: processId
                                                                                                         ),
                                                                                                         processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     Request,
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     false,
                                                     ProcessId: processId
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404
                            // Server: nginx/1.18.0 (Ubuntu)
                            // Date: Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type: application/json;charset=UTF-8
                            // Content-Length: 85
                            // Connection: keep-alive
                            // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                            // 
                            // {
                            //     "StatusCode": {
                            //         "Code":            "300",
                            //         "Description":     "Partner not found",
                            //         "AdditionalInfo":   null
                            //     }
                            // }

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                            out StatusCode  statusCode,
                                                            out String      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(Request,
                                                                                                         new Acknowledgement<PushEVSEDataRequest>(
                                                                                                             Request,
                                                                                                             HTTPResponse.Timestamp,
                                                                                                             HTTPResponse.EventTrackingId,
                                                                                                             HTTPResponse.Runtime,
                                                                                                             statusCode,
                                                                                                             ProcessId: processId
                                                                                                         ),
                                                                                                         processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     Request,
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     false,
                                                     ProcessId: processId
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

                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEDataRequest>(
                                     Request,
                                     DateTime.UtcNow,
                                     Request.EventTrackingId,
                                     DateTime.UtcNow - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     false
                                 )
                             );

                }

                if (result == null)
                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEDataRequest>(
                                     Request,
                                     DateTime.UtcNow,
                                     Request.EventTrackingId,
                                     DateTime.UtcNow - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         "HTTP request failed!"
                                     ),
                                     false
                                 )
                             );

            }


            #region Send OnPushEVSEDataResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEDataResponse != null)
                    await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                       Cast<OnPushEVSEDataResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp,
                                                     this,
                                                     //ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEDataRecords.ULongCount(),
                                                     Request.EVSEDataRecords,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEStatus        (Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(PushEVSEStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given PushEVSEStatus request must not be null!");

            //Request = _CustomPushEVSEStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped PushEVSEStatus request must not be null!");


            Byte                                               TransmissionRetry  = 0;
            OICPResult<Acknowledgement<PushEVSEStatusRequest>> result             = null;

            #endregion

            #region Send OnPushEVSEStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEStatusRequest != null)
                    await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPushEVSEStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp,
                                                     this,
                                                     //ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEStatusRecords.ULongCount(),
                                                     Request.EVSEStatusRecords,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE status to push?

            if (!Request.EVSEStatusRecords.Any())
            {

                result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(
                             Request,
                             Acknowledgement<PushEVSEStatusRequest>.Success(Request,
                                                                            StatusCodeDescription: "No EVSE status to push")
                         );

            }

            #endregion

            else
            {

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await (RemoteURL.Protocol == HTTPProtocols.http

                                                      ? new HTTPClient (RemoteURL.Hostname,
                                                                        RemotePort:  RemoteURL.Port ?? IPPort.HTTP,
                                                                        DNSClient:   DNSClient)

                                                      : new HTTPSClient(RemoteURL.Hostname,
                                                                        (sender, certificate, chain, policyErrors) => {
                                                                            return true;
                                                                        },
                                                                        (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => {
                                                                            return ClientCert;
                                                                        },
                                                                        ClientCert:  ClientCert,
                                                                        RemotePort:  RemoteURL.Port ?? IPPort.HTTPS,
                                                                        DNSClient:   DNSClient)).

                                                  Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                         RemoteURL.Path + ("/api/oicp/evsepush/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/status-records"),
                                                                                         requestbuilder => {
                                                                                             requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                             requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                             requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                         }),

                                                          RequestLogDelegate:   OnPushEVSEStatusHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEStatusHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? this.RequestTimeout).

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

                                    // HTTP/1.1 200
                                    // Server:             nginx/1.18.0 (Ubuntu)
                                    // Date:               Tue, 02 Mar 2021 17:51:14 GMT
                                    // Content-Type:       application/json;charset=utf-8
                                    // Transfer-Encoding:  chunked
                                    // Connection:         keep-alive
                                    // Process-ID:         332c9d01-2ea4-4d15-9d4a-bb9f5abd097c
                                    // 
                                    // {
                                    //     "Result":               true,
                                    //     "StatusCode": {
                                    //         "Code":             "000",
                                    //         "Description":      null,
                                    //         "AdditionalInfo":   null
                                    //     },
                                    //     "SessionID":            null,
                                    //     "CPOPartnerSessionID":  null,
                                    //     "EMPPartnerSessionID":  null
                                    // }

                                    if (Acknowledgement<PushEVSEStatusRequest>.TryParse(Request,
                                                                                        JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                        out Acknowledgement<PushEVSEStatusRequest>  acknowledgement,
                                                                                        out String                                  ErrorResponse,
                                                                                        HTTPResponse.Timestamp,
                                                                                        HTTPResponse.EventTrackingId,
                                                                                        HTTPResponse.Runtime,
                                                                                        processId,
                                                                                        CustomPushEVSEStatusAcknowledgementParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(Request,
                                                                                                            acknowledgement,
                                                                                                            processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     Request,
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     false,
                                                     ProcessId: processId
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
                                //     "message": "Error parsing/validating JSON.",
                                //     "validationErrors": [
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                                //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                                //         },
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                                //             "errorMessage":    "may not be null"
                                //         },
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                                //             "errorMessage":    "may not be empty"
                                //         },
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                                //             "errorMessage":    "may not be empty"
                                //         }
                                //     ]
                                // }

                                if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                                 out ValidationErrorList ValidationErrors))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.BadRequest(Request,
                                                                                                           ValidationErrors,
                                                                                                           processId);

                                }

                            }

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

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(Request,
                                                                                                           new Acknowledgement<PushEVSEStatusRequest>(
                                                                                                               Request,
                                                                                                               HTTPResponse.Timestamp,
                                                                                                               HTTPResponse.EventTrackingId,
                                                                                                               HTTPResponse.Runtime,
                                                                                                               statusCode,
                                                                                                               ProcessId: processId
                                                                                                           ),
                                                                                                           processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     Request,
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     false,
                                                     ProcessId: processId
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404
                            // Server: nginx/1.18.0 (Ubuntu)
                            // Date: Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type: application/json;charset=UTF-8
                            // Content-Length: 85
                            // Connection: keep-alive
                            // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                            // 
                            // {
                            //     "StatusCode": {
                            //         "Code":            "300",
                            //         "Description":     "Partner not found",
                            //         "AdditionalInfo":   null
                            //     }
                            // }

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                            out StatusCode  statusCode,
                                                            out String      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(Request,
                                                                                                           new Acknowledgement<PushEVSEStatusRequest>(
                                                                                                               Request,
                                                                                                               HTTPResponse.Timestamp,
                                                                                                               HTTPResponse.EventTrackingId,
                                                                                                               HTTPResponse.Runtime, 
                                                                                                               statusCode,
                                                                                                               ProcessId: processId
                                                                                                           ),
                                                                                                           processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     Request,
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     false,
                                                     ProcessId: processId
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

                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEStatusRequest>(
                                     Request,
                                     DateTime.UtcNow,
                                     Request.EventTrackingId,
                                     DateTime.UtcNow - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     false
                                 )
                             );

                }

                if (result == null)
                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEStatusRequest>(
                                     Request,
                                     DateTime.UtcNow,
                                     Request.EventTrackingId,
                                     DateTime.UtcNow - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         "HTTP request failed!",
                                         null
                                     ),
                                     false
                                 )
                             );

            }


            #region Send OnPushEVSEStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEStatusResponse != null)
                    await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPushEVSEStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp,
                                                     this,
                                                     //ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEStatusRecords.ULongCount(),
                                                     Request.EVSEStatusRecords,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeStart        (Request)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="Request">A AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(AuthorizeStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStart request must not be null!");

            //Request = _CustomAuthorizeStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStart request must not be null!");


            Byte                                    TransmissionRetry   = 0;
            OICPResult<AuthorizationStartResponse>  result              = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnAuthorizeStartRequest != null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp,
                                                     this,
                                                     //ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.SessionId,
                                                     Request.PartnerProductId,
                                                     Request.CPOPartnerSessionId,
                                                     Request.EMPPartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await (RemoteURL.Protocol == HTTPProtocols.http

                                                  ? new HTTPClient (RemoteURL.Hostname,
                                                                    RemotePort:  RemoteURL.Port ?? IPPort.HTTP,
                                                                    DNSClient:   DNSClient)

                                                  : new HTTPSClient(RemoteURL.Hostname,
                                                                    (sender, certificate, chain, policyErrors) => {
                                                                        return true;
                                                                    },
                                                                    (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => {
                                                                        return ClientCert;
                                                                    },
                                                                    ClientCert:  ClientCert,
                                                                    RemotePort:  RemoteURL.Port ?? IPPort.HTTPS,
                                                                    DNSClient:   DNSClient)).

                                                Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + ("/api/oicp/charging/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/authorize/start"),
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                        RequestLogDelegate:   OnAuthorizeStartHTTPRequest,
                                                        ResponseLogDelegate:  OnAuthorizeStartHTTPResponse,
                                                        CancellationToken:    Request.CancellationToken,
                                                        EventTrackingId:      Request.EventTrackingId,
                                                        RequestTimeout:       Request.RequestTimeout ?? this.RequestTimeout).

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

                                // HTTP/1.1 200
                                // Server:             nginx/1.18.0 (Ubuntu)
                                // Date:               Tue, 02 Mar 2021 21:58:48 GMT
                                // Content-Type:       application/json;charset=utf-8
                                // Transfer-Encoding:  chunked
                                // Connection:         keep-alive
                                // Process-ID:         64d20013-dce6-4039-bbaf-d56651ec597f
                                // 
                                // {
                                //     "SessionID":                          null,
                                //     "CPOPartnerSessionID":                "73bbf19b-c468-470b-b186-e79f10a9e950",
                                //     "EMPPartnerSessionID":                null,
                                //     "ProviderID":                         null,
                                //     "AuthorizationStatus":                "NotAuthorized",
                                //     "StatusCode": {
                                //         "Code":                               "210",
                                //         "Description":                        null,
                                //         "AdditionalInfo":                     null
                                //     },
                                //     "AuthorizationStopIdentifications":   null
                                // }

                                // {
                                //     "SessionID":            "8dd819d6-82e8-492c-afc9-8e5cdac35e5a",
                                //     "CPOPartnerSessionID":  "7e2d0869-2ed3-4d7b-9b0b-73bc37bd0e02",
                                //     "EMPPartnerSessionID":  "842bd7b3-bd3f-41ef-bfef-0c2b2cf81cba",
                                //     "ProviderID":           "DE-XXX",
                                //     "AuthorizationStatus":  "Authorized",
                                //     "StatusCode": {
                                //         "Code":                  "000",
                                //         "Description":           "Nice to meet you!",
                                //         "AdditionalInfo":        "Happy charging!"
                                //     },
                                //     "AuthorizationStopIdentifications": [{
                                //         "RFIDMifareFamilyIdentification": {
                                //             "UID": "99887766"
                                //         }
                                //     }, {
                                //         "RFIDMifareFamilyIdentification": {
                                //             "UID": "77665544"
                                //         }
                                //     }]
                                // }

                                // 210 (No valid contract)                         => No valid contact with any EMP!
                                // 102 (RFID Authentication failed – invalid UID)  => No positive authorization from any EMP!

                                if (AuthorizationStartResponse.TryParse(Request,
                                                                        JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                        out AuthorizationStartResponse  authorizationStartResponse,
                                                                        out String                      ErrorResponse,
                                                                        HTTPResponse.Timestamp,
                                                                        HTTPResponse.EventTrackingId,
                                                                        HTTPResponse.Runtime,
                                                                        processId,
                                                                        CustomAuthorizationStartResponseParser))
                                {

                                    result = OICPResult<AuthorizationStartResponse>.Success(Request,
                                                                                            authorizationStartResponse,
                                                                                            processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
                                             ),
                                             processId
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
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList ValidationErrors))
                            {

                                result = OICPResult<AuthorizationStartResponse>.BadRequest(Request,
                                                                                           ValidationErrors,
                                                                                           processId);

                            }

                        }

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

                                    result = OICPResult<AuthorizationStartResponse>.Failed(Request,
                                                                                           AuthorizationStartResponse.NotAuthorized(
                                                                                               Request,
                                                                                               statusCode,
                                                                                               ProcessId: processId
                                                                                           ),
                                                                                           processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
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

                result = OICPResult<AuthorizationStartResponse>.Failed(
                             Request,
                             AuthorizationStartResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            if (result == null)
                result = OICPResult<AuthorizationStartResponse>.Failed(
                             Request,
                             AuthorizationStartResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             )
                         );


            #region Send OnAuthorizeStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnAuthorizeStartResponse != null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartResponseHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp,
                                                     this,
                                                     //ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.SessionId,
                                                     Request.PartnerProductId,
                                                     Request.CPOPartnerSessionId,
                                                     Request.EMPPartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop         (Request)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="Request">A AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(AuthorizeStopRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStop request must not be null!");

            //Request = _CustomAuthorizeStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStop request must not be null!");


            Byte                                   TransmissionRetry   = 0;
            OICPResult<AuthorizationStopResponse>  result              = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnAuthorizeStopRequest != null)
            //        await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
            //                           Cast<OnAuthorizeStopRequestHandler>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.OperatorId,
            //                                         Request.SessionId,
            //                                         Request.Identification,
            //                                         Request.EVSEId,
            //                                         Request.CPOPartnerSessionId,
            //                                         Request.EMPPartnerSessionId,
            //                                         Request.RequestTimeout ?? RequestTimeout.Value))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopRequest));
            //}

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await (RemoteURL.Protocol == HTTPProtocols.http

                                                  ? new HTTPClient (RemoteURL.Hostname,
                                                                    RemotePort:  RemoteURL.Port ?? IPPort.HTTP,
                                                                    DNSClient:   DNSClient)

                                                  : new HTTPSClient(RemoteURL.Hostname,
                                                                    (sender, certificate, chain, policyErrors) => {
                                                                        return true;
                                                                    },
                                                                    (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => {
                                                                        return ClientCert;
                                                                    },
                                                                    ClientCert:  ClientCert,
                                                                    RemotePort:  RemoteURL.Port ?? IPPort.HTTPS,
                                                                    DNSClient:   DNSClient)).

                                                Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + ("/api/oicp/charging/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/authorize/stop"),
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                        RequestLogDelegate:   OnAuthorizeStopHTTPRequest,
                                                        ResponseLogDelegate:  OnAuthorizeStopHTTPResponse,
                                                        CancellationToken:    Request.CancellationToken,
                                                        EventTrackingId:      Request.EventTrackingId,
                                                        RequestTimeout:       Request.RequestTimeout ?? this.RequestTimeout).

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

                                if (AuthorizationStopResponse.TryParse(Request,
                                                                       JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                       out AuthorizationStopResponse  authorizationStopResponse,
                                                                       out String                     ErrorResponse,
                                                                       HTTPResponse.Timestamp,
                                                                       HTTPResponse.EventTrackingId,
                                                                       HTTPResponse.Runtime,
                                                                       processId,
                                                                       CustomAuthorizationStopResponseParser))
                                {

                                    result = OICPResult<AuthorizationStopResponse>.Success(Request,
                                                                                           authorizationStopResponse,
                                                                                           processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
                                             ),
                                             processId
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
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList ValidationErrors))
                            {

                                result = OICPResult<AuthorizationStopResponse>.BadRequest(Request,
                                                                                          ValidationErrors,
                                                                                          processId);

                            }

                        }

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
                                                        out StatusCode statusCode,
                                                        out String     ErrorResponse))
                                {

                                    result = OICPResult<AuthorizationStopResponse>.Failed(Request,
                                                                                          AuthorizationStopResponse.NotAuthorized(
                                                                                              Request,
                                                                                              statusCode,
                                                                                              ProcessId: processId
                                                                                          ),
                                                                                          processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
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
                        //         "Code":            "400",
                        //         "Description":     "Session is not valid",
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
                                                        out StatusCode statusCode,
                                                        out String     ErrorResponse))
                                {

                                    result = OICPResult<AuthorizationStopResponse>.Failed(Request,
                                                                                          AuthorizationStopResponse.NotAuthorized(
                                                                                              Request,
                                                                                              statusCode,
                                                                                              ProcessId: processId
                                                                                          ),
                                                                                          processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
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

                result = OICPResult<AuthorizationStopResponse>.Failed(
                             Request,
                             AuthorizationStopResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            if (result == null)
                result = OICPResult<AuthorizationStopResponse>.Failed(
                             Request,
                             AuthorizationStopResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             )
                         );


            #region Send OnAuthorizeStopResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnAuthorizeStopResponse != null)
            //        await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
            //                           Cast<OnAuthorizeStopResponseHandler>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.OperatorId,
            //                                         Request.SessionId,
            //                                         Request.Identification,
            //                                         Request.EVSEId,
            //                                         Request.CPOPartnerSessionId,
            //                                         Request.EMPPartnerSessionId,
            //                                         Request.RequestTimeout ?? RequestTimeout.Value,
            //                                         result.Content,
            //                                         Endtime - StartTime))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopResponse));
            //}

            #endregion

            return result;

        }

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>>

            SendChargeDetailRecord(SendChargeDetailRecordRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SendChargeDetailRecord request must not be null!");

            //Request = _CustomSendChargeDetailRecordRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SendChargeDetailRecord request must not be null!");


            Byte                                                        TransmissionRetry   = 0;
            OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>  result              = null;

            #endregion

            #region Send OnSendChargeDetailRecord event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnSendChargeDetailRecordRequest != null)
            //        await Task.WhenAll(OnSendChargeDetailRecordRequest.GetInvocationList().
            //                           Cast<OnSendChargeDetailRecordRequestHandler>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.ChargeDetailRecord,
            //                                         Request.RequestTimeout ?? RequestTimeout.Value))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            //}

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await (RemoteURL.Protocol == HTTPProtocols.http

                                                  ? new HTTPClient (RemoteURL.Hostname,
                                                                    RemotePort:  RemoteURL.Port ?? IPPort.HTTP,
                                                                    DNSClient:   DNSClient)

                                                  : new HTTPSClient(RemoteURL.Hostname,
                                                                    (sender, certificate, chain, policyErrors) => {
                                                                        return true;
                                                                    },
                                                                    (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => {
                                                                        return ClientCert;
                                                                    },
                                                                    ClientCert:  ClientCert,
                                                                    RemotePort:  RemoteURL.Port ?? IPPort.HTTPS,
                                                                    DNSClient:   DNSClient)).

                                                Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + ("/api/oicp/cdrmgmt/v22/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/charge-detail-record"),
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                        RequestLogDelegate:   OnSendChargeDetailRecordHTTPRequest,
                                                        ResponseLogDelegate:  OnSendChargeDetailRecordHTTPResponse,
                                                        CancellationToken:    Request.CancellationToken,
                                                        EventTrackingId:      Request.EventTrackingId,
                                                        RequestTimeout:       Request.RequestTimeout ?? this.RequestTimeout).

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

                                if (Acknowledgement<SendChargeDetailRecordRequest>.TryParse(Request,
                                                                                            JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                            out Acknowledgement<SendChargeDetailRecordRequest>  acknowledgement,
                                                                                            out String                                          ErrorResponse,
                                                                                            HTTPResponse.Timestamp,
                                                                                            HTTPResponse.EventTrackingId,
                                                                                            HTTPResponse.Runtime,
                                                                                            processId,
                                                                                            CustomSendChargeDetailRecordAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.Success(Request,
                                                                                                                acknowledgement,
                                                                                                                processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<SendChargeDetailRecordRequest>(
                                                 Request,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 false,
                                                 ProcessId: processId
                                             ),
                                             processId
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
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList ValidationErrors))
                            {

                                result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.BadRequest(Request,
                                                                                                               ValidationErrors,
                                                                                                               processId);

                            }

                        }

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
                                                        out StatusCode statusCode,
                                                        out String ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.Failed(Request,
                                                                                                               new Acknowledgement<SendChargeDetailRecordRequest>(
                                                                                                                   Request,
                                                                                                                   HTTPResponse.Timestamp,
                                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                                   HTTPResponse.Runtime,
                                                                                                                   statusCode,
                                                                                                                   ProcessId: processId
                                                                                                               ),
                                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<SendChargeDetailRecordRequest>(
                                                 Request,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 false,
                                                 ProcessId: processId
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

                result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.Failed(
                             Request,
                             new Acknowledgement<SendChargeDetailRecordRequest>(
                                 Request,
                                 DateTime.UtcNow,
                                 Request.EventTrackingId,
                                 DateTime.UtcNow - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 false
                             )
                         );

            }

            if (result == null)
                result = OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>.Failed(
                             Request,
                             new Acknowledgement<SendChargeDetailRecordRequest>(
                                 Request,
                                 DateTime.UtcNow,
                                 Request.EventTrackingId,
                                 DateTime.UtcNow - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     "HTTP request failed!",
                                     null
                                 ),
                                 false
                             )
                         );



            #region Send OnChargeDetailRecordSent event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnSendChargeDetailRecordResponse != null)
            //        await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
            //                           Cast<OnSendChargeDetailRecordResponseHandler>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp.Value,
            //                                         this,
            //                                         ClientId,
            //                                         Request.EventTrackingId,
            //                                         Request.ChargeDetailRecord,
            //                                         Request.RequestTimeout ?? RequestTimeout.Value,
            //                                         result.Content,
            //                                         Endtime - StartTime))).
            //                           ConfigureAwait(false);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordResponse));
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
