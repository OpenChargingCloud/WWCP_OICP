/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// The type of plugs.
    /// </summary>
    [Flags]
    public enum PlugTypes
    {

        /// <summary>
        /// Unknown plug type
        /// </summary>
        Unspecified                     = 0,

        /// <summary>
        /// Small paddle inductive
        /// </summary>
        SmallPaddleInductive            = 1,

        /// <summary>
        /// Large paddle inductive
        /// </summary>
        LargePaddleInductive            = 1 <<  1,

        /// <summary>
        /// AVCON connector
        /// </summary>
        AVCONConnector                  = 1 <<  2,

        /// <summary>
        /// Tesla connector
        /// </summary>
        TeslaConnector                  = 1 <<  3,

        /// <summary>
        /// NEMA5 20
        /// </summary>
        NEMA5_20                        = 1 <<  4,

        /// <summary>
        /// IEC Type E (French Standard)
        /// </summary>
        TypeEFrenchStandard             = 1 <<  5,

        /// <summary>
        /// IEC Type F (Schuko)
        /// </summary>
        TypeFSchuko                     = 1 <<  6,

        /// <summary>
        /// IEC Type G (British Standard)
        /// </summary>
        TypeGBritishStandard            = 1 <<  7,

        /// <summary>
        /// IEC Type J (Swiss Standard)
        /// </summary>
        TypeJSwissStandard              = 1 <<  8,

        /// <summary>
        /// Type 1 Connector with a cable attached
        /// </summary>
        Type1Connector_CableAttached    = 1 <<  9,

        /// <summary>
        /// Type 2 Outlet
        /// </summary>
        Type2Outlet                     = 1 << 10,

        /// <summary>
        /// Type 2 Connector with a cable attached
        /// </summary>
        Type2Connector_CableAttached    = 1 << 11,

        /// <summary>
        /// Type 3 Outlet
        /// </summary>
        Type3Outlet                     = 1 << 12,

        /// <summary>
        /// IEC 60309 Single Phase
        /// </summary>
        IEC60309SinglePhase             = 1 << 13,

        /// <summary>
        /// IEC 60309 Three Phases
        /// </summary>
        IEC60309ThreePhase              = 1 << 14,

        /// <summary>
        /// CCS Combo 2 Plug with a cable attached
        /// </summary>
        CCSCombo2Plug_CableAttached     = 1 << 15,

        /// <summary>
        /// CCS Combo 1 Plug with a cable attached
        /// </summary>
        CCSCombo1Plug_CableAttached     = 1 << 16,

        /// <summary>
        /// CHAdeMO
        /// </summary>
        CHAdeMO                         = 1 << 17

    }

}
