/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// Initiate a remote start of the given charging session at the given EVSE
    /// and for the given Provider/eMAId.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="ChargingProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification for this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="eMAId">The unique identification of the e-mobility account.</param>
    /// <returns>A RemoteStartResult task.</returns>
    public delegate Task<RemoteStartEVSEResult> OnRemoteStartDelegate(DateTime            Timestamp,
                                                                  CPOServer           Sender,
                                                                  CancellationToken   CancellationToken,
                                                                  EVSE_Id             EVSEId,
                                                                  ChargingProduct_Id  ChargingProductId,
                                                                  ChargingSession_Id  SessionId,
                                                                  ChargingSession_Id  PartnerSessionId,
                                                                  EVSP_Id             ProviderId,
                                                                  eMA_Id              eMAId);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification for this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification for this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    /// <returns>A RemoteStopResult task.</returns>
    public delegate Task<RemoteStopEVSEResult> OnRemoteStopDelegate(DateTime             Timestamp,
                                                                CPOServer            Sender,
                                                                CancellationToken    CancellationToken,
                                                                EVSE_Id              EVSEId,
                                                                ChargingSession_Id   SessionId,
                                                                ChargingSession_Id   PartnerSessionId,
                                                                EVSP_Id              ProviderId);


}


