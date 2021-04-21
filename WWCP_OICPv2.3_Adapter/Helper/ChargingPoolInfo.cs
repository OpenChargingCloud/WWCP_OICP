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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

using WWCP = org.GraphDefined.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class ChargingPoolInfo : IEquatable<ChargingPoolInfo>,
                                    IEnumerable<ChargeStationInfo>
    {

        #region Data

        private readonly List<ChargeStationInfo> _ChargingStations;

        #endregion

        #region Properties

        public CPInfoList                      CPInfoList     { get; }

        public ChargingPool_Id                 PoolId         { get; }

        public Address                         Address        { get; }

        public GeoCoordinates?                 GeoLocation    { get; }

        public IEnumerable<ChargeStationInfo>  ChargingStations
            => _ChargingStations;

        #endregion

        #region Constructor(s)

        public ChargingPoolInfo(CPInfoList       CPInfoList,
                                ChargingPool_Id  PoolId,
                                Address          Address,
                                GeoCoordinates?  GeoLocation)
        {

            this.CPInfoList         = CPInfoList  ?? throw new ArgumentNullException(nameof(CPInfoList),   "The given CPInfoList must not be null!");
            this.PoolId             = PoolId;
            this.Address            = Address     ?? throw new ArgumentNullException(nameof(Address),      "The given address must not be null!");
            this.GeoLocation        = GeoLocation ?? throw new ArgumentNullException(nameof(GeoLocation),  "The given geo location must not be null!");
            this._ChargingStations  = new List<ChargeStationInfo>();

        }

        #endregion


        #region (private) AddCSInfo(ChargingStationXMLId, EVSEId)

        private void AddCSInfo(String   ChargingStationXMLId,
                               EVSE_Id  EVSEId)
        {

            var _CSInfo = new ChargeStationInfo(this, ChargingStationXMLId, EVSEId);

            this._ChargingStations.Add(_CSInfo);

            Check();

        }

        #endregion

        #region AddOrUpdateCSInfo(ChargingStationXMLId, EVSEId)

        public void AddOrUpdateCSInfo(String   ChargingStationXMLId,
                                      EVSE_Id  EVSEId)
        {

            var ExCSInfos = ChargingStationXMLId.IsNotNullOrEmpty()
                                ? _ChargingStations.
                                      FirstOrDefault(CSInfo => CSInfo.StationXMLId == ChargingStationXMLId)
                                : _ChargingStations.
                                      FirstOrDefault(CSInfo => CSInfo.StationId    == ChargingStation_Id.Parse(EVSEId.ToWWCP().Value.ToString()));

            if (ChargingStationXMLId.IsNullOrEmpty())
            {

            }

            if (ExCSInfos == null)
                AddCSInfo(ChargingStationXMLId.IsNotNullOrEmpty()
                              ? ChargingStationXMLId
                              : WWCP.ChargingStation_Id.Create(EVSEId.ToWWCP().Value).ToString(),
                          EVSEId);

            else
            {
                ExCSInfos.AddEVSEId(EVSEId);
                Check();
            }

        }

        #endregion

        #region Check()

        public void Check()
        {
            _ChargingStations.ForEach(cs => cs.Check());
        }

        #endregion


        #region IEnumerable<ChargeStationInfo> Members

        public IEnumerator<ChargeStationInfo> GetEnumerator()
            => _ChargingStations.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _ChargingStations.GetEnumerator();

        #endregion

        #region IEquatable<CPInfo> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an CPInfo.
            var CPInfo = Object as ChargingPoolInfo;
            if ((Object) CPInfo == null)
                return false;

            return this.Equals(CPInfo);

        }

        #endregion

        #region Equals(ChargePoolInfo)

        /// <summary>
        /// Compares two CPInfos for equality.
        /// </summary>
        /// <param name="ChargePoolInfo">A CPInfo to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolInfo ChargePoolInfo)
        {

            if ((Object) ChargePoolInfo == null)
                return false;

            return ChargePoolInfo.PoolId.Equals(PoolId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return PoolId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", PoolId, "' => ",
                             _ChargingStations.Count,
                             " charging stations, ",
                             _ChargingStations.SelectMany(v => v.EVSEIds).Count(),
                             " EVSEs");

        #endregion

    }

}
