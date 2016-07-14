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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// OICP CPO Client XML methods.
    /// </summary>
    public static class CPOClientXMLMethods
    {

        #region PushEVSEDataXML  (EVSEDataRecordsGroup,   OICPAction = fullLoad, OperatorId = null, OperatorName = null)

        public static XElement PushEVSEDataXML(IGrouping<EVSEOperator, EVSEDataRecord>  EVSEDataRecordsGroup,
                                               ActionType                               OICPAction    = ActionType.fullLoad,
                                               EVSEOperator_Id                          OperatorId    = null,
                                               String                                   OperatorName  = null)
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

            if (EVSEDataRecordsGroup == null || !EVSEDataRecordsGroup.Any())
                throw new ArgumentNullException(nameof(EVSEDataRecordsGroup),  "The given lookup of EVSE data records must not be null or empty!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPushEvseData",
                                      new XElement(OICPNS.EVSEData + "ActionType", OICPAction.ToString()),

                                          new XElement(OICPNS.EVSEData + "OperatorEvseData",

                                              new XElement(OICPNS.EVSEData + "OperatorID", (OperatorId ?? EVSEDataRecordsGroup.Key.Id).OriginId),

                                              (OperatorName.IsNotNullOrEmpty() || EVSEDataRecordsGroup.Key.Name.Any())
                                                  ? new XElement(OICPNS.EVSEData + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                        ? OperatorName
                                                                                                        : EVSEDataRecordsGroup.Key.Name.First().Text))
                                                  : null,

                                              // <EvseDataRecord> ... </EvseDataRecord>
                                              EVSEDataRecordsGroup.
                                                  Where (evsedatarecord => evsedatarecord != null).
                                                  Select(evsedatarecord => evsedatarecord.ToXML()).
                                                  ToArray()

                                          )

                                      ));

        }

        #endregion

        #region PushEVSEStatusXML(EVSEStatusRecordsGroup, OICPAction = update,   OperatorId = null, OperatorName = null)

        public static XElement PushEVSEStatusXML(IGrouping<EVSEOperator, EVSEStatusRecord>  EVSEStatusRecordsGroup,
                                                 ActionType                                 OICPAction    = ActionType.update,
                                                 EVSEOperator_Id                            OperatorId    = null,
                                                 String                                     OperatorName  = null)
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

            if (EVSEStatusRecordsGroup == null || !EVSEStatusRecordsGroup.Any())
                throw new ArgumentNullException(nameof(EVSEStatusRecordsGroup),  "The given enumeration of EVSE status must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPushEvseStatus",
                                      new XElement(OICPNS.EVSEStatus + "ActionType", OICPAction.ToString()),

                                      new XElement(OICPNS.EVSEStatus + "OperatorEvseStatus",

                                          new XElement(OICPNS.EVSEStatus + "OperatorID", (OperatorId ?? EVSEStatusRecordsGroup.Key.Id).OriginId),

                                          (OperatorName.IsNotNullOrEmpty() || EVSEStatusRecordsGroup.Key.Name.Any())
                                                  ? new XElement(OICPNS.EVSEStatus + "OperatorName", (OperatorName.IsNotNullOrEmpty()
                                                                                                         ? OperatorName
                                                                                                         : EVSEStatusRecordsGroup.Key.Name.First().Text))
                                                  : null,

                                          // <EvseStatusRecord> ... </EvseStatusRecord>
                                          EVSEStatusRecordsGroup.
                                              Where (evsestatusrecord => evsestatusrecord != null).
                                              Select(evsestatusrecord => evsestatusrecord.ToXML()).
                                              ToArray())

                                      ));

        }

        #endregion


        #region AuthorizeStartXML(OperatorId, AuthToken, EVSEId = null, PartnerProductId = null, SessionId = null, PartnerSessionId = null)

        /// <summary>
        /// Create an OICP Authorize Start XML request.
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

                                          SessionId        != null ? new XElement(OICPNS.Authorization + "SessionID",        SessionId.       ToString()) : null,
                                          PartnerSessionId != null ? new XElement(OICPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,

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
        /// Create an OICP AuthorizeStop XML request.
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
        /// Create an OICP PullAuthenticationData XML request.
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


    }

}
