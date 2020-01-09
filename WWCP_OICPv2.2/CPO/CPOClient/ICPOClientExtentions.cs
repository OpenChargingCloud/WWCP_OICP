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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// Extention methods for the OICP CPO client interface.
    /// </summary>
    public static class ICPOClientExtentions
    {

        #region PushEVSEData(OperatorEVSEData, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorEVSEData">An operator EVSE data.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient                 ICPOClient,
                         OperatorEVSEData                OperatorEVSEData,
                         ActionTypes                     Action              = ActionTypes.fullLoad,

                         DateTime?                       Timestamp           = null,
                         CancellationToken?              CancellationToken   = null,
                         EventTracking_Id                EventTrackingId     = null,
                         TimeSpan?                       RequestTimeout      = null)


                => ICPOClient.PushEVSEData(new PushEVSEDataRequest(OperatorEVSEData,
                                                                   Action,

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(EVSEDataRecords, OperatorId, OperatorName = null, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient                 ICPOClient,
                         IEnumerable<EVSEDataRecord>     EVSEDataRecords,
                         Operator_Id                     OperatorId,
                         String                          OperatorName             = null,
                         ActionTypes                     Action                   = ActionTypes.fullLoad,
                         IncludeEVSEDataRecordsDelegate  IncludeEVSEDataRecords   = null,

                         DateTime?                       Timestamp                = null,
                         CancellationToken?              CancellationToken        = null,
                         EventTracking_Id                EventTrackingId          = null,
                         TimeSpan?                       RequestTimeout           = null)


                => ICPOClient.PushEVSEData(new PushEVSEDataRequest(new OperatorEVSEData(IncludeEVSEDataRecords != null
                                                                                            ? EVSEDataRecords.Where(evsedatarecord => IncludeEVSEDataRecords(evsedatarecord))
                                                                                            : EVSEDataRecords,
                                                                                        OperatorId,
                                                                                        OperatorName),
                                                                   Action,

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(EVSEDataRecord,  OperatorId, OperatorName = null, Action = insert, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE data records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient     ICPOClient,
                         EVSEDataRecord      EVSEDataRecord,
                         Operator_Id         OperatorId,
                         String              OperatorName        = null,
                         ActionTypes         Action              = ActionTypes.insert,

                         DateTime?           Timestamp           = null,
                         CancellationToken?  CancellationToken   = null,
                         EventTracking_Id    EventTrackingId     = null,
                         TimeSpan?           RequestTimeout      = null)


                => ICPOClient.PushEVSEData(new PushEVSEDataRequest(new OperatorEVSEData(new EVSEDataRecord[] { EVSEDataRecord },
                                                                                        OperatorId,
                                                                                        OperatorName),
                                                                   Action,

                                                                   Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(OperatorId, Action, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient          ICPOClient,
                         Operator_Id              OperatorId,
                         ActionTypes              Action,
                         params EVSEDataRecord[]  EVSEDataRecords)


            => ICPOClient.PushEVSEData(new PushEVSEDataRequest(new OperatorEVSEData(EVSEDataRecords,
                                                                                    OperatorId),
                                                               Action:          Action,

                                                               RequestTimeout:  ICPOClient.RequestTimeout));

        #endregion


        #region PushEVSEStatus(OperatorEVSEStatus, Action = update, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorEVSEStatus">An operator EVSE status.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient                   ICPOClient,
                           OperatorEVSEStatus                OperatorEVSEStatus,
                           ActionTypes                       Action              = ActionTypes.update,

                           DateTime?                         Timestamp           = null,
                           CancellationToken?                CancellationToken   = null,
                           EventTracking_Id                  EventTrackingId     = null,
                           TimeSpan?                         RequestTimeout      = null)


                => ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(OperatorEVSEStatus,
                                                                       Action,

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(EVSEStatusRecords, OperatorId, OperatorName = null, Action = update, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient                   ICPOClient,
                           IEnumerable<EVSEStatusRecord>     EVSEStatusRecords,
                           Operator_Id                       OperatorId,
                           String                            OperatorName               = null,
                           ActionTypes                       Action                     = ActionTypes.update,
                           IncludeEVSEStatusRecordsDelegate  IncludeEVSEStatusRecords   = null,

                           DateTime?                         Timestamp                  = null,
                           CancellationToken?                CancellationToken          = null,
                           EventTracking_Id                  EventTrackingId            = null,
                           TimeSpan?                         RequestTimeout             = null)


                => ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(new OperatorEVSEStatus(IncludeEVSEStatusRecords != null
                                                                                                  ? EVSEStatusRecords.Where(evsestatusrecord => IncludeEVSEStatusRecords(evsestatusrecord))
                                                                                                  : EVSEStatusRecords,
                                                                                              OperatorId,
                                                                                              OperatorName),
                                                                       Action,

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(EVSEStatusRecord,  OperatorId, OperatorName = null, Action = insert, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE status record onto the OICP server.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEStatusRecord">An EVSE status record.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE status records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient     ICPOClient,
                           EVSEStatusRecord    EVSEStatusRecord,
                           Operator_Id         OperatorId,
                           String              OperatorName        = null,
                           ActionTypes         Action              = ActionTypes.update,

                           DateTime?           Timestamp           = null,
                           CancellationToken?  CancellationToken   = null,
                           EventTracking_Id    EventTrackingId     = null,
                           TimeSpan?           RequestTimeout      = null)


                => ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(new OperatorEVSEStatus(new EVSEStatusRecord[] { EVSEStatusRecord },
                                                                                              OperatorId,
                                                                                              OperatorName),
                                                                       Action,

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(OperatorId, Action, params EVSEStatusRecords)

        /// <summary>
        /// Create a new task pushing EVSE status records onto the OICP server.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="EVSEStatusRecords">An array of EVSE status records.</param>
        public static Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient            ICPOClient,
                           Operator_Id                OperatorId,
                           ActionTypes                Action,
                           params EVSEStatusRecord[]  EVSEStatusRecords)


            => ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(new OperatorEVSEStatus(EVSEStatusRecords,
                                                                                          OperatorId),
                                                                   Action:          Action,

                                                                   RequestTimeout:  ICPOClient.RequestTimeout));

        #endregion


        #region AuthorizeStart(OperatorId, UID, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, ...)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">An Charging Station Operator identification.</param>
        /// <param name="UID">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<AuthorizationStart>>

            AuthorizeStart(this ICPOClient        ICPOClient,
                           Operator_Id            OperatorId,
                           UID                    UID,
                           EVSE_Id?               EVSEId                = null,
                           PartnerProduct_Id?     PartnerProductId      = null,
                           Session_Id?            SessionId             = null,
                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,

                           DateTime?              Timestamp             = null,
                           CancellationToken?     CancellationToken     = null,
                           EventTracking_Id       EventTrackingId       = null,
                           TimeSpan?              RequestTimeout        = null)


                => ICPOClient.AuthorizeStart(new AuthorizeStartRequest(OperatorId,
                                                                       Identification.FromUID(UID),
                                                                       EVSEId,
                                                                       PartnerProductId,
                                                                       SessionId,
                                                                       CPOPartnerSessionId,
                                                                       EMPPartnerSessionId,

                                                                       Timestamp,
                                                                       CancellationToken,
                                                                       EventTrackingId,
                                                                       RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region AuthorizeStop (OperatorId, SessionId, UID, EVSEId = null, PartnerSessionId = null, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="UID">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<AuthorizationStop>>

            AuthorizeStop(this ICPOClient        ICPOClient,
                          Operator_Id            OperatorId,
                          Session_Id             SessionId,
                          UID                    UID,
                          EVSE_Id?               EVSEId                = null,
                          CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                          EMPPartnerSession_Id?  EMPPartnerSessionId   = null,

                          DateTime?              Timestamp             = null,
                          CancellationToken?     CancellationToken     = null,
                          EventTracking_Id       EventTrackingId       = null,
                          TimeSpan?              RequestTimeout        = null)


                 => ICPOClient.AuthorizeStop(new AuthorizeStopRequest(OperatorId,
                                                                      SessionId,
                                                                      Identification.FromUID(UID),
                                                                      EVSEId,
                                                                      CPOPartnerSessionId,
                                                                      EMPPartnerSessionId,

                                                                      Timestamp,
                                                                      CancellationToken,
                                                                      EventTrackingId,
                                                                      RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

        #region SendChargeDetailRecord(ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>>

            SendChargeDetailRecord(this ICPOClient     ICPOClient,
                                   ChargeDetailRecord  ChargeDetailRecord,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)


                => ICPOClient.SendChargeDetailRecord(new SendChargeDetailRecordRequest(ChargeDetailRecord,

                                                                                       Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion


        #region PullAuthenticationData(OperatorId, ...)

        /// <summary>
        /// Pull authentication data from the OICP server.
        /// </summary>
        /// <param name="ICPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">An Charging Station Operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<AuthenticationData>>

            PullAuthenticationData(this ICPOClient     ICPOClient,
                                   Operator_Id         OperatorId,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id    EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)


                => ICPOClient.PullAuthenticationData(new PullAuthenticationDataRequest(OperatorId,

                                                                                       Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout ?? ICPOClient.RequestTimeout));

        #endregion

    }

}
