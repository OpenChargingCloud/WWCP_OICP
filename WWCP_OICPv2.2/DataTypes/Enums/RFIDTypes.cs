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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Extentions methods for RFID types.
    /// </summary>
    public static class RFIDTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a RFID type.
        /// </summary>
        /// <param name="Text">A text-representation of a RFID type.</param>
        public static RFIDTypes Parse(String Text)

            => Text.Trim() switch {
                "mifareCls"     => RFIDTypes.mifareCls,
                "mifareDes"     => RFIDTypes.mifareDes,
                "calypso"       => RFIDTypes.calypso,
                "nfc"           => RFIDTypes.nfc,
                "mifareFamily"  => RFIDTypes.mifareFamily,
                _               => RFIDTypes.Unspecified,
            };

        #endregion

        #region AsString(RFIDType)

        /// <summary>
        /// Return a text-representation of the given RFID type.
        /// </summary>
        /// <param name="RFIDType">A RFID type.</param>
        public static String AsString(this RFIDTypes RFIDType)

            => RFIDType switch {
                RFIDTypes.mifareCls     => "mifareCls",
                RFIDTypes.mifareDes     => "mifareDes",
                RFIDTypes.calypso       => "calypso",
                RFIDTypes.nfc           => "nfc",
                RFIDTypes.mifareFamily  => "mifareFamily",
                _                       => "Unspecified",
            };

        #endregion

    }

    /// <summary>
    /// RFID types.
    /// </summary>
    public enum RFIDTypes
    {

        /// <summary>
        /// Unspecified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// MiFare classic.
        /// </summary>
        mifareCls,

        /// <summary>
        /// MiFare DESFire.
        /// </summary>
        mifareDes,

        /// <summary>
        /// Calypso.
        /// </summary>
        calypso,

        /// <summary>
        /// NFC.
        /// </summary>
        nfc,

        /// <summary>
        /// MiFare family.
        /// </summary>
        mifareFamily

    }

}
