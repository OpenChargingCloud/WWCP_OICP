/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An identification for authorization.
    /// </summary>
    public class AuthorizationIdentification
    {

        #region Properties

        /// <summary>
        /// An RFID Id.
        /// </summary>
        public UID?            RFIDId                       { get; }

        /// <summary>
        /// An e-mobility account identification and its PIN.
        /// </summary>
        public EVCOIdWithPIN?  QRCodeIdentification         { get; }

        /// <summary>
        /// An e-mobility account identification (PnC).
        /// </summary>
        public EVCO_Id?        PlugAndChargeIdentification  { get; }

        /// <summary>
        /// An e-mobility account identification.
        /// </summary>
        public EVCO_Id?        RemoteIdentification         { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthorizationIdentification(RFIDId)

        private AuthorizationIdentification(UID RFIDId)
        {
            this.RFIDId  = RFIDId;
        }

        #endregion

        #region (private) AuthorizationIdentification(QRCodeIdentification)

        private AuthorizationIdentification(EVCOIdWithPIN QRCodeIdentification)
        {
            this.QRCodeIdentification  = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthorizationIdentification(PlugAndChargeIdentification, IsPnC)

        private AuthorizationIdentification(EVCO_Id  PlugAndChargeIdentification,
                                            Boolean              IsPnC)
        {
            this.PlugAndChargeIdentification  = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthorizationIdentification(RemoteIdentification)

        private AuthorizationIdentification(EVCO_Id  RemoteIdentification)
        {
            this.RemoteIdentification  = RemoteIdentification;
        }

        #endregion

        #endregion


        #region (static) FromRFIDId(RFIDId)

        public static AuthorizationIdentification FromRFIDId(UID RFIDId)

            => new AuthorizationIdentification(RFIDId);

        #endregion

        #region (static) FromQRCodeIdentification(EVCOId, PIN)

        public static AuthorizationIdentification FromQRCodeIdentification(EVCO_Id  EVCOId,
                                                                           String   PIN)

            => new AuthorizationIdentification(new EVCOIdWithPIN(EVCOId, PIN));

        #endregion

        #region (static) FromQRCodeIdentification(QRCodeIdentification)

        public static AuthorizationIdentification FromQRCodeIdentification(EVCOIdWithPIN  QRCodeIdentification)

            => new AuthorizationIdentification(QRCodeIdentification);

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification)

        public static AuthorizationIdentification FromPlugAndChargeIdentification(EVCO_Id  PlugAndChargeIdentification)

            => new AuthorizationIdentification(PlugAndChargeIdentification);

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        public static AuthorizationIdentification FromRemoteIdentification(EVCO_Id  RemoteIdentification)

            => new AuthorizationIdentification(RemoteIdentification);

        #endregion


        #region (static) Parse(AuthorizationIdentificationXML, OnException = null)

        /// <summary>
        /// Parse the givem XML as an OICP authorization identification.
        /// </summary>
        /// <param name="AuthorizationIdentificationXML">A XML representation of an OICP authorization identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationIdentification Parse(XElement             AuthorizationIdentificationXML,
                                                        OnExceptionDelegate  OnException  = null)
        {

            var RFIDmifarefamilyIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification");
            if (RFIDmifarefamilyIdentificationXML != null)
            {

                var UIDXML = RFIDmifarefamilyIdentificationXML.Element(OICPNS.CommonTypes + "UID");
                if (UIDXML == null)
                    throw new Exception("Missing 'UIDXML' XML tag!");

                return new AuthorizationIdentification(UID.Parse(UIDXML.Value));

            }

            var QRCodeIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "QRCodeIdentification");
            if (QRCodeIdentificationXML != null)
            {

                return new AuthorizationIdentification(EVCOIdWithPIN.Parse(QRCodeIdentificationXML));

            }

            var PlugAndChargeIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "PlugAndChargeIdentification");
            if (PlugAndChargeIdentificationXML != null)
            {

                return new AuthorizationIdentification(EVCO_Id.Parse(PlugAndChargeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes + "EVCOID")), true);

            }

            var RemoteIdentificationXML = AuthorizationIdentificationXML.Element(OICPNS.CommonTypes + "RemoteIdentification");
            if (RemoteIdentificationXML != null)
            {

                return new AuthorizationIdentification(EVCO_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes + "EVCOID")));

            }

            throw new Exception("Invalid 'AuthenticationData:Identification' XML tag!");

        }

        #endregion

        #region ToXML(XMLNamespace)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XMLNamespace">The XML namespace to use.</param>
        public XElement ToXML(XNamespace XMLNamespace)

            => new XElement(XMLNamespace + "Identification",

                   RFIDId.HasValue
                       ? new XElement(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification",
                             new XElement(OICPNS.CommonTypes + "UID", RFIDId.ToString()))
                       : null,

                   QRCodeIdentification.HasValue
                       ? QRCodeIdentification.Value.ToXML()
                       : null,

                   PlugAndChargeIdentification.HasValue
                       ? new XElement(OICPNS.CommonTypes + "PlugAndChargeIdentification",
                             new XElement(OICPNS.CommonTypes + "EVCOID", PlugAndChargeIdentification.ToString()))
                       : null,

                   RemoteIdentification.HasValue
                       ? new XElement(OICPNS.CommonTypes + "RemoteIdentification",
                             new XElement(OICPNS.CommonTypes + "EVCOID", RemoteIdentification.ToString()))
                       : null);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (RFIDId.HasValue)
                return RFIDId.ToString();

            if (QRCodeIdentification.HasValue)
                return QRCodeIdentification.ToString();

            if (PlugAndChargeIdentification.HasValue)
                return PlugAndChargeIdentification.ToString();

            if (RemoteIdentification.HasValue)
                return RemoteIdentification.ToString();

            return String.Empty;

        }

        #endregion


    }

}
