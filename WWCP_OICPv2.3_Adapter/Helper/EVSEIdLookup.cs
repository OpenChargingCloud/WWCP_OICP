///*
// * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Linq;
//using System.Collections.Generic;

//using WWCP = cloud.charging.open.protocols.WWCP;

//#endregion

//namespace cloud.charging.open.protocols.OICPv2_3
//{

//    public class EVSEIdLookup
//    {

//        #region Properties

//        public Operator_Id OperatorId { get; }


//        private readonly Dictionary<EVSE_Id, EVSEInfo> _EVSEs;

//        public IEnumerable<EVSEInfo> EVSEs
//            => _EVSEs.Values;

//        #endregion

//        #region Constructor(s)

//        public EVSEIdLookup(Operator_Id                    OperatorId,
//                            IEnumerable<ChargingPoolInfo>  ChargingPoolInfos)
//        {

//            #region Initial checks

//            if (OperatorId.IsNullOrEmpty)
//                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

//            #endregion

//            this.OperatorId  = OperatorId;
//            this._EVSEs      = new Dictionary<EVSE_Id, EVSEInfo>();

//            foreach (var poolinfo in ChargingPoolInfos)
//                foreach (var stationinfo in poolinfo)
//                    foreach (var evseid in stationinfo)

//                        // At least on HubjectQA one EVSEId is not unique!
//                        if (!_EVSEs.ContainsKey(evseid))
//                            _EVSEs.Add(evseid,
//                                       new EVSEInfo(
//                                           OperatorId,
//                                           poolinfo.   PoolId,
//                                           poolinfo.   Address,
//                                           poolinfo.   GeoLocation,
//                                           stationinfo.StationId
//                                       ));

//        }

//        #endregion


//        public EVSEInfo this[EVSE_Id EVSEId]
//            => _EVSEs[EVSEId];

//        public Boolean Contains(EVSE_Id EVSEId)
//            => _EVSEs.ContainsKey(EVSEId);


//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// </summary>
//        public override String ToString()
//        {
//            return _EVSEs.Count + " EVSEs";
//        }

//        #endregion

//    }

//}
