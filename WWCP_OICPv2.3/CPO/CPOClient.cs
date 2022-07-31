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

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO client.
    /// </summary>
    public partial class CPOClient : AHTTPClient,
                                     ICPOClient
    {

        #region (class) Counters

        public class Counters
        {

            public APICounterValues  PushEVSEData                        { get; }
            public APICounterValues  PushEVSEStatus                      { get; }

            public APICounterValues  PushPricingProductData              { get; }
            public APICounterValues  PushEVSEPricing                     { get; }

            public APICounterValues  AuthorizeStart                      { get; }
            public APICounterValues  AuthorizeStop                       { get; }

            public APICounterValues  SendChargingStartNotification       { get; }
            public APICounterValues  SendChargingProgressNotification    { get; }
            public APICounterValues  SendChargingEndNotification         { get; }
            public APICounterValues  SendChargingErrorNotification       { get; }

            public APICounterValues  SendChargeDetailRecord              { get; }


            public Counters(APICounterValues? PushEVSEData                       = null,
                            APICounterValues? PushEVSEStatus                     = null,

                            APICounterValues? PushPricingProductData             = null,
                            APICounterValues? PushEVSEPricing                    = null,

                            APICounterValues? AuthorizeStart                     = null,
                            APICounterValues? AuthorizeStop                      = null,

                            APICounterValues? SendChargingStartNotification      = null,
                            APICounterValues? SendChargingProgressNotification   = null,
                            APICounterValues? SendChargingEndNotification        = null,
                            APICounterValues? SendChargingErrorNotification      = null,

                            APICounterValues? SendChargeDetailRecord             = null)

            {

                this.PushEVSEData                      = PushEVSEData                     ?? new APICounterValues();
                this.PushEVSEStatus                    = PushEVSEStatus                   ?? new APICounterValues();

                this.PushPricingProductData            = PushPricingProductData           ?? new APICounterValues();
                this.PushEVSEPricing                   = PushEVSEPricing                  ?? new APICounterValues();

                this.AuthorizeStart                    = AuthorizeStart                   ?? new APICounterValues();
                this.AuthorizeStop                     = AuthorizeStop                    ?? new APICounterValues();

                this.SendChargingStartNotification     = SendChargingStartNotification    ?? new APICounterValues();
                this.SendChargingProgressNotification  = SendChargingProgressNotification ?? new APICounterValues();
                this.SendChargingEndNotification       = SendChargingEndNotification      ?? new APICounterValues();
                this.SendChargingErrorNotification     = SendChargingErrorNotification    ?? new APICounterValues();

                this.SendChargeDetailRecord            = SendChargeDetailRecord           ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PushEVSEData",                     PushEVSEData.                    ToJSON()),
                       new JProperty("PushEVSEStatus",                   PushEVSEStatus.                  ToJSON()),

                       new JProperty("PushPricingProductData",           PushPricingProductData.          ToJSON()),
                       new JProperty("PushEVSEPricing",                  PushEVSEPricing.                 ToJSON()),

                       new JProperty("AuthorizeStart",                   AuthorizeStart.                  ToJSON()),
                       new JProperty("AuthorizeStop",                    AuthorizeStop.                   ToJSON()),

                       new JProperty("SendChargingStartNotification",    SendChargingStartNotification.   ToJSON()),
                       new JProperty("SendChargingProgressNotification", SendChargingProgressNotification.ToJSON()),
                       new JProperty("SendChargingEndNotification",      SendChargingEndNotification.     ToJSON()),
                       new JProperty("SendChargingErrorNotification",    SendChargingErrorNotification.   ToJSON()),

                       new JProperty("SendChargeDetailRecord",           SendChargeDetailRecord.          ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.Number + " CPO Client";

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

        public Counters                                                                           Counter                                                      { get; }

        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEDataRequest>>                  CustomPushEVSEDataAcknowledgementParser                      { get; set; }
        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEStatusRequest>>                CustomPushEVSEStatusAcknowledgementParser                    { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<PushPricingProductDataRequest>>        CustomPushPricingProductDataAcknowledgementParser            { get; set; }
        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEPricingRequest>>               CustomPushEVSEPricingAcknowledgementParser                   { get; set; }

        public CustomJObjectParserDelegate<AuthorizationStartResponse>                            CustomAuthorizationStartResponseParser                       { get; set; }
        public CustomJObjectParserDelegate<AuthorizationStopResponse>                             CustomAuthorizationStopResponseParser                        { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargingStartNotificationRequest>>     CustomChargingStartNotificationAcknowledgementParser         { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingProgressNotificationRequest>>  CustomChargingProgressNotificationAcknowledgementParser      { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingEndNotificationRequest>>       CustomChargingEndNotificationAcknowledgementParser           { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingErrorNotificationRequest>>     CustomChargingErrorNotificationAcknowledgementParser         { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargeDetailRecordRequest>>            CustomSendChargeDetailRecordAcknowledgementParser            { get; set; }


        public Newtonsoft.Json.Formatting                                                         JSONFormat                                                   { get; set; }

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


        #region OnPushPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushPricingProductData will be send.
        /// </summary>
        public event OnPushPricingProductDataRequestDelegate   OnPushPricingProductDataRequest;

        /// <summary>
        /// An event fired whenever a PushPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                   OnPushPricingProductDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                  OnPushPricingProductDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event OnPushPricingProductDataResponseDelegate  OnPushPricingProductDataResponse;

        #endregion

        #region OnPushEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEPricing will be send.
        /// </summary>
        public event OnPushEVSEPricingRequestDelegate   OnPushEVSEPricingRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler            OnPushEVSEPricingHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler           OnPushEVSEPricingHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event OnPushEVSEPricingResponseDelegate  OnPushEVSEPricingResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStart request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate     OnAuthorizeStartRequest;

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
        public event OnAuthorizeStartResponseDelegate    OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStop request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

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
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion


        #region OnChargingStartNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingStartNotification will be send.
        /// </summary>
        public event OnChargingStartNotificationRequestDelegate   OnChargingStartNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingStartNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnChargingStartNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnChargingStartNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification had been received.
        /// </summary>
        public event OnChargingStartNotificationResponseDelegate  OnChargingStartNotificationResponse;

        #endregion

        #region OnChargingProgressNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification will be send.
        /// </summary>
        public event OnChargingProgressNotificationRequestDelegate   OnChargingProgressNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                         OnChargingProgressNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                        OnChargingProgressNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification had been received.
        /// </summary>
        public event OnChargingProgressNotificationResponseDelegate  OnChargingProgressNotificationResponse;

        #endregion

        #region OnChargingEndNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingEndNotification will be send.
        /// </summary>
        public event OnChargingEndNotificationRequestDelegate   OnChargingEndNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingEndNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnChargingEndNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnChargingEndNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification had been received.
        /// </summary>
        public event OnChargingEndNotificationResponseDelegate  OnChargingEndNotificationResponse;

        #endregion

        #region OnChargingErrorNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification will be send.
        /// </summary>
        public event OnChargingErrorNotificationRequestDelegate   OnChargingErrorNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnChargingErrorNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnChargingErrorNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification had been received.
        /// </summary>
        public event OnChargingErrorNotificationResponseDelegate  OnChargingErrorNotificationResponse;

        #endregion


        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestDelegate   OnSendChargeDetailRecordRequest;

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
        public event OnSendChargeDetailRecordResponseDelegate  OnSendChargeDetailRecordResponse;

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
        public CPOClient(URL?                                  RemoteURL                    = null,
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


        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(CPOClient));


        #region PushEVSEData                    (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>> PushEVSEData(PushEVSEDataRequest Request)
        {

            #region Initial checks

            //Request = _CustomPushEVSEDataRequestMapper(Request);

            Byte                                               TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushEVSEDataRequest>>?  result              = null;

            #endregion

            #region Send OnPushEVSEDataRequest event

            var StartTime = Timestamp.Now;

            Counter.PushEVSEData.IncRequests_OK();

            try
            {

                if (OnPushEVSEDataRequest != null)
                    await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                       Cast<OnPushEVSEDataRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEDataRequest));
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

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          RemoteCertificateValidator,
                                                                          null,
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

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepush/v23/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/data-records"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushEVSEDataHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEDataHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // cd .
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
                                                                                      JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String() ?? ""),
                                                                                      out Acknowledgement<PushEVSEDataRequest>?  acknowledgement,
                                                                                      out String?                                ErrorResponse,
                                                                                      HTTPResponse,
                                                                                      HTTPResponse.Timestamp,
                                                                                      HTTPResponse.EventTrackingId,
                                                                                      HTTPResponse.Runtime,
                                                                                      processId,
                                                                                      CustomPushEVSEDataAcknowledgementParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(Request,
                                                                                                          acknowledgement!,
                                                                                                          processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
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

                                if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String() ?? "",
                                                                 out ValidationErrorList?  validationErrors,
                                                                 out String?               errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.BadRequest(Request,
                                                                                                         validationErrors,
                                                                                                         processId);

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(Request,
                                                                                                         new Acknowledgement<PushEVSEDataRequest>(
                                                                                                             HTTPResponse.Timestamp,
                                                                                                             HTTPResponse.EventTrackingId,
                                                                                                             HTTPResponse.Runtime,
                                                                                                             statusCode!,
                                                                                                             Request,
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
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type:    application/json;charset=UTF-8
                            // Content-Length:  85
                            // Connection:      keep-alive
                            // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
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
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(Request,
                                                                                                         new Acknowledgement<PushEVSEDataRequest>(
                                                                                                             HTTPResponse.Timestamp,
                                                                                                             HTTPResponse.EventTrackingId,
                                                                                                             HTTPResponse.Runtime,
                                                                                                             statusCode!,
                                                                                                             Request,
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
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
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

                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEDataRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId,
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushEVSEDataRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId,
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

            }


            #region Send OnPushEVSEDataResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEDataResponse != null)
                    await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                       Cast<OnPushEVSEDataResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEStatus                  (Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>> PushEVSEStatus(PushEVSEStatusRequest Request)
        {

            #region Initial checks

            //Request = _CustomPushEVSEStatusRequestMapper(Request);

            Byte                                                 TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushEVSEStatusRequest>>?  result              = null;

            #endregion

            #region Send OnPushEVSEStatusRequest event

            var StartTime = Timestamp.Now;

            Counter.PushEVSEStatus.IncRequests_OK();

            try
            {

                if (OnPushEVSEStatusRequest != null)
                    await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPushEVSEStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEStatusRequest));
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

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          RemoteCertificateValidator,
                                                                          null,
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

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepush/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/status-records"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushEVSEStatusHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEStatusHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
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
                                                                                        JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String() ?? ""),
                                                                                        out Acknowledgement<PushEVSEStatusRequest>?  acknowledgement,
                                                                                        out String?                                  ErrorResponse,
                                                                                        HTTPResponse,
                                                                                        HTTPResponse.Timestamp,
                                                                                        HTTPResponse.EventTrackingId,
                                                                                        HTTPResponse.Runtime,
                                                                                        processId,
                                                                                        CustomPushEVSEStatusAcknowledgementParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(Request,
                                                                                                            acknowledgement!,
                                                                                                            processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
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

                                if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String() ?? "",
                                                                 out ValidationErrorList?  validationErrors,
                                                                 out String?               errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.BadRequest(Request,
                                                                                                           validationErrors,
                                                                                                           processId);

                                }

                            }

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(Request,
                                                                                                           new Acknowledgement<PushEVSEStatusRequest>(
                                                                                                               HTTPResponse.Timestamp,
                                                                                                               HTTPResponse.EventTrackingId,
                                                                                                               HTTPResponse.Runtime,
                                                                                                               statusCode!,
                                                                                                               Request,
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
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
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
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(Request,
                                                                                                           new Acknowledgement<PushEVSEStatusRequest>(
                                                                                                               HTTPResponse.Timestamp,
                                                                                                               HTTPResponse.EventTrackingId,
                                                                                                               HTTPResponse.Runtime, 
                                                                                                               statusCode!,
                                                                                                               Request,
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
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
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

                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEStatusRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId,
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushEVSEStatusRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId,
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

            }


            #region Send OnPushEVSEStatusResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEStatusResponse != null)
                    await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPushEVSEStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PushPricingProductData          (Request)

        /// <summary>
        /// Upload the given Pricing Product Data.
        /// </summary>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>> PushPricingProductData(PushPricingProductDataRequest Request)
        {

            #region Initial checks

            //Request = _CustomPushPricingProductDataRequestMapper(Request);

            Byte                                                         TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushPricingProductDataRequest>>?  result              = null;

            #endregion

            #region Send OnPushPricingProductDataRequest event

            var StartTime = Timestamp.Now;

            Counter.PushPricingProductData.IncRequests_OK();

            try
            {

                if (OnPushPricingProductDataRequest != null)
                    await Task.WhenAll(OnPushPricingProductDataRequest.GetInvocationList().
                                       Cast<OnPushPricingProductDataRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushPricingProductDataRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.PricingProductDataRecords.Any())
            {

                result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Success(
                             Request,
                             Acknowledgement<PushPricingProductDataRequest>.Success(Request,
                                                                                    StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else
            {

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          RemoteCertificateValidator,
                                                                          null,
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

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/dynamicpricing/v10/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/pricing-products"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushPricingProductDataHTTPRequest,
                                                          ResponseLogDelegate:  OnPushPricingProductDataHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // cd .
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

                                    if (Acknowledgement<PushPricingProductDataRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String() ?? ""),
                                                                                                out Acknowledgement<PushPricingProductDataRequest>?  acknowledgement,
                                                                                                out String?                                          ErrorResponse,
                                                                                                HTTPResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomPushPricingProductDataAcknowledgementParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Success(Request,
                                                                                                                    acknowledgement!,
                                                                                                                    processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushPricingProductDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
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

                                if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String() ?? "",
                                                                 out ValidationErrorList?  validationErrors,
                                                                 out String?               errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.BadRequest(Request,
                                                                                                                   validationErrors,
                                                                                                                   processId);

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<PushPricingProductDataRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime,
                                                                                                                       statusCode!,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushPricingProductDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type:    application/json;charset=UTF-8
                            // Content-Length:  85
                            // Connection:      keep-alive
                            // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
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
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<PushPricingProductDataRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime,
                                                                                                                       statusCode!,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushPricingProductDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
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

                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushPricingProductDataRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId,
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushPricingProductDataRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId,
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

            }


            #region Send OnPushPricingProductDataResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnPushPricingProductDataResponse != null)
                    await Task.WhenAll(OnPushPricingProductDataResponse.GetInvocationList().
                                       Cast<OnPushPricingProductDataResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushPricingProductDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEPricing                 (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEPricing request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>> PushEVSEPricing(PushEVSEPricingRequest Request)
        {

            #region Initial checks

            //Request = _CustomPushEVSEPricingRequestMapper(Request);

            Byte                                                  TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushEVSEPricingRequest>>?  result              = null;

            #endregion

            #region Send OnPushEVSEPricingRequest event

            var StartTime = Timestamp.Now;

            Counter.PushEVSEPricing.IncRequests_OK();

            try
            {

                if (OnPushEVSEPricingRequest != null)
                    await Task.WhenAll(OnPushEVSEPricingRequest.GetInvocationList().
                                       Cast<OnPushEVSEPricingRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEPricingRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.EVSEPricing.Any())
            {

                result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Success(
                             Request,
                             Acknowledgement<PushEVSEPricingRequest>.Success(Request,
                                                                             StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else
            {

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          RemoteCertificateValidator,
                                                                          null,
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

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/dynamicpricing/v10/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/evse-pricing"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushEVSEPricingHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEPricingHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // cd .
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

                                    if (Acknowledgement<PushEVSEPricingRequest>.TryParse(Request,
                                                                                         JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String() ?? ""),
                                                                                         out Acknowledgement<PushEVSEPricingRequest>?  acknowledgement,
                                                                                         out String?                                   ErrorResponse,
                                                                                         HTTPResponse,
                                                                                         HTTPResponse.Timestamp,
                                                                                         HTTPResponse.EventTrackingId,
                                                                                         HTTPResponse.Runtime,
                                                                                         processId,
                                                                                         CustomPushEVSEPricingAcknowledgementParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Success(Request,
                                                                                                             acknowledgement!,
                                                                                                             processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEPricingRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
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

                                if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String() ?? "",
                                                                 out ValidationErrorList?  validationErrors,
                                                                 out String?               errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.BadRequest(Request,
                                                                                                            validationErrors,
                                                                                                            processId);

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                                HTTPResponse.HTTPBody.Length > 0)
                            {

                                try
                                {

                                    if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(Request,
                                                                                                         new Acknowledgement<PushEVSEPricingRequest>(
                                                                                                             HTTPResponse.Timestamp,
                                                                                                             HTTPResponse.EventTrackingId,
                                                                                                             HTTPResponse.Runtime,
                                                                                                             statusCode!,
                                                                                                             Request,
                                                                                                             ProcessId: processId
                                                                                                         ),
                                                                                                         processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEPricingRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type:    application/json;charset=UTF-8
                            // Content-Length:  85
                            // Connection:      keep-alive
                            // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
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
                                                            out StatusCode?  statusCode,
                                                            out String?      ErrorResponse))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(Request,
                                                                                                         new Acknowledgement<PushEVSEPricingRequest>(
                                                                                                             HTTPResponse.Timestamp,
                                                                                                             HTTPResponse.EventTrackingId,
                                                                                                             HTTPResponse.Runtime,
                                                                                                             statusCode!,
                                                                                                             Request,
                                                                                                             ProcessId: processId
                                                                                                         ),
                                                                                                         processId);

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEPricingRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false,
                                                     ProcessId: processId
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

                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEPricingRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId,
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushEVSEPricingRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId,
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

            }


            #region Send OnPushEVSEPricingResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEPricingResponse != null)
                    await Task.WhenAll(OnPushEVSEPricingResponse.GetInvocationList().
                                       Cast<OnPushEVSEPricingResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEPricingResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeStart                  (Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>> AuthorizeStart(AuthorizeStartRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeStartRequestMapper(Request);

            Byte                                     TransmissionRetry   = 0;
            OICPResult<AuthorizationStartResponse>?  result              = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = Timestamp.Now;

            Counter.AuthorizeStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeStartRequest != null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
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
                                                                        HTTPResponse,
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<AuthorizationStartResponse>.BadRequest(Request,
                                                                                           validationErrors,
                                                                                           processId);

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
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

            result ??= OICPResult<AuthorizationStartResponse>.Failed(
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

            var Endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStartResponse != null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartResponseDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop                   (Request)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>> AuthorizeStop(AuthorizeStopRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeStopRequestMapper(Request);

            Byte                                    TransmissionRetry   = 0;
            OICPResult<AuthorizationStopResponse>?  result              = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = Timestamp.Now;

            Counter.AuthorizeStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeStopRequest != null)
                    await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeStopRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/authorize/stop"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeStopHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
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
                                                                       HTTPResponse,
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<AuthorizationStopResponse>.BadRequest(Request,
                                                                                          validationErrors,
                                                                                          processId);

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
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

            result ??= OICPResult<AuthorizationStopResponse>.Failed(
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

            var Endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStopResponse != null)
                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeStopResponseDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargingStartNotification   (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingStartNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>> SendChargingStartNotification(ChargingStartNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomSendChargingStartNotificationRequestMapper(Request);

            Byte                                                            TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingStartNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingStartNotificationRequest event

            var StartTime = Timestamp.Now;

            Counter.SendChargingStartNotification.IncRequests_OK();

            try
            {

                if (OnChargingStartNotificationRequest != null)
                    await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                       Cast<OnChargingStartNotificationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingStartNotificationRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingStartNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingStartNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
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

                                if (Acknowledgement<ChargingStartNotificationRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingStartNotificationRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingStartNotificationAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingStartNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 ProcessId: processId
                                             )
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.BadRequest(Request,
                                                                                                                  validationErrors,
                                                                                                                  processId);

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime,
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingStartNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
                                                )
                                            );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime, 
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingStartNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
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

                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingStartNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingStartNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );


            #region  OnChargingStartNotificationResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnChargingStartNotificationResponse != null)
                    await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                       Cast<OnChargingStartNotificationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingStartNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingProgressNotification(Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A ChargingProgressNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>> SendChargingProgressNotification(ChargingProgressNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomSendChargingProgressNotificationRequestMapper(Request);

            Byte                                                               TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingProgressNotificationRequest event

            var StartTime = Timestamp.Now;

            Counter.SendChargingProgressNotification.IncRequests_OK();

            try
            {

                if (OnChargingProgressNotificationRequest != null)
                    await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                       Cast<OnChargingProgressNotificationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingProgressNotificationRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingProgressNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingProgressNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
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

                                if (Acknowledgement<ChargingProgressNotificationRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingProgressNotificationRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingProgressNotificationAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingProgressNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 ProcessId: processId
                                             )
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.BadRequest(Request,
                                                                                                                     validationErrors,
                                                                                                                     processId);

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime,
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingProgressNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
                                                )
                                            );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime, 
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingProgressNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
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

                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingProgressNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingProgressNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );


            #region  OnChargingProgressNotificationResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnChargingProgressNotificationResponse != null)
                    await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                       Cast<OnChargingProgressNotificationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingProgressNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingEndNotification     (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingEndNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>> SendChargingEndNotification(ChargingEndNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomSendChargingEndNotificationRequestMapper(Request);

            Byte                                                          TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingEndNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingEndNotificationRequest event

            var StartTime = Timestamp.Now;

            Counter.SendChargingEndNotification.IncRequests_OK();

            try
            {

                if (OnChargingEndNotificationRequest != null)
                    await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                       Cast<OnChargingEndNotificationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingEndNotificationRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingEndNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingEndNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200 OK
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

                                if (Acknowledgement<ChargingEndNotificationRequest>.TryParse(Request,
                                                                                              JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                              out Acknowledgement<ChargingEndNotificationRequest>  acknowledgement,
                                                                                              out String                                              ErrorResponse,
                                                                                              HTTPResponse,
                                                                                              HTTPResponse.Timestamp,
                                                                                              HTTPResponse.EventTrackingId,
                                                                                              HTTPResponse.Runtime,
                                                                                              processId,
                                                                                              CustomChargingEndNotificationAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingEndNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 ProcessId: processId
                                             )
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.BadRequest(Request,
                                                                                                                validationErrors,
                                                                                                                processId);

                            }

                        }

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

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime,
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingEndNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
                                                )
                                            );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404 NotFound
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime, 
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingEndNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
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

                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingEndNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingEndNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );


            #region  OnChargingEndNotificationResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnChargingEndNotificationResponse != null)
                    await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                       Cast<OnChargingEndNotificationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingEndNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingErrorNotification   (Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A ChargingErrorNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>> SendChargingErrorNotification(ChargingErrorNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomSendChargingErrorNotificationRequestMapper(Request);

            Byte                                                            TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingErrorNotificationRequest event

            var StartTime = Timestamp.Now;

            Counter.SendChargingErrorNotification.IncRequests_OK();

            try
            {

                if (OnChargingErrorNotificationRequest != null)
                    await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                       Cast<OnChargingErrorNotificationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingErrorNotificationRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingErrorNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingErrorNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200 OK
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

                                if (Acknowledgement<ChargingErrorNotificationRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                out Acknowledgement<ChargingErrorNotificationRequest>  acknowledgement,
                                                                                                out String                                              ErrorResponse,
                                                                                                HTTPResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomChargingErrorNotificationAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Success(Request,
                                                                                                                    acknowledgement,
                                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingErrorNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 ProcessId: processId
                                             )
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.BadRequest(Request,
                                                                                                                  validationErrors,
                                                                                                                  processId);

                            }

                        }

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
                                                        out String?      errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime,
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingErrorNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
                                                )
                                            );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404 NotFound
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
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(Request,
                                                                                                                   new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                                                       HTTPResponse.Timestamp,
                                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                                       HTTPResponse.Runtime, 
                                                                                                                       statusCode,
                                                                                                                       Request,
                                                                                                                       ProcessId: processId
                                                                                                                   ),
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingErrorNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false,
                                                    ProcessId: processId
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

                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingErrorNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingErrorNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );


            #region  OnChargingErrorNotificationResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnChargingErrorNotificationResponse != null)
                    await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                       Cast<OnChargingErrorNotificationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingErrorNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord          (Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>> SendChargeDetailRecord(ChargeDetailRecordRequest Request)
        {

            #region Initial checks

            //Request = _CustomSendChargeDetailRecordRequestMapper(Request);

            Byte                                                     TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargeDetailRecordRequest>>?  result              = null;

            #endregion

            #region Send OnSendChargeDetailRecord event

            var StartTime = Timestamp.Now;

            Counter.SendChargeDetailRecord.IncRequests_OK();

            try
            {

                if (OnSendChargeDetailRecordRequest != null)
                    await Task.WhenAll(OnSendChargeDetailRecordRequest.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      null,
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/cdrmgmt/v22/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/charge-detail-record"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnSendChargeDetailRecordHTTPRequest,
                                                      ResponseLogDelegate:  OnSendChargeDetailRecordHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<ChargeDetailRecordRequest>.TryParse(Request,
                                                                                            JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                            out Acknowledgement<ChargeDetailRecordRequest> acknowledgement,
                                                                                            out String ErrorResponse,
                                                                                            HTTPResponse,
                                                                                            HTTPResponse.Timestamp,
                                                                                            HTTPResponse.EventTrackingId,
                                                                                            HTTPResponse.Runtime,
                                                                                            processId,
                                                                                            CustomSendChargeDetailRecordAcknowledgementParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Success(Request,
                                                                                                                acknowledgement,
                                                                                                                processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargeDetailRecordRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 OK
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
                                                             out ValidationErrorList?  validationErrors,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.BadRequest(Request,
                                                                                                           validationErrors,
                                                                                                           processId);

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized ||
                        HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
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

                        // => Operator/provider identification is not linked to the TLS client certificate!


                        // HTTP/1.1 404 Not Found
                        // Server:             nginx/1.18.0 (Ubuntu)
                        // Date:               Tue, 01 Jun 2021 21:22:45 GMT
                        // Content-Type:       application/json
                        // Transfer-Encoding:  chunked
                        // Connection:         close
                        // Process-ID:         51c4bb66-052c-4c1c-a288-31902dc81fd1
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "400",
                        //         "Description":     "Session found but status is not valid CLOSED!",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode statusCode,
                                                        out String ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(Request,
                                                                                                               new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                                                   HTTPResponse.Timestamp,
                                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                                   HTTPResponse.Runtime,
                                                                                                                   statusCode,
                                                                                                                   Request,
                                                                                                                   ProcessId: processId
                                                                                                               ),
                                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargeDetailRecordRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 ProcessId: processId
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

                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargeDetailRecordRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargeDetailRecordRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );


            #region Send OnChargeDetailRecordSent event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnSendChargeDetailRecordResponse != null)
                    await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordResponseDelegate>().
                                       Select(e => e(StartTime,
                                                     this,
                                                     Description,
                                                     Request,
                                                     result))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
