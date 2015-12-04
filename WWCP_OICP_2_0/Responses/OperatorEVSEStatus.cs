﻿/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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
using System.Collections.Generic;
using System.Xml.Linq;

using System.Linq;
using System.Globalization;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A group of OICP v2.0 EVSE status records.
    /// </summary>
    public class OperatorEVSEStatus
    {

        #region Properties

        #region OperatorId

        private readonly EVSEOperator_Id _OperatorId;

        /// <summary>
        /// The unique identification of an Electric Vehicle Supply Equipment Operator.
        /// </summary>
        public EVSEOperator_Id OperatorId
        {
            get
            {
                return _OperatorId;
            }
        }

        #endregion

        #region OperatorName

        private readonly String _OperatorName;

        /// <summary>
        /// The name of an Electric Vehicle Supply Equipment Operator.
        /// </summary>
        public String OperatorName
        {
            get
            {
                return _OperatorName;
            }
        }

        #endregion

        #region EVSEStatusRecords

        private readonly IEnumerable<EVSEStatusRecord> _EVSEStatusRecords;

        /// <summary>
        /// An enumeration of EVSE status records.
        /// </summary>
        public IEnumerable<EVSEStatusRecord> EVSEStatusRecords
        {
            get
            {
                return _EVSEStatusRecords;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP v2.0 EVSE status records.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an Electric Vehicle Supply Equipment Operator.</param>
        /// <param name="OperatorName">The name of an Electric Vehicle Supply Equipment Operator.</param>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        public OperatorEVSEStatus(EVSEOperator_Id                OperatorId,
                                  String                         OperatorName,
                                  IEnumerable<EVSEStatusRecord>  EVSEStatusRecords)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            #endregion

            this._OperatorId         = OperatorId;
            this._OperatorName       = OperatorName      != null ? OperatorName      : "";
            this._EVSEStatusRecords  = EVSEStatusRecords != null ? EVSEStatusRecords : new EVSEStatusRecord[0];

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

                return new OperatorEVSEStatus(EVSEOperator_Id.Parse(OperatorEVSEStatusXML.ElementValueOrFail(OICPNS.EVSEStatus + "OperatorID", "Missing OperatorID!")),
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
