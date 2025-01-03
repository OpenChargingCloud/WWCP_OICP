/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// An abstract generic OICP request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the OICP request.</typeparam>
    /// <param name="ProcessId">The optional unique OICP process identification.</param>
    /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
    /// <param name="Timestamp">The optional timestamp of the request.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="RequestTimeout">The timeout for this request.</param>
    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    public abstract class ARequest<TRequest>(Process_Id?        ProcessId           = null,
                                             JObject?           CustomData          = null,
                                             DateTime?          Timestamp           = null,
                                             EventTracking_Id?  EventTrackingId     = null,
                                             TimeSpan?          RequestTimeout      = null,
                                             CancellationToken  CancellationToken   = default) : IRequest,
                                                                                                 IEquatable<TRequest>

        where TRequest : class, IRequest

    {

        #region Data

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(60);

        #endregion

        #region Properties

        /// <summary>
        /// The unique OICP process identification.
        /// </summary>
        public Process_Id?        ProcessId            { get; }      = ProcessId;

        /// <summary>
        /// The optional timestamp of the request.
        /// </summary>
        public DateTime           Timestamp            { get; }      = Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        public EventTracking_Id   EventTrackingId      { get; }      = EventTrackingId ?? EventTracking_Id.New;

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        public TimeSpan?          RequestTimeout       { get; }      = RequestTimeout ?? DefaultRequestTimeout;

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?           CustomData           { get; set; } = CustomData;

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken  CancellationToken    { get; }      = CancellationToken;

        #endregion


        #region IEquatable<ARequest> Members

        /// <summary>
        /// Compare two abstract generic OICP requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract generic OICP request.</param>
        public abstract Boolean Equals(TRequest? ARequest);

        #endregion


    }

}
