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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// Extention methods for OICP EVSE search results.
    /// </summary>
    public static class EVSESearchResultExtentions
    {

        #region HasResults(this EVSESearchResult)

        /// <summary>
        /// The enumeration of EVSE matches has values.
        /// </summary>
        public static Boolean HasResults(this EVSESearchResult EVSESearchResult)
        {

            if (EVSESearchResult == null)
                return false;

            return EVSESearchResult.EVSEMatches.Any();

        }

        #endregion

    }


    /// <summary>
    /// An OICP EVSE search result.
    /// </summary>
    public class EVSESearchResult
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE matches.
        /// </summary>
        public IEnumerable<EVSEMatch> EVSEMatches { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP EVSE search matches.
        /// </summary>
        /// <param name="EVSEMatches">An enumeration of EVSE matches.</param>
        public EVSESearchResult(IEnumerable<EVSEMatch> EVSEMatches)
        {

            #region Initial checks

            if (EVSEMatches == null)
                throw new ArgumentNullException(nameof(EVSEMatches),  "The given enumeration of matching EVSEs must not be null!");

            #endregion

            this.EVSEMatches  = EVSEMatches;

        }

        #endregion


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

        #region (static) Parse(EVSESearchResultXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as an OICP EVSE search result.
        /// </summary>
        /// <param name="EVSESearchResultXML">A XML representation of an OICP EVSE search result.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSESearchResult Parse(XElement             EVSESearchResultXML,
                                             OnExceptionDelegate  OnException = null)
        {

            if (EVSESearchResultXML.Name != OICPNS.EVSESearch + "eRoamingEvseSearchResult")
                throw new Exception("Invalid eRoamingEvseSearchResult XML!");

            return new EVSESearchResult(EVSESearchResultXML.MapElementsOrFail(OICPNS.EVSESearch + "EvseMatches",
                                                                              OICPNS.EVSESearch + "EvseMatch",
                                                                              EVSEMatch.Parse,
                                                                              OnException));

        }

        #endregion


    }

}
