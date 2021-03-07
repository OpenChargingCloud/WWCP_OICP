/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    #region OnPullEVSEDataRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnPullEVSEDataRequestDelegate (DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        EMPClient                               Sender,
                                                        //String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionTypes                             Action,
                                                        UInt64                                  NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>             EVSEDataRecords,
                                                        TimeSpan                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPullEVSEDataResponseDelegate(DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        EMPClient                               Sender,
                                                        //String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionTypes                             Action,
                                                        UInt64                                  NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>             EVSEDataRecords,
                                                        TimeSpan                                RequestTimeout,
                                                        Acknowledgement<PullEVSEDataRequest>    Result,
                                                        TimeSpan                                Runtime);

    #endregion

    #region OnPullEVSEStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE status record will be send upstream.
    /// </summary>
    public delegate Task OnPullEVSEStatusRequestDelegate (DateTime                                LogTimestamp,
                                                          DateTime                                RequestTimestamp,
                                                          EMPClient                               Sender,
                                                          //String                                  SenderId,
                                                          EventTracking_Id                        EventTrackingId,
                                                          ActionTypes                             Action,
                                                          UInt64                                  NumberOfEVSEStatusRecords,
                                                          IEnumerable<EVSEStatusRecord>           EVSEStatusRecords,
                                                          TimeSpan                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE status record had been send upstream.
    /// </summary>
    public delegate Task OnPullEVSEStatusResponseDelegate(DateTime                                LogTimestamp,
                                                          DateTime                                RequestTimestamp,
                                                          EMPClient                               Sender,
                                                          //String                                  SenderId,
                                                          EventTracking_Id                        EventTrackingId,
                                                          ActionTypes                             Action,
                                                          UInt64                                  NumberOfEVSEStatusRecords,
                                                          IEnumerable<EVSEStatusRecord>           EVSEStatusRecords,
                                                          TimeSpan                                RequestTimeout,
                                                          Acknowledgement<PullEVSEStatusRequest>  Result,
                                                          TimeSpan                                Runtime);

    #endregion

}
