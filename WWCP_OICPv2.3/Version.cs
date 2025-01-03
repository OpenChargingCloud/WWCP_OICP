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
    /// This OICP version 2.3.
    /// </summary>
    public static class Version
    {

        /// <summary>
        /// This OICP version 2.3 as text "v2.3".
        /// </summary>
        public const           String      String   = "v2.3";

        /// <summary>
        /// This OICP version "2.3" as version identification.
        /// </summary>
        public readonly static Version_Id  Id       = Version_Id.Parse(String[1..]);

    }

}
