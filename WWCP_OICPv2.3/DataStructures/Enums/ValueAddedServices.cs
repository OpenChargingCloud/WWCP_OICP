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
    /// Extensions methods for value added services.
    /// </summary>
    public static class ValueAddedServicesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a value added service.
        /// </summary>
        /// <param name="Text">A text representation of a value added service.</param>
        public static ValueAddedServices Parse(String Text)
        {

            if (TryParse(Text, out var valueAddedService))
                return valueAddedService;

            throw new ArgumentException("Undefined value added service '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a value added service.
        /// </summary>
        /// <param name="Text">A text representation of a value added service.</param>
        public static ValueAddedServices? TryParse(String Text)
        {

            if (TryParse(Text, out var valueAddedService))
                return valueAddedService;

            return default;

        }

        #endregion

        #region TryParse(Text, out ValueAddedService)

        /// <summary>
        /// Parses the given text representation of a value added service.
        /// </summary>
        /// <param name="Text">A text representation of a value added service.</param>
        /// <param name="ValueAddedService">The parsed value added service.</param>
        public static Boolean TryParse(String Text, out ValueAddedServices ValueAddedService)
        {
            switch (Text?.Trim())
            {

                case "Reservation":
                    ValueAddedService = ValueAddedServices.Reservation;
                    return true;

                case "DynamicPricing":
                    ValueAddedService = ValueAddedServices.DynamicPricing;
                    return true;

                case "ParkingSensors":
                    ValueAddedService = ValueAddedServices.ParkingSensors;
                    return true;

                case "MaximumPowerCharging":
                    ValueAddedService = ValueAddedServices.MaximumPowerCharging;
                    return true;

                case "PredictiveChargePointUsage":
                    ValueAddedService = ValueAddedServices.PredictiveChargePointUsage;
                    return true;

                case "ChargingPlans":
                    ValueAddedService = ValueAddedServices.ChargingPlans;
                    return true;

                case "RoofProvided":
                    ValueAddedService = ValueAddedServices.RoofProvided;
                    return true;

                case "None":
                    ValueAddedService = ValueAddedServices.None;
                    return true;

                default:
                    ValueAddedService = ValueAddedServices.None;
                    return false;

            };
        }

        #endregion

        #region AsString(ValueAddedService)

        /// <summary>
        /// Return a text representation of the given value added service.
        /// </summary>
        /// <param name="ValueAddedService">A value added service.</param>
        public static String AsString(this ValueAddedServices ValueAddedService)

            => ValueAddedService switch {
                   ValueAddedServices.Reservation                 => "Reservation",
                   ValueAddedServices.DynamicPricing              => "DynamicPricing",
                   ValueAddedServices.ParkingSensors              => "ParkingSensors",
                   ValueAddedServices.MaximumPowerCharging        => "MaximumPowerCharging",
                   ValueAddedServices.PredictiveChargePointUsage  => "PredictiveChargePointUsage",
                   ValueAddedServices.ChargingPlans               => "ChargingPlans",
                   ValueAddedServices.RoofProvided                => "RoofProvided",
                   ValueAddedServices.None                        => "None",
                   _                                              => "None"
               };

        #endregion

    }


    /// <summary>
    /// Value added services at charging stations and/or EVSEs.
    /// </summary>
    public enum ValueAddedServices
    {

        /// <summary>
        /// There are no value added services available.
        /// </summary>
        None,

        /// <summary>
        /// Can an EV driver reserve the charging spot via remote services?
        /// </summary>
        Reservation,

        /// <summary>
        /// Does the EVSE support dynamic pricing? 
        /// </summary>
        DynamicPricing,

        /// <summary>
        /// Is for this EVSE a dynamic status of the corresponding
        /// parking lot in front of the EVSE available?
        /// </summary>
        ParkingSensors,

        /// <summary>
        /// Does the EVSE offer a dynamic maximum power charging?
        /// </summary>
        MaximumPowerCharging,

        /// <summary>
        /// Is for the EVSE a predictive usage statistic available?
        /// </summary>
        PredictiveChargePointUsage,

        /// <summary>
        /// Does the EVSE-ID offer charging plans, e.g. As described in ISO15118-2?
        /// </summary>
        ChargingPlans,

        /// <summary>
        /// Indicates if the charging station is under a roof.
        /// </summary>
        RoofProvided

    }

}
