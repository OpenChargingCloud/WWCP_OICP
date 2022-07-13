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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.Mobile
{

    /// <summary>
    /// Extention methods for the Mobile client interface.
    /// </summary>
    public static class IMobileClientExtentions
    {

        #region MobileAuthorizeStart(EVSEId, EVCOId, PIN,                       PartnerProductId = null, GetNewSession = null, ...)

        /// <summary>
        /// Authorize for starting a remote charging session (later).
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOId">The eMA identification.</param>
        /// <param name="PIN">The PIN of the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(this IMobileClient    IMobileClient,
                                 EVSE_Id               EVSEId,
                                 EVCO_Id               EVCOId,
                                 String                PIN,
                                 PartnerProduct_Id?    PartnerProductId   = null,
                                 Boolean?              GetNewSession      = null,

                                 DateTime?             Timestamp          = null,
                                 CancellationToken?    CancellationToken  = null,
                                 EventTracking_Id      EventTrackingId    = null,
                                 TimeSpan?             RequestTimeout     = null)


            => IMobileClient.MobileAuthorizeStart(new MobileAuthorizeStartRequest(EVSEId,
                                                                                  new QRCodeIdentification(EVCOId,
                                                                                                           PIN),
                                                                                  PartnerProductId,
                                                                                  GetNewSession,

                                                                                  Timestamp,
                                                                                  CancellationToken,
                                                                                  EventTrackingId,
                                                                                  RequestTimeout ?? IMobileClient.RequestTimeout));

        #endregion

        #region MobileAuthorizeStart(EVSEId, EVCOId, HashedPIN, Function, Salt, PartnerProductId = null, GetNewSession = null, ...)

        /// <summary>
        /// Authorize for starting a remote charging session (later).
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOId">The eMA identification.</param>
        /// <param name="HashedPIN">The PIN of the eMA identification.</param>
        /// <param name="Function">The crypto hash function of the eMA identification.</param>
        /// <param name="Salt">The Salt of the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(this IMobileClient    IMobileClient,
                                 EVSE_Id               EVSEId,
                                 EVCO_Id               EVCOId,
                                 String                HashedPIN,
                                 PINCrypto             Function,
                                 String                Salt,
                                 PartnerProduct_Id?    PartnerProductId    = null,
                                 Boolean?              GetNewSession       = null,

                                 DateTime?             Timestamp           = null,
                                 CancellationToken?    CancellationToken   = null,
                                 EventTracking_Id      EventTrackingId     = null,
                                 TimeSpan?             RequestTimeout      = null)


            => IMobileClient.MobileAuthorizeStart(new MobileAuthorizeStartRequest(EVSEId,
                                                                                  new QRCodeIdentification(EVCOId,
                                                                                                           HashedPIN,
                                                                                                           Function,
                                                                                                           Salt),
                                                                                  PartnerProductId,
                                                                                  GetNewSession,

                                                                                  Timestamp,
                                                                                  CancellationToken,
                                                                                  EventTrackingId,
                                                                                  RequestTimeout ?? IMobileClient.RequestTimeout));

        #endregion

        #region MobileAuthorizeStart(EVSEId, QRCodeIdentification,              PartnerProductId = null, GetNewSession = null, ...)

        /// <summary>
        /// Authorize for starting a remote charging session (later).
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOIdWithPIN">The eMA identification with its PIN.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(this IMobileClient    IMobileClient,
                                 EVSE_Id               EVSEId,
                                 QRCodeIdentification  QRCodeIdentification,
                                 PartnerProduct_Id?    PartnerProductId    = null,
                                 Boolean?              GetNewSession       = null,

                                 DateTime?             Timestamp           = null,
                                 CancellationToken?    CancellationToken   = null,
                                 EventTracking_Id      EventTrackingId     = null,
                                 TimeSpan?             RequestTimeout      = null)


                => IMobileClient.MobileAuthorizeStart(new MobileAuthorizeStartRequest(EVSEId,
                                                                                      QRCodeIdentification,
                                                                                      PartnerProductId,
                                                                                      GetNewSession,

                                                                                      Timestamp,
                                                                                      CancellationToken,
                                                                                      EventTrackingId,
                                                                                      RequestTimeout ?? IMobileClient.RequestTimeout));

        #endregion


        #region MobileRemoteStart(SessionId, ...)

        /// <summary>
        /// Start a remote charging session.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>>

            MobileRemoteStart(this IMobileClient  IMobileClient,
                              Session_Id          SessionId,

                              DateTime?           Timestamp           = null,
                              CancellationToken?  CancellationToken   = null,
                              EventTracking_Id    EventTrackingId     = null,
                              TimeSpan?           RequestTimeout      = null)


                => IMobileClient.MobileRemoteStart(new MobileRemoteStartRequest(SessionId,

                                                                                Timestamp,
                                                                                CancellationToken,
                                                                                EventTrackingId,
                                                                                RequestTimeout ?? IMobileClient.RequestTimeout));

        #endregion

        #region MobileRemoteStop (SessionId, ...)

        /// <summary>
        /// Stop a remote charging session.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>>

            MobileRemoteStop(this IMobileClient  IMobileClient,
                             Session_Id          SessionId,

                             DateTime?           Timestamp           = null,
                             CancellationToken?  CancellationToken   = null,
                             EventTracking_Id    EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null)


                => IMobileClient.MobileRemoteStop(new MobileRemoteStopRequest(SessionId,

                                                                              Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              RequestTimeout ?? IMobileClient.RequestTimeout));

        #endregion


    }

}
