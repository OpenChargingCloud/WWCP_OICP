/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    public class EVSEIdLookup
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

        #region ChargingPools

        private readonly Dictionary<EVSE_Id, EVSEInfo> _ChargingPools;

        public IEnumerable<EVSEInfo> ChargingPools
        {
            get
            {
                return _ChargingPools.Select(v => v.Value);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public EVSEIdLookup(EVSEOperator_Id                OperatorId,
                            IEnumerable<ChargingPoolInfo>  Data)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            #endregion

            this._OperatorId     = OperatorId;
            this._ChargingPools  = new Dictionary<EVSE_Id, EVSEInfo>();

            foreach (var pool in Data)
                foreach (var station in pool)
                    foreach (var evseid in station)

                        // At least on HubjectQA one EVSEId is not unique!
                        if (!_ChargingPools.ContainsKey(evseid))
                            _ChargingPools.Add(evseid, new EVSEInfo(pool.PoolId, pool.Address, pool.GeoLocation, station.StationId));

        }

        #endregion


        public EVSEInfo this[EVSE_Id EVSEId]
        {
            get
            {
                return _ChargingPools[EVSEId];
            }
        }


        #region (override) ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return _ChargingPools.Count + " charging pools";
        }

        #endregion

    }

}
