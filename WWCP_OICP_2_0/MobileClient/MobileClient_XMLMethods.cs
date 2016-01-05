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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 Mobile management methods.
    /// </summary>
    public static class MobileClient_XMLMethods
    {

        #region MobileAuthorizeStartXML(EVSEId, eMAIdWithPIN, PartnerProductId = null, GetNewSession = null)

        /// <summary>
        /// Create a new MobileAuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="eMAIdWithPIN">The eMA identification with its PIN.</param>
        /// <param name="PIN">The PIN for the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        public static XElement MobileAuthorizeStartXML(EVSE_Id       EVSEId,
                                                       eMAIdWithPIN  eMAIdWithPIN,
                                                       String        PartnerProductId  = null,
                                                       Boolean?      GetNewSession     = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:MobileAuthorization = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0"
            //                   xmlns:CommonTypes         = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <MobileAuthorization:eRoamingMobileAuthorizeStart>
            // 
            //          <MobileAuthorization:EvseID>DE*GEF*E123456789*1</MobileAuthorization:EvseID>
            // 
            //          <MobileAuthorization:QRCodeIdentification>
            // 
            //             <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            // 
            //             <!--You have a CHOICE of the next 2 items at this level-->
            //             <CommonTypes:PIN>1234</CommonTypes:PIN>
            // 
            //             <CommonTypes:HashedPIN>
            //                <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
            //                <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
            //                <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
            //             </CommonTypes:HashedPIN>
            // 
            //          </MobileAuthorization:QRCodeIdentification>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:PartnerProductID>AC1</MobileAuthorization:PartnerProductID>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:GetNewSession>?</MobileAuthorization:GetNewSession>
            // 
            //       </MobileAuthorization:eRoamingMobileAuthorizeStart>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            if (eMAIdWithPIN == null)
                throw new ArgumentNullException("eMAIdWithPIN", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart",

                                          new XElement(OICPNS.MobileAuthorization + "EvseID", EVSEId.OriginId.ToString()),

                                          eMAIdWithPIN.ToXML(OICPNS.MobileAuthorization),

                                          (PartnerProductId != null)
                                              ? new XElement(OICPNS.MobileAuthorization + "PartnerProductID", PartnerProductId.ToString())
                                              : null,

                                          (GetNewSession != null && GetNewSession.HasValue)
                                              ? new XElement(OICPNS.MobileAuthorization + "GetNewSession", GetNewSession.Value ? "true" : "false")
                                              : null

                                     ));

        }

        #endregion

        #region MobileRemoteStartXML(SessionId)

        /// <summary>
        /// Create a new MobileRemoteStart request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        public static XElement MobileRemoteStartXML(ChargingSession_Id  SessionId)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:MobileAuthorization = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <MobileAuthorization:eRoamingMobileRemoteStart>
            //          <MobileAuthorization:SessionID>de164e08-1c88-1293-537b-be355041070e</MobileAuthorization:SessionID>
            //       </MobileAuthorization:eRoamingMobileRemoteStart>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException("SessionId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart",
                                          new XElement(OICPNS.MobileAuthorization + "SessionID", SessionId.ToString())
                                     ));

        }

        #endregion

        #region MobileRemoteStopXML(SessionId)

        /// <summary>
        /// Create a new MobileRemoteStop request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        public static XElement MobileRemoteStopXML(ChargingSession_Id  SessionId)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:MobileAuthorization = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0">
            //
            //    <soapenv:Header/>
            //
            //    <soapenv:Body>
            //       <MobileAuthorization:eRoamingMobileRemoteStop>
            //          <MobileAuthorization:SessionID>de164e08-1c88-1293-537b-be355041070e</MobileAuthorization:SessionID>
            //       </MobileAuthorization:eRoamingMobileRemoteStop>
            //    </soapenv:Body>
            //
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException("SessionId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop",
                                          new XElement(OICPNS.MobileAuthorization + "SessionID", SessionId.ToString())
                                     ));

        }

        #endregion

    }

}
