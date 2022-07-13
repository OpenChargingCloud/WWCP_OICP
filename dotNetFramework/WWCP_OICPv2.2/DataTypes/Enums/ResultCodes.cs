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
    /// Extentions methods for result codes.
    /// </summary>
    public static class ResultCodesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a result code.
        /// </summary>
        /// <param name="Text">A text-representation of a result code.</param>
        public static ResultCodes Parse(String Text)

            => Text?.Trim() switch {
                "OK"             => ResultCodes.OK,
                "Partly"         => ResultCodes.Partly,
                "NotAuthorized"  => ResultCodes.NotAuthorized,
                "InvalidId"      => ResultCodes.InvalidId,
                "Server"         => ResultCodes.Server,
                "Format"         => ResultCodes.Format,
                _                => ResultCodes.Unknown,
            };

        #endregion

        #region AsString(this DeltaType)

        /// <summary>
        /// Return a text-representation of the given result code.
        /// </summary>
        /// <param name="DeltaType">A result code.</param>
        public static String AsString(this ResultCodes DeltaType)

            => DeltaType switch {
                ResultCodes.OK             => "OK",
                ResultCodes.Partly         => "Partly",
                ResultCodes.NotAuthorized  => "NotAuthorized",
                ResultCodes.InvalidId      => "InvalidId",
                ResultCodes.Server         => "Server",
                ResultCodes.Format         => "Format",
                _                          => "Unknown",
            };

        #endregion

    }

    /// <summary>
    /// Result and error codes for the class result as return value for method calls.
    /// </summary>
    public enum ResultCodes
    {

        /// <summary>
        /// Unknown result code.
        /// </summary>
        Unknown,

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        OK,

        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        Partly,

        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        NotAuthorized,

        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        InvalidId,

        /// <summary>
        /// Internal server error.
        /// </summary>
        Server,

        /// <summary>
        /// Data has technical errors.
        /// </summary>
        Format

    }

}
