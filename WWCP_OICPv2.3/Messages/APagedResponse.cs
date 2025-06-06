﻿/*
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
    /// An abstract generic paged response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
    /// <param name="ProcessId">The server side process identification of the request.</param>
    /// <param name="Runtime">The runtime of the request/response.</param>
    /// 
    /// <param name="Request">The request leading to this result. Might be null, when the request e.g. was not parsable!</param>
    /// 
    /// <param name="HTTPResponse">The optional HTTP response.</param>
    /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
    /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers, which will not be serialized.</param>
    public abstract class APagedResponse<TRequest, TResponse>(DateTime                ResponseTimestamp,
                                                              EventTracking_Id        EventTrackingId,
                                                              Process_Id              ProcessId,
                                                              TimeSpan                Runtime,

                                                              TRequest?               Request            = null,
                                                              Boolean?                FirstPage          = null,
                                                              Boolean?                LastPage           = null,
                                                              UInt64?                 Number             = null,
                                                              UInt64?                 NumberOfElements   = null,
                                                              UInt64?                 Size               = null,
                                                              UInt64?                 TotalElements      = null,
                                                              UInt64?                 TotalPages         = null,
                                                              StatusCode?             StatusCode         = null,

                                                              HTTPResponse?           HTTPResponse       = null,
                                                              JObject?                CustomData         = null,
                                                              UserDefinedDictionary?  InternalData       = null) : AResponse<TRequest, TResponse>(
                                                                                                                       ResponseTimestamp,
                                                                                                                       EventTrackingId,
                                                                                                                       ProcessId,
                                                                                                                       Runtime,
                                                                                                                       Request,
                                                                                                                       HTTPResponse,
                                                                                                                       CustomData,
                                                                                                                       InternalData
                                                                                                                   ),
                                                                                                                   IPagedResponse

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The optional status code of this response.
        /// </summary>
        [Optional]
        public StatusCode?  StatusCode          { get; } = StatusCode;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public Boolean?     FirstPage           { get; } = FirstPage;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public Boolean?     LastPage            { get; } = LastPage;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt64?      Number              { get; } = Number;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt64?      NumberOfElements    { get; } = NumberOfElements;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt64?      Size                { get; } = Size;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt64?      TotalElements       { get; } = TotalElements;

        /// <summary>
        /// 
        /// </summary>
        [Optional]
        public UInt64?      TotalPages          { get; } = TotalPages;

        #endregion


        #region ToJSON(CustomGetChargeDetailRecordsResponseSerializer = null, CustomOperatorEVSEStatusSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomIPagedResponseSerializer">A delegate to customize the serialization of paged responses.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        protected JObject ToJSON(CustomJObjectSerializerDelegate<IPagedResponse>?  CustomIPagedResponseSerializer   = null,
                                 CustomJObjectSerializerDelegate<StatusCode>?      CustomStatusCodeSerializer       = null)
        {

            var json = JSONObject.Create(

                           StatusCode is not null
                               ? new JProperty("StatusCode",        StatusCode.ToJSON(CustomStatusCodeSerializer))
                               : null,

                           FirstPage.HasValue
                               ? new JProperty("first",             FirstPage.Value)
                               : null,

                           LastPage.HasValue
                               ? new JProperty("last",              LastPage.Value)
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

            return CustomIPagedResponseSerializer is not null
                       ? CustomIPagedResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region (class) Builder

        /// <summary>
        /// An abstract generic paged response builder.
        /// </summary>
        /// <remarks>
        /// Create a new generic response.
        /// </remarks>
        /// <param name="Request">The request leading to this result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers, which will not be serialized.</param>
        public new abstract class Builder(TRequest?               Request             = null,
                                          DateTime?               ResponseTimestamp   = null,
                                          EventTracking_Id?       EventTrackingId     = null,
                                          TimeSpan?               Runtime             = null,

                                          HTTPResponse?           HTTPResponse        = null,
                                          Process_Id?             ProcessId           = null,
                                          StatusCode?             StatusCode          = null,
                                          Boolean?                FirstPage           = null,
                                          Boolean?                LastPage            = null,
                                          UInt64?                 Number              = null,
                                          UInt64?                 NumberOfElements    = null,
                                          UInt64?                 Size                = null,
                                          UInt64?                 TotalElements       = null,
                                          UInt64?                 TotalPages          = null,

                                          JObject?                CustomData          = null,
                                          UserDefinedDictionary?  InternalData        = null) : AResponse<TRequest, TResponse>.Builder(
                                                                                                    ResponseTimestamp,
                                                                                                    EventTrackingId,
                                                                                                    Runtime,
                                                                                                    Request,
                                                                                                    HTTPResponse,
                                                                                                    ProcessId,
                                                                                                    CustomData,
                                                                                                    InternalData
                                                                                                )
        {

            #region Properties

            /// <summary>
            /// The optional status code of this response.
            /// </summary>
            [Optional]
            public StatusCode.Builder  StatusCode          { get; }      = StatusCode is not null
                                                                               ? StatusCode.ToBuilder()
                                                                               : new StatusCode.Builder();

            /// <summary>
            /// Whether this is the first page of responses.
            /// </summary>
            [Optional]
            public Boolean?            FirstPage           { get; set; } = FirstPage;

            /// <summary>
            /// Whether this is the last page of responses.
            /// </summary>
            [Optional]
            public Boolean?            LastPage            { get; set; } = LastPage;

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt64?             Number              { get; set; } = Number;

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt64?             NumberOfElements    { get; set; } = NumberOfElements;

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt64?             Size                { get; set; } = Size;

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt64?             TotalElements       { get; set; } = TotalElements;

            /// <summary>
            /// 
            /// </summary>
            [Optional]
            public UInt64?             TotalPages          { get; set; } = TotalPages;

            #endregion

        }

        #endregion


    }

}
