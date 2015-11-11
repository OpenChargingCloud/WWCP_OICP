/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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
    /// OICP v2.0 CPOClient XML methods.
    /// </summary>
    public static class CPOClient_XMLMethods
    {

        #region PushEVSEDataXML(GroupedData,           OICPAction = fullLoad, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(ILookup<EVSEOperator, IEnumerable<EVSEDataRecord>>  GroupedData,
                                               ActionType                                          OICPAction    = ActionType.fullLoad,
                                               EVSEOperator_Id                                     OperatorId    = null,
                                               String                                              OperatorName  = null)
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
                                              datagroup.ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEDataXML(this EVSEDataRecords,  OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null)

        public static XElement PushEVSEDataXML(this IEnumerable<EVSEDataRecord> EVSEDataRecords,
                                               ActionType OICPAction = ActionType.insert,
                                               EVSEOperator_Id OperatorId = null,
                                               String OperatorName = null,
                                               Func<EVSEDataRecord, Boolean> IncludeEVSEs = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
            //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
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
            //             <EVSEData:EvseDataRecord>
            //                [...]
            //             </EVSEData:EvseDataRecord>
            //
            //          </EVSEData:OperatorEvseData>
            //       </EVSEData:eRoamingPushEvseData>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be null!");

            var _EVSEDataRecords = EVSEDataRecords.ToArray();

            if (_EVSEDataRecords.Length == 0)
                throw new ArgumentNullException("EVSEs", "The given parameter must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", OICPAction.ToString()),
                                      new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                          new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId != null
                                                                                           ? OperatorId
                                                                                           : _EVSEDataRecords.First().EVSEId.OperatorId).OriginId),

                                          OperatorName.IsNotNullOrEmpty()
                                              ? new XElement(OICPNS.EVSEData + "OperatorName", OperatorName)
                                              : null,

                                          // <EvseDataRecord> ... </EvseDataRecord>
                                          _EVSEDataRecords.
                                              Where (evsedatarecord => IncludeEVSEs(evsedatarecord)).
                                              Select(evsedatarecord => evsedatarecord.ToXML())

                                      )
                                  ));

        }

        #endregion


        #region PushEVSEStatusXML(GroupedData,           OICPAction = update, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(Dictionary<EVSEOperator, IEnumerable<EVSEStatusRecord>>  GroupedData,
                                                 ActionType                                               OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                                          OperatorId    = null,
                                                 String                                                   OperatorName  = null)
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

                                              datagroup.Value.Select(statusrecord => statusrecord.ToXML()).ToArray()

                                          )).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(this EVSEStatusRecords, OperatorId, OperatorName = null, OICPAction = update, IncludeEVSEIds = null)

        public static XElement PushEVSEStatusXML(this IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                                 EVSEOperator_Id                     OperatorId,
                                                 String                              OperatorName    = null,
                                                 ActionType                          OICPAction      = ActionType.update,
                                                 Func<EVSE_Id, Boolean>              IncludeEVSEIds  = null)
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

            if (EVSEStatusRecords == null)
                throw new ArgumentNullException("EVSEIdAndStatus", "The given parameter must not be null!");

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId",      "The given parameter must not be null!");

            var _EVSEIdAndStatus = EVSEStatusRecords.ToArray();

            if (_EVSEIdAndStatus.Length == 0)
                throw new ArgumentNullException("EVSEIdAndStatus", "The given parameter must not be empty!");

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

                                              _EVSEIdAndStatus.
                                                  Where(EVSEStatusRecord => IncludeEVSEIds(EVSEStatusRecord.Id)).
                                                  ToEvseStatusRecords().
                                                  ToArray()

                                          )
                                      ));

        }

        #endregion


        #region AuthorizeStartXML(OperatorId, AuthToken, EVSEId = null, PartnerProductId = null, SessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP EVSEData.0 Authorize Start XML request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static XElement AuthorizeStartXML(EVSEOperator_Id     OperatorId,
                                                 Auth_Token          AuthToken,
                                                 EVSE_Id             EVSEId            = null,   // OICP EVSEData.0: Optional
                                                 ChargingProduct_Id  PartnerProductId  = null,   // OICP EVSEData.0: Optional [100]
                                                 ChargingSession_Id  SessionId         = null,   // OICP EVSEData.0: Optional
                                                 ChargingSession_Id  PartnerSessionId  = null)   // OICP EVSEData.0: Optional [50]
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <Authorization:eRoamingAuthorizeStart>
            //
            //          <!--Optional:-->
            //          <Authorization:SessionID>?</Authorization:SessionID>
            //          <!--Optional:-->
            //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
            //
            //          <Authorization:OperatorID>?</Authorization:OperatorID>
            //
            //          <!--Optional:-->
            //          <Authorization:EVSEID>?</Authorization:EVSEID>
            //
            //          <Authorization:Identification>
            //
            //             <!--You have a CHOICE of the next 4 items at this level-->
            //             <CommonTypes:RFIDmifarefamilyIdentification>
            //                <CommonTypes:UID>?</CommonTypes:UID>
            //             </CommonTypes:RFIDmifarefamilyIdentification>
            //
            //             <CommonTypes:QRCodeIdentification>
            //
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //
            //                <!--You have a CHOICE of the next 2 items at this level-->
            //                <CommonTypes:PIN>?</CommonTypes:PIN>
            //
            //                <CommonTypes:HashedPIN>
            //                   <CommonTypes:Value>?</CommonTypes:Value>
            //                   <CommonTypes:Function>?</CommonTypes:Function>
            //                   <CommonTypes:Salt>?</CommonTypes:Salt>
            //                </CommonTypes:HashedPIN>
            //
            //             </CommonTypes:QRCodeIdentification>
            //
            //             <CommonTypes:PlugAndChargeIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:PlugAndChargeIdentification>
            //
            //             <CommonTypes:RemoteIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:RemoteIdentification>
            //
            //          </Authorization:Identification>
            //
            //          <!--Optional:-->
            //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
            //
            //       </Authorization:eRoamingAuthorizeStart>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeStart",

                                          PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString())                 : null,

                                          new XElement(OICPNS.Authorization + "OperatorID", OperatorId.OriginId),

                                          EVSEId != null
                                              ? new XElement(OICPNS.Authorization + "EVSEID", EVSEId.OriginId)
                                              : null,

                                          new XElement(OICPNS.Authorization + "Identification",
                                              new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(OICPNS.CommonTypes + "UID", AuthToken.ToString())
                                              )
                                          ),

                                          PartnerProductId != null
                                              ? new XElement(OICPNS.Authorization + "PartnerProductID", PartnerProductId.ToString())
                                              : null

                                     ));

        }

        #endregion

        #region AuthorizeStopXML(OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP EVSEData.0 AuthorizeStop XML request.
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

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <Authorization:eRoamingAuthorizeStop>
            // 
            //          <Authorization:SessionID>?</Authorization:SessionID>
            // 
            //          <!--Optional:-->
            //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
            // 
            //          <Authorization:OperatorID>?</Authorization:OperatorID>
            // 
            //          <!--Optional:-->
            //          <Authorization:EVSEID>?</Authorization:EVSEID>
            // 
            //          <Authorization:Identification>
            // 
            //             <!--You have a CHOICE of the next 4 items at this level-->
            //             <CommonTypes:RFIDmifarefamilyIdentification>
            //                <CommonTypes:UID>?</CommonTypes:UID>
            //             </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //             <CommonTypes:QRCodeIdentification>
            //
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //
            //                <!--You have a CHOICE of the next 2 items at this level-->
            //                <CommonTypes:PIN>?</CommonTypes:PIN>
            //
            //                <CommonTypes:HashedPIN>
            //                   <CommonTypes:Value>?</CommonTypes:Value>
            //                   <CommonTypes:Function>?</CommonTypes:Function>
            //                   <CommonTypes:Salt>?</CommonTypes:Salt>
            //                </CommonTypes:HashedPIN>
            //
            //             </CommonTypes:QRCodeIdentification>
            // 
            //             <CommonTypes:PlugAndChargeIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:PlugAndChargeIdentification>
            // 
            //             <CommonTypes:RemoteIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:RemoteIdentification>
            // 
            //          </Authorization:Identification>
            // 
            //       </Authorization:eRoamingAuthorizeStop>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeStop",

                                          new XElement(OICPNS.Authorization + "SessionID", SessionId.ToString()),

                                          PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,

                                          new XElement(OICPNS.Authorization + "OperatorID", OperatorId.OriginId),

                                          EVSEId != null
                                              ? new XElement(OICPNS.Authorization + "EVSEID", EVSEId.OriginId)
                                              : null,

                                          new XElement(OICPNS.Authorization + "Identification",
                                              new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                                                 new XElement(OICPNS.CommonTypes + "UID", AuthToken.ToString())
                                              )
                                          )

                                      ));

        }

        #endregion


        #region PullAuthenticationDataXML(OperatorId)

        /// <summary>
        /// Create an OICP EVSEData.0 PullAuthenticationData XML request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        public static XElement PullAuthenticationDataXML(EVSEOperator_Id OperatorId)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/EVSEData.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <AuthenticationData:eRoamingPullAuthenticationData>
            //          <AuthenticationData:OperatorID>?</AuthenticationData:OperatorID>
            //       </AuthenticationData:eRoamingPullAuthenticationData>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.AuthenticationData + "eRoamingPullAuthenticationData",
                                          new XElement(OICPNS.AuthenticationData + "OperatorID", OperatorId.OriginId)
                                      ));

        }

        #endregion


        #region SendChargeDetailRecordXML(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerProductId">The ev charging product identification.</param>
        /// <param name="SessionStart">The session start timestamp.</param>
        /// <param name="SessionEnd">The session end timestamp.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        public static XElement SendChargeDetailRecordXML(EVSE_Id              EVSEId,
                                                         ChargingSession_Id   SessionId,
                                                         ChargingProduct_Id   PartnerProductId,
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

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <Authorization:eRoamingChargeDetailRecord>
            // 
            //          <Authorization:SessionID>?</Authorization:SessionID>
            // 
            //          <!--Optional:-->
            //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
            // 
            //          <!--Optional:-->
            //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
            // 
            //          <Authorization:EvseID>?</Authorization:EvseID>
            // 
            //          <Authorization:Identification>
            // 
            //             <!--You have a CHOICE of the next 4 items at this level-->
            //             <CommonTypes:RFIDmifarefamilyIdentification>
            //                <CommonTypes:UID>?</CommonTypes:UID>
            //             </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //             <CommonTypes:QRCodeIdentification>
            // 
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            // 
            //                <!--You have a CHOICE of the next 2 items at this level-->
            //                <CommonTypes:PIN>?</CommonTypes:PIN>
            // 
            //                <CommonTypes:HashedPIN>
            //                   <CommonTypes:Value>?</CommonTypes:Value>
            //                   <CommonTypes:Function>?</CommonTypes:Function>
            //                   <CommonTypes:Salt>?</CommonTypes:Salt>
            //                </CommonTypes:HashedPIN>
            // 
            //             </CommonTypes:QRCodeIdentification>
            // 
            //             <CommonTypes:PlugAndChargeIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:PlugAndChargeIdentification>
            // 
            //             <CommonTypes:RemoteIdentification>
            //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
            //             </CommonTypes:RemoteIdentification>
            // 
            //          </Authorization:Identification>
            // 
            //          <!--Optional:-->
            //          <Authorization:ChargingStart>?</Authorization:ChargingStart>
            //          <!--Optional:-->
            //          <Authorization:ChargingEnd>?</Authorization:ChargingEnd>
            //          <Authorization:SessionStart>?</Authorization:SessionStart>
            //          <Authorization:SessionEnd>?</Authorization:SessionEnd>
            //
            //          <!--Optional:-->
            //          <Authorization:MeterValueStart>?</Authorization:MeterValueStart>
            //          <!--Optional:-->
            //          <Authorization:MeterValueEnd>?</Authorization:MeterValueEnd>
            //
            //          <!--Optional:-->
            //          <Authorization:MeterValueInBetween>
            //             <!--1 or more repetitions:-->
            //             <Authorization:MeterValue>?</Authorization:MeterValue>
            //          </Authorization:MeterValueInBetween>
            //
            //          <!--Optional:-->
            //          <Authorization:ConsumedEnergy>?</Authorization:ConsumedEnergy>
            //          <!--Optional:-->
            //          <Authorization:MeteringSignature>?</Authorization:MeteringSignature>
            //
            //          <!--Optional:-->
            //          <Authorization:HubOperatorID>?</Authorization:HubOperatorID>
            //          <!--Optional:-->
            //          <Authorization:HubProviderID>?</Authorization:HubProviderID>
            // 
            //       </Authorization:eRoamingChargeDetailRecord>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

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

            var _MeterValuesInBetween = MeterValuesInBetween != null ? MeterValuesInBetween.ToArray() : new Double[0];

            return SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecord",

                                 new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                                 new XElement(OICPNS.Authorization + "PartnerSessionID", (PartnerSessionId != null) ? PartnerSessionId.ToString() : ""),
                                 new XElement(OICPNS.Authorization + "PartnerProductID", (PartnerProductId != null) ? PartnerProductId.ToString() : ""),
                                 new XElement(OICPNS.Authorization + "EvseID",           EVSEId.OriginId),

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

                                 new XElement(OICPNS.Authorization + "SessionStart", SessionStart.ToIso8601()),
                                 new XElement(OICPNS.Authorization + "SessionEnd",   SessionEnd.  ToIso8601()),

                                 (MeterValueStart.HasValue) ? new XElement(OICPNS.Authorization + "MeterValueStart",  String.Format("{0:0.###}", MeterValueStart).Replace(",", ".")) : null,
                                 (MeterValueEnd.  HasValue) ? new XElement(OICPNS.Authorization + "MeterValueEnd",    String.Format("{0:0.###}", MeterValueEnd).  Replace(",", ".")) : null,

                                 _MeterValuesInBetween.Length > 0 ? new XElement(OICPNS.Authorization + "MeterValueInBetween",
                                                                        _MeterValuesInBetween.
                                                                            Select(value => new XElement(OICPNS.Authorization + "MeterValue", String.Format("{0:0.###}", value).Replace(",", "."))).
                                                                            ToArray()
                                                                    )
                                                                  : null,

                                 ConsumedEnergy    != null ? new XElement(OICPNS.Authorization + "ConsumedEnergy",    String.Format("{0:0.}", ConsumedEnergy).Replace(",", ".")) : null,
                                 MeteringSignature != null ? new XElement(OICPNS.Authorization + "MeteringSignature", MeteringSignature)        : null,

                                 HubOperatorId     != null ? new XElement(OICPNS.Authorization + "HubOperatorID",     HubOperatorId.ToString()) : null,
                                 HubProviderId     != null ? new XElement(OICPNS.Authorization + "HubProviderID",     HubProviderId.ToString()) : null

                             ));

        }

        #endregion


    }

}
