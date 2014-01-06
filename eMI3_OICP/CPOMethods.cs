/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

#endregion

namespace org.emi3group.IO.OICP
{

    /// <summary>
    /// CPO management operations.
    /// </summary>
    public static class CPOMethods
    {

        #region PushEVSEDataXML(this EVSEOperator, Action = fullLoad, OperatorID = null, OperatorName = null)

        public static XElement PushEVSEDataXML(this EVSEOperator  EVSEOperator,
                                               ActionType         Action       = ActionType.fullLoad,
                                               String             OperatorID   = null,
                                               String             OperatorName = null)
        {

            return EVSEOperator.EVSPools.
                                SelectMany(Pool    => Pool.ChargingStations).
                                SelectMany(Station => Station.EVSEs).
                                PushEVSEDataXML((OperatorID   == null) ? EVSEOperator.Id.ToString()      : OperatorID,
                                             (OperatorName == null) ? EVSEOperator.Name[Languages.de] : OperatorName,
                                             Action);

        }

        #endregion

        #region PushEVSEDataXML(this EVSPools, OperatorID, OperatorName, Action = fullLoad)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSPool>  EVSPools,
                                               String                     OperatorID,
                                               String                     OperatorName,
                                               ActionType                 Action = ActionType.fullLoad)
        {

            return EVSPools.SelectMany(Pool    => Pool.ChargingStations).
                            SelectMany(Station => Station.EVSEs).
                            PushEVSEDataXML(OperatorID,
                                         OperatorName,
                                         Action);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingStations, OperatorID, OperatorName, Action = fullLoad)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingStation>  ChargingStations,
                                               String                             OperatorID,
                                               String                             OperatorName,
                                               ActionType                         Action = ActionType.fullLoad)
        {

            return ChargingStations.SelectMany(Station => Station.EVSEs).
                                    PushEVSEDataXML(OperatorID,
                                                 OperatorName,
                                                 Action);

        }

        #endregion

        #region PushEVSEDataXML(this EVSEs, OperatorID, OperatorName, Action = ActionType.fullLoad)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSE>  EVSEs,
                                               String                  OperatorID,
                                               String                  OperatorName,
                                               ActionType              Action = ActionType.fullLoad)
        {

            //if (Action == ActionType.fullLoad ||
            //    Action == ActionType.insert   ||
            //    Action == ActionType.update)

            return SOAP.Encapsulation(new XElement(NS.OICPv1EVSEData + "HubjectPushEvseData",
                                      new XElement(NS.OICPv1EVSEData + "ActionType", Action.ToString()),
                                      new XElement(NS.OICPv1EVSEData + "OperatorEvseData",

                                          new XElement(NS.OICPv1EVSEData + "OperatorID", OperatorID),
                                          (OperatorName != null) ?
                                          new XElement(NS.OICPv1EVSEData + "OperatorName", OperatorName) : null,

                                          // EVSE => EvseDataRecord
                                          //EVSPools.Select(EVSPool =>
                                          //EVSPool.ChargingStations.Select(ChargingStation =>
                                          //ChargingStation.EVSEs.Select(EVSE => {

                                          EVSEs.Select(EVSE => {

                                              try
                                              {

                                                  return new XElement(NS.OICPv1EVSEData + "EvseDataRecord",

                                                      new XElement(NS.OICPv1EVSEData + "EvseId",                 EVSE.Id.ToString()),
                                                      new XElement(NS.OICPv1EVSEData + "ChargingStationId",      EVSE.ChargingStation.Id.ToString()),
                                                      new XElement(NS.OICPv1EVSEData + "ChargingStationName",    EVSE.ChargingStation.Pool.Name.First().Value),
                                                      new XElement(NS.OICPv1EVSEData + "EnChargingStationName",  EVSE.ChargingStation.Pool.Name.First().Value),

                                                      new XElement(NS.OICPv1EVSEData + "Address",
                                                          new XElement(NS.OICPv1CommonTypes + "Country",     EVSE.ChargingStation.Pool.Address.Country.Alpha3Code),
                                                          new XElement(NS.OICPv1CommonTypes + "City",        EVSE.ChargingStation.Pool.Address.City),
                                                          new XElement(NS.OICPv1CommonTypes + "Street",      EVSE.ChargingStation.Pool.Address.Street),
                                                          new XElement(NS.OICPv1CommonTypes + "PostalCode",  EVSE.ChargingStation.Pool.Address.PostalCode),
                                                          new XElement(NS.OICPv1CommonTypes + "HouseNum",    EVSE.ChargingStation.Pool.Address.HouseNumber),
                                                          new XElement(NS.OICPv1CommonTypes + "Floor",       EVSE.ChargingStation.Pool.Address.FloorLevel)
                                                      // <!--Optional:-->
                                                      // <v11:Region>?</v11:Region>
                                                      // <!--Optional:-->
                                                      // <v11:TimeZone>?</v11:TimeZone>
                                                      ),

                                                      new XElement(NS.OICPv1EVSEData + "GeoCoordinates",
                                                          new XElement(NS.OICPv1CommonTypes + "DecimalDegree",
                                                              new XElement(NS.OICPv1CommonTypes + "Longitude", EVSE.ChargingStation.GeoLocation.Longitude),
                                                              new XElement(NS.OICPv1CommonTypes + "Latitude",  EVSE.ChargingStation.GeoLocation.Latitude)
                                                          )
                                                      ),

                                                      new XElement(NS.OICPv1EVSEData + "Plugs",
                                                          EVSE.SocketOutlets.Select(Outlet =>
                                                             new XElement(NS.OICPv1EVSEData + "Plug", HubjectMapper.MapToPlugType(Outlet)))
                                                      ),

                                                      new XElement(NS.OICPv1EVSEData + "ChargingFacilities",
                                                          EVSE.SocketOutlets.Select(Outlet =>
                                                             new XElement(NS.OICPv1EVSEData + "ChargingFacility", "Unspecified"))//Outlet.Plug.ToString()))

                                                             // 100 - 120V, 1-Phase ≤10A
                                                             // 100 - 120V, 1-Phase ≤16A
                                                             // 100 - 120V, 1-Phase ≤32A
                                                             // 200 - 240V, 1-Phase ≤10A
                                                             // 200 - 240V, 1-Phase ≤16A
                                                             // 200 - 240V, 1-Phase ≤32A
                                                             // 200 - 240V, 1-Phase >32A
                                                             // 380 - 480V, 3-Phase ≤16A
                                                             // 380 - 480V, 3-Phase ≤32A
                                                             // 380 - 480V, 3-Phase ≤63A
                                                             // Battery exchange
                                                             // Unspecified
                                                             // DC Charging ≤20kW
                                                             // DC Charging ≤50kW
                                                             // DC Charging >50kW

                                                      ),

                            //                    <!--Optional:-->
                                                      //               <v1:ChargingModes>
                                                      //                  <!--1 or more repetitions:-->
                                                      //                  <v1:ChargingMode>?</v1:ChargingMode>

                                                      // Mode_1      IEC 61851-1
                                                      // Mode_2      IEC 61851-1
                                                      // Mode_3      IEC 61851-1
                                                      // Mode_4      IEC 61851-1
                                                      // CHAdeMO     CHAdeMo Specification

                                                      //               </v1:ChargingModes>

                                                      new XElement(NS.OICPv1EVSEData + "AuthenticationModes",
                                                          new XElement(NS.OICPv1EVSEData + "AuthenticationMode", "NFC RFID Classic"),
                                                          new XElement(NS.OICPv1EVSEData + "AuthenticationMode", "NFC RFID DESFire"),
                                                          new XElement(NS.OICPv1EVSEData + "AuthenticationMode", "REMOTE")
                                                      // EVSE.SocketOutlets.Select(Outlet =>
                                                      //    new XElement(NS.OICPv1EVSEData + "AuthenticationMode", "Unspecified"))//Outlet.Plug.ToString()))
                                                      ),

                            //                    <!--Optional:-->
                                                      //               <v1:MaxCapacity>?</v1:MaxCapacity>

                                                      new XElement(NS.OICPv1EVSEData + "PaymentOptions",
                                                          new XElement(NS.OICPv1EVSEData + "PaymentOption", "Contract")
                                                      ),

                                                      new XElement(NS.OICPv1EVSEData + "Accessibility", "Free publicly accessible"),
                                                      new XElement(NS.OICPv1EVSEData + "HotlinePhoneNum", "0800 0670000"),

                            //                    <!--Optional:-->
                                                      //               <v1:AdditionalInfo>?</v1:AdditionalInfo>

                            //                    <!--Optional:-->
                                                      //               <v1:EnAdditionalInfo>?</v1:EnAdditionalInfo>

                            //                    <!--Optional:-->
                                                      //               <v1:GeoChargingPointEntrance>
                                                      //                  <v11:DecimalDegree>
                                                      //                     <v11:Longitude>?</v11:Longitude>
                                                      //                     <v11:Latitude>?</v11:Latitude>
                                                      //                  </v11:DecimalDegree>
                                                      //               </v1:GeoChargingPointEntrance>

                                                      new XElement(NS.OICPv1EVSEData + "IsOpen24Hours", "true"),

                            //                    <!--Optional:-->
                                                      //               <v1:OpeningTime>?</v1:OpeningTime>

                            //                    <!--Optional:-->
                                                      //               <v1:HubOperatorID>?</v1:HubOperatorID>

                            //                    <!--Optional:-->
                                                      //               <v1:ClearinghouseID>?</v1:ClearinghouseID>

                                                      new XElement(NS.OICPv1EVSEData + "IsHubjectCompatible",  "true"),
                                                      new XElement(NS.OICPv1EVSEData + "DynamicInfoAvailable", "true")

                                                  );

                                              }
                                              catch (Exception e)
                                              {
                                                  return null;
                                              }

                                          })
                                      )
                                  ));

        }

        #endregion

        #region PushEVSEDataXML(this EVSE, OperatorID, OperatorName, Action = insert)

        public static XElement PushEVSEDataXML(this EVSE   EVSE,
                                               String      OperatorID,
                                               String      OperatorName,
                                               ActionType  Action = ActionType.insert)
        {

            return new EVSE[1] { EVSE }.PushEVSEDataXML(OperatorID,
                                                     OperatorName,
                                                     Action);

        }

        #endregion


        #region PushEVSEStatusXML(this EVSEOperator, Action = fullLoad, OperatorID = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(this EVSEOperator  EVSEOperator,
                                                 ActionType         Action       = ActionType.fullLoad,
                                                 String             OperatorID   = null,
                                                 String             OperatorName = null)
        {

            return EVSEOperator.EVSPools.
                                SelectMany(pool    => pool.ChargingStations).
                                SelectMany(station => station.EVSEs).

                                PushEVSEStatusXML((OperatorID   == null) ? EVSEOperator.Id.ToString()      : OperatorID,
                                               (OperatorName == null) ? EVSEOperator.Name.First().Value : OperatorName,
                                                Action);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEs, OperatorID, OperatorName, Action)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE>  EVSEs,
                                                 String                  OperatorID,
                                                 String                  OperatorName,
                                                 ActionType              Action)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1EVSEStatus + "HubjectPushEvseStatus",
                                 new XElement(NS.OICPv1EVSEStatus + "ActionType", Action.ToString()),
                                 new XElement(NS.OICPv1EVSEStatus + "OperatorEvseStatus",

                                     new XElement(NS.OICPv1EVSEStatus + "OperatorID", OperatorID),
                                     (OperatorName != null) ?
                                     new XElement(NS.OICPv1EVSEStatus + "OperatorName", OperatorName) : null,

                                     EVSEs.Select(EVSE =>
                                         new XElement(NS.OICPv1EVSEStatus + "EvseStatusRecord",
                                             new XElement(NS.OICPv1EVSEStatus + "EvseId",     EVSE.Id.    ToString()),
                                             new XElement(NS.OICPv1EVSEStatus + "EvseStatus", EVSE.Status.ToString())
                                         )
                                     )

                                 )
                             ));

        }

        #endregion


        #region AuthorizeStartXML(this EVSE, PartnerSessionID, UID)

        public static XElement AuthorizeStartXML(this EVSE   EVSE,
                                                 String      PartnerSessionID,
                                                 String      UID)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1Authorization + "HubjectAuthorizeStart",
                                 new XElement(NS.OICPv1Authorization + "PartnerSessionID", PartnerSessionID),
                                 new XElement(NS.OICPv1Authorization + "OperatorID",       EVSE.ChargingStation.Pool.Operator.Id),
                                 new XElement(NS.OICPv1Authorization + "EVSEID",           EVSE.Id),
                                 new XElement(NS.OICPv1Authorization + "Identification",
                                     new XElement(NS.OICPv1CommonTypes + "RFIDdesfireIdentification",
                                     //new XElement(NS.OICPv1CommonTypes + "RFIDclassicIdentification",
                                        new XElement(NS.OICPv1CommonTypes + "UID", UID)
                                     )
                                 )
                             ));

        }

        #endregion

        #region AuthorizeStopXML(this EVSE, SessionID, PartnerSessionID, UID)

        public static XElement AuthorizeStopXML(this EVSE   EVSE,
                                                String      SessionID,
                                                String      PartnerSessionID,
                                                String      UID)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1Authorization + "HubjectAuthorizeStop",
                                 new XElement(NS.OICPv1Authorization + "SessionID",        SessionID),
                                 new XElement(NS.OICPv1Authorization + "PartnerSessionID", PartnerSessionID),
                                 new XElement(NS.OICPv1Authorization + "OperatorID",       EVSE.ChargingStation.Pool.Operator.Id),
                                 new XElement(NS.OICPv1Authorization + "EVSEID",           EVSE.Id),
                                 new XElement(NS.OICPv1Authorization + "Identification",
                                     new XElement(NS.OICPv1CommonTypes + "RFIDdesfireIdentification",
                                        new XElement(NS.OICPv1CommonTypes + "UID", UID)
                                     )
                                 )
                             ));

        }

        #endregion


        #region SendChargeDetailRecordXML(this EVSE, SessionID, PartnerSessionID, UID, ...)

        public static XElement SendChargeDetailRecordXML(this EVSE  EVSE,
                                                         String     SessionID,
                                                         String     PartnerSessionID,
                                                         String     PartnerProductID,
                                                         String     UID,
                                                         DateTime   ChargeStart,
                                                         DateTime   ChargeEnd,
                                                         DateTime?  SessionStart    = null,
                                                         DateTime?  SessionEnd      = null,
                                                         UInt64?    MeterValueStart = null,
                                                         UInt64?    MeterValueEnd   = null)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1Authorization + "HubjectChargeDetailRecord",

                                 new XElement(NS.OICPv1Authorization + "SessionID",        SessionID),
                                 new XElement(NS.OICPv1Authorization + "PartnerSessionID", PartnerSessionID),
                                 new XElement(NS.OICPv1Authorization + "PartnerProductID", PartnerProductID),
                                 new XElement(NS.OICPv1Authorization + "EvseID",           EVSE.Id.ToString()),

                                 new XElement(NS.OICPv1Authorization + "Identification",
                                     new XElement(NS.OICPv1CommonTypes + "RFIDdesfireIdentification",
                                        new XElement(NS.OICPv1CommonTypes + "UID", UID)
                                     )
                                 ),

                                 new XElement(NS.OICPv1Authorization + "ChargingStart",   ChargeStart),
                                 new XElement(NS.OICPv1Authorization + "ChargingEnd",     ChargeEnd),
                                 (SessionStart.   HasValue) ? new XElement(NS.OICPv1Authorization + "SessionStart",    SessionStart)    : null,
                                 (SessionEnd.     HasValue) ? new XElement(NS.OICPv1Authorization + "SessionEnd",      SessionEnd)      : null,
                                 (MeterValueStart.HasValue) ? new XElement(NS.OICPv1Authorization + "MeterValueStart", MeterValueStart) : null,
                                 (MeterValueEnd.  HasValue) ? new XElement(NS.OICPv1Authorization + "MeterValueEnd",   MeterValueStart) : null

                                 //new XElement(NS.OICPv1Authorization + "MeterValueInBetween",
                                 //    new XElement(NS.OICPv1CommonTypes + "MeterValue", "...")
                                 //),

                                 //new XElement(NS.OICPv1Authorization + "ConsumedEnergy",    "..."),
                                 //new XElement(NS.OICPv1Authorization + "MeteringSignature", "..."),
                                 //new XElement(NS.OICPv1Authorization + "HubOperatorID",     "..."),
                                 //new XElement(NS.OICPv1Authorization + "HubProviderID",     "...")

                             ));

        }

        #endregion

    }

}
