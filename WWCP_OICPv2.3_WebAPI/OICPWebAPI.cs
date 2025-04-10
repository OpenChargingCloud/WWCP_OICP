/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.CentralService;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.WebAPI
{

    /// <summary>
    /// A HTTP API providing OICP v2.3 data structures and HTTP server side events.
    /// </summary>
    public class OICPWebAPI
    {

        #region Data

        private readonly HTTPAPI httpAPI;

        /// <summary>
        /// The default HTTP URI prefix.
        /// </summary>
        public static readonly HTTPPath             DefaultURLPathPrefix        = HTTPPath.Parse("/");

        public static readonly HTTPEventSource_Id   DebugLogId                  = HTTPEventSource_Id.Parse("OICPDebugLog");

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public HTTPPath                                     URLPathPrefix     { get; }

        public Boolean                                      DisableLogging    { get; }

        public String                                       LoggingPath       { get; }

        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<JObject>                     DebugLog          { get; }


        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient?                                   DNSClient         { get; }

        #endregion

        #region Events

        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new ();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new ();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new ();

        #endregion

        #endregion

        #region Constructor(s)

        public OICPWebAPI(IPPort?    HTTPPort         = null,
                          HTTPPath?  URLPathPrefix    = null,
                          Boolean?   DisableLogging   = null,
                          String?    LoggingPath      = null,
                          DNSClient? DNSClient        = null)
        {

            this.httpAPI         = new HTTPAPI(HTTPServerPort: HTTPPort ?? IPPort.Parse(9000));
            this.URLPathPrefix   = URLPathPrefix ?? DefaultURLPathPrefix;
            this.DNSClient       = DNSClient;

            // Logging
            this.DisableLogging  = this.DisableLogging == false;
            this.LoggingPath     = LoggingPath    ?? Path.Combine(AppContext.BaseDirectory, "OICPWebAPI");

            if (this.LoggingPath[^1] != Path.DirectorySeparatorChar)
                this.LoggingPath += Path.DirectorySeparatorChar;

            if (!this.DisableLogging)
                Directory.CreateDirectory(this.LoggingPath);


            // Link HTTP events...
            httpAPI.HTTPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            httpAPI.HTTPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            httpAPI.HTTPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            this.DebugLog        = httpAPI.AddJSONEventSource(EventIdentification:      DebugLogId,
                                                              URLTemplate:              this.URLPathPrefix + "/" + DebugLogId.ToString(),
                                                              MaxNumberOfCachedEvents:  10000,
                                                              RetryInterval:            TimeSpan.FromSeconds(5),
                                                              EnableLogging:            true,
                                                              LogfilePath:              this.LoggingPath);

        }

        #endregion


        #region RegisterAllEvents(...)

        public void RegisterAllEvents(EMPClient          EMPClient)
        {

            #region OnPullEVSEData

            EMPClient.OnPullEVSEDataHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullEVSEDataHTTPResponse",   httpRequest);

            EMPClient.OnPullEVSEDataRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPullEVSEDataRequest",        request. ToJSON(EMPClient.CustomPullEVSEDataRequestSerializer,
                                                                                                                                                                                 EMPClient.CustomGeoCoordinatesSerializer));

            EMPClient.OnPullEVSEDataResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullEVSEDataResponse",       response.ToJSON(request.           ToJSON(EMPClient.CustomPullEVSEDataRequestSerializer,
                                                                                                                                                                                                           EMPClient.CustomGeoCoordinatesSerializer),
                                                                                                                                                                                 response.Response?.ToJSON(//CustomPullEVSEDataResponseSerializer,
                                                                                                                                                                                                           //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                           //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                           //CustomDataCodeSerializer
                                                                                                                                                                                                           )));

            EMPClient.OnPullEVSEDataHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullEVSEDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPullEVSEStatus

            EMPClient.OnPullEVSEStatusHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullEVSEStatusHTTPResponse", httpRequest);

            EMPClient.OnPullEVSEStatusRequest      += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPullEVSEStatusRequest",      request. ToJSON(EMPClient.CustomPullEVSEStatusRequestSerializer,
                                                                                                                                                                                 EMPClient.CustomGeoCoordinatesSerializer));

            EMPClient.OnPullEVSEStatusResponse     += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullEVSEStatusResponse",     response.ToJSON(request.           ToJSON(EMPClient.CustomPullEVSEStatusRequestSerializer,
                                                                                                                                                                                                           EMPClient.CustomGeoCoordinatesSerializer),
                                                                                                                                                                                 response.Response?.ToJSON(//CustomPullEVSEStatusResponseSerializer,
                                                                                                                                                                                                           //CustomOperatorEVSEStatusSerializer,
                                                                                                                                                                                                           //CustomEVSEStatusRecordSerializer,
                                                                                                                                                                                                           //CustomStatusCodeSerializer
                                                                                                                                                                                                           )));

            EMPClient.OnPullEVSEStatusHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullEVSEStatusHTTPResponse", httpResponse);

            #endregion

            #region OnPullEVSEStatusById

            EMPClient.OnPullEVSEStatusByIdHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullEVSEStatusByIdHTTPResponse", httpRequest);

            EMPClient.OnPullEVSEStatusByIdRequest      += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPullEVSEStatusByIdRequest",      request. ToJSON(EMPClient.CustomPullEVSEStatusByIdRequestSerializer));

            EMPClient.OnPullEVSEStatusByIdResponse     += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullEVSEStatusByIdResponse",     response.ToJSON(request.           ToJSON(EMPClient.CustomPullEVSEStatusByIdRequestSerializer),
                                                                                                                                                                                         response.Response?.ToJSON(//CustomPullEVSEStatusByIdResponseSerializer,
                                                                                                                                                                                                                   //CustomOperatorEVSEStatusByIdSerializer,
                                                                                                                                                                                                                   //CustomEVSEStatusByIdRecordSerializer,
                                                                                                                                                                                                                   //CustomStatusByIdCodeSerializer
                                                                                                                                                                                                                   )));

            EMPClient.OnPullEVSEStatusByIdHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullEVSEStatusByIdHTTPResponse", httpResponse);

            #endregion

            #region OnPullEVSEStatusByOperatorId

            EMPClient.OnPullEVSEStatusByOperatorIdHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdHTTPResponse", httpRequest);

            EMPClient.OnPullEVSEStatusByOperatorIdRequest      += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdRequest",      request. ToJSON(EMPClient.CustomPullEVSEStatusByOperatorIdRequestSerializer));

            EMPClient.OnPullEVSEStatusByOperatorIdResponse     += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdResponse",     response.ToJSON(request.ToJSON(EMPClient.CustomPullEVSEStatusByOperatorIdRequestSerializer),
                                                                                                                                                                                                         response.Response?.ToJSON(//CustomPullEVSEStatusByOperatorIdResponseSerializer,
                                                                                                                                                                                                                                   //CustomOperatorEVSEStatusByOperatorIdSerializer,
                                                                                                                                                                                                                                   //CustomEVSEStatusByOperatorIdRecordSerializer,
                                                                                                                                                                                                                                   //CustomStatusByOperatorIdCodeSerializer
                                                                                                                                                                                                                                   )));

            EMPClient.OnPullEVSEStatusByOperatorIdHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdHTTPResponse", httpResponse);

            #endregion


            #region OnPullPricingProductData

            EMPClient.OnPullPricingProductDataHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullPricingProductDataHTTPResponse",   httpRequest);

            EMPClient.OnPullPricingProductDataRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPullPricingProductDataRequest",        request. ToJSON(EMPClient.CustomPullPricingProductDataRequestSerializer));

            EMPClient.OnPullPricingProductDataResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullPricingProductDataResponse",       response.ToJSON(request.           ToJSON(EMPClient.CustomPullPricingProductDataRequestSerializer),
                                                                                                                                                                                     response.Response?.ToJSON(//CustomPullPricingProductDataResponseSerializer,
                                                                                                                                                                                                               //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                               //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                               //CustomDataCodeSerializer
                                                                                                                                                                                                               )));

            EMPClient.OnPullPricingProductDataHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullPricingProductDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPullEVSEPricing

            EMPClient.OnPullEVSEPricingHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullEVSEPricingHTTPResponse",   httpRequest);

            EMPClient.OnPullEVSEPricingRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPullEVSEPricingRequest",        request. ToJSON(EMPClient.CustomPullEVSEPricingRequestSerializer));

            EMPClient.OnPullEVSEPricingResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullEVSEPricingResponse",       response.ToJSON(request.           ToJSON(EMPClient.CustomPullEVSEPricingRequestSerializer),
                                                                                                                                                                                       response.Response?.ToJSON(//CustomPullEVSEPricingResponseSerializer,
                                                                                                                                                                                                                 //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                 //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                 //CustomDataCodeSerializer
                                                                                                                                                                                                                 )));

            EMPClient.OnPullEVSEPricingHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullEVSEPricingHTTPResponse",   httpResponse);

            #endregion


            #region OnPushAuthenticationData

            EMPClient.OnPushAuthenticationDataHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPushAuthenticationDataHTTPResponse",   httpRequest);

            EMPClient.OnPushAuthenticationDataRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnPushAuthenticationDataRequest",        request. ToJSON(EMPClient.CustomPushAuthenticationDataRequestSerializer));

            EMPClient.OnPushAuthenticationDataResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnPushAuthenticationDataResponse",       response.ToJSON(request.ToJSON(EMPClient.CustomPushAuthenticationDataRequestSerializer),
                                                                                                                                                                                     response.Response?.ToJSON(//CustomPushAuthenticationDataResponseSerializer,
                                                                                                                                                                                                               //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                               //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                               //CustomDataCodeSerializer
                                                                                                                                                                                                               )));

            EMPClient.OnPushAuthenticationDataHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPushAuthenticationDataHTTPResponse",   httpResponse);

            #endregion


            #region OnAuthorizeRemoteReservationStart

            EMPClient.OnAuthorizeRemoteReservationStartHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartHTTPResponse",   httpRequest);

            EMPClient.OnAuthorizeRemoteReservationStartRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartRequest",        request. ToJSON(EMPClient.CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                                                                                                    EMPClient.CustomIdentificationSerializer));

            EMPClient.OnAuthorizeRemoteReservationStartResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartResponse",       response.ToJSON(request.           ToJSON(EMPClient.CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                                                                                                                                 EMPClient.CustomIdentificationSerializer),
                                                                                                                                                                                                       response.Response?.ToJSON(//CustomAuthorizeRemoteReservationStartResponseSerializer,
                                                                                                                                                                                                                                 //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                                 //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                                 //CustomDataCodeSerializer
                                                                                                                                                                                                                                 )));

            EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteReservationStop

            EMPClient.OnAuthorizeRemoteReservationStopHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopHTTPResponse",   httpRequest);

            EMPClient.OnAuthorizeRemoteReservationStopRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopRequest",        request. ToJSON(EMPClient.CustomAuthorizeRemoteReservationStopRequestSerializer));

            EMPClient.OnAuthorizeRemoteReservationStopResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopResponse",       response.ToJSON(request.           ToJSON(EMPClient.CustomAuthorizeRemoteReservationStopRequestSerializer),
                                                                                                                                                                                                     response.Response?.ToJSON(//CustomAuthorizeRemoteReservationStopResponseSerializer,
                                                                                                                                                                                                                               //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                               //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                               //CustomDataCodeSerializer
                                                                                                                                                                                                                               )));

            EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteStart

            EMPClient.OnAuthorizeRemoteStartHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnAuthorizeRemoteStartHTTPResponse",   httpRequest);

            EMPClient.OnAuthorizeRemoteStartRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnAuthorizeRemoteStartRequest",        request. ToJSON(EMPClient.CustomAuthorizeRemoteStartRequestSerializer,
                                                                                                                                                                                                 EMPClient.CustomIdentificationSerializer));

            EMPClient.OnAuthorizeRemoteStartResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnAuthorizeRemoteStartResponse",       response.ToJSON(request.           ToJSON(EMPClient.CustomAuthorizeRemoteStartRequestSerializer,
                                                                                                                                                                                                                           EMPClient.CustomIdentificationSerializer),
                                                                                                                                                                                 response.Response?.ToJSON(//CustomAuthorizeRemoteStartResponseSerializer,
                                                                                                                                                                                                           //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                           //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                           //CustomDataCodeSerializer
                                                                                                                                                                                                           )));

            EMPClient.OnAuthorizeRemoteStartHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnAuthorizeRemoteStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteStop

            EMPClient.OnAuthorizeRemoteStopHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnAuthorizeRemoteStopHTTPResponse",   httpRequest);

            EMPClient.OnAuthorizeRemoteStopRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnAuthorizeRemoteStopRequest",        request. ToJSON(EMPClient.CustomAuthorizeRemoteStopRequestSerializer));

            EMPClient.OnAuthorizeRemoteStopResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnAuthorizeRemoteStopResponse",       response.ToJSON(request.ToJSON(EMPClient.CustomAuthorizeRemoteStopRequestSerializer),
                                                                                                                                                                               response.Response?.ToJSON(//CustomAuthorizeRemoteStopResponseSerializer,
                                                                                                                                                                                                         //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                         //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                         //CustomDataCodeSerializer
                                                                                                                                                                                                         )));

            EMPClient.OnAuthorizeRemoteStopHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnAuthorizeRemoteStopHTTPResponse",   httpResponse);

            #endregion


            #region OnGetChargeDetailRecords

            EMPClient.OnGetChargeDetailRecordsHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnGetChargeDetailRecordsHTTPResponse",   httpRequest);

            EMPClient.OnGetChargeDetailRecordsRequest        += (timestamp, empClient, request)                    => DebugLog.SubmitEvent("OnGetChargeDetailRecordsRequest",        request. ToJSON(EMPClient.CustomGetChargeDetailRecordsRequestSerializer));

            EMPClient.OnGetChargeDetailRecordsResponse       += (timestamp, empClient, request, response, runtime) => DebugLog.SubmitEvent("OnGetChargeDetailRecordsResponse",       response.ToJSON(request.ToJSON(EMPClient.CustomGetChargeDetailRecordsRequestSerializer),
                                                                                                                                                                                     response.Response?.ToJSON(//CustomGetChargeDetailRecordsResponseSerializer,
                                                                                                                                                                                                               //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                               //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                               //CustomDataCodeSerializer
                                                                                                                                                                                                               )));

            EMPClient.OnGetChargeDetailRecordsHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnGetChargeDetailRecordsHTTPResponse",   httpResponse);

            #endregion

        }

        public void RegisterAllEvents(EMPServerAPI       EMPServerAPI)
        {

            #region OnAuthorizeStart

            EMPServerAPI.OnAuthorizeStartHTTPRequest      += (timestamp, httpAPI, httpRequest)                 => DebugLog.SubmitEvent("OnAuthorizeStartHTTPResponse",   httpRequest);

            EMPServerAPI.OnAuthorizeStartRequest          += (timestamp, httpAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeStartRequest",        request.ToJSON(//EMPServerAPI.CustomAuthorizeStartRequestSerializer,
                                                                                                                                                                                        //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                        ));

            EMPServerAPI.OnAuthorizeStartResponse         += (timestamp, httpAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeStartResponse",       new JObject(
                                                                                                                                                                             new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomAuthorizeStartRequestSerializer,
                                                                                                                                                                                                                                //EMPServerAPI.CustomId
                                                                                                                                                                                                                                )),
                                                                                                                                                                             new JProperty("response", response.ToJSON(EMPServerAPI.CustomAuthorizationStartSerializer,
                                                                                                                                                                                                                       EMPServerAPI.CustomStatusCodeSerializer,
                                                                                                                                                                                                                       EMPServerAPI.CustomIdentificationSerializer))
                                                                                                                                                                         ));

            EMPServerAPI.OnAuthorizationStartHTTPResponse += (timestamp, httpAPI, httpRequest, httpResponse)   => DebugLog.SubmitEvent("OnAuthorizeStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeStop

            EMPServerAPI.OnAuthorizeStopHTTPRequest      += (timestamp, httpAPI, httpRequest)                 => DebugLog.SubmitEvent("OnAuthorizeStopHTTPResponse",   httpRequest);

            EMPServerAPI.OnAuthorizeStopRequest          += (timestamp, httpAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeStopRequest",        request.ToJSON(//EMPServerAPI.CustomAuthorizeStopRequestSerializer,
                                                                                                                                                                                      //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                      ));

            EMPServerAPI.OnAuthorizeStopResponse         += (timestamp, httpAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeStopResponse",       new JObject(
                                                                                                                                                                           new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomAuthorizeStopRequestSerializer,
                                                                                                                                                                                                                              //EMPServerAPI.CustomId
                                                                                                                                                                                                                              )),
                                                                                                                                                                           new JProperty("response", response.ToJSON(EMPServerAPI.CustomAuthorizationStopSerializer,
                                                                                                                                                                                                                     EMPServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                     ));

            EMPServerAPI.OnAuthorizationStopHTTPResponse += (timestamp, httpAPI, httpRequest, httpResponse)   => DebugLog.SubmitEvent("OnAuthorizeStopHTTPResponse",   httpResponse);

            #endregion


            #region OnChargingNotifications

            EMPServerAPI.OnChargingNotificationsHTTPRequest  += (timestamp, httpAPI, httpRequest)                    => DebugLog.SubmitEvent("OnChargingNotificationsHTTPResponse",     httpRequest);


            EMPServerAPI.OnChargingStartNotificationRequest  += (timestamp, httpAPI, request)                        => DebugLog.SubmitEvent("OnChargingStartNotificationRequest",      request.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                       //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                       ));

            EMPServerAPI.OnChargingStartNotificationResponse += (timestamp, httpAPI, request, response, runtime)     => DebugLog.SubmitEvent("OnChargingStartNotificationResponse",     new JObject(
                                                                                                                                                                                            new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                                                               //EMPServerAPI.CustomId
                                                                                                                                                                                                                                               )),
                                                                                                                                                                                            new JProperty("response", response.ToJSON(EMPServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                      EMPServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                        ));


            EMPServerAPI.OnChargingProgressNotificationRequest  += (timestamp, httpAPI, request)                     => DebugLog.SubmitEvent("OnChargingProgressNotificationRequest",   request.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                       //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                       ));

            EMPServerAPI.OnChargingProgressNotificationResponse += (timestamp, httpAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargingProgressNotificationResponse",  new JObject(
                                                                                                                                                                                            new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                                                               //EMPServerAPI.CustomId
                                                                                                                                                                                                                                               )),
                                                                                                                                                                                            new JProperty("response", response.ToJSON(EMPServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                      EMPServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                        ));


            EMPServerAPI.OnChargingEndNotificationRequest  += (timestamp, httpAPI, request)                          => DebugLog.SubmitEvent("OnChargingEndNotificationRequest",          request.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                         //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                         ));

            EMPServerAPI.OnChargingEndNotificationResponse += (timestamp, httpAPI, request, response, runtime)       => DebugLog.SubmitEvent("OnChargingEndNotificationResponse",         new JObject(
                                                                                                                                                                                              new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                                                                 //EMPServerAPI.CustomId
                                                                                                                                                                                                                                                 )),
                                                                                                                                                                                              new JProperty("response", response.ToJSON(EMPServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                        EMPServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                          ));


            EMPServerAPI.OnChargingErrorNotificationRequest  += (timestamp, httpAPI, request)                        => DebugLog.SubmitEvent("OnChargingErrorNotificationRequest",      request.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                       //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                       ));

            EMPServerAPI.OnChargingErrorNotificationResponse += (timestamp, httpAPI, request, response, runtime)     => DebugLog.SubmitEvent("OnChargingErrorNotificationResponse",     new JObject(
                                                                                                                                                                                            new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomChargingNotificationsRequestSerializer,
                                                                                                                                                                                                                                               //EMPServerAPI.CustomId
                                                                                                                                                                                                                                               )),
                                                                                                                                                                                            new JProperty("response", response.ToJSON(EMPServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                      EMPServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                        ));


            EMPServerAPI.OnAuthorizationStopHTTPResponse     += (timestamp, httpAPI, httpRequest, httpResponse)      => DebugLog.SubmitEvent("OnChargingNotificationsHTTPResponse",     httpResponse);

            #endregion


            #region OnChargeDetailRecord

            EMPServerAPI.OnChargeDetailRecordHTTPRequest  += (timestamp, httpAPI, httpRequest)                 => DebugLog.SubmitEvent("OnChargeDetailRecordHTTPResponse",   httpRequest);

            EMPServerAPI.OnChargeDetailRecordRequest      += (timestamp, httpAPI, request)                     => DebugLog.SubmitEvent("OnChargeDetailRecordRequest",        request.ToJSON(//EMPServerAPI.CustomChargeDetailRecordRequestSerializer,
                                                                                                                                                                                            //EMPServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                            ));

            EMPServerAPI.OnChargeDetailRecordResponse     += (timestamp, httpAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargeDetailRecordResponse",       new JObject(
                                                                                                                                                                                 new JProperty("request",  response.Request?.ToJSON(//EMPServerAPI.CustomChargeDetailRecordRequestSerializer,
                                                                                                                                                                                                                                    //EMPServerAPI.CustomId
                                                                                                                                                                                                                                    )),
                                                                                                                                                                                 new JProperty("response", response.ToJSON(EMPServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                           EMPServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                             ));

            EMPServerAPI.OnAuthorizationStartHTTPResponse += (timestamp, httpAPI, httpRequest, httpResponse)   => DebugLog.SubmitEvent("OnChargeDetailRecordHTTPResponse",   httpResponse);

            #endregion

        }

        public void RegisterAllEvents(CPOClient          CPOClient)
        {

            #region OnPushEVSEData

            CPOClient.OnPushEVSEDataHTTPRequest    += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPushEVSEDataHTTPResponse",     httpRequest);

            CPOClient.OnPushEVSEDataRequest        += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnPushEVSEDataRequest",          request. ToJSON(CPOClient.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                   CPOClient.CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                   CPOClient.CustomEVSEDataRecordSerializer,
                                                                                                                                                                                   CPOClient.CustomAddressSerializer,
                                                                                                                                                                                   CPOClient.CustomChargingFacilitySerializer,
                                                                                                                                                                                   CPOClient.CustomGeoCoordinatesSerializer,
                                                                                                                                                                                   CPOClient.CustomEnergyMeterSerializer,
                                                                                                                                                                                   CPOClient.CustomTransparencySoftwareStatusSerializer,
                                                                                                                                                                                   CPOClient.CustomTransparencySoftwareSerializer,
                                                                                                                                                                                   CPOClient.CustomEnergySourceSerializer,
                                                                                                                                                                                   CPOClient.CustomEnvironmentalImpactSerializer,
                                                                                                                                                                                   CPOClient.CustomOpeningTimesSerializer));

            CPOClient.OnPushEVSEDataResponse       += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnPushEVSEDataResponse",         response.ToJSON(request.           ToJSON(CPOClient.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                                             CPOClient.CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                             CPOClient.CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                             CPOClient.CustomAddressSerializer,
                                                                                                                                                                                                             CPOClient.CustomChargingFacilitySerializer,
                                                                                                                                                                                                             CPOClient.CustomGeoCoordinatesSerializer,
                                                                                                                                                                                                             CPOClient.CustomEnergyMeterSerializer,
                                                                                                                                                                                                             CPOClient.CustomTransparencySoftwareStatusSerializer,
                                                                                                                                                                                                             CPOClient.CustomTransparencySoftwareSerializer,
                                                                                                                                                                                                             CPOClient.CustomEnergySourceSerializer,
                                                                                                                                                                                                             CPOClient.CustomEnvironmentalImpactSerializer,
                                                                                                                                                                                                             CPOClient.CustomOpeningTimesSerializer),
                                                                                                                                                                                   response.Response?.ToJSON(//CustomPushEVSEDataResponseSerializer,
                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                             )));

            CPOClient.OnPushEVSEDataHTTPResponse   += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPushEVSEDataHTTPResponse",     httpResponse);

            #endregion

            #region OnPushEVSEStatus

            CPOClient.OnPushEVSEStatusHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPushEVSEStatusHTTPResponse",   httpRequest);

            CPOClient.OnPushEVSEStatusRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnPushEVSEStatusRequest",        request. ToJSON(CPOClient.CustomPushEVSEStatusRequestSerializer,
                                                                                                                                                                                   CPOClient.CustomOperatorEVSEStatusSerializer,
                                                                                                                                                                                   CPOClient.CustomEVSEStatusRecordSerializer));

            CPOClient.OnPushEVSEStatusResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnPushEVSEStatusResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomPushEVSEStatusRequestSerializer,
                                                                                                                                                                                                             CPOClient.CustomOperatorEVSEStatusSerializer,
                                                                                                                                                                                                             CPOClient.CustomEVSEStatusRecordSerializer),
                                                                                                                                                                                   response.Response?.ToJSON(//CustomPushEVSEStatusResponseSerializer,
                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                             )));

            CPOClient.OnPushEVSEStatusHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPushEVSEStatusHTTPResponse",   httpResponse);

            #endregion


            #region OnPushPricingProductData

            CPOClient.OnPushPricingProductDataHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPushPricingProductDataHTTPResponse",   httpRequest);

            CPOClient.OnPushPricingProductDataRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnPushPricingProductDataRequest",        request. ToJSON(CPOClient.CustomPushPricingProductDataRequestSerializer,
                                                                                                                                                                                                   CPOClient.CustomPricingProductDataSerializer,
                                                                                                                                                                                                   CPOClient.CustomPricingProductDataRecordSerializer));

            CPOClient.OnPushPricingProductDataResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnPushPricingProductDataResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomPushPricingProductDataRequestSerializer,
                                                                                                                                                                                                                             CPOClient.CustomPricingProductDataSerializer,
                                                                                                                                                                                                                             CPOClient.CustomPricingProductDataRecordSerializer),
                                                                                                                                                                                   response.Response?.ToJSON(//CustomPushPricingProductDataResponseSerializer,
                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                             )));

            CPOClient.OnPushPricingProductDataHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPushPricingProductDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPushEVSEPricing

            CPOClient.OnPushEVSEPricingHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPushEVSEPricingHTTPResponse",   httpRequest);

            CPOClient.OnPushEVSEPricingRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnPushEVSEPricingRequest",        request. ToJSON(CPOClient.CustomPushEVSEPricingRequestSerializer,
                                                                                                                                                                                     CPOClient.CustomEVSEPricingSerializer));

            CPOClient.OnPushEVSEPricingResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnPushEVSEPricingResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomPushEVSEPricingRequestSerializer,
                                                                                                                                                                                                               CPOClient.CustomEVSEPricingSerializer),
                                                                                                                                                                                     response.Response?.ToJSON(//CustomPushEVSEPricingResponseSerializer,
                                                                                                                                                                                                               //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                               //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                               //CustomDataCodeSerializer
                                                                                                                                                                                                               )));

            CPOClient.OnPushEVSEPricingHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPushEVSEPricingHTTPResponse",   httpResponse);

            #endregion


            #region OnPullAuthenticationData

            CPOClient.OnPullAuthenticationDataHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnPullAuthenticationDataHTTPResponse",   httpRequest);

            CPOClient.OnPullAuthenticationDataRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnPullAuthenticationDataRequest",        request. ToJSON(CPOClient.CustomPullAuthenticationDataRequestSerializer));

            CPOClient.OnPullAuthenticationDataResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnPullAuthenticationDataResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomPullAuthenticationDataRequestSerializer),
                                                                                                                                                                                                   response.Response?.ToJSON(//CustomPullAuthenticationDataResponseSerializer,
                                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                                             )));

            CPOClient.OnPullAuthenticationDataHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnPullAuthenticationDataHTTPResponse",   httpResponse);

            #endregion


            #region OnAuthorizeStart

            CPOClient.OnAuthorizeStartHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnAuthorizeStartHTTPResponse",   httpRequest);

            CPOClient.OnAuthorizeStartRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnAuthorizeStartRequest",        request. ToJSON(CPOClient.CustomAuthorizeStartRequestSerializer,
                                                                                                                                                                                   CPOClient.CustomIdentificationSerializer));

            CPOClient.OnAuthorizeStartResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnAuthorizeStartResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomAuthorizeStartRequestSerializer,
                                                                                                                                                                                                             CPOClient.CustomIdentificationSerializer),
                                                                                                                                                                                   response.Response?.ToJSON(//CustomAuthorizeStartResponseSerializer,
                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                             )));

            CPOClient.OnAuthorizeStartHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnAuthorizeStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeStop

            CPOClient.OnAuthorizeStopHTTPRequest   += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnAuthorizeStopHTTPResponse",    httpRequest);

            CPOClient.OnAuthorizeStopRequest       += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnAuthorizeStopRequest",         request. ToJSON(CPOClient.CustomAuthorizeStopRequestSerializer,
                                                                                                                                                                                   CPOClient.CustomIdentificationSerializer));

            CPOClient.OnAuthorizeStopResponse      += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnAuthorizeStopResponse",        response.ToJSON(request.           ToJSON(CPOClient.CustomAuthorizeStopRequestSerializer,
                                                                                                                                                                                                             CPOClient.CustomIdentificationSerializer),
                                                                                                                                                                                   response.Response?.ToJSON(//CustomAuthorizeStopResponseSerializer,
                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                             )));

            CPOClient.OnAuthorizeStopHTTPResponse  += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnAuthorizeStopHTTPResponse",    httpResponse);

            #endregion


            #region OnChargingStartNotification

            CPOClient.OnChargingStartNotificationHTTPRequest     += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnChargingStartNotificationHTTPResponse",      httpRequest);

            CPOClient.OnChargingStartNotificationRequest         += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnChargingStartNotificationRequest",           request. ToJSON(CPOClient.CustomChargingStartNotificationRequestSerializer,
                                                                                                                                                                                                               CPOClient.CustomIdentificationSerializer));

            CPOClient.OnChargingStartNotificationResponse        += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnChargingStartNotificationResponse",          response.ToJSON(request.           ToJSON(CPOClient.CustomChargingStartNotificationRequestSerializer,
                                                                                                                                                                                                                                         CPOClient.CustomIdentificationSerializer),
                                                                                                                                                                                                               response.Response?.ToJSON(//CustomChargingStartNotificationResponseSerializer,
                                                                                                                                                                                                                                         //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                                         //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                                         //CustomDataCodeSerializer
                                                                                                                                                                                                                                         )));

            CPOClient.OnChargingStartNotificationHTTPResponse    += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnChargingStartNotificationHTTPResponse",      httpResponse);

            #endregion

            #region OnChargingProgressNotification

            CPOClient.OnChargingProgressNotificationHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnChargingProgressNotificationHTTPResponse",   httpRequest);

            CPOClient.OnChargingProgressNotificationRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnChargingProgressNotificationRequest",        request. ToJSON(CPOClient.CustomChargingProgressNotificationRequestSerializer,
                                                                                                                                                                                                               CPOClient.CustomIdentificationSerializer));

            CPOClient.OnChargingProgressNotificationResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnChargingProgressNotificationResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomChargingProgressNotificationRequestSerializer,
                                                                                                                                                                                                                                         CPOClient.CustomIdentificationSerializer),
                                                                                                                                                                                                               response.Response?.ToJSON(//CustomChargingProgressNotificationResponseSerializer,
                                                                                                                                                                                                                                         //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                                         //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                                         //CustomDataCodeSerializer
                                                                                                                                                                                                                                         )));

            CPOClient.OnChargingProgressNotificationHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnChargingProgressNotificationHTTPResponse",   httpResponse);

            #endregion

            #region OnChargingEndNotification

            CPOClient.OnChargingEndNotificationHTTPRequest       += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnChargingEndNotificationHTTPResponse",        httpRequest);

            CPOClient.OnChargingEndNotificationRequest           += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnChargingEndNotificationRequest",             request. ToJSON(CPOClient.CustomChargingEndNotificationRequestSerializer,
                                                                                                                                                                                                               CPOClient.CustomIdentificationSerializer));

            CPOClient.OnChargingEndNotificationResponse          += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnChargingEndNotificationResponse",            response.ToJSON(request.           ToJSON(CPOClient.CustomChargingEndNotificationRequestSerializer,
                                                                                                                                                                                                                                         CPOClient.CustomIdentificationSerializer),
                                                                                                                                                                                                               response.Response?.ToJSON(//CustomChargingEndNotificationResponseSerializer,
                                                                                                                                                                                                                                         //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                                         //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                                         //CustomDataCodeSerializer
                                                                                                                                                                                                                                         )));

            CPOClient.OnChargingEndNotificationHTTPResponse      += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnChargingEndNotificationHTTPResponse",        httpResponse);

            #endregion

            #region OnChargingErrorNotification

            CPOClient.OnChargingErrorNotificationHTTPRequest     += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnChargingErrorNotificationHTTPResponse",      httpRequest);

            CPOClient.OnChargingErrorNotificationRequest         += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnChargingErrorNotificationRequest",           request. ToJSON(CPOClient.CustomChargingErrorNotificationRequestSerializer,
                                                                                                                                                                                                               CPOClient.CustomIdentificationSerializer));

            CPOClient.OnChargingErrorNotificationResponse        += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnChargingErrorNotificationResponse",          response.ToJSON(request.           ToJSON(CPOClient.CustomChargingErrorNotificationRequestSerializer,
                                                                                                                                                                                                                                         CPOClient.CustomIdentificationSerializer),
                                                                                                                                                                                                               response.Response?.ToJSON(//CustomChargingErrorNotificationResponseSerializer,
                                                                                                                                                                                                                                         //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                                         //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                                         //CustomDataCodeSerializer
                                                                                                                                                                                                                                         )));

            CPOClient.OnChargingErrorNotificationHTTPResponse    += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnChargingErrorNotificationHTTPResponse",      httpResponse);

            #endregion


            #region OnSendChargeDetailRecord

            CPOClient.OnSendChargeDetailRecordHTTPRequest  += (timestamp, httpClient, httpRequest)               => DebugLog.SubmitEvent("OnSendChargeDetailRecordHTTPResponse",   httpRequest);

            CPOClient.OnSendChargeDetailRecordRequest      += (timestamp, cpoClient, request)                    => DebugLog.SubmitEvent("OnSendChargeDetailRecordRequest",        request. ToJSON(CPOClient.CustomChargeDetailRecordRequestSerializer,
                                                                                                                                                                                                   CPOClient.CustomChargeDetailRecordSerializer,
                                                                                                                                                                                                   CPOClient.CustomIdentificationSerializer,
                                                                                                                                                                                                   CPOClient.CustomSignedMeteringValueSerializer,
                                                                                                                                                                                                   CPOClient.CustomCalibrationLawVerificationSerializer));

            CPOClient.OnSendChargeDetailRecordResponse     += (timestamp, cpoClient, request, response, runtime) => DebugLog.SubmitEvent("OnSendChargeDetailRecordResponse",       response.ToJSON(request.           ToJSON(CPOClient.CustomChargeDetailRecordRequestSerializer,
                                                                                                                                                                                                                             CPOClient.CustomChargeDetailRecordSerializer,
                                                                                                                                                                                                                             CPOClient.CustomIdentificationSerializer,
                                                                                                                                                                                                                             CPOClient.CustomSignedMeteringValueSerializer,
                                                                                                                                                                                                                             CPOClient.CustomCalibrationLawVerificationSerializer),
                                                                                                                                                                                                   response.Response?.ToJSON(//CustomSendChargeDetailRecordResponseSerializer,
                                                                                                                                                                                                                             //CustomOperatorEVSEDataSerializer,
                                                                                                                                                                                                                             //CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                                             //CustomDataCodeSerializer
                                                                                                                                                                                                                             )));

            CPOClient.OnSendChargeDetailRecordHTTPResponse += (timestamp, httpClient, httpRequest, httpResponse) => DebugLog.SubmitEvent("OnSendChargeDetailRecordHTTPResponse",   httpResponse);

            #endregion

        }

        public void RegisterAllEvents(CPOServerAPI       CPOServerAPI)
        {

            #region OnAuthorizeRemoteReservationStart

            CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartHTTPResponse",   httpRequest);

            CPOServerAPI.OnAuthorizeRemoteReservationStartRequest       += (timestamp, cpoServerAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartRequest",        request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                                                                                                            //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                            ));

            CPOServerAPI.OnAuthorizeRemoteReservationStartResponse      += (timestamp, cpoServerAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartResponse",       new JObject(
                                                                                                                                                                                                                 new JProperty("request",  response.Request?.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                                                                                                                                                    //CPOServerAPI.CustomId
                                                                                                                                                                                                                                                                    )),
                                                                                                                                                                                                                 new JProperty("response", response.ToJSON(CPOServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                                           CPOServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                                             ));

            CPOServerAPI.OnAuthorizeRemoteReservationStartHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteReservationStop

            CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPRequest    += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopHTTPResponse",    httpRequest);

            CPOServerAPI.OnAuthorizeRemoteReservationStopRequest        += (timestamp, cpoServerAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopRequest",         request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteReservationStopRequestSerializer,
                                                                                                                                                                                                                              //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ));

            CPOServerAPI.OnAuthorizeRemoteReservationStopResponse       += (timestamp, cpoServerAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopResponse",        new JObject(
                                                                                                                                                                                                                 new JProperty("request",  response.Request?.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteReservationStopRequestSerializer,
                                                                                                                                                                                                                                                                    //CPOServerAPI.CustomId
                                                                                                                                                                                                                                                                    )),
                                                                                                                                                                                                                 new JProperty("response", response.ToJSON(CPOServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                                           CPOServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                                             ));

            CPOServerAPI.OnAuthorizeRemoteReservationStopHTTPResponse   += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopHTTPResponse",    httpResponse);

            #endregion


            #region OnAuthorizeRemoteStart

            CPOServerAPI.OnAuthorizeRemoteStartHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteStartHTTPResponse",   httpRequest);

            CPOServerAPI.OnAuthorizeRemoteStartRequest       += (timestamp, cpoServerAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteStartRequest",        request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteStartRequestSerializer,
                                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                      ));

            CPOServerAPI.OnAuthorizeRemoteStartResponse      += (timestamp, cpoServerAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteStartResponse",       new JObject(
                                                                                                                                                                                           new JProperty("request",  response.Request?.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteStartRequestSerializer,
                                                                                                                                                                                                                                              //CPOServerAPI.CustomId
                                                                                                                                                                                                                                              )),
                                                                                                                                                                                           new JProperty("response", response.ToJSON(CPOServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                     CPOServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                       ));

            CPOServerAPI.OnAuthorizeRemoteStartHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteStop

            CPOServerAPI.OnAuthorizeRemoteStopHTTPRequest    += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteStopHTTPResponse",    httpRequest);

            CPOServerAPI.OnAuthorizeRemoteStopRequest        += (timestamp, cpoServerAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteStopRequest",         request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteStopRequestSerializer,
                                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                      ));

            CPOServerAPI.OnAuthorizeRemoteStopResponse       += (timestamp, cpoServerAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteStopResponse",        new JObject(
                                                                                                                                                                                           new JProperty("request",  response.Request?.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteStopRequestSerializer,
                                                                                                                                                                                                                                              //CPOServerAPI.CustomId
                                                                                                                                                                                                                                              )),
                                                                                                                                                                                           new JProperty("response", response.ToJSON(CPOServerAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                     CPOServerAPI.CustomStatusCodeSerializer))
                                                                                                                                                                                       ));

            CPOServerAPI.OnAuthorizeRemoteStopHTTPResponse   += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteStopHTTPResponse",    httpResponse);

            #endregion

        }

        public void RegisterAllEvents(CentralServiceAPI  CentralServiceAPI)
        {
            RegisterAllEvents(CentralServiceAPI.EMPClientAPI);
            RegisterAllEvents(CentralServiceAPI.CPOClientAPI);
        }

        public void RegisterAllEvents(EMPClientAPI       EMPClientAPI)
        {

            #region OnPullEVSEData

            EMPClientAPI.OnPullEVSEDataHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullEVSEDataHTTPRequest",    httpRequest);

            EMPClientAPI.OnPullEVSEDataRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPullEVSEDataRequest",        request.ToJSON(//CPOServerAPI.CustomPullEVSEDataRequestSerializer,
                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                      ));

            EMPClientAPI.OnPullEVSEDataResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullEVSEDataResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//EMPClientAPI.CustomPullEVSEDataRequestSerializer
                                                                                                                                                                                                          //EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ),
                                                                                                                                                                       response.Response?.ToJSON(EMPClientAPI.CustomPullEVSEDataResponseSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomEVSEDataRecordSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomAddressSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomChargingFacilitySerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomGeoCoordinatesSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomEnergyMeterSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomTransparencySoftwareStatusSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomTransparencySoftwareSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomEnergySourceSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomEnvironmentalImpactSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomOpeningTimesSerializer,
                                                                                                                                                                                                 EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnPullEVSEDataHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullEVSEDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPullEVSEStatus

            EMPClientAPI.OnPullEVSEStatusHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullEVSEStatusHTTPRequest",    httpRequest);

            EMPClientAPI.OnPullEVSEStatusRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPullEVSEStatusRequest",        request.ToJSON(//CPOServerAPI.CustomPullEVSEStatusRequestSerializer,
                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                      ));

            EMPClientAPI.OnPullEVSEStatusResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullEVSEStatusResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomPullEVSEStatusRequestSerializer
                                                                                                                                                                                                              //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                              ),
                                                                                                                                                                           response.Response?.ToJSON(EMPClientAPI.CustomPullEVSEStatusResponseSerializer,
                                                                                                                                                                                                     EMPClientAPI.CustomOperatorEVSEStatusSerializer,
                                                                                                                                                                                                     EMPClientAPI.CustomEVSEStatusRecordSerializer,
                                                                                                                                                                                                     EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnPullEVSEStatusHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullEVSEStatusHTTPResponse",   httpResponse);

            #endregion

            #region OnPullEVSEStatusById

            EMPClientAPI.OnPullEVSEStatusByIdHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullEVSEStatusByIdHTTPRequest",    httpRequest);

            EMPClientAPI.OnPullEVSEStatusByIdRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPullEVSEStatusByIdRequest",        request.ToJSON(//CPOServerAPI.CustomPullEVSEStatusByIdRequestSerializer,
                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                      ));

            EMPClientAPI.OnPullEVSEStatusByIdResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullEVSEStatusByIdResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomPullEVSEStatusByIdRequestSerializer
                                                                                                                                                                                                                      //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                      ),
                                                                                                                                                                                   response.Response?.ToJSON(EMPClientAPI.CustomPullEVSEStatusByIdResponseSerializer,
                                                                                                                                                                                                             EMPClientAPI.CustomEVSEStatusRecordSerializer)));

            EMPClientAPI.OnPullEVSEStatusByIdHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullEVSEStatusByIdHTTPResponse",   httpResponse);

            #endregion

            #region OnPullEVSEStatusByOperatorId

            EMPClientAPI.OnPullEVSEStatusByOperatorIdHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdHTTPRequest",    httpRequest);

            EMPClientAPI.OnPullEVSEStatusByOperatorIdRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdRequest",        request.ToJSON(//CPOServerAPI.CustomPullEVSEStatusByOperatorIdRequestSerializer,
                                                                                                                                                                                                                  //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                  ));

            EMPClientAPI.OnPullEVSEStatusByOperatorIdResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomPullEVSEStatusByOperatorIdRequestSerializer
                                                                                                                                                                                                                                      //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                      ),
                                                                                                                                                                                                   response.Response?.ToJSON(EMPClientAPI.CustomPullEVSEStatusByOperatorIdResponseSerializer,
                                                                                                                                                                                                                             EMPClientAPI.CustomOperatorEVSEStatusSerializer,
                                                                                                                                                                                                                             EMPClientAPI.CustomEVSEStatusRecordSerializer,
                                                                                                                                                                                                                             EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnPullEVSEStatusByOperatorIdHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullEVSEStatusByOperatorIdHTTPResponse",   httpResponse);

            #endregion


            #region OnPullPricingProductData

            EMPClientAPI.OnPullPricingProductDataHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullPricingProductDataHTTPRequest",    httpRequest);

            EMPClientAPI.OnPullPricingProductDataRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPullPricingProductDataRequest",        request.ToJSON(//CPOServerAPI.CustomPullPricingProductDataRequestSerializer,
                                                                                                                                                                                                          //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ));

            EMPClientAPI.OnPullPricingProductDataResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullPricingProductDataResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomPullPricingProductDataRequestSerializer
                                                                                                                                                                                                                              //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ),
                                                                                                                                                                                           response.Response?.ToJSON(EMPClientAPI.CustomPullPricingProductDataResponseSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomPricingProductDataSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomPricingProductDataRecordSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnPullPricingProductDataHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullPricingProductDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPullEVSEPricing

            EMPClientAPI.OnPullEVSEPricingHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullEVSEPricingHTTPRequest",    httpRequest);

            EMPClientAPI.OnPullEVSEPricingRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPullEVSEPricingRequest",        request.ToJSON(//CPOServerAPI.CustomPullEVSEPricingRequestSerializer,
                                                                                                                                                                                            //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                            ));

            EMPClientAPI.OnPullEVSEPricingResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullEVSEPricingResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomPullEVSEPricingRequestSerializer
                                                                                                                                                                                                                //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                ),
                                                                                                                                                                             response.Response?.ToJSON(EMPClientAPI.CustomPullEVSEPricingResponseSerializer,
                                                                                                                                                                                                       EMPClientAPI.CustomOperatorEVSEPricingSerializer,
                                                                                                                                                                                                       EMPClientAPI.CustomEVSEPricingSerializer,
                                                                                                                                                                                                       EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnPullEVSEPricingHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullEVSEPricingHTTPResponse",   httpResponse);

            #endregion


            #region OnPushAuthenticationData

            EMPClientAPI.OnPushAuthenticationDataHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPushAuthenticationDataHTTPRequest",    httpRequest);

            EMPClientAPI.OnPushAuthenticationDataRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnPushAuthenticationDataRequest",        request.ToJSON(//CPOServerAPI.CustomPushAuthenticationDataRequestSerializer,
                                                                                                                                                                                                          //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ));

            EMPClientAPI.OnPushAuthenticationDataResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPushAuthenticationDataResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomPushAuthenticationDataRequestSerializer
                                                                                                                                                                                                                              //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ),
                                                                                                                                                                                           response.Response?.ToJSON(EMPClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnPushAuthenticationDataHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPushAuthenticationDataHTTPResponse",   httpResponse);

            #endregion


            #region OnAuthorizeRemoteReservationStart

            EMPClientAPI.OnAuthorizeRemoteReservationStartHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartHTTPRequest",    httpRequest);

            EMPClientAPI.OnAuthorizeRemoteReservationStartRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartRequest",        request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                                                                                                            //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                            ));

            EMPClientAPI.OnAuthorizeRemoteReservationStartResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomAuthorizeRemoteReservationStartRequestSerializer
                                                                                                                                                                                                                                                //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                                ),
                                                                                                                                                                                                             response.Response?.ToJSON(EMPClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                       EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnAuthorizeRemoteReservationStartHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteReservationStop

            EMPClientAPI.OnAuthorizeRemoteReservationStopHTTPRequest    += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopHTTPRequest",     httpRequest);

            EMPClientAPI.OnAuthorizeRemoteReservationStopRequest        += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopRequest",         request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteReservationStopRequestSerializer,
                                                                                                                                                                                                                            //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                            ));

            EMPClientAPI.OnAuthorizeRemoteReservationStopResponse       += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopResponse",        response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomAuthorizeRemoteReservationStopRequestSerializer
                                                                                                                                                                                                                                                //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                                ),
                                                                                                                                                                                                             response.Response?.ToJSON(EMPClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                                       EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnAuthorizeRemoteReservationStopHTTPResponse   += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteReservationStopHTTPResponse",    httpResponse);

            #endregion

            #region OnAuthorizeRemoteStart

            EMPClientAPI.OnAuthorizeRemoteStartHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteStartHTTPRequest",    httpRequest);

            EMPClientAPI.OnAuthorizeRemoteStartRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteStartRequest",        request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteStartRequestSerializer,
                                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                      ));

            EMPClientAPI.OnAuthorizeRemoteStartResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteStartResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomAuthorizeRemoteStartRequestSerializer
                                                                                                                                                                                                                          //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                          ),
                                                                                                                                                                                       response.Response?.ToJSON(EMPClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                 EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnAuthorizeRemoteStartHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeRemoteStop

            EMPClientAPI.OnAuthorizeRemoteStopHTTPRequest    += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeRemoteStopHTTPRequest",     httpRequest);

            EMPClientAPI.OnAuthorizeRemoteStopRequest        += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeRemoteStopRequest",         request.ToJSON(//CPOServerAPI.CustomAuthorizeRemoteStopRequestSerializer,
                                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                      ));

            EMPClientAPI.OnAuthorizeRemoteStopResponse       += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeRemoteStopResponse",        response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomAuthorizeRemoteStopRequestSerializer
                                                                                                                                                                                                                          //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                          ),
                                                                                                                                                                                       response.Response?.ToJSON(EMPClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                 EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnAuthorizeRemoteStopHTTPResponse   += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeRemoteStopHTTPResponse",    httpResponse);

            #endregion


            #region OnGetChargeDetailRecords

            EMPClientAPI.OnGetChargeDetailRecordsHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnGetChargeDetailRecordsHTTPRequest",    httpRequest);

            EMPClientAPI.OnGetChargeDetailRecordsRequest       += (timestamp, empClientAPI, request)                     => DebugLog.SubmitEvent("OnGetChargeDetailRecordsRequest",        request.ToJSON(//CPOServerAPI.CustomGetChargeDetailRecordsRequestSerializer,
                                                                                                                                                                                                          //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ));

            EMPClientAPI.OnGetChargeDetailRecordsResponse      += (timestamp, empClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnGetChargeDetailRecordsResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CentralServiceAPI.EMPClientAPI.CustomGetChargeDetailRecordsRequestSerializer
                                                                                                                                                                                                                              //CentralServiceAPI.EMPClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ),
                                                                                                                                                                                           response.Response?.ToJSON(EMPClientAPI.CustomGetChargeDetailRecordsResponseSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomIPagedResponseSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomChargeDetailRecordSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomIdentificationSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomSignedMeteringValueSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomCalibrationLawVerificationSerializer,
                                                                                                                                                                                                                     EMPClientAPI.CustomStatusCodeSerializer)));

            EMPClientAPI.OnGetChargeDetailRecordsHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnGetChargeDetailRecordsHTTPResponse",   httpResponse);

            #endregion

        }

        public void RegisterAllEvents(CPOClientAPI       CPOClientAPI)
        {

            #region OnPushEVSEData

            CPOClientAPI.OnPushEVSEDataHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPushEVSEDataHTTPRequest",    httpRequest);

            CPOClientAPI.OnPushEVSEDataRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnPushEVSEDataRequest",        request.ToJSON(//CPOServerAPI.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                      ));

            CPOClientAPI.OnPushEVSEDataResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPushEVSEDataResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEDataRequestSerializer
                                                                                                                                                                                                          //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ),
                                                                                                                                                                       response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                 CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnPushEVSEDataHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPushEVSEDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPushEVSEStatus

            CPOClientAPI.OnPushEVSEStatusHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPushEVSEStatusHTTPRequest",    httpRequest);

            CPOClientAPI.OnPushEVSEStatusRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnPushEVSEStatusRequest",        request.ToJSON(//CPOServerAPI.CustomPushEVSEStatusRequestSerializer,
                                                                                                                                                                                          //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                          ));

            CPOClientAPI.OnPushEVSEStatusResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPushEVSEStatusResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEStatusRequestSerializer
                                                                                                                                                                                                                              //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ),
                                                                                                                                                                                           response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                     CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnPushEVSEStatusHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPushEVSEStatusHTTPResponse",   httpResponse);

            #endregion


            #region OnPushPricingProductData

            CPOClientAPI.OnPushPricingProductDataHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPushPricingProductDataHTTPRequest",    httpRequest);

            CPOClientAPI.OnPushPricingProductDataRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnPushPricingProductDataRequest",        request.ToJSON(//CPOServerAPI.CustomPushPricingProductDataRequestSerializer,
                                                                                                                                                                                                          //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ));

            CPOClientAPI.OnPushPricingProductDataResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPushPricingProductDataResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushPricingProductDataRequestSerializer
                                                                                                                                                                                                                              //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ),
                                                                                                                                                                                           response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                     CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnPushPricingProductDataHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPushPricingProductDataHTTPResponse",   httpResponse);

            #endregion

            #region OnPushEVSEPricing

            CPOClientAPI.OnPushEVSEPricingHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPushEVSEPricingHTTPRequest",    httpRequest);

            CPOClientAPI.OnPushEVSEPricingRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnPushEVSEPricingRequest",        request.ToJSON(//CPOServerAPI.CustomPushEVSEPricingRequestSerializer,
                                                                                                                                                                                            //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                            ));

            CPOClientAPI.OnPushEVSEPricingResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPushEVSEPricingResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEPricingRequestSerializer
                                                                                                                                                                                                                //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                ),
                                                                                                                                                                             response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                       CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnPushEVSEPricingHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPushEVSEPricingHTTPResponse",   httpResponse);

            #endregion


            #region OnPullAuthenticationData

            CPOClientAPI.OnPullAuthenticationDataHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnPullAuthenticationDataHTTPRequest",    httpRequest);

            CPOClientAPI.OnPullAuthenticationDataRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnPullAuthenticationDataRequest",        request.ToJSON(//CPOServerAPI.CustomPullAuthenticationDataRequestSerializer,
                                                                                                                                                                                                          //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                          ));

            CPOClientAPI.OnPullAuthenticationDataResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnPullAuthenticationDataResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPullAuthenticationDataRequestSerializer
                                                                                                                                                                                                                              //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                              ),
                                                                                                                                                                                           response.Response?.ToJSON(CPOClientAPI.CustomPullAuthenticationDataResponseSerializer,
                                                                                                                                                                                                                     CPOClientAPI.CustomProviderAuthenticationDataSerializer,
                                                                                                                                                                                                                     CPOClientAPI.CustomIdentificationSerializer,
                                                                                                                                                                                                                     CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnPullAuthenticationDataHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnPullAuthenticationDataHTTPResponse",   httpResponse);

            #endregion


            #region OnAuthorizeStart

            CPOClientAPI.OnAuthorizeStartHTTPRequest      += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeStartHTTPRequest",    httpRequest);

            CPOClientAPI.OnAuthorizeStartRequest          += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeStartRequest",        request.ToJSON(//CPOServerAPI.CustomAuthorizeStartRequestSerializer,
                                                                                                                                                                                             //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                             ));

            CPOClientAPI.OnAuthorizeStartResponse         += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeStartResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomAuthorizeStartRequestSerializer
                                                                                                                                                                                                                 //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                 ),
                                                                                                                                                                              response.Response?.ToJSON(CPOClientAPI.CustomAuthorizationStartSerializer,
                                                                                                                                                                                                        CPOClientAPI.CustomStatusCodeSerializer,
                                                                                                                                                                                                        CPOClientAPI.CustomIdentificationSerializer)));

            CPOClientAPI.OnAuthorizationStartHTTPResponse += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeStartHTTPResponse",   httpResponse);

            #endregion

            #region OnAuthorizeStop

            CPOClientAPI.OnAuthorizeStopHTTPRequest       += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnAuthorizeStopHTTPRequest",     httpRequest);

            CPOClientAPI.OnAuthorizeStopRequest           += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnAuthorizeStopRequest",         request.ToJSON(//CPOServerAPI.CustomAuthorizeStopRequestSerializer,
                                                                                                                                                                                             //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                             ));

            CPOClientAPI.OnAuthorizeStopResponse          += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnAuthorizeStopResponse",        response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomAuthorizeStopRequestSerializer
                                                                                                                                                                                                                 //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                 ),
                                                                                                                                                                              response.Response?.ToJSON(CPOClientAPI.CustomAuthorizationStopSerializer,
                                                                                                                                                                                                        CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnAuthorizationStopHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnAuthorizeStopHTTPResponse",    httpResponse);

            #endregion


            #region OnChargingNotifications

            CPOClientAPI.OnChargingNotificationHTTPRequest      += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnChargingNotificationHTTPRequest",        httpRequest);


            CPOClientAPI.OnChargingStartNotificationRequest     += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnChargingStartNotificationRequest",       request.ToJSON(//CPOServerAPI.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                                             //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                             ));

            CPOClientAPI.OnChargingStartNotificationResponse    += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargingStartNotificationResponse",      response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEDataRequestSerializer
                                                                                                                                                                                                                                 //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                 ),
                                                                                                                                                                                              response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                        CPOClientAPI.CustomStatusCodeSerializer)));


            CPOClientAPI.OnChargingProgressNotificationRequest  += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnChargingProgressNotificationRequest",    request.ToJSON(//CPOServerAPI.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                      ));

            CPOClientAPI.OnChargingProgressNotificationResponse += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargingProgressNotificationResponse",   response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEDataRequestSerializer
                                                                                                                                                                                                                                 //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                 ),
                                                                                                                                                                                              response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                        CPOClientAPI.CustomStatusCodeSerializer)));


            CPOClientAPI.OnChargingEndNotificationRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnChargingEndNotificationRequest",         request.ToJSON(//CPOServerAPI.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                                      //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                      ));

            CPOClientAPI.OnChargingEndNotificationResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargingEndNotificationResponse",        response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEDataRequestSerializer
                                                                                                                                                                                                                                 //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                 ),
                                                                                                                                                                                              response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                        CPOClientAPI.CustomStatusCodeSerializer)));


            CPOClientAPI.OnChargingErrorNotificationRequest  += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnChargingErrorNotificationRequest",          request.ToJSON(//CPOServerAPI.CustomPushEVSEDataRequestSerializer,
                                                                                                                                                                                                             //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                             ));

            CPOClientAPI.OnChargingErrorNotificationResponse += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargingErrorNotificationResponse",         response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomPushEVSEDataRequestSerializer
                                                                                                                                                                                                                                 //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                                 ),
                                                                                                                                                                                              response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                                        CPOClientAPI.CustomStatusCodeSerializer)));


            CPOClientAPI.OnChargingNotificationHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnChargingNotificationHTTPResponse",          httpResponse);

            #endregion


            #region OnChargeDetailRecord

            CPOClientAPI.OnChargeDetailRecordHTTPRequest   += (timestamp, httpAPI, httpRequest)                      => DebugLog.SubmitEvent("OnChargeDetailRecordHTTPRequest",    httpRequest);

            CPOClientAPI.OnChargeDetailRecordRequest       += (timestamp, cpoClientAPI, request)                     => DebugLog.SubmitEvent("OnChargeDetailRecordRequest",        request.ToJSON(//CPOServerAPI.CustomChargeDetailRecordRequestSerializer,
                                                                                                                                                                                                  //CPOServerAPI.CustomIdentificationSerializer
                                                                                                                                                                                                  ));

            CPOClientAPI.OnChargeDetailRecordResponse      += (timestamp, cpoClientAPI, request, response, runtime)  => DebugLog.SubmitEvent("OnChargeDetailRecordResponse",       response.ToJSON(response.Response?.Request?.ToJSON(//CPOClientAPI.CustomChargeDetailRecordRequestSerializer
                                                                                                                                                                                                                      //CPOClientAPI.CustomIdentificationSerializer
                                                                                                                                                                                                                      ),
                                                                                                                                                                                   response.Response?.ToJSON(CPOClientAPI.CustomAcknowledgementSerializer,
                                                                                                                                                                                                             CPOClientAPI.CustomStatusCodeSerializer)));

            CPOClientAPI.OnChargeDetailRecordHTTPResponse  += (timestamp, httpAPI, httpRequest, httpResponse)        => DebugLog.SubmitEvent("OnChargeDetailRecordHTTPResponse",   httpResponse);

            #endregion

        }

        #endregion


    }

}
