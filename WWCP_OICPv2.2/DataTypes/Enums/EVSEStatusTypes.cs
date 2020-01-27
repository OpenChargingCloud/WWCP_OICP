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

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// OICP XML I/O.
    /// </summary>
    public static partial class XML_IO
    {

        #region AsEVSEStatusType(EVSEStatusType)

        /// <summary>
        /// Parses OICP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An EVSE status.</param>
        public static EVSEStatusTypes AsEVSEStatusType(String EVSEStatusType)
        {

            switch (EVSEStatusType.Trim())
            {

                case "Available":
                    return EVSEStatusTypes.Available;

                case "Reserved":
                    return EVSEStatusTypes.Reserved;

                case "Occupied":
                    return EVSEStatusTypes.Occupied;

                case "OutOfService":
                    return EVSEStatusTypes.OutOfService;

                case "EvseNotFound":
                    return EVSEStatusTypes.EvseNotFound;


                default:
                    return EVSEStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsString(EVSEStatusType)

        public static String AsText(this EVSEStatusTypes EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case EVSEStatusTypes.Available:
                    return "Available";

                case EVSEStatusTypes.Reserved:
                    return "Reserved";

                case EVSEStatusTypes.Occupied:
                    return "Occupied";

                case EVSEStatusTypes.OutOfService:
                    return "OutOfService";

                case EVSEStatusTypes.EvseNotFound:
                    return "EvseNotFound";


                default:
                    return "Unknown";

            }

        }

        #endregion

    }


    /// <summary>
    /// The current dynamic status of an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public enum EVSEStatusTypes
    {

        /// <summary>
        /// The status or EVSE is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The EVSE is available.
        /// </summary>
        Available,

        /// <summary>
        /// The EVSE is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The EVSE is occupied.
        /// </summary>
        Occupied,

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE was not found.
        /// (PullAuthorizationStartById)
        /// </summary>
        EvseNotFound

    }

}
