/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    public delegate Boolean IncludeEVSEStatusRecordsDelegate(EVSEStatusRecord  EVSEStatusRecord);

    #region OnPushEVSEData/-Status

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataRequestDelegate (DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        CPOClient                               Sender,
                                                        String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionType                              ActionType,
                                                        ILookup<EVSEOperator, EVSEDataRecord>   EVSEDataRecords,
                                                        UInt32                                  NumberOfEVSEs,
                                                        TimeSpan?                               RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataResponseDelegate(DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        CPOClient                               Sender,
                                                        String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionType                              ActionType,
                                                        ILookup<EVSEOperator, EVSEDataRecord>   EVSEDataRecords,
                                                        UInt32                                  NumberOfEVSEs,
                                                        TimeSpan?                               RequestTimeout,
                                                        eRoamingAcknowledgement                 Result,
                                                        TimeSpan                                Duration);


    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusRequestDelegate (DateTime                                 LogTimestamp,
                                                          DateTime                                 RequestTimestamp,
                                                          CPOClient                                Sender,
                                                          String                                   SenderId,
                                                          EventTracking_Id                         EventTrackingId,
                                                          ActionType                               ActionType,
                                                          ILookup<EVSEOperator, EVSEStatusRecord>  EVSEStatusRecords,
                                                          UInt32                                   NumberOfEVSEs,
                                                          TimeSpan?                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusResponseDelegate(DateTime                                 LogTimestamp,
                                                          DateTime                                 RequestTimestamp,
                                                          CPOClient                                Sender,
                                                          String                                   SenderId,
                                                          EventTracking_Id                         EventTrackingId,
                                                          ActionType                               ActionType,
                                                          ILookup<EVSEOperator, EVSEStatusRecord>  EVSEStatusRecords,
                                                          UInt32                                   NumberOfEVSEs,
                                                          TimeSpan?                                RequestTimeout,
                                                          eRoamingAcknowledgement                  Result,
                                                          TimeSpan                                 Duration);

    #endregion

    #region OnAuthorizeStart/-Stop

    /// <summary>
    /// A delegate called whenever an 'authorize start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartHandler(DateTime                       LogTimestamp,
                                                 DateTime                       RequestTimestamp,
                                                 CPOClient                      Sender,
                                                 String                         SenderId,
                                                 EVSEOperator_Id                OperatorId,
                                                 Auth_Token                     AuthToken,
                                                 EVSE_Id                        EVSEId,
                                                 ChargingSession_Id             SessionId,
                                                 ChargingProduct_Id             PartnerProductId,
                                                 ChargingSession_Id             PartnerSessionId,
                                                 TimeSpan?                      RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to a 'authorize start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStartedHandler(DateTime                     Timestamp,
                                                   CPOClient                    Sender,
                                                   String                       SenderId,
                                                   EVSEOperator_Id              OperatorId,
                                                   Auth_Token                   AuthToken,
                                                   EVSE_Id                      EVSEId,
                                                   ChargingSession_Id           SessionId,
                                                   ChargingProduct_Id           PartnerProductId,
                                                   ChargingSession_Id           PartnerSessionId,
                                                   TimeSpan?                    RequestTimeout,
                                                   eRoamingAuthorizationStart   Result,
                                                   TimeSpan                     Duration);


    /// <summary>
    /// A delegate called whenever an 'authorize stop' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopRequestHandler(DateTime                     LogTimestamp,
                                                       DateTime                     RequestTimestamp,
                                                       CPOClient                    Sender,
                                                       String                       SenderId,
                                                       EVSEOperator_Id              OperatorId,
                                                       ChargingSession_Id           SessionId,
                                                       Auth_Token                   AuthToken,
                                                       EVSE_Id                      EVSEId,
                                                       ChargingSession_Id           PartnerSessionId,
                                                       TimeSpan?                    RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to a 'authorize stop' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStopResponseHandler(DateTime                    Timestamp,
                                                        CPOClient                   Sender,
                                                        String                      SenderId,
                                                        EVSEOperator_Id             OperatorId,
                                                        ChargingSession_Id          SessionId,
                                                        Auth_Token                  AuthToken,
                                                        EVSE_Id                     EVSEId,
                                                        ChargingSession_Id          PartnerSessionId,
                                                        TimeSpan?                   RequestTimeout,
                                                        eRoamingAuthorizationStop   Result,
                                                        TimeSpan                    Duration);

    #endregion

    #region OnSendChargeDetailRecord

    /// <summary>
    /// A delegate called whenever a 'charge detail record' will be send.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordRequestHandler (DateTime                  LogTimestamp,
                                                                 DateTime                  RequestTimestamp,
                                                                 CPOClient                 Sender,
                                                                 String                    SenderId,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 ChargeDetailRecord        ChargeDetailRecord,
                                                                 TimeSpan?                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a sent 'charge detail record' had been received.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordResponseHandler(DateTime                  Timestamp,
                                                                 DateTime                  RequestTimestamp,
                                                                 CPOClient                 Sender,
                                                                 String                    SenderId,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 ChargeDetailRecord        ChargeDetailRecord,
                                                                 TimeSpan?                 RequestTimeout,
                                                                 eRoamingAcknowledgement   Result,
                                                                 TimeSpan                  Duration);

    #endregion

    #region OnPullAuthenticationData

    /// <summary>
    /// A delegate called whenever a 'pull authentication data' request will be send.
    /// </summary>
    public delegate Task OnPullAuthenticationDataRequestHandler (DateTime                     LogTimestamp,
                                                                 DateTime                     RequestTimestamp,
                                                                 CPOClient                    Sender,
                                                                 String                       SenderId,
                                                                 EventTracking_Id             EventTrackingId,
                                                                 EVSEOperator_Id              OperatorId,
                                                                 TimeSpan?                    RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull authentication data' request had been received.
    /// </summary>
    public delegate Task OnPullAuthenticationDataResponseHandler(DateTime                     Timestamp,
                                                                 CPOClient                    Sender,
                                                                 String                       SenderId,
                                                                 EventTracking_Id             EventTrackingId,
                                                                 EVSEOperator_Id              OperatorId,
                                                                 TimeSpan?                    RequestTimeout,
                                                                 eRoamingAuthenticationData   Result,
                                                                 TimeSpan                     Duration);

    #endregion

}
