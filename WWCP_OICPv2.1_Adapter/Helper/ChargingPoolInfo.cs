/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class ChargingPoolInfo : IEquatable<ChargingPoolInfo>,
                                    IEnumerable<ChargeStationInfo>
    {

        #region Properties

        public CPInfoList            CPInfoList     { get; }

        public WWCP.ChargingPool_Id  PoolId         { get; }

        public Address               Address        { get; }

        public GeoCoordinate?        GeoLocation    { get; }


        private readonly List<ChargeStationInfo> _ChargingStations;

        public IEnumerable<ChargeStationInfo> ChargingStations
            => _ChargingStations;

        #endregion

        #region Constructor(s)

        public ChargingPoolInfo(CPInfoList            CPInfoList,
                                WWCP.ChargingPool_Id  PoolId,
                                Address               Address,
                                GeoCoordinate?        GeoLocation)
        {

            #region Initial checks

            if (CPInfoList  == null)
                throw new ArgumentNullException(nameof(CPInfoList),   "The given CPInfoList must not be null!");

            if (PoolId      == null)
                throw new ArgumentNullException(nameof(PoolId),       "The given charging pool identification must not be null!");

            if (Address     == null)
                throw new ArgumentNullException(nameof(Address),      "The given address must not be null!");

            if (GeoLocation == null)
                throw new ArgumentNullException(nameof(GeoLocation),  "The given geo location must not be null!");

            #endregion

            this.CPInfoList         = CPInfoList;
            this.PoolId             = PoolId;
            this.Address            = Address;
            this.GeoLocation        = GeoLocation;
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
                                      FirstOrDefault(CSInfo => CSInfo.StationId    == WWCP.ChargingStation_Id.Create(EVSEId.ToWWCP().Value));

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
