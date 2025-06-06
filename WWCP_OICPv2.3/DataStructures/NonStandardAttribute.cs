﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Tag a struct, class or property as 'non-standard'.
    /// </summary>
    /// <remarks>
    /// Create a new 'non-standard'-tag having the given tags.
    /// </remarks>
    /// <param name="Tags">Some tags.</param>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Property,
                    AllowMultiple = false,
                    Inherited = true)]
    public class NonStandardAttribute(params String[] Tags) : Attribute
    {

        #region Tags

        /// <summary>
        /// Additional tags of the 'non-standard'-tag.
        /// </summary>
        public IEnumerable<String>  Tags    { get; } = Tags?.Where(tag => !String.IsNullOrEmpty(tag)).Distinct().ToArray() ?? [];

        #endregion

    }

}
