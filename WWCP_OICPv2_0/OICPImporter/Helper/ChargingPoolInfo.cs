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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    public class ChargingPoolInfo : IEquatable<ChargingPoolInfo>,
                                    IEnumerable<ChargeStationInfo>
    {

        #region Properties

        #region CPInfoList

        private readonly CPInfoList _CPInfoList;

        public CPInfoList CPInfoList
        {
            get
            {
                return _CPInfoList;
            }
        }

        #endregion

        #region PoolId

        private readonly ChargingPool_Id _PoolId;

        public ChargingPool_Id PoolId
        {
            get
            {
                return _PoolId;
            }
        }

        #endregion

        #region Address

        private readonly Address _Address;

        public Address Address
        {
            get
            {
                return _Address;
            }
        }

        #endregion

        #region GeoLocation

        private readonly GeoCoordinate _GeoLocation;

        public GeoCoordinate GeoLocation
        {
            get
            {
                return _GeoLocation;
            }
        }

        #endregion

        #region ChargingStations

        private readonly List<ChargeStationInfo> _ChargingStations;

        public IEnumerable<ChargeStationInfo> ChargingStations
        {
            get
            {
                return _ChargingStations;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public ChargingPoolInfo(CPInfoList       CPInfoList,
                                ChargingPool_Id  PoolId,
                                Address          Address,
                                GeoCoordinate    GeoLocation)
        {

            #region Initial checks

            if (CPInfoList  == null)
                throw new ArgumentNullException("CPInfoList",  "The given parameter must not be null!");

            if (PoolId      == null)
                throw new ArgumentNullException("PoolId",      "The given parameter must not be null!");

            if (Address     == null)
                throw new ArgumentNullException("Address",     "The given parameter must not be null!");

            if (GeoLocation == null)
                throw new ArgumentNullException("GeoLocation", "The given parameter must not be null!");

            #endregion

            this._CPInfoList        = CPInfoList;
            this._PoolId            = PoolId;
            this._Address           = Address;
            this._GeoLocation       = GeoLocation;
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
                                      Where(CSInfo => CSInfo.StationXMLId == ChargingStationXMLId).
                                      //Where(CSInfo => CSInfo.StationId    == ChargingStation_Id.Create(EVSEId)).
                                      FirstOrDefault()
                                : _ChargingStations.
                                      Where(CSInfo => CSInfo.StationId    == ChargingStation_Id.Create(EVSEId)).
                                      FirstOrDefault();

            if (ExCSInfos == null)
                AddCSInfo(ChargingStationXMLId, EVSEId);

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
        {
            return _ChargingStations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ChargingStations.GetEnumerator();
        }

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

            return ChargePoolInfo._PoolId.Equals(_PoolId);

        }

        #endregion

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "'" + PoolId + "' => " + _ChargingStations.Count + " charging stations, " + _ChargingStations.SelectMany(v => v.EVSEIds).Count() + " EVSEs";
        }

        #endregion

    }

}
