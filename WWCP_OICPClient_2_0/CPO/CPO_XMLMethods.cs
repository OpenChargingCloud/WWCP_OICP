/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICP>
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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using System.Globalization;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    /// <summary>
    /// OICP v2.0 CPO management operations.
    /// </summary>
    public static class CPO_XMLMethods
    {

        #region PushEVSEDataXML(this GroupedData,      Action = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(ILookup<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                                               ActionType                                Action        = ActionType.fullLoad,
                                               EVSEOperator_Id                           OperatorId    = null,
                                               String                                    OperatorName  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPushEvseData>
            //
            //          <v2:ActionType>?</v2:ActionType>
            //
            //          <v2:OperatorEvseData>
            //
            //             <v2:OperatorID>?</v2:OperatorID>
            //
            //             <!--Optional:-->
            //             <v2:OperatorName>?</v2:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <v2:EvseDataRecord deltaType="?" lastUpdate="?">
            //                [...]
            //             </v2:EvseDataRecord>
            //
            //          </v2:OperatorEvseData>
            //       </v2:eRoamingPushEvseData>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", Action.ToString()),
                                      GroupedData.Select(datagroup =>
                                          new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                              new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId != null
                                                                                                ? OperatorId
                                                                                                : datagroup.Key.Id).OriginId),

                                              (OperatorName.IsNotNullOrEmpty() || datagroup.Key.Name.Any())
                                                  ? new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                        ? OperatorName
                                                                                                        : datagroup.Key.Name.First().Text))
                                                  : null,

                                              // <EvseDataRecord> ... </EvseDataRecord>
                                              datagroup.SelectMany(evses => evses.ToEvseDataRecordXML()).ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEDataXML(this EVSEOperator,     Action = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this EVSEOperator       EVSEOperator,
                                               ActionType              Action        = ActionType.fullLoad,
                                               EVSEOperator_Id         OperatorId    = null,
                                               String                  OperatorName  = null,
                                               Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            #endregion

            return new EVSEOperator[] { EVSEOperator }.
                       PushEVSEDataXML(Action,
                                       OperatorId,
                                       OperatorName,
                                       IncludeEVSEs);

        }

        #endregion

        #region PushEVSEDataXML(this EVSEOperators,    Action = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSEOperator>  EVSEOperators,
                                               ActionType                      Action        = ActionType.fullLoad,
                                               EVSEOperator_Id                 OperatorId    = null,
                                               String                          OperatorName  = null,
                                               Func<EVSE_Id, Boolean>          IncludeEVSEs  = null)
        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException("EVSEOperators", "The given parameter must not be null!");

            var _EVSEOperators = EVSEOperators.ToArray();

            if (_EVSEOperators.Length == 0)
                throw new ArgumentNullException("EVSEOperators", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return PushEVSEDataXML(_EVSEOperators.ToLookup(evseoperator => evseoperator,
                                                           evseoperator => evseoperator.SelectMany(pool    => pool.ChargingStations).
                                                                                        SelectMany(station => station.EVSEs).
                                                                                        Where     (evse    => IncludeEVSEs(evse.Id))),
                                   Action,
                                   OperatorId,
                                   OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingPool,     Action = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this ChargingPool       ChargingPool,
                                               ActionType              Action        = ActionType.insert,
                                               EVSEOperator_Id         OperatorId    = null,
                                               String                  OperatorName  = null,
                                               Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given parameter must not be null!");

            #endregion

            return new ChargingPool[] { ChargingPool }.
                       PushEVSEDataXML(Action,
                                       OperatorId,
                                       OperatorName,
                                       IncludeEVSEs);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingPools,    Action = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingPool>  ChargingPools,
                                               ActionType                      Action        = ActionType.insert,
                                               EVSEOperator_Id                 OperatorId    = null,
                                               String                          OperatorName  = null,
                                               Func<EVSE_Id, Boolean>          IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException("ChargingPools", "The given parameter must not be null!");

            var _ChargingPools = ChargingPools.ToArray();

            if (_ChargingPools.Length == 0)
                throw new ArgumentNullException("ChargingPools", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return PushEVSEDataXML(_ChargingPools.ToLookup(pool => pool.EVSEOperator,
                                                           pool => pool.SelectMany(station => station.EVSEs).
                                                                        Where     (evse    => IncludeEVSEs(evse.Id))),
                                   Action,
                                   OperatorId,
                                   OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingStation,  Action = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this ChargingStation    ChargingStation,
                                               ActionType              Action        = ActionType.insert,
                                               EVSEOperator_Id         OperatorId    = null,
                                               String                  OperatorName  = null,
                                               Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return new ChargingStation[] { ChargingStation }.
                       PushEVSEDataXML(Action,
                                       OperatorId,
                                       OperatorName,
                                       IncludeEVSEs);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingStations, Action = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingStation>  ChargingStations,
                                               ActionType                         Action        = ActionType.insert,
                                               EVSEOperator_Id                    OperatorId    = null,
                                               String                             OperatorName  = null,
                                               Func<EVSE_Id, Boolean>             IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException("ChargingStations", "The given parameter must not be null!");

            var _ChargingStations = ChargingStations.ToArray();

            if (_ChargingStations.Length == 0)
                throw new ArgumentNullException("ChargingStations", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return PushEVSEDataXML(_ChargingStations.ToLookup(station => station.ChargingPool.EVSEOperator,
                                                              station => station.Where(evse => IncludeEVSEs(evse.Id))),
                                   Action,
                                   OperatorId,
                                   OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this EVSE,             Action = insert,   OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(this EVSE        EVSE,
                                               EVSEOperator_Id  OperatorId    = null,
                                               String           OperatorName  = null,
                                               ActionType       Action        = ActionType.insert)
        {

            return new EVSE[] { EVSE }.
                       PushEVSEDataXML(Action,
                                       OperatorId,
                                       OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this EVSEs,            Action = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSE>  EVSEs,
                                               ActionType              Action        = ActionType.insert,
                                               EVSEOperator_Id         OperatorId    = null,
                                               String                  OperatorName  = null,
                                               Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPushEvseData>
            //
            //          <v2:ActionType>?</v2:ActionType>
            //
            //          <v2:OperatorEvseData>
            //
            //             <v2:OperatorID>?</v2:OperatorID>
            //
            //             <!--Optional:-->
            //             <v2:OperatorName>?</v2:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <v2:EvseDataRecord deltaType="?" lastUpdate="?">
            //                [...]
            //             </v2:EvseDataRecord>
            //
            //          </v2:OperatorEvseData>
            //       </v2:eRoamingPushEvseData>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be null!");

            var _EVSEs = EVSEs.ToArray();

            if (_EVSEs.Length == 0)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be empty!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", Action.ToString()),
                                      new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                          new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId != null
                                                                                           ? OperatorId
                                                                                           : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Id).OriginId),

                                          (OperatorName.IsNotNullOrEmpty() || _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.Any())
                                              ? new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                    ? OperatorName
                                                                                                    : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.First().Text))
                                              : null,

                                          // <EvseDataRecord> ... </EvseDataRecord>
                                          _EVSEs.
                                              Where(evse => IncludeEVSEs(evse.Id)).
                                              ToEvseDataRecordXML().
                                              ToArray()

                                      )
                                  ));

        }

        #endregion

        #region (internal) ToEvseDataRecords(this EVSE)

        internal static XElement ToEvseDataRecordXML(this EVSE EVSE)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">

            // <v2:EvseDataRecord deltaType="?" lastUpdate="?">
            //
            //    <v2:EvseId>?</v2:EvseId>
            //
            //    <!--Optional:-->
            //    <v2:ChargingStationId>?</v2:ChargingStationId>
            //    <!--Optional:-->
            //    <v2:ChargingStationName>?</v2:ChargingStationName>
            //    <!--Optional:-->
            //    <v2:EnChargingStationName>?</v2:EnChargingStationName>
            //
            //    <v2:Address>
            //       <v21:Country>?</v21:Country>
            //       <v21:City>?</v21:City>
            //       <v21:Street>?</v21:Street>
            //       <!--Optional:-->
            //       <v21:PostalCode>?</v21:PostalCode>
            //       <!--Optional:-->
            //       <v21:HouseNum>?</v21:HouseNum>
            //       <!--Optional:-->
            //       <v21:Floor>?</v21:Floor>
            //       <!--Optional:-->
            //       <v21:Region>?</v21:Region>
            //       <!--Optional:-->
            //       <v21:TimeZone>?</v21:TimeZone>
            //    </v2:Address>
            //
            //    <v2:GeoCoordinates>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <v21:Google>
            //          <v21:Coordinates>?</v21:Coordinates>
            //       </v21:Google>
            //       <v21:DecimalDegree>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DecimalDegree>
            //       <v21:DegreeMinuteSeconds>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DegreeMinuteSeconds>
            //    </v2:GeoCoordinates>
            //
            //    <v2:Plugs>
            //       <!--1 or more repetitions:-->
            //       <v2:Plug>?</v2:Plug>
            //    </v2:Plugs>
            //
            //    <!--Optional:-->
            //    <v2:ChargingFacilities>
            //       <!--1 or more repetitions:-->
            //       <v2:ChargingFacility>?</v2:ChargingFacility>
            //    </v2:ChargingFacilities>
            //
            //    <!--Optional:-->
            //    <v2:ChargingModes>
            //       <!--1 or more repetitions:-->
            //       <v2:ChargingMode>?</v2:ChargingMode>
            //    </v2:ChargingModes>
            //
            //    <v2:AuthenticationModes>
            //       <!--1 or more repetitions:-->
            //       <v2:AuthenticationMode>?</v2:AuthenticationMode>
            //    </v2:AuthenticationModes>
            //
            //    <!--Optional:-->
            //    <v2:MaxCapacity>?</v2:MaxCapacity>
            //
            //    <!--Optional:-->
            //    <v2:PaymentOptions>
            //       <!--1 or more repetitions:-->
            //       <v2:PaymentOption>?</v2:PaymentOption>
            //    </v2:PaymentOptions>
            //
            //    <v2:Accessibility>?</v2:Accessibility>
            //    <v2:HotlinePhoneNum>?</v2:HotlinePhoneNum>

            //    <!--Optional:-->
            //    <v2:AdditionalInfo>?</v2:AdditionalInfo>
            //
            //    <!--Optional:-->
            //    <v2:EnAdditionalInfo>?</v2:EnAdditionalInfo>
            //
            //    <!--Optional:-->
            //    <v2:GeoChargingPointEntrance>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <v21:Google>
            //          <v21:Coordinates>?</v21:Coordinates>
            //       </v21:Google>
            //       <v21:DecimalDegree>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DecimalDegree>
            //       <v21:DegreeMinuteSeconds>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DegreeMinuteSeconds>
            //    </v2:GeoChargingPointEntrance>
            //
            //    <v2:IsOpen24Hours>?</v2:IsOpen24Hours>
            //    <!--Optional:-->
            //    <v2:OpeningTime>?</v2:OpeningTime>
            //
            //    <!--Optional:-->
            //    <v2:HubOperatorID>?</v2:HubOperatorID>
            //
            //    <!--Optional:-->
            //    <v2:ClearinghouseID>?</v2:ClearinghouseID>
            //
            //    <v2:IsHubjectCompatible>?</v2:IsHubjectCompatible>
            //    <v2:DynamicInfoAvailable>?</v2:DynamicInfoAvailable>
            //
            // </v2:EvseDataRecord>

            #endregion

            #region Inital checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given parameter must not be null!");

            #endregion

            return new XElement(OICPNS.EVSEData + "EvseDataRecord",

                new XElement(OICPNS.EVSEData + "EvseId",                EVSE.Id.ToFormat(IdFormatType.OLD)),
                new XElement(OICPNS.EVSEData + "ChargingStationId",     EVSE.ChargingStation.Id.ToString()),
                new XElement(OICPNS.EVSEData + "ChargingStationName",   EVSE.ChargingStation.ChargingPool.Name[Languages.de].SubstringMax(50)),
                new XElement(OICPNS.EVSEData + "EnChargingStationName", EVSE.ChargingStation.ChargingPool.Name[Languages.en].SubstringMax(50)),

                new XElement(OICPNS.EVSEData + "Address",
                    new XElement(OICPNS.CommonTypes + "Country",        EVSE.ChargingStation.Address.Country.Alpha3Code),
                    new XElement(OICPNS.CommonTypes + "City",           EVSE.ChargingStation.Address.City),
                    new XElement(OICPNS.CommonTypes + "Street",         EVSE.ChargingStation.Address.Street), // OICPv2.0 requires at least 5 characters!
                    new XElement(OICPNS.CommonTypes + "PostalCode",     EVSE.ChargingStation.Address.PostalCode),
                    new XElement(OICPNS.CommonTypes + "HouseNum",       EVSE.ChargingStation.Address.HouseNumber),
                    new XElement(OICPNS.CommonTypes + "Floor",          EVSE.ChargingStation.Address.FloorLevel)
                // <!--Optional:-->
                // <v11:Region>?</v11:Region>
                // <!--Optional:-->
                // <v11:TimeZone>?</v11:TimeZone>
                ),

                new XElement(OICPNS.EVSEData + "GeoCoordinates",
                    new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                        new XElement(OICPNS.CommonTypes + "Longitude",  EVSE.ChargingStation.GeoLocation.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                        new XElement(OICPNS.CommonTypes + "Latitude",   EVSE.ChargingStation.GeoLocation.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                    )
                ),

                new XElement(OICPNS.EVSEData + "Plugs",
                    EVSE.SocketOutlets.Select(Outlet =>
                       new XElement(OICPNS.EVSEData + "Plug", OICPMapper.AsString(Outlet)))
                ),

                EVSE.SocketOutlets.Any()
                    ? new XElement(OICPNS.EVSEData + "ChargingFacilities",
                         EVSE.SocketOutlets.Select(Outlet => {

                             var ChargingFacility = "Unspecified";

                             if (Outlet.Plug == PlugTypes.Type2Outlet ||
                                 Outlet.Plug == PlugTypes.Type2Connector_CableAttached)// Mennekes_Type_2
                             {

                                 if (EVSE.MaxPower <= 44.0)
                                     ChargingFacility = "380 - 480V, 3-Phase ≤63A";

                                 if (EVSE.MaxPower <= 22.0)
                                     ChargingFacility = "380 - 480V, 3-Phase ≤32A";

                                 if (EVSE.MaxPower <= 11.0)
                                     ChargingFacility = "380 - 480V, 3-Phase ≤16A";

                             }

                             else if (Outlet.Plug == PlugTypes.TypeFSchuko)
                             {

                                 if (EVSE.MaxPower > 7.2)
                                     ChargingFacility = "200 - 240V, 1-Phase >32A";

                                 if (EVSE.MaxPower <= 7.2)
                                     ChargingFacility = "200 - 240V, 1-Phase ≤32A";

                                 if (EVSE.MaxPower <= 3.6)
                                     ChargingFacility = "200 - 240V, 1-Phase ≤16A";

                                 if (EVSE.MaxPower <= 2.25)
                                     ChargingFacility = "200 - 240V, 1-Phase ≤10A";

                             }

                             // 100 - 120V, 1-Phase ≤10A
                             // 100 - 120V, 1-Phase ≤16A
                             // 100 - 120V, 1-Phase ≤32A

                             // Battery exchange
                             // Unspecified
                             // DC Charging ≤20kW
                             // DC Charging ≤50kW
                             // DC Charging >50kW

                             return new XElement(OICPNS.EVSEData + "ChargingFacility", ChargingFacility);

                         })

                     )
                    : null,

                EVSE.ChargingModes.Any()
                    ? new XElement(OICPNS.EVSEData + "ChargingModes",
                          EVSE.ChargingModes.Select(ChargingMode => new XElement(OICPNS.EVSEData + "ChargingMode", ChargingMode.ToString())))
                    : null,

                EVSE.ChargingStation.AuthenticationModes.Any()
                    ? new XElement(OICPNS.EVSEData + "AuthenticationModes",
                          EVSE.ChargingStation.AuthenticationModes.Select(AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode.ToString())))
                    : null,

                EVSE.MaxCapacity_kWh > 0
                    ? new XElement(OICPNS.EVSEData + "MaxCapacity", EVSE.MaxCapacity_kWh)
                    : null,

                EVSE.ChargingStation.PaymentOptions.Any()
                    ? new XElement(OICPNS.EVSEData + "PaymentOptions",
                          EVSE.ChargingStation.PaymentOptions.Select(PaymentOption => new XElement(OICPNS.EVSEData + "PaymentOption", PaymentOption.ToString())))
                    : null,

                new XElement(OICPNS.EVSEData + "Accessibility",   EVSE.ChargingStation.Accessibility.ToString().Replace("_", " ")),
                new XElement(OICPNS.EVSEData + "HotlinePhoneNum", EVSE.ChargingStation.HotlinePhoneNum),  // RegEx: \+[0-9]{5,15}

                EVSE.Description.Any()
                    ? new XElement(OICPNS.EVSEData + "AdditionalInfo", EVSE.Description.First().Text)
                    : null,

                EVSE.Description.has(Languages.en)
                    ? new XElement(OICPNS.EVSEData + "EnAdditionalInfo", EVSE.Description[Languages.en])
                    : null,


                EVSE.ChargingStation.EntranceLocation != null
                    ? new XElement(OICPNS.EVSEData + "GeoChargingPointEntrance",
                          new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                              new XElement(OICPNS.CommonTypes + "Longitude",  EVSE.ChargingStation.GeoLocation.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                              new XElement(OICPNS.CommonTypes + "Latitude",   EVSE.ChargingStation.GeoLocation.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                          )
                      )
                    : null,

                new XElement(OICPNS.EVSEData + "IsOpen24Hours",         EVSE.ChargingStation.ChargingPool.OpeningTime.IsOpen24Hours ? "true" : "false"),

                EVSE.ChargingStation.ChargingPool.OpeningTime.IsOpen24Hours
                    ? null
                    : new XElement(OICPNS.EVSEData + "OpeningTime",     EVSE.ChargingStation.ChargingPool.OpeningTime.Text),

                // <!--Optional:-->
                // <v1:HubOperatorID>?</v1:HubOperatorID>

                // <!--Optional:-->
                // <v1:ClearinghouseID>?</v1:ClearinghouseID>

                new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   EVSE.ChargingStation.IsHubjectCompatible ? "true" : "false"),
                new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  EVSE.ChargingStation.DynamicInfoAvailable ? "true" : "false")

            );

        }

        #endregion

        #region (internal) ToEvseDataRecords(this EVSEs)

        internal static IEnumerable<XElement> ToEvseDataRecordXML(this IEnumerable<EVSE> EVSEs)
        {

            #region Inital checks

            if (EVSEs == null)
                return new XElement[0];

            var _EVSEs = EVSEs.ToArray();

            if (_EVSEs.Length == 0)
                return new XElement[0];

            #endregion

            return _EVSEs.Select(EVSE => EVSE.ToEvseDataRecordXML());

        }

        #endregion

        #region (internal) ToXML(this EVSEDataRecord)

        internal static XElement ToXML(this EVSEDataRecord EVSEDataRecord)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">

            // <v2:EvseDataRecord deltaType="?" lastUpdate="?">
            //
            //    <v2:EvseId>?</v2:EvseId>
            //
            //    <!--Optional:-->
            //    <v2:ChargingStationId>?</v2:ChargingStationId>
            //    <!--Optional:-->
            //    <v2:ChargingStationName>?</v2:ChargingStationName>
            //    <!--Optional:-->
            //    <v2:EnChargingStationName>?</v2:EnChargingStationName>
            //
            //    <v2:Address>
            //       <v21:Country>?</v21:Country>
            //       <v21:City>?</v21:City>
            //       <v21:Street>?</v21:Street>
            //       <!--Optional:-->
            //       <v21:PostalCode>?</v21:PostalCode>
            //       <!--Optional:-->
            //       <v21:HouseNum>?</v21:HouseNum>
            //       <!--Optional:-->
            //       <v21:Floor>?</v21:Floor>
            //       <!--Optional:-->
            //       <v21:Region>?</v21:Region>
            //       <!--Optional:-->
            //       <v21:TimeZone>?</v21:TimeZone>
            //    </v2:Address>
            //
            //    <v2:GeoCoordinates>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <v21:Google>
            //          <v21:Coordinates>?</v21:Coordinates>
            //       </v21:Google>
            //       <v21:DecimalDegree>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DecimalDegree>
            //       <v21:DegreeMinuteSeconds>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DegreeMinuteSeconds>
            //    </v2:GeoCoordinates>
            //
            //    <v2:Plugs>
            //       <!--1 or more repetitions:-->
            //       <v2:Plug>?</v2:Plug>
            //    </v2:Plugs>
            //
            //    <!--Optional:-->
            //    <v2:ChargingFacilities>
            //       <!--1 or more repetitions:-->
            //       <v2:ChargingFacility>?</v2:ChargingFacility>
            //    </v2:ChargingFacilities>
            //
            //    <!--Optional:-->
            //    <v2:ChargingModes>
            //       <!--1 or more repetitions:-->
            //       <v2:ChargingMode>?</v2:ChargingMode>
            //    </v2:ChargingModes>
            //
            //    <v2:AuthenticationModes>
            //       <!--1 or more repetitions:-->
            //       <v2:AuthenticationMode>?</v2:AuthenticationMode>
            //    </v2:AuthenticationModes>
            //
            //    <!--Optional:-->
            //    <v2:MaxCapacity>?</v2:MaxCapacity>
            //
            //    <!--Optional:-->
            //    <v2:PaymentOptions>
            //       <!--1 or more repetitions:-->
            //       <v2:PaymentOption>?</v2:PaymentOption>
            //    </v2:PaymentOptions>
            //
            //    <v2:Accessibility>?</v2:Accessibility>
            //    <v2:HotlinePhoneNum>?</v2:HotlinePhoneNum>

            //    <!--Optional:-->
            //    <v2:AdditionalInfo>?</v2:AdditionalInfo>
            //
            //    <!--Optional:-->
            //    <v2:EnAdditionalInfo>?</v2:EnAdditionalInfo>
            //
            //    <!--Optional:-->
            //    <v2:GeoChargingPointEntrance>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <v21:Google>
            //          <v21:Coordinates>?</v21:Coordinates>
            //       </v21:Google>
            //       <v21:DecimalDegree>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DecimalDegree>
            //       <v21:DegreeMinuteSeconds>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DegreeMinuteSeconds>
            //    </v2:GeoChargingPointEntrance>
            //
            //    <v2:IsOpen24Hours>?</v2:IsOpen24Hours>
            //    <!--Optional:-->
            //    <v2:OpeningTime>?</v2:OpeningTime>
            //
            //    <!--Optional:-->
            //    <v2:HubOperatorID>?</v2:HubOperatorID>
            //
            //    <!--Optional:-->
            //    <v2:ClearinghouseID>?</v2:ClearinghouseID>
            //
            //    <v2:IsHubjectCompatible>?</v2:IsHubjectCompatible>
            //    <v2:DynamicInfoAvailable>?</v2:DynamicInfoAvailable>
            //
            // </v2:EvseDataRecord>

            #endregion

            #region Inital checks

            if (EVSEDataRecord                     == null)
                throw new ArgumentNullException("EVSEDataRecord",                       "The given parameter must not be null!");

            if (EVSEDataRecord.EVSEId              == null)
                throw new ArgumentNullException("EVSEDataRecord.EVSE",                  "The given EVSE Id must not be null!");

            if (EVSEDataRecord.Address             == null)
                throw new ArgumentNullException("EVSEDataRecord.Address",               "The given address must not be null!");

            if (EVSEDataRecord.Address.Country     == null)
                throw new ArgumentNullException("EVSEDataRecord.Address.Country",       "The given country must not be null!");

            if (EVSEDataRecord.Address.City.  IsNullOrEmpty())
                throw new ArgumentNullException("EVSEDataRecord.Address.City",          "The given city must not be null!");

            if (EVSEDataRecord.Address.Street.IsNullOrEmpty())
                throw new ArgumentNullException("EVSEDataRecord.Address.Street",        "The given street must not be null!");

            if (EVSEDataRecord.GeoCoordinates       == null)
                throw new ArgumentNullException("EVSEDataRecord.GeoCoordinate",         "The given geo coordinate must not be null!");

            if (EVSEDataRecord.Plugs               == null || !EVSEDataRecord.Plugs.              Any())
                throw new ArgumentNullException("Plugs",                                "There must be at least one plug defined!");

            if (EVSEDataRecord.AuthenticationModes == null || !EVSEDataRecord.AuthenticationModes.Any())
                throw new ArgumentNullException("EVSEDataRecord.AuthenticationModes",   "The given authentication modes must not be null!");

            if (EVSEDataRecord.HotlinePhoneNum.IsNullOrEmpty())
                throw new ArgumentNullException("EVSEDataRecord.HotlinePhoneNum",       "The given hotline phone number must not be null!");

            #endregion

            return new XElement(OICPNS.EVSEData + "EvseDataRecord",

                new XElement(OICPNS.EVSEData + "EvseId",                EVSEDataRecord.EVSEId.OriginId),
                new XElement(OICPNS.EVSEData + "ChargingStationId",     EVSEDataRecord.ChargingStationId),
                new XElement(OICPNS.EVSEData + "ChargingStationName",   EVSEDataRecord.ChargingStationName.  SubstringMax(50)),
                new XElement(OICPNS.EVSEData + "EnChargingStationName", EVSEDataRecord.EnChargingStationName.SubstringMax(50)),

                new XElement(OICPNS.EVSEData + "Address",
                    new XElement(OICPNS.CommonTypes + "Country",        EVSEDataRecord.Address.Country.Alpha3Code),
                    new XElement(OICPNS.CommonTypes + "City",           EVSEDataRecord.Address.City),
                    new XElement(OICPNS.CommonTypes + "Street",         EVSEDataRecord.Address.Street), // OICPv2.0 requires at least 5 characters!
                    new XElement(OICPNS.CommonTypes + "PostalCode",     EVSEDataRecord.Address.PostalCode),
                    new XElement(OICPNS.CommonTypes + "HouseNum",       EVSEDataRecord.Address.HouseNumber),
                    new XElement(OICPNS.CommonTypes + "Floor",          EVSEDataRecord.Address.FloorLevel)
                // <!--Optional:-->
                // <v11:Region>?</v11:Region>
                // <!--Optional:-->
                // <v11:TimeZone>?</v11:TimeZone>
                ),

                new XElement(OICPNS.EVSEData + "GeoCoordinates",
                    new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                        new XElement(OICPNS.CommonTypes + "Longitude",  EVSEDataRecord.GeoCoordinates.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                        new XElement(OICPNS.CommonTypes + "Latitude",   EVSEDataRecord.GeoCoordinates.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                    )
                ),

                new XElement(OICPNS.EVSEData + "Plugs",
                    EVSEDataRecord.Plugs.                   Select(Plug               => new XElement(OICPNS.EVSEData + "Plug",               OICPMapper.AsString(Plug)))
                ),

                EVSEDataRecord.ChargingFacilities.Any()
                    ? new XElement(OICPNS.EVSEData + "ChargingFacilities",
                          EVSEDataRecord.ChargingFacilities.Select(ChargingFacility   => new XElement(OICPNS.EVSEData + "ChargingFacility",   ChargingFacility)))
                    : null,

                EVSEDataRecord.ChargingModes.Any()
                    ? new XElement(OICPNS.EVSEData + "ChargingModes",
                          EVSEDataRecord.ChargingModes.     Select(ChargingMode       => new XElement(OICPNS.EVSEData + "ChargingMode",       ChargingMode)))
                    : null,

                new XElement(OICPNS.EVSEData + "AuthenticationModes",
                    EVSEDataRecord.AuthenticationModes.     Select(AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode))
                ),

                new XElement(OICPNS.EVSEData + "MaxCapacity", EVSEDataRecord.MaxCapacity),

                new XElement(OICPNS.EVSEData + "PaymentOptions",
                    EVSEDataRecord.PaymentOptions.          Select(PaymentOption      => new XElement(OICPNS.EVSEData + "PaymentOption",      PaymentOption))
                ),

                new XElement(OICPNS.EVSEData + "Accessibility",     EVSEDataRecord.Accessibility),
                new XElement(OICPNS.EVSEData + "HotlinePhoneNum",   EVSEDataRecord.HotlinePhoneNum),  // RegEx: \+[0-9]{5,15}

                EVSEDataRecord.AdditionalInfo.IsNotNullOrEmpty()
                    ? new XElement(OICPNS.EVSEData + "AdditionalInfo", EVSEDataRecord.AdditionalInfo)
                    : null,

                EVSEDataRecord.EnAdditionalInfo.Any()
                    ? new XElement(OICPNS.EVSEData + "EnAdditionalInfo", EVSEDataRecord.EnAdditionalInfo[Languages.en])
                    : null,

                new XElement(OICPNS.EVSEData + "GeoChargingPointEntrance",
                    new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                        new XElement(OICPNS.CommonTypes + "Longitude", EVSEDataRecord.GeoChargingPointEntrance.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                        new XElement(OICPNS.CommonTypes + "Latitude",  EVSEDataRecord.GeoChargingPointEntrance.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                    )
                ),

                new XElement(OICPNS.EVSEData + "IsOpen24Hours",         EVSEDataRecord.IsOpen24Hours ? "true" : "false"),

                EVSEDataRecord.OpeningTime.IsOpen24Hours
                    ? null
                    : new XElement(OICPNS.EVSEData + "OpeningTime",     EVSEDataRecord.OpeningTime.Text),

                EVSEDataRecord.HubOperatorId != null
                    ? new XElement(OICPNS.EVSEData + "HubOperatorID",   EVSEDataRecord.HubOperatorId.ToString())
                    : null,

                EVSEDataRecord.ClearinghouseId != null
                    ? new XElement(OICPNS.EVSEData + "ClearinghouseID", EVSEDataRecord.ClearinghouseId.ToString())
                    : null,

                new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   EVSEDataRecord.IsHubjectCompatible  ? "true" : "false"),
                new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  EVSEDataRecord.DynamicInfoAvailable ? "true" : "false")

            );

        }

        #endregion

        #region (internal) ToXML(this EVSEDataRecords)

        internal static IEnumerable<XElement> ToXML(this IEnumerable<EVSEDataRecord> EVSEDataRecords)
        {

            #region Inital checks

            if (EVSEDataRecords == null)
                return new XElement[0];

            var _EVSEDataRecords = EVSEDataRecords.ToArray();

            if (_EVSEDataRecords.Length == 0)
                return new XElement[0];

            #endregion

            return _EVSEDataRecords.Select(EVSEDataRecord => EVSEDataRecord.ToXML());

        }

        #endregion


        #region PushEVSEStatusXML(this GroupedData,      Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(Dictionary<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                                                 ActionType                                   Action        = ActionType.update,
                                                 EVSEOperator_Id                              OperatorId    = null,
                                                 String                                       OperatorName  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPushEvseStatus>
            //
            //          <v2:ActionType>?</v2:ActionType>
            //
            //          <v2:OperatorEvseStatus>
            //
            //             <v2:OperatorID>?</v2:OperatorID>
            //
            //             <!--Optional:-->
            //             <v2:OperatorName>?</v2:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <v2:EvseStatusRecord>
            //                <v2:EvseId>?</v2:EvseId>
            //                <v2:EvseStatus>?</v2:EvseStatus>
            //             </v2:EvseStatusRecord>
            //
            //          </v2:OperatorEvseStatus>
            //
            //       </v2:eRoamingPushEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseStatus",
                                      new XElement(OICPNS.EVSEData + "ActionType", Action.ToString()),
                                      GroupedData.Select(datagroup =>
                                          new XElement(OICPNS.EVSEData + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId != null
                                                                                                ? OperatorId
                                                                                                : datagroup.Key.Id).OriginId),

                                              (OperatorName.IsNotNullOrEmpty() || datagroup.Key.Name.Any())
                                                  ? new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                        ? OperatorName
                                                                                                        : datagroup.Key.Name.First().Text))
                                                  : null,

                                              datagroup.Value.ToEvseDataRecordXML().ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEOperator,     Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this EVSEOperator       EVSEOperator,
                                                 ActionType              Action        = ActionType.update,
                                                 EVSEOperator_Id         OperatorId    = null,
                                                 String                  OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            #endregion

            return new EVSEOperator[] { EVSEOperator }.
                       PushEVSEStatusXML(Action,
                                         OperatorId,
                                         OperatorName,
                                         IncludeEVSEs);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEOperators,    Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSEOperator>  EVSEOperators,
                                                 ActionType                      Action        = ActionType.update,
                                                 EVSEOperator_Id                 OperatorId    = null,
                                                 String                          OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>          IncludeEVSEs  = null)
        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException("EVSEOperators", "The given parameter must not be null!");

            var _EVSEOperators = EVSEOperators.ToArray();

            if (_EVSEOperators.Length == 0)
                throw new ArgumentNullException("EVSEOperators", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return PushEVSEStatusXML(_EVSEOperators.ToDictionary(evseoperator => evseoperator,
                                                                 evseoperator => evseoperator.SelectMany(pool    => pool.ChargingStations).
                                                                                              SelectMany(station => station.EVSEs).
                                                                                              Where     (evse    => IncludeEVSEs(evse.Id))),
                                     Action,
                                     OperatorId,
                                     OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingPool,     Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this ChargingPool       ChargingPool,
                                                 ActionType              Action        = ActionType.update,
                                                 EVSEOperator_Id         OperatorId    = null,
                                                 String                  OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given parameter must not be null!");

            #endregion

            return new ChargingPool[] { ChargingPool }.
                       PushEVSEStatusXML(Action,
                                         OperatorId,
                                         OperatorName,
                                         IncludeEVSEs);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingPools,    Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<ChargingPool>  ChargingPools,
                                                 ActionType                      Action        = ActionType.update,
                                                 EVSEOperator_Id                 OperatorId    = null,
                                                 String                          OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>          IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException("ChargingPools", "The given parameter must not be null!");

            var _ChargingPools = ChargingPools.ToArray();

            if (_ChargingPools.Length == 0)
                throw new ArgumentNullException("ChargingPools", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return PushEVSEStatusXML(_ChargingPools.ToDictionary(pool => pool.EVSEOperator,
                                                                 pool => pool.SelectMany(station => station.EVSEs).
                                                                              Where     (evse    => IncludeEVSEs(evse.Id))),
                                     Action,
                                     OperatorId,
                                     OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingStation,  Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this ChargingStation    ChargingStation,
                                                 ActionType              Action        = ActionType.update,
                                                 EVSEOperator_Id         OperatorId    = null,
                                                 String                  OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return new ChargingStation[] { ChargingStation }.
                       PushEVSEStatusXML(Action,
                                         OperatorId,
                                         OperatorName,
                                         IncludeEVSEs);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingStations, Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<ChargingStation>  ChargingStations,
                                                 ActionType                         Action        = ActionType.update,
                                                 EVSEOperator_Id                    OperatorId    = null,
                                                 String                             OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>             IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException("ChargingStations", "The given parameter must not be null!");

            var _ChargingStations = ChargingStations.ToArray();

            if (_ChargingStations.Length == 0)
                throw new ArgumentNullException("ChargingStations", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return PushEVSEStatusXML(_ChargingStations.ToDictionary(station => station.ChargingPool.EVSEOperator,
                                                                    station => station.Where(evse => IncludeEVSEs(evse.Id))),
                                     Action,
                                     OperatorId,
                                     OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSE,             Action = update, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(this EVSE        EVSE,
                                                 EVSEOperator_Id  OperatorId    = null,
                                                 String           OperatorName  = null,
                                                 ActionType       Action        = ActionType.update)
        {

            return new EVSE[] { EVSE }.
                       PushEVSEStatusXML(Action,
                                         OperatorId,
                                         OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEs,            Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE>  EVSEs,
                                                 ActionType              Action        = ActionType.update,
                                                 EVSEOperator_Id         OperatorId    = null,
                                                 String                  OperatorName  = null,
                                                 Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPushEvseStatus>
            //
            //          <v2:ActionType>?</v2:ActionType>
            //
            //          <v2:OperatorEvseStatus>
            //
            //             <v2:OperatorID>?</v2:OperatorID>
            //
            //             <!--Optional:-->
            //             <v2:OperatorName>?</v2:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <v2:EvseStatusRecord>
            //                <v2:EvseId>?</v2:EvseId>
            //                <v2:EvseStatus>?</v2:EvseStatus>
            //             </v2:EvseStatusRecord>
            //
            //          </v2:OperatorEvseStatus>
            //
            //       </v2:eRoamingPushEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be null!");

            var _EVSEs = EVSEs.ToArray();

            if (_EVSEs.Length == 0)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",
                                          new XElement(OICPNS.EVSEStatus + "ActionType", Action.ToString()),
                                          new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId != null
                                                                                               ? OperatorId
                                                                                               : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Id).OriginId),

                                              (OperatorName.IsNotNullOrEmpty() || _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.Any())
                                                  ? new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                        ? OperatorName
                                                                                                        : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.First().Text))
                                                  : null,

                                              _EVSEs.
                                                  Where(evse => IncludeEVSEs(evse.Id)).
                                                  ToEvseStatusRecords().
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEIdAndStatus,       OperatorId, OperatorName = null, Action = update, IncludeEVSEIds = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEIdAndStatus,
                                                 EVSEOperator_Id                                          OperatorId,
                                                 String                                                   OperatorName    = null,
                                                 ActionType                                               Action          = ActionType.update,
                                                 Func<EVSE_Id, Boolean>                                   IncludeEVSEIds  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPushEvseStatus>
            //
            //          <v2:ActionType>?</v2:ActionType>
            //
            //          <v2:OperatorEvseStatus>
            //
            //             <v2:OperatorID>?</v2:OperatorID>
            //
            //             <!--Optional:-->
            //             <v2:OperatorName>?</v2:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <v2:EvseStatusRecord>
            //                <v2:EvseId>?</v2:EvseId>
            //                <v2:EvseStatus>?</v2:EvseStatus>
            //             </v2:EvseStatusRecord>
            //
            //          </v2:OperatorEvseStatus>
            //
            //       </v2:eRoamingPushEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEIdAndStatus == null)
                throw new ArgumentNullException("EVSEIdAndStatus", "The given parameter must not be null!");

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId",      "The given parameter must not be null!");

            if (OperatorName.IsNullOrEmpty())
                throw new ArgumentNullException("OperatorName",    "The given parameter must not be null!");

            var _EVSEIdAndStatus = EVSEIdAndStatus.ToArray();

            if (_EVSEIdAndStatus.Length == 0)
                throw new ArgumentNullException("EVSEIdAndStatus", "The given parameter must not be empty!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",
                                          new XElement(OICPNS.EVSEStatus + "ActionType", ActionType.delete.ToString()),
                                          new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEStatus + "OperatorID", OperatorId.OriginId),

                                              OperatorName.IsNotNullOrEmpty()
                                                  ? new XElement(OICPNS.EVSEData + "OperatorName", OperatorName)
                                                  : null,

                                              _EVSEIdAndStatus.
                                                  Where(kvp => IncludeEVSEIds(kvp.Key)).
                                                  ToEvseStatusRecords().
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEIds, CommonStatus, OperatorId, OperatorName = null, Action = update, IncludeEVSEIds = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE_Id>  EVSEIds,
                                                 EVSEStatusType             CommonStatus,
                                                 EVSEOperator_Id            OperatorId,
                                                 String                     OperatorName    = null,
                                                 ActionType                 Action          = ActionType.update,
                                                 Func<EVSE_Id, Boolean>     IncludeEVSEIds  = null)

        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPushEvseStatus>
            //
            //          <v2:ActionType>?</v2:ActionType>
            //
            //          <v2:OperatorEvseStatus>
            //
            //             <v2:OperatorID>?</v2:OperatorID>
            //
            //             <!--Optional:-->
            //             <v2:OperatorName>?</v2:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <v2:EvseStatusRecord>
            //                <v2:EvseId>?</v2:EvseId>
            //                <v2:EvseStatus>?</v2:EvseStatus>
            //             </v2:EvseStatusRecord>
            //
            //          </v2:OperatorEvseStatus>
            //
            //       </v2:eRoamingPushEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEIds == null)
                throw new ArgumentNullException("EVSEIds", "The given parameter must not be null!");

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (OperatorName.IsNullOrEmpty())
                throw new ArgumentNullException("OperatorName", "The given parameter must not be null!");

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                throw new ArgumentNullException("EVSEIds", "The given parameter must not be empty!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",
                                          new XElement(OICPNS.EVSEStatus + "ActionType", Action.ToString()),
                                          new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEStatus + "OperatorID", OperatorId.OriginId),

                                              OperatorName.IsNotNullOrEmpty()
                                                  ? new XElement(OICPNS.EVSEData + "OperatorName", OperatorName)
                                                  : null,

                                              _EVSEIds.
                                                  Where(evseid => IncludeEVSEIds(evseid)).
                                                  ToEvseStatusRecords(CommonStatus).
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSE)

        internal static XElement ToEvseStatusRecords(this EVSE EVSE)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">

            // <v2:EvseStatusRecord>
            //    <v2:EvseId>?</v2:EvseId>
            //    <v2:EvseStatus>?</v2:EvseStatus>
            // </v2:EvseStatusRecord>

            #endregion

            #region Inital checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given parameter must not be null!");

            #endregion

            return new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                       new XElement(OICPNS.EVSEStatus + "EvseId",     EVSE.Id.                             OriginId),
                       new XElement(OICPNS.EVSEStatus + "EvseStatus", EVSE.Status.Value.AsOICPEVSEStatus().ToString())
                   );

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSEs)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<EVSE> EVSEs)
        {

            #region Inital checks

            if (EVSEs == null)
                return new XElement[0];

            var _EVSEs = EVSEs.ToArray();

            if (_EVSEs.Length == 0)
                return new XElement[0];

            #endregion

            return _EVSEs.Select(evse => evse.ToEvseStatusRecords());

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSEIdAndStatus)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> EVSEIdAndStatus)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">

            // <v2:EvseStatusRecord>
            //    <v2:EvseId>?</v2:EvseId>
            //    <v2:EvseStatus>?</v2:EvseStatus>
            // </v2:EvseStatusRecord>

            #endregion

            #region Inital checks

            if (EVSEIdAndStatus == null)
                return new XElement[0];

            var _EVSEIdAndStatus = EVSEIdAndStatus.ToArray();

            if (_EVSEIdAndStatus.Length == 0)
                return new XElement[0];

            #endregion

            return _EVSEIdAndStatus.Select(kvp => new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                                                      new XElement(OICPNS.EVSEStatus + "EvseId",     kvp.Key.                     OriginId),
                                                      new XElement(OICPNS.EVSEStatus + "EvseStatus", kvp.Value.AsOICPEVSEStatus().ToString())
                                                  ));

        }

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<KeyValuePair<EVSE_Id, OICPEVSEStatus>> EVSEIdAndStatus)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">

            //             <v2:EvseStatusRecord>
            //                <v2:EvseId>?</v2:EvseId>
            //                <v2:EvseStatus>?</v2:EvseStatus>
            //             </v2:EvseStatusRecord>

            #endregion

            #region Inital checks

            if (EVSEIdAndStatus == null)
                return new XElement[0];

            var _EVSEIdAndStatus = EVSEIdAndStatus.ToArray();

            if (_EVSEIdAndStatus.Length == 0)
                return new XElement[0];

            #endregion

            return EVSEIdAndStatus.Select(kvp => new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                                                     new XElement(OICPNS.EVSEStatus + "EvseId",     kvp.Key.  OriginId),
                                                     new XElement(OICPNS.EVSEStatus + "EvseStatus", kvp.Value.ToString())
                                                 ));

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSEIds, CommonStatus)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<EVSE_Id>  EVSEIds,
                                                                  EVSEStatusType             CommonStatus)
        {

            #region Inital checks

            if (EVSEIds == null)
                return new XElement[0];

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                return new XElement[0];

            #endregion

            return EVSEIds.ToEvseStatusRecords(CommonStatus.AsOICPEVSEStatus());

        }

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<EVSE_Id>  EVSEIds,
                                                                  OICPEVSEStatus             CommonStatus)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">

            // <v2:EvseStatusRecord>
            //    <v2:EvseId>?</v2:EvseId>
            //    <v2:EvseStatus>?</v2:EvseStatus>
            // </v2:EvseStatusRecord>

            #endregion

            #region Inital checks

            if (EVSEIds == null)
                return new XElement[0];

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                return new XElement[0];

            #endregion

            return _EVSEIds.Select(evseid => new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                                                 new XElement(OICPNS.EVSEStatus + "EvseId",     evseid.      OriginId),
                                                 new XElement(OICPNS.EVSEStatus + "EvseStatus", CommonStatus.ToString())
                                             ));

        }

        #endregion



        #region AuthorizeStartXML(this EVSE, AuthToken, PartnerProductId = null, SessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP v2.0 Authorize Start XML request.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStartXML(this EVSE           EVSE,
                                                 Auth_Token          AuthToken,
                                                 String              PartnerProductId  = null,   // OICP v2.0: Optional [100]
                                                 ChargingSession_Id  SessionId         = null,   // OICP v2.0: Optional
                                                 ChargingSession_Id  PartnerSessionId  = null)   // OICP v2.0: Optional [50]
        {

            #region Initial checks

            if (EVSE      == null)
                throw new ArgumentNullException("EVSE",      "The given parameter must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException("AuthToken", "The given parameter must not be null!");

            #endregion

            return AuthorizeStartXML(EVSE.ChargingStation.ChargingPool.EVSEOperator.Id,
                                     AuthToken,
                                     EVSE.Id,
                                     PartnerProductId,
                                     SessionId,
                                     PartnerSessionId);

        }

        #endregion

        #region AuthorizeStartXML(OperatorId, AuthToken, EVSEId = null, PartnerProductId = null, SessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP v2.0 Authorize Start XML request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStartXML(EVSEOperator_Id     OperatorId,
                                                 Auth_Token          AuthToken,
                                                 EVSE_Id             EVSEId            = null,   // OICP v2.0: Optional
                                                 String              PartnerProductId  = null,   // OICP v2.0: Optional [100]
                                                 ChargingSession_Id  SessionId         = null,   // OICP v2.0: Optional
                                                 ChargingSession_Id  PartnerSessionId  = null)   // OICP v2.0: Optional [50]
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeStart",

                                          PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString())                 : null,

                                          new XElement(OICPNS.Authorization + "OperatorID",       OperatorId.ToFormat(IdFormatType.OLD)),

                                          EVSEId           != null ? new XElement(OICPNS.Authorization + "EVSEID",           EVSEId.          ToFormat(IdFormatType.OLD)) : null,

                                          new XElement(OICPNS.Authorization + "Identification",
                                              new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(OICPNS.CommonTypes + "UID", AuthToken.ToString())
                                              )
                                          )

                                     ));

        }

        #endregion


        #region AuthorizeStopXML(this EVSE, SessionId, AuthToken, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP v2.0 Authorize Stop XML request.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="SessionId">The session identification.</param>
        /// <param name="AuthToken">The (RFID) user identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStopXML(this EVSE           EVSE,
                                                ChargingSession_Id  SessionId,
                                                Auth_Token          AuthToken,
                                                ChargingSession_Id  PartnerSessionId = null)
        {

            #region Initial checks

            if (EVSE      == null)
                throw new ArgumentNullException("EVSE",      "The given parameter must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException("SessionId", "The given parameter must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException("AuthToken", "The given parameter must not be null!");

            #endregion

            return AuthorizeStopXML(EVSE.ChargingStation.ChargingPool.EVSEOperator.Id,
                                    SessionId,
                                    AuthToken,
                                    EVSE.Id,
                                    PartnerSessionId);

        }

        #endregion

        #region AuthorizeStopXML(OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP v2.0 Authorize Stop XML request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="SessionId">The session identification.</param>
        /// <param name="AuthToken">The (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStopXML(EVSEOperator_Id     OperatorId,
                                                ChargingSession_Id  SessionId,
                                                Auth_Token          AuthToken,
                                                EVSE_Id             EVSEId            = null,
                                                ChargingSession_Id  PartnerSessionId  = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeStop",

                                          new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),

                                          PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString())       : null,

                                          new XElement(OICPNS.Authorization + "OperatorID",       OperatorId.ToFormat(IdFormatType.OLD)),

                                          EVSEId           != null ? new XElement(OICPNS.Authorization + "EVSEID",           EVSEId.ToFormat(IdFormatType.OLD)) : null,

                                          new XElement(OICPNS.Authorization + "Identification",
                                              new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(OICPNS.CommonTypes + "UID", AuthToken.ToString())
                                              )
                                          )

                                      ));

        }

        #endregion


        #region SendChargeDetailRecordXML(this EVSE, SessionId, PartnerSessionId, AuthToken, EVCOId, ...)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">Your charging product identification.</param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">The partner session identification.</param>
        /// <param name="ChargingStart">The timestamp of the charging start.</param>
        /// <param name="ChargingEnd">The timestamp of the charging end.</param>
        /// <param name="MeterValueStart">The initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">The final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">Optional meter values during the charging session.</param>
        public static XElement SendChargeDetailRecordXML(this EVSE            EVSE,
                                                         ChargingSession_Id   SessionId,
                                                         String               PartnerProductId,
                                                         DateTime             SessionStart,
                                                         DateTime             SessionEnd,
                                                         Auth_Token           AuthToken             = null,
                                                         eMA_Id               eMAId                 = null,
                                                         ChargingSession_Id   PartnerSessionId      = null,
                                                         DateTime?            ChargingStart         = null,
                                                         DateTime?            ChargingEnd           = null,
                                                         Double?              MeterValueStart       = null,
                                                         Double?              MeterValueEnd         = null,
                                                         IEnumerable<Double>  MeterValuesInBetween  = null)

        {

            #region Initial checks

            if (EVSE             == null)
                throw new ArgumentNullException("EVSE",             "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",        "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId", "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",     "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",       "The given parameter must not be null!");

            #endregion

            return SendChargeDetailRecordXML(EVSE.Id,
                                             SessionId,
                                             PartnerProductId,
                                             SessionStart,
                                             SessionEnd,
                                             AuthToken,
                                             eMAId,
                                             PartnerSessionId,
                                             ChargingStart,
                                             ChargingEnd,
                                             MeterValueStart,
                                             MeterValueEnd);

        }

        #endregion

        #region SendChargeDetailRecordXML(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">An optional timestamp of the charging start.</param>
        /// <param name="ChargingEnd">An optional timestamp of the charging end.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        public static XElement SendChargeDetailRecordXML(EVSE_Id              EVSEId,
                                                         ChargingSession_Id   SessionId,
                                                         String               PartnerProductId,
                                                         DateTime             SessionStart,
                                                         DateTime             SessionEnd,
                                                         Auth_Token           AuthToken             = null,
                                                         eMA_Id               eMAId                 = null,
                                                         ChargingSession_Id   PartnerSessionId      = null,
                                                         DateTime?            ChargingStart         = null,
                                                         DateTime?            ChargingEnd           = null,
                                                         Double?              MeterValueStart       = null,
                                                         Double?              MeterValueEnd         = null,
                                                         IEnumerable<Double>  MeterValuesInBetween  = null,
                                                         Double?              ConsumedEnergy        = null,
                                                         String               MeteringSignature     = null,
                                                         EVSEOperator_Id      HubOperatorId         = null,
                                                         EVSP_Id              HubProviderId         = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException("EVSEId",            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException("SessionId",         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            if (AuthToken        == null &&
                eMAId            == null)
                throw new ArgumentNullException("AuthToken / eMAId", "At least one of the given parameters must not be null!");

            #endregion

            var _MeterValuesInBetween = MeterValuesInBetween.ToArray();

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecord",

                                 new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                                 new XElement(OICPNS.Authorization + "PartnerSessionID", (PartnerSessionId != null) ? PartnerSessionId.ToString() : ""),
                                 new XElement(OICPNS.Authorization + "PartnerProductID", PartnerProductId),
                                 new XElement(OICPNS.Authorization + "EvseID",           EVSEId.ToFormat(IdFormatType.OLD)),

                                 new XElement(OICPNS.Authorization + "Identification",
                                     (AuthToken != null)
                                         ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                new XElement(OICPNS.CommonTypes + "UID", AuthToken.ToString())
                                           )
                                         : new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                                new XElement(OICPNS.CommonTypes + "EVCOID", eMAId.ToString())
                                           )
                                 ),

                                 (ChargingStart.  HasValue) ? new XElement(OICPNS.Authorization + "ChargingStart",    ChargingStart.  Value.ToIso8601()) : null,
                                 (ChargingEnd.    HasValue) ? new XElement(OICPNS.Authorization + "ChargingEnd",      ChargingEnd.    Value.ToIso8601()) : null,
                                 new XElement(OICPNS.Authorization + "SessionStart", SessionStart),
                                 new XElement(OICPNS.Authorization + "SessionEnd",   SessionEnd),
                                 (MeterValueStart.HasValue) ? new XElement(OICPNS.Authorization + "MeterValueStart",  MeterValueStart.Value.ToString(CultureInfo.InvariantCulture.NumberFormat)) : null,
                                 (MeterValueEnd.  HasValue) ? new XElement(OICPNS.Authorization + "MeterValueEnd",    MeterValueEnd.  Value.ToString(CultureInfo.InvariantCulture.NumberFormat)) : null,

                                 _MeterValuesInBetween.Length > 0 ? new XElement(OICPNS.Authorization + "MeterValueInBetween",
                                                                        _MeterValuesInBetween.
                                                                            Select(value => new XElement(OICPNS.CommonTypes + "MeterValue", value.ToString(CultureInfo.InvariantCulture.NumberFormat))).
                                                                            ToArray()
                                                                    )
                                                                  : null,

                                 ConsumedEnergy    != null ? new XElement(OICPNS.Authorization + "ConsumedEnergy",    ConsumedEnergy.Value.ToString(CultureInfo.InvariantCulture.NumberFormat)) : null,
                                 MeteringSignature != null ? new XElement(OICPNS.Authorization + "MeteringSignature", MeteringSignature)        : null,
                                 HubOperatorId     != null ? new XElement(OICPNS.Authorization + "HubOperatorID",     HubOperatorId.ToString()) : null,
                                 HubProviderId     != null ? new XElement(OICPNS.Authorization + "HubProviderID",     HubProviderId.ToString()) : null

                             ));

        }

        #endregion


    }

}
