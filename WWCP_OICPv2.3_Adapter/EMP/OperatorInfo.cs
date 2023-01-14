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

using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class OperatorInfo : IEnumerable<ChargingPoolInfo>
    {

        #region Data

        private static Regex InvalidCharactersRegEx = new Regex("[^A-Z0-9]");

        #endregion

        #region Properties

        /// <summary>
        /// The operator identification.
        /// </summary>
        public Operator_Id  OperatorId      { get; }

        /// <summary>
        /// The operator name.
        /// </summary>
        public String?      OperatorName    { get; }


        private readonly Dictionary<ChargingPool_Id, ChargingPoolInfo> chargingPools;

        /// <summary>
        /// All charging pools.
        /// </summary>
        public IEnumerable<ChargingPoolInfo> ChargingPools
            => chargingPools.Select(v => v.Value);

        #endregion

        #region Constructor(s)

        public OperatorInfo(Operator_Id                     OperatorId,
                            String?                         OperatorName    = null,
                            IEnumerable<ChargingPoolInfo>?  ChargingPools   = null)
        {

            if (OperatorId.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            this.OperatorId     = OperatorId;
            this.OperatorName   = OperatorName;
            this.chargingPools  = ChargingPools is not null
                                      ? ChargingPools.ToDictionary(pool => pool.PoolId)
                                      : new Dictionary<ChargingPool_Id, ChargingPoolInfo>();

        }

        #endregion


        #region AddOrUpdateEVSE(EVSE)

        public void AddOrUpdateEVSE(EVSEDataRecord EVSE)
        {

            var chargingPoolId = EVSE.ChargingPoolId
                                     ?? ChargingPool_Id.Generate(OperatorId,
                                                                 EVSE.Address,
                                                                 EVSE.GeoCoordinates,
                                                                 EVSE.SubOperatorName);

            if (!chargingPools.TryGetValue(chargingPoolId, out var chargingPoolInfo))
            {

                chargingPoolInfo = new ChargingPoolInfo(this,
                                                        chargingPoolId,
                                                        EVSE.Address,
                                                        EVSE.GeoCoordinates);

                chargingPools.Add(chargingPoolInfo.PoolId,
                                  chargingPoolInfo);

            }

            chargingPoolInfo.AddOrUpdateEVSE(EVSE);

        }

        #endregion

        #region VerifyUniquenessOfChargingStationIds()

        //public EVSEIdLookup VerifyUniquenessOfChargingStationIds()
        //{

        //    //_ChargingPools.ForEach(cp => cp.Value.Check());

        //    // ChargingStationIds are not unique!
        //    var DuplicateChargingStationIds = _ChargingPools.Values.
        //                                           SelectMany(cpinfo   => cpinfo.ChargingStations).
        //                                           ToLookup  (csinfo   => csinfo.StationId,
        //                                                      csinfo   => csinfo.ChargePoolInfo.PoolId).
        //                                           Where     (grouping => grouping.Count() > 1).
        //                                           ToArray();

        //    if (DuplicateChargingStationIds.Length > 0)
        //    {

        //        // ChargingStationXMLIds are not unique!
        //        var DuplicateChargingStationXMLIds = _ChargingPools.Values.
        //                                                  SelectMany(cpinfo   => cpinfo.ChargingStations).
        //                                                  ToLookup  (csinfo   => csinfo.StationId,
        //                                                             csinfo   => csinfo.ChargePoolInfo.PoolId).
        //                                                  Where     (grouping => grouping.Count() > 1).
        //                                                  ToArray();

        //        #region ChargingStationXMLIds are not unique...

        //        //if (DuplicateChargingStationXMLIds.Length == 0)
        //        //{
        //        //    _ChargingPools.Values.
        //        //         SelectMany(cpinfo => cpinfo.ChargingStations).
        //        //         ForEach   (csinfo => {

        //        //             csinfo.StationId = WWCP.ChargingStation_Id.Parse(csinfo.ChargePoolInfo.PoolId.OperatorId,
        //        //                                                              // Replace invalid characters BEFORE grouping!
        //        //                                                              InvalidCharactersRegEx.Replace(csinfo.StationXMLId.ToUpper(), "").Substring(30));

        //        //         });
        //        //}

        //        #endregion

        //        #region ...or they are!

        //        //else
        //        //    _ChargingPools.Values.
        //        //         SelectMany(cpinfo => cpinfo.ChargingStations).
        //        //         ForEach   (csinfo => {

        //        //             csinfo.StationId = WWCP.ChargingStation_Id.Parse(csinfo.ChargePoolInfo.PoolId.OperatorId,
        //        //                                                              new SHA1CryptoServiceProvider().
        //        //                                                                  ComputeHash(Encoding.UTF8.GetBytes(csinfo.Select(evseid => evseid.ToString()).Aggregate())).
        //        //                                                                              ToHexString().
        //        //                                                                              SubstringMax(50).
        //        //                                                                              ToUpper());

        //        //          });

        //        #endregion

        //    }


        //    #region Make sure no StationId is null!

        //    //_ChargingPools.Values.
        //    //    SelectMany(cpinfo => cpinfo.ChargingStations).
        //    //    ForEach   (csinfo => {
        //    //        if (csinfo.StationId == null)
        //    //        {

        //    //            csinfo.StationId = ChargingStation_Id.Parse(csinfo.ChargePoolInfo.OperatorInfo.OperatorId,
        //    //                                                        new SHA1CryptoServiceProvider().
        //    //                                                            ComputeHash(Encoding.UTF8.GetBytes(csinfo.Select(evseid => evseid.ToString()).Aggregate())).
        //    //                                                                        ToHexString().
        //    //                                                                        Substring(0, 12).
        //    //                                                                        ToUpper());

        //    //        }
        //    //    });

        //    #endregion

        //    #region Check if all ChargingStationIds are unique

        //    var CS3 = _ChargingPools.Values.
        //                  SelectMany(cpinfo   => cpinfo.ChargingStations).
        //                  ToLookup  (csinfo   => csinfo.StationId,
        //                             csinfo   => csinfo.ChargePoolInfo.PoolId).
        //                  Where     (grouping => grouping.Count() > 1);

        //    if (CS3.Any())
        //    {
        //        DebugX.Log("Still not unique!");
        //    }

        //    #endregion

        //    return new EVSEIdLookup(OperatorId, this);

        //}

        #endregion


        #region (static) TryParse(JSON, out EVSEDataRecord, out ErrorResponse)

        /// <summary>
        /// Try to parse the given JSON representation of an operator info.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorInfo">The parsed operator info.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject            JSON,
                                       out OperatorInfo?  OperatorInfo,
                                       out String?        ErrorResponse)
        {

            try
            {

                OperatorInfo = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse OperatorId      [mandatory]

                if (!JSON.ParseMandatory("operatorId",
                                         "operator identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorName    [optional]

                if (JSON.ParseOptional("operatorName",
                                       "operator name",
                                       out String OperatorName,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingPools   [optional]

                if (JSON.ParseOptionalJSON("chargingPools",
                                           "charging pools",
                                           ChargingPoolInfo.TryParse,
                                           out IEnumerable<ChargingPoolInfo> ChargingPools,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                OperatorInfo = new OperatorInfo(OperatorId,
                                                OperatorName,
                                                ChargingPools);

                return true;

            }
            catch (Exception e)
            {
                OperatorInfo   = default;
                ErrorResponse  = "The given JSON representation of an operator info is invalid: " + e.Message;
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
                   new JProperty("operatorId",     OperatorId.ToString()),
                   new JProperty("operatorName",   OperatorName),
                   new JProperty("chargingPools",  JSONArray.Create(
                       ChargingPools.Select(chargingPool => chargingPool.ToJSON()))
                   )
               );

        #endregion


        #region IEnumerable<ChargingPoolInfo> Members

        public IEnumerator<ChargingPoolInfo> GetEnumerator()
            => chargingPools.Values.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => chargingPools.Values.GetEnumerator();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorName, " (", OperatorId.ToString(), ") ", " => ",
                             chargingPools.Values.                                                                                           Count(), " charging pools, ",
                             chargingPools.Values.SelectMany(pool => pool.ChargingStations).                                                 Count(), " charging stations, ",
                             chargingPools.Values.SelectMany(pool => pool.ChargingStations.SelectMany(stations => stations.EVSEDataRecords)).Count(), " EVSEs.");

        #endregion

    }

}
