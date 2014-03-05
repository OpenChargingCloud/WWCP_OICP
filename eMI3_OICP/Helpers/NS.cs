/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

namespace org.emi3group.IO.OICP
{

    /// <summary>
    /// (OICP) XML Namespaces
    /// </summary>
    public static class NS
    {

        /// <summary>
        /// The namespace for the XML SOAP Envelope.
        /// </summary>
        public static readonly XNamespace SOAPEnvelope          = "http://schemas.xmlsoap.org/soap/envelope/";

        /// <summary>
        /// The namespace for the common types within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace OICPv1CommonTypes     = "http://www.hubject.com/b2b/services/commontypes/v1";

        /// <summary>
        /// The namespace for the EVSE Data within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace OICPv1EVSEData        = "http://www.hubject.com/b2b/services/evsedata/v1";

        /// <summary>
        /// The namespace for the EVSE Status within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace OICPv1EVSEStatus      = "http://www.hubject.com/b2b/services/evsestatus/v1";

        /// <summary>
        /// The namespace for the Authorization within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace OICPv1Authorization   = "http://www.hubject.com/b2b/services/authorization/v1";

        /// <summary>
        /// The namespace for EVSE Serach within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace OICPv1EVSESearch      = "http://www.hubject.com/b2b/services/evsesearch/v1";

    }

}
