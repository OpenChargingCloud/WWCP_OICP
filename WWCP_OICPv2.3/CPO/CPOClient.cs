/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.HTTP
{


    #region OnPushEVSEDataRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataRequestDelegate (DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        ICPOClient                              Sender,
                                                        String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionTypes                             Action,
                                                        UInt64                                  NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>             EVSEDataRecords,
                                                        TimeSpan                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataResponseDelegate(DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        ICPOClient                              Sender,
                                                        String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionTypes                             Action,
                                                        UInt64                                  NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>             EVSEDataRecords,
                                                        TimeSpan                                RequestTimeout,
                                                        Acknowledgement<PushEVSEDataRequest>    Result,
                                                        TimeSpan                                Runtime);

    #endregion


    /// <summary>
    /// The CPO client.
    /// </summary>
    public partial class CPOClient : HTTPClient
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


        #region Properties

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        public Logger  HTTPLogger    { get; }

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
        public event ClientRequestLogHandler         OnPushEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPushEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate  OnPushEVSEDataResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMSP client.
        /// </summary>
        /// <param name="RemoteVersionsURL">The remote URL of the VERSIONS endpoint to connect to.</param>
        /// <param name="AccessToken">The access token.</param>
        /// <param name="MyCommonAPI">My Common API.</param>
        /// <param name="Description">An optional description of this client.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="RemoteCertificateValidator">An optional remote SSL/TLS certificate validator.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(URL                                  RemoteVersionsURL,
                         //AccessToken                          AccessToken,
                         //CommonAPI                            MyCommonAPI,
                         String                               Description                  = null,
                         HTTPHostname?                        VirtualHostname              = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         TimeSpan?                            RequestTimeout               = null,
                         Byte?                                MaxNumberOfRetries           = null,
                         DNSClient                            DNSClient                    = null)

            //: base(RemoteVersionsURL,
            //       AccessToken,
            //       MyCommonAPI,
            //       Description,
            //       VirtualHostname,
            //       RemoteCertificateValidator,
            //       RequestTimeout,
            //       MaxNumberOfRetries,
            //       DNSClient)

        {

            this.HTTPLogger  = new Logger(this);

        }

        #endregion

        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(CPOClient));


        #region PushEVSEData  (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(PushEVSEDataRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PushEVSEData request must not be null!");

            Request = _CustomPushEVSEDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PushEVSEData request must not be null!");


            Byte                                               TransmissionRetry  = 0;
            HTTPResponse<Acknowledgement<PushEVSEDataRequest>> result             = null;

            #endregion

            #region Send OnPushEVSEDataRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEDataRequest != null)
                    await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                       Cast<OnPushEVSEDataRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEDataRecords.ULongCount(),
                                                     Request.EVSEDataRecords,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
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

                result = HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.OK(
                             Acknowledgement<PushEVSEDataRequest>.Success(Request,
                                                                          StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URLPrefix + EVSEDataURL,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomPushEVSEDataSOAPRequestMapper(Request,
                                                                                          SOAP.Encapsulation(Request.ToXML(CustomPushEVSEDataRequestSerializer: CustomPushEVSEDataRequestSerializer,
                                                                                                                           CustomOperatorEVSEDataSerializer:    CustomOperatorEVSEDataSerializer,
                                                                                                                           CustomEVSEDataRecordSerializer:      CustomEVSEDataRecordSerializer))),
                                                     "eRoamingPushEvseData",
                                                     RequestLogDelegate:   OnPushEVSEDataHTTPRequest,
                                                     ResponseLogDelegate:  OnPushEVSEDataHTTPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                          Acknowledgement<PushEVSEDataRequest>.Parse(request,
                                                                                                                                                     xml,
                                                                                                                                                     CustomPushEVSEDataParser,
                                                                                                                                                     CustomStatusCodeParser,
                                                                                                                                                     onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<Acknowledgement<PushEVSEDataRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<PushEVSEDataRequest>(
                                                                        Request,
                                                                        StatusCodes.DataError,
                                                                        httpresponse.Content.ToString()
                                                                    ),

                                                                    IsFault: true

                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return new HTTPResponse<Acknowledgement<PushEVSEDataRequest>>(

                                                                 httpresponse,

                                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                                     Request,
                                                                     StatusCodes.ServiceNotAvailable,
                                                                     httpresponse.HTTPStatusCode.ToString(),
                                                                     httpresponse.HTTPBody.      ToUTF8String()
                                                                 ),

                                                                 IsFault: true);

                                                         }

                                                         return new HTTPResponse<Acknowledgement<PushEVSEDataRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<PushEVSEDataRequest>(
                                                                        Request,
                                                                        StatusCodes.DataError,
                                                                        httpresponse.HTTPStatusCode.ToString(),
                                                                        httpresponse.HTTPBody.      ToUTF8String()
                                                                    ),

                                                                    IsFault: true

                                                                );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.ExceptionThrown(

                                                                new Acknowledgement<PushEVSEDataRequest>(
                                                                    Request,
                                                                    StatusCodes.ServiceNotAvailable,
                                                                    exception.Message,
                                                                    exception.StackTrace
                                                                ),

                                                                Exception: exception

                                                            );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.ClientError(
                                 new Acknowledgement<PushEVSEDataRequest>(
                                     Request,
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnPushEVSEDataResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEDataResponse != null)
                    await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                       Cast<OnPushEVSEDataResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEDataRecords.ULongCount(),
                                                     Request.EVSEDataRecords,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
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


        #region GetLocation    (CountryCode, PartyId, LocationId, ...)

        /// <summary>
        /// Get the charging location specified by the given location identification from the remote API.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional location to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<OICPResponse<Location>>

            GetLocation(CountryCode         CountryCode,
                        Party_Id            PartyId,
                        Location_Id         LocationId,

                        Request_Id?         RequestId           = null,
                        Correlation_Id?     CorrelationId       = null,
                        Version_Id?         VersionId           = null,

                        DateTime?           Timestamp           = null,
                        CancellationToken?  CancellationToken   = null,
                        EventTracking_Id    EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null)

        {

            OICPResponse<Location> response;

            #region Send OnGetLocationRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                //Counters.GetLocation.IncRequests();

                //if (OnGetLocationRequest != null)
                //    await Task.WhenAll(OnGetLocationRequest.GetInvocationList().
                //                       Cast<OnGetLocationRequestDelegate>().
                //                       Select(e => e(StartTime,
                //                                     Request.Timestamp.Value,
                //                                     this,
                //                                     ClientId,
                //                                     Request.EventTrackingId,

                //                                     Request.PartnerId,
                //                                     Request.OperatorId,
                //                                     Request.ChargingPoolId,
                //                                     Request.StatusEventDate,
                //                                     Request.AvailabilityStatus,
                //                                     Request.TransactionId,
                //                                     Request.AvailabilityStatusUntil,
                //                                     Request.AvailabilityStatusComment,

                //                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                //                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetLocationRequest));
            }

            #endregion


            try
            {

                var requestId      = RequestId     ?? Request_Id.Random();
                var correlationId  = CorrelationId ?? Correlation_Id.Random();
                var remoteURL      = await GetRemoteURL(VersionId,
                                                        ModuleIDs.Locations,
                                                        InterfaceRoles.RECEIVER);

                if (remoteURL.HasValue)
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await (remoteURL.Value.Protocol == HTTPProtocols.http

                                                  ? new HTTPClient (remoteURL.Value.Hostname,
                                                                    RemotePort:  remoteURL.Value.Port ?? IPPort.HTTP,
                                                                    DNSClient:   DNSClient)

                                                  : new HTTPSClient(remoteURL.Value.Hostname,
                                                                    RemoteCertificateValidator,
                                                                    RemotePort:  remoteURL.Value.Port ?? IPPort.HTTPS,
                                                                    DNSClient:   DNSClient)).

                                              Execute(client => client.CreateRequest(HTTPMethod.GET,
                                                                                     remoteURL.Value.Path + CountryCode.ToString() +
                                                                                                            PartyId.    ToString() +
                                                                                                            LocationId. ToString(),
                                                                                     requestbuilder => {
                                                                                         requestbuilder.Authorization = TokenAuth;
                                                                                         requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                         requestbuilder.Set("X-Request-ID",      requestId);
                                                                                         requestbuilder.Set("X-Correlation-ID",  correlationId);
                                                                                     }),

                                                      RequestLogDelegate:   OnGetLocationHTTPRequest,
                                                      ResponseLogDelegate:  OnGetLocationHTTPResponse,
                                                      CancellationToken:    CancellationToken,
                                                      EventTrackingId:      EventTrackingId,
                                                      RequestTimeout:       RequestTimeout ?? this.RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion

                    response = OICPResponse<Location>.ParseJObject(HTTPResponse,
                                                                   requestId,
                                                                   correlationId,
                                                                   json => Location.Parse(json));

                }

                else
                    response = new OICPResponse<String, Location>("",
                                                                  default,
                                                                  -1,
                                                                  "No remote URL available!");

            }

            catch (Exception e)
            {

                response = new OICPResponse<String, Location>("",
                                                              default,
                                                              -1,
                                                              e.Message,
                                                              e.StackTrace);

            }


            #region Send OnGetLocationResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                // Update counters
                //if (response.HTTPStatusCode == HTTPStatusCode.OK && response.Content.RequestStatus.Code == 1)
                //    Counters.SetChargingPoolAvailabilityStatus.IncResponses_OK();
                //else
                //    Counters.SetChargingPoolAvailabilityStatus.IncResponses_Error();


                //if (OnGetLocationResponse != null)
                //    await Task.WhenAll(OnGetLocationResponse.GetInvocationList().
                //                       Cast<OnGetLocationResponseDelegate>().
                //                       Select(e => e(Endtime,
                //                                     Request.Timestamp.Value,
                //                                     this,
                //                                     ClientId,
                //                                     Request.EventTrackingId,

                //                                     Request.PartnerId,
                //                                     Request.OperatorId,
                //                                     Request.ChargingPoolId,
                //                                     Request.StatusEventDate,
                //                                     Request.AvailabilityStatus,
                //                                     Request.TransactionId,
                //                                     Request.AvailabilityStatusUntil,
                //                                     Request.AvailabilityStatusComment,

                //                                     Request.RequestTimeout ?? RequestTimeout.Value,
                //                                     result.Content,
                //                                     Endtime - StartTime))).
                //                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetLocationResponse));
            }

            #endregion

            return response;

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
