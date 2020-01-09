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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// The common interface of an OICP request message.
    /// </summary>
    public interface IRequest
    {

        /// <summary>
        /// The optional timestamp of the request.
        /// </summary>
        DateTime?           Timestamp           { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        CancellationToken?  CancellationToken   { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        EventTracking_Id    EventTrackingId     { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        TimeSpan?           RequestTimeout      { get; }

    }

}
