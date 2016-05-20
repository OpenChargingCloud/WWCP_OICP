/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Linq;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class EVSEIdLookup
    {

        #region Properties

        #region EVSEOperatorId

        private readonly EVSEOperator_Id _EVSEOperatorId;

        public EVSEOperator_Id EVSEOperatorId
        {
            get
            {
                return _EVSEOperatorId;
            }
        }

        #endregion

        #region EVSEs

        private readonly Dictionary<EVSE_Id, EVSEInfo> _EVSEs;

        public IEnumerable<EVSEInfo> EVSEs
        {
            get
            {
                return _EVSEs.Select(v => v.Value);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public EVSEIdLookup(EVSEOperator_Id                OperatorId,
                            IEnumerable<ChargingPoolInfo>  ChargingPoolInfos)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            this._EVSEOperatorId  = OperatorId;
            this._EVSEs           = new Dictionary<EVSE_Id, EVSEInfo>();

            foreach (var poolinfo in ChargingPoolInfos)
                foreach (var stationinfo in poolinfo)
                    foreach (var evseid in stationinfo)

                        // At least on HubjectQA one EVSEId is not unique!
                        if (!_EVSEs.ContainsKey(evseid))
                            _EVSEs.Add(evseid, new EVSEInfo(poolinfo.   PoolId,
                                                            poolinfo.   Address,
                                                            poolinfo.   GeoLocation,
                                                            stationinfo.StationId));

        }

        #endregion


        public EVSEInfo this[EVSE_Id EVSEId]
        {
            get
            {
                return _EVSEs[EVSEId];
            }
        }


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _EVSEs.Count + " EVSEs";
        }

        #endregion

    }

}
