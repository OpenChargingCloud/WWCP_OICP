﻿/*
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
    /// Extentions methods for plug types.
    /// </summary>
    public static class PlugTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a plug type.
        /// </summary>
        /// <param name="Text">A text-representation of a plug type.</param>
        public static PlugTypes Parse(String Text)

            => Text?.Trim() switch {
                "Small Paddle Inductive"             => PlugTypes.SmallPaddleInductive,
                "Large Paddle Inductive"             => PlugTypes.LargePaddleInductive,
                "AVCONConnector"                     => PlugTypes.AVCONConnector,
                "TeslaConnector"                     => PlugTypes.TeslaConnector,
                "NEMA 5-20"                          => PlugTypes.NEMA5_20,
                "Type E French Standard"             => PlugTypes.TypeEFrenchStandard,
                "Type F Schuko"                      => PlugTypes.TypeFSchuko,
                "Type G British Standard"            => PlugTypes.TypeGBritishStandard,
                "Type J Swiss Standard"              => PlugTypes.TypeJSwissStandard,
                "Type 1 Connector (Cable Attached)"  => PlugTypes.Type1Connector_CableAttached,
                "Type 2 Outlet"                      => PlugTypes.Type2Outlet,
                "Type 2 Connector (Cable Attached)"  => PlugTypes.Type2Connector_CableAttached,
                "Type 3 Outlet"                      => PlugTypes.Type3Outlet,
                "IEC 60309 Single Phase"             => PlugTypes.IEC60309SinglePhase,
                "IEC 60309 Three Phase"              => PlugTypes.IEC60309ThreePhase,
                "CCS Combo 2 Plug (Cable Attached)"  => PlugTypes.CCSCombo2Plug_CableAttached,
                "CCS Combo 1 Plug (Cable Attached)"  => PlugTypes.CCSCombo1Plug_CableAttached,
                "CHAdeMO"                            => PlugTypes.CHAdeMO,
                _                                    => PlugTypes.Unspecified,
            };

        #endregion

        #region AsString(PlugType)

        /// <summary>
        /// Return a text-representation of the given plug type.
        /// </summary>
        /// <param name="PlugType">A plug type.</param>
        public static String AsString(this PlugTypes PlugType)

            => PlugType switch {
                PlugTypes.SmallPaddleInductive          => "Small Paddle Inductive",
                PlugTypes.LargePaddleInductive          => "Large Paddle Inductive",
                PlugTypes.AVCONConnector                => "AVCONConnector",
                PlugTypes.TeslaConnector                => "TeslaConnector",
                PlugTypes.NEMA5_20                      => "NEMA 5-20",
                PlugTypes.TypeEFrenchStandard           => "Type E French Standard",
                PlugTypes.TypeFSchuko                   => "Type F Schuko",
                PlugTypes.TypeGBritishStandard          => "Type G British Standard",
                PlugTypes.TypeJSwissStandard            => "Type J Swiss Standard",
                PlugTypes.Type1Connector_CableAttached  => "Type 1 Connector (Cable Attached)",
                PlugTypes.Type2Outlet                   => "Type 2 Outlet",
                PlugTypes.Type2Connector_CableAttached  => "Type 2 Connector (Cable Attached)",
                PlugTypes.Type3Outlet                   => "Type 3 Outlet",
                PlugTypes.IEC60309SinglePhase           => "IEC 60309 Single Phase",
                PlugTypes.IEC60309ThreePhase            => "IEC 60309 Three Phase",
                PlugTypes.CCSCombo2Plug_CableAttached   => "CCS Combo 2 Plug (Cable Attached)",
                PlugTypes.CCSCombo1Plug_CableAttached   => "CCS Combo 1 Plug (Cable Attached)",
                PlugTypes.CHAdeMO                       => "CHAdeMO",
                _                                       => "Unspecified",
            };

        #endregion

    }


    /// <summary>
    /// The plug type.
    /// </summary>
    [Flags]
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
