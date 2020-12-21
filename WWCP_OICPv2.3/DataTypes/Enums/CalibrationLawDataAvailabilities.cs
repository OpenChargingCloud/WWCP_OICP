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
    /// Extentions methods for EVSE status types.
    /// </summary>
    public static class CalibrationLawDataAvailabilitiesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a calibration law data availability object.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of a calibration law data availability object.</param>
        public static CalibrationLawDataAvailabilities Parse(String Text)
        {
            switch (Text?.Trim()) {
                case "Local"    : return CalibrationLawDataAvailabilities.Local;
                case "External" : return CalibrationLawDataAvailabilities.External;
                default         : return CalibrationLawDataAvailabilities.NotAvailable;
            };
        }

        #endregion

        #region AsString(CalibrationLawDataAvailability)

        /// <summary>
        /// Return a text-representation of the given calibration law data availability object.
        /// </summary>
        /// <param name="CalibrationLawDataAvailability">A calibration law data availability object.</param>
        public static String AsString(this CalibrationLawDataAvailabilities CalibrationLawDataAvailability
            )
        {
            switch (CalibrationLawDataAvailability) {
                case CalibrationLawDataAvailabilities.Local    : return "Local";
                case CalibrationLawDataAvailabilities.External : return "External";
                default:                                         return "Not Available";
            };
        }

        #endregion

    }

    /// <summary>
    /// Error and status codes.
    /// </summary>
    public enum CalibrationLawDataAvailabilities
    {

        /// <summary>
        /// Calibration law data is shown at the charging station.
        /// </summary>
        Local,

        /// <summary>
        /// Calibration law data is provided externaly
        /// </summary>
        External,

        /// <summary>
        /// Calibration law data is not provided.
        /// </summary>
        NotAvailable,

    }

}
