/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 CPO management operations.
    /// </summary>
    public static class WWCPCPOClient_XMLMethods
    {

        #region Data

        private static Regex HotlinePhoneNumberRegExpr = new Regex("\\+[^0-9]");

        #endregion


        #region PushEVSEDataXML(GroupedData,           OICPAction = fullLoad, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(ILookup<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                                               ActionType                                OICPAction    = ActionType.fullLoad,
                                               EVSEOperator_Id                           OperatorId    = null,
                                               String                                    OperatorName  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
            //                   xmlns:CommonTypes     = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEData:eRoamingPushEvseData>
            //
            //          <EVSEData:ActionType>?</EVSEData:ActionType>
            //
            //          <EVSEData:OperatorEvseData>
            //
            //             <EVSEData:OperatorID>?</EVSEData:OperatorID>
            //
            //             <!--Optional:-->
            //             <EVSEData:OperatorName>?</EVSEData:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <EVSEData:EvseDataRecord deltaType="?" lastUpdate="?">
            //                [...]
            //             </EVSEData:EvseDataRecord>
            //
            //          </EVSEData:OperatorEvseData>
            //       </EVSEData:eRoamingPushEvseData>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", OICPAction.ToString()),
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
                                              datagroup.SelectMany(evses => evses.Select(EVSE => EVSE.ToEvseDataRecordXML())).ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEDataXML(this EVSEOperator,     OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this EVSEOperator    EVSEOperator,
                                               ActionType           OICPAction    = ActionType.fullLoad,
                                               EVSEOperator_Id      OperatorId    = null,
                                               String               OperatorName  = null,
                                               Func<EVSE, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            #endregion

            return new EVSEOperator[] { EVSEOperator }.
                       PushEVSEDataXML(OICPAction,
                                       OperatorId,
                                       OperatorName,
                                       IncludeEVSEs);

        }

        #endregion

        #region PushEVSEDataXML(this EVSEOperators,    OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSEOperator>  EVSEOperators,
                                               ActionType                      OICPAction    = ActionType.fullLoad,
                                               EVSEOperator_Id                 OperatorId    = null,
                                               String                          OperatorName  = null,
                                               Func<EVSE, Boolean>             IncludeEVSEs  = null)
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
                                                                                        Where     (evse    => IncludeEVSEs(evse))),
                                   OICPAction,
                                   OperatorId,
                                   OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingPool,     OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this ChargingPool    ChargingPool,
                                               ActionType           OICPAction    = ActionType.insert,
                                               EVSEOperator_Id      OperatorId    = null,
                                               String               OperatorName  = null,
                                               Func<EVSE, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given parameter must not be null!");

            #endregion

            return new ChargingPool[] { ChargingPool }.
                       PushEVSEDataXML(OICPAction,
                                       OperatorId,
                                       OperatorName,
                                       IncludeEVSEs);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingPools,    OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingPool>  ChargingPools,
                                               ActionType                      OICPAction    = ActionType.insert,
                                               EVSEOperator_Id                 OperatorId    = null,
                                               String                          OperatorName  = null,
                                               Func<EVSE, Boolean>             IncludeEVSEs  = null)
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
                                                                        Where     (evse    => IncludeEVSEs(evse))),
                                   OICPAction,
                                   OperatorId,
                                   OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingStation,  OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this ChargingStation  ChargingStation,
                                               ActionType            OICPAction    = ActionType.insert,
                                               EVSEOperator_Id       OperatorId    = null,
                                               String                OperatorName  = null,
                                               Func<EVSE, Boolean>   IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return new ChargingStation[] { ChargingStation }.
                       PushEVSEDataXML(OICPAction,
                                       OperatorId,
                                       OperatorName,
                                       IncludeEVSEs);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingStations, OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingStation>  ChargingStations,
                                               ActionType                         OICPAction    = ActionType.insert,
                                               EVSEOperator_Id                    OperatorId    = null,
                                               String                             OperatorName  = null,
                                               Func<EVSE, Boolean>                IncludeEVSEs  = null)
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
                                                              station => station.Where(evse => IncludeEVSEs(evse))),
                                   OICPAction,
                                   OperatorId,
                                   OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this EVSE,             OICPAction = insert,   OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(this EVSE        EVSE,
                                               EVSEOperator_Id  OperatorId    = null,
                                               String           OperatorName  = null,
                                               ActionType       OICPAction    = ActionType.insert)
        {

            return new EVSE[] { EVSE }.
                       PushEVSEDataXML(OICPAction,
                                       OperatorId,
                                       OperatorName);

        }

        #endregion

        #region PushEVSEDataXML(this EVSEs,            OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSE>  EVSEs,
                                               ActionType              OICPAction    = ActionType.insert,
                                               EVSEOperator_Id         OperatorId    = null,
                                               String                  OperatorName  = null,
                                               Func<EVSE, Boolean>     IncludeEVSEs  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
            //                   xmlns:CommonTypes     = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEData:eRoamingPushEvseData>
            //
            //          <EVSEData:ActionType>?</EVSEData:ActionType>
            //
            //          <EVSEData:OperatorEvseData>
            //
            //             <EVSEData:OperatorID>?</EVSEData:OperatorID>
            //
            //             <!--Optional:-->
            //             <EVSEData:OperatorName>?</EVSEData:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <EVSEData:EvseDataRecord deltaType="?" lastUpdate="?">
            //                [...]
            //             </EVSEData:EvseDataRecord>
            //
            //          </EVSEData:OperatorEvseData>
            //       </EVSEData:eRoamingPushEvseData>
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

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", OICPAction.ToString()),
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
                                              Where (evse => IncludeEVSEs(evse)).
                                              Select(evse => evse.ToEvseDataRecordXML())

                                      )
                                  ));

        }

        #endregion

        #region PushEVSEDataXML(this EVSEDataRecord,   OICPAction = insert,   OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(this EVSEDataRecord  EVSEDataRecord,
                                               EVSEOperator_Id      OperatorId    = null,
                                               String               OperatorName  = null,
                                               ActionType           OICPAction    = ActionType.insert)
        {

            return new EVSEDataRecord[] { EVSEDataRecord }.
                       PushEVSEDataXML(OICPAction,
                                       OperatorId,
                                       OperatorName);

        }

        #endregion

        #region (internal) ToEvseDataRecords(this EVSE)

        internal static XElement ToEvseDataRecordXML(this EVSE EVSE)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">

            // <EVSEData:EvseDataRecord>
            //
            //    <EVSEData:EvseId>?</EVSEData:EvseId>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingStationId>?</EVSEData:ChargingStationId>
            //    <!--Optional:-->
            //    <EVSEData:ChargingStationName>?</EVSEData:ChargingStationName>
            //    <!--Optional:-->
            //    <EVSEData:EnChargingStationName>?</EVSEData:EnChargingStationName>
            //
            //    <EVSEData:Address>
            //       <CommonTypes:Country>?</CommonTypes:Country>
            //       <CommonTypes:City>?</CommonTypes:City>
            //       <CommonTypes:Street>?</CommonTypes:Street>
            //       <!--Optional:-->
            //       <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
            //       <!--Optional:-->
            //       <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
            //       <!--Optional:-->
            //       <CommonTypes:Floor>?</CommonTypes:Floor>
            //       <!--Optional:-->
            //       <CommonTypes:Region>?</CommonTypes:Region>
            //       <!--Optional:-->
            //       <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
            //    </EVSEData:Address>
            //
            //    <EVSEData:GeoCoordinates>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //
            //       <CommonTypes:Google>
            //          <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //       </CommonTypes:Google>
            //
            //       <CommonTypes:DecimalDegree>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DecimalDegree>
            //
            //       <CommonTypes:DegreeMinuteSeconds>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DegreeMinuteSeconds>
            //
            //    </EVSEData:GeoCoordinates>
            //
            //    <EVSEData:Plugs>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:Plug>?</EVSEData:Plug>
            //    </EVSEData:Plugs>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingFacilities>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ChargingFacility>?</EVSEData:ChargingFacility>
            //    </EVSEData:ChargingFacilities>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingModes>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ChargingMode>?</EVSEData:ChargingMode>
            //    </EVSEData:ChargingModes>
            //
            //    <EVSEData:AuthenticationModes>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:AuthenticationMode>?</EVSEData:AuthenticationMode>
            //    </EVSEData:AuthenticationModes>
            //
            //    <!--Optional:-->
            //    <EVSEData:MaxCapacity>?</EVSEData:MaxCapacity>
            //
            //    <!--Optional:-->
            //    <EVSEData:PaymentOptions>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:PaymentOption>?</EVSEData:PaymentOption>
            //    </EVSEData:PaymentOptions>
            //
            //    <EVSEData:Accessibility>?</EVSEData:Accessibility>
            //    <EVSEData:HotlinePhoneNum>?</EVSEData:HotlinePhoneNum>
            //
            //    <!--Optional:-->
            //    <EVSEData:AdditionalInfo>?</EVSEData:AdditionalInfo>
            //
            //    <!--Optional:-->
            //    <EVSEData:EnAdditionalInfo>?</EVSEData:EnAdditionalInfo>
            //
            //    <!--Optional:-->
            //    <EVSEData:GeoChargingPointEntrance>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //
            //       <CommonTypes:Google>
            //          <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //       </CommonTypes:Google>
            //
            //       <CommonTypes:DecimalDegree>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DecimalDegree>
            //
            //       <CommonTypes:DegreeMinuteSeconds>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DegreeMinuteSeconds>
            //
            //    </EVSEData:GeoChargingPointEntrance>
            //
            //    <EVSEData:IsOpen24Hours>?</EVSEData:IsOpen24Hours>
            //    <!--Optional:-->
            //    <EVSEData:OpeningTime>?</EVSEData:OpeningTime>
            //
            //    <!--Optional:-->
            //    <EVSEData:HubOperatorID>?</EVSEData:HubOperatorID>
            //
            //    <!--Optional:-->
            //    <EVSEData:ClearinghouseID>?</EVSEData:ClearinghouseID>
            //
            //    <EVSEData:IsHubjectCompatible>?</EVSEData:IsHubjectCompatible>
            //    <EVSEData:DynamicInfoAvailable>?</EVSEData:DynamicInfoAvailable>
            //
            // </EVSEData:EvseDataRecord>

            #endregion

            #region Inital checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given parameter must not be null!");

            #endregion

            return new XElement(OICPNS.EVSEData + "EvseDataRecord",

                new XElement(OICPNS.EVSEData + "EvseId",                EVSE.Id.OriginId),
                new XElement(OICPNS.EVSEData + "ChargingStationId",     EVSE.ChargingStation.Id.ToString()),
                new XElement(OICPNS.EVSEData + "ChargingStationName",   EVSE.ChargingStation.ChargingPool.Name[Languages.de].SubstringMax(50)),
                new XElement(OICPNS.EVSEData + "EnChargingStationName", EVSE.ChargingStation.ChargingPool.Name[Languages.en].SubstringMax(50)),

                new XElement(OICPNS.EVSEData + "Address",

                    new XElement(OICPNS.CommonTypes + "Country",          EVSE.ChargingStation.Address.Country.Alpha3Code),
                    new XElement(OICPNS.CommonTypes + "City",             EVSE.ChargingStation.Address.City.FirstText),
                    new XElement(OICPNS.CommonTypes + "Street",           EVSE.ChargingStation.Address.Street), // OICPEVSEData.0 requires at least 5 characters!

                    EVSE.ChargingStation.Address.PostalCode. IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "PostalCode", EVSE.ChargingStation.Address.PostalCode)
                        : null,

                    EVSE.ChargingStation.Address.HouseNumber.IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "HouseNum",   EVSE.ChargingStation.Address.HouseNumber)
                        : null,

                    EVSE.ChargingStation.Address.FloorLevel. IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "Floor",      EVSE.ChargingStation.Address.FloorLevel)
                        : null

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
                          EVSE.ChargingStation.AuthenticationModes.Select(AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode.ToString().Replace("_", " "))))
                    : null,

                EVSE.MaxCapacity_kWh > 0
                    ? new XElement(OICPNS.EVSEData + "MaxCapacity", EVSE.MaxCapacity_kWh)
                    : null,

                EVSE.ChargingStation.PaymentOptions.Any()
                    ? new XElement(OICPNS.EVSEData + "PaymentOptions",
                          EVSE.ChargingStation.PaymentOptions.Select(PaymentOption => new XElement(OICPNS.EVSEData + "PaymentOption", PaymentOption.ToString().Replace("_", " "))))
                    : null,

                new XElement(OICPNS.EVSEData + "Accessibility",   EVSE.ChargingStation.Accessibility.ToString().Replace("_", " ")),
                new XElement(OICPNS.EVSEData + "HotlinePhoneNum", HotlinePhoneNumberRegExpr.Replace(EVSE.ChargingStation.HotlinePhoneNumber, "")),  // RegEx: \+[0-9]{5,15}

                EVSE.Description.Any()
                    ? new XElement(OICPNS.EVSEData + "AdditionalInfo", EVSE.Description.First().Text)
                    : null,

                // ToDo: OICP v2.0 Multi-Language support
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

                new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   EVSE.ChargingStation.IsHubjectCompatible  ? "true" : "false"),
                new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  EVSE.ChargingStation.DynamicInfoAvailable ? "true" : "false")

            );

        }

        #endregion



        #region PushEVSEStatusXML(GroupedData,           OICPAction = update, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(Dictionary<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                                                 ActionType                                   OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                              OperatorId    = null,
                                                 String                                       OperatorName  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEData:eRoamingPushEvseStatus>
            //
            //          <EVSEData:ActionType>?</EVSEData:ActionType>
            //
            //          <EVSEData:OperatorEvseStatus>
            //
            //             <EVSEData:OperatorID>?</EVSEData:OperatorID>
            //
            //             <!--Optional:-->
            //             <EVSEData:OperatorName>?</EVSEData:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <EVSEData:EvseStatusRecord>
            //                <EVSEData:EvseId>?</EVSEData:EvseId>
            //                <EVSEData:EvseStatus>?</EVSEData:EvseStatus>
            //             </EVSEData:EvseStatusRecord>
            //
            //          </EVSEData:OperatorEvseStatus>
            //
            //       </EVSEData:eRoamingPushEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",

                                      new XElement(OICPNS.EVSEStatus + "ActionType", OICPAction.ToString()),

                                      GroupedData.Select(datagroup =>
                                          new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEStatus + "OperatorID", (OperatorId != null
                                                                                                 ? OperatorId
                                                                                                 : datagroup.Key.Id).OriginId),

                                              (OperatorName.IsNotNullOrEmpty() || datagroup.Key.Name.Any())
                                                  ? new XElement(OICPNS.EVSEStatus + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                         ? OperatorName
                                                                                                         : datagroup.Key.Name.First().Text))
                                                  : null,

                                              datagroup.Value.ToEvseStatusRecords().ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEOperator,     OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this EVSEOperator    EVSEOperator,
                                                 ActionType           OICPAction    = ActionType.update,
                                                 EVSEOperator_Id      OperatorId    = null,
                                                 String               OperatorName  = null,
                                                 Func<EVSE, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            #endregion

            return new EVSEOperator[] { EVSEOperator }.
                       PushEVSEStatusXML(OICPAction,
                                         OperatorId,
                                         OperatorName,
                                         IncludeEVSEs);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEOperators,    OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSEOperator>  EVSEOperators,
                                                 ActionType                      OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                 OperatorId    = null,
                                                 String                          OperatorName  = null,
                                                 Func<EVSE, Boolean>             IncludeEVSEs  = null)
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
                                                                                              Where     (evse    => IncludeEVSEs(evse))),
                                     OICPAction,
                                     OperatorId,
                                     OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingPool,     OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this ChargingPool    ChargingPool,
                                                 ActionType           OICPAction    = ActionType.update,
                                                 EVSEOperator_Id      OperatorId    = null,
                                                 String               OperatorName  = null,
                                                 Func<EVSE, Boolean>  IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given parameter must not be null!");

            #endregion

            return new ChargingPool[] { ChargingPool }.
                       PushEVSEStatusXML(OICPAction,
                                         OperatorId,
                                         OperatorName,
                                         IncludeEVSEs);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingPools,    OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<ChargingPool>  ChargingPools,
                                                 ActionType                      OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                 OperatorId    = null,
                                                 String                          OperatorName  = null,
                                                 Func<EVSE, Boolean>             IncludeEVSEs  = null)
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
                                                                              Where     (evse    => IncludeEVSEs(evse))),
                                     OICPAction,
                                     OperatorId,
                                     OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingStation,  OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this ChargingStation  ChargingStation,
                                                 ActionType            OICPAction    = ActionType.update,
                                                 EVSEOperator_Id       OperatorId    = null,
                                                 String                OperatorName  = null,
                                                 Func<EVSE, Boolean>   IncludeEVSEs  = null)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given parameter must not be null!");

            #endregion

            return new ChargingStation[] { ChargingStation }.
                       PushEVSEStatusXML(OICPAction,
                                         OperatorId,
                                         OperatorName,
                                         IncludeEVSEs);

        }

        #endregion

        #region PushEVSEStatusXML(this ChargingStations, OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<ChargingStation>  ChargingStations,
                                                 ActionType                         OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                    OperatorId    = null,
                                                 String                             OperatorName  = null,
                                                 Func<EVSE, Boolean>                IncludeEVSEs  = null)
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
                                                                    station => station.Where(evse => IncludeEVSEs(evse))),
                                     OICPAction,
                                     OperatorId,
                                     OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSE,             OICPAction = update, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(this EVSE        EVSE,
                                                 EVSEOperator_Id  OperatorId    = null,
                                                 String           OperatorName  = null,
                                                 ActionType       OICPAction    = ActionType.update)
        {

            return new EVSE[] { EVSE }.
                       PushEVSEStatusXML(OICPAction,
                                         OperatorId,
                                         OperatorName);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEs,            OICPAction = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE>  EVSEs,
                                                 ActionType              OICPAction    = ActionType.update,
                                                 EVSEOperator_Id         OperatorId    = null,
                                                 String                  OperatorName  = null,
                                                 Func<EVSE, Boolean>     IncludeEVSEs  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEData:eRoamingPushEvseStatus>
            //
            //          <EVSEData:ActionType>?</EVSEData:ActionType>
            //
            //          <EVSEData:OperatorEvseStatus>
            //
            //             <EVSEData:OperatorID>?</EVSEData:OperatorID>
            //
            //             <!--Optional:-->
            //             <EVSEData:OperatorName>?</EVSEData:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <EVSEData:EvseStatusRecord>
            //                <EVSEData:EvseId>?</EVSEData:EvseId>
            //                <EVSEData:EvseStatus>?</EVSEData:EvseStatus>
            //             </EVSEData:EvseStatusRecord>
            //
            //          </EVSEData:OperatorEvseStatus>
            //
            //       </EVSEData:eRoamingPushEvseStatus>
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
                                          new XElement(OICPNS.EVSEStatus + "ActionType", OICPAction.ToString()),
                                          new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEStatus + "OperatorID", (OperatorId != null
                                                                                                 ? OperatorId
                                                                                                 : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Id).OriginId),

                                              (OperatorName.IsNotNullOrEmpty() || _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.Any())
                                                  ? new XElement(OICPNS.EVSEStatus + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                         ? OperatorName
                                                                                                         : _EVSEs.First().ChargingStation.ChargingPool.EVSEOperator.Name.First().Text))
                                                  : null,

                                              _EVSEs.
                                                  Where(evse => IncludeEVSEs(evse)).
                                                  ToEvseStatusRecords().
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEIds, CommonStatus, OperatorId, OperatorName = null, OICPAction = update, IncludeEVSEIds = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE_Id>  EVSEIds,
                                                 EVSEStatusType             CommonStatus,
                                                 EVSEOperator_Id            OperatorId,
                                                 String                     OperatorName    = null,
                                                 ActionType                 OICPAction      = ActionType.update,
                                                 Func<EVSE_Id, Boolean>     IncludeEVSEIds  = null)

        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <EVSEStatus:eRoamingPushEvseStatus>
            //
            //          <EVSEStatus:ActionType>?</EVSEStatus:ActionType>
            //
            //          <EVSEStatus:OperatorEvseStatus>
            //
            //             <EVSEStatus:OperatorID>?</EVSEStatus:OperatorID>
            //
            //             <!--Optional:-->
            //             <EVSEStatus:OperatorName>?</EVSEStatus:OperatorName>
            //
            //             <!--Zero or more repetitions:-->
            //             <EVSEStatus:EvseStatusRecord>
            //                <EVSEStatus:EvseId>?</EVSEStatus:EvseId>
            //                <EVSEStatus:EvseStatus>?</EVSEStatus:EvseStatus>
            //             </EVSEStatus:EvseStatusRecord>
            //
            //          </EVSEStatus:OperatorEvseStatus>
            //
            //       </EVSEStatus:eRoamingPushEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEIds == null)
                throw new ArgumentNullException("EVSEIds", "The given parameter must not be null!");

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                throw new ArgumentNullException("EVSEIds", "The given parameter must not be empty!");

            if (IncludeEVSEIds == null)
                IncludeEVSEIds = EVSEId => true;

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",
                                          new XElement(OICPNS.EVSEStatus + "ActionType", OICPAction.ToString()),
                                          new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                              new XElement(OICPNS.EVSEStatus + "OperatorID", OperatorId.OriginId),

                                              OperatorName.IsNotNullOrEmpty()
                                                  ? new XElement(OICPNS.EVSEStatus + "OperatorName", OperatorName)
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
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">

            // <EVSEData:EvseStatusRecord>
            //    <EVSEData:EvseId>?</EVSEData:EvseId>
            //    <EVSEData:EvseStatus>?</EVSEData:EvseStatus>
            // </EVSEData:EvseStatusRecord>

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

        #region (internal) ToEvseStatusRecords(this EVSEStatusRecords)

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<EVSEStatusRecord> EVSEStatusRecords)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
            //
            // <EVSEStatus:EvseStatusRecord>
            //    <EVSEStatus:EvseId>?</EVSEData:EvseId>
            //    <EVSEStatus:EvseStatus>?</EVSEData:EvseStatus>
            // </EVSEStatus:EvseStatusRecord>

            #endregion

            #region Inital checks

            if (EVSEStatusRecords == null)
                return new XElement[0];

            var _EVSEStatusRecords = EVSEStatusRecords.ToArray();

            if (_EVSEStatusRecords.Length == 0)
                return new XElement[0];

            #endregion

            return _EVSEStatusRecords.Select(statusrecord => new XElement(OICPNS.EVSEStatus + "EvseStatusRecord",
                                                                 new XElement(OICPNS.EVSEStatus + "EvseId",     statusrecord.Id.    OriginId),
                                                                 new XElement(OICPNS.EVSEStatus + "EvseStatus", statusrecord.Status.ToString())
                                                             ));

        }

        internal static IEnumerable<XElement> ToEvseStatusRecords(this IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> EVSEIdAndStatus)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">

            //             <EVSEData:EvseStatusRecord>
            //                <EVSEData:EvseId>?</EVSEData:EvseId>
            //                <EVSEData:EvseStatus>?</EVSEData:EvseStatus>
            //             </EVSEData:EvseStatusRecord>

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
                                                                  WWCP.EVSEStatusType        CommonStatus)
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
                                                                  OICP_2_0.EVSEStatusType    CommonStatus)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">

            // <EVSEData:EvseStatusRecord>
            //    <EVSEData:EvseId>?</EVSEData:EvseId>
            //    <EVSEData:EvseStatus>?</EVSEData:EvseStatus>
            // </EVSEData:EvseStatusRecord>

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


    }

}
