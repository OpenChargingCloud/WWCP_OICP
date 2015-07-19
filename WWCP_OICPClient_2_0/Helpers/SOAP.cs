﻿/*
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

using SOAPNS = org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_2_0
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

            return new XElement(SOAPNS.NS.SOAPEnvelope + "Envelope",
                       new XAttribute(XNamespace.Xmlns + "eMI3",                SOAPNS.NS.SOAPEnvelope.        NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "CommonTypes",         NS.OICPv2_0CommonTypes.        NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSEData",            NS.OICPv2_0EVSEData.           NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSEStatus",          NS.OICPv2_0EVSEStatus.         NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "MobileAuthorization", NS.OICPv2_0MobileAuthorization.NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "Authorization",       NS.OICPv2_0Authorization.      NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSESearch",          NS.OICPv2_0EVSESearch.         NamespaceName),

                       new XElement(SOAPNS.NS.SOAPEnvelope + "Header"),
                       new XElement(SOAPNS.NS.SOAPEnvelope + "Body", XML));

        }

        #endregion

    }

}
