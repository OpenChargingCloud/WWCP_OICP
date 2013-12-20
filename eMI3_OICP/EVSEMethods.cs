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
using de.eMI3.IO.OICP;

#endregion

namespace org.emi3group.IO.OICP
{

    #region ActionType

    /// <summary>
    /// The type of action when updating remote data.
    /// </summary>
    public enum ActionType
    {
        fullLoad,
        update,
        insert,
        delete
    }

    #endregion

    /// <summary>
    /// EVSE management operations.
    /// </summary>
    public static class EVSEMethods
    {

        #region Namespace

        /// <summary>
        /// The namespace for XML SOAP.
        /// </summary>
        public static readonly XNamespace NS_SOAPEnv              = "http://schemas.xmlsoap.org/soap/envelope/";

        /// <summary>
        /// The namespace for the common types within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace NS_OICPv1CommonTypes    = "http://www.hubject.com/b2b/services/commontypes/v1";

        /// <summary>
        /// The namespace for the EVSE Data within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace NS_OICPv1EVSEData       = "http://www.hubject.com/b2b/services/evsedata/v1";

        /// <summary>
        /// The namespace for the EVSE Status within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace NS_OICPv1EVSEStatus     = "http://www.hubject.com/b2b/services/evsestatus/v1";

        /// <summary>
        /// The namespace for the Authorization within the Open Intercharge Protocol (OICP) Version 1.0.
        /// </summary>
        public static readonly XNamespace NS_OICPv1Authorization  = "http://www.hubject.com/b2b/services/authorization/v1";

        #endregion

        #region Encapsulate(EmbeddedXML)

        /// <summary>
        /// Encapsulate the given XML Soap data within a SOAP frame.
        /// </summary>
        /// <param name="EmbeddedXML"></param>
        /// <returns></returns>
        public static XElement Encapsulate(XElement EmbeddedXML)
        {

            return new XElement(NS_SOAPEnv + "Envelope",
                       new XAttribute(XNamespace.Xmlns + "eMI3",            NS_SOAPEnv.            NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "CommonTypes",     NS_OICPv1CommonTypes.  NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSEData",        NS_OICPv1EVSEData.     NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "EVSEStatus",      NS_OICPv1EVSEStatus.   NamespaceName),
                       new XAttribute(XNamespace.Xmlns + "Authorization",   NS_OICPv1Authorization.NamespaceName),

                       new XElement(NS_SOAPEnv + "Header"),
                       new XElement(NS_SOAPEnv + "Body", EmbeddedXML));

        }

        #endregion


        #region PushEVSEData(this EVSEOperator, Action = fullLoad, OperatorID = null, OperatorName = null)

        public static XElement PushEVSEData(this EVSEOperator  EVSEOperator,
                                            ActionType         Action       = ActionType.fullLoad,
                                            String             OperatorID   = null,
                                            String             OperatorName = null)
        {

            return EVSEOperator.EVSPools.
                                SelectMany(Pool    => Pool.ChargingStations).
                                SelectMany(Station => Station.EVSEs).
                                PushEVSEData((OperatorID   == null) ? EVSEOperator.Id.ToString()      : OperatorID,
                                             (OperatorName == null) ? EVSEOperator.Name[Languages.de] : OperatorName,
                                             Action);

        }

        #endregion

        #region PushEVSEData(this EVSPools, OperatorID, OperatorName, Action = fullLoad)

        public static XElement PushEVSEData(this IEnumerable<EVSPool>  EVSPools,
                                            String                     OperatorID,
                                            String                     OperatorName,
                                            ActionType                 Action = ActionType.fullLoad)
        {

            return EVSPools.SelectMany(Pool    => Pool.ChargingStations).
                            SelectMany(Station => Station.EVSEs).
                            PushEVSEData(OperatorID,
                                         OperatorName,
                                         Action);

        }

        #endregion

        #region PushEVSEData(this ChargingStations, OperatorID, OperatorName, Action = fullLoad)

        public static XElement PushEVSEData(this IEnumerable<ChargingStation>  ChargingStations,
                                            String                             OperatorID,
                                            String                             OperatorName,
                                            ActionType                         Action = ActionType.fullLoad)
        {

            return ChargingStations.SelectMany(Station => Station.EVSEs).
                                    PushEVSEData(OperatorID,
                                                 OperatorName,
                                                 Action);

        }

        #endregion

        #region PushEVSEData(this EVSEs, OperatorID, OperatorName, Action = ActionType.fullLoad)

        public static XElement PushEVSEData(this IEnumerable<EVSE>  EVSEs,
                                            String                  OperatorID,
                                            String                  OperatorName,
                                            ActionType              Action = ActionType.fullLoad)
        {

            //if (Action == ActionType.fullLoad ||
            //    Action == ActionType.insert   ||
            //    Action == ActionType.update)

            return Encapsulate(new XElement(NS_OICPv1EVSEData + "HubjectPushEvseData",
                                 new XElement(NS_OICPv1EVSEData + "ActionType", Action.ToString()),
                                 new XElement(NS_OICPv1EVSEData + "OperatorEvseData",

                                     new XElement(NS_OICPv1EVSEData + "OperatorID", OperatorID),
                                     (OperatorName != null) ?
                                     new XElement(NS_OICPv1EVSEData + "OperatorName", OperatorName) : null,

                                     // EVSE => EvseDataRecord
                                     //EVSPools.Select(EVSPool =>
                                     //EVSPool.ChargingStations.Select(ChargingStation =>
                                     //ChargingStation.EVSEs.Select(EVSE => {

                                     EVSEs.Select(EVSE => {

                                         try
                                         {

                                             return new XElement(NS_OICPv1EVSEData + "EvseDataRecord",

                                                 new XElement(NS_OICPv1EVSEData + "EvseId",                 EVSE.Id.ToString()),
                                                 new XElement(NS_OICPv1EVSEData + "ChargingStationId",      EVSE.ChargingStation.Id.ToString()),
                                                 new XElement(NS_OICPv1EVSEData + "ChargingStationName",    EVSE.ChargingStation.Pool.Name.First().Value),
                                                 new XElement(NS_OICPv1EVSEData + "EnChargingStationName",  EVSE.ChargingStation.Pool.Name.First().Value),

                                                 new XElement(NS_OICPv1EVSEData + "Address",
                                                     new XElement(NS_OICPv1CommonTypes + "Country",     EVSE.ChargingStation.Pool.Address.Country.Alpha3Code),
                                                     new XElement(NS_OICPv1CommonTypes + "City",        EVSE.ChargingStation.Pool.Address.City),
                                                     new XElement(NS_OICPv1CommonTypes + "Street",      EVSE.ChargingStation.Pool.Address.Street),
                                                     new XElement(NS_OICPv1CommonTypes + "PostalCode",  EVSE.ChargingStation.Pool.Address.PostalCode),
                                                     new XElement(NS_OICPv1CommonTypes + "HouseNum",    EVSE.ChargingStation.Pool.Address.HouseNumber),
                                                     new XElement(NS_OICPv1CommonTypes + "Floor",       EVSE.ChargingStation.Pool.Address.FloorLevel)
                                                 // <!--Optional:-->
                                                 // <v11:Region>?</v11:Region>
                                                 // <!--Optional:-->
                                                 // <v11:TimeZone>?</v11:TimeZone>
                                                 ),

                                                 new XElement(NS_OICPv1EVSEData + "GeoCoordinates",
                                                     new XElement(NS_OICPv1CommonTypes + "DecimalDegree",
                                                         new XElement(NS_OICPv1CommonTypes + "Longitude", EVSE.ChargingStation.GeoLocation.Longitude),
                                                         new XElement(NS_OICPv1CommonTypes + "Latitude",  EVSE.ChargingStation.GeoLocation.Latitude)
                                                     )
                                                 ),

                                                 new XElement(NS_OICPv1EVSEData + "Plugs",
                                                     EVSE.SocketOutlets.Select(Outlet =>
                                                        new XElement(NS_OICPv1EVSEData + "Plug", HubjectMapper.MapToPlugType(Outlet)))
                                                 ),

                                                 new XElement(NS_OICPv1EVSEData + "ChargingFacilities",
                                                     EVSE.SocketOutlets.Select(Outlet =>
                                                        new XElement(NS_OICPv1EVSEData + "ChargingFacility", "Unspecified"))//Outlet.Plug.ToString()))

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

                            //               <!--Optional:-->
                                                 //               <v1:ChargingModes>
                                                 //                  <!--1 or more repetitions:-->
                                                 //                  <v1:ChargingMode>?</v1:ChargingMode>

                                                 // Mode_1      IEC 61851-1
                                                 // Mode_2      IEC 61851-1
                                                 // Mode_3      IEC 61851-1
                                                 // Mode_4      IEC 61851-1
                                                 // CHAdeMO     CHAdeMo Specification

                                                 //               </v1:ChargingModes>

                                                 new XElement(NS_OICPv1EVSEData + "AuthenticationModes",
                                                     new XElement(NS_OICPv1EVSEData + "AuthenticationMode", "NFC RFID Classic"),
                                                     new XElement(NS_OICPv1EVSEData + "AuthenticationMode", "NFC RFID DESFire"),
                                                     new XElement(NS_OICPv1EVSEData + "AuthenticationMode", "REMOTE")
                                                 // EVSE.SocketOutlets.Select(Outlet =>
                                                 //    new XElement(NS_OICPv1EVSEData + "AuthenticationMode", "Unspecified"))//Outlet.Plug.ToString()))
                                                 ),

                            //               <!--Optional:-->
                                                 //               <v1:MaxCapacity>?</v1:MaxCapacity>

                                                 new XElement(NS_OICPv1EVSEData + "PaymentOptions",
                                                     new XElement(NS_OICPv1EVSEData + "PaymentOption", "Contract")
                                                 ),

                                                 new XElement(NS_OICPv1EVSEData + "Accessibility", "Free publicly accessible"),
                                                 new XElement(NS_OICPv1EVSEData + "HotlinePhoneNum", "0800 0670000"),

                            //               <!--Optional:-->
                                                 //               <v1:AdditionalInfo>?</v1:AdditionalInfo>

                            //               <!--Optional:-->
                                                 //               <v1:EnAdditionalInfo>?</v1:EnAdditionalInfo>

                            //               <!--Optional:-->
                                                 //               <v1:GeoChargingPointEntrance>
                                                 //                  <v11:DecimalDegree>
                                                 //                     <v11:Longitude>?</v11:Longitude>
                                                 //                     <v11:Latitude>?</v11:Latitude>
                                                 //                  </v11:DecimalDegree>
                                                 //               </v1:GeoChargingPointEntrance>

                                                 new XElement(NS_OICPv1EVSEData + "IsOpen24Hours", "true"),

                            //               <!--Optional:-->
                                                 //               <v1:OpeningTime>?</v1:OpeningTime>

                            //               <!--Optional:-->
                                                 //               <v1:HubOperatorID>?</v1:HubOperatorID>

                            //               <!--Optional:-->
                                                 //               <v1:ClearinghouseID>?</v1:ClearinghouseID>

                                                 new XElement(NS_OICPv1EVSEData + "IsHubjectCompatible",  "true"),
                                                 new XElement(NS_OICPv1EVSEData + "DynamicInfoAvailable", "true")

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

        #region PushEVSEData(this EVSE, OperatorID, OperatorName, Action = insert)

        public static XElement PushEVSEData(this EVSE   EVSE,
                                            String      OperatorID,
                                            String      OperatorName,
                                            ActionType  Action = ActionType.insert)
        {

            return new EVSE[1] { EVSE }.PushEVSEData(OperatorID,
                                                     OperatorName,
                                                     Action);

        }

        #endregion


        #region PushEVSEStatus(this EVSEOperator, Action = fullLoad, OperatorID = null, OperatorName = null)

        public static XElement PushEVSEStatus(this EVSEOperator  EVSEOperator,
                                              ActionType         Action       = ActionType.fullLoad,
                                              String             OperatorID   = null,
                                              String             OperatorName = null)
        {

            return EVSEOperator.EVSPools.
                                SelectMany(pool    => pool.ChargingStations).
                                SelectMany(station => station.EVSEs).

                                PushEVSEStatus((OperatorID   == null) ? EVSEOperator.Id.ToString()      : OperatorID,
                                               (OperatorName == null) ? EVSEOperator.Name.First().Value : OperatorName,
                                                Action);

        }

        #endregion

        #region PushEVSEStatus(this EVSEs, OperatorID, OperatorName, Action)

        public static XElement PushEVSEStatus(this IEnumerable<EVSE>  EVSEs,
                                              String                  OperatorID,
                                              String                  OperatorName,
                                              ActionType              Action)
        {

            return Encapsulate(new XElement(NS_OICPv1EVSEStatus + "HubjectPushEvseStatus",
                                 new XElement(NS_OICPv1EVSEStatus + "ActionType", Action.ToString()),
                                 new XElement(NS_OICPv1EVSEStatus + "OperatorEvseStatus",

                                     new XElement(NS_OICPv1EVSEStatus + "OperatorID", OperatorID),
                                     (OperatorName != null) ?
                                     new XElement(NS_OICPv1EVSEStatus + "OperatorName", OperatorName) : null,

                                     EVSEs.Select(EVSE =>
                                         new XElement(NS_OICPv1EVSEStatus + "EvseStatusRecord",
                                             new XElement(NS_OICPv1EVSEStatus + "EvseId",     EVSE.Id.    ToString()),
                                             new XElement(NS_OICPv1EVSEStatus + "EvseStatus", EVSE.Status.ToString())
                                         )
                                     )

                                 )
                             ));

        }

        #endregion


        #region AuthorizeStart(this EVSE, PartnerSessionID, UID)

        public static XElement AuthorizeStart(this EVSE   EVSE,
                                              String      PartnerSessionID,
                                              String      UID)
        {

            return Encapsulate(new XElement(NS_OICPv1Authorization + "HubjectAuthorizeStart",
                                 new XElement(NS_OICPv1Authorization + "PartnerSessionID", PartnerSessionID),
                                 new XElement(NS_OICPv1Authorization + "OperatorID",       EVSE.ChargingStation.Pool.Operator.Id),
                                 new XElement(NS_OICPv1Authorization + "EVSEID",           EVSE.Id),
                                 new XElement(NS_OICPv1Authorization + "Identification",
                                     new XElement(NS_OICPv1CommonTypes + "RFIDdesfireIdentification",
                                     //new XElement(NS_OICPv1CommonTypes + "RFIDclassicIdentification",
                                        new XElement(NS_OICPv1CommonTypes + "UID", UID)
                                     )
                                 )
                             ));

        }

        #endregion

        #region AuthorizeStop(this EVSE, SessionID, PartnerSessionID, UID)

        public static XElement AuthorizeStop(this EVSE   EVSE,
                                             String      SessionID,
                                             String      PartnerSessionID,
                                             String      UID)
        {

            return Encapsulate(new XElement(NS_OICPv1Authorization + "HubjectAuthorizeStop",
                                 new XElement(NS_OICPv1Authorization + "SessionID",        SessionID),
                                 new XElement(NS_OICPv1Authorization + "PartnerSessionID", PartnerSessionID),
                                 new XElement(NS_OICPv1Authorization + "OperatorID",       EVSE.ChargingStation.Pool.Operator.Id),
                                 new XElement(NS_OICPv1Authorization + "EVSEID",           EVSE.Id),
                                 new XElement(NS_OICPv1Authorization + "Identification",
                                     new XElement(NS_OICPv1CommonTypes + "RFIDdesfireIdentification",
                                        new XElement(NS_OICPv1CommonTypes + "UID", UID)
                                     )
                                 )
                             ));

        }

        #endregion


        #region SendChargeDetailRecord(this EVSE, SessionID, PartnerSessionID, UID)

        public static XElement SendChargeDetailRecord(this EVSE  EVSE,
                                                      String     SessionID,
                                                      String     PartnerSessionID,
                                                      String     PartnerProductID,
                                                      String     UID)
        {

            return Encapsulate(new XElement(NS_OICPv1Authorization + "HubjectChargeDetailRecord",

                                 new XElement(NS_OICPv1Authorization + "SessionID",        SessionID),
                                 new XElement(NS_OICPv1Authorization + "PartnerSessionID", PartnerSessionID),
                                 new XElement(NS_OICPv1Authorization + "PartnerProductID", PartnerProductID),
                                 new XElement(NS_OICPv1Authorization + "EVSEID",           EVSE.Id),

                                 new XElement(NS_OICPv1Authorization + "Identification",
                                     new XElement(NS_OICPv1CommonTypes + "RFIDdesfireIdentification",
                                        new XElement(NS_OICPv1CommonTypes + "UID", UID)
                                     )
                                 ),

                                 new XElement(NS_OICPv1Authorization + "ChargingStart",   "..."),
                                 new XElement(NS_OICPv1Authorization + "ChargingEnd",     "..."),
                                 new XElement(NS_OICPv1Authorization + "SessionStart",    "..."),
                                 new XElement(NS_OICPv1Authorization + "SessionEnd",      "..."),
                                 new XElement(NS_OICPv1Authorization + "MeterValueStart", "..."),
                                 new XElement(NS_OICPv1Authorization + "MeterValueEnd",   "..."),

                                 new XElement(NS_OICPv1Authorization + "MeterValueInBetween",
                                     new XElement(NS_OICPv1CommonTypes + "MeterValue", "...")
                                 ),

                                 new XElement(NS_OICPv1Authorization + "ConsumedEnergy",    "..."),
                                 new XElement(NS_OICPv1Authorization + "MeteringSignature", "..."),
                                 new XElement(NS_OICPv1Authorization + "HubOperatorID",     "..."),
                                 new XElement(NS_OICPv1Authorization + "HubProviderID",     "...")

                             ));

        }

        #endregion

    }

}
