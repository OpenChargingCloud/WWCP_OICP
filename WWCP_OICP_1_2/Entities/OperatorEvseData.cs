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

namespace org.GraphDefined.WWCP.OICP_1_2
{

    /// <summary>
    /// A OICP v1.2 Operator EVSE data set.
    /// </summary>
    public class OperatorEvseData
    {

        #region Properties

        #region OperatorId

        private readonly EVSEOperator_Id _OperatorId;

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

        public OperatorEvseData(EVSEOperator_Id              OperatorId,
                                String                       OperatorName,
                                IEnumerable<EVSEDataRecord>  EVSEDataRecords)

        {

            this._OperatorId       = OperatorId;
            this._OperatorName     = OperatorName;
            this._EVSEDataRecords  = EVSEDataRecords;

        }

        #endregion

    }

}
