/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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
using System.Diagnostics;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// OICP v1.2 CPO management operations.
    /// </summary>
    public static class CPO_XMLMethods
    {

        #region PushEVSEDataXML(this GroupedData,      Action = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(ILookup<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                                               ActionType                                Action        = ActionType.fullLoad,
                                               EVSEOperator_Id                           OperatorId    = null,
                                               String                                    OperatorName  = null)
        {

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", Action.ToString()),
                                      GroupedData.Select(datagroup =>
                                          new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                              new XElement(OICPNS.EVSEData + "OperatorID",   (OperatorId   != null ? OperatorId   : datagroup.Key.Id).ToFormat(IdFormatType.OLD)),
                                              new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName != null ? OperatorName : datagroup.Key.Name.First().Text)),

                                              datagroup.SelectMany(v => v.ToEvseDataRecords()).ToArray()

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

                                          new XElement(OICPNS.EVSEData + "OperatorID",   (OperatorId   != null ? OperatorId   : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Id).ToFormat(IdFormatType.OLD)),
                                          new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName != null ? OperatorName : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.FirstOrDefault().Text)),
                                          _EVSEs.
                                              Where(evse => IncludeEVSEs(evse.Id)).
                                              ToEvseDataRecords().
                                              ToArray()

                                      )
                                  ));

        }

        #endregion

        #region (internal) ToEvseDataRecords(this EVSEs)

        internal static IEnumerable<XElement> ToEvseDataRecords(this IEnumerable<EVSE> EVSEs)
        {

            return EVSEs.Select(EVSE => {

                try
                {

                    return new XElement(OICPNS.EVSEData + "EvseDataRecord",

                        new XElement(OICPNS.EVSEData + "EvseId",                EVSE.Id.ToFormat(IdFormatType.OLD)),
                        new XElement(OICPNS.EVSEData + "ChargingStationId",     EVSE.ChargingStation.Id.ToString()),
                        new XElement(OICPNS.EVSEData + "ChargingStationName",   EVSE.ChargingStation.ChargingPool.Name[Languages.de].SubstringMax(50)),
                        new XElement(OICPNS.EVSEData + "EnChargingStationName", EVSE.ChargingStation.ChargingPool.Name[Languages.en].SubstringMax(50)),

                        new XElement(OICPNS.EVSEData + "Address",
                            new XElement(OICPNS.CommonTypes + "Country",        EVSE.ChargingStation.Address.Country.Alpha3Code),
                            new XElement(OICPNS.CommonTypes + "City",           EVSE.ChargingStation.Address.City),
                            new XElement(OICPNS.CommonTypes + "Street",         EVSE.ChargingStation.Address.Street), // OICPv1.2 requires at least 5 characters!
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

                        new XElement(OICPNS.EVSEData + "ChargingFacilities",
                            EVSE.SocketOutlets.Select(Outlet =>
                            {

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

                        ),

                        // <!--Optional:-->
                        // <v1:ChargingModes>
                        //     <!--1 or more repetitions:-->
                        // <v1:ChargingMode>?</v1:ChargingMode>

                        // Mode_1      IEC 61851-1
                        // Mode_2      IEC 61851-1
                        // Mode_3      IEC 61851-1
                        // Mode_4      IEC 61851-1
                        // CHAdeMO     CHAdeMo Specification

                        // </v1:ChargingModes>

                        new XElement(OICPNS.EVSEData + "AuthenticationModes",
                            new XElement(OICPNS.EVSEData + "AuthenticationMode", "NFC RFID Classic"),
                            new XElement(OICPNS.EVSEData + "AuthenticationMode", "NFC RFID DESFire"),
                            new XElement(OICPNS.EVSEData + "AuthenticationMode", "REMOTE"),
                        //new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "PnC"),
                            new XElement(OICPNS.EVSEData + "AuthenticationMode", "Direct Payment")
                        // EVSE.SocketOutlets.Select(Outlet =>
                        //    new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "Unspecified"))//Outlet.Plug.ToString()))
                        ),

                        // <!--Optional:-->
                        //               <v1:MaxCapacity>?</v1:MaxCapacity>

                        new XElement(OICPNS.EVSEData + "PaymentOptions",
                            new XElement(OICPNS.EVSEData + "PaymentOption", "Contract")
                        // ??????????????????????? SMS!
                        ),

                        new XElement(OICPNS.EVSEData + "Accessibility", "Free publicly accessible"),
                        new XElement(OICPNS.EVSEData + "HotlinePhoneNum", "+8000670000"),  // RegEx: \+[0-9]{5,15}

                        // <!--Optional:-->
                        // <v1:AdditionalInfo>?</v1:AdditionalInfo>

                        // <!--Optional:-->
                        // <v1:EnAdditionalInfo>?</v1:EnAdditionalInfo>

                        // <!--Optional:-->
                        // <v1:GeoChargingPointEntrance>
                        //    <v11:DecimalDegree>
                        //       <v11:Longitude>?</v11:Longitude>
                        //       <v11:Latitude>?</v11:Latitude>
                        //    </v11:DecimalDegree>
                        // </v1:GeoChargingPointEntrance>

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
                catch (Exception e)
                {
                    Debug.WriteLine("Exception in CPOMethods: " + e.Message);
                    return null;
                }

            });

        }

        #endregion


        #region PushEVSEStatusXML(this GroupedData,      Action = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(Dictionary<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                                                 ActionType                                   Action        = ActionType.update,
                                                 EVSEOperator_Id                              OperatorId    = null,
                                                 String                                       OperatorName  = null)
        {

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseStatus",
                                      new XElement(OICPNS.EVSEData + "ActionType", Action.ToString()),
                                      GroupedData.Select(datagroup =>
                                          new XElement(OICPNS.EVSEData + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEData + "OperatorID",   (OperatorId   != null ? OperatorId   : datagroup.Key.Id).ToFormat(IdFormatType.OLD)),
                                              new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName != null ? OperatorName : datagroup.Key.Name.First().Text)),

                                              datagroup.Value.ToEvseDataRecords().ToArray()

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

                                              new XElement(OICPNS.EVSEStatus + "OperatorID",   (OperatorId   != null ? OperatorId   : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Id).ToFormat(IdFormatType.OLD)),
                                              new XElement(OICPNS.EVSEStatus + "OperatorName", (OperatorName != null ? OperatorName : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.FirstOrDefault().Text)),

                                              _EVSEs.
                                                  Where(evse => IncludeEVSEs(evse.Id)).
                                                  ToEvseStatusRecords().
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEIdAndStatus,       OperatorId, OperatorName, Action = update, IncludeEVSEIds = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEIdAndStatus,
                                                 EVSEOperator_Id                                          OperatorId,
                                                 String                                                   OperatorName,
                                                 ActionType                                               Action          = ActionType.update,
                                                 Func<EVSE_Id, Boolean>                                   IncludeEVSEIds  = null)
        {

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

                                              new XElement(OICPNS.EVSEStatus + "OperatorID",   OperatorId.ToFormat(IdFormatType.OLD)),
                                              new XElement(OICPNS.EVSEStatus + "OperatorName", OperatorName),

                                              _EVSEIdAndStatus.
                                                  Where(kvp => IncludeEVSEIds(kvp.Key)).
                                                  ToEvseStatusRecords().
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEIds, CommonStatus, OperatorId, OperatorName, Action = update, IncludeEVSEIds = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE_Id>  EVSEIds,
                                                 EVSEStatusType             CommonStatus,
                                                 EVSEOperator_Id            OperatorId,
                                                 String                     OperatorName,
                                                 ActionType                 Action          = ActionType.update,
                                                 Func<EVSE_Id, Boolean>     IncludeEVSEIds  = null)

        {

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

                                              new XElement(OICPNS.EVSEStatus + "OperatorID",   OperatorId.ToFormat(IdFormatType.OLD)),
                                              new XElement(OICPNS.EVSEStatus + "OperatorName", OperatorName),

                                              _EVSEIds.
                                                  Where(evseid => IncludeEVSEIds(evseid)).
                                                  ToEvseStatusRecords(CommonStatus).
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSEs)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<EVSE> EVSEs)
        {

            return EVSEs.Select(evse => {

                try
                {

                    return new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                        new XElement(OICPNS.EVSEStatus + "EvseId",     evse.Id.                               ToFormat(IdFormatType.OLD)),
                        new XElement(OICPNS.EVSEStatus + "EvseStatus", evse.Status.Value.AsHubjectEVSEState().ToString())
                    );

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception in CPOMethods: " + e.Message);
                    return null;
                }

            });

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSEIdAndStatus)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> EVSEIdAndStatus)
        {

            return EVSEIdAndStatus.Select(kvp => {

                try
                {

                    return new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                        new XElement(OICPNS.EVSEStatus + "EvseId",     kvp.Key.                       ToFormat(IdFormatType.OLD)),
                        new XElement(OICPNS.EVSEStatus + "EvseStatus", kvp.Value.AsHubjectEVSEState().ToString())
                    );

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception in CPOMethods: " + e.Message);
                    return null;
                }

            });

        }

        #endregion

        #region (internal) ToEvseStatusRecords(this EVSEIds, CommonStatus)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<EVSE_Id>  EVSEIds,
                                                                  EVSEStatusType             CommonStatus)
        {

            return EVSEIds.Select(evseid => {

                try
                {

                    return new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                        new XElement(OICPNS.EVSEStatus + "EvseId",     evseid.                           ToFormat(IdFormatType.OLD)),
                        new XElement(OICPNS.EVSEStatus + "EvseStatus", CommonStatus.AsHubjectEVSEState().ToString())
                    );

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception in CPOMethods: " + e.Message);
                    return null;
                }

            });

        }

        #endregion



        #region AuthorizeStartXML(this EVSE, AuthToken, PartnerProductId = null, HubjectSessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP authorize start XML request.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional Hubject session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStartXML(this EVSE           EVSE,
                                                 Auth_Token          AuthToken,
                                                 String              PartnerProductId  = null,   // OICP v1.2: Optional [100]
                                                 ChargingSession_Id  HubjectSessionId  = null,   // OICP v1.2: Optional
                                                 ChargingSession_Id  PartnerSessionId  = null)   // OICP v1.2: Optional [50]
        {

            return AuthorizeStartXML(EVSE.ChargingStation.ChargingPool.EVSEOperator.Id,
                                     AuthToken,
                                     EVSE.Id,
                                     PartnerProductId,
                                     HubjectSessionId,
                                     PartnerSessionId);

        }

        #endregion

        #region AuthorizeStartXML(OperatorId, AuthToken, EVSEId = null, PartnerProductId = null, HubjectSessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP authorize start XML request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="HubjectSessionId">An optional Hubject session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStartXML(EVSEOperator_Id     OperatorId,
                                                 Auth_Token          AuthToken,
                                                 EVSE_Id             EVSEId            = null,   // OICP v1.2: Optional
                                                 String              PartnerProductId  = null,   // OICP v1.2: Optional [100]
                                                 ChargingSession_Id  HubjectSessionId  = null,   // OICP v1.2: Optional
                                                 ChargingSession_Id  PartnerSessionId  = null)   // OICP v1.2: Optional [50]
        {

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


        #region AuthorizeStopXML(this EVSE, SessionID, PartnerSessionID, UID)

        /// <summary>
        /// Create an OICP authorize stop XML request.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="SessionID">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionID">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public static XElement AuthorizeStopXML(this EVSE          EVSE,
                                                ChargingSession_Id  SessionID,
                                                ChargingSession_Id  PartnerSessionID,
                                                Auth_Token              UID)
        {

            return AuthorizeStopXML(EVSE.ChargingStation.ChargingPool.EVSEOperator.Id,
                                    EVSE.Id,
                                    SessionID,
                                    PartnerSessionID,
                                    UID);

        }

        #endregion

        #region AuthorizeStopXML(OperatorId, EVSEId, SessionID, PartnerSessionID, UID)

        /// <summary>
        /// Create an OICP authorize stop XML request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="UID">A RFID user identification.</param>
        public static XElement AuthorizeStopXML(EVSEOperator_Id    OperatorId,
                                                EVSE_Id            EVSEId,
                                                ChargingSession_Id  SessionId,
                                                ChargingSession_Id  PartnerSessionId,
                                                Auth_Token              UID)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeStop",
                                          new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                                          new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()),
                                          new XElement(OICPNS.Authorization + "OperatorID",       OperatorId.ToFormat(IdFormatType.OLD)),
                                          new XElement(OICPNS.Authorization + "EVSEID",           EVSEId.    ToFormat(IdFormatType.OLD)),
                                          new XElement(OICPNS.Authorization + "Identification",
                                              new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(OICPNS.CommonTypes + "UID", UID.ToString())
                                              )
                                          )
                                      ));

        }

        #endregion


        #region SendChargeDetailRecordXML(this EVSE, SessionId, PartnerSessionId, UID, EVCOId, ...)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="PartnerProductId">Your charging product identification.</param>
        /// <param name="ChargeStart">The timestamp of the charging start.</param>
        /// <param name="ChargeEnd">The timestamp of the charging end.</param>
        /// <param name="UID">The optional RFID user identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="MeterValueStart">The initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">The final value of the energy meter.</param>
        public static XElement SendChargeDetailRecordXML(this EVSE           EVSE,
                                                         ChargingSession_Id  SessionId,
                                                         ChargingSession_Id  PartnerSessionId,
                                                         String              PartnerProductId,
                                                         DateTime            ChargeStart,
                                                         DateTime            ChargeEnd,
                                                         Auth_Token          UID              = null,
                                                         eMA_Id              EVCOId           = null,
                                                         DateTime?           SessionStart     = null,
                                                         DateTime?           SessionEnd       = null,
                                                         Double?             MeterValueStart  = null,
                                                         Double?             MeterValueEnd    = null)

        {

            return SendChargeDetailRecordXML(EVSE.Id,
                                             SessionId,
                                             PartnerSessionId,
                                             PartnerProductId,
                                             ChargeStart,
                                             ChargeEnd,
                                             UID,
                                             EVCOId,
                                             SessionStart,
                                             SessionEnd,
                                             MeterValueStart,
                                             MeterValueEnd);

        }

        #endregion

        #region SendChargeDetailRecordXML(EVSEId, SessionId, PartnerSessionId, UID, EVCOId, ...)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerSessionId">Your own session identification.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="ChargeStart">The timestamp of the charging start.</param>
        /// <param name="ChargeEnd">The timestamp of the charging end.</param>
        /// <param name="UID">The optional RFID user identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="MeterValueStart">The initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">The final value of the energy meter.</param>
        public static XElement SendChargeDetailRecordXML(EVSE_Id             EVSEId,
                                                         ChargingSession_Id  SessionId,
                                                         ChargingSession_Id  PartnerSessionId,
                                                         String              PartnerProductId,
                                                         DateTime            ChargeStart,
                                                         DateTime            ChargeEnd,
                                                         Auth_Token          UID              = null,
                                                         eMA_Id              EVCOId           = null,
                                                         DateTime?           SessionStart     = null,
                                                         DateTime?           SessionEnd       = null,
                                                         Double?             MeterValueStart  = null,
                                                         Double?             MeterValueEnd    = null)

        {

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecord",

                                 new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                                 new XElement(OICPNS.Authorization + "PartnerSessionID", (PartnerSessionId != null) ? PartnerSessionId.ToString() : ""),
                                 new XElement(OICPNS.Authorization + "PartnerProductID", PartnerProductId),
                                 new XElement(OICPNS.Authorization + "EvseID",           EVSEId.ToFormat(IdFormatType.OLD)),

                                 new XElement(OICPNS.Authorization + "Identification",
                                     (UID != null)
                                         ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                new XElement(OICPNS.CommonTypes + "UID", UID.ToString())
                                           )
                                         : new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                                                new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString())
                                           )
                                 ),

                                 new XElement(OICPNS.Authorization + "ChargingStart",   ChargeStart),  // "2014-02-01T15:45:00+02:00"
                                 new XElement(OICPNS.Authorization + "ChargingEnd",     ChargeEnd),
                                 (SessionStart.   HasValue) ? new XElement(OICPNS.Authorization + "SessionStart",    SessionStart)    : null,
                                 (SessionEnd.     HasValue) ? new XElement(OICPNS.Authorization + "SessionEnd",      SessionEnd)      : null,
                                 (MeterValueStart.HasValue) ? new XElement(OICPNS.Authorization + "MeterValueStart", MeterValueStart) : null,
                                 (MeterValueEnd.  HasValue) ? new XElement(OICPNS.Authorization + "MeterValueEnd",   MeterValueEnd)   : null

                                 //new XElement(NS.OICPv1_2Authorization + "MeterValueInBetween",
                                 //    new XElement(NS.OICPv1_2CommonTypes + "MeterValue", "...")
                                 //),

                                 //new XElement(NS.OICPv1_2Authorization + "ConsumedEnergy",    "..."),
                                 //new XElement(NS.OICPv1_2Authorization + "MeteringSignature", "..."),
                                 //new XElement(NS.OICPv1_2Authorization + "HubOperatorID",     "..."),
                                 //new XElement(NS.OICPv1_2Authorization + "HubProviderID",     "...")

                             ));

        }

        #endregion

    }

}
