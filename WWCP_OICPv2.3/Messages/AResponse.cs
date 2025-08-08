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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An abstract generic OICP response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
    /// <param name="ProcessId">The server side process identification of the request.</param>
    /// <param name="Runtime">The runtime of the request/response.</param>
    /// 
    /// <param name="Request">The request leading to this result. Might be null, when the request e.g. was not parsable!</param>
    /// <param name="HTTPResponse">The optional HTTP response.</param>
    /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
    /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers, which will not be serialized.</param>
    public abstract class AResponse<TRequest, TResponse>(DateTimeOffset          ResponseTimestamp,
                                                         EventTracking_Id        EventTrackingId,
                                                         Process_Id              ProcessId,
                                                         TimeSpan                Runtime,

                                                         TRequest?               Request        = null,
                                                         HTTPResponse?           HTTPResponse   = null,
                                                         JObject?                CustomData     = null,
                                                         UserDefinedDictionary?  InternalData   = null) : IResponse,
                                                                                                          IEquatable<TResponse>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The timestamp of the response creation.
        /// </summary>
        [Mandatory]
        public DateTimeOffset          ResponseTimestamp    { get; }      = ResponseTimestamp;

        /// <summary>
        /// An optional event tracking identification for correlating this response with other events.
        /// </summary>
        public EventTracking_Id        EventTrackingId      { get; }      = EventTrackingId;

        /// <summary>
        /// The runtime of the request/response.
        /// </summary>
        public TimeSpan                Runtime              { get; }      = Runtime;

        /// <summary>
        /// The request leading to this response.
        /// Might be null, when the request was not parsable!
        /// </summary>
        [Optional]
        public TRequest?               Request              { get; }      = Request;

        /// <summary>
        /// The HTTP response.
        /// </summary>
        [Optional]
        public HTTPResponse?           HTTPResponse         { get; }      = HTTPResponse;

        /// <summary>
        /// The server side process identification of the request.
        /// </summary>
        [Mandatory]
        public Process_Id              ProcessId            { get; }      = ProcessId;

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                CustomData           { get; set; } = CustomData;

        /// <summary>
        /// Optional internal customer specific data, which will not be serialized.
        /// </summary>
        public UserDefinedDictionary?  InternalData         { get; set; } = InternalData;

        #endregion


        #region IEquatable<AResponse> Members

        /// <summary>
        /// Compare two abstract responses for equality.
        /// </summary>
        /// <param name="AResponse">Another abstract response.</param>
        public abstract Boolean Equals(TResponse? AResponse);

        #endregion

        #region ToJSON()

        ///// <summary>
        ///// Compare two abstract responses for equality.
        ///// </summary>
        //public abstract JObject ToJSON();

        #endregion


        #region (class) Builder

        /// <summary>
        /// An abstract generic response builder.
        /// </summary>
        public abstract class Builder
        {

            #region Properties

            /// <summary>
            /// The timestamp of the response message creation.
            /// </summary>
            [Mandatory]
            public DateTimeOffset?         ResponseTimestamp    { get; set; }

            /// <summary>
            /// An optional event tracking identification for correlating this response with other events.
            /// </summary>
            public EventTracking_Id?       EventTrackingId      { get; set; }

            /// <summary>
            /// The runtime of the request/response.
            /// </summary>
            public TimeSpan?               Runtime              { get; set; }

            /// <summary>
            /// The optional Hubject process identification of the request.
            /// </summary>
            [Optional]
            public Process_Id?             ProcessId            { get; set; }

            /// <summary>
            /// The request leading to this response.
            /// </summary>
            [Mandatory]
            public TRequest?               Request              { get; set; }

            /// <summary>
            /// The HTTP response.
            /// </summary>
            [Optional]
            public HTTPResponse?           HTTPResponse         { get; set; }

            /// <summary>
            /// Optional custom data, e.g. in combination with custom parsers and serializers.
            /// </summary>
            [Optional]
            public JObject?                CustomData           { get; set; }

            /// <summary>
            /// Optional internal customer specific data, which will not be serialized.
            /// </summary>
            [Optional]
            public UserDefinedDictionary?  InternalData         { get; set; }

            #endregion

            #region Constructor(s)

#pragma warning disable IDE0290 // Use primary constructor

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
            /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers, which will not be serialized.</param>
            protected Builder(DateTimeOffset?         ResponseTimestamp   = null,
                              EventTracking_Id?       EventTrackingId     = null,
                              TimeSpan?               Runtime             = null,
                              TRequest?               Request             = null,
                              HTTPResponse?           HTTPResponse        = null,
                              Process_Id?             ProcessId           = null,
                              JObject?                CustomData          = null,
                              UserDefinedDictionary?  InternalData        = null)
            {

                this.ResponseTimestamp  = ResponseTimestamp;
                this.EventTrackingId    = EventTrackingId;
                this.Runtime            = Runtime;
                this.Request            = Request;
                this.HTTPResponse       = HTTPResponse;
                this.ProcessId          = ProcessId;
                this.CustomData         = CustomData;
                this.InternalData       = InternalData;

            }

#pragma warning restore IDE0290 // Use primary constructor

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable response.
            /// </summary>
            public abstract TResponse  ToImmutable();

            #endregion

        }

        #endregion

    }

}
