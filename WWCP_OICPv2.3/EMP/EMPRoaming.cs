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
using System.Xml.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using System.Security.Authentication;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP roaming object combines the EMP client and EMP server
    /// and adds additional logging for both.
    /// </summary>
    public class EMPRoaming : IEMPClient
    {

        #region Properties

        /// <summary>
        /// The EMP client part.
        /// </summary>
        public EMPClient     EMPClient    { get; }

        #region IEMPClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                  IHTTPClient.RemoteURL
            => EMPClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                        IHTTPClient.VirtualHostname
            => EMPClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        String                               IHTTPClient.Description
        {

            get
            {
                return EMPClient.Description;
            }

            set
            {
                EMPClient.Description = value;
            }

        }

        /// <summary>
        /// The remote SSL/TLS certificate validator.
        /// </summary>
        RemoteCertificateValidationCallback  IHTTPClient.RemoteCertificateValidator
            => EMPClient.RemoteCertificateValidator;

        /// <summary>
        /// The SSL/TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate                      IHTTPClient.ClientCert
            => EMPClient.ClientCert;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                               IHTTPClient.HTTPUserAgent
            => EMPClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                             IHTTPClient.RequestTimeout
        {

            get
            {
                return EMPClient.RequestTimeout;
            }

            set
            {
                EMPClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate       IHTTPClient.TransmissionRetryDelay
            => EMPClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicationg with the remote OICP service.
        /// </summary>
        UInt16                               IHTTPClient.MaxNumberOfRetries
            => EMPClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                              IHTTPClient.UseHTTPPipelining
            => EMPClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger                     IHTTPClient.HTTPLogger
        {

            get
            {
                return EMPClient.HTTPLogger;
            }

            set
            {
                if (value is EMPClient.Logger logger)
                    EMPClient.HTTPLogger = logger;
            }

        }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        DNSClient                            IHTTPClient.DNSClient
            => EMPClient.DNSClient;

        #endregion


        /// <summary>
        /// The EMP server part.
        /// </summary>
        public EMPServerAPI  EMPServer    { get; }

        #endregion

        #region Events

        // EMPClient methods

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate OnPullEVSEDataRequest
        {

            add
            {
                EMPClient.OnPullEVSEDataRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEDataHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEDataHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEDataHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEDataHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate OnPullEVSEDataResponse
        {

            add
            {
                EMPClient.OnPullEVSEDataResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataResponse -= value;
            }

        }

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate OnPullEVSEStatusRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEStatusHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEStatusHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate OnPullEVSEStatusResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusResponse -= value;
            }

        }

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        ///// <summary>
        ///// An event fired whenever a 'pull EVSE status by id' request will be send.
        ///// </summary>
        //public event OnPullEVSEStatusByIdRequestDelegate OnPullEVSEStatusByIdRequest
        //{

        //    add
        //    {
        //        EMPClient.OnPullEVSEStatusByIdRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPullEVSEStatusByIdRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a 'pull EVSE status by id' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnPullEVSEStatusByIdSOAPRequest
        //{

        //    add
        //    {
        //        EMPClient.OnPullEVSEStatusByIdSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPullEVSEStatusByIdSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'pull EVSE status by id' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnPullEVSEStatusByIdOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnPullEVSEStatusByIdHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPullEVSEStatusByIdHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'pull EVSE status by id' request had been received.
        ///// </summary>
        //public event OnPullEVSEStatusByIdResponseDelegate OnPullEVSEStatusByIdResponse
        //{

        //    add
        //    {
        //        EMPClient.OnPullEVSEStatusByIdResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPullEVSEStatusByIdResponse -= value;
        //    }

        //}

        #endregion


        #region OnPushAuthenticationDataRequest/-Response

        ///// <summary>
        ///// An event fired whenever a 'push authentication data' request will be send.
        ///// </summary>
        //public event OnPushAuthenticationDataRequestDelegate OnPushAuthenticationDataRequest
        //{

        //    add
        //    {
        //        EMPClient.OnPushAuthenticationDataRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPushAuthenticationDataRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a 'push authentication data' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnPushAuthenticationDataSOAPRequest
        //{

        //    add
        //    {
        //        EMPClient.OnPushAuthenticationDataSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPushAuthenticationDataSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'push authentication data' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnPushAuthenticationDataOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnPushAuthenticationDataHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPushAuthenticationDataHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'push authentication data' request had been received.
        ///// </summary>
        //public event OnPushAuthenticationDataResponseDelegate OnPushAuthenticationDataResponse
        //{

        //    add
        //    {
        //        EMPClient.OnPushAuthenticationDataResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnPushAuthenticationDataResponse -= value;
        //    }

        //}

        #endregion


        #region OnAuthorizeRemoteReservationStartRequest/-Response

        ///// <summary>
        ///// An event fired whenever a 'reservation start' request will be send.
        ///// </summary>
        //public event OnAuthorizeRemoteReservationStartRequestDelegate OnAuthorizeRemoteReservationStartRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a 'reservation start' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnAuthorizeRemoteReservationStartSOAPRequest

        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'reservation start' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnAuthorizeRemoteReservationStartOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'reservation start' request had been received.
        ///// </summary>
        //public event OnAuthorizeRemoteReservationStartResponseDelegate OnAuthorizeRemoteReservationStartResponse
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStartResponse -= value;
        //    }

        //}

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        ///// <summary>
        ///// An event fired whenever a 'reservation stop' request will be send.
        ///// </summary>
        //public event OnAuthorizeRemoteReservationStopRequestDelegate OnAuthorizeRemoteReservationStopRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a 'reservation stop' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnAuthorizeRemoteReservationStopSOAPRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'reservation stop' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnAuthorizeRemoteReservationStopOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'reservation stop' request had been received.
        ///// </summary>
        //public event OnAuthorizeRemoteReservationStopResponseDelegate OnAuthorizeRemoteReservationStopResponse
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteReservationStopResponse -= value;
        //    }

        //}

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        ///// <summary>
        ///// An event fired whenever an 'authorize remote start' request will be send.
        ///// </summary>
        //public event OnAuthorizeRemoteStartRequestDelegate OnAuthorizeRemoteStartRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStartRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStartRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever an 'authorize remote start' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnAuthorizeRemoteStartSOAPRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStartSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStartSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to an 'authorize remote start' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnAuthorizeRemoteStartOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStartHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStartHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to an 'authorize remote start' request had been received.
        ///// </summary>
        //public event OnAuthorizeRemoteStartResponseDelegate OnAuthorizeRemoteStartResponse
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStartResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStartResponse -= value;
        //    }

        //}

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        ///// <summary>
        ///// An event fired whenever an 'authorize remote stop' request will be send.
        ///// </summary>
        //public event OnAuthorizeRemoteStopRequestDelegate OnAuthorizeRemoteStopRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStopRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStopRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever an 'authorize remote stop' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnAuthorizeRemoteStopSOAPRequest
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStopSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStopSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to an 'authorize remote stop' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnAuthorizeRemoteStopOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStopHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStopHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to an 'authorize remote stop' request had been received.
        ///// </summary>
        //public event OnAuthorizeRemoteStopResponseDelegate OnAuthorizeRemoteStopResponse
        //{

        //    add
        //    {
        //        EMPClient.OnAuthorizeRemoteStopResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnAuthorizeRemoteStopResponse -= value;
        //    }

        //}

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        ///// <summary>
        ///// An event fired whenever a 'get charge detail records' request will be send.
        ///// </summary>
        //public event OnGetChargeDetailRecordsRequestDelegate OnGetChargeDetailRecordsRequest
        //{

        //    add
        //    {
        //        EMPClient.OnGetChargeDetailRecordsRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnGetChargeDetailRecordsRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a 'get charge detail records' HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnGetChargeDetailRecordsSOAPRequest
        //{

        //    add
        //    {
        //        EMPClient.OnGetChargeDetailRecordsSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnGetChargeDetailRecordsSOAPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a 'get charge detail records' HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnGetChargeDetailRecordsOICPResult
        //{

        //    add
        //    {
        //        EMPClient.OnGetChargeDetailRecordsHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnGetChargeDetailRecordsHTTPResponse -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response for a 'get charge detail records' request was received.
        ///// </summary>
        //public event OnGetChargeDetailRecordsResponseDelegate OnGetChargeDetailRecordsResponse
        //{

        //    add
        //    {
        //        EMPClient.OnGetChargeDetailRecordsResponse += value;
        //    }

        //    remove
        //    {
        //        EMPClient.OnGetChargeDetailRecordsResponse -= value;
        //    }

        //}

        #endregion


        // EMPServer methods

        #region OnAuthorizeStart    (HTTP)(Request/-Response)

        ///// <summary>
        ///// An event sent whenever a authorize start HTTP request was received.
        ///// </summary>
        //public event RequestLogHandler OnAuthorizeStartHTTPRequest
        //{

        //    add
        //    {
        //        EMPServer.OnAuthorizeStartHTTPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPServer.OnAuthorizeStartHTTPRequest -= value;
        //    }

        //}

        /// <summary>
        /// An event sent whenever a authorize start request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate OnAuthorizeStartRequest
        {

            add
            {
                EMPServer.OnAuthorizeStartRequest += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStartRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate OnAuthorizeStart
        {

            add
            {
                EMPServer.OnAuthorizeStart += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate OnAuthorizeStartResponse
        {

            add
            {
                EMPServer.OnAuthorizeStartResponse += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStartResponse -= value;
            }

        }

        ///// <summary>
        ///// An event sent whenever a authorize start SOAP response was sent.
        ///// </summary>
        //public event HTTPResponseLogEvent OnAuthorizeStartOICPResult
        //{

        //    add
        //    {
        //        EMPServer.OnAuthorizationStartHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPServer.OnAuthorizationStartHTTPResponse -= value;
        //    }

        //}

        #endregion

        #region OnAuthorizeStop     (HTTP)(Request/-Response)

        ///// <summary>
        ///// An event sent whenever a authorize stop HTTP request was received.
        ///// </summary>
        //public event RequestLogHandler OnAuthorizeStopSOAPRequest
        //{

        //    add
        //    {
        //        EMPServer.OnAuthorizeStopSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPServer.OnAuthorizeStopSOAPRequest -= value;
        //    }

        //}

        /// <summary>
        /// An event sent whenever a authorize stop HTTP request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate OnAuthorizeStopRequest
        {

            add
            {
                EMPServer.OnAuthorizeStopRequest += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStopRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate  OnAuthorizeStop
        {

            add
            {
                EMPServer.OnAuthorizeStop += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStop -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop SOAP response was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate OnAuthorizeStopResponse
        {

            add
            {
                EMPServer.OnAuthorizeStopResponse += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStopResponse -= value;
            }

        }

        ///// <summary>
        ///// An event sent whenever a authorize stop SOAP response was sent.
        ///// </summary>
        //public event AccessLogHandler OnAuthorizeStopOICPResult
        //{

        //    add
        //    {
        //        EMPServer.OnAuthorizeStopHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPServer.OnAuthorizeStopHTTPResponse -= value;
        //    }

        //}

        #endregion

        #region OnChargeDetailRecord(HTTP)(Request/-Response)

        ///// <summary>
        ///// An event sent whenever a charge detail record HTTP request was received.
        ///// </summary>
        //public event RequestLogHandler OnChargeDetailRecordSOAPRequest
        //{

        //    add
        //    {
        //        EMPServer.OnChargeDetailRecordSOAPRequest += value;
        //    }

        //    remove
        //    {
        //        EMPServer.OnChargeDetailRecordSOAPRequest -= value;
        //    }

        //}

        /// <summary>
        /// An event sent whenever a charge detail record request was received.
        /// </summary>
        public event OnChargeDetailRecordRequestDelegate OnChargeDetailRecordRequest
        {

            add
            {
                EMPServer.OnChargeDetailRecordRequest += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecordRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate OnChargeDetailRecord
        {

            add
            {
                EMPServer.OnChargeDetailRecord += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecord -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event OnChargeDetailRecordResponseDelegate OnChargeDetailRecordResponse
        {

            add
            {
                EMPServer.OnChargeDetailRecordResponse += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecordResponse -= value;
            }

        }

        ///// <summary>
        ///// An event sent whenever a charge detail record SOAP response was sent.
        ///// </summary>
        //public event AccessLogHandler OnChargeDetailRecordOICPResult
        //{

        //    add
        //    {
        //        EMPServer.OnChargeDetailRecordHTTPResponse += value;
        //    }

        //    remove
        //    {
        //        EMPServer.OnChargeDetailRecordHTTPResponse -= value;
        //    }

        //}

        #endregion


        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        #region (private) EMPRoaming(EMPServer, EMPClient)

        /// <summary>
        /// Create a new EMP roaming.
        /// </summary>
        /// <param name="EMPServer">An EMP Server.</param>
        /// <param name="EMPClient">An EMP client.</param>
        private EMPRoaming(EMPServerAPI  EMPServer,
                           EMPClient     EMPClient)
        {

            this.EMPClient  = EMPClient;
            this.EMPServer  = EMPServer;

            // Link HTTP events...
            EMPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

        #endregion


        #region EMPRoaming(EMPClient)

        /// <summary>
        /// Create a new EMP roaming.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        public EMPRoaming(EMPClient EMPClient)

            : this(null,
                   EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMPClient must not be null!"))

        { }

        #endregion

        #region EMPRoaming(EMPServer)

        /// <summary>
        /// Create a new EMP roaming.
        /// </summary>
        /// <param name="EMPServer">An EMP Server.</param>
        public EMPRoaming(EMPServerAPI  EMPServer)

            : this(EMPServer ?? throw new ArgumentNullException(nameof(EMPServer), "The given EMPServer must not be null!"),
                   null)

        { }

        #endregion

        #region EMPRoaming(EMPClient, EMPServer)

        /// <summary>
        /// Create a new EMP roaming.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// <param name="EMPServer">An EMP Server.</param>
        public EMPRoaming(EMPClient     EMPClient,
                          EMPServerAPI  EMPServer)

            : this(EMPServer ?? throw new ArgumentNullException(nameof(EMPServer), "The given EMPServer must not be null!"),
                   EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMPClient must not be null!"))

        { }

        #endregion

        #endregion


        #region PullEVSEData              (Request)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">An PullEVSEData request.</param>
        public Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(PullEVSEDataRequest Request)

                => EMPClient.PullEVSEData(Request);

        #endregion

        #region PullEVSEStatus            (Request)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(PullEVSEStatusRequest Request)

                => EMPClient.PullEVSEStatus(Request);

        #endregion

        #region PullEVSEStatusById        (Request)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public Task<OICPResult<PullEVSEStatusByIdResponse>>

            PullEVSEStatusById(PullEVSEStatusByIdRequest Request)

                => EMPClient.PullEVSEStatusById(Request);

        #endregion

        #region PullEVSEStatusByOperatorId(Request)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>

            PullEVSEStatusByOperatorId(PullEVSEStatusByOperatorIdRequest Request)

                => EMPClient.PullEVSEStatusByOperatorId(Request);

        #endregion


        #region PushAuthenticationData    (Request)

        ///// <summary>
        ///// Create a new task pushing provider authentication data records onto the OICP server.
        ///// </summary>
        ///// <param name="Request">An PushAuthenticationData request.</param>
        //public Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

        //    PushAuthenticationData(PushAuthenticationDataRequest Request)

        //        => EMPClient.PushAuthenticationData(Request);

        #endregion


        #region ReservationStart          (Request)

        /// <summary>
        /// Create a reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request)

                => EMPClient.AuthorizeRemoteReservationStart(Request);

        #endregion

        #region ReservationStop           (Request)

        /// <summary>
        /// Delete a reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(AuthorizeRemoteReservationStopRequest Request)

                => EMPClient.AuthorizeRemoteReservationStop(Request);

        #endregion

        #region RemoteStart               (Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)

                => EMPClient.AuthorizeRemoteStart(Request);

        #endregion

        #region RemoteStop                (Request)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(AuthorizeRemoteStopRequest Request)

                => EMPClient.AuthorizeRemoteStop(Request);

        #endregion


        #region GetChargeDetailRecords    (Request)

        ///// <summary>
        ///// Create a new task querying charge detail records from the OICP server.
        ///// </summary>
        ///// <param name="Request">An GetChargeDetailRecords request.</param>
        //public Task<OICPResult<GetChargeDetailRecordsResponse>>

        //    GetChargeDetailRecords(GetChargeDetailRecordsRequest Request)

        //        => EMPClient.GetChargeDetailRecords(Request);

        #endregion


        #region Start()

        public void Start()
        {
            EMPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            EMPServer.Shutdown(Message, Wait);
        }

        #endregion

        public void Dispose()
        { }

    }

}
