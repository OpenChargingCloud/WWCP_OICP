/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0.Server
{

    /// <summary>
    /// OICP v2.0 XML I/O.
    /// </summary>
    public static class OICPv2_0_XML_IO
    {

        public static XNamespace OICPPlusEVSEOperator = "http://ld.graphdefined.org/e-Mobility/OICPPlus/EVSEOperator/v2.0";


        #region ToXML(this EVSEDataRecords, XMLNamespaces = null, EVSEDataRecord2XML = null, XMLPostProcessing = null)

        /// <summary>
        /// Convert the given enumeration of EVSE data records to XML.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSEDataRecord2XML">An optional delegate to process an EVSE data record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public static XElement ToXML(this IEnumerable<EVSEDataRecord>  EVSEDataRecords,
                                     XMLNamespacesDelegate             XMLNamespaces       = null,
                                     EVSEDataRecord2XMLDelegate        EVSEDataRecord2XML  = null,
                                     XMLPostProcessingDelegate         XMLPostProcessing   = null)
        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException("EVSEDataRecords", "The given enumeration of EVSE data records must not be null!");

            if (EVSEDataRecord2XML == null)
                EVSEDataRecord2XML = (evsedatarecord, xml) => xml;

            if (XMLPostProcessing == null)
                XMLPostProcessing = xml => xml;

            #endregion

            return XMLPostProcessing(
                       SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingEvseData",
                                              new XElement(OICPNS.EVSEData + "EvseData",

                                                  EVSEDataRecords.ToLookup(evsedatarecord => evsedatarecord.EVSEOperator).
                                                    Select(group =>

                                                      group.Where(evsedatarecord => evsedatarecord != null).Any()
                                                          ? new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                                                new XElement(OICPNS.EVSEData + "OperatorID", group.Key.Id.OriginId),

                                                                group.Key.Name.Any()
                                                                    ? new XElement(OICPNS.EVSEData + "OperatorName", group.Key.Name.FirstText)
                                                                    : null,

                                                                new XElement(OICPPlusEVSEOperator + "DataLicense",
                                                                    new XElement(OICPPlusEVSEOperator + "Id",           group.Key.DataLicense.AsShort()),
                                                                    new XElement(OICPPlusEVSEOperator + "Description",  group.Key.DataLicense.AsText()),
                                                                    new XElement(OICPPlusEVSEOperator + "URI",          group.Key.DataLicense.AsLink())
                                                                ),

                                                                // <EvseDataRecord> ... </EvseDataRecord>
                                                                group.Where (evsedatarecord => evsedatarecord != null).
                                                                      Select(evsedatarecord => EVSEDataRecord2XML(evsedatarecord, evsedatarecord.ToXML())).
                                                                      ToArray()

                                                            )
                                                          : null

                                                      ).ToArray()

                                                  )
                                              ),
                                          XMLNamespaces));

        }

        #endregion

        #region ToXML(this EVSEs, XMLNamespaces = null, EVSEStatusRecord2XML = null, XMLPostProcessing = null)

        /// <summary>
        /// Convert the given enumeration of EVSEs into an EVSE status records XML.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSEStatusRecord2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public static XElement ToXML(this IEnumerable<EVSE>        EVSEs,
                                     XMLNamespacesDelegate         XMLNamespaces         = null,
                                     EVSEStatusRecord2XMLDelegate  EVSEStatusRecord2XML  = null,
                                     XMLPostProcessingDelegate     XMLPostProcessing     = null)
        {

            //return CPOClient_XMLMethods.PushEVSEStatusXML();

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException("EVSEs",  "The given enumeration of EVSEs must not be null!");

            if (EVSEStatusRecord2XML == null)
                EVSEStatusRecord2XML = (evsestatusrecord, xml) => xml;

            if (XMLPostProcessing == null)
                XMLPostProcessing = xml => xml;

            #endregion

            return XMLPostProcessing(
                       SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingEvseStatus",
                                              new XElement(OICPNS.EVSEStatus + "EvseStatuses",

                                                  EVSEs.ToLookup(evse => evse.Operator, evse => new EVSEStatusRecord(evse)).
                                                    Select(group =>

                                                      group.Where(evsestatusrecord => evsestatusrecord != null).Any()
                                                          ? new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                                                new XElement(OICPNS.EVSEStatus + "OperatorID", group.Key.Id.OriginId),

                                                                group.Key.Name.Any()
                                                                    ? new XElement(OICPNS.EVSEStatus + "OperatorName", group.Key.Name.FirstText)
                                                                    : null,

                                                                new XElement(OICPPlusEVSEOperator + "DataLicense",
                                                                    new XElement(OICPPlusEVSEOperator + "Id",           group.Key.DataLicense.AsShort()),
                                                                    new XElement(OICPPlusEVSEOperator + "Description",  group.Key.DataLicense.AsText()),
                                                                    new XElement(OICPPlusEVSEOperator + "URI",          group.Key.DataLicense.AsLink())
                                                                ),

                                                                // <EvseStatusRecord> ... </EvseStatusRecord>
                                                                group.Where (evsestatusrecord => evsestatusrecord != null).
                                                                      Select(evsestatusrecord => EVSEStatusRecord2XML(evsestatusrecord, evsestatusrecord.ToXML())).
                                                                      ToArray()

                                                            )
                                                          : null

                                                      ).ToArray()

                                                  )
                                              ),
                                          XMLNamespaces));


        }

        #endregion


    }

}
