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
    /// The different formats of e-mobility provider identifications.
    /// </summary>
    public enum ProviderIdFormats
    {

        /// <summary>
        /// The old DIN format.
        /// (Only used in combination with eMAIds!)
        /// </summary>
        DIN,

        /// <summary>
        /// The old DIN format with a '*' as separator.
        /// </summary>
        DIN_STAR,

        /// <summary>
        /// The old DIN format with a '-' as separator.
        /// (Only used in combination with eMAIds!)
        /// </summary>
        DIN_HYPHEN,


        /// <summary>
        /// The new ISO format.
        /// </summary>
        ISO,

        /// <summary>
        /// The new ISO format with a '-' as separator.
        /// </summary>
        ISO_HYPHEN

    }

}
