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

        #region AsPaymetOptions(PaymetOption)

        /// <summary>
        /// Maps an OICP paymet option to a WWCP paymet option.
        /// </summary>
        /// <param name="PaymetOption">A paymet option.</param>
        public static PaymentOptions AsPaymetOption(String PaymetOption)
        {

            switch (PaymetOption.Trim())
            {

                case "No Payment":
                    return PaymentOptions.NoPayment;

                case "Direct":
                    return PaymentOptions.Direct;

                case "Contract":
                    return PaymentOptions.Contract;

                default:
                    return PaymentOptions.Unspecified;

            }

        }

        #endregion

        #region AsString(PaymentOption)

        public static String AsString(this PaymentOptions PaymentOption)
        {

            switch (PaymentOption)
            {

                case PaymentOptions.NoPayment:
                    return "No Payment";

                case PaymentOptions.Direct:
                    return "Direct";

                case PaymentOptions.Contract:
                    return "Contract";


                default:
                    return "Unkown";

            }

        }

        #endregion

    }


    /// <summary>
    /// OICP payment options.
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
