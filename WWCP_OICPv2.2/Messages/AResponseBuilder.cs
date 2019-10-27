/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// An abstract generic OICP response builder.
    /// </summary>
    /// <typeparam name="TRequest">The type of the OICP request.</typeparam>
    /// <typeparam name="TResponse">The type of the OICP response.</typeparam>
    public abstract class AResponseBuilder<TRequest, TResponse> : ACustomData.Builder,
                                                                  IResponse,
                                                                  IEquatable<TResponse>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The request leading to this response.
        /// </summary>
        public TRequest Request             { get; }

        /// <summary>
        /// The timestamp of the response message creation.
        /// </summary>
        public DateTime ResponseTimestamp   { get; set; }

        #endregion

        #region Constructor(s)

        #region AResponse(Request, CustomData = null)

        /// <summary>
        /// Create a new generic OICP response.
        /// </summary>
        /// <param name="Request">The OICP request leading to this result.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                    Request,
                                   Dictionary<String, Object>  CustomData  = null)

            : this(Request,
                   DateTime.Now,
                   CustomData)

        { }

        #endregion

        #region AResponse(Request, ResponseTimestamp = null, CustomData = null)

        /// <summary>
        /// Create a new generic OICP response.
        /// </summary>
        /// <param name="Request">The OICP request leading to this result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                    Request,
                                   DateTime?                   ResponseTimestamp  = null,
                                   Dictionary<String, Object>  CustomData         = null)

            : base(CustomData)

        {

            this.Request            = Request;
            this.ResponseTimestamp  = ResponseTimestamp ?? DateTime.UtcNow;

        }

        #endregion

        #region AResponse(Request, CustomData = null)

        /// <summary>
        /// Create a new generic OICP response.
        /// </summary>
        /// <param name="Request">The OICP request leading to this result.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                                   Request,
                                   IEnumerable<KeyValuePair<String, Object>>  CustomData  = null)

            : this(Request,
                   DateTime.UtcNow,
                   CustomData)

        { }

        #endregion

        #region AResponse(Request, ResponseTimestamp = null, CustomData = null)

        /// <summary>
        /// Create a new generic OICP response.
        /// </summary>
        /// <param name="Request">The OICP request leading to this result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="CustomData">Optional customer-specific data of the response.</param>
        protected AResponseBuilder(TRequest                                   Request,
                                   DateTime?                                  ResponseTimestamp  = null,
                                   IEnumerable<KeyValuePair<String, Object>>  CustomData         = null)

            : base(CustomData)

        {

            this.Request            = Request;
            this.ResponseTimestamp  = ResponseTimestamp ?? DateTime.UtcNow;

        }

        #endregion

        #endregion


        #region IEquatable<AResponse> Members

        /// <summary>
        /// Compare two abstract responses for equality.
        /// </summary>
        /// <param name="AResponse">Another abstract OICP response.</param>
        public abstract Boolean Equals(TResponse AResponse);

        #endregion

        #region (abstract) ToImmutable

        /// <summary>
        /// Return an immutable response.
        /// </summary>
        public abstract TResponse ToImmutable { get; }

        #endregion

    }


}
