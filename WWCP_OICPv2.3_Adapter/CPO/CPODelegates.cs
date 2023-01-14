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

#region Usings

using System;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using WWCP = cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    ///// <summary>
    ///// A delegate which allows you to modify the convertion from WWCP EVSEs to EVSE data records.
    ///// </summary>
    ///// <param name="EVSE">A WWCP EVSE.</param>
    ///// <param name="EVSEDataRecord">An EVSE data record.</param>
    //public delegate EVSEDataRecord      EVSE2EVSEDataRecordDelegate                      (WWCP.EVSE                EVSE,
    //                                                                                      EVSEDataRecord           EVSEDataRecord);

    ///// <summary>
    ///// A delegate which allows you to modify the convertion from WWCP EVSE status updates to EVSE status records.
    ///// </summary>
    ///// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    ///// <param name="EVSEStatusRecord">An OICP EVSE status record.</param>
    //public delegate EVSEStatusRecord    EVSEStatusUpdate2EVSEStatusRecordDelegate        (WWCP.EVSEStatusUpdate    EVSEStatusUpdate,
    //                                                                                      EVSEStatusRecord         EVSEStatusRecord);

    ///// <summary>
    ///// A delegate which allows you to modify the convertion from WWCP charge detail records to OICP charge detail records.
    ///// </summary>
    ///// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    ///// <param name="OCIPChargeDetailRecord">An OICP charge detail record.</param>
    //public delegate ChargeDetailRecord  WWCPChargeDetailRecord2ChargeDetailRecordDelegate(WWCP.ChargeDetailRecord  WWCPChargeDetailRecord,
    //                                                                                      ChargeDetailRecord       OCIPChargeDetailRecord);

    ///// <summary>
    ///// A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.
    ///// </summary>
    ///// <param name="OCIPChargeDetailRecord">An OICP charge detail record.</param>
    ///// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    //public delegate WWCP.ChargeDetailRecord  ChargeDetailRecord2WWCPChargeDetailRecordDelegate(ChargeDetailRecord       OICPChargeDetailRecord,
    //                                                                                           WWCP.ChargeDetailRecord  WWCPChargeDetailRecord);


    #region OnPushEVSEDataWWCPRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE data will be send upstream.
    /// </summary>
    public delegate void OnPushEVSEDataWWCPRequestDelegate   (DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              WWCP.EMPRoamingProvider_Id       SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              WWCP.RoamingNetwork_Id           RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              IEnumerable<EVSEDataRecord>      EVSEDataRecords,
                                                              IEnumerable<Warning>             Warnings,
                                                              TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever new EVSE data had been send upstream.
    /// </summary>
    public delegate void OnPushEVSEDataWWCPResponseDelegate  (DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              WWCP.EMPRoamingProvider_Id       SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              WWCP.RoamingNetwork_Id           RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              IEnumerable<EVSEDataRecord>      EVSEDataRecords,
                                                              TimeSpan?                        RequestTimeout,
                                                              WWCP.PushEVSEDataResult          Result,
                                                              TimeSpan                         Runtime);

    #endregion

    #region OnPushEVSEStatusWWCPRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate void OnPushEVSEStatusWWCPRequestDelegate (DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              WWCP.EMPRoamingProvider_Id       SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              WWCP.RoamingNetwork_Id           RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              IEnumerable<EVSEStatusRecord>    EVSEDataRecords,
                                                              IEnumerable<Warning>             Warnings,
                                                              TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate void OnPushEVSEStatusWWCPResponseDelegate(DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              WWCP.EMPRoamingProvider_Id       SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              WWCP.RoamingNetwork_Id           RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              IEnumerable<EVSEStatusRecord>    EVSEDataRecords,
                                                              TimeSpan?                        RequestTimeout,
                                                              WWCP.PushEVSEStatusResult        Result,
                                                              TimeSpan                         Runtime);

    #endregion

}
