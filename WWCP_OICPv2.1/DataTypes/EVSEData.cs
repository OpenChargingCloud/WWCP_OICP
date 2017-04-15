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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A group of OICP operator EVSE data records or a status code.
    /// </summary>
    public class EVSEData
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records grouped by their operators.
        /// </summary>
        public IEnumerable<OperatorEVSEData>  OperatorEVSEData  { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode                     StatusCode        { get; }

        #endregion

        #region Constructor(s)

        #region EVSEData(OperatorEVSEData, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE data records or a status code.
        /// </summary>
        /// <param name="OperatorEVSEData">An enumeration of EVSE data records grouped by their operators.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        public EVSEData(IEnumerable<OperatorEVSEData>  OperatorEVSEData,
                        StatusCode                     StatusCode  = null)
        {

            #region Initial checks

            if (OperatorEVSEData == null)
                throw new ArgumentNullException(nameof(OperatorEVSEData),  "The given operator EVSE data must not be null!");

            #endregion

            this.OperatorEVSEData  = OperatorEVSEData;
            this.StatusCode        = StatusCode ?? new StatusCode(StatusCodes.Success);

        }

        #endregion

        #region EVSEData(Code, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new group of OICP operator EVSE data records or a status code.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        public EVSEData(StatusCodes  Code,
                        String       Description     = null,
                        String       AdditionalInfo  = null)

            : this(new OperatorEVSEData[0],
                   new StatusCode(Code,
                                  Description,
                                  AdditionalInfo))

        { }

        #endregion

        #endregion


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

        #region (static) Parse(eRoamingEVSEDataXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as EVSE data records or a status code.
        /// </summary>
        /// <param name="EVSEDataXML">A XML representation of EVSE data records or a status code.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEData Parse(XElement             EVSEDataXML,
                                     OnExceptionDelegate  OnException = null)
        {

            if (EVSEDataXML.Name != OICPNS.EVSEData + "eRoamingEvseData")
                throw new Exception("Invalid eRoamingEvseData XML!");

            var _EVSEDataXML  = EVSEDataXML.Element   (OICPNS.EVSEData + "EvseData");
            var _StatusCode   = EVSEDataXML.MapElement(OICPNS.EVSEData + "StatusCode",
                                                       StatusCode.Parse);

            if (_EVSEDataXML != null)
            {

                var OperatorEvseDataXMLs = _EVSEDataXML.Elements(OICPNS.EVSEData + "OperatorEvseData");

                if (OperatorEvseDataXMLs != null)
                    return new EVSEData(OICPv2_1.OperatorEVSEData.Parse(OperatorEvseDataXMLs, OnException),
                                                _StatusCode);

            }

            return _StatusCode != null
                       ? new EVSEData(_StatusCode.Code,
                                              _StatusCode.Description,
                                              _StatusCode.AdditionalInfo)
                       : new EVSEData(StatusCodes.DataError);

        }

        #endregion


    }

}
