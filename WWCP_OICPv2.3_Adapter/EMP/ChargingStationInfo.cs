/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class ChargingStationInfo : IEquatable<ChargingStationInfo>,
                                       IEnumerable<EVSEDataRecord>
    {

        #region Data

        private Regex MappedCharactersRegEx  = new Regex("[_/\\-]");
        private Regex InvalidCharactersRegEx = new Regex("[^A-Z0-9\\*]");

        private readonly Dictionary<EVSE_Id, EVSEDataRecord> EVSEs;

        #endregion

        #region Properties

        public ChargingPoolInfo             ChargePoolInfo    { get; }

        public ChargingStation_Id           StationId         { get; }

        public Address                      Address           { get; }

        public GeoCoordinates?              GeoCoordinates    { get; }

        public IEnumerable<EVSEDataRecord>  EVSEDataRecords
            => EVSEs.Values;

        #endregion

        #region Constructor(s)

        public ChargingStationInfo(ChargingPoolInfo              ChargePoolInfo,
                                   ChargingStation_Id            StationId,
                                   Address?                      Address          = null,
                                   GeoCoordinates?               GeoCoordinates   = null,
                                   IEnumerable<EVSEDataRecord>?  EVSEs            = null)
        {

            this.ChargePoolInfo  = ChargePoolInfo ?? throw new ArgumentNullException(nameof(ChargePoolInfo),  "The given charging pool information must not be null!");
            this.StationId       = StationId;
            this.Address         = Address        ?? throw new ArgumentNullException(nameof(Address),         "The given address must not be null!");
            this.GeoCoordinates  = GeoCoordinates ?? throw new ArgumentNullException(nameof(GeoCoordinates),  "The given geo coordinates must not be null!");

            this.EVSEs           = EVSEs is not null
                                       ? EVSEs.ToDictionary(evse => evse.Id)
                                       : new Dictionary<EVSE_Id, EVSEDataRecord>();

        }

        #endregion


        #region AddOrUpdateEVSE(EVSE)

        public void AddOrUpdateEVSE(EVSEDataRecord EVSE)
        {

            if (!EVSEs.ContainsKey(EVSE.Id))
                EVSEs.Add(EVSE.Id, EVSE);

            else
                DebugX.Log("Duplicate EVSE identification: '" + EVSE.Id + "'!");

        }

        #endregion


        #region (static) TryParse(JSON, out ChargingStationInfo, out ErrorResponse)

        /// <summary>
        /// Try to parse the given JSON representation of an operator info.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingStationInfo">The parsed charging station info.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                   JSON,
                                       out ChargingStationInfo?  ChargingStationInfo,
                                       out String?               ErrorResponse)
        {

            try
            {

                ChargingStationInfo = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse StationId           [mandatory]

                if (!JSON.ParseMandatory("stationId",
                                         "charging station identification",
                                         ChargingStation_Id.TryParse,
                                         out ChargingStation_Id StationId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Address             [optional]

                if (JSON.ParseOptionalJSON("address",
                                           "charging station address",
                                           OICPv2_3.Address.TryParse,
                                           out Address Address,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse GeoCoordinates      [optional]

                if (JSON.ParseOptionalJSON("geoCoordinates",
                                           "charging station geo coordinates",
                                           OICPv2_3.GeoCoordinates.TryParse,
                                           out GeoCoordinates? GeoCoordinates,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EVSEDataRecords     [optional]

                if (JSON.ParseOptionalJSON("evses",
                                           "evses",
                                           EVSEDataRecord.TryParse,
                                           out IEnumerable<EVSEDataRecord> EVSEDataRecords,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ChargingStationInfo = new ChargingStationInfo(null,
                                                              StationId,
                                                              Address,
                                                              GeoCoordinates,
                                                              EVSEDataRecords);

                return true;

            }
            catch (Exception e)
            {
                ChargingStationInfo  = default;
                ErrorResponse        = "The given JSON representation of a charging station info is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON()

        public JObject ToJSON()

            => JSONObject.Create(

                   new JProperty("stationId",             StationId.           ToString()),
                   new JProperty("address",               Address.             ToJSON()),

                   GeoCoordinates is not null
                       ? new JProperty("geoCoordinates",  GeoCoordinates.Value.ToJSON())
                       : null,

                   new JProperty("evses",                 JSONArray.Create(
                                                              EVSEDataRecords.Select(evseDataRecord => evseDataRecord.ToJSON()))
                                                          )

               );

        #endregion


        #region IEnumerable<EVSEDataRecord> Members

        public IEnumerator<EVSEDataRecord> GetEnumerator()
            => EVSEs.Values.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => EVSEs.Values.GetEnumerator();

        #endregion

        #region IEquatable<ChargingStationInfo> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationInfo chargingStationInfo &&
                   Equals(chargingStationInfo);

        #endregion

        #region Equals(ChargingStationInfo)

        /// <summary>
        /// Compares two ChargingStationInfos for equality.
        /// </summary>
        /// <param name="ChargingStationInfo">A ChargingStationInfo to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationInfo? ChargingStationInfo)

            => ChargingStationInfo is not null &&
               StationId.Equals(ChargingStationInfo.StationId);

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

                return StationId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat("'", StationId, "' / '", StationId, "' => ", EVSEs.Count, " EVSEs");

        #endregion

    }

}
