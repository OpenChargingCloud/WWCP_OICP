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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Mobile
{

    /// <summary>
    /// The common interface of all OICP Mobile clients.
    /// </summary>
    public interface IMobileClient : IHTTPClient
    {

        #region Properties

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        TimeSpan?                        RequestTimeout            { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Mobile Authorization requests.
        /// </summary>
        String                           MobileAuthorizationURI    { get; }

        /// <summary>
        /// The attached OICP Mobile client (HTTP/SOAP client) logger.
        /// </summary>
        MobileClient.MobileClientLogger  Logger                    { get; }

        #endregion

        #region Events

        #region OnMobileAuthorizeStartRequest

        /// <summary>
        /// An event fired whenever a 'mobile authorize start' request will be send.
        /// </summary>
        event OnMobileAuthorizeStartRequestHandler   OnMobileAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a 'mobile authorize start' SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler                OnMobileAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'mobile authorize start' SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler               OnMobileAuthorizeStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'mobile authorize start' request had been received.
        /// </summary>
        event OnMobileAuthorizeStartResponseHandler  OnMobileAuthorizeStartResponse;

        #endregion

        #region OnMobileRemoteStart

        /// <summary>
        /// An event fired whenever a MobileRemoteStart request will be send.
        /// </summary>
        event OnMobileRemoteStartRequestDelegate   OnMobileRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a MobileRemoteStart SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler              OnMobileRemoteStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStart SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler             OnMobileRemoteStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStart request had been received.
        /// </summary>
        event OnMobileRemoteStartResponseDelegate  OnMobileRemoteStartResponse;

        #endregion

        #region OnMobileRemoteStop

        /// <summary>
        /// An event fired whenever a MobileRemoteStop request will be send.
        /// </summary>
        event OnMobileRemoteStopRequestDelegate    OnMobileRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a MobileRemoteStop SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler              OnMobileRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStop SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler             OnMobileRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStop request had been received.
        /// </summary>
        event OnMobileRemoteStopResponseDelegate   OnMobileRemoteStopResponse;

        #endregion

        #endregion


        /// <summary>
        /// Authorize for starting a remote charging session (later).
        /// </summary>
        /// <param name="Request">A MobileAuthorizeStart request.</param>
        Task<HTTPResponse<MobileAuthorizationStart>>
            MobileAuthorizeStart(MobileAuthorizeStartRequest  Request);


        /// <summary>
        /// Start a remote charging session.
        /// </summary>
        /// <param name="Request">A MobileRemoteStart request.</param>
        Task<HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>>
            MobileRemoteStart   (MobileRemoteStartRequest     Request);


        /// <summary>
        /// Stop a remote charging session.
        /// </summary>
        /// <param name="Request">A MobileRemoteStop request.</param>
        Task<HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>>
            MobileRemoteStop    (MobileRemoteStopRequest      Request);


    }

}
