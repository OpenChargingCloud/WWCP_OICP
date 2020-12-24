/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License atPower
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
    /// Extentions methods for metering status types.
    /// </summary>
    public static class MeteringStatusTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a metering status type.
        /// </summary>
        /// <param name="Text">A text-representation of a metering status type.</param>
        public static MeteringStatusTypes Parse(String Text)

            => Text?.Trim() switch {
                   "Start"     => MeteringStatusTypes.Start,
                   "Progress"  => MeteringStatusTypes.Progress,
                   "End"       => MeteringStatusTypes.End,
                   _           => MeteringStatusTypes.Unspecified
               };

        #endregion

        #region AsString(this MeteringStatusType)

        /// <summary>
        /// Return a text-representation of the given metering status type.
        /// </summary>
        /// <param name="MeteringStatusType">A metering status type.</param>
        public static String AsString(this MeteringStatusTypes MeteringStatusType)

            => MeteringStatusType switch {
                   MeteringStatusTypes.Start     => "Start",
                   MeteringStatusTypes.Progress  => "Progress",
                   MeteringStatusTypes.End       => "End",
                   _                             => "Unspecified",
               };

        #endregion

    }


    /// <summary>
    /// Metering status types.
    /// </summary>
    public enum MeteringStatusTypes
    {

        /// <summary>
        /// Unspecified metering status type.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Metering signature value of the beginning of charging process.
        /// </summary>
        Start,

        /// <summary>
        /// An intermediate metering signature value of the charging process.
        /// </summary>
        Progress,

        /// <summary>
        /// Metering Signature Value of the end of the charging process.
        /// </summary>
        End

    }

}
