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
using System.Security.Cryptography.X509Certificates;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.HTTP
{


    #region OnPushEVSEDataRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataRequestDelegate (DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        CPOClient                               Sender,
                                                        //String                                  SenderId,
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
                                                        CPOClient                               Sender,
                                                        //String                                  SenderId,
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

            this.RemoteURL                   = URL.Parse("https://service-qa.hubject.com");
            this.RemoteCertificateValidator  = (sender, certificate, chain, policyErrors) => true;
            this.ClientCert                  = ClientCert;

        }

        #endregion

        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(CPOClient));


        #region PushEVSEData  (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<Acknowledgement<PushEVSEDataRequest>>

            PushEVSEData(PushEVSEDataRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PushEVSEData request must not be null!");

            //Request = _CustomPushEVSEDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PushEVSEData request must not be null!");


            Byte                                 TransmissionRetry  = 0;
            Acknowledgement<PushEVSEDataRequest> result             = null;

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

                result = Acknowledgement<PushEVSEDataRequest>.Success(Request,
                                                                      StatusCodeDescription: "No EVSE data to push");

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

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.HTTPBody.Length > 0)
                            {

                                var JSON = JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String());

                                if (Acknowledgement<PushEVSEDataRequest>.TryParse(Request,
                                                                                  JSON,
                                                                                  out Acknowledgement<PushEVSEDataRequest> Acknowledgement,
                                                                                  out String ErrorResponse))
                                {


                                }

                            }

                            TransmissionRetry = Byte.MaxValue - 1;
                            break;

                        }

                        else if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                        { }

                    }
                    while (TransmissionRetry++ < MaxNumberOfRetries);

                }
                catch (Exception e)
                {
                    
                }

                //if (result == null)
                //    result = HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.ClientError(
                //                 new Acknowledgement<PushEVSEDataRequest>(
                //                     Request,
                //                     StatusCodes.SystemError,
                //                     "HTTP request failed!"
                //                 )
                //             );

            }


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


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        { }

        #endregion

    }

}
