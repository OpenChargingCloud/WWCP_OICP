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
    /// A group of OICP provider authentication data records.
    /// </summary>
    public class eRoamingAuthenticationData
    {

        #region Properties

        /// <summary>
        /// An enumeration of provider authentication data records.
        /// </summary>
        public IEnumerable<ProviderAuthenticationData> ProviderAuthenticationDataRecords  { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode                              StatusCode                         { get; }

        #endregion

        #region Constructor(s)

        #region eRoamingAuthenticationData(ProviderAuthenticationDataRecords, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP provider authentication data records.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        public eRoamingAuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                          StatusCode                               StatusCode  = null)
        {

            #region Initial checks

            if (ProviderAuthenticationDataRecords == null)
                throw new ArgumentNullException(nameof(ProviderAuthenticationDataRecords),  "The given provider authentication data records must not be null!");

            #endregion

            this.ProviderAuthenticationDataRecords  = ProviderAuthenticationDataRecords;
            this.StatusCode                         = StatusCode ?? new StatusCode(StatusCodes.SystemError);

        }

        #endregion

        #region eRoamingAuthenticationData(Code, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new group of OICP provider authentication data records.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        public eRoamingAuthenticationData(StatusCodes  Code,
                                          String       Description     = null,
                                          String       AdditionalInfo  = null)

        {

            this.ProviderAuthenticationDataRecords  = new ProviderAuthenticationData[0];
            this.StatusCode                         = new StatusCode(Code,
                                                                     Description,
                                                                     AdditionalInfo);

        }

        #endregion

        #endregion


        #region (static) Parse(eRoamingAuthenticationDataXML)

        public static eRoamingAuthenticationData Parse(XElement eRoamingAuthenticationDataXML)
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

            var AuthenticationDataXML  = eRoamingAuthenticationDataXML.Element   (OICPNS.AuthenticationData + "AuthenticationData");
            var StatusCodeXML          = eRoamingAuthenticationDataXML.MapElement(OICPNS.AuthenticationData + "StatusCode",
                                                                                  StatusCode.Parse);

            if (AuthenticationDataXML != null)
                return new eRoamingAuthenticationData(AuthenticationDataXML.
                                                          Elements  (OICPNS.AuthenticationData + "ProviderAuthenticationData").
                                                          SafeSelect(ProviderAuthenticationDataXML => ProviderAuthenticationData.Parse(ProviderAuthenticationDataXML)).
                                                          Where     (ProviderAuthenticationData    => ProviderAuthenticationData != null),
                                                      StatusCodeXML);


            return StatusCodeXML != null
                       ? new eRoamingAuthenticationData(StatusCodeXML.Code,
                                                        StatusCodeXML.Description,
                                                        StatusCodeXML.AdditionalInfo)
                       : new eRoamingAuthenticationData(StatusCodes.DataError);

        }

        #endregion


    }

}
