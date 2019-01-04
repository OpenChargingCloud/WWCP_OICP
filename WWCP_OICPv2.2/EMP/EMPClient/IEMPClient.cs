/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// The common interface of all OICP EMP clients.
    /// </summary>
    public interface IEMPClient : IHTTPClient
    {

        #region Properties

        /// <summary>
        /// An optional default e-mobility provider identification.
        /// </summary>
        Provider_Id?     DefaultProviderId       { get; }

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        TimeSpan?        RequestTimeout          { get; }

        #endregion

        #region Events

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' request will be send.
        /// </summary>
        event OnPullEVSEDataRequestHandler   OnPullEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler        OnPullEVSEDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler       OnPullEVSEDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' request had been received.
        /// </summary>
        event OnPullEVSEDataResponseHandler  OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' request will be send.
        /// </summary>
        event OnPullEVSEStatusRequestHandler   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler          OnPullEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler         OnPullEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' request had been received.
        /// </summary>
        event OnPullEVSEStatusResponseHandler  OnPullEVSEStatusResponse;

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' request will be send.
        /// </summary>
        event OnPullEVSEStatusByIdRequestHandler   OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler              OnPullEVSEStatusByIdSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler             OnPullEVSEStatusByIdSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' request had been received.
        /// </summary>
        event OnPullEVSEStatusByIdResponseHandler  OnPullEVSEStatusByIdResponse;

        #endregion


        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'push authentication data' request will be send.
        /// </summary>
        event OnPushAuthenticationDataRequestHandler   OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a 'push authentication data' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                  OnPushAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                 OnPushAuthenticationDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' request had been received.
        /// </summary>
        event OnPushAuthenticationDataResponseHandler  OnPushAuthenticationDataResponse;

        #endregion


        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation start' request will be send.
        /// </summary>
        event OnAuthorizeRemoteReservationStartRequestHandler   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event fired whenever a 'reservation start' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                           OnAuthorizeRemoteReservationStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                          OnAuthorizeRemoteReservationStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' request had been received.
        /// </summary>
        event OnAuthorizeRemoteReservationStartResponseHandler  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation stop' request will be send.
        /// </summary>
        event OnAuthorizeRemoteReservationStopRequestHandler   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event fired whenever a 'reservation stop' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                          OnAuthorizeRemoteReservationStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                         OnAuthorizeRemoteReservationStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' request had been received.
        /// </summary>
        event OnAuthorizeRemoteReservationStopResponseHandler  OnAuthorizeRemoteReservationStopResponse;

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote start' request will be send.
        /// </summary>
        event OnAuthorizeRemoteStartRequestHandler   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote start' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                OnAuthorizeRemoteStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler               OnAuthorizeRemoteStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' request had been received.
        /// </summary>
        event OnAuthorizeRemoteStartResponseHandler  OnAuthorizeRemoteStartResponse;

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' request will be send.
        /// </summary>
        event OnAuthorizeRemoteStopRequestHandler   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler               OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler              OnAuthorizeRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' request had been received.
        /// </summary>
        event OnAuthorizeRemoteStopResponseHandler  OnAuthorizeRemoteStopResponse;

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a 'get charge detail records' request will be send.
        /// </summary>
        event OnGetChargeDetailRecordsRequestHandler   OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event fired whenever a 'get charge detail records' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                  OnGetChargeDetailRecordsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'get charge detail records' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                 OnGetChargeDetailRecordsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a 'get charge detail records' request was received.
        /// </summary>
        event OnGetChargeDetailRecordsResponseHandler  OnGetChargeDetailRecordsResponse;

        #endregion

        #endregion

        //event OnDataReadDelegate OnDataRead;


        Task<HTTPResponse<PullEVSEDataResponse>>
            PullEVSEData(PullEVSEDataRequest Request);

        Task<HTTPResponse<EVSEStatus>>
            PullEVSEStatus(PullEVSEStatusRequest Request);

        Task<HTTPResponse<EVSEStatusById>>
            PullEVSEStatusById(PullEVSEStatusByIdRequest Request);


        Task<HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>>
            PushAuthenticationData(PushAuthenticationDataRequest Request);


        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>
            AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request);

        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>
            AuthorizeRemoteReservationStop (AuthorizeRemoteReservationStopRequest   Request);


        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>>
            AuthorizeRemoteStart(AuthorizeRemoteStartRequest Request);

        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>>
            AuthorizeRemoteStop(AuthorizeRemoteStopRequest Request);


        Task<HTTPResponse<GetChargeDetailRecordsResponse>>
            GetChargeDetailRecords(GetChargeDetailRecordsRequest Request);

    }

}
