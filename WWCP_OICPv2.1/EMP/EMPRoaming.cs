/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP roaming client for EMPs.
    /// </summary>
    public class EMPRoaming : IEMPClient
    {

        #region Properties

        /// <summary>
        /// The EMP client part.
        /// </summary>
        public EMPClient        EMPClient           { get; }

        /// <summary>
        /// The EMP server part.
        /// </summary>
        public EMPServer        EMPServer           { get; }

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger  EMPServerLogger     { get; }

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        public TimeSpan?        RequestTimeout      { get; }


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => EMPServer?.DNSClient;

        #endregion

        #region Events

        // EMPClient logging methods

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestHandler OnPullEVSEDataRequest
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
        /// An event fired whenever a 'pull EVSE data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEDataSOAPRequest
        {

            add
            {
                EMPClient.OnPullEVSEDataSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEDataSOAPResponse
        {

            add
            {
                EMPClient.OnPullEVSEDataSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseHandler OnPullEVSEDataResponse
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
        public event OnPullEVSEStatusRequestHandler OnPullEVSEStatusRequest
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
        /// An event fired whenever a 'pull EVSE status' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEStatusSOAPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEStatusSOAPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseHandler OnPullEVSEStatusResponse
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

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestHandler OnPullEVSEStatusByIdRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEStatusByIdSOAPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEStatusByIdSOAPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseHandler OnPullEVSEStatusByIdResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdResponse -= value;
            }

        }

        #endregion


        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'push authentication data' request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestHandler OnPushAuthenticationDataRequest
        {

            add
            {
                EMPClient.OnPushAuthenticationDataRequest += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'push authentication data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushAuthenticationDataSOAPRequest
        {

            add
            {
                EMPClient.OnPushAuthenticationDataSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushAuthenticationDataSOAPResponse
        {

            add
            {
                EMPClient.OnPushAuthenticationDataSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseHandler OnPushAuthenticationDataResponse
        {

            add
            {
                EMPClient.OnPushAuthenticationDataResponse += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataResponse -= value;
            }

        }

        #endregion


        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestHandler OnAuthorizeRemoteReservationStartRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'reservation start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeRemoteReservationStartSOAPRequest

        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeRemoteReservationStartSOAPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseHandler OnAuthorizeRemoteReservationStartResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestHandler OnAuthorizeRemoteReservationStopRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'reservation stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeRemoteReservationStopSOAPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeRemoteReservationStopSOAPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseHandler OnAuthorizeRemoteReservationStopResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestHandler OnAuthorizeRemoteStartRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an 'authorize remote start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeRemoteStartSOAPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeRemoteStartSOAPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseHandler OnAuthorizeRemoteStartResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestHandler OnAuthorizeRemoteStopRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeRemoteStopSOAPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeRemoteStopSOAPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseHandler OnAuthorizeRemoteStopResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopResponse -= value;
            }

        }

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a 'get charge detail records' request will be send.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestHandler OnGetChargeDetailRecordsRequest
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'get charge detail records' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnGetChargeDetailRecordsSOAPRequest
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'get charge detail records' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnGetChargeDetailRecordsSOAPResponse
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a 'get charge detail records' request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseHandler OnGetChargeDetailRecordsResponse
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsResponse -= value;
            }

        }

        #endregion


        // EMPServer methods

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start SOAP request was received.
        /// </summary>
        public event RequestLogHandler OnAuthorizeStartSOAPRequest
        {

            add
            {
                EMPServer.OnAuthorizeStartSOAPRequest += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start request was received.
        /// </summary>
        public event OnAuthorizeStartRequestHandler OnAuthorizeStartRequest
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
        public event OnAuthorizeStartResponseHandler OnAuthorizeStartResponse
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

        /// <summary>
        /// An event sent whenever a authorize start SOAP response was sent.
        /// </summary>
        public event AccessLogHandler OnAuthorizeStartSOAPResponse
        {

            add
            {
                EMPServer.OnAuthorizeStartSOAPResponse += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStartSOAPResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize stop SOAP request was received.
        /// </summary>
        public event RequestLogHandler OnAuthorizeStopSOAPRequest
        {

            add
            {
                EMPServer.OnAuthorizeStopSOAPRequest += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop SOAP request was received.
        /// </summary>
        public event OnAuthorizeStopRequestHandler OnAuthorizeStopRequest
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
        public event OnAuthorizeStopResponseHandler OnAuthorizeStopResponse
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

        /// <summary>
        /// An event sent whenever a authorize stop SOAP response was sent.
        /// </summary>
        public event AccessLogHandler OnAuthorizeStopSOAPResponse
        {

            add
            {
                EMPServer.OnAuthorizeStopSOAPResponse += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStopSOAPResponse -= value;
            }

        }

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record SOAP request was received.
        /// </summary>
        public event RequestLogHandler OnChargeDetailRecordSOAPRequest
        {

            add
            {
                EMPServer.OnChargeDetailRecordSOAPRequest += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecordSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record request was received.
        /// </summary>
        public event OnChargeDetailRecordRequestHandler OnChargeDetailRecordRequest
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
        public event OnChargeDetailRecordResponseHandler OnChargeDetailRecordResponse
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

        /// <summary>
        /// An event sent whenever a charge detail record SOAP response was sent.
        /// </summary>
        public event AccessLogHandler OnChargeDetailRecordSOAPResponse
        {

            add
            {
                EMPServer.OnChargeDetailRecordSOAPResponse += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecordSOAPResponse -= value;
            }

        }

        #endregion


        // Generic HTTP/SOAP server logging

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                EMPServer.RequestLog += value;
            }

            remove
            {
                EMPServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                EMPServer.AccessLog += value;
            }

            remove
            {
                EMPServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                EMPServer.ErrorLog += value;
            }

            remove
            {
                EMPServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Custom request mappers

        #region CustomPullEVSEData      (SOAP)RequestMapper

        #region CustomPullEVSEDataRequestMapper

        public Func<PullEVSEDataRequest, PullEVSEDataRequest> CustomPullEVSEDataRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEDataRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEDataSOAPRequestMapper

        public Func<PullEVSEDataRequest, XElement, XElement> CustomPullEVSEDataSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEDataRequest>, Acknowledgement<PullEVSEDataRequest>.Builder> CustomPullEVSEDataResponseMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEDataResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEDataResponseMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatus    (SOAP)RequestMapper

        #region CustomPullEVSEStatusRequestMapper

        public Func<PullEVSEStatusRequest, PullEVSEStatusRequest> CustomPullEVSEStatusRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusSOAPRequestMapper

        public Func<PullEVSEStatusRequest, XElement, XElement> CustomPullEVSEStatusSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEStatusRequest>, Acknowledgement<PullEVSEStatusRequest>.Builder> CustomPullEVSEStatusResponseMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusResponseMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusById(SOAP)RequestMapper

        #region CustomPullEVSEStatusByIdRequestMapper

        public Func<PullEVSEStatusByIdRequest, PullEVSEStatusByIdRequest> CustomPullEVSEStatusByIdRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusByIdRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusByIdRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusByIdSOAPRequestMapper

        public Func<PullEVSEStatusByIdRequest, XElement, XElement> CustomPullEVSEStatusByIdSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusByIdSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusByIdSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEStatusByIdRequest>, Acknowledgement<PullEVSEStatusByIdRequest>.Builder> CustomPullEVSEStatusByIdResponseMapper
        {

            get
            {
                return EMPClient.CustomPullEVSEStatusByIdResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPullEVSEStatusByIdResponseMapper = value;
            }

        }

        #endregion


        #region CustomPushAuthenticationData(SOAP)RequestMapper

        #region CustomPushAuthenticationDataRequestMapper

        public Func<PushAuthenticationDataRequest, PushAuthenticationDataRequest> CustomPushAuthenticationDataRequestMapper
        {

            get
            {
                return EMPClient.CustomPushAuthenticationDataRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPushAuthenticationDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushAuthenticationDataSOAPRequestMapper

        public Func<PushAuthenticationDataRequest, XElement, XElement> CustomPushAuthenticationDataSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomPushAuthenticationDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPushAuthenticationDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PushAuthenticationDataRequest>, Acknowledgement<PushAuthenticationDataRequest>.Builder> CustomPushAuthenticationDataResponseMapper
        {

            get
            {
                return EMPClient.CustomPushAuthenticationDataResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomPushAuthenticationDataResponseMapper = value;
            }

        }

        #endregion


        #region CustomAuthorizeRemoteReservationStart(SOAP)RequestMapper

        #region CustomAuthorizeRemoteReservationStartRequestMapper

        public Func<AuthorizeRemoteReservationStartRequest, AuthorizeRemoteReservationStartRequest> CustomAuthorizeRemoteReservationStartRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStartRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStartSOAPRequestMapper

        public Func<AuthorizeRemoteReservationStartRequest, XElement, XElement> CustomAuthorizeRemoteReservationStartSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteReservationStartRequest>, Acknowledgement<AuthorizeRemoteReservationStartRequest>.Builder> CustomAuthorizeRemoteReservationStartResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStartResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStartResponseMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStop(SOAP)RequestMapper

        #region CustomAuthorizeRemoteReservationStopRequestMapper

        public Func<AuthorizeRemoteReservationStopRequest, AuthorizeRemoteReservationStopRequest> CustomAuthorizeRemoteReservationStopRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStopRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStopSOAPRequestMapper

        public Func<AuthorizeRemoteReservationStopRequest, XElement, XElement> CustomAuthorizeRemoteReservationStopSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteReservationStopRequest>, Acknowledgement<AuthorizeRemoteReservationStopRequest>.Builder> CustomAuthorizeRemoteReservationStopResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteReservationStopResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteReservationStopResponseMapper = value;
            }

        }

        #endregion


        #region CustomAuthorizeRemoteStart(SOAP)RequestMapper

        #region CustomAuthorizeRemoteStartRequestMapper

        public Func<AuthorizeRemoteStartRequest, AuthorizeRemoteStartRequest> CustomAuthorizeRemoteStartRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStartRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStartSOAPRequestMapper

        public Func<AuthorizeRemoteStartRequest, XElement, XElement> CustomAuthorizeRemoteStartSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteStartRequest>, Acknowledgement<AuthorizeRemoteStartRequest>.Builder> CustomAuthorizeRemoteStartResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStartResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStartResponseMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStop(SOAP)Mappers

        #region CustomAuthorizeRemoteStopRequestMapper

        public Func<AuthorizeRemoteStopRequest, AuthorizeRemoteStopRequest> CustomAuthorizeRemoteStopRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStopRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStopSOAPRequestMapper

        public Func<AuthorizeRemoteStopRequest, XElement, XElement> CustomAuthorizeRemoteStopSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteStopRequest>, Acknowledgement<AuthorizeRemoteStopRequest>.Builder> CustomAuthorizeRemoteStopResponseMapper
        {

            get
            {
                return EMPClient.CustomAuthorizeRemoteStopResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomAuthorizeRemoteStopResponseMapper = value;
            }

        }

        #endregion


        #region CustomGetChargeDetailRecords(SOAP)RequestMapper

        #region CustomGetChargeDetailRecordsRequestMapper

        public Func<GetChargeDetailRecordsRequest, GetChargeDetailRecordsRequest> CustomGetChargeDetailRecordsRequestMapper
        {

            get
            {
                return EMPClient.CustomGetChargeDetailRecordsRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomGetChargeDetailRecordsRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetChargeDetailRecordsSOAPRequestMapper

        public Func<XElement, XElement> CustomGetChargeDetailRecordsSOAPRequestMapper
        {

            get
            {
                return EMPClient.CustomGetChargeDetailRecordsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomGetChargeDetailRecordsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<GetChargeDetailRecordsResponse, GetChargeDetailRecordsResponse.Builder> CustomGetChargeDetailRecordsResponseMapper
        {

            get
            {
                return EMPClient.CustomGetChargeDetailRecordsResponseMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomGetChargeDetailRecordsResponseMapper = value;
            }

        }


        public CustomMapperDelegate<ChargeDetailRecord> CustomChargeDetailRecordXMLMapper
        {

            get
            {
                return EMPClient.CustomChargeDetailRecordXMLMapper;
            }

            set
            {
                if (value != null)
                    EMPClient.CustomChargeDetailRecordXMLMapper = value;
            }

        }


        #endregion

        #endregion

        #region Constructor(s)

        #region EMPRoaming(EMPClient, EMPServer, ServerLoggingContext = EMPServerLogger.DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPRoaming(EMPClient               EMPClient,
                          EMPServer               EMPServer,
                          String                  ServerLoggingContext   = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator  = null)
        {

            this.EMPClient        = EMPClient;
            this.EMPServer        = EMPServer;
            this.EMPServerLogger  = new EMPServerLogger(EMPServer,
                                                        ServerLoggingContext,
                                                        LogfileCreator);

        }

        #endregion

        #region EMPRoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public EMPRoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          X509Certificate                      ClientCert                      = null,
                          String                               RemoteHTTPVirtualHost           = null,
                          String                               URIPrefix                       = EMPClient.DefaultURIPrefix,
                          String                               HTTPUserAgent                   = EMPClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,

                          String                               ServerName                      = EMPServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort                   = null,
                          String                               ServerURIPrefix                 = EMPServer.DefaultURIPrefix,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          Boolean                              ServerAutoStart                 = false,

                          String                               ClientLoggingContext            = EMPClient.EMPClientLogger.DefaultContext,
                          String                               ServerLoggingContext            = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogfileCreator                  = null,

                          DNSClient                            DNSClient                       = null)

            : this(new EMPClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCert,
                                 RemoteHTTPVirtualHost,
                                 URIPrefix,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new EMPServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 ServerContentType,
                                 ServerRegisterHTTPRootService,
                                 DNSClient,
                                 false),

                   ServerLoggingContext,
                   LogfileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region PullEVSEData      (Request)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">An PullEVSEData request.</param>
        public Task<HTTPResponse<EVSEData>>

            PullEVSEData(PullEVSEDataRequest Request)

                => EMPClient.PullEVSEData(Request);

        #endregion

        #region PullEVSEStatus    (Request)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public Task<HTTPResponse<EVSEStatus>>

            PullEVSEStatus(PullEVSEStatusRequest Request)

                => EMPClient.PullEVSEStatus(Request);

        #endregion

        #region PullEVSEStatusById(Request)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public Task<HTTPResponse<EVSEStatusById>>

            PullEVSEStatusById(PullEVSEStatusByIdRequest Request)

                => EMPClient.PullEVSEStatusById(Request);

        #endregion


        #region PushAuthenticationData(Request)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="Request">An PushAuthenticationData request.</param>
        public Task<HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(PushAuthenticationDataRequest Request)

                => EMPClient.PushAuthenticationData(Request);

        #endregion


        #region ReservationStart(Request)

        /// <summary>
        /// Create a reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request)

                => EMPClient.AuthorizeRemoteReservationStart(Request);

        #endregion

        #region ReservationStop (Request)

        /// <summary>
        /// Delete a reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(AuthorizeRemoteReservationStopRequest Request)

                => EMPClient.AuthorizeRemoteReservationStop(Request);

        #endregion


        #region RemoteStart(Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)

                => EMPClient.AuthorizeRemoteStart(Request);

        #endregion

        #region RemoteStop (Request)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(AuthorizeRemoteStopRequest Request)

                => EMPClient.AuthorizeRemoteStop(Request);

        #endregion


        #region GetChargeDetailRecords(Request)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="Request">An GetChargeDetailRecords request.</param>
        public Task<HTTPResponse<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(GetChargeDetailRecordsRequest Request)

                => EMPClient.GetChargeDetailRecords(Request);

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


    }

}
