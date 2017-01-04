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
using System.Xml.Linq;

using SOAPNS = org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OICP SOAP helpers.
    /// </summary>
    public static class SOAP
    {

        /// <summary>
        /// Encapsulate the given XML within a XML SOAP frame.
        /// </summary>
        /// <param name="SOAPBody">The internal XML for the SOAP body.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        public static XElement Encapsulation(XElement                      SOAPBody,
                                             SOAPNS.XMLNamespacesDelegate  XMLNamespaces = null)
        {

            #region Initial checks

            if (SOAPBody == null)
                throw new ArgumentNullException(nameof(SOAPBody), "The given XML must not be null!");

            if (XMLNamespaces == null)
                XMLNamespaces = xml => xml;

            #endregion

            return XMLNamespaces(
                new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "SOAP",                SOAPNS.NS.SOAPEnvelope_v1_1.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "CommonTypes",         OICPNS.CommonTypes.         NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "EVSEData",            OICPNS.EVSEData.            NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "EVSEStatus",          OICPNS.EVSEStatus.          NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "MobileAuthorization", OICPNS.MobileAuthorization. NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "Authorization",       OICPNS.Authorization.       NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "AuthenticationData",  OICPNS.AuthenticationData.  NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "EVSESearch",          OICPNS.EVSESearch.          NamespaceName),

                    new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Header"),
                    new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Body",  SOAPBody)
                )
            );

        }

    }

}
