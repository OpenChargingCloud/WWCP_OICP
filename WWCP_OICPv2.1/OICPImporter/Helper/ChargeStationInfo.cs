/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class ChargeStationInfo : IEquatable<ChargeStationInfo>,
                                     IEnumerable<EVSE_Id>
    {

        #region Properties

        #region ChargePoolInfo

        private readonly ChargingPoolInfo _ChargePoolInfo;

        public ChargingPoolInfo ChargePoolInfo
        {
            get
            {
                return _ChargePoolInfo;
            }
        }

        #endregion

        #region StationXMLId

        private readonly String _StationXMLId;

        public String StationXMLId
        {
            get
            {
                return _StationXMLId;
            }
        }

        #endregion

        #region StationId

        private ChargingStation_Id _StationId;

        public ChargingStation_Id StationId
        {

            get
            {
                return _StationId;
            }

            internal set
            {
                if (value != null)
                    _StationId = value;
            }

        }

        #endregion

        #region EVSEIds

        private readonly List<EVSE_Id> _EVSEIds;

        public IEnumerable<EVSE_Id> EVSEIds
        {
            get
            {
                return _EVSEIds;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public ChargeStationInfo(ChargingPoolInfo  ChargePoolInfo,
                                 String            StationXMLId,
                                 EVSE_Id           EVSEId)
        {

            #region Initial checks

            if (ChargePoolInfo == null)
                throw new ArgumentNullException("ChargePoolInfo", "The given parameter must not be null!");

            if (EVSEId         == null)
                throw new ArgumentNullException(nameof(EVSEId),         "The given parameter must not be null!");

            #endregion

            this._ChargePoolInfo  = ChargePoolInfo;
            this._StationXMLId    = StationXMLId != null ? StationXMLId : "";
            this._EVSEIds         = new List<EVSE_Id>() { EVSEId };

            Check();

        }

        #endregion


        #region AddEVSEId(EVSEId)

        public void AddEVSEId(EVSE_Id EVSEId)
        {
            this._EVSEIds.Add(EVSEId);
            Check();
        }

        #endregion

        #region Check()

        public void Check()
        {

            _StationId = null;

            // 1st: Try to use the given ChargingStationId from the XML...
            if (StationXMLId.StartsWith(ChargePoolInfo.CPInfoList.OperatorId.ToFormat(IdFormatType.OLD)) ||
                StationXMLId.StartsWith(ChargePoolInfo.CPInfoList.OperatorId.ToFormat(IdFormatType.NEW)))
                ChargingStation_Id.TryParse(StationXMLId, out _StationId);

            // 2nd: Try to use the given EVSE Ids to find a common prefix...
            if (_StationId == null)
                _StationId = ChargingStation_Id.Create(this._EVSEIds);

            // Alternative: Try to use a modified StationXML Id...
            if (_StationId == null)
            {
                var rgx = new Regex("[^A-Z0-9]");
                ChargingStation_Id.TryParse(ChargePoolInfo.PoolId.OperatorId, rgx.Replace(_StationXMLId.ToUpper(), ""), out _StationId);
            }

        }

        #endregion


        #region IEnumerable<EVSE_Id> Members

        public IEnumerator<EVSE_Id> GetEnumerator()
        {
            return _EVSEIds.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _EVSEIds.GetEnumerator();
        }

        #endregion

        #region IEquatable<CSInfo> Members

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

            // Check if the given object is an CSInfo.
            var CSInfo = Object as ChargeStationInfo;
            if ((Object) CSInfo == null)
                return false;

            return this.Equals(CSInfo);

        }

        #endregion

        #region Equals(ChargeStationInfo)

        /// <summary>
        /// Compares two CSInfos for equality.
        /// </summary>
        /// <param name="ChargeStationInfo">A CSInfo to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeStationInfo ChargeStationInfo)
        {

            if ((Object) ChargeStationInfo == null)
                return false;

            return ChargeStationInfo._StationXMLId.Equals(_StationXMLId);

        }

        #endregion

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "'" + StationXMLId + "' / '" + StationId + "' => " + _EVSEIds.Count + " EVSEs";
        }

        #endregion

    }

}
