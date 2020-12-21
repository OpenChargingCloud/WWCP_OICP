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
    /// Extentions methods for energy types.
    /// </summary>
    public static class EnergyTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of an energy type.
        /// </summary>
        /// <param name="Text">A text-representation of an energy type.</param>
        public static EnergyTypes Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "Solar"                : return EnergyTypes.Solar;
                case "Wind"                 : return EnergyTypes.Wind;
                case "Hydro Power"          : return EnergyTypes.HydroPower;
                case "Geothermal Energy"    : return EnergyTypes.GeothermalEnergy;
                case "Biomass"              : return EnergyTypes.Biomass;
                case "Coal"                 : return EnergyTypes.Coal;
                case "Nuclear Energy"       : return EnergyTypes.NuclearEnergy;
                case "Petroleum"            : return EnergyTypes.Petroleum;
                case "Natural Gas"          : return EnergyTypes.NaturalGas;
                default                     : return EnergyTypes.Unspecified;
            };
        }

        #endregion

        #region AsString(EnergyType)

        /// <summary>
        /// Return a text-representation of the given energy type.
        /// </summary>
        /// <param name="EnergyType">An energy type.</param>
        public static String AsString(this EnergyTypes EnergyType)
        {
            switch (EnergyType)
            {
                case EnergyTypes.Solar            : return "Solar";
                case EnergyTypes.Wind             : return "Wind";
                case EnergyTypes.HydroPower       : return "Hydro Power";
                case EnergyTypes.GeothermalEnergy : return "Geothermal Energy";
                case EnergyTypes.Biomass          : return "Biomass";
                case EnergyTypes.Coal             : return "Coal";
                case EnergyTypes.NuclearEnergy    : return "Nuclear Energy";
                case EnergyTypes.Petroleum        : return "Petroleum";
                case EnergyTypes.NaturalGas       : return "Natural Gas";
                default                           : return "Unspecified";
            };
        }

        #endregion

    }


    /// <summary>
    /// Energy types.
    /// </summary>
    public enum EnergyTypes
    {

        /// <summary>
        /// Unknown energy type.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Energy coming from Solar radiation.
        /// </summary>
        Solar,

        /// <summary>
        /// Energy produced by wind.
        /// </summary>
        Wind,

        /// <summary>
        /// Energy produced by the movement of water.
        /// </summary>
        HydroPower,

        /// <summary>
        /// Energy coming from the sub-surface of the earth.
        /// </summary>
        GeothermalEnergy,

        /// <summary>
        /// Energy produced using plant or animal material as fuel.
        /// </summary>
        Biomass,

        /// <summary>
        /// Energy produced using coal as fuel.
        /// </summary>
        Coal,

        /// <summary>
        /// Energy being produced by nuclear fission.
        /// </summary>
        NuclearEnergy,

        /// <summary>
        /// Energy produced by using Petroleum as fuel.
        /// </summary>
        Petroleum,

        /// <summary>
        /// Energy produced using Natural Gas as fuel.
        /// </summary>
        NaturalGas

    }

}
