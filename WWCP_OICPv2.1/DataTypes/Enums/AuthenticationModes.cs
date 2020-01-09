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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The supported ways of ev driver authentication.
    /// </summary>
    [Flags]
    public enum AuthenticationModes
    {

        /// <summary>
        /// Unknown authentication mode.
        /// </summary>
        Unknown             =  0,

        /// <summary>
        /// Using MiFare classic RFID cards.
        /// </summary>
        NFC_RFID_Classic    =  1,

        /// <summary>
        /// Using MiFare DESFire RFID cards.
        /// </summary>
        NFC_RFID_DESFire    =  2,

        /// <summary>
        /// Using ISO/IEC 15118 Plug-and-Charge (PnC)
        /// </summary>
        PnC                 =  4,

        /// <summary>
        /// Using an eMAId/EVCOId via an app, QR-code or phone.
        /// </summary>
        REMOTE              =  8,

        /// <summary>
        /// Direct payment, e.g. via InterCharge Direct.
        /// </summary>
        DirectPayment       = 16

    }

}