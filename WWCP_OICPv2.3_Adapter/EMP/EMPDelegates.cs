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

using WWCP = cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// A delegate which allows you to modify the convertion from EVSE data records to WWCP EVSEs.
    /// </summary>
    /// <param name="EVSEDataRecord">An OICP EVSE data record.</param>
    /// <param name="EVSE">A WWCP EVSE.</param>
    public delegate WWCP.EVSE                EVSEDataRecord2EVSEDelegate                      (EVSEDataRecord           EVSEDataRecord,
                                                                                               WWCP.EVSE                EVSE);

    /// <summary>
    /// A delegate which allows you to modify the convertion from EVSE status records to WWCP EVSE status updates.
    /// </summary>
    /// <param name="EVSEStatusRecord">An OICP EVSE status record.</param>
    /// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    public delegate WWCP.EVSEStatusUpdate    EVSEStatusRecord2EVSEStatusUpdateDelegate        (EVSEStatusRecord         EVSEStatusRecord,
                                                                                               WWCP.EVSEStatusUpdate    EVSEStatusUpdate);

    /// <summary>
    /// A delegate which allows you to modify the convertion from charge detail records to WWCP charge detail records.
    /// </summary>
    /// <param name="EVSEStatusRecord">An OICP charge detail record.</param>
    /// <param name="EVSEStatus">A WWCP charge detail record.</param>
    public delegate WWCP.ChargeDetailRecord  ChargeDetailRecord2WWCPChargeDetailRecordDelegate(ChargeDetailRecord       ChargeDetailRecord,
                                                                                               WWCP.ChargeDetailRecord  WWCPChargeDetailRecord);

}
