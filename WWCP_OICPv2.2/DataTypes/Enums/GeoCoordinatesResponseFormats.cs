﻿/*
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
    public static class GeoCoordinatesResponseFormatsExtentions
    {

        #region Parse(GeoCoordinatesResponseFormat)

        /// <summary>
        /// Parses the given text-representation of a geo coordinate response format.
        /// </summary>
        /// <param name="Text">A text-representation of a geo coordinate response format.</param>
        public static GeoCoordinatesResponseFormats Parse(String Text)

            => Text.Trim() switch {
                "Google"               => GeoCoordinatesResponseFormats.Google,
                "DegreeMinuteSeconds"  => GeoCoordinatesResponseFormats.DegreeMinuteSeconds,
                "DecimalDegree"        => GeoCoordinatesResponseFormats.DecimalDegree,
                _                      => GeoCoordinatesResponseFormats.Unspecified,
            };

        #endregion

        #region AsString(GeoCoordinatesResponseFormat)

        /// <summary>
        /// Return a text-representation of the given geo coordinate response format.
        /// </summary>
        /// <param name="GeoCoordinatesResponseFormat">A geo coordinate response format.</param>
        /// <returns></returns>
        public static String AsString(this GeoCoordinatesResponseFormats GeoCoordinatesResponseFormat)

            => GeoCoordinatesResponseFormat switch {
                GeoCoordinatesResponseFormats.Google               => "Google",
                GeoCoordinatesResponseFormats.DegreeMinuteSeconds  => "DegreeMinuteSeconds",
                GeoCoordinatesResponseFormats.DecimalDegree        => "DecimalDegree",
                _                                                  => "Unspecified",
            };

        #endregion

    }

    /// <summary>
    /// Defines the format of geo coordinates that shall be provided with the response.
    /// </summary>
    public enum GeoCoordinatesResponseFormats
    {

        /// <summary>
        /// Unspecified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Google format.
        /// </summary>
        Google,

        /// <summary>
        /// e.g. 42° 23' 5''.
        /// </summary>
        DegreeMinuteSeconds,

        /// <summary>
        /// e.g. 51.23234°.
        /// </summary>
        DecimalDegree

    }

}
