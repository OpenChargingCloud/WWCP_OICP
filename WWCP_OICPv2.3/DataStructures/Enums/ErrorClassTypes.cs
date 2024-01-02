/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extensions methods for error class types.
    /// </summary>
    public static class ErrorClassTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an error class type.
        /// </summary>
        /// <param name="Text">A text representation of an error class type.</param>
        public static ErrorClassTypes Parse(String Text)
        {

            if (TryParse(Text, out var errorClassTypes))
                return errorClassTypes;

            throw new ArgumentException("Undefined error class type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an error class type.
        /// </summary>
        /// <param name="Text">A text representation of an error class type.</param>
        public static ErrorClassTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var errorClassTypes))
                return errorClassTypes;

            return default;

        }

        #endregion

        #region TryParse(Text, out ErrorClassType)

        /// <summary>
        /// Parses the given text representation of an error class type.
        /// </summary>
        /// <param name="Text">A text representation of an error class type.</param>
        /// <param name="ErrorClassType">The parsed error class type.</param>
        public static Boolean TryParse(String Text, out ErrorClassTypes ErrorClassType)
        {
            switch (Text?.Trim())
            {

                case "ConnectorError":
                    ErrorClassType = ErrorClassTypes.ConnectorError;
                    return true;

                case "CriticalError":
                    ErrorClassType = ErrorClassTypes.CriticalError;
                    return true;

                default:
                    ErrorClassType = ErrorClassTypes.UnknownError;
                    return false;

            }
        }

        #endregion

        #region AsString(ErrorClassType)

        /// <summary>
        /// Return a text representation of the given error class.
        /// </summary>
        /// <param name="ErrorClassType">An error class.</param>
        public static String AsString(this ErrorClassTypes ErrorClassType)

            => ErrorClassType switch {
                   ErrorClassTypes.ConnectorError => "ConnectorError",
                   ErrorClassTypes.CriticalError  => "CriticalError",
                   _                              => "Unknown"
               };

        #endregion

    }

    /// <summary>
    /// Charging notifications error class types.
    /// </summary>
    public enum ErrorClassTypes
    {

        /// <summary>
        /// No error information available.
        /// </summary>
        UnknownError,

        /// <summary>
        /// The charging process cannot be started or stopped.
        /// The EV driver needs to check if the the plug is properly inserted or taken out from the socket.
        /// </summary>
        ConnectorError,

        /// <summary>
        /// Charging process stopped abruptly. Reason: Physical check at the station is required. Station cannot be reset online.
        /// Error with the software or hardware of the station locally.
        /// Communication failure with the vehicle.
        /// The error needs to be investigated.
        /// Ground Failure.
        /// </summary>
        CriticalError

    }

}
