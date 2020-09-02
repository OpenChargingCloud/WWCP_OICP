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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// Extentions methods for payment options.
    /// </summary>
    public static class PaymentOptionsExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a paymet option.
        /// </summary>
        /// <param name="Text">A text-representation of a payment option.</param>
        public static PaymentOptions Parse(String Text)

            => Text.Trim() switch {
                "No Payment"  => PaymentOptions.NoPayment,
                "Direct"      => PaymentOptions.Direct,
                "Contract"    => PaymentOptions.Contract,
                _             => PaymentOptions.Unspecified,
            };

        #endregion

        #region AsString(PaymentOption)

        /// <summary>
        /// Return a text-representation of the given paymet option.
        /// </summary>
        /// <param name="PaymentOption">A paymet option.</param>
        public static String AsString(this PaymentOptions PaymentOption)

            => PaymentOption switch {
                PaymentOptions.NoPayment  => "No Payment",
                PaymentOptions.Direct     => "Direct",
                PaymentOptions.Contract   => "Contract",
                _                         => "Unspecified",
            };

        #endregion

    }


    /// <summary>
    /// Payment options.
    /// </summary>
    [Flags]
    public enum PaymentOptions
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified,

        /// <summary>
        /// No Payment.
        /// </summary>
        NoPayment,

        /// <summary>
        /// Direct payment via e.g. cash, credit/debit card, SMS or phone call.
        /// </summary>
        Direct,

        /// <summary>
        /// Payment via contract.
        /// </summary>
        Contract

    }

}
