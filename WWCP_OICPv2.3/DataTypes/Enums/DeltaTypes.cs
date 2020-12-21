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

namespace cloud.charging.open.protocols.OICPv2_3
{


    /// <summary>
    /// Extentions methods for delta types.
    /// </summary>
    public static class DeltaTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of a delta type.
        /// </summary>
        /// <param name="Text">A text-representation of a delta type.</param>
        public static DeltaTypes Parse(String Text)
        {
            switch (Text?.Trim())
            {
                case "update" : return DeltaTypes.update;
                case "insert" : return DeltaTypes.insert;
                case "delete" : return DeltaTypes.delete;
                default       : return DeltaTypes.unknown;
            }
        }

        #endregion

        #region AsString(this DeltaType)

        /// <summary>
        /// Return a text-representation of the given delta type.
        /// </summary>
        /// <param name="DeltaType">A delta type.</param>
        public static String AsString(this DeltaTypes DeltaType)
        {
            switch (DeltaType)
            {
                case DeltaTypes.update : return "update";
                case DeltaTypes.insert : return "insert";
                case DeltaTypes.delete : return "delete";
                default                : return "unknown";
            }
        }

        #endregion

    }


    /// <summary>
    /// Delta types for e.g. EVSE data records.
    /// </summary>
    public enum DeltaTypes
    {

        /// <summary>
        /// Unknown delta type.
        /// </summary>
        unknown,

        /// <summary>
        /// Updated record.
        /// </summary>
        update,

        /// <summary>
        /// Inserted record.
        /// </summary>
        insert,

        /// <summary>
        /// Deleted record.
        /// </summary>
        delete

    }

}
