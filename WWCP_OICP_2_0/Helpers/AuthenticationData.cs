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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    public class AuthenticationData2
    { }

    public class AuthenticationData : ILookup<EVSE_Id, AuthenticationData2>
    {

        #region Data

        private readonly Dictionary<EVSE_Id, IEnumerable<AuthenticationData2>>  _AuthenticationData;

        #endregion

        #region Constructor(s)

        public AuthenticationData(Dictionary<EVSE_Id, IEnumerable<AuthenticationData2>>  Data = null)
        {

            _AuthenticationData = Data != null ? Data : new Dictionary<EVSE_Id, IEnumerable<AuthenticationData2>>();

        }

        #endregion


        public static AuthenticationData Parse(XElement XML)
        {

            #region Documentation

            // <AuthenticationData:AuthenticationData>
            // 
            //    <!--Zero or more repetitions:-->
            //    <AuthenticationData:ProviderAuthenticationData>
            // 
            //       <AuthenticationData:ProviderID>?</AuthenticationData:ProviderID>
            // 
            //       <!--Zero or more repetitions:-->
            //       <AuthenticationData:AuthenticationDataRecord>
            //          <AuthenticationData:Identification>
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
            //          </AuthenticationData:Identification>
            //       </AuthenticationData:AuthenticationDataRecord>
            // 
            //    </AuthenticationData:ProviderAuthenticationData>
            // </AuthenticationData:AuthenticationData>

            #endregion

            var __AuthenticationData = new Dictionary<EVSE_Id, IEnumerable<AuthenticationData2>>();
//            __AuthenticationData.Add()

            return new AuthenticationData(__AuthenticationData);

        }


        public Boolean Contains(EVSE_Id key)
        {
            return _AuthenticationData.ContainsKey(key);
        }

        public Int32 Count
        {
            get
            {
                return _AuthenticationData.Count;
            }
        }

        public IEnumerable<AuthenticationData2> this[EVSE_Id key]
        {
            get
            {

                IEnumerable<AuthenticationData2> AuthenticationData2 = null;
                if (_AuthenticationData.TryGetValue(key, out AuthenticationData2))
                    return AuthenticationData2;

                return new AuthenticationData2[0];

            }
        }

        public IEnumerator<IGrouping<EVSE_Id, AuthenticationData2>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }

}
