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

using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.WebAPI
{

    /// <summary>
    /// OICP+ XML namspaces.
    /// </summary>
    public static class OICPPlusNS
    {

        /// <summary>
        /// The XML namspace for OICP+ common types.
        /// </summary>
        public static XNamespace CommonTypes   = "http://ld.charing.cloud/OICPPlus/CommonTypes/v2.1";

        /// <summary>
        /// The XML namspace for OICP+ EVSE operators. 
        /// </summary>
        public static XNamespace EVSEOperator  = "http://ld.charing.cloud/OICPPlus/EVSEOperator/v2.1";

        /// <summary>
        /// The XML namspace for OICP+ EVSE data.
        /// </summary>
        public static XNamespace EVSEData      = "http://ld.charing.cloud/OICPPlus/EVSEData/v2.1";

        /// <summary>
        /// The XML namspace for OICP+ EVSE status.
        /// </summary>
        public static XNamespace EVSEStatus    = "http://ld.charing.cloud/OICPPlus/EVSEStatus/v2.1";

    }

}
