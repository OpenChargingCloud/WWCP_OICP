﻿/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    #region OnMobileAuthorizeStart

    /// <summary>
    /// A delegate called whenever a MobileAuthorizeStart request will be send.
    /// </summary>
    public delegate Task OnMobileAuthorizeStartRequestDelegate (DateTime                   LogTimestamp,
                                                                DateTime                   RequestTimestamp,
                                                                MobileClient               Sender,
                                                                String                     SenderId,
                                                                EventTracking_Id           EventTrackingId,
                                                                EVSE_Id                    EVSEId,
                                                                EVCOIdWithPIN              EVCOIdWithPIN,
                                                                String                     ProductId,
                                                                Boolean?                   GetNewSession,
                                                                TimeSpan?                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a MobileAuthorizeStart request had been received.
    /// </summary>
    public delegate Task OnMobileAuthorizeStartResponseDelegate(DateTime                   LogTimestamp,
                                                                DateTime                   RequestTimestamp,
                                                                MobileClient               Sender,
                                                                String                     SenderId,
                                                                EventTracking_Id           EventTrackingId,
                                                                EVSE_Id                    EVSEId,
                                                                EVCOIdWithPIN              EVCOIdWithPIN,
                                                                String                     ProductId,
                                                                Boolean?                   GetNewSession,
                                                                TimeSpan?                  RequestTimeout,
                                                                MobileAuthorizationStart   Result,
                                                                TimeSpan                   Duration);

    #endregion

    #region OnMobileRemoteStart

    /// <summary>
    /// A delegate called whenever a MobileAuthorizeStart request will be send.
    /// </summary>
    public delegate Task OnMobileRemoteStartRequestDelegate (DateTime             LogTimestamp,
                                                             DateTime             RequestTimestamp,
                                                             MobileClient         Sender,
                                                             String               SenderId,
                                                             EventTracking_Id     EventTrackingId,
                                                             Session_Id           SessionId,
                                                             TimeSpan?            RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a MobileAuthorizeStart request had been received.
    /// </summary>
    public delegate Task OnMobileRemoteStartResponseDelegate(DateTime             LogTimestamp,
                                                             DateTime             RequestTimestamp,
                                                             MobileClient         Sender,
                                                             String               SenderId,
                                                             EventTracking_Id     EventTrackingId,
                                                             Session_Id           SessionId,
                                                             TimeSpan?            RequestTimeout,
                                                             Acknowledgement      Result,
                                                             TimeSpan             Duration);

    #endregion

    #region OnMobileRemoteStop

    /// <summary>
    /// A delegate called whenever a MobileAuthorizeStart request will be send.
    /// </summary>
    public delegate Task OnMobileRemoteStopRequestDelegate (DateTime              LogTimestamp,
                                                            DateTime              RequestTimestamp,
                                                            MobileClient          Sender,
                                                            String                SenderId,
                                                            EventTracking_Id      EventTrackingId,
                                                            Session_Id            SessionId,
                                                            TimeSpan?             RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a MobileAuthorizeStart request had been received.
    /// </summary>
    public delegate Task OnMobileRemoteStopResponseDelegate(DateTime              LogTimestamp,
                                                            DateTime              RequestTimestamp,
                                                            MobileClient          Sender,
                                                            String                SenderId,
                                                            EventTracking_Id      EventTrackingId,
                                                            Session_Id            SessionId,
                                                            TimeSpan?             RequestTimeout,
                                                            Acknowledgement       Result,
                                                            TimeSpan              Duration);

    #endregion

}
