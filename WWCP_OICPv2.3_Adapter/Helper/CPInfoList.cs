/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

using WWCP = org.GraphDefined.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class CPInfoList : IEnumerable<ChargingPoolInfo>
    {

        #region Data

        private static Regex InvalidCharactersRegEx = new Regex("[^A-Z0-9]");

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public WWCP.ChargingStationOperator_Id  OperatorId   { get; }


        private readonly Dictionary<ChargingPool_Id, ChargingPoolInfo> _ChargingPools;

        /// <summary>
        /// All charging pools.
        /// </summary>
        public IEnumerable<ChargingPoolInfo> ChargingPools
            => _ChargingPools.Select(v => v.Value);

        #endregion

        #region Constructor(s)

        public CPInfoList(WWCP.ChargingStationOperator_Id OperatorId)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            this.OperatorId      = OperatorId;
            this._ChargingPools  = new Dictionary<ChargingPool_Id, ChargingPoolInfo>();

        }

        #endregion


        #region AddOrUpdateCPInfo(ChargingPoolId, Address, PoolLocation, ChargingStationXMLId, EVSEId)

        public void AddOrUpdateCPInfo(ChargingPool_Id  ChargingPoolId,
                                      Address          Address,
                                      GeoCoordinates?  PoolLocation,
                                      String           ChargingStationXMLId,
                                      EVSE_Id          EVSEId)
        {


            // Existing charging pool...
            if (_ChargingPools.TryGetValue(ChargingPoolId, out ChargingPoolInfo chargingPoolInfo))
                chargingPoolInfo.AddOrUpdateCSInfo(ChargingStationXMLId, EVSEId);

            // ...or a new one!
            else
            {

                chargingPoolInfo = new ChargingPoolInfo(this, ChargingPoolId, Address, PoolLocation);
                chargingPoolInfo.AddOrUpdateCSInfo(ChargingStationXMLId.IsNotNullOrEmpty()
                                                        ? ChargingStationXMLId
                                                        : WWCP.ChargingStation_Id.Create(EVSEId.ToWWCP().Value).ToString(),
                                                    EVSEId);

                _ChargingPools.Add(chargingPoolInfo.PoolId, chargingPoolInfo);

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

                //if (DuplicateChargingStationXMLIds.Length == 0)
                //{
                //    _ChargingPools.Values.
                //         SelectMany(cpinfo => cpinfo.ChargingStations).
                //         ForEach   (csinfo => {

                //             csinfo.StationId = WWCP.ChargingStation_Id.Parse(csinfo.ChargePoolInfo.PoolId.OperatorId,
                //                                                              // Replace invalid characters BEFORE grouping!
                //                                                              InvalidCharactersRegEx.Replace(csinfo.StationXMLId.ToUpper(), "").Substring(30));

                //         });
                //}

                #endregion

                #region ...or they are!

                //else
                //    _ChargingPools.Values.
                //         SelectMany(cpinfo => cpinfo.ChargingStations).
                //         ForEach   (csinfo => {

                //             csinfo.StationId = WWCP.ChargingStation_Id.Parse(csinfo.ChargePoolInfo.PoolId.OperatorId,
                //                                                              new SHA1CryptoServiceProvider().
                //                                                                  ComputeHash(Encoding.UTF8.GetBytes(csinfo.Select(evseid => evseid.ToString()).Aggregate())).
                //                                                                              ToHexString().
                //                                                                              SubstringMax(50).
                //                                                                              ToUpper());

                //          });

                #endregion

            }


            #region Make sure no StationId is null!

            //_ChargingPools.Values.
            //    SelectMany(cpinfo => cpinfo.ChargingStations).
            //    ForEach   (csinfo => {
            //        if (csinfo.StationId == null)
            //        {

            //            csinfo.StationId = ChargingStation_Id.Parse(csinfo.ChargePoolInfo.CPInfoList.OperatorId,
            //                                                        new SHA1CryptoServiceProvider().
            //                                                            ComputeHash(Encoding.UTF8.GetBytes(csinfo.Select(evseid => evseid.ToString()).Aggregate())).
            //                                                                        ToHexString().
            //                                                                        Substring(0, 12).
            //                                                                        ToUpper());

            //        }
            //    });

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

            return new EVSEIdLookup(OperatorId, this);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _ChargingPools.Count + " charging pools";
        }

        #endregion

    }

}
