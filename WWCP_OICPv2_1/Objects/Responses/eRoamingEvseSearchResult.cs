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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public static partial class Ext
    {

        #region HasResults(this eRoamingEvseSearchResult)

        /// <summary>
        /// The enumeration of EVSE matches has values.
        /// </summary>
        public static Boolean HasResults(this eRoamingEvseSearchResult eRoamingEvseSearchResult)
        {

            if (eRoamingEvseSearchResult == null)
                return false;

            return eRoamingEvseSearchResult.EVSEMatches.Any();

        }

        #endregion

    }


    /// <summary>
    /// A group of OICP v2.0 EVSE search result.
    /// </summary>
    public class eRoamingEvseSearchResult
    {

        #region Properties

        #region EVSEMatches

        private readonly IEnumerable<EVSEMatch> _EVSEMatches;

        /// <summary>
        /// An enumeration of EVSE matches.
        /// </summary>
        public IEnumerable<EVSEMatch> EVSEMatches
        {
            get
            {
                return _EVSEMatches;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP v2.0 EVSE search result.
        /// </summary>
        /// <param name="EVSEMatches">An enumeration of EVSE matches.</param>
        public eRoamingEvseSearchResult(IEnumerable<EVSEMatch> EVSEMatches)
        {

            #region Initial checks

            if (EVSEMatches == null)
                throw new ArgumentNullException("EVSEMatches", "The given parameter must not be null!");

            #endregion

            this._EVSEMatches  = EVSEMatches;

        }

        #endregion


        #region (static) Parse(eRoamingEvseSearchResultXML, OnException = null)

        public static eRoamingEvseSearchResult Parse(XElement             eRoamingEvseSearchResultXML,
                                                     OnExceptionDelegate  OnException = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSESearch  = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <EVSESearch:eRoamingEvseSearchResult>
            //          <EVSESearch:EvseMatches>
            //
            //             <!--Zero or more repetitions:-->
            //             <EVSESearch:EvseMatch>
            //
            //                <EVSESearch:Distance>99.1</EVSESearch:Distance>
            //
            //                <EVSESearch:EVSE deltaType="?" lastUpdate="?">
            //
            //                   [...]
            //
            //                </EVSESearch:EVSE>
            //             </EVSESearch:EvseMatch>
            //          </EVSESearch:EvseMatches>
            //       </EVSESearch:eRoamingEvseSearchResult>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            if (eRoamingEvseSearchResultXML.Name != OICPNS.EVSESearch + "eRoamingEvseSearchResult")
                throw new Exception("Invalid eRoamingEvseSearchResult XML!");

            return new eRoamingEvseSearchResult(eRoamingEvseSearchResultXML.MapElementsOrFail(OICPNS.EVSESearch + "EvseMatches",
                                                                                              "XML element 'EvseMatches' not found!",
                                                                                              OICPNS.EVSESearch + "EvseMatch",
                                                                                              EVSEMatch.Parse,
                                                                                              OnException));

        }

        #endregion


    }

}
