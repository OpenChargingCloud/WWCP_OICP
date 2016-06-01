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

using System;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    [Flags]
    public enum AuthenticationModes
    {

        Unkown              =  0,
        NFC_RFID_Classic    =  1,
        NFC_RFID_DESFire    =  2,
        PnC                 =  4,   // ISO/IEC 15118 PLC
        REMOTE              =  8,   // App, QR-Code, Phone
        DirectPayment       = 16    // Remote use via direct payment. E.g. intercharge direct

    }

}