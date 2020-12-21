/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extentions methods for authentication modes.
    /// </summary>
    public static class AuthenticationModesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of an authentication mode.
        /// </summary>
        /// <param name="Text">A text-representation of an authentication mode.</param>
        public static AuthenticationModes Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "NFC RFID Classic"           : return AuthenticationModes.NFC_RFID_Classic;
                case "NFC RFID DESFire"           : return AuthenticationModes.NFC_RFID_DESFire;
                case "PnC"                        : return AuthenticationModes.PnC;
                case "REMOTE"                     : return AuthenticationModes.REMOTE;
                case "Direct Payment"             : return AuthenticationModes.DirectPayment;
                case "No Authentication Required" : return AuthenticationModes.NoAuthenticationRequired;
                default                           : return AuthenticationModes.Unknown;
            }
        }

        #endregion

        #region AsString(AuthenticationMode)

        /// <summary>
        /// Return a text-representation of the given authentication mode.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode.</param>
        /// <returns></returns>
        public static String AsString(this AuthenticationModes AuthenticationMode)
        {
            switch (AuthenticationMode)
            {
                case AuthenticationModes.NFC_RFID_Classic         : return "NFC RFID Classic";
                case AuthenticationModes.NFC_RFID_DESFire         : return "NFC RFID DESFire";
                case AuthenticationModes.PnC                      : return "PnC";
                case AuthenticationModes.REMOTE                   : return "REMOTE";
                case AuthenticationModes.DirectPayment            : return "Direct Payment";
                case AuthenticationModes.NoAuthenticationRequired : return "No Authentication Required";
                default                                           : return "Unkown";
            }
        }

        #endregion

    }


    /// <summary>
    /// The supported ways of ev driver authentication.
    /// </summary>
    public enum AuthenticationModes
    {

        /// <summary>
        /// Unknown authentication mode
        /// </summary>
        Unknown,

        /// <summary>
        /// Using MiFare classic RFID cards
        /// </summary>
        NFC_RFID_Classic,

        /// <summary>
        /// Using MiFare DESFire RFID cards
        /// </summary>
        NFC_RFID_DESFire,

        /// <summary>
        /// Using ISO/IEC 15118 Plug-and-Charge (PnC)
        /// </summary>
        PnC,

        /// <summary>
        /// Using an eMAId/EVCOId via an app, QR-code or phone
        /// </summary>
        REMOTE,

        /// <summary>
        /// Direct payment, e.g. via InterCharge Direct
        /// </summary>
        DirectPayment,

        /// <summary>
        /// No Authentication Required
        /// </summary>
        NoAuthenticationRequired

    }

}