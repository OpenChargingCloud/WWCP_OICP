/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

namespace org.GraphDefined.WWCP
{

    public static partial class OICPMapper
    {

        public static PlugType AsOICPPlugType(String Text)
        {

            switch (Text)
            {

                case "Small Paddle Inductive":
                    return PlugType.SmallPaddleInductive;

                case "Large Paddle Inductive":
                    return PlugType.LargePaddleInductive;

                case "AVCONConnector":
                    return PlugType.AVCONConnector;

                case "TeslaConnector":
                    return PlugType.TeslaConnector;

                case "NEMA 5-20":
                    return PlugType.NEMA5_20;

                case "Type E French Standard":
                    return PlugType.TypeEFrenchStandard;

                case "Type F Schuko":
                    return PlugType.TypeFSchuko;

                case "Type G British Standard":
                    return PlugType.TypeGBritishStandard;

                case "Type J Swiss Standard":
                    return PlugType.TypeJSwissStandard;

                case "Type 1 Connector (Cable Attached)":
                    return PlugType.Type1Connector_CableAttached;

                case "Type 2 Outlet":
                    return PlugType.Type2Outlet;

                case "Type 2 Connector (Cable Attached)":
                    return PlugType.Type2Connector_CableAttached;

                case "Type 3 Outlet":
                    return PlugType.Type3Outlet;

                case "IEC 60309 Single Phase":
                    return PlugType.IEC60309SinglePhase;

                case "IEC 60309 Three Phase":
                    return PlugType.IEC60309ThreePhase;

                case "CCS Combo 2 Plug (Cable Attached)":
                    return PlugType.CCSCombo2Plug_CableAttached;

                case "CCS Combo 1 Plug (Cable Attached)":
                    return PlugType.CCSCombo1Plug_CableAttached;

                case "CHAdeMO DC CHAdeMO Connector":
                    return PlugType.CHAdeMO_DC_CHAdeMOConnector;


                default:
                    return PlugType.Unspecified;

            }

        }

        public static String AsString(this PlugType PlugType)
        {

            switch (PlugType)
            {

                case PlugType.SmallPaddleInductive:
                    return "Small Paddle Inductive";

                case PlugType.LargePaddleInductive:
                    return "Large Paddle Inductive";

                case PlugType.AVCONConnector:
                    return "AVCONConnector";

                case PlugType.TeslaConnector:
                    return "TeslaConnector";

                case PlugType.NEMA5_20:
                    return "NEMA 5-20";

                case PlugType.TypeEFrenchStandard:
                    return "Type E French Standard";

                case PlugType.TypeFSchuko:
                    return "Type F Schuko";

                case PlugType.TypeGBritishStandard:
                    return "Type G British Standard";

                case PlugType.TypeJSwissStandard:
                    return "Type J Swiss Standard";

                case PlugType.Type1Connector_CableAttached:
                    return "Type 1 Connector (Cable Attached)";

                case PlugType.Type2Outlet:
                    return "Type 2 Outlet";

                case PlugType.Type2Connector_CableAttached:
                    return "Type 2 Connector (Cable Attached)";

                case PlugType.Type3Outlet:
                    return "Type 3 Outlet";

                case PlugType.IEC60309SinglePhase:
                    return "IEC 60309 Single Phase";

                case PlugType.IEC60309ThreePhase:
                    return "IEC 60309 Three Phase";

                case PlugType.CCSCombo2Plug_CableAttached:
                    return "CCS Combo 2 Plug (Cable Attached)";

                case PlugType.CCSCombo1Plug_CableAttached:
                    return "CCS Combo 1 Plug (Cable Attached)";

                case PlugType.CHAdeMO_DC_CHAdeMOConnector:
                    return "CHAdeMO DC CHAdeMO Connector";


                default:
                    return "Unspecified";

            }

        }


    }

}
