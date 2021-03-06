﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An abstract generic response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public abstract class AResponse<TRequest, TResponse> : IResponse,
                                                           IEquatable<TResponse>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The request leading to this response.
        /// </summary>
        [Mandatory]
        public TRequest          Request              { get; }

        /// <summary>
        /// The timestamp of the response creation.
        /// </summary>
        [Mandatory]
        public DateTime          ResponseTimestamp    { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this response with other events.
        /// </summary>
        public EventTracking_Id  EventTrackingId      { get; }

        /// <summary>
        /// The runtime of the request/response.
        /// </summary>
        public TimeSpan          Runtime              { get; }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        [Optional]
        public HTTPResponse      HTTPResponse         { get; }

        /// <summary>
        /// The optional Hubject process identification of the request.
        /// </summary>
        [Optional]
        public Process_Id?       ProcessId            { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject           CustomData           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic response.
        /// </summary>
        /// <param name="Request">The request leading to this result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        protected AResponse(TRequest          Request,
                            DateTime          ResponseTimestamp,
                            EventTracking_Id  EventTrackingId,
                            TimeSpan          Runtime,
                            HTTPResponse      HTTPResponse   = null,
                            Process_Id?       ProcessId      = null,
                            JObject           CustomData     = null)
        {

            this.Request            = Request ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!");
            this.ResponseTimestamp  = ResponseTimestamp;
            this.EventTrackingId    = EventTrackingId;
            this.Runtime            = Runtime;
            this.HTTPResponse       = HTTPResponse;
            this.ProcessId          = ProcessId;
            this.CustomData         = CustomData;

        }

        #endregion


        #region IEquatable<AResponse> Members

        /// <summary>
        /// Compare two abstract responses for equality.
        /// </summary>
        /// <param name="AResponse">Another abstract response.</param>
        public abstract Boolean Equals(TResponse AResponse);

        #endregion


        #region (class) Builder

        /// <summary>
        /// An abstract generic response builder.
        /// </summary>
        public abstract class Builder
        {

            #region Properties

            /// <summary>
            /// The request leading to this response.
            /// </summary>
            [Mandatory]
            public TRequest          Request              { get; set; }

            /// <summary>
            /// The timestamp of the response message creation.
            /// </summary>
            [Mandatory]
            public DateTime?         ResponseTimestamp    { get; set; }

            /// <summary>
            /// An optional event tracking identification for correlating this response with other events.
            /// </summary>
            public EventTracking_Id  EventTrackingId      { get; set; }

            /// <summary>
            /// The runtime of the request/response.
            /// </summary>
            public TimeSpan?         Runtime              { get; set; }

            /// <summary>
            /// The optional Hubject process identification of the request.
            /// </summary>
            [Optional]
            public Process_Id?       ProcessId            { get; set; }

            /// <summary>
            /// The HTTP response.
            /// </summary>
            [Optional]
            public HTTPResponse      HTTPResponse         { get; set; }

            /// <summary>
            /// Optional custom data, e.g. in combination with custom parsers and serializers.
            /// </summary>
            [Optional]
            public JObject           CustomData           { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new generic response.
            /// </summary>
            /// <param name="Request">The request leading to this result.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="CustomData">Optional customer-specific data of the response.</param>
            protected Builder(TRequest          Request             = null,
                              DateTime?         ResponseTimestamp   = null,
                              EventTracking_Id  EventTrackingId     = null,
                              TimeSpan?         Runtime             = null,
                              HTTPResponse      HTTPResponse        = null,
                              Process_Id?       ProcessId           = null,
                              JObject           CustomData          = null)
            {

                this.Request            = Request;
                this.ResponseTimestamp  = ResponseTimestamp;
                this.EventTrackingId    = EventTrackingId;
                this.Runtime            = Runtime;
                this.HTTPResponse       = HTTPResponse;
                this.ProcessId          = ProcessId;
                this.CustomData         = CustomData;

            }

            #endregion


            /// <summary>
            /// Return an immutable response.
            /// </summary>
            public abstract TResponse  ToImmutable();

        }

        #endregion

    }

}
