/*
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
    /// An abstract generic paged response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public abstract class APagedResponse<TRequest, TResponse> : AResponse<TRequest, TResponse>,
                                                                IPagedResponse

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode  StatusCode          { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public Boolean?    First               { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public Boolean?    Last                { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt32?     Number              { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt32?     NumberOfElements    { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt32?     Size                { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt32?     TotalElements       { get; }

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt32?     TotalPages          { get; }

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
        /// 
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected APagedResponse(TRequest          Request,
                                 DateTime          ResponseTimestamp,
                                 EventTracking_Id  EventTrackingId,
                                 TimeSpan          Runtime,

                                 HTTPResponse      HTTPResponse       = null,
                                 Process_Id?       ProcessId          = null,
                                 StatusCode        StatusCode         = null,
                                 Boolean?          First              = null,
                                 Boolean?          Last               = null,
                                 UInt32?           Number             = null,
                                 UInt32?           NumberOfElements   = null,
                                 UInt32?           Size               = null,
                                 UInt32?           TotalElements      = null,
                                 UInt32?           TotalPages         = null,

                                 JObject           CustomData         = null)

            : base(Request,
                   ResponseTimestamp,
                   EventTrackingId,
                   Runtime,
                   HTTPResponse,
                   ProcessId,
                   CustomData)

        {

            this.StatusCode        = StatusCode;
            this.First             = First;
            this.Last              = Last;
            this.Number            = Number;
            this.NumberOfElements  = NumberOfElements;
            this.Size              = Size;
            this.TotalElements     = TotalElements;
            this.TotalPages        = TotalPages;

        }

        #endregion


        #region ToJSON(CustomGetChargeDetailRecordsResponseSerializer = null, CustomOperatorEVSEStatusSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomIPagedResponseSerializer">A delegate to customize the serialization of paged responses.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        protected JObject ToJSON(CustomJObjectSerializerDelegate<IPagedResponse>  CustomIPagedResponseSerializer   = null,
                                 CustomJObjectSerializerDelegate<StatusCode>      CustomStatusCodeSerializer       = null)
        {

            var JSON = JSONObject.Create(

                           StatusCode != null
                               ? new JProperty("StatusCode",        StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           First.HasValue
                               ? new JProperty("first",             First.Value)
                               : null,

                           Last.HasValue
                               ? new JProperty("last",              Last.Value)
                               : null,

                           Number.HasValue
                               ? new JProperty("number",            Number.Value)
                               : null,

                           NumberOfElements.HasValue
                               ? new JProperty("numberOfElements",  NumberOfElements.Value)
                               : null,

                           Size.HasValue
                               ? new JProperty("size",              Size.Value)
                               : null,

                           TotalElements.HasValue
                               ? new JProperty("totalElements",     TotalElements.Value)
                               : null,

                           TotalPages.HasValue
                               ? new JProperty("totalPages",        TotalPages.Value)
                               : null

                       );

            return CustomIPagedResponseSerializer != null
                       ? CustomIPagedResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region (class) Builder

        /// <summary>
        /// An abstract generic paged response builder.
        /// </summary>
        public new abstract class Builder : AResponse<TRequest, TResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// The optional status code of this response.
            /// </summary>
            [Optional]
            public StatusCode  StatusCode          { get; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public Boolean?    First               { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public Boolean?    Last                { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt32?     Number              { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt32?     NumberOfElements    { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt32?     Size                { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt32?     TotalElements       { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt32?     TotalPages          { get; set; }

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
            /// 
            /// <param name="CustomData">Optional customer-specific data of the response.</param>
            protected Builder(TRequest          Request             = null,
                              DateTime?         ResponseTimestamp   = null,
                              EventTracking_Id  EventTrackingId     = null,
                              TimeSpan?         Runtime             = null,

                              HTTPResponse      HTTPResponse        = null,
                              Process_Id?       ProcessId           = null,
                              StatusCode        StatusCode          = null,
                              Boolean?          First               = null,
                              Boolean?          Last                = null,
                              UInt32?           Number              = null,
                              UInt32?           NumberOfElements    = null,
                              UInt32?           Size                = null,
                              UInt32?           TotalElements       = null,
                              UInt32?           TotalPages          = null,

                              JObject           CustomData          = null)

            : base(Request,
                   ResponseTimestamp,
                   EventTrackingId,
                   Runtime,
                   HTTPResponse,
                   ProcessId,
                   CustomData)

            {

                this.StatusCode        = StatusCode != null ? StatusCode.ToBuilder() : new StatusCode.Builder();
                this.First             = First;
                this.Last              = Last;
                this.Number            = Number;
                this.NumberOfElements  = NumberOfElements;
                this.Size              = Size;
                this.TotalElements     = TotalElements;
                this.TotalPages        = TotalPages;

            }

            #endregion

        }

        #endregion

    }

}
