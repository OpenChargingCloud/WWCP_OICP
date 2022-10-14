/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    public class EVSEToOICPException : OICPException
    {

        public EVSE  EVSE   { get; }

        public EVSEToOICPException(EVSE       EVSE,
                                   Exception  InnerException)

            : base("Could not convert EVSE '" + EVSE.Id + "' to an OICP " + Version.Number + " EVSEDataRecord!",
                   InnerException)

        {

            this.EVSE = EVSE;

        }

    }

}
