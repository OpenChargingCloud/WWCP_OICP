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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class CPInfoList : IEnumerable<ChargingPoolInfo>
    {

        #region Data

        private static Regex InvalidCharactersRegEx = new Regex("[^A-Z0-9]");

        #endregion

        #region Properties

        #region OperatorId

        private readonly ChargingStationOperator_Id _OperatorId;

        public ChargingStationOperator_Id OperatorId
        {
            get
            {
                return _OperatorId;
            }
        }

        #endregion

        #region ChargingPools

        private readonly Dictionary<ChargingPool_Id, ChargingPoolInfo> _ChargingPools;

        public IEnumerable<ChargingPoolInfo> ChargingPools
        {
            get
            {
                return _ChargingPools.Select(v => v.Value);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public CPInfoList(ChargingStationOperator_Id OperatorId)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            this._OperatorId     = OperatorId;
            this._ChargingPools  = new Dictionary<ChargingPool_Id, ChargingPoolInfo>();

        }

        #endregion


        #region AddOrUpdateCPInfo(ChargingPoolId, Address, PoolLocation, ChargingStationXMLId, EVSEId)

        public void AddOrUpdateCPInfo(ChargingPool_Id  ChargingPoolId,
                                      Address          Address,
                                      GeoCoordinate?   PoolLocation,
                                      String           ChargingStationXMLId,
                                      EVSE_Id          EVSEId)
        {

            ChargingPoolInfo _ChargingPoolInfo = null;

            // Existing charging pool...
            if (_ChargingPools.TryGetValue(ChargingPoolId, out _ChargingPoolInfo))
                _ChargingPoolInfo.AddOrUpdateCSInfo(ChargingStationXMLId, EVSEId);

            // ...or a new one!
            else
            {

                _ChargingPoolInfo = new ChargingPoolInfo(this, ChargingPoolId, Address, PoolLocation);
                _ChargingPoolInfo.AddOrUpdateCSInfo(ChargingStationXMLId.IsNotNullOrEmpty()
                                                        ? ChargingStationXMLId
                                                        : ChargingStation_Id.Create(EVSEId.ToWWCP().Value).ToString(),
                                                    EVSEId);

                _ChargingPools.Add(_ChargingPoolInfo.PoolId, _ChargingPoolInfo);

            }

        }

        #endregion

        #region VerifyUniquenessOfChargingStationIds()

        public EVSEIdLookup VerifyUniquenessOfChargingStationIds()
        {

            _ChargingPools.ForEach(cp => cp.Value.Check());

            // ChargingStationIds are not unique!
            var DuplicateChargingStationIds = _ChargingPools.Values.
                                                   SelectMany(cpinfo   => cpinfo.ChargingStations).
                                                   ToLookup  (csinfo   => csinfo.StationId, // <-- !!!
                                                              csinfo   => csinfo.ChargePoolInfo.PoolId).
                                                   Where     (grouping => grouping.Count() > 1).
                                                   ToArray();

            if (DuplicateChargingStationIds.Length > 0)
            {

                // ChargingStationXMLIds are not unique!
                var DuplicateChargingStationXMLIds = _ChargingPools.Values.
                                                          SelectMany(cpinfo   => cpinfo.ChargingStations).
                                                          ToLookup  (csinfo   => csinfo.StationXMLId, // <-- !!!
                                                                     csinfo   => csinfo.ChargePoolInfo.PoolId).
                                                          Where     (grouping => grouping.Count() > 1).
                                                          ToArray();

                #region ChargingStationXMLIds are not unique...

                if (DuplicateChargingStationXMLIds.Length == 0)
                {
                    _ChargingPools.Values.
                         SelectMany(cpinfo => cpinfo.ChargingStations).
                         ForEach   (csinfo => {

                             csinfo.StationId = ChargingStation_Id.Parse(csinfo.ChargePoolInfo.PoolId.OperatorId,
                                                                         // Replace invalid characters BEFORE grouping!
                                                                         InvalidCharactersRegEx.Replace(csinfo.StationXMLId.ToUpper(), "").Substring(30));

                         });
                }

                #endregion

                #region ...or they are!

                else
                    _ChargingPools.Values.
                         SelectMany(cpinfo => cpinfo.ChargingStations).
                         ForEach   (csinfo => {

                             csinfo.StationId = ChargingStation_Id.Parse(csinfo.ChargePoolInfo.PoolId.OperatorId,
                                                                         new SHA1CryptoServiceProvider().
                                                                             ComputeHash(Encoding.UTF8.GetBytes(csinfo.Select(evseid => evseid.ToString()).Aggregate())).
                                                                                         ToHexString().
                                                                                         SubstringMax(50).
                                                                                         ToUpper());

                          });

                #endregion

            }


            #region Make sure no StationId is null!

            _ChargingPools.Values.
                SelectMany(cpinfo => cpinfo.ChargingStations).
                ForEach   (csinfo => {
                    if (csinfo.StationId == null)
                    {

                        csinfo.StationId = ChargingStation_Id.Parse(csinfo.ChargePoolInfo.CPInfoList.OperatorId,
                                                                    new SHA1CryptoServiceProvider().
                                                                        ComputeHash(Encoding.UTF8.GetBytes(csinfo.Select(evseid => evseid.ToString()).Aggregate())).
                                                                                    ToHexString().
                                                                                    Substring(0, 12).
                                                                                    ToUpper());

                    }
                });

            #endregion

            #region Check if all ChargingStationIds are unique

            var CS3 = _ChargingPools.Values.
                          SelectMany(cpinfo   => cpinfo.ChargingStations).
                          ToLookup  (csinfo   => csinfo.StationId,
                                     csinfo   => csinfo.ChargePoolInfo.PoolId).
                          Where     (grouping => grouping.Count() > 1);

            if (CS3.Any())
            {
                DebugX.Log("Still not unique!");
            }

            #endregion

            return new EVSEIdLookup(_OperatorId, this);

        }

        #endregion


        #region IEnumerable<ChargingPoolInfo> Members

        public IEnumerator<ChargingPoolInfo> GetEnumerator()
        {
            return _ChargingPools.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ChargingPools.Values.GetEnumerator();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _ChargingPools.Count + " charging pools";
        }

        #endregion

    }

}
