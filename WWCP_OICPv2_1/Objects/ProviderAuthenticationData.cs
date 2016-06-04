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
using System.Collections.Generic;
using System.Xml.Linq;

using System.Linq;
using System.Globalization;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP v2.0 provider authentication data record.
    /// </summary>
    public class ProviderAuthenticationData
    {

        #region Properties

        #region ProviderId

        private readonly EVSP_Id _ProviderId;

        /// <summary>
        /// The unique identification of an Electric Vehicle Service Provider.
        /// </summary>
        public EVSP_Id ProviderId
        {
            get
            {
                return _ProviderId;
            }
        }

        #endregion

        #region AuthorizationIdentifications

        private readonly IEnumerable<AuthorizationIdentification> _AuthorizationIdentifications;

        /// <summary>
        /// An enumeration of authorization identifications records.
        /// </summary>
        public IEnumerable<AuthorizationIdentification> AuthorizationIdentifications
        {
            get
            {
                return _AuthorizationIdentifications;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP v2.0 provider authentication data record.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an Electric Vehicle Service Provider.</param>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications records.</param>
        public ProviderAuthenticationData(EVSP_Id                                   ProviderId,
                                          IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications)
        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            this._ProviderId                    = ProviderId;
            this._AuthorizationIdentifications  = AuthorizationIdentifications != null ? AuthorizationIdentifications : new AuthorizationIdentification[0];

        }

        #endregion


        #region (static) Parse(ProviderAuthenticationDataXML, OnException = null)

        public static ProviderAuthenticationData Parse(XElement             ProviderAuthenticationDataXML,
                                                       OnExceptionDelegate  OnException = null)
        {

            #region Initial checks

            if (ProviderAuthenticationDataXML == null)
                return null;

            #endregion

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/v2.0"
            //                   xmlns:CommonTypes        = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //  [...]
            // 
            //    <!--Zero or more repetitions:-->
            //    <AuthenticationData:ProviderAuthenticationData>
            // 
            //       <AuthenticationData:ProviderID>DE*GDF</AuthenticationData:ProviderID>
            // 
            //       <!--Zero or more repetitions:-->
            //       <AuthenticationData:AuthenticationDataRecord>
            //          <AuthenticationData:Identification>
            // 
            //             <!--You have a CHOICE of the next 4 items at this level-->
            //             <CommonTypes:RFIDmifarefamilyIdentification>
            //                <CommonTypes:UID>08152305</CommonTypes:UID>
            //             </CommonTypes:RFIDmifarefamilyIdentification>
            // 
            //             <CommonTypes:QRCodeIdentification>
            // 
            //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            // 
            //                <!--You have a CHOICE of the next 2 items at this level-->
            //                <CommonTypes:PIN>?</CommonTypes:PIN>
            // 
            //                <CommonTypes:HashedPIN>
            //                   <CommonTypes:Value>f7cf02826ba923e3d31c1c3015899076</CommonTypes:Value>
            //                   <CommonTypes:Function>MD5|SHA-1</CommonTypes:Function>
            //                   <CommonTypes:Salt>22c7c09370af2a3f07fe8665b140498a</CommonTypes:Salt>
            //                </CommonTypes:HashedPIN>
            // 
            //             </CommonTypes:QRCodeIdentification>
            // 
            //             <CommonTypes:PlugAndChargeIdentification>
            //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //             </CommonTypes:PlugAndChargeIdentification>
            // 
            //             <CommonTypes:RemoteIdentification>
            //                <CommonTypes:EVCOID>DE*GDF*01234ABCD*Z</CommonTypes:EVCOID>
            //             </CommonTypes:RemoteIdentification>
            // 
            //          </AuthenticationData:Identification>
            //       </AuthenticationData:AuthenticationDataRecord>
            // 
            //    </AuthenticationData:ProviderAuthenticationData>
            // 
            //  [...]
            // 
            // </soapenv:Envelope>

            #endregion

            try
            {

                return new ProviderAuthenticationData(ProviderAuthenticationDataXML.MapValueOrFail(OICPNS.AuthenticationData + "ProviderID",
                                                                                                   EVSP_Id.Parse,
                                                                                                   "Missing ProviderID!"),

                                                      ProviderAuthenticationDataXML.Elements(OICPNS.AuthenticationData + "AuthenticationDataRecord").
                                                                                    Select(XML => XML.Element(OICPNS.AuthenticationData + "Identification")).
                                                                                    Where (XML => XML != null).
                                                                                    Select(XML => AuthorizationIdentification.Parse(XML))
                                                                                             //(EvseDataRecordXML, e) => AuthorizationIdentification.Parse(EvseDataRecordXML, e),
                                                                                             //OnException)
                                           );

            }
            catch (Exception e)
            {

                if (OnException != null)
                    OnException(DateTime.Now, ProviderAuthenticationDataXML, e);

                return null;

            }

        }

        #endregion

        #region (static) Parse(ProviderAuthenticationDataXMLs, OnException = null)

        public static IEnumerable<ProviderAuthenticationData> Parse(IEnumerable<XElement>  ProviderAuthenticationDataXMLs,
                                                                    OnExceptionDelegate    OnException = null)
        {

            #region Initial checks

            if (ProviderAuthenticationDataXMLs == null)
                return new ProviderAuthenticationData[0];

            var _ProviderAuthenticationDataXMLs = ProviderAuthenticationDataXMLs.ToArray();

            if (_ProviderAuthenticationDataXMLs.Length == 0)
                return new ProviderAuthenticationData[0];

            #endregion

            return ProviderAuthenticationDataXMLs.
                       Select(ProviderAuthenticationDataXML => ProviderAuthenticationData.Parse(ProviderAuthenticationDataXML, OnException)).
                       Where (OperatorEvseData    => OperatorEvseData != null);

        }

        #endregion

    }

}
