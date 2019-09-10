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

using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OICP XML Namespaces
    /// </summary>
    public static class OICPNS
    {

        /// <summary>
        /// The namespace for the common types within the Open Intercharge Protocol (OICP) Version 2.0.
        /// </summary>
        public static readonly XNamespace CommonTypes          = "http://www.hubject.com/b2b/services/commontypes/v2.0";

#if OICPv2_1
        /// <summary>
        /// The namespace for the EVSE Data within the Open Intercharge Protocol (OICP) Version 2.1.
        /// </summary>
        public static readonly XNamespace EVSEData             = "http://www.hubject.com/b2b/services/evsedata/v2.1";
#endif

#if OICPv2_2
        /// <summary>
        /// The namespace for the EVSE Data within the Open Intercharge Protocol (OICP) Version 2.2.
        /// </summary>
        public static readonly XNamespace EVSEData             = "http://www.hubject.com/b2b/services/evsedata/v2.2";
#endif

        /// <summary>
        /// The namespace for the EVSE Status within the Open Intercharge Protocol (OICP) Version 2.0.
        /// </summary>
        public static readonly XNamespace EVSEStatus           = "http://www.hubject.com/b2b/services/evsestatus/v2.0";

        /// <summary>
        /// The namespace for the Authorization within the Open Intercharge Protocol (OICP) Version 2.0.
        /// </summary>
        public static readonly XNamespace Authorization        = "http://www.hubject.com/b2b/services/authorization/v2.0";

#if OICPv2_1
        /// <summary>
        /// The namespace for the Reservation within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace Reservation          = "http://www.hubject.com/b2b/services/reservation/v1.0";
#endif

#if OICPv2_2
        /// <summary>
        /// The namespace for the Reservation within the Open Intercharge Protocol (OICP) Version 1.1.
        /// </summary>
        public static readonly XNamespace Reservation          = "http://www.hubject.com/b2b/services/reservation/v1.1";
#endif


#if OICPv2_1
        /// <summary>
        /// The namespace for the Authentication Data within the Open Intercharge Protocol (OICP) Version 2.0.
        /// </summary>
        public static readonly XNamespace AuthenticationData   = "http://www.hubject.com/b2b/services/authenticationdata/v2.0";
#endif

#if OICPv2_2
        /// <summary>
        /// The namespace for the Authentication Data within the Open Intercharge Protocol (OICP) Version 2.2.
        /// </summary>
        public static readonly XNamespace AuthenticationData   = "http://www.hubject.com/b2b/services/authenticationdata/v2.2";
#endif

        /// <summary>
        /// The namespace for the Mobile Authorization within the Open Intercharge Protocol (OICP) Version 2.0.
        /// </summary>
        public static readonly XNamespace MobileAuthorization  = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0";

#if OICPv2_2
        ///// <summary>
        ///// The namespace for B2B2C feedback within the Open Intercharge Protocol (OICP) Version 2.2.
        ///// </summary>
        public static readonly XNamespace B2B2CFeedback           = "http://www.hubject.com/b2b/services/b2b2cfeedback/v1.0";
#endif

    }

}
