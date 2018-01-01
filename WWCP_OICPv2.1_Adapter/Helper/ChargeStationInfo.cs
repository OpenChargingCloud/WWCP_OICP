/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

        #region Data

        private Regex MappedCharactersRegEx  = new Regex("[_/\\-]");
        private Regex InvalidCharactersRegEx = new Regex("[^A-Z0-9\\*]");

        #endregion

        #region Properties

        public ChargingPoolInfo  ChargePoolInfo   { get; }

        public String            StationXMLId     { get; }

        #region StationId

        private WWCP.ChargingStation_Id? _StationId;

        public WWCP.ChargingStation_Id StationId
        {

            get
            {
                return _StationId.Value;
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

            if (_StationId.HasValue)
                return;

            EVSE_Id            __EVSEId;
            WWCP.ChargingStation_Id __StationId;


            // 1st: Try to use the given ChargingStationId from the XML...
            if (WWCP.ChargingStation_Id.TryParse(StationXMLId, out __StationId))
                _StationId = __StationId;

            else if (EVSE_Id.TryParse(StationXMLId, out __EVSEId))
                _StationId = WWCP.ChargingStation_Id.Parse(WWCP.ChargingStation_Id.Parse(__EVSEId.OperatorId.ToWWCP().Value, __EVSEId.Suffix).ToString());


            else if (StationXMLId.StartsWith(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.DIN),      StringComparison.Ordinal))
            {

                if (WWCP.ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.DIN) +
                                                     "S" +
                                                     StationXMLId.Substring(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.DIN).Length + 1),

                                                     out __StationId))

                    _StationId = __StationId;

            }

            else if (StationXMLId.StartsWith(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO),      StringComparison.Ordinal))
            {

                if (WWCP.ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO) +
                                                     "S" +
                                                     StationXMLId.Substring(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO).Length + 1),

                                                     out __StationId))

                    _StationId = __StationId;

            }

            else if (StationXMLId.StartsWith(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO_STAR), StringComparison.Ordinal))
            {

                if (WWCP.ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO_STAR) +
                                                     "S" +
                                                     StationXMLId.Substring(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO_STAR).Length + 2),

                                                     out __StationId))

                    _StationId = __StationId;

            }


            // 2nd:  Try to use a modified StationXML Id...
            if (!_StationId.HasValue && StationXMLId.IsNotNullOrEmpty())
                _StationId = WWCP.ChargingStation_Id.Parse(_EVSEIds[0].OperatorId.ToWWCP().Value,
                                                           InvalidCharactersRegEx.Replace(MappedCharactersRegEx.Replace(StationXMLId.ToUpper(), "*"), "").SubstringMax(50));


            // 3rd: Try to use the given EVSE Ids to find a common prefix...
            if (!_StationId.HasValue && StationXMLId.IsNullOrEmpty())
                _StationId = WWCP.ChargingStation_Id.Create(_EVSEIds.Select(evse => evse.ToWWCP().Value));


            if (!_StationId.HasValue)
            {

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
