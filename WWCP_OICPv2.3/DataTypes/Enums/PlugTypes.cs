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
    /// Extentions methods for charging plug types.
    /// </summary>
    public static class PlugTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a plug type.
        /// </summary>
        /// <param name="Text">A text-representation of a plug type.</param>
        public static PlugTypes Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "Small Paddle Inductive"            : return PlugTypes.SmallPaddleInductive;
                case "Large Paddle Inductive"            : return PlugTypes.LargePaddleInductive;
                case "AVCON Connector"                   : return PlugTypes.AVCONConnector;
                case "Tesla Connector"                   : return PlugTypes.TeslaConnector;
                case "NEMA 5-20"                         : return PlugTypes.NEMA5_20;
                case "Type E French Standard"            : return PlugTypes.TypeEFrenchStandard;
                case "Type F Schuko"                     : return PlugTypes.TypeFSchuko;
                case "Type G British Standard"           : return PlugTypes.TypeGBritishStandard;
                case "Type J Swiss Standard"             : return PlugTypes.TypeJSwissStandard;
                case "Type 1 Connector (Cable Attached)" : return PlugTypes.Type1Connector_CableAttached;
                case "Type 2 Outlet"                     : return PlugTypes.Type2Outlet;
                case "Type 2 Connector (Cable Attached)" : return PlugTypes.Type2Connector_CableAttached;
                case "Type 3 Outlet"                     : return PlugTypes.Type3Outlet;
                case "IEC 60309 Single Phase"            : return PlugTypes.IEC60309SinglePhase;
                case "IEC 60309 Three Phase"             : return PlugTypes.IEC60309ThreePhase;
                case "CCS Combo 2 Plug (Cable Attached)" : return PlugTypes.CCSCombo2Plug_CableAttached;
                case "CCS Combo 1 Plug (Cable Attached)" : return PlugTypes.CCSCombo1Plug_CableAttached;
                case "CHAdeMO"                           : return PlugTypes.CHAdeMO;
                default                                  : return PlugTypes.Unspecified;
            };
        }

        #endregion

        #region AsString(PlugType)

        /// <summary>
        /// Return a text-representation of the given plug type.
        /// </summary>
        /// <param name="PlugType">A plug type.</param>
        public static String AsString(this PlugTypes PlugType)
        {
            switch (PlugType)
            {
                case PlugTypes.SmallPaddleInductive         : return "Small Paddle Inductive";
                case PlugTypes.LargePaddleInductive         : return "Large Paddle Inductive";
                case PlugTypes.AVCONConnector               : return "AVCON Connector";
                case PlugTypes.TeslaConnector               : return "Tesla Connector";
                case PlugTypes.NEMA5_20                     : return "NEMA 5-20";
                case PlugTypes.TypeEFrenchStandard          : return "Type E French Standard";
                case PlugTypes.TypeFSchuko                  : return "Type F Schuko";
                case PlugTypes.TypeGBritishStandard         : return "Type G British Standard";
                case PlugTypes.TypeJSwissStandard           : return "Type J Swiss Standard";
                case PlugTypes.Type1Connector_CableAttached : return "Type 1 Connector (Cable Attached)";
                case PlugTypes.Type2Outlet                  : return "Type 2 Outlet";
                case PlugTypes.Type2Connector_CableAttached : return "Type 2 Connector (Cable Attached)";
                case PlugTypes.Type3Outlet                  : return "Type 3 Outlet";
                case PlugTypes.IEC60309SinglePhase          : return "IEC 60309 Single Phase";
                case PlugTypes.IEC60309ThreePhase           : return "IEC 60309 Three Phase";
                case PlugTypes.CCSCombo2Plug_CableAttached  : return "CCS Combo 2 Plug (Cable Attached)";
                case PlugTypes.CCSCombo1Plug_CableAttached  : return "CCS Combo 1 Plug (Cable Attached)";
                case PlugTypes.CHAdeMO                      : return "CHAdeMO";
                default                                     : return "Unspecified";
            };
        }

        #endregion

    }


    /// <summary>
    /// Charging plug types.
    /// </summary>
    public enum PlugTypes
    {

        /// <summary>
        /// Unknown plug type
        /// </summary>
        Unspecified,

        /// <summary>
        /// Small paddle inductive
        /// </summary>
        SmallPaddleInductive,

        /// <summary>
        /// Large paddle inductive
        /// </summary>
        LargePaddleInductive,

        /// <summary>
        /// AVCON connector
        /// </summary>
        AVCONConnector,

        /// <summary>
        /// Tesla connector
        /// </summary>
        TeslaConnector,

        /// <summary>
        /// NEMA5 20
        /// </summary>
        NEMA5_20,

        /// <summary>
        /// IEC Type E (French Standard)
        /// </summary>
        TypeEFrenchStandard,

        /// <summary>
        /// IEC Type F (Schuko)
        /// </summary>
        TypeFSchuko,

        /// <summary>
        /// IEC Type G (British Standard)
        /// </summary>
        TypeGBritishStandard,

        /// <summary>
        /// IEC Type J (Swiss Standard)
        /// </summary>
        TypeJSwissStandard,

        /// <summary>
        /// Type 1 Connector with a cable attached
        /// </summary>
        Type1Connector_CableAttached,

        /// <summary>
        /// Type 2 Outlet
        /// </summary>
        Type2Outlet,

        /// <summary>
        /// Type 2 Connector with a cable attached
        /// </summary>
        Type2Connector_CableAttached,

        /// <summary>
        /// Type 3 Outlet
        /// </summary>
        Type3Outlet,

        /// <summary>
        /// IEC 60309 Single Phase
        /// </summary>
        IEC60309SinglePhase,

        /// <summary>
        /// IEC 60309 Three Phases
        /// </summary>
        IEC60309ThreePhase,

        /// <summary>
        /// CCS Combo 2 Plug with a cable attached
        /// </summary>
        CCSCombo2Plug_CableAttached,

        /// <summary>
        /// CCS Combo 1 Plug with a cable attached
        /// </summary>
        CCSCombo1Plug_CableAttached,

        /// <summary>
        /// CHAdeMO
        /// </summary>
        CHAdeMO

    }

}
