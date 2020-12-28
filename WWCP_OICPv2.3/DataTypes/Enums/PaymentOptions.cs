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
    /// Extentions methods for payment options.
    /// </summary>
    public static class PaymentOptionsExtentions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text-representation of a paymet option.
        /// </summary>
        /// <param name="Text">A text-representation of a payment option.</param>
        public static PaymentOptions Parse(String Text)

            => Text?.Trim() switch {
                   "No Payment"  => PaymentOptions.NoPayment,
                   "Direct"      => PaymentOptions.Direct,
                   "Contract"    => PaymentOptions.Contract,
                   _             => throw new ArgumentException("Undefined paymet option '" + Text + "'!")
            };

        #endregion

        #region TryParse(Text, out PaymentOption)

        /// <summary>
        /// Parses the given text-representation of a paymet option.
        /// </summary>
        /// <param name="Text">A text-representation of a payment option.</param>
        /// <param name="PaymentOption">The parsed payment option.</param>
        public static Boolean TryParse(String Text, out PaymentOptions PaymentOption)
        {
            switch (Text?.Trim())
            {

                case "No Payment":
                    PaymentOption = PaymentOptions.NoPayment;
                    return true;

                case "Direct":
                    PaymentOption = PaymentOptions.Direct;
                    return true;

                case "Contract":
                    PaymentOption = PaymentOptions.Contract;
                    return true;

                default:
                    PaymentOption = PaymentOptions.Contract;
                    return false;

            }
        }

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
    public enum PaymentOptions
    {

        /// <summary>
        /// No payment / free.
        /// </summary>
        /// <remarks>No Payment can not be combined with another payment option!</remarks>
        NoPayment,

        /// <summary>
        /// Direct payment via e.g. cash, credit/debit card, SMS or phone call.
        /// </summary>
        Direct,

        /// <summary>
        /// Payment via e.g. contract or subscription.
        /// </summary>
        Contract

    }

}
