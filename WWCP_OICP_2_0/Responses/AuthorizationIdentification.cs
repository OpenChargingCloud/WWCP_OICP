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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    public class AuthorizationIdentification
    {

        #region Properties

        #region AuthToken

        private readonly Auth_Token _AuthToken;

        public Auth_Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        #endregion

        #endregion


        public AuthorizationIdentification(Auth_Token  AuthToken = null)
        {

            this._AuthToken     = AuthToken;

        }


        public static AuthorizationIdentification Parse(XElement             AuthorizationIdentificationXML,
                                                        OnExceptionDelegate  OnException  = null)
        {

            Auth_Token _AuthToken = null;

            var RFIDmifarefamilyIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification");
            if (RFIDmifarefamilyIdentificationXML != null)
            {

                var UIDXML = RFIDmifarefamilyIdentificationXML.Element(OICPNS.CommonTypes + "UID");

                if (RFIDmifarefamilyIdentificationXML != null)
                    _AuthToken = Auth_Token.Parse(RFIDmifarefamilyIdentificationXML.Value);

            }

            return new AuthorizationIdentification(_AuthToken);

        }

    }

}
