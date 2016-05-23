/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    [Flags]
    public enum ValueAddedServices
    {

        /// <summary>
        /// There are no value added services available.
        /// </summary>
        None                        =  0,

        /// <summary>
        /// Can an EV driver reserve the charging spot
        /// via remote services?
        /// </summary>
        Reservation                 =  1,

        /// <summary>
        /// Does the EVSE support dynamic pricing? 
        /// </summary>
        DynamicPricing              =  2,

        /// <summary>
        /// Is for this EVSE a dynamic status of the corresponding
        /// parking lot in front of the EVSE available?
        /// </summary>
        ParkingSensors              =  4,

        /// <summary>
        /// Does the EVSE offer a dynamic maximum power charging?
        /// </summary>
        MaximumPowerCharging        =  8,

        /// <summary>
        /// Is for the EVSE a predictive usage statistic available?
        /// </summary>
        PredictiveChargePointUsage  = 16,

        /// <summary>
        /// Does the EVSE offer charging plans, e.g. ISO 15118-2?
        /// </summary>
        ChargingPlans               = 32

    }

}