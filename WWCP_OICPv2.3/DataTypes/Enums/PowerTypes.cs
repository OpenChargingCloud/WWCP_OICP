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
    /// Extentions methods for power types.
    /// </summary>
    public static class PowerTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a power type.
        /// </summary>
        /// <param name="Text">A text-representation of a power type.</param>
        public static PowerTypes Parse(String Text)

            => Text?.Trim() switch {
                   "AC_1_PHASE"  => PowerTypes.AC_1_PHASE,
                   "AC_3_PHASE"  => PowerTypes.AC_3_PHASE,
                   "DC"          => PowerTypes.DC,
                   _             => PowerTypes.Unspecified,
               };

        #endregion

        #region AsString(this PowerType)

        /// <summary>
        /// Return a text-representation of the given power type.
        /// </summary>
        /// <param name="PowerType">A power type.</param>
        public static String AsString(this PowerTypes PowerType)

            => PowerType switch {
                   PowerTypes.AC_1_PHASE  => "AC_1_PHASE",
                   PowerTypes.AC_3_PHASE  => "AC_3_PHASE",
                   PowerTypes.DC          => "DC",
                   _                      => "Unspecified",
               };

        #endregion

    }


    /// <summary>
    /// Power types.
    /// </summary>
    public enum PowerTypes
    {

        /// <summary>
        /// Unspecified power type.
        /// </summary>
        Unspecified,

        /// <summary>
        /// AC_1_PHASE
        /// </summary>
        AC_1_PHASE,

        /// <summary>
        /// AC_3_PHASE
        /// </summary>
        AC_3_PHASE,

        /// <summary>
        /// DC
        /// </summary>
        DC

    }

}
