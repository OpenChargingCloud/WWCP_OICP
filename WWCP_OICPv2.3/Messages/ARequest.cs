/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
    public abstract class ARequest<TRequest> : IRequest,
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
        /// The optional timestamp of the request.
        /// </summary>
        public DateTime                  Timestamp                  { get; }

        /// <summary>
        /// An optional token source to cancel this request.
        /// </summary>
        public CancellationTokenSource?  CancellationTokenSource    { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken?        CancellationToken          { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        public EventTracking_Id?         EventTrackingId            { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        public TimeSpan?                 RequestTimeout             { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                  CustomData                 { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic request message.
        /// </summary>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public ARequest(JObject?            CustomData          = null,
                        DateTime?           Timestamp           = null,
                        CancellationToken?  CancellationToken   = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null)
        {

            this.CustomData               = CustomData;
            this.Timestamp                = Timestamp         ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.CancellationTokenSource  = CancellationToken is null
                                                ? new CancellationTokenSource()
                                                : null;
            this.CancellationToken        = CancellationToken ?? CancellationTokenSource!.Token;
            this.EventTrackingId          = EventTrackingId   ?? EventTracking_Id.New;
            this.RequestTimeout           = RequestTimeout    ?? DefaultRequestTimeout;

        }

        #endregion


        #region IEquatable<ARequest> Members

        /// <summary>
        /// Compare two abstract requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract OICP request.</param>
        public abstract Boolean Equals(TRequest? ARequest);

        #endregion


    }

}
