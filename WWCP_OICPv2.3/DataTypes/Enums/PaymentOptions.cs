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

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a paymet option.
        /// </summary>
        /// <param name="Text">A text-representation of a payment option.</param>
        public static PaymentOptions Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "No Payment" : return PaymentOptions.NoPayment;
                case "Direct"     : return PaymentOptions.Direct;
                case "Contract"   : return PaymentOptions.Contract;
                default           : return PaymentOptions.Unspecified;
            }
        }

        #endregion

        #region AsString(PaymentOption)

        /// <summary>
        /// Return a text-representation of the given paymet option.
        /// </summary>
        /// <param name="PaymentOption">A paymet option.</param>
        public static String AsString(this PaymentOptions PaymentOption)
        {
            switch (PaymentOption)
            {
                case PaymentOptions.NoPayment : return "No Payment";
                case PaymentOptions.Direct    : return "Direct";
                case PaymentOptions.Contract  : return "Contract";
                default                       : return "Unspecified";
            }
        }

        #endregion

    }


    /// <summary>
    /// Payment options.
    /// </summary>
    public enum PaymentOptions
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified,

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
