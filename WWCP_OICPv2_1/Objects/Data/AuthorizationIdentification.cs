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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public class AuthorizationIdentification
    {

        #region Properties

        /// <summary>
        /// An authentication token.
        /// </summary>
        public Auth_Token    AuthToken                    { get; }

        /// <summary>
        /// An e-mobility account identification and its PIN.
        /// </summary>
        public eMAIdWithPIN  QRCodeIdentification         { get; }

        /// <summary>
        /// An e-mobility account identification (PnC).
        /// </summary>
        public eMobilityAccount_Id        PlugAndChargeIdentification  { get; }

        /// <summary>
        /// An e-mobility account identification.
        /// </summary>
        public eMobilityAccount_Id        RemoteIdentification         { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthorizationIdentification(AuthToken)

        private AuthorizationIdentification(Auth_Token  AuthToken)
        {
            this.AuthToken                     = AuthToken;
        }

        #endregion

        #region (private) AuthorizationIdentification(QRCodeIdentification)

        private AuthorizationIdentification(eMAIdWithPIN QRCodeIdentification)
        {
            this.QRCodeIdentification = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthorizationIdentification(PlugAndChargeIdentification, IsPnC)

        private AuthorizationIdentification(eMobilityAccount_Id PlugAndChargeIdentification,
                                           Boolean IsPnC)
        {
            this.PlugAndChargeIdentification   = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthorizationIdentification(RemoteIdentification)

        private AuthorizationIdentification(eMobilityAccount_Id RemoteIdentification)
        {
            this.RemoteIdentification          = RemoteIdentification;
        }

        #endregion


        #region AuthorizationIdentification(AuthInfo)

        public AuthorizationIdentification(AuthInfo AuthInfo)
        {

            this.AuthToken                    = AuthInfo.AuthToken;
            this.QRCodeIdentification         = AuthInfo.QRCodeIdentification != null
                                                    ? new eMAIdWithPIN(AuthInfo.QRCodeIdentification.eMAId,
                                                                       AuthInfo.QRCodeIdentification.PIN,
                                                                       AuthInfo.QRCodeIdentification.Function,
                                                                       AuthInfo.QRCodeIdentification.Salt)
                                                    : null;
            this.PlugAndChargeIdentification  = AuthInfo.PlugAndChargeIdentification;
            this.RemoteIdentification         = AuthInfo.RemoteIdentification;

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

        public static AuthorizationIdentification FromQRCodeIdentification(eMobilityAccount_Id  eMAId,
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

        public static AuthorizationIdentification FromPlugAndChargeIdentification(eMobilityAccount_Id PlugAndChargeIdentification)
        {
            return new AuthorizationIdentification(PlugAndChargeIdentification);
        }

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        public static AuthorizationIdentification FromRemoteIdentification(eMobilityAccount_Id RemoteIdentification)
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

                return new AuthorizationIdentification(eMobilityAccount_Id.Parse(PlugAndChargeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes + "EVCOID")), true);

            }

            var RemoteIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "RemoteIdentification");
            if (RemoteIdentificationXML != null)
            {

                return new AuthorizationIdentification(eMobilityAccount_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes + "EVCOID")));

            }

            throw new Exception("Invalid 'AuthenticationData:Identification' XML tag!");

        }

        #endregion

        #region ToXML(XMLNamespace)

        public XElement ToXML(XNamespace XMLNamespace)
        {

            return new XElement(XMLNamespace + "Identification",

                AuthToken != null
                    ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                          new XElement(OICPNS.CommonTypes + "UID", AuthToken.ToString()))
                    : null,

                QRCodeIdentification != null
                    ? QRCodeIdentification.ToXML()
                    : null,

                PlugAndChargeIdentification != null
                    ? new XElement(OICPNS.CommonTypes + "PlugAndChargeIdentification",
                          new XElement(OICPNS.CommonTypes + "EVCOID", PlugAndChargeIdentification.ToString()))
                    : null,

                RemoteIdentification != null
                    ? new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                          new XElement(OICPNS.CommonTypes + "EVCOID", RemoteIdentification.ToString()))
                    : null);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (AuthToken != null)
                return AuthToken.ToString();

            if (QRCodeIdentification != null)
                return QRCodeIdentification.ToString();

            if (PlugAndChargeIdentification != null)
                return PlugAndChargeIdentification.ToString();

            if (RemoteIdentification != null)
                return RemoteIdentification.ToString();

            return String.Empty;

        }

        #endregion


    }

}
