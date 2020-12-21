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
    /// Extentions methods for value added services.
    /// </summary>
    public static class ValueAddedServicesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a value added service.
        /// </summary>
        /// <param name="Text">A text-representation of a value added service.</param>
        public static ValueAddedServices Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "Reservation"                : return ValueAddedServices.Reservation;
                case "DynamicPricing"             : return ValueAddedServices.DynamicPricing;
                case "ParkingSensors"             : return ValueAddedServices.ParkingSensors;
                case "MaximumPowerCharging"       : return ValueAddedServices.MaximumPowerCharging;
                case "PredictiveChargePointUsage" : return ValueAddedServices.PredictiveChargePointUsage;
                case "ChargingPlans"              : return ValueAddedServices.ChargingPlans;
                case "RoofProvided"               : return ValueAddedServices.RoofProvided;
                default                           : return ValueAddedServices.None;
            }
        }

        #endregion

        #region AsString(ValueAddedService)

        /// <summary>
        /// Return a text-representation of the given value added service.
        /// </summary>
        /// <param name="ValueAddedService">A value added service.</param>
        public static String AsString(this ValueAddedServices ValueAddedService)
        {
            switch (ValueAddedService)
            {
                case ValueAddedServices.Reservation                : return "Reservation";
                case ValueAddedServices.DynamicPricing             : return "DynamicPricing";
                case ValueAddedServices.ParkingSensors             : return "ParkingSensors";
                case ValueAddedServices.MaximumPowerCharging       : return "MaximumPowerCharging";
                case ValueAddedServices.PredictiveChargePointUsage : return "PredictiveChargePointUsage";
                case ValueAddedServices.ChargingPlans              : return "ChargingPlans";
                case ValueAddedServices.RoofProvided               : return "RoofProvided";
                default                                            : return "None";
            }
        }

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
