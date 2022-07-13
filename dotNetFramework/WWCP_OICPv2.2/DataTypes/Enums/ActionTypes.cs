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
    /// Extentions methods for action types.
    /// </summary>
    public static class ActionTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of an action type.
        /// </summary>
        /// <param name="Text">A text-representation of an action type.</param>
        public static ActionTypes Parse(String Text)

            => Text switch {
                "fullLoad"  => ActionTypes.fullLoad,
                "update"    => ActionTypes.update,
                "insert"    => ActionTypes.insert,
                "delete"    => ActionTypes.delete,
                _           => ActionTypes.Unknown,
            };

        #endregion

        #region AsString(this ActionType)

        /// <summary>
        /// Return a text-representation of the given action type.
        /// </summary>
        /// <param name="ActionType">An action type.</param>
        public static String AsString(this ActionTypes ActionType)

            => ActionType switch {
                ActionTypes.fullLoad  => "fullLoad",
                ActionTypes.update    => "update",
                ActionTypes.insert    => "insert",
                ActionTypes.delete    => "delete",
                _                     => "Unknown",
            };

        #endregion

    }


    /// <summary>
    /// The type of data management action when updating remote data.
    /// </summary>
    public enum ActionTypes
    {

        /// <summary>
        /// Unknown action.
        /// </summary>
        Unknown,

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
