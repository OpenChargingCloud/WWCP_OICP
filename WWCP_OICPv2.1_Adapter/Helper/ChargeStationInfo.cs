﻿/*
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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class ChargeStationInfo : IEquatable<ChargeStationInfo>,
                                     IEnumerable<EVSE_Id>
    {

        #region Properties

        public ChargingPoolInfo  ChargePoolInfo   { get; }

        public String            StationXMLId     { get; }

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
            => _EVSEIds;

        #endregion

        #endregion

        #region Constructor(s)

        public ChargeStationInfo(ChargingPoolInfo  ChargePoolInfo,
                                 String            StationXMLId,
                                 EVSE_Id           EVSEId)
        {

            #region Initial checks

            if (ChargePoolInfo == null)
                throw new ArgumentNullException(nameof(ChargePoolInfo),  "The given charging pool information must not be null!");

            #endregion

            this.ChargePoolInfo  = ChargePoolInfo;
            this.StationXMLId    = StationXMLId ?? "";
            this._EVSEIds        = new List<EVSE_Id> { EVSEId };

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

            ChargingStation_Id? __StationId = null;

            // 1st: Try to use the given ChargingStationId from the XML...
            if (StationXMLId.StartsWith(ChargePoolInfo.CPInfoList.OperatorId.ToString(WWCP.OperatorIdFormats.DIN), StringComparison.Ordinal) ||
                StationXMLId.StartsWith(ChargePoolInfo.CPInfoList.OperatorId.ToString(WWCP.OperatorIdFormats.ISO), StringComparison.Ordinal))
            {
                ChargingStation_Id ___StationId;
                if (ChargingStation_Id.TryParse(StationXMLId, out ___StationId))
                    __StationId = ___StationId;
            }

            // 2nd: Try to use the given EVSE Ids to find a common prefix...
            if (__StationId == null && StationXMLId.IsNullOrEmpty())
            {
                var CSId = ChargingStation_Id.Create(_EVSEIds.Select(evse => evse.ToWWCP().Value));
                if (CSId.HasValue)
                    __StationId = CSId.Value;
            }

            // Alternative: Try to use a modified StationXML Id...
            if (__StationId == null && StationXMLId.IsNotNullOrEmpty())
            {
                var rgx = new Regex("[^A-Z0-9]");
                __StationId = ChargingStation_Id.Parse(ChargePoolInfo.PoolId.OperatorId, rgx.Replace(StationXMLId.ToUpper(), "").SubstringMax(30));
            }

            if (!__StationId.HasValue)
            {

            }

            _StationId = __StationId.Value;

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

            return ChargeStationInfo.StationXMLId.Equals(StationXMLId);

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

                return StationXMLId.GetHashCode();

            }
        }

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
