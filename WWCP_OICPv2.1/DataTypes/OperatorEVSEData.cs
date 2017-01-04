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
    /// A group of OICP EVSE Data Records.
    /// </summary>
    public class OperatorEVSEData
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord>  EVSEDataRecords   { get; }

        /// <summary>
        /// The unqiue identification of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public Operator_Id                  OperatorId        { get; }

        /// <summary>
        /// An optional name of the charging station operator maintaining the given EVSE data records.
        /// </summary>
        public String                       OperatorName      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP EVSE data records.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">An optional name of the charging station operator maintaining the given EVSE data records.</param>
        public OperatorEVSEData(IEnumerable<EVSEDataRecord>  EVSEDataRecords,
                                Operator_Id                  OperatorId,
                                String                       OperatorName)
        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException(nameof(EVSEDataRecords), "The given enumeration of EVSE data records must not be null!");

            #endregion

            this.EVSEDataRecords  = EVSEDataRecords;
            this.OperatorId       = OperatorId;
            this.OperatorName     = OperatorName;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
        //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        // 
        // [...]
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

        #region (static) Parse(OperatorEVSEDataXML,  OnException = null)

        /// <summary>
        /// Parse the givem XML as an eumeration of OICP EVSE Data Records.
        /// </summary>
        /// <param name="OperatorEVSEDataXML">A XML representation of an enumeration of OICP EVSE Data Records.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static OperatorEVSEData Parse(XElement             OperatorEVSEDataXML,
                                             OnExceptionDelegate  OnException = null)
        {

            #region Initial checks

            if (OperatorEVSEDataXML == null)
                return null;

            #endregion

            try
            {

                return new OperatorEVSEData(

                           OperatorEVSEDataXML.MapElements          (OICPNS.EVSEData + "EvseDataRecord",
                                                                     (EvseDataRecordXML, e) => EVSEDataRecord.Parse(EvseDataRecordXML, e),
                                                                     OnException),

                           OperatorEVSEDataXML.MapValueOrFail       (OICPNS.EVSEData + "OperatorID",
                                                                     Operator_Id.Parse,
                                                                     "Missing OperatorID!"),

                           OperatorEVSEDataXML.ElementValueOrDefault(OICPNS.EVSEData + "OperatorName")

                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, OperatorEVSEDataXML, e);

                return null;

            }

        }

        #endregion

        #region (static) Parse(OperatorEVSEDataXMLs, OnException = null)

        /// <summary>
        /// Parse the givem XML as an enumeration of OICP EVSE Data Records.
        /// </summary>
        /// <param name="OperatorEVSEDataXMLs">A XML representation of an enumeration of OICP EVSE Data Records.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static IEnumerable<OperatorEVSEData> Parse(IEnumerable<XElement>  OperatorEVSEDataXMLs,
                                                          OnExceptionDelegate    OnException = null)
        {

            #region Initial checks

            if (OperatorEVSEDataXMLs == null)
                return new OperatorEVSEData[0];

            #endregion

            return OperatorEVSEDataXMLs.
                       SafeSelect(OperatorEVSEDataXML => Parse(OperatorEVSEDataXML, OnException)).
                       Where     (OperatorEvseData    => OperatorEvseData != null);

        }

        #endregion

    }

}
