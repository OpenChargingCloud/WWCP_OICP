/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
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
    /// SOAP helpers.
    /// </summary>
    public static class SOAP
    {

        #region Encapsulation(XML)

        /// <summary>
        /// Encapsulate the given XML within a XML SOAP frame.
        /// </summary>
        /// <param name="XML">The internal XML.</param>
        public static XElement Encapsulation(XElement XML)
        {

            return new XElement(NS.SOAPEnvelope + "Envelope",
                       new XAttribute(XNamespace.Xmlns + "eMI3",                NS.SOAPEnvelope.             NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "CommonTypes",         NS.OICPv1CommonTypes.        NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSEData",            NS.OICPv1EVSEData.           NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSEStatus",          NS.OICPv1EVSEStatus.         NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "MobileAuthorization", NS.OICPv1MobileAuthorization.NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "Authorization",       NS.OICPv1Authorization.      NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSESearch",          NS.OICPv1EVSESearch.         NamespaceName),

                       new XElement(NS.SOAPEnvelope + "Header"),
                       new XElement(NS.SOAPEnvelope + "Body", XML));

        }

        #endregion

    }

}
