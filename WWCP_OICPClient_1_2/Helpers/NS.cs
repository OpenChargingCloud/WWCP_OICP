/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// (OICP) XML Namespaces
    /// </summary>
    public static class NS
    {

        /// <summary>
        /// The namespace for the common types within the Open Intercharge Protocol (OICP) Version 1.2.
        /// </summary>
        public static readonly XNamespace OICPv1_2CommonTypes         = "http://www.hubject.com/b2b/services/commontypes/v1.2";

        /// <summary>
        /// The namespace for the EVSE Data within the Open Intercharge Protocol (OICP) Version 1.2.
        /// </summary>
        public static readonly XNamespace OICPv1_2EVSEData            = "http://www.hubject.com/b2b/services/evsedata/v1.2";

        /// <summary>
        /// The namespace for the EVSE Status within the Open Intercharge Protocol (OICP) Version 1.2.
        /// </summary>
        public static readonly XNamespace OICPv1_2EVSEStatus          = "http://www.hubject.com/b2b/services/evsestatus/v1.2";

        /// <summary>
        /// The namespace for the Authorization within the Open Intercharge Protocol (OICP) Version 1.2.
        /// </summary>
        public static readonly XNamespace OICPv1_2Authorization       = "http://www.hubject.com/b2b/services/authorization/v1.2";

        /// <summary>
        /// The namespace for the Mobile Authorization within the Open Intercharge Protocol (OICP) Version 1.2.
        /// </summary>
        public static readonly XNamespace OICPv1_2MobileAuthorization = "http://www.hubject.com/b2b/services/mobileauthorization/v1.2";

        /// <summary>
        /// The namespace for EVSE Serach within the Open Intercharge Protocol (OICP) Version 1.2.
        /// </summary>
        public static readonly XNamespace OICPv1_2EVSESearch          = "http://www.hubject.com/b2b/services/evsesearch/v1.2";

    }

}
