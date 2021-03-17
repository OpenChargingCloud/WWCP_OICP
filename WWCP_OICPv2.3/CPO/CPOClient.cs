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

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The OICP CPO client.
    /// </summary>
    public partial class CPOClient : AHTTPClient,
                                     ICPOClient
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

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const    String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.Number + " CPO Client";

        /// <summary>
        /// The default timeout for HTTP requests.
        /// </summary>
        public new readonly TimeSpan  DefaultRequestTimeout       = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The default maximum number of transmission retries for HTTP request.
        /// </summary>
        public new const    UInt16    DefaultMaxNumberOfRetries   = 3;

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

        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEDataRequest>>                   CustomPushEVSEDataAcknowledgementParser                       { get; set; }
        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEStatusRequest>>                 CustomPushEVSEStatusAcknowledgementParser                     { get; set; }


        public CustomJObjectParserDelegate<AuthorizationStartResponse>                             CustomAuthorizationStartResponseParser                        { get; set; }
        public CustomJObjectParserDelegate<AuthorizationStopResponse>                              CustomAuthorizationStopResponseParser                         { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargingNotificationsStartRequest>>     CustomChargingNotificationsStartAcknowledgementParser         { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingNotificationsProgressRequest>>  CustomChargingNotificationsProgressAcknowledgementParser      { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingNotificationsEndRequest>>       CustomChargingNotificationsEndAcknowledgementParser           { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingNotificationsErrorRequest>>     CustomChargingNotificationsErrorAcknowledgementParser         { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<SendChargeDetailRecordRequest>>         CustomSendChargeDetailRecordAcknowledgementParser             { get; set; }

        #endregion

        #region Events

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEData will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnPushEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPushEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEData HTTP request had been received.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate  OnPushEVSEDataResponse;

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEStatus will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEStatus HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnPushEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEStatus HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnPushEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEStatus HTTP request had been received.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStart request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestHandler     OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler            OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler           OnAuthorizeStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart request had been received.
        /// </summary>
        public event OnAuthorizeStartResponseHandler    OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStop request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestHandler   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnAuthorizeStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop request had been received.
        /// </summary>
        public event OnAuthorizeStopResponseHandler  OnAuthorizeStopResponse;

        #endregion


        #region OnChargingNotificationsStartRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingNotificationsStart will be send.
        /// </summary>
        public event OnChargingNotificationsStartRequestHandler   OnChargingNotificationsStartRequest;

        /// <summary>
        /// An event fired whenever a ChargingNotificationsStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnChargingNotificationsStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnChargingNotificationsStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsStart had been received.
        /// </summary>
        public event OnChargingNotificationsStartResponseHandler  OnChargingNotificationsStartResponse;

        #endregion

        #region OnChargingNotificationsProgressRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingNotificationsProgress will be send.
        /// </summary>
        public event OnChargingNotificationsProgressRequestHandler   OnChargingNotificationsProgressRequest;

        /// <summary>
        /// An event fired whenever a ChargingNotificationsProgress HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                         OnChargingNotificationsProgressHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsProgress HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                        OnChargingNotificationsProgressHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsProgress had been received.
        /// </summary>
        public event OnChargingNotificationsProgressResponseHandler  OnChargingNotificationsProgressResponse;

        #endregion

        #region OnChargingNotificationsEndRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingNotificationsEnd will be send.
        /// </summary>
        public event OnChargingNotificationsEndRequestHandler   OnChargingNotificationsEndRequest;

        /// <summary>
        /// An event fired whenever a ChargingNotificationsEnd HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnChargingNotificationsEndHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsEnd HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnChargingNotificationsEndHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsEnd had been received.
        /// </summary>
        public event OnChargingNotificationsEndResponseHandler  OnChargingNotificationsEndResponse;

        #endregion

        #region OnChargingNotificationsErrorRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingNotificationsError will be send.
        /// </summary>
        public event OnChargingNotificationsErrorRequestHandler   OnChargingNotificationsErrorRequest;

        /// <summary>
        /// An event fired whenever a ChargingNotificationsError HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnChargingNotificationsErrorHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsError HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnChargingNotificationsErrorHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingNotificationsError had been received.
        /// </summary>
        public event OnChargingNotificationsErrorResponseHandler  OnChargingNotificationsErrorResponse;

        #endregion


        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestHandler   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnSendChargeDetailRecordHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargeDetailRecord HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSendChargeDetailRecordHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargeDetailRecord had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler  OnSendChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO client.
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
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public CPOClient(URL?                                 RemoteURL                    = null,
                         HTTPHostname?                        VirtualHostname              = null,
                         String                               Description                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         X509Certificate                      ClientCert                   = null,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         TransmissionRetryDelayDelegate       TransmissionRetryDelay       = null,
                         UInt16?                              MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                         Boolean                              DisableLogging               = false,
                         String                               LoggingContext               = Logger.DefaultContext,
                         LogfileCreatorDelegate               LogfileCreator               = null,
                         DNSClient                            DNSClient                    = null)

            : base(RemoteURL           ?? URL.Parse("https://service.hubject-qa.com"),
                   VirtualHostname,
                   Description,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   ClientCert,
                   HTTPUserAgent       ?? DefaultHTTPUserAgent,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries  ?? DefaultMaxNumberOfRetries,
                   false,
                   null,
                   DNSClient)

        {

            base.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingContext,
                                                LogfileCreator)
                                   : null;

        }

        #endregion

        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(CPOClient));


        #region PushEVSEData                     (Request)

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

        #region PushEVSEStatus                   (Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>> PushEVSEStatus(PushEVSEStatusRequest Request)
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


        #region AuthorizeStart                   (Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>> AuthorizeStart(AuthorizeStartRequest Request)
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

        #region AuthorizeStop                    (Request)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>> AuthorizeStop(AuthorizeStopRequest Request)
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


        #region SendChargingNotificationsStart   (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsStart request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>> SendChargingNotificationsStart(ChargingNotificationsStartRequest Request)
        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given ChargingNotificationsStart request must not be null!");

            //Request = _CustomSendChargingNotificationsStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped ChargingNotificationsStart request must not be null!");


            Byte                                                           TransmissionRetry  = 0;
            OICPResult<Acknowledgement<ChargingNotificationsStartRequest>> result             = null;

            #endregion

            #region  OnChargingNotificationsStartRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsStartRequest != null)
            //        await Task.WhenAll(OnChargingNotificationsStartRequest.GetInvocationList().
            //                           Cast<OnChargingNotificationsStartRequestDelegate>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsStartRequest));
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

                                                 Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                         RequestLogDelegate:   OnChargingNotificationsStartHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingNotificationsStartHTTPResponse,
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

                                if (Acknowledgement<ChargingNotificationsStartRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingNotificationsStartRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingNotificationsStartAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingNotificationsStartRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsStartRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsStartRequest>(
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsStartRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsStartRequest>(
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

                result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsStartRequest>(
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
                result = OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsStartRequest>(
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


            #region  OnChargingNotificationsStartResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsStartResponse != null)
            //        await Task.WhenAll(OnChargingNotificationsStartResponse.GetInvocationList().
            //                           Cast<OnChargingNotificationsStartResponseDelegate>().
            //                           Select(e => e(Endtime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsStartResponse));
            //}

            #endregion

            return result;

        }

        #endregion

        #region SendChargingNotificationsProgress(Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsProgress request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>> SendChargingNotificationsProgress(ChargingNotificationsProgressRequest Request)
        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given ChargingNotificationsProgress request must not be null!");

            //Request = _CustomSendChargingNotificationsProgressRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped ChargingNotificationsProgress request must not be null!");


            Byte                                                           TransmissionRetry  = 0;
            OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>> result             = null;

            #endregion

            #region  OnChargingNotificationsProgressRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsProgressRequest != null)
            //        await Task.WhenAll(OnChargingNotificationsProgressRequest.GetInvocationList().
            //                           Cast<OnChargingNotificationsProgressRequestDelegate>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsProgressRequest));
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

                                                 Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                         RequestLogDelegate:   OnChargingNotificationsProgressHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingNotificationsProgressHTTPResponse,
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

                                if (Acknowledgement<ChargingNotificationsProgressRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingNotificationsProgressRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingNotificationsProgressAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingNotificationsProgressRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsProgressRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsProgressRequest>(
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsProgressRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsProgressRequest>(
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

                result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsProgressRequest>(
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
                result = OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsProgressRequest>(
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


            #region  OnChargingNotificationsProgressResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsProgressResponse != null)
            //        await Task.WhenAll(OnChargingNotificationsProgressResponse.GetInvocationList().
            //                           Cast<OnChargingNotificationsProgressResponseDelegate>().
            //                           Select(e => e(Endtime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsProgressResponse));
            //}

            #endregion

            return result;

        }

        #endregion

        #region SendChargingNotificationsEnd     (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsEnd request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>> SendChargingNotificationsEnd(ChargingNotificationsEndRequest Request)
        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given ChargingNotificationsEnd request must not be null!");

            //Request = _CustomSendChargingNotificationsEndRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped ChargingNotificationsEnd request must not be null!");


            Byte                                                           TransmissionRetry  = 0;
            OICPResult<Acknowledgement<ChargingNotificationsEndRequest>> result             = null;

            #endregion

            #region  OnChargingNotificationsEndRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsEndRequest != null)
            //        await Task.WhenAll(OnChargingNotificationsEndRequest.GetInvocationList().
            //                           Cast<OnChargingNotificationsEndRequestDelegate>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsEndRequest));
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

                                                 Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                         RequestLogDelegate:   OnChargingNotificationsEndHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingNotificationsEndHTTPResponse,
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

                                if (Acknowledgement<ChargingNotificationsEndRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingNotificationsEndRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingNotificationsEndAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingNotificationsEndRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsEndRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsEndRequest>(
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsEndRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsEndRequest>(
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

                result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsEndRequest>(
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
                result = OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsEndRequest>(
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


            #region  OnChargingNotificationsEndResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsEndResponse != null)
            //        await Task.WhenAll(OnChargingNotificationsEndResponse.GetInvocationList().
            //                           Cast<OnChargingNotificationsEndResponseDelegate>().
            //                           Select(e => e(Endtime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsEndResponse));
            //}

            #endregion

            return result;

        }

        #endregion

        #region SendChargingNotificationsError   (Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsError request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>> SendChargingNotificationsError(ChargingNotificationsErrorRequest Request)
        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given ChargingNotificationsError request must not be null!");

            //Request = _CustomSendChargingNotificationsErrorRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped ChargingNotificationsError request must not be null!");


            Byte                                                           TransmissionRetry  = 0;
            OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>> result             = null;

            #endregion

            #region  OnChargingNotificationsErrorRequest event

            var StartTime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsErrorRequest != null)
            //        await Task.WhenAll(OnChargingNotificationsErrorRequest.GetInvocationList().
            //                           Cast<OnChargingNotificationsErrorRequestDelegate>().
            //                           Select(e => e(StartTime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsErrorRequest));
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

                                                 Execute(client => client.CreateRequest(HTTPMethod.POST,
                                                                                        RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                        requestbuilder => {
                                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                            requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                            requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes();
                                                                                        }),

                                                         RequestLogDelegate:   OnChargingNotificationsErrorHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingNotificationsErrorHTTPResponse,
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

                                if (Acknowledgement<ChargingNotificationsErrorRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingNotificationsErrorRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingNotificationsErrorAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingNotificationsErrorRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsErrorRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsErrorRequest>(
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

                                    result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingNotificationsErrorRequest>(
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

                                result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingNotificationsErrorRequest>(
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

                result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsErrorRequest>(
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
                result = OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>.Failed(
                                Request,
                                new Acknowledgement<ChargingNotificationsErrorRequest>(
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


            #region  OnChargingNotificationsErrorResponse event

            var Endtime = DateTime.UtcNow;

            //try
            //{

            //    if (OnChargingNotificationsErrorResponse != null)
            //        await Task.WhenAll(OnChargingNotificationsErrorResponse.GetInvocationList().
            //                           Cast<OnChargingNotificationsErrorResponseDelegate>().
            //                           Select(e => e(Endtime,
            //                                         Request.Timestamp,
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
            //    e.Log(nameof(CPOClient) + "." + nameof(OnChargingNotificationsErrorResponse));
            //}

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord           (Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>> SendChargeDetailRecord(SendChargeDetailRecordRequest Request)
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
