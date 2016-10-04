/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.WebAPI
{

    /// <summary>
    /// OICP XML I/O.
    /// </summary>
    public static class OICP_XML_IO
    {

        #region ToXML(this EVSEDataRecords, RoamingNetwork, XMLNamespaces = null, EVSEDataRecord2XML = null, XMLPostProcessing = null)

        /// <summary>
        /// Convert the given enumeration of EVSE data records to XML.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="RoamingNetwork">The WWCP roaming network.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSEDataRecord2XML">An optional delegate to process an EVSE data record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public static XElement ToXML(this IEnumerable<EVSEDataRecord>  EVSEDataRecords,
                                     RoamingNetwork                    RoamingNetwork,
                                     XMLNamespacesDelegate             XMLNamespaces       = null,
                                     EVSEDataRecord2XMLDelegate        EVSEDataRecord2XML  = null,
                                     XMLPostProcessingDelegate         XMLPostProcessing   = null)
        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException(nameof(EVSEDataRecords), "The given enumeration of EVSE data records must not be null!");

            var _EVSEDataRecords = EVSEDataRecords.ToArray();

            if (EVSEDataRecord2XML == null)
                EVSEDataRecord2XML = (rn, evsedatarecord, xml) => xml;

            if (XMLPostProcessing == null)
                XMLPostProcessing = xml => xml;

            #endregion

            return XMLPostProcessing(
                       SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingEvseData",
                                              new XElement(OICPNS.EVSEData + "EvseData",

                                                  _EVSEDataRecords.Any()

                                                      ? _EVSEDataRecords.
                                                            ToLookup(evsedatarecord => evsedatarecord.EVSE?.Operator).
                                                            Select(group => group.Any(evsedatarecord => evsedatarecord != null)

                                                                       ? new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                                                             new XElement(OICPNS.EVSEData + "OperatorID", group.Key.Id.OriginId),

                                                                             group.Key.Name.Any()
                                                                                 ? new XElement(OICPNS.EVSEData + "OperatorName", group.Key.Name.FirstText)
                                                                                 : null,

                                                                             new XElement(OICPPlusNS.EVSEOperator + "DataLicenses",
                                                                                 group.Key.DataLicenses.SafeSelect(license => new XElement(OICPPlusNS.EVSEOperator + "DataLicense",
                                                                                                                                  new XElement(OICPPlusNS.EVSEOperator + "Id", license.Id),
                                                                                                                                  new XElement(OICPPlusNS.EVSEOperator + "Description", license.Description),
                                                                                                                                  license.URIs.Any()
                                                                                                                                      ? new XElement(OICPPlusNS.EVSEOperator + "DataLicenseURIs",
                                                                                                                                            license.URIs.SafeSelect(uri => new XElement(OICPPlusNS.EVSEOperator + "DataLicenseURI", uri)))
                                                                                                                                      : null
                                                                                                                              ))
                                                                             ),

                                                                             // <EvseDataRecord> ... </EvseDataRecord>
                                                                             group.Where(evsedatarecord => evsedatarecord != null).
                                                                                   Select(evsedatarecord => EVSEDataRecord2XML(RoamingNetwork, evsedatarecord, evsedatarecord.ToXML())).
                                                                                   ToArray()

                                                                         )

                                                                       : null

                                                            ).ToArray()

                                                        : null

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
        /// <param name="RoamingNetwork">The WWCP roaming network.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSEStatusRecord2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public static XElement ToXML(this IEnumerable<EVSE>        EVSEs,
                                     RoamingNetwork                RoamingNetwork,
                                     XMLNamespacesDelegate         XMLNamespaces         = null,
                                     EVSEStatusRecord2XMLDelegate  EVSEStatusRecord2XML  = null,
                                     XMLPostProcessingDelegate     XMLPostProcessing     = null)
        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs),  "The given enumeration of EVSEs must not be null!");

            if (EVSEStatusRecord2XML == null)
                EVSEStatusRecord2XML = (rn, evsestatusrecord, xml) => xml;

            if (XMLPostProcessing == null)
                XMLPostProcessing = xml => xml;

            #endregion

            return XMLPostProcessing(
                       SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingEvseStatus",
                                              new XElement(OICPNS.EVSEStatus + "EvseStatuses",

                                                  EVSEs.ToLookup(evse => evse.Operator,
                                                                 evse => {

                                                                     try
                                                                     {
                                                                         return new EVSEStatusRecord(evse);
                                                                     }
                                                                     catch (Exception)
                                                                     { }

                                                                     return null;

                                                                 }).

                                                        Select(group => group.Any(evsestatusrecord => evsestatusrecord != null)

                                                            ? new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                                                new XElement(OICPNS.EVSEStatus + "OperatorID", group.Key.Id.OriginId),

                                                                group.Key.Name.Any()
                                                                    ? new XElement(OICPNS.EVSEStatus + "OperatorName", group.Key.Name.FirstText)
                                                                    : null,

                                                                new XElement(OICPPlusNS.EVSEOperator + "DataLicenses",
                                                                    group.Key.DataLicenses.SafeSelect(license => new XElement(OICPPlusNS.EVSEOperator + "DataLicense",
                                                                                                                     new XElement(OICPPlusNS.EVSEOperator + "Id",           license.Id),
                                                                                                                     new XElement(OICPPlusNS.EVSEOperator + "Description",  license.Description),
                                                                                                                     license.URIs.Any()
                                                                                                                         ? new XElement(OICPPlusNS.EVSEOperator + "DataLicenseURIs", 
                                                                                                                               license.URIs.SafeSelect(uri => new XElement(OICPPlusNS.EVSEOperator + "DataLicenseURI", uri)))
                                                                                                                         : null
                                                                                                                 ))
                                                                ),

                                                                // <EvseStatusRecord> ... </EvseStatusRecord>
                                                                group.Where (evsestatusrecord => evsestatusrecord != null).
                                                                      Select(evsestatusrecord => EVSEStatusRecord2XML(RoamingNetwork, evsestatusrecord, evsestatusrecord.ToXML())).
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
