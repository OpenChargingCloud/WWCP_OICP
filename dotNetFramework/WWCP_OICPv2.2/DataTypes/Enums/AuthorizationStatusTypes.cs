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
    /// Extentions methods for authorization results.
    /// </summary>
    public static class AuthorizationStatusTypesExtentions
    {

        #region Parse(Text)

        /// <summary>
        /// Parses the given text-representation of an authorization result.
        /// </summary>
        /// <param name="Text">A text-representation of an authorization result.</param>
        public static AuthorizationStatusTypes Parse(String Text)

            => Text.Trim() switch {
                "Authorized" => AuthorizationStatusTypes.Authorized,
                _            => AuthorizationStatusTypes.NotAuthorized,
            };

        #endregion

        #region AsString(AuthorizationStatusType)

        /// <summary>
        /// Return a text-representation of the given authorization result.
        /// </summary>
        /// <param name="AuthorizationStatusType">An authorization result.</param>
        public static String AsString(this AuthorizationStatusTypes AuthorizationStatusType)

            => AuthorizationStatusType switch {
                AuthorizationStatusTypes.Authorized  => "Authorized",
                _                                    => "NotAuthorized",
            };

        #endregion

    }

    /// <summary>
    /// The result of an authorization.
    /// </summary>
    public enum AuthorizationStatusTypes
    {

        /// <summary>
        /// Authorized.
        /// </summary>
        Authorized,

        /// <summary>
        /// Not authorized.
        /// </summary>
        NotAuthorized

    }

}
