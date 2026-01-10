/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extensions methods for energy types.
    /// </summary>
    public static class EnergyTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an energy type.
        /// </summary>
        /// <param name="Text">A text representation of an energy type.</param>
        public static EnergyTypes Parse(String Text)
        {

            if (TryParse(Text, out var energyType))
                return energyType;

            throw new ArgumentException("Undefined energy type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an energy type.
        /// </summary>
        /// <param name="Text">A text representation of an energy type.</param>
        public static EnergyTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var energyType))
                return energyType;

            return default;

        }

        #endregion

        #region TryParse(Text, out EnergyType)

        /// <summary>
        /// Parses the given text representation of an energy type.
        /// </summary>
        /// <param name="Text">A text representation of an energy type.</param>
        /// <param name="EnergyType">The parsed energy type.</param>
        public static Boolean TryParse(String Text, out EnergyTypes EnergyType)
        {
            switch (Text?.Trim())
            {

                case "Solar":
                    EnergyType = EnergyTypes.Solar;
                    return true;

                case "Wind":
                    EnergyType = EnergyTypes.Wind;
                    return true;

                case "HydroPower":
                    EnergyType = EnergyTypes.HydroPower;
                    return true;

                case "GeothermalEnergy":
                    EnergyType = EnergyTypes.GeothermalEnergy;
                    return true;

                case "Biomass":
                    EnergyType = EnergyTypes.Biomass;
                    return true;

                case "Coal":
                    EnergyType = EnergyTypes.Coal;
                    return true;

                case "NuclearEnergy":
                    EnergyType = EnergyTypes.NuclearEnergy;
                    return true;

                case "Petroleum":
                    EnergyType = EnergyTypes.Petroleum;
                    return true;

                case "NaturalGas":
                    EnergyType = EnergyTypes.NaturalGas;
                    return true;

                default:
                    EnergyType = EnergyTypes.Solar;
                    return false;

            }
        }

        #endregion

        #region AsString(EnergyType)

        /// <summary>
        /// Return a text representation of the given energy type.
        /// </summary>
        /// <param name="EnergyType">An energy type.</param>
        public static String AsString(this EnergyTypes EnergyType)

            => EnergyType switch {
                   EnergyTypes.Solar             => "Solar",
                   EnergyTypes.Wind              => "Wind",
                   EnergyTypes.HydroPower        => "HydroPower",
                   EnergyTypes.GeothermalEnergy  => "GeothermalEnergy",
                   EnergyTypes.Biomass           => "Biomass",
                   EnergyTypes.Coal              => "Coal",
                   EnergyTypes.NuclearEnergy     => "NuclearEnergy",
                   EnergyTypes.Petroleum         => "Petroleum",
                   EnergyTypes.NaturalGas        => "NaturalGas",
                   _                             => "unknown",
               };

        #endregion

    }


    /// <summary>
    /// Energy types.
    /// </summary>
    public enum EnergyTypes
    {

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
