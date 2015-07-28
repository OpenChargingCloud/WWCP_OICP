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

        public static PlugTypes AsOICPPlugType(String Text)
        {

            switch (Text)
            {

                case "Small Paddle Inductive":
                    return PlugTypes.SmallPaddleInductive;

                case "Large Paddle Inductive":
                    return PlugTypes.LargePaddleInductive;

                case "AVCONConnector":
                    return PlugTypes.AVCONConnector;

                case "TeslaConnector":
                    return PlugTypes.TeslaConnector;

                case "NEMA 5-20":
                    return PlugTypes.NEMA5_20;

                case "Type E French Standard":
                    return PlugTypes.TypeEFrenchStandard;

                case "Type F Schuko":
                    return PlugTypes.TypeFSchuko;

                case "Type G British Standard":
                    return PlugTypes.TypeGBritishStandard;

                case "Type J Swiss Standard":
                    return PlugTypes.TypeJSwissStandard;

                case "Type 1 Connector (Cable Attached)":
                    return PlugTypes.Type1Connector_CableAttached;

                case "Type 2 Outlet":
                    return PlugTypes.Type2Outlet;

                case "Type 2 Connector (Cable Attached)":
                    return PlugTypes.Type2Connector_CableAttached;

                case "Type 3 Outlet":
                    return PlugTypes.Type3Outlet;

                case "IEC 60309 Single Phase":
                    return PlugTypes.IEC60309SinglePhase;

                case "IEC 60309 Three Phase":
                    return PlugTypes.IEC60309ThreePhase;

                case "CCS Combo 2 Plug (Cable Attached)":
                    return PlugTypes.CCSCombo2Plug_CableAttached;

                case "CCS Combo 1 Plug (Cable Attached)":
                    return PlugTypes.CCSCombo1Plug_CableAttached;

                case "CHAdeMO DC CHAdeMO Connector":
                    return PlugTypes.CHAdeMO_DC_CHAdeMOConnector;


                default:
                    return PlugTypes.Unspecified;

            }

        }

        public static String AsString(this PlugTypes PlugType)
        {

            switch (PlugType)
            {

                case PlugTypes.SmallPaddleInductive:
                    return "Small Paddle Inductive";

                case PlugTypes.LargePaddleInductive:
                    return "Large Paddle Inductive";

                case PlugTypes.AVCONConnector:
                    return "AVCONConnector";

                case PlugTypes.TeslaConnector:
                    return "TeslaConnector";

                case PlugTypes.NEMA5_20:
                    return "NEMA 5-20";

                case PlugTypes.TypeEFrenchStandard:
                    return "Type E French Standard";

                case PlugTypes.TypeFSchuko:
                    return "Type F Schuko";

                case PlugTypes.TypeGBritishStandard:
                    return "Type G British Standard";

                case PlugTypes.TypeJSwissStandard:
                    return "Type J Swiss Standard";

                case PlugTypes.Type1Connector_CableAttached:
                    return "Type 1 Connector (Cable Attached)";

                case PlugTypes.Type2Outlet:
                    return "Type 2 Outlet";

                case PlugTypes.Type2Connector_CableAttached:
                    return "Type 2 Connector (Cable Attached)";

                case PlugTypes.Type3Outlet:
                    return "Type 3 Outlet";

                case PlugTypes.IEC60309SinglePhase:
                    return "IEC 60309 Single Phase";

                case PlugTypes.IEC60309ThreePhase:
                    return "IEC 60309 Three Phase";

                case PlugTypes.CCSCombo2Plug_CableAttached:
                    return "CCS Combo 2 Plug (Cable Attached)";

                case PlugTypes.CCSCombo1Plug_CableAttached:
                    return "CCS Combo 1 Plug (Cable Attached)";

                case PlugTypes.CHAdeMO_DC_CHAdeMOConnector:
                    return "CHAdeMO DC CHAdeMO Connector";


                default:
                    return "Unspecified";

            }

        }


    }

}
