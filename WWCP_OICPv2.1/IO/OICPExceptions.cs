/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OICP exceptions.
    /// </summary>
    public class OICPException : ApplicationException
    {

        public StatusCode StatusCode { get; }

        public OICPException(String Message)
            : base(Message)
        { }

        public OICPException(String     Message,
                             Exception  InnerException)
            : base(Message,
                   InnerException)
        { }

        /// <summary>
        /// Create a new OICP exception for the given status code.
        /// </summary>
        /// <param name="StatusCode"></param>
        public OICPException(StatusCode  StatusCode,
                             String      Message = null)
            : base(Message)
        {
            this.StatusCode  = StatusCode;
        }

    }



    public class InvalidEVSEIdentificationException : OICPException
    {

        public String EVSEId { get; }

        public InvalidEVSEIdentificationException(String EVSEId)
            : base("Invalid EVSE identification '" + EVSEId + "'!")
        {
            this.EVSEId = EVSEId;
        }

    }

}
