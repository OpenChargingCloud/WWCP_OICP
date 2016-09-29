﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OICP v2.0 exceptions.
    /// </summary>
    public class OICPException : ApplicationException
    {

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// Create a new OICP v2.0 exception for the given status code.
        /// </summary>
        /// <param name="StatusCode"></param>
        public OICPException(StatusCode  StatusCode)
        {
            this._StatusCode  = StatusCode;
        }

    }

}
