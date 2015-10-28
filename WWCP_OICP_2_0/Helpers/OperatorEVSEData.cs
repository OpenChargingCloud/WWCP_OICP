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

using System.Linq;
using System.Globalization;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A group of OICP v2.0 EVSE data records.
    /// </summary>
    public class OperatorEVSEData
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

        #region EVSEDataRecords

        private readonly IEnumerable<EVSEDataRecord> _EVSEDataRecords;

        /// <summary>
        /// An enumeration of EVSE data records.
        /// </summary>
        public IEnumerable<EVSEDataRecord> EVSEDataRecords
        {
            get
            {
                return _EVSEDataRecords;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP v2.0 EVSE data records.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an Electric Vehicle Supply Equipment Operator.</param>
        /// <param name="OperatorName">The name of an Electric Vehicle Supply Equipment Operator.</param>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        public OperatorEVSEData(EVSEOperator_Id              OperatorId,
                                String                       OperatorName,
                                IEnumerable<EVSEDataRecord>  EVSEDataRecords)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            #endregion

            this._OperatorId       = OperatorId;
            this._OperatorName     = OperatorName    != null ? OperatorName    : "";
            this._EVSEDataRecords  = EVSEDataRecords != null ? EVSEDataRecords : new EVSEDataRecord[0];

        }

        #endregion


        #region (static) Parse(OperatorEVSEDataXML)

        public static OperatorEVSEData Parse(XElement  OperatorEVSEDataXML)
        {

            #region Initial checks

            if (OperatorEVSEDataXML == null)
                return null;

            #endregion

            try
            {

                return new OperatorEVSEData(EVSEOperator_Id.Parse(OperatorEVSEDataXML.ElementValueOrFail   (OICPNS.EVSEData + "OperatorID",   "Missing OperatorID!")),
                                            OperatorEVSEDataXML.ElementValueOrDefault(OICPNS.EVSEData + "OperatorName", ""),
                                            OperatorEVSEDataXML.Elements             (OICPNS.EVSEData + "EvseDataRecord").
                                                SafeSelect(XML => {

                                                    try
                                                    {
                                                        return XMLMethods.ParseEVSEDataRecordXML(XML);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        return null;
                                                    }

                                                }));

            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region (static) Parse(OperatorEVSEDataXMLs)

        public static IEnumerable<OperatorEVSEData> Parse(IEnumerable<XElement>  OperatorEVSEDataXMLs)
        {

            #region Initial checks

            if (OperatorEVSEDataXMLs == null)
                return new OperatorEVSEData[0];

            var _OperatorEVSEDataXMLs = OperatorEVSEDataXMLs.ToArray();

            if (_OperatorEVSEDataXMLs.Length == 0)
                return new OperatorEVSEData[0];

            #endregion

            try
            {
                return OperatorEVSEDataXMLs.Select(OperatorEVSEDataXML => OperatorEVSEData.Parse(OperatorEVSEDataXML));
            }
            catch (Exception e)
            {
                return new OperatorEVSEData[0];
            }

        }

        #endregion


    }

}
