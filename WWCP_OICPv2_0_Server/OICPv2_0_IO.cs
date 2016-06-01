/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Aegir;

using org.GraphDefined.WWCP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// OICP v2.0 JSON/XML I/O.
    /// </summary>
    public static class OICPv2_0_IO
    {

        #region ParseChargingPool(this HTTPRequest, DefaultServerName, RoamingNetwork, out ChargingPool, out HTTPResponse)

        public static Boolean ParseChargingPool(this HTTPRequest  HTTPRequest,
                                                String            DefaultServerName,
                                                RoamingNetwork    RoamingNetwork,
                                                out ChargingPool  ChargingPool,
                                                out HTTPResponse  HTTPResponse)
        {

            ChargingPool_Id ChargingPoolId  = null;
                            ChargingPool    = null;
                            HTTPResponse    = null;

            if (HTTPRequest.ParsedURIParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now
                };

                return false;

            }

            if (!ChargingPool_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out ChargingPoolId))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingPoolId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingPoolId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseChargingStation(this HTTPRequest, DefaultServerName, RoamingNetwork, out ChargingStation, out HTTPResponse)

        public static Boolean ParseChargingStation(this HTTPRequest     HTTPRequest,
                                                   String               DefaultServerName,
                                                   RoamingNetwork       RoamingNetwork,
                                                   out ChargingStation  ChargingStation,
                                                   out HTTPResponse     HTTPResponse)
        {

            ChargingStation_Id ChargingStationId  = null;
                               ChargingStation    = null;
                               HTTPResponse       = null;

            if (HTTPRequest.ParsedURIParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now
                };

                return false;

            }

            if (!ChargingStation_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out ChargingStationId))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingStationId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetChargingStationbyId(ChargingStationId, out ChargingStation))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingStationId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseEVSE(this HTTPRequest, DefaultServerName, RoamingNetwork, out EVSE, out HTTPResponse)

        public static Boolean ParseEVSE(this HTTPRequest  HTTPRequest,
                                        String            DefaultServerName,
                                        RoamingNetwork    RoamingNetwork,
                                        out EVSE          EVSE,
                                        out HTTPResponse  HTTPResponse)
        {

            EVSE_Id EVSEId        = null;
                    EVSE          = null;
                    HTTPResponse  = null;

            if (HTTPRequest.ParsedURIParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now
                };

                return false;

            }

            if (!EVSE_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out EVSEId))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid EVSEId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetEVSEbyId(EVSEId, out EVSE))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown EVSEId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion

        #region ParseChargingReservation(this HTTPRequest, DefaultServerName, RoamingNetwork, out ChargingReservation, out HTTPResponse)

        public static Boolean ParseChargingReservation(this HTTPRequest         HTTPRequest,
                                                       String                   DefaultServerName,
                                                       RoamingNetwork           RoamingNetwork,
                                                       out ChargingReservation  ChargingReservation,
                                                       out HTTPResponse         HTTPResponse)
        {

            ChargingReservation_Id ChargingReservationId  = null;
                                   ChargingReservation    = null;
                                   HTTPResponse           = null;

            if (HTTPRequest.ParsedURIParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now
                };

                return false;

            }

            if (!ChargingReservation_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out ChargingReservationId))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid ChargingReservationId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            //ToDo: May fail for empty sequences!
            if (!RoamingNetwork.TryGetReservationById(ChargingReservationId, out ChargingReservation))
            {

                HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown ChargingReservationId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion


        // ------------------------------------------------


        #region ToJSON(this IId, JPropertyKey)

        /// <summary>
        /// Create a JSON representation of the given object..
        /// </summary>
        /// <param name="Object">An object.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        public static JProperty ToJSON(this IId Id, String JPropertyKey)
        {

            return (Id != null)
                       ? new JProperty(JPropertyKey, Id.ToString())
                       : null;

        }

        #endregion

        #region ToJSON(this Location, JPropertyKey)

        /// <summary>
        /// Create a JSON representation of the given GeoLocation.
        /// </summary>
        /// <param name="Location">A GeoLocation.</param>
        /// <param name="JPropertyKey">The name of the JSON property key.</param>
        public static JProperty ToJSON(this GeoCoordinate Location, String JPropertyKey)
        {

            if (Location == null)
                return null;

            if (Location.Longitude.Value == 0 && Location.Latitude.Value == 0)
                return null;

            return new JProperty(JPropertyKey,
                                 org.GraphDefined.Vanaheimr.Hermod.JSONObject.Create(
                                     Location.Projection != GravitationalModel.WGS84 ? new JProperty("projection", Location.Projection.ToString()) : null,
                                     new JProperty("lat", Location.Latitude. Value),
                                     new JProperty("lng", Location.Longitude.Value),
                                     Location.Altitude.Value != 0.0                  ? new JProperty("altitude",   Location.Altitude.Value)        : null)
                                );

        }

        #endregion

        #region ToJSON(this OpeningTimes)

        public static JObject ToJSON(this OpeningTimes OpeningTimes)
        {

            return (OpeningTimes != null)
                       ? OpeningTimes.IsOpen24Hours
                             ? new JObject()
                             : new JObject(new JProperty("Text", OpeningTimes.FreeText))
                       : null;

        }

        #endregion

        #region ToJSON(this OpeningTimes, JPropertyKey)

        public static JProperty ToJSON(this OpeningTimes OpeningTimes, String JPropertyKey)
        {

            return (OpeningTimes != null)
                       ? OpeningTimes.IsOpen24Hours
                             ? new JProperty(JPropertyKey,  "24/7")
                             : new JProperty(JPropertyKey,  OpeningTimes.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this GridConnection, JPropertyKey)

        public static JProperty ToJSON(this GridConnection GridConnection, String JPropertyKey)
        {

            return (GridConnection != GridConnection.Unknown)
                       ? new JProperty(JPropertyKey,
                                       GridConnection.ToString())
                       : null;

        }

        #endregion

        #region ToJSON(this ChargingStationUIFeatures, JPropertyKey)

        public static JProperty ToJSON(this ChargingStationUIFeatures ChargingStationUIFeatures, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 ChargingStationUIFeatures.ToString());

        }

        #endregion

        #region ToJSON(this AuthenticationModes, JPropertyKey)

        public static JProperty ToJSON(this ReactiveSet<AuthenticationMode> AuthenticationModes, String JPropertyKey)
        {

            return new JProperty(JPropertyKey,
                                 new JArray(AuthenticationModes.SafeSelect(mode => mode.ToString())));

        }

        #endregion

        #region ToJSON(this Text, JPropertyKey)

        public static JProperty ToJSON(this String Text, String JPropertyKey)
        {

            return (Text.IsNotNullOrEmpty())
                        ? new JProperty(JPropertyKey, Text)
                        : null;

        }

        #endregion


        // -----

        #region ToJSON(this Address)

        public static JObject ToJSON(this Address _Address)
        {

            return (_Address != null)
                       ? org.GraphDefined.Vanaheimr.Hermod.JSONObject.Create(_Address.FloorLevel         .ToJSON("floorLevel"),
                                           _Address.HouseNumber        .ToJSON("houseNumber"),
                                           _Address.Street             .ToJSON("street"),
                                           _Address.PostalCode         .ToJSON("postalCode"),
                                           _Address.PostalCodeSub      .ToJSON("postalCodeSub"),
                                           _Address.City               .ToJSON("city"),
                                           (_Address.Country != null)
                                                ? _Address.Country.CountryName.ToJSON("country")
                                                : null)
                       : null;

        }

        #endregion

        #region ToJSON(this Address, JPropertyKey)

        public static JProperty ToJSON(this Address Address, String JPropertyKey)
        {

            return (Address != null)
                       ? new JProperty(JPropertyKey,
                                       Address.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this Addresses, JPropertyKey)

        public static JArray ToJSON(this IEnumerable<Address> Addresses)
        {

            return (Addresses != null && Addresses.Any())
                        ? new JArray(Addresses.SafeSelect(v => v.ToJSON()))
                        : null;

        }

        #endregion

        #region ToJSON(this Addresses, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<Address> Addresses, String JPropertyKey)
        {

            return (Addresses != null)
                        ? new JProperty(JPropertyKey,
                                        Addresses.ToJSON())
                        : null;

        }

        #endregion


        // -----


        #region ToJSON(this ChargingPoolStatus, Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusType>>>>  ChargingPoolStatus,
                                     UInt64                                                                                             Skip  = 0,
                                     UInt64                                                                                             Take  = 0)
        {

            if (ChargingPoolStatus == null)
                return new JObject();

            var _ChargingPoolStatus = Take == 0
                                          ? ChargingPoolStatus.Skip(Skip)
                                          : ChargingPoolStatus.Skip(Skip).Take(Take);

            return new JObject(_ChargingPoolStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple charging pool status having the exact same ISO 8601 timestamp!
                                                                            GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                            Select           (group => group.First()).

                                                                            OrderByDescending(tsv   => tsv.Timestamp).
                                                                            Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                     tsv.Value.    ToString())))

                                                            )));

        }

        #endregion

        #region ToJSON(this ChargingStationStatus, Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusType>>>>  ChargingStationStatus,
                                     UInt64                                                                                                   Skip  = 0,
                                     UInt64                                                                                                   Take  = 0)
        {

            if (ChargingStationStatus == null)
                return new JObject();

            var _ChargingStationStatus = Take == 0
                                             ? ChargingStationStatus.Skip(Skip)
                                             : ChargingStationStatus.Skip(Skip).Take(Take);

            return new JObject(_ChargingStationStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                            GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                            Select           (group => group.First()).

                                                                            OrderByDescending(tsv   => tsv.Timestamp).
                                                                            Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                     tsv.Value.    ToString())))

                                                            )));

        }

        #endregion

        #region ToJSON(this EVSEStatus, Skip = 0, Take = 0)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>>  EVSEStatus,
                                     UInt64                                                                             Skip  = 0,
                                     UInt64                                                                             Take  = 0)
        {

            if (EVSEStatus == null)
                return new JObject();

            var _EVSEStatus = Take == 0
                                  ? EVSEStatus.Skip(Skip).           ToArray()
                                  : EVSEStatus.Skip(Skip).Take(Take).ToArray();

            if (_EVSEStatus.Length == 0)
                return new JObject();

            return new JObject(_EVSEStatus.
                                   SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                new JObject(statuslist.Value.

                                                                            // Will filter multiple evse status having the exact same ISO 8601 timestamp!
                                                                            GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                            Select           (group => group.First()).

                                                                            OrderByDescending(tsv   => tsv.Timestamp).
                                                                            Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                     tsv.Value.    ToString())))

                                                            )));

        }

        #endregion


        #region ToJSON(this ChargingPool, Embedded = false, ExpandChargingStations = true)

        public static JObject ToJSON(this ChargingPool  ChargingPool,
                                     Boolean            Embedded                = false,
                                     Boolean            ExpandChargingStations  = true)
        {

            return org.GraphDefined.Vanaheimr.Hermod.JSONObject.Create(ChargingPool.Id.ToJSON("ChargingPoolId"),
                               ChargingPool.Name.                   ToJSON("Name"),
                               ChargingPool.Description.            ToJSON("Description"),

                               new JProperty("OperatorId",  ChargingPool.Operator.Id.ToString()),

                                           ChargingPool.GeoLocation.ToJSON("GeoLocation"),
                                               ChargingPool.Address.ToJSON("Address"),
                                   ChargingPool.AuthenticationModes.ToJSON("AuthenticationModes"),
                                           ChargingPool.OpeningTimes.ToJSON("OpeningTimes"),

                               ExpandChargingStations
                                   ? new JProperty("ChargingStations",   new JArray(ChargingPool.ChargingStations.OrderBy(station   => station.Id).Select(station   =>   station.ToJSON(Embedded: false, ExpandEVSEs: true))))
                                   : new JProperty("ChargingStationIds", new JArray(ChargingPool.ChargingStationIds.OrderBy(stationId => stationId).Select(stationId => stationId.ToString())))

                           );

        }

        #endregion

        #region ToJSON(this ChargingPool, JPropertyKey)

        public static JObject ToJSON(this ChargingPool EVChargingPool, String JPropertyKey)
        {

            return new JObject(JPropertyKey,
                               EVChargingPool.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingPools, Skip = 0, Take = 0, Embedded = false, ExpandChargingStations = true)

        public static JArray ToJSON(this IEnumerable<ChargingPool>  ChargingPools,
                                    UInt64                          Skip                    = 0,
                                    UInt64                          Take                    = 0,
                                    Boolean                         Embedded                = false,
                                    Boolean                         ExpandChargingStations  = true)
        {

            if (ChargingPools == null)
                return new JArray();

            return Take == 0

                       ? new JArray(ChargingPools.
                                        Where(pool => pool != null).
                                        Skip(Skip).
                                        SafeSelect(pool => pool.ToJSON()))

                       : new JArray(ChargingPools.
                                        Where(pool => pool != null).
                                        Skip(Skip).
                                        Take(Take).
                                        SafeSelect(pool => pool.ToJSON()));

        }

        #endregion

        #region ToJSON(this ChargingPools, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingPool> ChargingPools, String JPropertyKey)
        {

            return (ChargingPools != null)
                        ? new JProperty(JPropertyKey,
                                        ChargingPools.ToJSON())
                        : new JProperty(JPropertyKey, new JArray());

        }

        #endregion


        #region ToJSON(this ChargingStation, Embedded = false, ExpandEVSEs = true)

        public static JObject ToJSON(this ChargingStation  ChargingStation,
                                     Boolean               Embedded     = false,
                                     Boolean               ExpandEVSEs  = true)
        {

            // Embedded means it is served as a substructure of e.g. a ChargingPool
            if (Embedded)
                return org.GraphDefined.Vanaheimr.Hermod.JSONObject.Create(ChargingStation.Id.ToJSON("ChargingStationId"),
                                   ChargingStation.Name.                    ToJSON("Name"),
                                   ChargingStation.Description.             ToJSON("Description"),
                                   ChargingStation.GeoLocation         != ChargingStation.ChargingPool.GeoLocation         ?         ChargingStation.GeoLocation.ToJSON("GeoLocation")         : null,
                                   ChargingStation.Address             != ChargingStation.ChargingPool.Address             ?             ChargingStation.Address.ToJSON("Address")             : null,
                                   ChargingStation.AuthenticationModes != ChargingStation.ChargingPool.AuthenticationModes ? ChargingStation.AuthenticationModes.ToJSON("AuthenticationModes") : null,
                                   ChargingStation.OpeningTimes         != ChargingStation.ChargingPool.OpeningTimes         ?         ChargingStation.OpeningTimes.ToJSON("OpeningTimes")        : null,

                                   ExpandEVSEs
                                       ? new JProperty("EVSEs",             new JArray(ChargingStation.EVSEs.OrderBy(evse   => evse.Id).Select(evse   =>   evse.ToJSON(Embedded: true))))
                                       : new JProperty("EVSEIds",           new JArray(ChargingStation.EVSEIds.OrderBy(evseid => evseid).Select(evseid => evseid.ToString())))
                                  );

            else
                return org.GraphDefined.Vanaheimr.Hermod.JSONObject.Create(ChargingStation.Id.ToJSON("ChargingStationId"),
                                   new JProperty("ChargingPoolId",          ChargingStation.ChargingPool.Id.ToString()),
                                   new JProperty("OperatorId",              ChargingStation.ChargingPool.Operator.Id.ToString()),

                                   ChargingStation.Name.                    ToJSON("Name"),
                                   ChargingStation.Description.             ToJSON("Description"),
                                                ChargingStation.GeoLocation.ToJSON("GeoLocation"),
                                                    ChargingStation.Address.ToJSON("Address"),
                                        ChargingStation.AuthenticationModes.ToJSON("AuthenticationModes"),
                                                ChargingStation.OpeningTimes.ToJSON("OpeningTimes"),

                                   ExpandEVSEs
                                       ? new JProperty("EVSEs",             new JArray(ChargingStation.EVSEs.OrderBy(evse   => evse.Id).Select(evse   =>   evse.ToJSON(Embedded: true))))
                                       : new JProperty("EVSEIds",           new JArray(ChargingStation.EVSEIds.OrderBy(evseid => evseid).Select(evseid => evseid.ToString())))
                                  );

        }

        #endregion

        #region ToJSON(this ChargingStation, JPropertyKey)

        public static JProperty ToJSON(this ChargingStation ChargingStation, String JPropertyKey)
        {

            return new JProperty(JPropertyKey, ChargingStation.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingStations, Skip = 0, Take = 0, Embedded = false, ExpandEVSEs = true)

        public static JArray ToJSON(this IEnumerable<ChargingStation>  ChargingStations,
                                    UInt64                             Skip         = 0,
                                    UInt64                             Take         = 0,
                                    Boolean                            Embedded     = false,
                                    Boolean                            ExpandEVSEs  = true)
        {

            if (ChargingStations == null)
                return new JArray();

            return Take == 0

                       ? new JArray(ChargingStations.
                                        Where     (station => station != null).
                                        Skip      (Skip).
                                        SafeSelect(station => station.ToJSON(false, ExpandEVSEs)))

                       : new JArray(ChargingStations.
                                        Where     (station => station != null).
                                        Skip      (Skip).
                                        Take      (Take).
                                        SafeSelect(station => station.ToJSON(false, ExpandEVSEs)));

        }

        #endregion

        #region ToJSON(this ChargingStations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingStation> ChargingStations, String JPropertyKey)
        {

            return (ChargingStations != null)
                        ? new JProperty(JPropertyKey,
                                        ChargingStations.ToJSON())
                        : new JProperty(JPropertyKey, new JArray());

        }

        #endregion


        #region ToJSON(this EVSE, Embedded = false, ExpandOperatorId = false)

        public static JObject ToJSON(this EVSE  EVSE,
                                     Boolean    Embedded          = false,
                                     Boolean    ExpandOperatorId  = false)
        {

            // Embedded means it is served as a substructure of e.g. a ChargingStation
            if (Embedded)
                return JSONObject.Create(
                           EVSE.Id.ToJSON("EVSEId"),
                           EVSE.Description.                         ToJSON("Description"),

                           EVSE.SocketOutlets.Count > 0 ? new JProperty("SocketOutlets",      new JArray(EVSE.SocketOutlets.ToJSON()))         : null,
        //                   EVSE.GuranteedMinPower   > 0 ? new JProperty("GuranteedMinPower",  EVSE.GuranteedMinPower)                          : null,
                           EVSE.MaxPower            > 0 ? new JProperty("MaxPower",           EVSE.MaxPower)                                   : null
                       );

            else
                return JSONObject.Create(
                           EVSE.Id.ToJSON("EVSEId"),
                           new JProperty("ChargingStationId",        EVSE.ChargingStation.             Id.ToString()),
                           new JProperty("ChargingPoolId",           EVSE.ChargingStation.ChargingPool.Id.ToString()),
                           ExpandOperatorId ? new JProperty("Operator",    new JObject(new JProperty("OperatorId", EVSE.Operator.Id.  ToString()),
                                                                                       new JProperty("Name",       EVSE.Operator.Name.ToJSON())))
                                            : new JProperty("OperatorId",  EVSE.Operator.Id.ToString()),

                           EVSE.Description.                         ToJSON("Description"),

                                    EVSE.ChargingStation.GeoLocation.ToJSON("GeoLocation"),
                                        EVSE.ChargingStation.Address.ToJSON("Address"),
                            EVSE.ChargingStation.AuthenticationModes.ToJSON("AuthenticationModes"),
                                   EVSE.ChargingStation.OpeningTimes.ToJSON("OpeningTimes"),

                           EVSE.SocketOutlets.Count > 0 ? new JProperty("SocketOutlets",      new JArray(EVSE.SocketOutlets.ToJSON()))         : null,
        //                   EVSE.GuranteedMinPower   > 0 ? new JProperty("GuranteedMinPower",  EVSE.GuranteedMinPower)                          : null,
                           EVSE.MaxPower            > 0 ? new JProperty("MaxPower",           EVSE.MaxPower)                                   : null
                       );

        }

        #endregion

        #region ToJSON(this EVSE, JPropertyKey)

        public static JProperty ToJSON(this EVSE EVSE, String JPropertyKey)
        {

            return new JProperty(JPropertyKey, EVSE.ToJSON());

        }

        #endregion

        #region ToJSON(this EVSEs, Skip = 0, Take = 0, Embedded = false)

        public static JArray ToJSON(this IEnumerable<EVSE>  EVSEs,
                                    UInt64                  Skip      = 0,
                                    UInt64                  Take      = 0,
                                    Boolean                 Embedded  = false)
        {

            if (EVSEs == null)
                return new JArray();

            return Take == 0

                       ? new JArray(EVSEs.
                                        Where     (evse => evse != null).
                                        Skip      (Skip).
                                        SafeSelect(evse => evse.ToJSON(Embedded)))

                       : new JArray(EVSEs.
                                        Where     (evse => evse != null).
                                        Skip      (Skip).
                                        Take      (Take).
                                        SafeSelect(evse => evse.ToJSON(Embedded)));

        }

        #endregion

        #region ToJSON(this EVSEs, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<EVSE> EVSEs, String JPropertyKey)
        {

            return (EVSEs != null)
                        ? new JProperty(JPropertyKey,
                                        EVSEs.ToJSON())
                        : new JProperty(JPropertyKey, new JArray());

        }

        #endregion


        #region ToJSON(this SocketOutlet, IncludeParentIds = true)

        public static JObject ToJSON(this SocketOutlet  SocketOutlet,
                                     Boolean            IncludeParentIds = true)
        {

            return org.GraphDefined.Vanaheimr.Hermod.JSONObject.Create(new JProperty("Plug",        SocketOutlet.Plug.       ToString()),
                                     SocketOutlet.Cable       != CableType.none ? new JProperty("Cable",       SocketOutlet.Cable.      ToString()) : null,
                                     SocketOutlet.CableLength  > 0              ? new JProperty("CableLength", SocketOutlet.CableLength.ToString()) : null);

        }

        #endregion

        #region ToJSON(this SocketOutlet, JPropertyKey)

        public static JProperty ToJSON(this SocketOutlet SocketOutlet, String JPropertyKey)
        {

            return new JProperty(JPropertyKey, SocketOutlet.ToJSON());

        }

        #endregion

        #region ToJSON(this SocketOutlets, IncludeParentIds = true)

        public static JArray ToJSON(this IEnumerable<SocketOutlet>  SocketOutlets,
                                    Boolean                         IncludeParentIds = true)
        {

            return (SocketOutlets != null && SocketOutlets.Any())
                        ? new JArray(SocketOutlets.SafeSelect(socket => socket.ToJSON(IncludeParentIds)))
                        : new JArray();

        }

        #endregion

        #region ToJSON(this SocketOutlets, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<SocketOutlet> SocketOutlets, String JPropertyKey)
        {

            return (SocketOutlets != null)
                        ? new JProperty(JPropertyKey,
                                        SocketOutlets.ToJSON())
                        : new JProperty(JPropertyKey, new JArray());

        }

        #endregion


        #region ToJSON(this ChargingReservation)

        public static JObject ToJSON(this ChargingReservation  ChargingReservation)
        {

            #region Initial checks

            if (ChargingReservation == null)
                throw new ArgumentNullException("ChargingReservation", "The given charging reservation must not be null!");

            #endregion

            return JSONObject.Create(new JProperty("ReservationId",           ChargingReservation.Id.               ToString()),
                                     new JProperty("ReservationType",         ChargingReservation.ReservationLevel.  ToString()),
                                     new JProperty("ChargingPoolId",          ChargingReservation.ChargingPoolId.   ToString()),
                                     new JProperty("ChargingStationId",       ChargingReservation.ChargingStationId.ToString()),
                                     new JProperty("EVSEId",                  ChargingReservation.EVSEId.           ToString()),
                                     new JProperty("StartTime",               ChargingReservation.StartTime.        ToIso8601()),
                                     new JProperty("Duration",       (UInt32) ChargingReservation.Duration.         TotalSeconds),

                                     (ChargingReservation.AuthTokens.Any() ||
                                      ChargingReservation.eMAIds. Any() ||
                                      ChargingReservation.PINs.   Any())
                                          ? new JProperty("AuthorizedIds", JSONObject.Create(

                                                ChargingReservation.AuthTokens.Any()
                                                    ? new JProperty("AuthTokens", new JArray(ChargingReservation.AuthTokens.Select(v => v.ToString())))
                                                    : null,

                                                ChargingReservation.eMAIds.Any()
                                                    ? new JProperty("eMAIds",  new JArray(ChargingReservation.eMAIds. Select(v => v.ToString())))
                                                    : null,

                                                ChargingReservation.PINs.Any()
                                                    ? new JProperty("PINs",    new JArray(ChargingReservation.PINs.   Select(v => v.ToString())))
                                                    : null

                                              ))
                                          : null

                                    );

        }

        #endregion

        #region ToJSON(this ChargingReservation, JPropertyKey)

        public static JProperty ToJSON(this ChargingReservation ChargingReservation, String JPropertyKey)
        {

            #region Initial checks

            if (ChargingReservation == null)
                throw new ArgumentNullException("ChargingReservation",  "The given charging reservation must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException("JPropertyKey",         "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargingReservation.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingReservation)

        public static JArray ToJSON(this IEnumerable<ChargingReservation>  ChargingReservations,
                                    Boolean                                IncludeParentIds = true)
        {

            #region Initial checks

            if (ChargingReservations == null)
                return new JArray();

            #endregion

            return ChargingReservations != null && ChargingReservations.Any()
                       ? new JArray(ChargingReservations.SafeSelect(reservation => reservation.ToJSON()))
                       : new JArray();

        }

        #endregion

        #region ToJSON(this ChargingReservations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingReservation> ChargingReservations, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException("JPropertyKey", "The json property key must not be null or empty!");

            #endregion

            return ChargingReservations != null
                       ? new JProperty(JPropertyKey, ChargingReservations.ToJSON())
                       : new JProperty(JPropertyKey, new JArray());

        }

        #endregion


        #region ToJSON(this ChargingSession)

        public static JObject ToJSON(this ChargingSession  ChargingSession)
        {

            return JSONObject.Create(

                new JProperty("ChargingSessionId",  ChargingSession.Id.ToString()),
                new JProperty("EVSEId",             ChargingSession.EVSE.Id.ToString()),

                 ChargingSession.ChargingProductId   != null
                     ? new JProperty("ChargingProductId", ChargingSession.ChargingProductId.ToString())
                     : null,

                 ChargingSession.Reservation != null
                     ? new JProperty("ChargingReservationId", ChargingSession.Reservation.Id.ToString())
                     : null

            );

        }

        #endregion

        #region ToJSON(this ChargingSession, JPropertyKey)

        public static JProperty ToJSON(this ChargingSession ChargingSession, String JPropertyKey)
        {

            #region Initial checks

            if (ChargingSession == null)
                throw new ArgumentNullException("ChargingSession", "The given charging session must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException("JPropertyKey", "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargingSession.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingSessions)

        public static JArray ToJSON(this IEnumerable<ChargingSession>  ChargingSessions)
        {

            #region Initial checks

            if (ChargingSessions == null)
                return new JArray();

            #endregion

            return ChargingSessions != null && ChargingSessions.Any()
                       ? new JArray(ChargingSessions.SafeSelect(session => session.ToJSON()))
                       : new JArray();

        }

        #endregion

        #region ToJSON(this ChargingSessions, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingSession> ChargingSessions, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException("JPropertyKey", "The json property key must not be null or empty!");

            #endregion

            return ChargingSessions != null
                       ? new JProperty(JPropertyKey, ChargingSessions.ToJSON())
                       : new JProperty(JPropertyKey, new JArray());

        }

        #endregion


    }

}
