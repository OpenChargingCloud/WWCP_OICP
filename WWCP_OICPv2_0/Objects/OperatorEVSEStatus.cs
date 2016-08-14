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

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A group of OICP EVSE status records.
    /// </summary>
    public class OperatorEVSEStatus
    {

        #region Properties

        /// <summary>
        /// The unique identification of an Charging Station Operator.
        /// </summary>
        public ChargingStationOperator_Id                OperatorId          { get; }

        /// <summary>
        /// The name of an Charging Station Operator.
        /// </summary>
        public String                         OperatorName        { get; }

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord>  EVSEStatusRecords   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP EVSE status records.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an Charging Station Operator.</param>
        /// <param name="OperatorName">The name of an Charging Station Operator.</param>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        public OperatorEVSEStatus(ChargingStationOperator_Id                OperatorId,
                                  String                         OperatorName,
                                  IEnumerable<EVSEStatusRecord>  EVSEStatusRecords)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            this.OperatorId         = OperatorId;
            this.OperatorName       = OperatorName      != null ? OperatorName      : "";
            this.EVSEStatusRecords  = EVSEStatusRecords != null ? EVSEStatusRecords : new EVSEStatusRecord[0];

        }

        #endregion


        #region (static) Parse(OperatorEVSEStatusXML)

        public static OperatorEVSEStatus Parse(XElement OperatorEVSEStatusXML)
        {

            #region Initial checks

            if (OperatorEVSEStatusXML == null)
                return null;

            #endregion

            try
            {

                return new OperatorEVSEStatus(ChargingStationOperator_Id.Parse(OperatorEVSEStatusXML.ElementValueOrFail(OICPNS.EVSEStatus + "OperatorID", "Missing OperatorID!")),
                                              OperatorEVSEStatusXML.ElementValueOrDefault(OICPNS.EVSEStatus + "OperatorName", ""),
                                              OperatorEVSEStatusXML.Elements             (OICPNS.EVSEStatus + "EvseStatusRecord").
                                                                    SafeSelect(EvseStatusRecordXML => EVSEStatusRecord.Parse(EvseStatusRecordXML)).
                                                                    Where     (statusrecord        => statusrecord != null));

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region (static) Parse(OperatorEVSEDataXMLs)

        public static IEnumerable<OperatorEVSEStatus> Parse(IEnumerable<XElement> OperatorEVSEStatusXMLs)
        {

            #region Initial checks

            if (OperatorEVSEStatusXMLs == null)
                return new OperatorEVSEStatus[0];

            var _OperatorEVSEStatusXMLs = OperatorEVSEStatusXMLs.ToArray();

            if (_OperatorEVSEStatusXMLs.Length == 0)
                return new OperatorEVSEStatus[0];

            #endregion

            try
            {
                return OperatorEVSEStatusXMLs.Select(OperatorEVSEStatusXML => OperatorEVSEStatus.Parse(OperatorEVSEStatusXML));
            }
            catch (Exception e)
            {
                return new OperatorEVSEStatus[0];
            }

        }

        #endregion


    }

}
