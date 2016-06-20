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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The type of plugs.
    /// </summary>
    [Flags]
    public enum PlugTypes
    {

        Unspecified                     = 0,
        SmallPaddleInductive            = 1,
        LargePaddleInductive            = 1 <<  1,
        AVCONConnector                  = 1 <<  2,
        TeslaConnector                  = 1 <<  3,
        NEMA5_20                        = 1 <<  4,
        TypeEFrenchStandard             = 1 <<  5,
        TypeFSchuko                     = 1 <<  6,
        TypeGBritishStandard            = 1 <<  7,
        TypeJSwissStandard              = 1 <<  8,
        Type1Connector_CableAttached    = 1 <<  9,
        Type2Outlet                     = 1 << 10,
        Type2Connector_CableAttached    = 1 << 11,
        Type3Outlet                     = 1 << 12,
        IEC60309SinglePhase             = 1 << 13,
        IEC60309ThreePhase              = 1 << 14,
        CCSCombo2Plug_CableAttached     = 1 << 15,
        CCSCombo1Plug_CableAttached     = 1 << 16,
        CHAdeMO                         = 1 << 17

    }

}
