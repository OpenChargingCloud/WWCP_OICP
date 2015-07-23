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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

using org.GraphDefined.WWCP.IO.OICP;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// CPO management operations.
    /// </summary>
    public static class CPOMethods
    {

        #region PushEVSEDataXML(this EVSEOperator, Action = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this EVSEOperator       EVSEOperator,
                                               ActionType              Action        = ActionType.fullLoad,
                                               EVSEOperator_Id         OperatorId    = null,
                                               String                  OperatorName  = null,
                                               Func<EVSE_Id, Boolean>  IncludeEVSEs  = null)
        {

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            return EVSEOperator.ChargingPools.
                                SelectMany(Pool    => Pool.ChargingStations).
                                SelectMany(Station => Station.EVSEs).
                                Where     (EVSE    => IncludeEVSEs(EVSE.Id)).
                                PushEVSEDataXML((OperatorId == null) ? EVSEOperator.Id : OperatorId,
                                                (OperatorName == null) ? EVSEOperator.Name[Languages.de] : OperatorName,
                                                Action);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingPools, OperatorId, OperatorName, Action = fullLoad, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingPool>  ChargingPools,
                                               EVSEOperator_Id                 OperatorId,
                                               String                          OperatorName,
                                               ActionType                      Action        = ActionType.fullLoad,
                                               Func<EVSE_Id, Boolean>          IncludeEVSEs  = null)
        {

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            return ChargingPools.SelectMany(Pool    => Pool.ChargingStations).
                                 SelectMany(Station => Station.EVSEs).
                                 Where     (EVSE    => IncludeEVSEs(EVSE.Id)).
                                 PushEVSEDataXML(OperatorId,
                                                 OperatorName,
                                                 Action);

        }

        #endregion

        #region PushEVSEDataXML(this ChargingStations, OperatorId, OperatorName, Action = fullLoad, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<ChargingStation>  ChargingStations,
                                               EVSEOperator_Id                    OperatorId,
                                               String                             OperatorName,
                                               ActionType                         Action        = ActionType.fullLoad,
                                               Func<EVSE_Id, Boolean>             IncludeEVSEs  = null)
        {

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            return ChargingStations.SelectMany(Station => Station.EVSEs).
                                    Where     (EVSE    => IncludeEVSEs(EVSE.Id)).
                                    PushEVSEDataXML(OperatorId,
                                                    OperatorName,
                                                    Action);

        }

        #endregion

        #region PushEVSEDataXML(this EVSEs, OperatorId, OperatorName, Action = ActionType.fullLoad)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSE>  EVSEs,
                                               EVSEOperator_Id         OperatorId,
                                               String                  OperatorName,
                                               ActionType              Action  = ActionType.fullLoad)
        {

            //if (Action == ActionType.fullLoad ||
            //    Action == ActionType.insert   ||
            //    Action == ActionType.update)

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSEData + "eRoamingPushEvseData",
                                      new XElement(NS.OICPv1_2EVSEData + "ActionType", Action.ToString()),
                                      new XElement(NS.OICPv1_2EVSEData + "OperatorEvseData",

                                          new XElement(NS.OICPv1_2EVSEData + "OperatorID", OperatorId.ToFormat(IdFormatType.OLD)),
                                          (OperatorName != null) ?
                                          new XElement(NS.OICPv1_2EVSEData + "OperatorName", OperatorName) : null,

                                          // EVSE => EvseDataRecord
                                          //charging pools.Select(charging pool =>
                                          //charging pool.ChargingStations.Select(ChargingStation =>
                                          //ChargingStation.EVSEs.Select(EVSE => {

                                          EVSEs.Select(EVSE => {

                                              try
                                              {

                                                  return new XElement(NS.OICPv1_2EVSEData + "EvseDataRecord",

                                                      new XElement(NS.OICPv1_2EVSEData + "EvseId",                 EVSE.Id.ToFormat(IdFormatType.OLD)),
                                                      new XElement(NS.OICPv1_2EVSEData + "ChargingStationId",      EVSE.ChargingStation.Id.ToString()),
                                                      new XElement(NS.OICPv1_2EVSEData + "ChargingStationName",    EVSE.ChargingStation.ChargingPool.Name[Languages.de].SubstringMax(50)),
                                                      new XElement(NS.OICPv1_2EVSEData + "EnChargingStationName",  EVSE.ChargingStation.ChargingPool.Name[Languages.en].SubstringMax(50)),

                                                      new XElement(NS.OICPv1_2EVSEData + "Address",
                                                          new XElement(NS.OICPv1_2CommonTypes + "Country",     EVSE.ChargingStation.ChargingPool.Address.Country.Alpha3Code),
                                                          new XElement(NS.OICPv1_2CommonTypes + "City",        EVSE.ChargingStation.ChargingPool.Address.City),
                                                          new XElement(NS.OICPv1_2CommonTypes + "Street",      EVSE.ChargingStation.ChargingPool.Address.Street), // OICPv1.2 requires at least 5 characters!
                                                          new XElement(NS.OICPv1_2CommonTypes + "PostalCode",  EVSE.ChargingStation.ChargingPool.Address.PostalCode),
                                                          new XElement(NS.OICPv1_2CommonTypes + "HouseNum",    EVSE.ChargingStation.ChargingPool.Address.HouseNumber),
                                                          new XElement(NS.OICPv1_2CommonTypes + "Floor",       EVSE.ChargingStation.ChargingPool.Address.FloorLevel)
                                                      // <!--Optional:-->
                                                      // <v11:Region>?</v11:Region>
                                                      // <!--Optional:-->
                                                      // <v11:TimeZone>?</v11:TimeZone>
                                                      ),

                                                      new XElement(NS.OICPv1_2EVSEData + "GeoCoordinates",
                                                          new XElement(NS.OICPv1_2CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                                                              new XElement(NS.OICPv1_2CommonTypes + "Longitude", EVSE.ChargingStation.GeoLocation.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                                                              new XElement(NS.OICPv1_2CommonTypes + "Latitude",  EVSE.ChargingStation.GeoLocation.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                                                          )
                                                      ),

                                                      new XElement(NS.OICPv1_2EVSEData + "Plugs",
                                                          EVSE.SocketOutlets.Select(Outlet =>
                                                             new XElement(NS.OICPv1_2EVSEData + "Plug", eMI3_OICP_Mapper.MapToPlugType(Outlet)))
                                                      ),

                                                      new XElement(NS.OICPv1_2EVSEData + "ChargingFacilities",
                                                          EVSE.SocketOutlets.Select(Outlet => {

                                                              var ChargingFacility = "Unspecified";

                                                              if (Outlet.Plug == PlugType.Type2Outlet ||
                                                                  Outlet.Plug == PlugType.Type2Connector_CableAttached)// Mennekes_Type_2
                                                              {

                                                                  if (Outlet.MaxPower <= 44.0)
                                                                      ChargingFacility = "380 - 480V, 3-Phase ≤63A";

                                                                  if (Outlet.MaxPower <= 22.0)
                                                                      ChargingFacility = "380 - 480V, 3-Phase ≤32A";

                                                                  if (Outlet.MaxPower <= 11.0)
                                                                      ChargingFacility = "380 - 480V, 3-Phase ≤16A";

                                                              }

                                                              else if (Outlet.Plug == PlugType.TypeFSchuko)
                                                              {

                                                                  if (Outlet.MaxPower >  7.2)
                                                                      ChargingFacility = "200 - 240V, 1-Phase >32A";

                                                                  if (Outlet.MaxPower <= 7.2)
                                                                      ChargingFacility = "200 - 240V, 1-Phase ≤32A";

                                                                  if (Outlet.MaxPower <= 3.6)
                                                                      ChargingFacility = "200 - 240V, 1-Phase ≤16A";

                                                                  if (Outlet.MaxPower <= 2.25)
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

                                                              return new XElement(NS.OICPv1_2EVSEData + "ChargingFacility", ChargingFacility);

                                                          })

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

                                                      new XElement(NS.OICPv1_2EVSEData + "AuthenticationModes",
                                                          new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "NFC RFID Classic"),
                                                          new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "NFC RFID DESFire"),
                                                          new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "REMOTE"),
                                                          //new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "PnC"),
                                                          new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "Direct Payment")
                                                      // EVSE.SocketOutlets.Select(Outlet =>
                                                      //    new XElement(NS.OICPv1_2EVSEData + "AuthenticationMode", "Unspecified"))//Outlet.Plug.ToString()))
                                                      ),

                            //                    <!--Optional:-->
                                                      //               <v1:MaxCapacity>?</v1:MaxCapacity>

                                                      new XElement(NS.OICPv1_2EVSEData + "PaymentOptions",
                                                          new XElement(NS.OICPv1_2EVSEData + "PaymentOption", "Contract")
                                                          // ??????????????????????? SMS!
                                                      ),

                                                      new XElement(NS.OICPv1_2EVSEData + "Accessibility", "Free publicly accessible"),
                                                      new XElement(NS.OICPv1_2EVSEData + "HotlinePhoneNum", "+8000670000"),  // RegEx: \+[0-9]{5,15}

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

                                                      new XElement(NS.OICPv1_2EVSEData + "IsOpen24Hours", EVSE.ChargingStation.ChargingPool.OpeningTime.IsOpen24Hours ? "true" : "false"),

                                                      EVSE.ChargingStation.ChargingPool.OpeningTime.IsOpen24Hours
                                                          ? null
                                                          : new XElement(NS.OICPv1_2EVSEData + "OpeningTime",   EVSE.ChargingStation.ChargingPool.OpeningTime.Text),

                            //                    <!--Optional:-->
                                                      //               <v1:HubOperatorID>?</v1:HubOperatorID>

                            //                    <!--Optional:-->
                                                      //               <v1:ClearinghouseID>?</v1:ClearinghouseID>

                                                      new XElement(NS.OICPv1_2EVSEData + "IsHubjectCompatible",  "true"),
                                                      new XElement(NS.OICPv1_2EVSEData + "DynamicInfoAvailable", "true")

                                                  );

                                              }
                                              catch (Exception e)
                                              {
                                                  Debug.WriteLine("Exception in CPOMethods: " + e.Message);
                                                  return null;
                                              }

                                          })
                                      )
                                  ));

        }

        #endregion

        #region PushEVSEDataXML(this EVSE, OperatorId, OperatorName, Action = insert)

        public static XElement PushEVSEDataXML(this EVSE        EVSE,
                                               EVSEOperator_Id  OperatorId,
                                               String           OperatorName,
                                               ActionType       Action  = ActionType.insert)
        {

            return new EVSE[1] { EVSE }.PushEVSEDataXML(OperatorId,
                                                        OperatorName,
                                                        Action);

        }

        #endregion


        #region PushEVSEStatusXML(this EVSEOperator, Action = fullLoad, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(this EVSEOperator  EVSEOperator,
                                                 ActionType         Action        = ActionType.fullLoad,
                                                 EVSEOperator_Id    OperatorId    = null,
                                                 String             OperatorName  = null)
        {

            return EVSEOperator.ChargingPools.
                                SelectMany(pool    => pool.ChargingStations).
                                SelectMany(station => station.EVSEs).

                                PushEVSEStatusXML((OperatorId   == null) ? EVSEOperator.Id                 : OperatorId,
                                                  (OperatorName == null) ? EVSEOperator.Name.First().Text : OperatorName,
                                                   Action);

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEs, OperatorId, OperatorName, Action)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSE>  EVSEs,
                                                 EVSEOperator_Id         OperatorId,
                                                 String                  OperatorName,
                                                 ActionType              Action)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSEStatus + "eRoamingPushEvseStatus",
                                          new XElement(NS.OICPv1_2EVSEStatus + "ActionType", Action.ToString()),
                                          new XElement(NS.OICPv1_2EVSEStatus + "OperatorEvseStatus",

                                              new XElement(NS.OICPv1_2EVSEStatus + "OperatorID", OperatorId.ToFormat(IdFormatType.OLD)),
                                              (OperatorName != null) ?
                                              new XElement(NS.OICPv1_2EVSEStatus + "OperatorName", OperatorName) : null,

                                              EVSEs.Select(EVSE =>
                                                  new XElement(NS.OICPv1_2EVSEStatus + "EvseStatusRecord",
                                                      new XElement(NS.OICPv1_2EVSEStatus + "EvseId",     EVSE.Id.                 ToString()),
                                                      new XElement(NS.OICPv1_2EVSEStatus + "EvseStatus", EVSE.Status.Value.ToString())
                                                  )
                                              )

                                          )
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEStates, OperatorId, OperatorName, Action)

        public static XElement PushEVSEStatusXML(this IEnumerable<KeyValuePair<EVSE_Id, HubjectEVSEState>>  EVSEStates,
                                                 EVSEOperator_Id                                            OperatorId,
                                                 String                                                     OperatorName,
                                                 ActionType                                                 Action)
        {

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2EVSEStatus + "eRoamingPushEvseStatus",
                                          new XElement(NS.OICPv1_2EVSEStatus + "ActionType", Action.ToString()),
                                          new XElement(NS.OICPv1_2EVSEStatus + "OperatorEvseStatus",

                                              new XElement(NS.OICPv1_2EVSEStatus + "OperatorID", OperatorId.ToFormat(IdFormatType.OLD)),
                                              (OperatorName != null) ?
                                              new XElement(NS.OICPv1_2EVSEStatus + "OperatorName", OperatorName) : null,

                                              EVSEStates.Select(EvseIdAndState =>
                                                  new XElement(NS.OICPv1_2EVSEStatus + "EvseStatusRecord",
                                                      new XElement(NS.OICPv1_2EVSEStatus + "EvseId",     EvseIdAndState.Key.  ToFormat(IdFormatType.OLD)),
                                                      new XElement(NS.OICPv1_2EVSEStatus + "EvseStatus", EvseIdAndState.Value.ToString())
                                                  )
                                              )

                                          )
                                      ));

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

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2Authorization + "eRoamingAuthorizeStart",
                                          PartnerSessionId != null ? new XElement(NS.OICPv1_2Authorization + "PartnerSessionID", PartnerSessionId.ToString())                 : null,
                                          new XElement(NS.OICPv1_2Authorization + "OperatorID",       OperatorId.ToFormat(IdFormatType.OLD)),
                                          EVSEId           != null ? new XElement(NS.OICPv1_2Authorization + "EVSEID",           EVSEId.          ToFormat(IdFormatType.OLD)) : null,
                                          new XElement(NS.OICPv1_2Authorization + "Identification",
                                              new XElement(NS.OICPv1_2CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(NS.OICPv1_2CommonTypes + "UID", AuthToken.ToString())
                                              )
                                          )
                                          
                                      ));


            // <eMI3:Envelope xmlns:eMI3="http://schemas.xmlsoap.org/soap/envelope/"
            //                xmlns:Authorization="http://www.hubject.com/b2b/services/authorization/v1.2"
            //                xmlns:CommonTypes="http://www.hubject.com/b2b/services/commontypes/v1.2"
            //                xmlns:EVSEData="http://www.hubject.com/b2b/services/evsedata/v1.2"
            //                xmlns:EVSESearch="http://www.hubject.com/b2b/services/evsesearch/v1.2"
            //                xmlns:EVSEStatus="http://www.hubject.com/b2b/services/evsestatus/v1.2"
            //                xmlns:MobileAuthorization="http://www.hubject.com/b2b/services/mobileauthorization/v1.2">
            //   <eMI3:Header/>
            //   <eMI3:Body>
            //     <Authorization:eRoamingAuthorizeStart>
            //       <Authorization:OperatorID>+49*822</Authorization:OperatorID>
            //       <Authorization:Identification>
            //         <CommonTypes:RFIDmifarefamilyIdentification>
            //           <CommonTypes:UID>8027C0FA1B7604</CommonTypes:UID>
            //         </CommonTypes:RFIDmifarefamilyIdentification>
            //       </Authorization:Identification>
            //       <Authorization:EVSEID>+49*822*285808576*1</Authorization:EVSEID>
            //     </Authorization:eRoamingAuthorizeStart></eMI3:Body></eMI3:Envelope>

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

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2Authorization + "eRoamingAuthorizeStop",
                                          new XElement(NS.OICPv1_2Authorization + "SessionID",        SessionId.ToString()),
                                          new XElement(NS.OICPv1_2Authorization + "PartnerSessionID", PartnerSessionId.ToString()),
                                          new XElement(NS.OICPv1_2Authorization + "OperatorID",       OperatorId.ToFormat(IdFormatType.OLD)),
                                          new XElement(NS.OICPv1_2Authorization + "EVSEID",           EVSEId.    ToFormat(IdFormatType.OLD)),
                                          new XElement(NS.OICPv1_2Authorization + "Identification",
                                              new XElement(NS.OICPv1_2CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(NS.OICPv1_2CommonTypes + "UID", UID.ToString())
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

            return SOAP.Encapsulation(new XElement(NS.OICPv1_2Authorization + "eRoamingChargeDetailRecord",

                                 new XElement(NS.OICPv1_2Authorization + "SessionID",        SessionId.ToString()),
                                 new XElement(NS.OICPv1_2Authorization + "PartnerSessionID", (PartnerSessionId != null) ? PartnerSessionId.ToString() : ""),
                                 new XElement(NS.OICPv1_2Authorization + "PartnerProductID", PartnerProductId),
                                 new XElement(NS.OICPv1_2Authorization + "EvseID",           EVSEId.ToFormat(IdFormatType.OLD)),

                                 new XElement(NS.OICPv1_2Authorization + "Identification",
                                     (UID != null)
                                         ? new XElement(NS.OICPv1_2CommonTypes + "RFIDmifarefamilyIdentification",
                                                new XElement(NS.OICPv1_2CommonTypes + "UID", UID.ToString())
                                           )
                                         : new XElement(NS.OICPv1_2CommonTypes + "RemoteIdentification",
                                                new XElement(NS.OICPv1_2CommonTypes + "EVCOID", EVCOId.ToString())
                                           )
                                 ),

                                 new XElement(NS.OICPv1_2Authorization + "ChargingStart",   ChargeStart),  // "2014-02-01T15:45:00+02:00"
                                 new XElement(NS.OICPv1_2Authorization + "ChargingEnd",     ChargeEnd),
                                 (SessionStart.   HasValue) ? new XElement(NS.OICPv1_2Authorization + "SessionStart",    SessionStart)    : null,
                                 (SessionEnd.     HasValue) ? new XElement(NS.OICPv1_2Authorization + "SessionEnd",      SessionEnd)      : null,
                                 (MeterValueStart.HasValue) ? new XElement(NS.OICPv1_2Authorization + "MeterValueStart", MeterValueStart) : null,
                                 (MeterValueEnd.  HasValue) ? new XElement(NS.OICPv1_2Authorization + "MeterValueEnd",   MeterValueEnd)   : null

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
