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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

using WWCP = org.GraphDefined.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class ChargeStationInfo : IEquatable<ChargeStationInfo>,
                                     IEnumerable<EVSE_Id>
    {

        #region Data

        private Regex MappedCharactersRegEx  = new Regex("[_/\\-]");
        private Regex InvalidCharactersRegEx = new Regex("[^A-Z0-9\\*]");

        private readonly List<EVSE_Id> _EVSEIds;

        #endregion

        #region Properties

        public ChargingPoolInfo      ChargePoolInfo    { get; }

        public String                StationXMLId      { get; }

        public ChargingStation_Id?   StationId         { get; internal set; }

        public IEnumerable<EVSE_Id>  EVSEIds
            => _EVSEIds;

        #endregion

        #region Constructor(s)

        public ChargeStationInfo(ChargingPoolInfo  ChargePoolInfo,
                                 String            StationXMLId,
                                 EVSE_Id           EVSEId)
        {

            this.ChargePoolInfo  = ChargePoolInfo ?? throw new ArgumentNullException(nameof(ChargePoolInfo),  "The given charging pool information must not be null!");
            this.StationXMLId    = StationXMLId   ?? "";
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

            if (StationId.HasValue)
                return;


            // 1st: Try to use the given ChargingStationId from the XML...
            if (ChargingStation_Id.TryParse(StationXMLId, out ChargingStation_Id stationId))
                StationId = stationId;

            else if (EVSE_Id.TryParse(StationXMLId, out EVSE_Id __EVSEId))
                StationId = ChargingStation_Id.Parse(WWCP.ChargingStation_Id.Parse(__EVSEId.OperatorId.ToWWCP().Value, __EVSEId.Suffix).ToString());


            else if (StationXMLId.StartsWith(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.DIN), StringComparison.Ordinal))
            {

                if (ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.DIN) +
                                                "S" +
                                                StationXMLId.Substring(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.DIN).Length + 1),

                                                out stationId))

                    StationId = stationId;

            }

            else if (StationXMLId.StartsWith(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO), StringComparison.Ordinal))
            {

                if (ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO) +
                                                "S" +
                                                StationXMLId.Substring(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO).Length + 1),

                                                out stationId))

                    StationId = stationId;

            }

            else if (StationXMLId.StartsWith(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO_STAR), StringComparison.Ordinal))
            {

                if (ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO_STAR) +
                                                "S" +
                                                StationXMLId.Substring(_EVSEIds[0].OperatorId.ToWWCP().Value.ToString(WWCP.OperatorIdFormats.ISO_STAR).Length + 2),

                                                out stationId))

                    StationId = stationId;

            }


            //// 2nd:  Try to use a modified StationXML Id...
            //if (!StationId.HasValue && StationXMLId.IsNotNullOrEmpty())
            //    StationId = ChargingStation_Id.TryParse(_EVSEIds[0].OperatorId.ToWWCP().Value,
            //                                            InvalidCharactersRegEx.Replace(MappedCharactersRegEx.Replace(StationXMLId.ToUpper(), "*"), "").SubstringMax(50));


            //// 3rd: Try to use the given EVSE Ids to find a common prefix...
            //if (!StationId.HasValue && StationXMLId.IsNullOrEmpty())
            //    StationId = ChargingStation_Id.Create(_EVSEIds.Select(evse => evse.ToWWCP().Value));


            if (!StationId.HasValue)
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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "'" + StationXMLId + "' / '" + StationId + "' => " + _EVSEIds.Count + " EVSEs";
        }

        #endregion

    }

}
