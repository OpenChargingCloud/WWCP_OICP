/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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
using System.Collections.Generic;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A group of OICP v2.0 operator EVSE data records or a status code.
    /// </summary>
    public class eRoamingEVSEData
    {

        #region Properties

        #region OperatorEVSEData

        private readonly IEnumerable<OperatorEVSEData> _OperatorEVSEData;

        /// <summary>
        /// An enumeration of EVSE data records grouped by their operators.
        /// </summary>
        public IEnumerable<OperatorEVSEData> OperatorEVSEData
        {
            get
            {
                return _OperatorEVSEData;
            }
        }

        #endregion

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) eRoamingEVSEData(OperatorEVSEData, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP v2.0 operator EVSE data records or a status code.
        /// </summary>
        /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        private eRoamingEVSEData(IEnumerable<OperatorEVSEData>  OperatorEVSEData,
                                 StatusCode                     StatusCode  = null)
        {

            #region Initial checks

            if (OperatorEVSEData == null)
                throw new ArgumentNullException("OperatorEVSEData", "The given parameter must not be null!");

            #endregion

            this._OperatorEVSEData  = OperatorEVSEData;
            this._StatusCode        = StatusCode != null ? StatusCode : new StatusCode(0);

        }

        #endregion

        #region (private) eRoamingEVSEData(StatusCode)

        /// <summary>
        /// Create a new group of OICP v2.0 operator EVSE data records or a status code.
        /// </summary>
        /// <pparam name="StatusCode">The status code for this request.</pparam>
        private eRoamingEVSEData(StatusCode  StatusCode)
        {

            this._OperatorEVSEData  = new OperatorEVSEData[0];
            this._StatusCode        = StatusCode == null ? StatusCode : new StatusCode(-1);

        }

        #endregion

        #endregion


        #region (static) Parse(eRoamingEVSEDataXML)

        public static eRoamingEVSEData Parse(XElement eRoamingEVSEDataXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            // [...]
            //
            //  <EVSEData:eRoamingEvseData>
            //
            //     <EVSEData:EvseData>
            //
            //        <!--Zero or more repetitions:-->
            //        <EVSEData:OperatorEvseData>
            //
            //           <EVSEData:OperatorID>?</EVSEData:OperatorID>
            //
            //           <!--Optional:-->
            //           <EVSEData:OperatorName>?</EVSEData:OperatorName>
            //
            //           <!--Zero or more repetitions:-->
            //           <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="?">
            //              [...]
            //           </EVSEData:EvseDataRecord>
            //
            //        </EVSEData:OperatorEvseData>
            //
            //     </EVSEData:EvseData>
            //
            //     <!--Optional:-->
            //     <EVSEData:StatusCode>
            //
            //        <CommonTypes:Code>?</CommonTypes:Code>
            //
            //        <!--Optional:-->
            //        <CommonTypes:Description>?</CommonTypes:Description>
            //
            //        <!--Optional:-->
            //        <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
            //
            //     </EVSEData:StatusCode>
            //
            //  </EVSEData:eRoamingEvseData>
            //
            // [...]

            #endregion

            if (eRoamingEVSEDataXML.Name != OICPNS.EVSEData + "eRoamingEvseData")
                throw new Exception("Invalid eRoamingEvseData XML!");

            var EVSEDataXML    = eRoamingEVSEDataXML.Element(OICPNS.EVSEData + "EvseData");
            var StatusCodeXML  = eRoamingEVSEDataXML.Element(OICPNS.EVSEData + "StatusCode");

            if (EVSEDataXML != null)
            {

                var OperatorEvseDataXMLs = EVSEDataXML.Elements(OICPNS.EVSEData + "OperatorEvseData");

                if (OperatorEvseDataXMLs != null)
                    return new eRoamingEVSEData(OICP_2_0.OperatorEVSEData.Parse(OperatorEvseDataXMLs),
                                                StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);

            }

            return new eRoamingEVSEData(StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);

        }

        #endregion


    }

}
