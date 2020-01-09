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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OICP payment options.
    /// </summary>
    [Flags]
    public enum PaymentOptions
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified   = 0,

        /// <summary>
        /// Free charging.
        /// </summary>
        Free          = 1,

        /// <summary>
        /// Direct payment via e.g. cash, credit/debit card, SMS or phone call.
        /// </summary>
        Direct        = 1 << 1,

        /// <summary>
        /// Direct payment via SMS.
        /// </summary>
        SMS           = 1 << 2,

        /// <summary>
        /// Direct payment via cash.
        /// </summary>
        Cash          = 1 << 3,

        /// <summary>
        /// Direct payment credit card.
        /// </summary>
        CreditCard    = 1 << 4,

        /// <summary>
        /// Payment via contract.
        /// </summary>
        Contract      = 1 << 5

    }

}
