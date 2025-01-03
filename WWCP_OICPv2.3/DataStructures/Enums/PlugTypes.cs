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
    /// Extensions methods for charging plug types.
    /// </summary>
    public static class PlugTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a plug type.
        /// </summary>
        /// <param name="Text">A text representation of a plug type.</param>
        public static PlugTypes Parse(String Text)
        {

            if (TryParse(Text, out var plugType))
                return plugType;

            throw new ArgumentException("Invalid plug type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a plug type.
        /// </summary>
        /// <param name="Text">A text representation of a plug type.</param>
        public static PlugTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var plugType))
                return plugType;

            return default;

        }

        #endregion

        #region TryParse(Text, out PlugType)

        /// <summary>
        /// Parses the given text representation of a plug type.
        /// </summary>
        /// <param name="Text">A text representation of a plug type.</param>
        /// <param name="PlugType">The parsed plug type.</param>
        public static Boolean TryParse(String Text, out PlugTypes PlugType)
        {
            switch (Text?.Trim())
            {

                case "Small Paddle Inductive":
                    PlugType = PlugTypes.SmallPaddleInductive;
                    return true;

                case "Large Paddle Inductive":
                    PlugType = PlugTypes.LargePaddleInductive;
                    return true;

                case "AVCON Connector":
                    PlugType = PlugTypes.AVCONConnector;
                    return true;

                case "Tesla Connector":
                    PlugType = PlugTypes.TeslaConnector;
                    return true;

                case "NEMA 5-20":
                    PlugType = PlugTypes.NEMA5_20;
                    return true;

                case "Type E French Standard":
                    PlugType = PlugTypes.TypeEFrenchStandard;
                    return true;

                case "Type F Schuko":
                    PlugType = PlugTypes.TypeFSchuko;
                    return true;

                case "Type G British Standard":
                    PlugType = PlugTypes.TypeGBritishStandard;
                    return true;

                case "Type J Swiss Standard":
                    PlugType = PlugTypes.TypeJSwissStandard;
                    return true;

                case "Type 1 Connector (Cable Attached)":
                    PlugType = PlugTypes.Type1Connector_CableAttached;
                    return true;

                case "Type 2 Outlet":
                    PlugType = PlugTypes.Type2Outlet;
                    return true;

                case "Type 2 Connector (Cable Attached)":
                    PlugType = PlugTypes.Type2Connector_CableAttached;
                    return true;

                case "Type 3 Outlet":
                    PlugType = PlugTypes.Type3Outlet;
                    return true;

                case "IEC 60309 Single Phase":
                    PlugType = PlugTypes.IEC60309SinglePhase;
                    return true;

                case "IEC 60309 Three Phase":
                    PlugType = PlugTypes.IEC60309ThreePhase;
                    return true;

                case "CCS Combo 2 Plug (Cable Attached)":
                    PlugType = PlugTypes.CCSCombo2Plug_CableAttached;
                    return true;

                case "CCS Combo 1 Plug (Cable Attached)":
                    PlugType = PlugTypes.CCSCombo1Plug_CableAttached;
                    return true;

                case "CHAdeMO":
                    PlugType = PlugTypes.CHAdeMO;
                    return true;

                default:
                    PlugType = PlugTypes.TypeFSchuko;
                    return false;

            };
        }

        #endregion

        #region AsString(PlugType)

        /// <summary>
        /// Return a text representation of the given plug type.
        /// </summary>
        /// <param name="PlugType">A plug type.</param>
        public static String AsString(this PlugTypes PlugType)

            => PlugType switch {
                   PlugTypes.SmallPaddleInductive          => "Small Paddle Inductive",
                   PlugTypes.LargePaddleInductive          => "Large Paddle Inductive",
                   PlugTypes.AVCONConnector                => "AVCON Connector",
                   PlugTypes.TeslaConnector                => "Tesla Connector",
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
                   _                                       => throw new ArgumentException("Invalid plug type!")
               };

        #endregion

    }


    /// <summary>
    /// Charging plug types.
    /// </summary>
    public enum PlugTypes
    {

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
