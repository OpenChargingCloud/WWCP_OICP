/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
 * This file is part of WWCP OCHP <https://github.com/OpenChargingCloud/WWCP_OCHP>
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
using System.Collections.Generic;

using org.GraphDefined.WWCP.OICPv2_1.CPO;
using org.GraphDefined.WWCP.OICPv2_1.EMP;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    // CPO event delegates...

    #region OnPushEvseData

    /// <summary>
    /// Add charge detail records.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="CDRInfos">An enumeration of charge detail records.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<PushEVSEDataResponse>

        OnPushEvseDataDelegate(DateTime               Timestamp,
                               CentralServer          Sender,
                               CancellationToken      CancellationToken,
                               EventTracking_Id       EventTrackingId,

                               //IEnumerable<CDRInfo>   CDRInfos,

                               TimeSpan?              QueryTimeout = null);

    #endregion



    // EMP event delegates...


}
