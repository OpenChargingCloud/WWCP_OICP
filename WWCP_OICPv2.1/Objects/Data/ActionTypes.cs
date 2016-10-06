/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// The type of data management action when updating remote data.
    /// </summary>
    public enum ActionTypes
    {

        /// <summary>
        /// Replace all server-side data with the new data.
        /// </summary>
        fullLoad,

        /// <summary>
        /// Update the server-side data with the given data.
        /// Will act like an 'upsert' for dynamic EVSE states!
        /// </summary>
        update,

        /// <summary>
        /// Insert the given data into the server-side data.
        /// Will fail if it already exists!
        /// </summary>
        insert,

        /// <summary>
        /// Delete the given data from the server-side data set.
        /// </summary>
        delete

    }

}
