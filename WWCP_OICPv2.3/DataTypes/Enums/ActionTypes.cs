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
    /// Extensions methods for action types.
    /// </summary>
    public static class ActionTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text-representation of an action type.
        /// </summary>
        /// <param name="Text">A text-representation of an action type.</param>
        public static ActionTypes Parse(String Text)
        {

            if (TryParse(Text, out ActionTypes actionType))
                return actionType;

            throw new ArgumentException("Undefined action type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text-representation of an action type.
        /// </summary>
        /// <param name="Text">A text-representation of an action type.</param>
        public static ActionTypes? TryParse(String Text)
        {

            if (TryParse(Text, out ActionTypes actionType))
                return actionType;

            return default;

        }

        #endregion

        #region TryParse(Text, out ActionType)

        /// <summary>
        /// Parses the given text-representation of an action type.
        /// </summary>
        /// <param name="Text">A text-representation of an action type.</param>
        /// <param name="ActionType">The parsed action type</param>
        public static Boolean TryParse(String Text, out ActionTypes ActionType)
        {
            switch (Text?.Trim())
            {

                case "fullLoad":
                    ActionType = ActionTypes.FullLoad;
                    return true;

                case "update":
                    ActionType = ActionTypes.Update;
                    return true;

                case "insert":
                    ActionType = ActionTypes.Insert;
                    return true;

                case "delete":
                    ActionType = ActionTypes.Delete;
                    return true;

                default:
                    ActionType = ActionTypes.FullLoad;
                    return false;

            }
        }

        #endregion

        #region AsString(this ActionType)

        /// <summary>
        /// Return a text-representation of the given action type.
        /// </summary>
        /// <param name="ActionType">An action type.</param>
        public static String AsString(this ActionTypes ActionType)

            => ActionType switch {
                   ActionTypes.FullLoad  => "fullLoad",
                   ActionTypes.Update    => "update",
                   ActionTypes.Insert    => "insert",
                   ActionTypes.Delete    => "delete",
                   _                     => "unknown",
               };

        #endregion

    }


    /// <summary>
    /// The type of data management action when updating remote data.
    /// </summary>
    public enum ActionTypes
    {

        /// <summary>
        /// Replace all server-side data with the new data.
        /// </summary>
        FullLoad,

        /// <summary>
        /// Update the server-side data with the given data.
        /// Will act like an 'upsert' for dynamic EVSE states!
        /// </summary>
        Update,

        /// <summary>
        /// Insert the given data into the server-side data.
        /// Will fail if it already exists!
        /// </summary>
        Insert,

        /// <summary>
        /// Delete the given data from the server-side data set.
        /// </summary>
        Delete

    }

}
