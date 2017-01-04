/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A group of OICP provider authentication data records.
    /// </summary>
    public class AuthenticationData
    {

        #region Properties

        /// <summary>
        /// An enumeration of provider authentication data records.
        /// </summary>
        public IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords    { get; }

        /// <summary>
        /// The status code for this request.
        /// </summary>
        public StatusCode                               StatusCode                           { get; }

        #endregion

        #region Constructor(s)

        #region AuthenticationData(ProviderAuthenticationDataRecords, StatusCode  = null)

        /// <summary>
        /// Create a new group of OICP provider authentication data records.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="StatusCode">An optional status code for this request.</param>
        public AuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
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

        #region AuthenticationData(Code, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new group of OICP provider authentication data records.
        /// </summary>
        /// <param name="Code">The result code of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="AdditionalInfo">An optional additional information.</param>
        public AuthenticationData(StatusCodes  Code,
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

        #region (static) Parse(eRoamingAuthenticationDataXML)

        /// <summary>
        /// Parse the givem XML as an OICP provider authentication data records.
        /// </summary>
        /// <param name="AuthenticationDataXML">A XML representation of OICP provider authentication data records.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthenticationData Parse(XElement             AuthenticationDataXML,
                                               OnExceptionDelegate  OnException  = null)
        {

            if (AuthenticationDataXML.Name != OICPNS.AuthenticationData + "eRoamingAuthenticationData")
                throw new Exception("Invalid eRoamingAuthenticationData XML!");

            var _AuthenticationDataXML  = AuthenticationDataXML.Element   (OICPNS.AuthenticationData + "AuthenticationData");
            var _StatusCode             = AuthenticationDataXML.MapElement(OICPNS.AuthenticationData + "StatusCode",
                                                                           StatusCode.Parse);

            if (_AuthenticationDataXML != null)
                return new AuthenticationData(_AuthenticationDataXML.
                                                  Elements  (OICPNS.AuthenticationData + "ProviderAuthenticationData").
                                                  SafeSelect(ProviderAuthenticationDataXML => ProviderAuthenticationData.Parse(ProviderAuthenticationDataXML)).
                                                  Where     (ProviderAuthenticationData    => ProviderAuthenticationData != null),
                                              _StatusCode);


            return _StatusCode != null

                       ? new AuthenticationData(_StatusCode.Code,
                                                _StatusCode.Description,
                                                _StatusCode.AdditionalInfo)

                       : new AuthenticationData(StatusCodes.DataError);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => (ProviderAuthenticationDataRecords != null ? ProviderAuthenticationDataRecords.Count().ToString() : "0") + " Authentication data records";

        #endregion

    }

}
