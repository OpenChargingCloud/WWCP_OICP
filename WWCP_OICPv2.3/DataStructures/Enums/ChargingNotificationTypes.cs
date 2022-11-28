/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
    /// Extensions methods for charging notification types.
    /// </summary>
    public static class ChargingNotificationTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a charging notification type.
        /// </summary>
        /// <param name="Text">A text representation of a charging notification type.</param>
        public static ChargingNotificationTypes Parse(String Text)
        {

            if (TryParse(Text, out var chargingNotificationType))
                return chargingNotificationType;

            throw new ArgumentException("Undefined charging notification type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a charging notification type.
        /// </summary>
        /// <param name="Text">A text representation of a charging notification type.</param>
        public static ChargingNotificationTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingNotificationType))
                return chargingNotificationType;

            return default;

        }

        #endregion

        #region TryParse(Text, out ChargingNotificationType)

        /// <summary>
        /// Parses the given text representation of a charging notification type.
        /// </summary>
        /// <param name="Text">A text representation of a charging notification type.</param>
        /// <param name="ChargingNotificationType">The parsed charging notification type.</param>
        public static Boolean TryParse(String Text, out ChargingNotificationTypes ChargingNotificationType)
        {
            switch (Text?.Trim()?.ToLower())
            {

                case "start":
                    ChargingNotificationType = ChargingNotificationTypes.Start;
                    return true;

                case "progress":
                    ChargingNotificationType = ChargingNotificationTypes.Progress;
                    return true;

                case "end":
                    ChargingNotificationType = ChargingNotificationTypes.End;
                    return true;

                case "error":
                    ChargingNotificationType = ChargingNotificationTypes.Error;
                    return true;

                default:
                    ChargingNotificationType = ChargingNotificationTypes.Error;
                    return false;

            }
        }

        #endregion

        #region AsString(ChargingMode)

        /// <summary>
        /// Return a text representation of the given charging notification type.
        /// </summary>
        /// <param name="ChargingMode">A charging notification type.</param>
        public static String AsString(this ChargingNotificationTypes ChargingMode)

            => ChargingMode switch {
                   ChargingNotificationTypes.Start    => "Start",
                   ChargingNotificationTypes.Progress => "Progress",
                   ChargingNotificationTypes.End      => "End",
                   _                                  => "Error",
               };

        #endregion

    }


    /// <summary>
    /// Charging notification types.
    /// </summary>
    public enum ChargingNotificationTypes
    {

        /// <summary>
        /// Indicates if the Notification refers to the start of a charging process.
        /// </summary>
        Start,

        /// <summary>
        /// Indicates if the Notification of the progress of the charging session.
        /// </summary>
        Progress,

        /// <summary>
        /// Indicates if the Notification refers to an end of a charging process.
        /// </summary>
        End,

        /// <summary>
        /// Indicates if the Notification refers to an error.
        /// </summary>
        Error

    }

}
