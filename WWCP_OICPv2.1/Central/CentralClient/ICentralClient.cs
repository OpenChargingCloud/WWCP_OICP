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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// The common interface of all OICP Central clients.
    /// </summary>
    public interface ICentralClient : IHTTPClient
    {

        #region Properties

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        TimeSpan?                          RequestTimeout      { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Reservation requests.
        /// </summary>
        String                             ReservationURI      { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Authorization requests.
        /// </summary>
        String                             AuthorizationURI    { get; }

        /// <summary>
        /// The attached OICP Mobile client (HTTP/SOAP client) logger.
        /// </summary>
        CentralClient.CentralClientLogger  Logger              { get; }

        #endregion

        #region Events

        // Towards CPOs

        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote reservation start' request will be send.
        /// </summary>
        event OnAuthorizeRemoteReservationStartRequestHandler   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote reservation start' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                           OnAuthorizeRemoteReservationStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote reservation start' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                          OnAuthorizeRemoteReservationStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote reservation start' request had been received.
        /// </summary>
        event OnAuthorizeRemoteReservationStartResponseHandler  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'authorize remote reservation stop' request will be send.
        /// </summary>
        event OnAuthorizeRemoteReservationStopRequestHandler   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event fired whenever a 'authorize remote reservation stop' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                          OnAuthorizeRemoteReservationStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote reservation stop' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                         OnAuthorizeRemoteReservationStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote reservation stop' request had been received.
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
        /// An event fired whenever a 'authorize remote stop' request will be send.
        /// </summary>
        event OnAuthorizeRemoteStopRequestHandler   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a 'authorize remote stop' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler               OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote stop' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler              OnAuthorizeRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote stop' request had been received.
        /// </summary>
        event OnAuthorizeRemoteStopResponseHandler  OnAuthorizeRemoteStopResponse;

        #endregion


        // Towards EMPs

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        event OnAuthorizeStartRequestHandler   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler          OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler         OnAuthorizeStartSOAPResponse;

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        event OnAuthorizeStartResponseHandler  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize stop' request will be send.
        /// </summary>
        event OnAuthorizeStopRequestHandler   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize stop' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler         OnAuthorizeStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler        OnAuthorizeStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' request had been received.
        /// </summary>
        event OnAuthorizeStopResponseHandler  OnAuthorizeStopResponse;

        #endregion

        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a 'send charge detail record' request will be send.
        /// </summary>
        event OnSendChargeDetailRecordRequestHandler   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a 'send charge detail record' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                  OnSendChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'send charge detail record' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                 OnSendChargeDetailRecordSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'send charge detail record' request had been received.
        /// </summary>
        event OnSendChargeDetailRecordResponseHandler  OnSendChargeDetailRecordResponse;

        #endregion

        #endregion


        #region Towards CPOs...

        /// <summary>
        /// Reserve an EVSE at an operator.
        /// </summary>
        /// <param name="Request">A AuthorizeRemoteReservationStart request.</param>
        Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>>
            AuthorizeRemoteReservationStart(EMP.AuthorizeRemoteReservationStartRequest Request);


        /// <summary>
        /// Delete a reservation of an EVSE at an operator.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>>
            AuthorizeRemoteReservationStop (EMP.AuthorizeRemoteReservationStopRequest  Request);


        /// <summary>
        /// Start a remote charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>>
            AuthorizeRemoteStart(EMP.AuthorizeRemoteStartRequest Request);


        /// <summary>
        /// Stop a remote charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>>
            AuthorizeRemoteStop (EMP.AuthorizeRemoteStopRequest  Request);

        #endregion

        #region Towards EMPs...



        #endregion

    }

}