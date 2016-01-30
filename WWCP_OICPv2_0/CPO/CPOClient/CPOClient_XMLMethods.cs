/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// OICP v2.0 CPO client XML methods.
    /// </summary>
    public static class CPOClient_XMLMethods
    {

        #region PushEVSEDataXML  (GroupedEVSEs,      OICPAction = fullLoad, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(ILookup<EVSEOperator, EVSEDataRecord>  GroupedEVSEs,
                                               ActionType                             OICPAction    = ActionType.fullLoad,
                                               EVSEOperator_Id                        OperatorId    = null,
                                               String                                 OperatorName  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
            //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
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

            if (GroupedEVSEs == null)
                throw new ArgumentNullException(nameof(GroupedEVSEs),  "The given llokup of EVSE data records must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", OICPAction.ToString()),
                                      GroupedEVSEs.Select(group =>

                                          group.Where(evsedatarecord => evsedatarecord != null).Any()
                                              ? new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                                    new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId != null
                                                                                                      ? OperatorId
                                                                                                      : group.Key.Id).OriginId),

                                                    (OperatorName.IsNotNullOrEmpty() || group.Key.Name.Any())
                                                        ? new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                              ? OperatorName
                                                                                                              : group.Key.Name.First().Text))
                                                        : null,

                                                    // <EvseDataRecord> ... </EvseDataRecord>
                                                    group.Where (evsedatarecord => evsedatarecord != null).
                                                          Select(evsedatarecord => evsedatarecord.ToXML()).
                                                          ToArray()
                                                )
                                              : null

                                          ).ToArray()
                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(GroupedEVSEStatus, OICPAction = update,   OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(ILookup<EVSEOperator_Id, EVSEStatusRecord>  GroupedEVSEStatus,
                                                 ActionType                                  OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                             OperatorId    = null,
                                                 String                                      OperatorName  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            //       <EVSEStatus:eRoamingPushEvseStatus>
            //          <EVSEStatus:ActionType>fullLoad|update|insert|delete</EVSEStatus:ActionType>
            //          <EVSEStatus:OperatorEvseStatus>
            // 
            //             <EVSEStatus:OperatorID>DE*GEF</EVSEStatus:OperatorID>
            //             <!--Optional:-->
            //             <EVSEStatus:OperatorName>Test-CPO</EVSEStatus:OperatorName>
            // 
            //             <!--One or more repetitions:-->
            //             <EVSEStatus:EvseStatusRecord>
            //                <EVSEStatus:EvseId>DE*GEF*E1234*1</EVSEStatus:EvseId>
            //                <EVSEStatus:EvseStatus>Occupied</EVSEStatus:EvseStatus>
            //             </EVSEStatus:EvseStatusRecord>
            // 
            //          </EVSEStatus:OperatorEvseStatus>
            //       </EVSEStatus>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (GroupedEVSEStatus == null)
                throw new ArgumentNullException(nameof(GroupedEVSEStatus),  "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",
                                      new XElement(OICPNS.EVSEStatus + "ActionType", OICPAction.ToString()),
                                      GroupedEVSEStatus.Select(group =>

                                          group.Where(evsestatusrecord => evsestatusrecord != null).Any()
                                              ? new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                                    new XElement(OICPNS.EVSEStatus + "OperatorID", (OperatorId != null
                                                                                                       ? OperatorId
                                                                                                       : group.Key).OriginId),

                                                    //(OperatorName.IsNotNullOrEmpty() || group.Key.Name.Any())
                                                    //    ? new XElement(OICPNS.EVSEStatus + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                    //                                                           ? OperatorName
                                                    //                                                           : group.Key.Name.First().Text))
                                                    //    : null,

                                                    // <EvseStatusRecord> ... </EvseStatusRecord>
                                                    group.Where (evsestatusrecord => evsestatusrecord != null).
                                                          Select(evsestatusrecord => evsestatusrecord.ToXML()).
                                                    ToArray()
                                                )
                                              : null

                                          ).ToArray()
                                      ));

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
                                                 ChargingProduct_Id  PartnerProductId  = null,   // OICP v2.0: Optional [100]
                                                 ChargingSession_Id  SessionId         = null,   // OICP v2.0: Optional
                                                 ChargingSession_Id  PartnerSessionId  = null)   // OICP v2.0: Optional [50]
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
                throw new ArgumentNullException(nameof(OperatorId), "The given EVSE operator identification must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given authentication token must not be null!");

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

        #region AuthorizeStopXML (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStop XML request.
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
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given parameter must not be null!");

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
        /// Create an OICP v2.0 PullAuthenticationData XML request.
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
            //          <AuthenticationData:OperatorID>DE*GEF</AuthenticationData:OperatorID>
            //       </AuthenticationData:eRoamingPullAuthenticationData>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.AuthenticationData + "eRoamingPullAuthenticationData",
                                          new XElement(OICPNS.AuthenticationData + "OperatorID", OperatorId.OriginId)
                                      ));

        }

        #endregion


        #region SendChargeDetailRecordXML(ChargeDetailRecord)

        /// <summary>
        /// Create an OICP v2.0 SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="ChargeDetailRecord">The charge detail record.</param>
        public static XElement SendChargeDetailRecordXML(eRoamingChargeDetailRecord  ChargeDetailRecord)
        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord),  "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(ChargeDetailRecord.ToXML());

        }

        #endregion

        #region SendChargeDetailRecordXML(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, AuthToken = null, eMAId = null, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 SendChargeDetailRecord XML request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="PartnerProductId">The ev charging product identification.</param>
        /// <param name="SessionStart">The session start timestamp.</param>
        /// <param name="SessionEnd">The session end timestamp.</param>
        /// <param name="Identification">An identification.</param>
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
        public static XElement SendChargeDetailRecordXML(EVSE_Id                      EVSEId,
                                                         ChargingSession_Id           SessionId,
                                                         ChargingProduct_Id           PartnerProductId,
                                                         DateTime                     SessionStart,
                                                         DateTime                     SessionEnd,
                                                         AuthorizationIdentification  Identification,
                                                         ChargingSession_Id           PartnerSessionId      = null,
                                                         DateTime?                    ChargingStart         = null,
                                                         DateTime?                    ChargingEnd           = null,
                                                         Double?                      MeterValueStart       = null,
                                                         Double?                      MeterValueEnd         = null,
                                                         IEnumerable<Double>          MeterValuesInBetween  = null,
                                                         Double?                      ConsumedEnergy        = null,
                                                         String                       MeteringSignature     = null,
                                                         HubOperator_Id               HubOperatorId         = null,
                                                         EVSP_Id                      HubProviderId         = null)
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
                throw new ArgumentNullException(nameof(EVSEId),            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException(nameof(SessionId),         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException(nameof(PartnerProductId),  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException(nameof(SessionStart),      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException(nameof(SessionEnd),        "The given parameter must not be null!");

            if (Identification   == null)
                throw new ArgumentNullException(nameof(Identification),    "The given parameter must not be null!");

            #endregion


            return SOAP.Encapsulation(

                       new XElement(OICPNS.Authorization + "eRoamingChargeDetailRecord",

                           new XElement(OICPNS.Authorization + "SessionID",        SessionId.ToString()),
                           new XElement(OICPNS.Authorization + "PartnerSessionID", (PartnerSessionId != null) ? PartnerSessionId.ToString() : ""),
                           new XElement(OICPNS.Authorization + "PartnerProductID", (PartnerProductId != null) ? PartnerProductId.ToString() : ""),
                           new XElement(OICPNS.Authorization + "EvseID",           EVSEId.OriginId),

                           Identification.ToXML(OICPNS.Authorization),

                           (ChargingStart.  HasValue) ? new XElement(OICPNS.Authorization + "ChargingStart",    ChargingStart.  Value.ToIso8601()) : null,
                           (ChargingEnd.    HasValue) ? new XElement(OICPNS.Authorization + "ChargingEnd",      ChargingEnd.    Value.ToIso8601()) : null,

                           new XElement(OICPNS.Authorization + "SessionStart", SessionStart.ToIso8601()),
                           new XElement(OICPNS.Authorization + "SessionEnd",   SessionEnd.  ToIso8601()),

                           (MeterValueStart.HasValue) ? new XElement(OICPNS.Authorization + "MeterValueStart",  String.Format("{0:0.###}", MeterValueStart).Replace(",", ".")) : null,
                           (MeterValueEnd.  HasValue) ? new XElement(OICPNS.Authorization + "MeterValueEnd",    String.Format("{0:0.###}", MeterValueEnd).  Replace(",", ".")) : null,

                           MeterValuesInBetween != null
                               ? new XElement(OICPNS.Authorization + "MeterValueInBetween",
                                     MeterValuesInBetween.
                                         SafeSelect(value => new XElement(OICPNS.Authorization + "MeterValue", String.Format("{0:0.###}", value).Replace(",", "."))).
                                         ToArray()
                                 )
                               : null,

                           ConsumedEnergy    != null ? new XElement(OICPNS.Authorization + "ConsumedEnergy",    String.Format("{0:0.}", ConsumedEnergy).Replace(",", ".")) : null,
                           MeteringSignature != null ? new XElement(OICPNS.Authorization + "MeteringSignature", MeteringSignature)        : null,

                           HubOperatorId     != null ? new XElement(OICPNS.Authorization + "HubOperatorID",     HubOperatorId.ToString()) : null,
                           HubProviderId     != null ? new XElement(OICPNS.Authorization + "HubProviderID",     HubProviderId.ToString()) : null

                       )
                   );

        }

        #endregion

    }

}
