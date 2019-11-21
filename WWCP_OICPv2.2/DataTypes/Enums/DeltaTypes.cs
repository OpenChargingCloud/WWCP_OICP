/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// OICP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        #region AsDeltaType(Text)

        public static DeltaTypes AsDeltaType(this String Text)
        {

            switch (Text)
            {

                case "update":
                    return DeltaTypes.update;

                case "insert":
                    return DeltaTypes.insert;

                case "delete":
                    return DeltaTypes.delete;

                default:
                    return DeltaTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this DeltaType)

        public static String AsText(this DeltaTypes DeltaType)
        {

            switch (DeltaType)
            {

                case DeltaTypes.update:
                    return "update";

                case DeltaTypes.insert:
                    return "insert";

                case DeltaTypes.delete:
                    return "delete";

                default:
                    return "Unknown";

            }

        }

        #endregion

    }


    /// <summary>
    /// OICP delta type for e.g. EVSE data records.
    /// </summary>
    public enum DeltaTypes
    {

        /// <summary>
        /// Unknown delta type.
        /// </summary>
        Unknown,

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
