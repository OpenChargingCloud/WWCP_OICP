/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// A delegate which allows you to modify EVSE data records before sending them upstream.
    /// </summary>
    /// <param name="EVSE">A WWCP EVSE.</param>
    /// <param name="EVSEDataRecord">An EVSE data record.</param>
    public delegate EVSEDataRecord    EVSE2EVSEDataRecordDelegate              (EVSE              EVSE,
                                                                                EVSEDataRecord    EVSEDataRecord);


    /// <summary>
    /// A delegate which allows you to modify EVSE status records before sending them upstream.
    /// </summary>
    /// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    /// <param name="EVSEStatusRecord">An OICP EVSE status record.</param>
    public delegate EVSEStatusRecord  EVSEStatusUpdate2EVSEStatusRecordDelegate(EVSEStatusUpdate  EVSEStatusUpdate,
                                                                                EVSEStatusRecord  EVSEStatusRecord);



    /// <summary>
    /// A delegate called whenever new EVSE data will be send upstream.
    /// </summary>
    public delegate void OnPushEVSEDataWWCPRequestDelegate   (DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              CSORoamingProvider_Id            SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              RoamingNetwork_Id                RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              UInt64                           NumberOfEVSEDataRecords,
                                                              IEnumerable<EVSEDataRecord>      EVSEDataRecords,
                                                              IEnumerable<String>              Warnings,
                                                              TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever new EVSE data had been send upstream.
    /// </summary>
    public delegate void OnPushEVSEDataWWCPResponseDelegate  (DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              CSORoamingProvider_Id            SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              RoamingNetwork_Id                RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              UInt64                           NumberOfEVSEDataRecords,
                                                              IEnumerable<EVSEDataRecord>      EVSEDataRecords,
                                                              IEnumerable<String>              Warnings,
                                                              TimeSpan?                        RequestTimeout,
                                                              WWCP.Acknowledgement             Result,
                                                              TimeSpan                         Runtime);


    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate void OnPushEVSEStatusWWCPRequestDelegate (DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              CSORoamingProvider_Id            SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              RoamingNetwork_Id                RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              UInt64                           NumberOfEVSEDataRecords,
                                                              IEnumerable<EVSEStatusRecord>    EVSEDataRecords,
                                                              IEnumerable<String>              Warnings,
                                                              TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate void OnPushEVSEStatusWWCPResponseDelegate(DateTime                         LogTimestamp,
                                                              DateTime                         RequestTimestamp,
                                                              Object                           Sender,
                                                              CSORoamingProvider_Id            SenderId,
                                                              EventTracking_Id                 EventTrackingId,
                                                              RoamingNetwork_Id                RoamingNetworkId,
                                                              ActionTypes                      ServerAction,
                                                              UInt64                           NumberOfEVSEDataRecords,
                                                              IEnumerable<EVSEStatusRecord>    EVSEDataRecords,
                                                              IEnumerable<String>              Warnings,
                                                              TimeSpan?                        RequestTimeout,
                                                              WWCP.Acknowledgement             Result,
                                                              TimeSpan                         Runtime);


}
