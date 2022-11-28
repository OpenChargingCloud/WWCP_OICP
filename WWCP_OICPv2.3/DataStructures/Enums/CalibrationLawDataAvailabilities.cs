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
    /// Extensions methods for EVSE status types.
    /// </summary>
    public static class CalibrationLawDataAvailabilitiesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text-representation of a calibration law data availability.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of a calibration law data availability.</param>
        public static CalibrationLawDataAvailabilities Parse(String Text)
        {

            if (TryParse(Text, out var calibrationLawDataAvailability))
                return calibrationLawDataAvailability;

            throw new ArgumentException("Undefined calibration law data availability '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text-representation of a calibration law data availability.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of a calibration law data availability.</param>
        public static CalibrationLawDataAvailabilities? TryParse(String Text)
        {

            if (TryParse(Text, out var calibrationLawDataAvailability))
                return calibrationLawDataAvailability;

            return default;

        }

        #endregion

        #region TryParse(Text, out CalibrationLawDataAvailability)

        /// <summary>
        /// Parses the given text-representation of a calibration law data availability.
        /// </summary>
        /// <param name="Text">Parses the given text-representation of a calibration law data availability.</param>
        /// <param name="CalibrationLawDataAvailability">The parsed calibration law data availability.</param>
        public static Boolean TryParse(String Text, out CalibrationLawDataAvailabilities CalibrationLawDataAvailability)
        {
            switch (Text?.Trim())
            {

                case "Local":
                    CalibrationLawDataAvailability = CalibrationLawDataAvailabilities.Local;
                    return true;

                case "External":
                    CalibrationLawDataAvailability = CalibrationLawDataAvailabilities.External;
                    return true;

                case "Not Available":
                    CalibrationLawDataAvailability = CalibrationLawDataAvailabilities.NotAvailable;
                    return true;

                default:
                    CalibrationLawDataAvailability = CalibrationLawDataAvailabilities.NotAvailable;
                    return false;

            }
        }

        #endregion

        #region AsString(CalibrationLawDataAvailability)

        /// <summary>
        /// Return a text-representation of the given calibration law data availability.
        /// </summary>
        /// <param name="CalibrationLawDataAvailability">A calibration law data availability.</param>
        public static String AsString(this CalibrationLawDataAvailabilities CalibrationLawDataAvailability)

            => CalibrationLawDataAvailability switch {
                   CalibrationLawDataAvailabilities.Local         => "Local",
                   CalibrationLawDataAvailabilities.External      => "External",
                   CalibrationLawDataAvailabilities.NotAvailable  => "Not Available",
                   _                                              => "Undefined"
               };

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
