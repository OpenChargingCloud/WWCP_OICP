/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Collections.Generic;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public delegate T CustomParserDelegate<T>(XElement XML, T ResponseBuilder);

    public delegate XElement CustomSerializerDelegate<T>(T ResponseBuilder, XElement XML);

    public delegate TB CustomMapper2Delegate<TB>(TB ResponseBuilder);

    public delegate TB CustomMapperDelegate<T, TB>(XElement XML, TB ResponseBuilder);


    /// <summary>
    /// The common interface of an OICP response message.
    /// </summary>
    public interface IResponse
    {

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
   //     Result    Result              { get; }

        /// <summary>
        /// The timestamp of the response message creation.
        /// </summary>
        DateTime  ResponseTimestamp   { get; }

    }

}
