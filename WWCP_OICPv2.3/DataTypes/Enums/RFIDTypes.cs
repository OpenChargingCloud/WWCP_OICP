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
    /// Extentions methods for RFID types.
    /// </summary>
    public static class RFIDTypesExtentions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text-representation of a RFID type.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID type.</param>
        public static RFIDTypes Parse(String Text)
        {

            if (TryParse(Text, out RFIDTypes rfidType))
                return rfidType;

            throw new ArgumentException("Undefined RFID type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text-representation of a RFID type.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID type.</param>
        public static RFIDTypes? TryParse(String Text)
        {

            if (TryParse(Text, out RFIDTypes rfidType))
                return rfidType;

            return default;

        }

        #endregion

        #region TryParse(Text, out RFIDType)

        /// <summary>
        /// Parses the given text-representation of a RFID type.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID type.</param>
        /// <param name="RFIDType">The parsed RFID type.</param>
        public static Boolean TryParse(String Text, out RFIDTypes RFIDType)
        {
            switch (Text?.Trim())
            {

                case "mifareCls":
                    RFIDType = RFIDTypes.MifareClassic;
                    return true;

                case "mifareDes":
                    RFIDType = RFIDTypes.MifareDESFire;
                    return true;

                case "calypso":
                    RFIDType = RFIDTypes.Calypso;
                    return true;

                case "nfc":
                    RFIDType = RFIDTypes.NFC;
                    return true;

                case "mifareFamily":
                    RFIDType = RFIDTypes.MifareFamily;
                    return true;

                default:
                    RFIDType = RFIDTypes.MifareFamily;
                    return false;

            };
        }

        #endregion

        #region AsString(RFIDType)

        /// <summary>
        /// Return a text-representation of the given RFID type.
        /// </summary>
        /// <param name="RFIDType">A RFID type.</param>
        public static String AsString(this RFIDTypes RFIDType)

            => RFIDType switch {
                   RFIDTypes.MifareClassic  => "mifareCls",
                   RFIDTypes.MifareDESFire  => "mifareDes",
                   RFIDTypes.Calypso        => "calypso",
                   RFIDTypes.NFC            => "nfc",
                   RFIDTypes.MifareFamily   => "mifareFamily",
                   _                        => "unknown",
               };

        #endregion

    }

    /// <summary>
    /// RFID types.
    /// </summary>
    public enum RFIDTypes
    {

        /// <summary>
        /// MiFare classic.
        /// </summary>
        MifareClassic,

        /// <summary>
        /// MiFare DESFire.
        /// </summary>
        MifareDESFire,

        /// <summary>
        /// Calypso.
        /// </summary>
        Calypso,

        /// <summary>
        /// NFC.
        /// </summary>
        NFC,

        /// <summary>
        /// MiFare family.
        /// </summary>
        MifareFamily

    }

}
