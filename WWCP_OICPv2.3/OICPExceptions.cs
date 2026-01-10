/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// OICP exceptions.
    /// </summary>
    public class OICPException : ApplicationException
    {

        #region Properties

        public StatusCode?  StatusCode    { get; }

        #endregion

        #region Constructor(s)

        #region OICPException(Message)

        public OICPException(String Message)
            : base(Message)
        { }

        #endregion

        #region OICPException(Message, InnerException)

        public OICPException(String     Message,
                             Exception  InnerException)
            : base(Message,
                   InnerException)
        { }

        #endregion

        #region OICPException(StatusCode, Message = null)

        /// <summary>
        /// Create a new OICP exception for the given status code.
        /// </summary>
        /// <param name="StatusCode"></param>
        public OICPException(StatusCode  StatusCode,
                             String?     Message = null)
            : base(Message)
        {
            this.StatusCode  = StatusCode;
        }

        #endregion

        #endregion

    }



    public class InvalidEVSEIdentificationException : OICPException
    {

        #region Properties

        public String  EVSEId    { get; }

        #endregion

        #region Constructor(s)

        public InvalidEVSEIdentificationException(String EVSEId)
            : base("Invalid EVSE identification '" + EVSEId + "'!")
        {
            this.EVSEId = EVSEId;
        }

        #endregion

    }

}
