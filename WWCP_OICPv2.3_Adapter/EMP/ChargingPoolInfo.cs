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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class ChargingPoolInfo : IEquatable<ChargingPoolInfo>,
                                    IEnumerable<ChargingStationInfo>
    {

        #region Data

        private readonly Dictionary<ChargingStation_Id, ChargingStationInfo> chargingStations;

        #endregion

        #region Properties

        public OperatorInfo                      OperatorInfo      { get; internal set; }

        public ChargingPool_Id                   PoolId            { get; }

        public Address?                          Address           { get; }

        public GeoCoordinates?                   GeoCoordinates    { get; }

        public IEnumerable<ChargingStationInfo>  ChargingStations
            => chargingStations.Values;

        #endregion

        #region Constructor(s)

        public ChargingPoolInfo(OperatorInfo                       OperatorInfo,
                                ChargingPool_Id                    PoolId,
                                Address?                           Address                = null,
                                GeoCoordinates?                    GeoCoordinates         = null,
                                IEnumerable<ChargingStationInfo>?  ChargingStationInfos   = null)
        {

            if (PoolId.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(PoolId), "The given pool identification must not be null or empty!");

            this.OperatorInfo       = OperatorInfo   ?? throw new ArgumentNullException(nameof(OperatorInfo),    "The given OperatorInfo must not be null!");
            this.PoolId             = PoolId;
            this.Address            = Address        ?? throw new ArgumentNullException(nameof(Address),         "The given address must not be null!");
            this.GeoCoordinates     = GeoCoordinates ?? throw new ArgumentNullException(nameof(GeoCoordinates),  "The given geo coordinates must not be null!");

            this.chargingStations  = ChargingStationInfos != null
                                          ? ChargingStationInfos.ToDictionary(station => station.StationId)
                                          : new Dictionary<ChargingStation_Id, ChargingStationInfo>();

        }

        #endregion


        #region AddOrUpdateEVSE(EVSE)

        public void AddOrUpdateEVSE(EVSEDataRecord EVSE)
        {

            var chargingStationId = EVSE.ChargingStationId
                                        ?? ChargingStation_Id.Generate(EVSE.OperatorId,
                                                                       EVSE.Address,
                                                                       EVSE.GeoCoordinates,
                                                                       EVSE.SubOperatorName,
                                                                       EVSE.ChargingStationName);

            if (!chargingStations.TryGetValue(chargingStationId, out var chargingStationInfo))
            {

                chargingStationInfo = new ChargingStationInfo(this,
                                                              chargingStationId,
                                                              EVSE.Address,
                                                              EVSE.GeoCoordinates);

                chargingStations.Add(chargingStationInfo.StationId,
                                     chargingStationInfo);

            }

            chargingStationInfo.AddOrUpdateEVSE(EVSE);

        }

        #endregion


        #region (static) TryParse(JSON, out ChargingPoolInfo, out ErrorResponse)

        /// <summary>
        /// Try to parse the given JSON representation of an operator info.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingPoolInfo">The parsed charging pool info.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                JSON,
                                       out ChargingPoolInfo?  ChargingPoolInfo,
                                       out String?            ErrorResponse)
        {

            try
            {

                ChargingPoolInfo = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse PoolId              [mandatory]

                if (!JSON.ParseMandatory("poolId",
                                         "charging pool identification",
                                         ChargingPool_Id.TryParse,
                                         out ChargingPool_Id PoolId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Address             [optional]

                if (JSON.ParseOptionalJSON("address",
                                           "charging pool address",
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
                                           "charging pool geo coordinates",
                                           OICPv2_3.GeoCoordinates.TryParse,
                                           out GeoCoordinates? GeoCoordinates,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingStations    [optional]

                if (JSON.ParseOptionalJSON("chargingStations",
                                           "charging stations",
                                           ChargingStationInfo.TryParse,
                                           out IEnumerable<ChargingStationInfo> ChargingStations,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ChargingPoolInfo = new ChargingPoolInfo(null,
                                                        PoolId,
                                                        Address,
                                                        GeoCoordinates,
                                                        null);

                return true;

            }
            catch (Exception e)
            {
                ChargingPoolInfo  = default;
                ErrorResponse     = "The given JSON representation of a charging pool info is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON()

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>

        public JObject ToJSON()

            => JSONObject.Create(

                         new JProperty("poolId",            PoolId.              ToString()),

                   Address != null
                       ? new JProperty("address",           Address.             ToJSON())
                       : null,

                   GeoCoordinates.HasValue
                       ? new JProperty("geoCoordinates",    GeoCoordinates.Value.ToJSON())
                       : null,

                   ChargingStations.SafeAny()
                       ? new JProperty("chargingStations",  JSONArray.Create(ChargingStations.Select(chargingStation => chargingStation.ToJSON())))
                       : null

               );

        #endregion


        #region IEnumerable<ChargeStationInfo> Members

        public IEnumerator<ChargingStationInfo> GetEnumerator()
            => chargingStations.Values.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => chargingStations.Values.GetEnumerator();

        #endregion

        #region IEquatable<ChargingPoolInfo> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolInfo chargingPoolInfo &&
                   Equals(chargingPoolInfo);

        #endregion

        #region Equals(ChargingPoolInfo)

        /// <summary>
        /// Compares two ChargingPoolInfos for equality.
        /// </summary>
        /// <param name="ChargingPoolInfo">A ChargingPoolInfo to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolInfo? ChargingPoolInfo)

            => ChargingPoolInfo is not null &&
               PoolId.Equals(ChargingPoolInfo.PoolId);

        #endregion

        #endregion

        #region (override) GetHashCode()

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
                             chargingStations.Count,
                             " charging stations, ",
                             chargingStations.SelectMany(v => v.Value.EVSEDataRecords).Count(),
                             " EVSEs");

        #endregion

    }

}
