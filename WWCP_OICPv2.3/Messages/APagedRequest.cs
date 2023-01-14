/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An abstract generic request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the OICP request.</typeparam>
    public abstract class APagedRequest<TRequest> : ARequest<TRequest>

        where TRequest : class, IRequest

    {

        #region Properties

        /// <summary>
        /// The optional page number of the request page.
        /// </summary>
        public UInt32?               Page         { get; }

        /// <summary>
        /// The optional size of a request page.
        /// </summary>
        public UInt32?               Size         { get; }

        /// <summary>
        /// Optional sorting criteria in the format: property(,asc|desc).
        /// </summary>
        public IEnumerable<String>?  SortOrder    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic request message.
        /// </summary>
        /// <param name="ProcessId">The optional unique OICP process identification.</param>
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public APagedRequest(Process_Id?           ProcessId           = null,
                             UInt32?               Page                = null,
                             UInt32?               Size                = null,
                             IEnumerable<String>?  SortOrder           = null,
                             JObject?              CustomData          = null,

                             DateTime?             Timestamp           = null,
                             CancellationToken?    CancellationToken   = null,
                             EventTracking_Id?     EventTrackingId     = null,
                             TimeSpan?             RequestTimeout      = null)

            : base(ProcessId,
                   CustomData,
                   Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.Page       = Page;
            this.Size       = Size;
            this.SortOrder  = SortOrder;

        }

        #endregion

    }

}
