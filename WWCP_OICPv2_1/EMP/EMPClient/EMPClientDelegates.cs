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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A delegate called whenever an authorize temote start request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartRequestHandler(DateTime             Timestamp,
                                                              EMPClient            Sender,
                                                              String               SenderId,
                                                              EVSP_Id              ProviderId,
                                                              EVSE_Id              EVSEId,
                                                              eMA_Id               eMAId,
                                                              ChargingSession_Id   SessionId,
                                                              ChargingSession_Id   PartnerSessionId,
                                                              ChargingProduct_Id   PartnerProductId,
                                                              TimeSpan?            QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for an authorize remote start request was received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartResponseHandler(DateTime                  Timestamp,
                                                               EMPClient                 Sender,
                                                               String                    SenderId,
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
    /// A delegate called whenever an authorize remote stop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopRequestHandler(DateTime             Timestamp,
                                                             EMPClient            Sender,
                                                             String               SenderId,
                                                             ChargingSession_Id   SessionId,
                                                             EVSP_Id              ProviderId,
                                                             EVSE_Id              EVSEId,
                                                             ChargingSession_Id   PartnerSessionId,
                                                             TimeSpan?            QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for an authorize remote stop request was received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopResponseHandler(DateTime                  Timestamp,
                                                              EMPClient                 Sender,
                                                              String                    SenderId,
                                                              ChargingSession_Id        SessionId,
                                                              EVSP_Id                   ProviderId,
                                                              EVSE_Id                   EVSEId,
                                                              ChargingSession_Id        PartnerSessionId,
                                                              TimeSpan?                 QueryTimeout,
                                                              eRoamingAcknowledgement   Result,
                                                              TimeSpan                  Duration);



    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords request will be send.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsRequestHandler(DateTime             Timestamp,
                                                                EMPClient            Sender,
                                                                String               SenderId,
                                                                EVSP_Id              ProviderId,
                                                                DateTime             From,
                                                                DateTime             To,
                                                                TimeSpan?            QueryTimeout);

    /// <summary>
    /// A delegate called whenever a response for a GetChargeDetailRecords request was received.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsResponseHandler(DateTime                         Timestamp,
                                                                 EMPClient                        Sender,
                                                                 String                           SenderId,
                                                                 EVSP_Id                          ProviderId,
                                                                 DateTime                         From,
                                                                 DateTime                         To,
                                                                 TimeSpan?                        QueryTimeout,
                                                                 IEnumerable<ChargeDetailRecord>  Result,
                                                                 TimeSpan                         Duration);

}
