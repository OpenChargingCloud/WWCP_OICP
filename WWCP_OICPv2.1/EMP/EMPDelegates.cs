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
    /// A delegate which allows you to modify EVSE data records
    /// after receiving them.
    /// </summary>
    /// <param name="EVSEDataRecord">An OICPv2.1 EVSE data record.</param>
    /// <param name="EVSE">A WWCP EVSE.</param>
    public delegate EVSE EVSEDataRecord2EVSEDelegate(EVSEDataRecord  EVSEDataRecord,
                                                     EVSE            EVSE);

}
