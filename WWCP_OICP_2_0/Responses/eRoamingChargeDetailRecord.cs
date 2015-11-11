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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A group of OICP v2.0 provider authentication data records.
    /// </summary>
    public class eRoamingChargeDetailRecord
    {

        #region Properties

        #region ProviderAuthenticationDataRecords

        private readonly IEnumerable<ProviderAuthenticationData> _ProviderAuthenticationDataRecords;

        /// <summary>
        /// An enumeration of provider authentication data records.
        /// </summary>
        public IEnumerable<ProviderAuthenticationData> ProviderAuthenticationDataRecords
        {
            get
            {
                return _ProviderAuthenticationDataRecords;
            }
        }

        #endregion

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group of OICP v2.0 provider authentication data records.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        public eRoamingChargeDetailRecord(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                          StatusCode                               StatusCode  = null)
        {

            #region Initial checks

            if (ProviderAuthenticationDataRecords == null)
                throw new ArgumentNullException("ProviderAuthenticationDataRecords", "The given parameter must not be null!");

            #endregion

            this._ProviderAuthenticationDataRecords  = ProviderAuthenticationDataRecords;
            this._StatusCode                         = StatusCode != null ? StatusCode : new StatusCode(0);

        }

        #endregion


        #region (static) Parse(eRoamingAuthenticationDataXML)

        public static eRoamingChargeDetailRecord Parse(XElement eRoamingAuthenticationDataXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
            //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            //       <AuthenticationData:eRoamingAuthenticationData>
            // 
            //          <AuthenticationData:AuthenticationData>
            // 
            //             <!--Zero or more repetitions:-->
            //             <AuthenticationData:ProviderAuthenticationData>
            // 
            //                <AuthenticationData:ProviderID>DE*GDF</AuthenticationData:ProviderID>
            // 
            //                <!--Zero or more repetitions:-->
            //                <AuthenticationData:AuthenticationDataRecord>
            //                   <AuthenticationData:Identification>
            // 
            //                      <!--You have a CHOICE of the next 4 items at this level-->
            //                      <CommonTypes:RFIDmifarefamilyIdentification>
            //                         <CommonTypes:UID>08152305</CommonTypes:UID>
            //                      </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //                      <CommonTypes:QRCodeIdentification>
            // 
            //                         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            // 
            //                         <!--You have a CHOICE of the next 2 items at this level-->
            //                         <CommonTypes:PIN>?</CommonTypes:PIN>
            // 
            //                         <CommonTypes:HashedPIN>
            //                            <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
            //                            <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
            //                            <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
            //                         </CommonTypes:HashedPIN>
            // 
            //                      </CommonTypes:QRCodeIdentification>
            // 
            //                      <CommonTypes:PlugAndChargeIdentification>
            //                         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //                      </CommonTypes:PlugAndChargeIdentification>
            // 
            //                      <CommonTypes:RemoteIdentification>
            //                         <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //                      </CommonTypes:RemoteIdentification>
            // 
            //                   </AuthenticationData:Identification>
            //                </AuthenticationData:AuthenticationDataRecord>
            // 
            //             </AuthenticationData:ProviderAuthenticationData>
            //          </AuthenticationData:AuthenticationData>
            // 
            //          <!--Optional:-->
            //          <AuthenticationData:StatusCode>
            // 
            //             <CommonTypes:Code>?</CommonTypes:Code>
            // 
            //             <!--Optional:-->
            //             <CommonTypes:Description>?</CommonTypes:Description>
            // 
            //             <!--Optional:-->
            //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
            // 
            //          </AuthenticationData:StatusCode>
            // 
            //       </AuthenticationData:eRoamingAuthenticationData>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion


            if (eRoamingAuthenticationDataXML.Name != OICPNS.AuthenticationData + "eRoamingAuthenticationData")
                throw new Exception("Invalid eRoamingAuthenticationData XML!");

            var AuthenticationDataXML  = eRoamingAuthenticationDataXML.Element(OICPNS.AuthenticationData + "AuthenticationData");
            var StatusCodeXML          = eRoamingAuthenticationDataXML.Element(OICPNS.AuthenticationData + "StatusCode");

           // if (AuthenticationDataXML != null)
           //     return new eRoamingAuthenticationData(AuthenticationDataXML.
           //                                               Elements  (OICPNS.AuthenticationData + "ProviderAuthenticationData").
           //                                               SafeSelect(ProviderAuthenticationDataXML => ProviderAuthenticationData.Parse(ProviderAuthenticationDataXML)).
           //                                               Where     (ProviderAuthenticationData    => ProviderAuthenticationData != null),
           //                                           StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);
           //
           //
           // return new eRoamingAuthenticationData(StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);

            return null;

        }

        #endregion

        #region (static) ParseXML(eRoamingChargeDetailRecordXML)

        public static IEnumerable<eRoamingChargeDetailRecord> ParseXML(XElement eRoamingChargeDetailRecordXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            //       <Authorization:eRoamingChargeDetailRecords>
            // 
            //          <!--Zero or more repetitions:-->
            //          <Authorization:eRoamingChargeDetailRecord>
            // 
            //             <Authorization:SessionID>de164e08-1c88-1293-537b-be355041070e</Authorization:SessionID>
            // 
            //             <!--Optional:-->
            //             <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
            // 
            //             <!--Optional:-->
            //             <Authorization:PartnerProductID>AC1</Authorization:PartnerProductID>
            // 
            //             <Authorization:EvseID>DE*GEF*E123456789*1</Authorization:EvseID>
            // 
            //             <Authorization:Identification>
            //               <!--You have a CHOICE of the next 4 items at this level-->
            // 
            //               <CommonTypes:RFIDmifarefamilyIdentification>
            //                  <CommonTypes:UID>08152305</CommonTypes:UID>
            //               </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //               <CommonTypes:QRCodeIdentification>
            // 
            //                  <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            // 
            //                  <!--You have a CHOICE of the next 2 items at this level-->
            //                  <CommonTypes:PIN>1234</CommonTypes:PIN>
            // 
            //                  <CommonTypes:HashedPIN>
            //                     <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
            //                     <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
            //                     <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
            //                  </CommonTypes:HashedPIN>
            // 
            //               </CommonTypes:QRCodeIdentification>
            // 
            //               <CommonTypes:PlugAndChargeIdentification>
            //                  <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //               </CommonTypes:PlugAndChargeIdentification>
            // 
            //               <CommonTypes:RemoteIdentification>
            //                  <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //               </CommonTypes:RemoteIdentification>
            // 
            //             </Authorization:Identification>
            // 
            //             <!--Optional:-->
            //             <Authorization:ChargingStart>2015-10-23T15:45:30.000Z</Authorization:ChargingStart>
            //             <!--Optional:-->
            //             <Authorization:ChargingEnd>2015-10-23T16:59:31.000Z</Authorization:ChargingEnd>
            // 
            //             <Authorization:SessionStart>2015-10-23T15:45:00.000Z</Authorization:SessionStart>
            //             <Authorization:SessionEnd>2015-10-23T17:45:00.000Z</Authorization:SessionEnd>
            // 
            //             <!--Optional:-->
            //             <Authorization:MeterValueStart>123.456</Authorization:MeterValueStart>
            //             <!--Optional:-->
            //             <Authorization:MeterValueEnd>234.567</Authorization:MeterValueEnd>
            //             <!--Optional:-->
            //             <Authorization:MeterValueInBetween>
            //               <!--1 or more repetitions: \d\.\d{0,3} -->
            //               <Authorization:MeterValue>123.456</Authorization:MeterValue>
            //               <Authorization:MeterValue>189.768</Authorization:MeterValue>
            //               <Authorization:MeterValue>223.312</Authorization:MeterValue>
            //               <Authorization:MeterValue>234.560</Authorization:MeterValue>
            //               <Authorization:MeterValue>234.567</Authorization:MeterValue>
            //             </Authorization:MeterValueInBetween>
            // 
            //             <!--Optional:-->
            //             <Authorization:ConsumedEnergy>111.111</Authorization:ConsumedEnergy>
            //             <!--Optional:-->
            //             <Authorization:MeteringSignature>?</Authorization:MeteringSignature>
            // 
            //             <!--Optional:-->
            //             <Authorization:HubOperatorID>?</Authorization:HubOperatorID>
            //             <!--Optional:-->
            //             <Authorization:HubProviderID>?</Authorization:HubProviderID>
            // 
            //          </Authorization:eRoamingChargeDetailRecord>
            // 
            //       </Authorization:eRoamingChargeDetailRecords>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion


            if (eRoamingChargeDetailRecordXML.Name != OICPNS.Authorization + "eRoamingChargeDetailRecords")
                throw new Exception("Invalid eRoamingChargeDetailRecord XML!");

            var ChargeDetailRecordsXMLs = eRoamingChargeDetailRecordXML.Elements(OICPNS.Authorization + "eRoamingChargeDetailRecord");
            //var StatusCodeXML = eRoamingChargeDetailRecordXML.Element(OICPNS.AuthenticationData + "StatusCode");

            // if (AuthenticationDataXML != null)
            //     return new eRoamingAuthenticationData(AuthenticationDataXML.
            //                                               Elements  (OICPNS.AuthenticationData + "ProviderAuthenticationData").
            //                                               SafeSelect(ProviderAuthenticationDataXML => ProviderAuthenticationData.Parse(ProviderAuthenticationDataXML)).
            //                                               Where     (ProviderAuthenticationData    => ProviderAuthenticationData != null),
            //                                           StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);
            //
            //
            // return new eRoamingAuthenticationData(StatusCodeXML != null ? StatusCode.Parse(StatusCodeXML) : null);

            return null;

        }

        #endregion


    }

}
