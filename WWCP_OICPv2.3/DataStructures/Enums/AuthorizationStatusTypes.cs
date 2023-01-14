/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// Extensions methods for authorization status types.
    /// </summary>
    public static class AuthorizationStatusTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of an authorization status type.
        /// </summary>
        /// <param name="Text">A text representation of an authorization status type.</param>
        public static AuthorizationStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out var authorizationStatusType))
                return authorizationStatusType;

            throw new ArgumentException("Undefined authorization status type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of an authorization status type.
        /// </summary>
        /// <param name="Text">A text representation of an authorization status type.</param>
        public static AuthorizationStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var authorizationStatusType))
                return authorizationStatusType;

            return default;

        }

        #endregion

        #region TryParse(Text, out AuthorizationStatusType)

        /// <summary>
        /// Parses the given text representation of an authorization status type.
        /// </summary>
        /// <param name="Text">A text representation of an authorization status type.</param>
        /// <param name="AuthorizationStatusType">The parsed authorization status type</param>
        public static Boolean TryParse(String Text, out AuthorizationStatusTypes AuthorizationStatusType)
        {
            switch (Text?.Trim())
            {

                case "Authorized":
                    AuthorizationStatusType = AuthorizationStatusTypes.Authorized;
                    return true;

                case "NotAuthorized":
                    AuthorizationStatusType = AuthorizationStatusTypes.NotAuthorized;
                    return true;

                default:
                    AuthorizationStatusType = AuthorizationStatusTypes.NotAuthorized;
                    return false;

            }
        }

        #endregion

        #region AsString(AuthorizationStatusType)

        /// <summary>
        /// Return a text representation of the given authorization status type.
        /// </summary>
        /// <param name="AuthorizationStatusType">An authorization status type.</param>
        /// <returns></returns>
        public static String AsString(this AuthorizationStatusTypes AuthorizationStatusType)

            => AuthorizationStatusType switch {
                   AuthorizationStatusTypes.Authorized     => "Authorized",
                   AuthorizationStatusTypes.NotAuthorized  => "NotAuthorized",
                   _                                       => "Undefined"
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
