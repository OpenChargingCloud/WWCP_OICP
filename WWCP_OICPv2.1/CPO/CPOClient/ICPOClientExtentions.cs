/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    public static class ICPOClientExtentions
    {

        #region PushEVSEData(EVSEDataRecords, OperatorId, OperatorName = null, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
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
        public static async Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient                 ICPOClient,
                         IEnumerable<EVSEDataRecord>     EVSEDataRecords,
                         ChargingStationOperator_Id      OperatorId,
                         String                          OperatorName             = null,
                         ActionTypes                     Action                   = ActionTypes.fullLoad,
                         IncludeEVSEDataRecordsDelegate  IncludeEVSEDataRecords   = null,

                         DateTime?                       Timestamp                = null,
                         CancellationToken?              CancellationToken        = null,
                         EventTracking_Id                EventTrackingId          = null,
                         TimeSpan?                       RequestTimeout           = null)


                => await ICPOClient.PushEVSEData(new PushEVSEDataRequest(IncludeEVSEDataRecords != null
                                                                             ? EVSEDataRecords.Where(evsedatarecord => IncludeEVSEDataRecords(evsedatarecord))
                                                                             : EVSEDataRecords,
                                                                         OperatorId,
                                                                         OperatorName,
                                                                         Action,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(EVSEDataRecord,  OperatorId, OperatorName = null, Action = insert, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE data records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient                 ICPOClient,
                         EVSEDataRecord                  EVSEDataRecord,
                         ChargingStationOperator_Id      OperatorId,
                         String                          OperatorName             = null,
                         ActionTypes                     Action                   = ActionTypes.insert,

                         DateTime?                       Timestamp                = null,
                         CancellationToken?              CancellationToken        = null,
                         EventTracking_Id                EventTrackingId          = null,
                         TimeSpan?                       RequestTimeout           = null)


                => await ICPOClient.PushEVSEData(new PushEVSEDataRequest(new EVSEDataRecord[] { EVSEDataRecord },
                                                                         OperatorId,
                                                                         OperatorName,
                                                                         Action,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(OperatorId, Action, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public static async Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient             ICPOClient,
                         ChargingStationOperator_Id  OperatorId,
                         ActionTypes                 Action,
                         params EVSEDataRecord[]     EVSEDataRecords)


            => await ICPOClient.PushEVSEData(new PushEVSEDataRequest(EVSEDataRecords,
                                                                     OperatorId,
                                                                     Action:          Action,

                                                                     RequestTimeout:  ICPOClient.RequestTimeout));

        #endregion


        #region PushEVSEStatus(EVSEStatusRecords, OperatorId, OperatorName = null, Action = update, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
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
        public static async Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient                   ICPOClient,
                           IEnumerable<EVSEStatusRecord>     EVSEStatusRecords,
                           ChargingStationOperator_Id        OperatorId,
                           String                            OperatorName               = null,
                           ActionTypes                       Action                     = ActionTypes.update,
                           IncludeEVSEStatusRecordsDelegate  IncludeEVSEStatusRecords   = null,

                           DateTime?                         Timestamp                  = null,
                           CancellationToken?                CancellationToken          = null,
                           EventTracking_Id                  EventTrackingId            = null,
                           TimeSpan?                         RequestTimeout             = null)


                => await ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(IncludeEVSEStatusRecords != null
                                                                                 ? EVSEStatusRecords.Where(evsestatusrecord => IncludeEVSEStatusRecords(evsestatusrecord))
                                                                                 : EVSEStatusRecords,
                                                                             OperatorId,
                                                                             OperatorName,
                                                                             Action,

                                                                             Timestamp,
                                                                             CancellationToken,
                                                                             EventTrackingId,
                                                                             RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(EVSEStatusRecord,  OperatorId, OperatorName = null, Action = insert, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE status record onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatusRecord">An EVSE status record.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE status records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient             ICPOClient,
                           EVSEStatusRecord            EVSEStatusRecord,
                           ChargingStationOperator_Id  OperatorId,
                           String                      OperatorName        = null,
                           ActionTypes                 Action              = ActionTypes.update,

                           DateTime?                   Timestamp           = null,
                           CancellationToken?          CancellationToken   = null,
                           EventTracking_Id            EventTrackingId     = null,
                           TimeSpan?                   RequestTimeout      = null)


                => await ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(new EVSEStatusRecord[] { EVSEStatusRecord },
                                                                             OperatorId,
                                                                             OperatorName,
                                                                             Action,

                                                                             Timestamp,
                                                                             CancellationToken,
                                                                             EventTrackingId,
                                                                             RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(OperatorId, Action, params EVSEStatusRecords)

        /// <summary>
        /// Create a new task pushing EVSE status records onto the OICP server.
        /// </summary>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="EVSEStatusRecords">An array of EVSE status records.</param>
        public static async Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient             ICPOClient,
                           ChargingStationOperator_Id  OperatorId,
                           ActionTypes                 Action,
                           params EVSEStatusRecord[]   EVSEStatusRecords)


            => await ICPOClient.PushEVSEStatus(new PushEVSEStatusRequest(EVSEStatusRecords,
                                                                         OperatorId,
                                                                         Action:          Action,

                                                                         RequestTimeout:  ICPOClient.RequestTimeout));

        #endregion

    }

}
