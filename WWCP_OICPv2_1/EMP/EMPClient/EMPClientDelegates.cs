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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    #region OnPushAuthenticationData

    /// <summary>
    /// A delegate called whenever a 'push authentication data' request will be send.
    /// </summary>
    public delegate Task OnPushAuthenticationDataRequestHandler (DateTime                                 Timestamp,
                                                                 EMPClient                                Sender,
                                                                 String                                   SenderId,
                                                                 EventTracking_Id                         EventTrackingId,
                                                                 IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                                                 ActionType                               OICPAction,
                                                                 TimeSpan?                                QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'push authentication data' request had been received.
    /// </summary>
    public delegate Task OnPushAuthenticationDataResponseHandler(DateTime                                 Timestamp,
                                                                 EMPClient                                Sender,
                                                                 String                                   SenderId,
                                                                 EventTracking_Id                         EventTrackingId,
                                                                 IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                                                 ActionType                               OICPAction,
                                                                 TimeSpan?                                QueryTimeout,
                                                                 eRoamingAcknowledgement                  Result,
                                                                 TimeSpan                                 Duration);

    #endregion

    #region OnReservationStart/-Stop

    /// <summary>
    /// A delegate called whenever a 'reservation start' request will be send.
    /// </summary>
    public delegate Task OnReservationStartRequestHandler (DateTime                  Timestamp,
                                                           EMPClient                 Sender,
                                                           String                    SenderId,
                                                           EventTracking_Id          EventTrackingId,
                                                           EVSP_Id                   ProviderId,
                                                           EVSE_Id                   EVSEId,
                                                           eMA_Id                    eMAId,
                                                           ChargingSession_Id        SessionId,
                                                           ChargingSession_Id        PartnerSessionId,
                                                           ChargingProduct_Id        PartnerProductId,
                                                           TimeSpan?                 QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'reservation start' request had been received.
    /// </summary>
    public delegate Task OnReservationStartResponseHandler(DateTime                  Timestamp,
                                                           EMPClient                 Sender,
                                                           String                    SenderId,
                                                           EventTracking_Id          EventTrackingId,
                                                           EVSP_Id                   ProviderId,
                                                           EVSE_Id                   EVSEId,
                                                           eMA_Id                    eMAId,
                                                           ChargingSession_Id        SessionId,
                                                           ChargingSession_Id        PartnerSessionId,
                                                           ChargingProduct_Id        PartnerProductId,
                                                           TimeSpan?                 QueryTimeout,
                                                           eRoamingAcknowledgement   Result,
                                                           TimeSpan                  Duration);


    /// <summary>
    /// A delegate called whenever a reservation stop request will be send.
    /// </summary>
    public delegate Task OnReservationStopRequestHandler  (DateTime                  Timestamp,
                                                           EMPClient                 Sender,
                                                           String                    SenderId,
                                                           EventTracking_Id          EventTrackingId,
                                                           ChargingSession_Id        SessionId,
                                                           EVSP_Id                   ProviderId,
                                                           EVSE_Id                   EVSEId,
                                                           ChargingSession_Id        PartnerSessionId,
                                                           TimeSpan?                 QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for a reservation stop request had been received.
    /// </summary>
    public delegate Task OnReservationStopResponseHandler (DateTime                  Timestamp,
                                                           EMPClient                 Sender,
                                                           String                    SenderId,
                                                           EventTracking_Id          EventTrackingId,
                                                           ChargingSession_Id        SessionId,
                                                           EVSP_Id                   ProviderId,
                                                           EVSE_Id                   EVSEId,
                                                           ChargingSession_Id        PartnerSessionId,
                                                           TimeSpan?                 QueryTimeout,
                                                           eRoamingAcknowledgement   Result,
                                                           TimeSpan                  Duration);

    #endregion

    #region OnRemoteStart/-Stop

    /// <summary>
    /// A delegate called whenever an 'authorize remote start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartRequestHandler (DateTime                  Timestamp,
                                                               EMPClient                 Sender,
                                                               String                    SenderId,
                                                               EventTracking_Id          EventTrackingId,
                                                               EVSP_Id                   ProviderId,
                                                               EVSE_Id                   EVSEId,
                                                               eMA_Id                    eMAId,
                                                               ChargingSession_Id        SessionId,
                                                               ChargingSession_Id        PartnerSessionId,
                                                               ChargingProduct_Id        PartnerProductId,
                                                               TimeSpan?                 QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for an 'authorize remote start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartResponseHandler(DateTime                  Timestamp,
                                                               EMPClient                 Sender,
                                                               String                    SenderId,
                                                               EventTracking_Id          EventTrackingId,
                                                               EVSP_Id                   ProviderId,
                                                               EVSE_Id                   EVSEId,
                                                               eMA_Id                    eMAId,
                                                               ChargingSession_Id        SessionId,
                                                               ChargingSession_Id        PartnerSessionId,
                                                               ChargingProduct_Id        PartnerProductId,
                                                               TimeSpan?                 QueryTimeout,
                                                               eRoamingAcknowledgement   Result,
                                                               TimeSpan                  Duration);


    /// <summary>
    /// A delegate called whenever an 'authorize remote stop' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopRequestHandler  (DateTime                  Timestamp,
                                                               EMPClient                 Sender,
                                                               String                    SenderId,
                                                               EventTracking_Id          EventTrackingId,
                                                               ChargingSession_Id        SessionId,
                                                               EVSP_Id                   ProviderId,
                                                               EVSE_Id                   EVSEId,
                                                               ChargingSession_Id        PartnerSessionId,
                                                               TimeSpan?                 QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for an 'authorize remote stop' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopResponseHandler (DateTime                  Timestamp,
                                                               EMPClient                 Sender,
                                                               String                    SenderId,
                                                               EventTracking_Id          EventTrackingId,
                                                               ChargingSession_Id        SessionId,
                                                               EVSP_Id                   ProviderId,
                                                               EVSE_Id                   EVSEId,
                                                               ChargingSession_Id        PartnerSessionId,
                                                               TimeSpan?                 QueryTimeout,
                                                               eRoamingAcknowledgement   Result,
                                                               TimeSpan                  Duration);

    #endregion

    #region OnGetChargeDetailRecords

    /// <summary>
    /// A delegate called whenever a 'get charge detail records' request will be send.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsRequestHandler (DateTime                         LogTimestamp,
                                                                 DateTime                         RequestTimestamp,
                                                                 EMPClient                        Sender,
                                                                 String                           SenderId,
                                                                 EventTracking_Id                 EventTrackingId,
                                                                 EVSP_Id                          ProviderId,
                                                                 DateTime                         From,
                                                                 DateTime                         To,
                                                                 TimeSpan?                        QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'get charge detail records' request had been received.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsResponseHandler(DateTime                         Timestamp,
                                                                 EMPClient                        Sender,
                                                                 String                           SenderId,
                                                                 EventTracking_Id                 EventTrackingId,
                                                                 EVSP_Id                          ProviderId,
                                                                 DateTime                         From,
                                                                 DateTime                         To,
                                                                 TimeSpan?                        QueryTimeout,
                                                                 IEnumerable<ChargeDetailRecord>  Result,
                                                                 TimeSpan                         Duration);

    #endregion

}
