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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
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

        #region QRCodeIdentification

        private readonly eMAIdWithPIN _QRCodeIdentification;

        public eMAIdWithPIN QRCodeIdentification
        {
            get
            {
                return _QRCodeIdentification;
            }
        }

        #endregion

        #region PlugAndChargeIdentification

        private readonly eMA_Id _PlugAndChargeIdentification;

        public eMA_Id PlugAndChargeIdentification
        {
            get
            {
                return _PlugAndChargeIdentification;
            }
        }

        #endregion

        #region RemoteIdentification

        private readonly eMA_Id _RemoteIdentification;

        public eMA_Id RemoteIdentification
        {
            get
            {
                return _RemoteIdentification;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) AuthorizationIdentification(AuthToken)

        private AuthorizationIdentification(Auth_Token  AuthToken)
        {
            this._AuthToken                     = AuthToken;
        }

        #endregion

        #region (private) AuthorizationIdentification(QRCodeIdentification)

        private AuthorizationIdentification(eMAIdWithPIN QRCodeIdentification)
        {
            this._QRCodeIdentification = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthorizationIdentification(PlugAndChargeIdentification, IsPnC)

        private AuthorizationIdentification(eMA_Id PlugAndChargeIdentification,
                                           Boolean IsPnC)
        {
            this._PlugAndChargeIdentification   = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthorizationIdentification(RemoteIdentification)

        private AuthorizationIdentification(eMA_Id RemoteIdentification)
        {
            this._RemoteIdentification          = RemoteIdentification;
        }

        #endregion


        #region AuthorizationIdentification(AuthInfo)

        public AuthorizationIdentification(AuthInfo AuthInfo)
        {

            this._AuthToken                    = AuthInfo.AuthToken;
            this._QRCodeIdentification         = AuthInfo.QRCodeIdentification != null
                                                     ? new eMAIdWithPIN(AuthInfo.QRCodeIdentification.eMAId,
                                                                        AuthInfo.QRCodeIdentification.PIN,
                                                                        AuthInfo.QRCodeIdentification.Function,
                                                                        AuthInfo.QRCodeIdentification.Salt)
                                                     : null;
            this._PlugAndChargeIdentification  = AuthInfo.PlugAndChargeIdentification;
            this._RemoteIdentification         = AuthInfo.RemoteIdentification;

        }

        #endregion

        #endregion


        #region (static) FromAuthToken(AuthToken)

        public static AuthorizationIdentification FromAuthToken(Auth_Token AuthToken)
        {
            return new AuthorizationIdentification(AuthToken);
        }

        #endregion

        #region (static) FromQRCodeIdentification(eMAId, PIN)

        public static AuthorizationIdentification FromQRCodeIdentification(eMA_Id  eMAId,
                                                                           String  PIN)
        {
            return new AuthorizationIdentification(new eMAIdWithPIN(eMAId, PIN));
        }

        #endregion

        #region (static) FromQRCodeIdentification(QRCodeIdentification)

        public static AuthorizationIdentification FromQRCodeIdentification(eMAIdWithPIN QRCodeIdentification)
        {
            return new AuthorizationIdentification(QRCodeIdentification);
        }

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification)

        public static AuthorizationIdentification FromPlugAndChargeIdentification(eMA_Id PlugAndChargeIdentification)
        {
            return new AuthorizationIdentification(PlugAndChargeIdentification);
        }

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        public static AuthorizationIdentification FromRemoteIdentification(eMA_Id RemoteIdentification)
        {
            return new AuthorizationIdentification(RemoteIdentification);
        }

        #endregion


        #region (static) Parse(AuthorizationIdentificationXML, OnException = null)

        public static AuthorizationIdentification Parse(XElement             AuthorizationIdentificationXML,
                                                        OnExceptionDelegate  OnException  = null)
        {

            var RFIDmifarefamilyIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification");
            if (RFIDmifarefamilyIdentificationXML != null)
            {

                var UIDXML = RFIDmifarefamilyIdentificationXML.Element(OICPNS.CommonTypes + "UID");
                if (UIDXML == null)
                    throw new Exception("Missing 'UIDXML' XML tag!");

                return new AuthorizationIdentification(Auth_Token.Parse(RFIDmifarefamilyIdentificationXML.Value));

            }

            var QRCodeIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "QRCodeIdentification");
            if (QRCodeIdentificationXML != null)
            {

                return new AuthorizationIdentification(eMAIdWithPIN.Parse(QRCodeIdentificationXML));

            }

            var PlugAndChargeIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "PlugAndChargeIdentification");
            if (PlugAndChargeIdentificationXML != null)
            {

                return new AuthorizationIdentification(eMA_Id.Parse(PlugAndChargeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes + "EVCOID")), true);

            }

            var RemoteIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "PlugAndChargeIdentification");
            if (RemoteIdentificationXML != null)
            {

                return new AuthorizationIdentification(eMA_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes + "EVCOID")));

            }

            throw new Exception("Invalid 'AuthenticationData:Identification' XML tag!");

        }

        #endregion

        #region ToXML(XMLNamespace)

        public XElement ToXML(XNamespace XMLNamespace)
        {

            return new XElement(XMLNamespace + "Identification",

                _AuthToken != null
                    ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                          new XElement(OICPNS.CommonTypes + "UID", _AuthToken.ToString()))
                    : null,

                _QRCodeIdentification != null
                    ? _QRCodeIdentification.ToXML()
                    : null,

                _PlugAndChargeIdentification != null
                    ? new XElement(OICPNS.CommonTypes + "PlugAndChargeIdentification",
                          new XElement(OICPNS.CommonTypes + "EVCOID", _PlugAndChargeIdentification.ToString()))
                    : null,

                _RemoteIdentification != null
                    ? new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                          new XElement(OICPNS.CommonTypes + "EVCOID", _RemoteIdentification.ToString()))
                    : null);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (_AuthToken != null)
                return _AuthToken.ToString();

            if (_QRCodeIdentification != null)
                return _QRCodeIdentification.ToString();

            if (_PlugAndChargeIdentification != null)
                return _PlugAndChargeIdentification.ToString();

            if (_RemoteIdentification != null)
                return _RemoteIdentification.ToString();

            return String.Empty;

        }

        #endregion


    }

}
