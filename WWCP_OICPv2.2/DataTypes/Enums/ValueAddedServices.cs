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
    /// OICP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        #region AsValueAddedService(ValueAddedService)

        /// <summary>
        /// Parses the OICP ValueAddedService.
        /// </summary>
        /// <param name="ValueAddedService">A value added service.</param>
        public static ValueAddedServices AsValueAddedService(String ValueAddedService)
        {

            switch (ValueAddedService.Trim())
            {

                case "Reservation":
                    return ValueAddedServices.Reservation;

                case "DynamicPricing":
                    return ValueAddedServices.DynamicPricing;

                case "ParkingSensors":
                    return ValueAddedServices.ParkingSensors;

                case "MaximumPowerCharging":
                    return ValueAddedServices.MaximumPowerCharging;

                case "PredictiveChargePointUsage":
                    return ValueAddedServices.PredictiveChargePointUsage;

                case "ChargingPlans":
                    return ValueAddedServices.ChargingPlans;

                default:
                    return ValueAddedServices.None;

            }

        }

        #endregion

        #region AsString(ValueAddedService)

        public static String AsString(this ValueAddedServices ValueAddedService)
        {

            switch (ValueAddedService)
            {

                case ValueAddedServices.Reservation:
                    return "Reservation";

                case ValueAddedServices.DynamicPricing:
                    return "DynamicPricing";

                case ValueAddedServices.ParkingSensors:
                    return "ParkingSensors";

                case ValueAddedServices.MaximumPowerCharging:
                    return "MaximumPowerCharging";

                case ValueAddedServices.PredictiveChargePointUsage:
                    return "PredictiveChargePointUsage";

                case ValueAddedServices.ChargingPlans:
                    return "ChargingPlans";

                default:
                    return "None";

            }

        }

        #endregion

    }


    /// <summary>
    /// Value added services at charging stations and/or EVSEs.
    /// </summary>
    [Flags]
    public enum ValueAddedServices
    {

        /// <summary>
        /// There are no value added services available.
        /// </summary>
        None                        = 0,

        /// <summary>
        /// Can an EV driver reserve the charging spot via remote services?
        /// </summary>
        Reservation                 = 1,

        /// <summary>
        /// Does the EVSE support dynamic pricing? 
        /// </summary>
        DynamicPricing              = 1 << 1,

        /// <summary>
        /// Is for this EVSE a dynamic status of the corresponding
        /// parking lot in front of the EVSE available?
        /// </summary>
        ParkingSensors              = 1 << 2,

        /// <summary>
        /// Does the EVSE offer a dynamic maximum power charging?
        /// </summary>
        MaximumPowerCharging        = 1 << 3,

        /// <summary>
        /// Is for the EVSE a predictive usage statistic available?
        /// </summary>
        PredictiveChargePointUsage  = 1 << 4,

        /// <summary>
        /// Does the EVSE offer charging plans, e.g. ISO 15118-2?
        /// </summary>
        ChargingPlans               = 1 << 5

    }

}