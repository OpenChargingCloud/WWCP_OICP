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

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A group of OICP v2.0 EVSE data records.
    /// </summary>
    public class eRoamingEVSEData
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
        public eRoamingEVSEData(EVSEOperator_Id              OperatorId,
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

    }

}
