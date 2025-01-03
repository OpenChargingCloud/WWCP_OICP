/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extensions methods for authentication modes.
    /// </summary>
    public static class AuthenticationModesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an authentication mode.
        /// </summary>
        /// <param name="Text">A text representation of an authentication mode.</param>
        public static AuthenticationModes Parse(String Text)
        {

            if (TryParse(Text, out var authenticationMode))
                return authenticationMode;

            throw new ArgumentException("Undefined authentication mode '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an authentication mode.
        /// </summary>
        /// <param name="Text">A text representation of an authentication mode.</param>
        public static AuthenticationModes? TryParse(String Text)
        {

            if (TryParse(Text, out var authenticationMode))
                return authenticationMode;

            return default;

        }

        #endregion

        #region TryParse(Text, out AuthenticationMode)

        /// <summary>
        /// Parses the given text representation of an authentication mode.
        /// </summary>
        /// <param name="Text">A text representation of an authentication mode.</param>
        /// <param name="AuthenticationMode">The parsed authentication mode</param>
        public static Boolean TryParse(String Text, out AuthenticationModes AuthenticationMode)
        {
            switch (Text?.Trim())
            {

                case "NFC RFID Classic":
                    AuthenticationMode = AuthenticationModes.NFC_RFID_Classic;
                    return true;

                case "NFC RFID DESFire":
                    AuthenticationMode = AuthenticationModes.NFC_RFID_DESFire;
                    return true;

                case "PnC":
                    AuthenticationMode = AuthenticationModes.PnC;
                    return true;

                case "REMOTE":
                    AuthenticationMode = AuthenticationModes.REMOTE;
                    return true;

                case "Direct Payment":
                    AuthenticationMode = AuthenticationModes.DirectPayment;
                    return true;

                case "No Authentication Required":
                    AuthenticationMode = AuthenticationModes.NoAuthenticationRequired;
                    return true;

                default:
                    AuthenticationMode = AuthenticationModes.NoAuthenticationRequired;
                    return false;

            }
        }

        #endregion

        #region AsString(AuthenticationMode)

        /// <summary>
        /// Return a text representation of the given authentication mode.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode.</param>
        /// <returns></returns>
        public static String AsString(this AuthenticationModes AuthenticationMode)

            => AuthenticationMode switch {
                   AuthenticationModes.NFC_RFID_Classic          => "NFC RFID Classic",
                   AuthenticationModes.NFC_RFID_DESFire          => "NFC RFID DESFire",
                   AuthenticationModes.PnC                       => "PnC",
                   AuthenticationModes.REMOTE                    => "REMOTE",
                   AuthenticationModes.DirectPayment             => "Direct Payment",
                   AuthenticationModes.NoAuthenticationRequired  => "No Authentication Required",
                   _                                             => "Undefined"
               };

        #endregion

    }


    /// <summary>
    /// The supported ways of ev driver authentication.
    /// </summary>
    public enum AuthenticationModes
    {

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