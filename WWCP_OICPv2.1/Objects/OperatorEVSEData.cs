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

    /// <summary>
    /// A group of OICP EVSE data records.
    /// </summary>
    public class OperatorEVSEData
    {

        #region Properties

        /// <summary>
        /// The unique identification of an Charging Station Operator.
        /// </summary>
        public ChargingStationOperator_Id              OperatorId        { get; }

        /// <summary>
        /// The name of an Charging Station Operator.
        /// </summary>
        public I18NString                   OperatorName      { get; }

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP EVSE data records.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an Charging Station Operator.</param>
        /// <param name="OperatorName">The name of an Charging Station Operator.</param>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        public OperatorEVSEData(ChargingStationOperator_Id              OperatorId,
                                I18NString                   OperatorName,
                                IEnumerable<EVSEDataRecord>  EVSEDataRecords)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            this.OperatorId       = OperatorId;
            this.OperatorName     = OperatorName    != null ? OperatorName    : new I18NString();
            this.EVSEDataRecords  = EVSEDataRecords != null ? EVSEDataRecords : new EVSEDataRecord[0];

        }

        #endregion


        #region (static) Parse(OperatorEVSEDataXML, OnException = null)

        public static OperatorEVSEData Parse(XElement             OperatorEVSEDataXML,
                                             OnExceptionDelegate  OnException = null)
        {

            #region Initial checks

            if (OperatorEVSEDataXML == null)
                return null;

            #endregion

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            // 
            //      <!--Zero or more repetitions:-->
            //      <EVSEData:OperatorEvseData>
            // 
            //         <EVSEData:OperatorID>?</EVSEData:OperatorID>
            // 
            //         <!--Optional:-->
            //         <EVSEData:OperatorName>?</EVSEData:OperatorName>
            // 
            //         <!--Zero or more repetitions:-->
            //         <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="?">
            //            [...]
            //         </EVSEData:EvseDataRecord>
            // 
            //      </EVSEData:OperatorEvseData>
            //
            // [...]
            //
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion

            try
            {

                return new OperatorEVSEData(OperatorEVSEDataXML.MapValueOrFail(OICPNS.EVSEData + "OperatorID",
                                                                               ChargingStationOperator_Id.Parse,
                                                                               "Missing OperatorID!"),

                                            OperatorEVSEDataXML.MapValueOrNull(OICPNS.EVSEData + "OperatorName",
                                                                               v => new I18NString(Languages.unknown, v)),

                                            OperatorEVSEDataXML.MapElements(OICPNS.EVSEData + "EvseDataRecord",
                                                                            (EvseDataRecordXML, e) => EVSEDataRecord.Parse(EvseDataRecordXML, e),
                                                                            OnException)
                                           );

            }
            catch (Exception e)
            {

                if (OnException != null)
                    OnException(DateTime.Now, OperatorEVSEDataXML, e);

                return null;

            }

        }

        #endregion

        #region (static) Parse(OperatorEVSEDataXMLs, OnException = null)

        public static IEnumerable<OperatorEVSEData> Parse(IEnumerable<XElement>  OperatorEVSEDataXMLs,
                                                          OnExceptionDelegate    OnException = null)
        {

            #region Initial checks

            if (OperatorEVSEDataXMLs == null)
                return new OperatorEVSEData[0];

            var _OperatorEVSEDataXMLs = OperatorEVSEDataXMLs.ToArray();

            if (_OperatorEVSEDataXMLs.Length == 0)
                return new OperatorEVSEData[0];

            #endregion

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            // 
            //      <EVSEData:EvseData>
            //         <!--Zero or more repetitions:-->
            //         <EVSEData:OperatorEvseData>
            // 
            //            <EVSEData:OperatorID>?</EVSEData:OperatorID>
            // 
            //            <!--Optional:-->
            //            <EVSEData:OperatorName>?</EVSEData:OperatorName>
            // 
            //            <!--Zero or more repetitions:-->
            //            <EVSEData:EvseDataRecord deltaType="update|insert|delete" lastUpdate="?">
            //               [...]
            //            </EVSEData:EvseDataRecord>
            // 
            //         </EVSEData:OperatorEvseData>
            //      </EVSEData:EvseData>
            //
            // [...]
            //
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion

            return OperatorEVSEDataXMLs.
                       Select(OperatorEVSEDataXML => OperatorEVSEData.Parse(OperatorEVSEDataXML, OnException)).
                       Where (OperatorEvseData    => OperatorEvseData != null);

        }

        #endregion

    }

}
