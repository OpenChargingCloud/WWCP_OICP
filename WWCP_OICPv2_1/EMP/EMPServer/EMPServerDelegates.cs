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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// Initiate an AuthorizeStart for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="OperatorId">An EVSE operator identification.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="EVSEId">An optional EVSE identification.</param>
    /// <param name="SessionId">An optional session identification.</param>
    /// <param name="PartnerProductId">An optional partner product identification.</param>
    /// <param name="PartnerSessionId">An optional partner session identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<AuthStartEVSEResult> OnAuthorizeStartDelegate(DateTime            Timestamp,
                                                                       EMPServer           Sender,
                                                                       CancellationToken   CancellationToken,
                                                                       EventTracking_Id    EventTrackingId,
                                                                       EVSEOperator_Id     OperatorId,
                                                                       Auth_Token          AuthToken,
                                                                       EVSE_Id             EVSEId            = null,
                                                                       ChargingSession_Id  SessionId         = null,
                                                                       ChargingProduct_Id  PartnerProductId  = null,
                                                                       ChargingSession_Id  PartnerSessionId  = null,
                                                                       TimeSpan?           QueryTimeout      = null);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification for this charging session on the partner side.</param>
    /// <param name="OperatorId">The unique identification of the EVSE operator.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="AuthToken">A (RFID) user identification.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<AuthStopEVSEResult> OnAuthorizeStopDelegate(DateTime            Timestamp,
                                                                     EMPServer           Sender,
                                                                     CancellationToken   CancellationToken,
                                                                     EventTracking_Id    EventTrackingId,
                                                                     ChargingSession_Id  SessionId,
                                                                     ChargingSession_Id  PartnerSessionId,
                                                                     EVSEOperator_Id     OperatorId,
                                                                     EVSE_Id             EVSEId,
                                                                     Auth_Token          AuthToken,
                                                                     TimeSpan?           QueryTimeout  = null);


    /// <summary>
    /// Send a charge detail record.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="ChargeDetailRecord">A charge detail record.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<SendCDRResult> OnChargeDetailRecordDelegate(DateTime                    Timestamp,
                                                                     CancellationToken           CancellationToken,
                                                                     EventTracking_Id            EventTrackingId,
                                                                     ChargeDetailRecord  ChargeDetailRecord,
                                                                     TimeSpan?                   QueryTimeout  = null);

}
