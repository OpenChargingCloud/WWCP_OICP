/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
    /// Extentions methods for geo coordinates formats.
    /// </summary>
    public static class GeoCoordinatesFormatsExtentions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text-representation of a geo coordinates format.
        /// </summary>
        /// <param name="Text">A text-representation of a geo coordinates format.</param>
        public static GeoCoordinatesFormats Parse(String Text)
        {

            if (TryParse(Text, out GeoCoordinatesFormats geoCoordinatesFormat))
                return geoCoordinatesFormat;

            throw new ArgumentException("Undefined geo coordinates format '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text-representation of a geo coordinates format.
        /// </summary>
        /// <param name="Text">A text-representation of a geo coordinates format.</param>
        public static GeoCoordinatesFormats? TryParse(String Text)
        {

            if (TryParse(Text, out GeoCoordinatesFormats geoCoordinatesFormat))
                return geoCoordinatesFormat;

            return default;

        }

        #endregion

        #region TryParse(Text, out GeoCoordinatesFormat)

        /// <summary>
        /// Parses the given text-representation of a geo coordinates format.
        /// </summary>
        /// <param name="Text">A text-representation of a geo coordinates format.</param>
        /// <param name="GeoCoordinatesFormat">The parsed geo coordinates format.</param>
        public static Boolean TryParse(String Text, out GeoCoordinatesFormats GeoCoordinatesFormat)
        {
            switch (Text?.Trim())
            {

                case "Google":
                    GeoCoordinatesFormat = GeoCoordinatesFormats.Google;
                    return true;

                case "DecimalDegree":
                    GeoCoordinatesFormat = GeoCoordinatesFormats.DecimalDegree;
                    return true;

                case "DegreeMinuteSeconds":
                    GeoCoordinatesFormat = GeoCoordinatesFormats.DegreeMinuteSeconds;
                    return true;

                default:
                    GeoCoordinatesFormat = GeoCoordinatesFormats.DecimalDegree;
                    return false;

            };
        }

        #endregion

        #region AsString(GeoCoordinatesFormat)

        /// <summary>
        /// Return a text-representation of the given geo coordinates format.
        /// </summary>
        /// <param name="GeoCoordinatesFormat">A geo coordinates format.</param>
        public static String AsString(this GeoCoordinatesFormats GeoCoordinatesFormat)

            => GeoCoordinatesFormat switch {
                   GeoCoordinatesFormats.Google               => "Google",
                   GeoCoordinatesFormats.DecimalDegree        => "DecimalDegree",
                   GeoCoordinatesFormats.DegreeMinuteSeconds  => "DegreeMinuteSeconds",
                   _                                          => "unknown",
               };

        #endregion

    }

    /// <summary>
    /// The format of geographical coordinates.
    /// </summary>
    public enum GeoCoordinatesFormats
    {

        /// <summary>
        /// Google geo format, e.g. "47.662249 9.360922".
        /// </summary>
        Google,

        /// <summary>
        /// DecimalDegree geo format, e.g. "9.360922" "-21.568201".
        /// </summary>
        DecimalDegree,

        /// <summary>
        /// DegreeMinuteSeconds geo format, e.g. "9° 21' 39.32''", "-21° 34' 23.16".
        /// </summary>
        DegreeMinuteSeconds

    }

}
