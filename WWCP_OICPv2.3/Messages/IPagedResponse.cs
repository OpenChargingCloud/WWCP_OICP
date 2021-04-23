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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The interface of paged responses.
    /// </summary>
    public interface IPagedResponse
    {

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

    }

}
