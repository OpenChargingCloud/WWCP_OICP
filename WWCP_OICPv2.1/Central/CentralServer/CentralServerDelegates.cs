/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
 * This file is part of WWCP OCHP <https://github.com/OpenChargingCloud/WWCP_OCHP>
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.WWCP.OICPv2_1.CPO;
using org.GraphDefined.WWCP.OICPv2_1.EMP;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    // EMP

    #region OnPullEVSEData      (Request|Response)Handler

    /// <summary>
    /// A delegate called whenever a PullEVSEData request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEDataRequestDelegate (DateTime                         LogTimestamp,
                                       DateTime                         RequestTimestamp,
                                       CentralServer                    Sender,
                                       String                           SenderId,
                                       EventTracking_Id                 EventTrackingId,
                                       Provider_Id                      ProviderId,
                                       GeoCoordinate?                   SearchCenter,
                                       Single                           DistanceKM,
                                       DateTime?                        LastCall,
                                       GeoCoordinatesResponseFormats?   GeoCoordinatesResponseFormat,
                                       TimeSpan                         RequestTimeout);


    /// <summary>
    /// Send a PullEVSEData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<EVSEData>

        OnPullEVSEDataDelegate(DateTime             Timestamp,
                               CentralServer        Sender,
                               PullEVSEDataRequest  Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEData response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEDataResponseDelegate(DateTime                         Timestamp,
                                       CentralServer                    Sender,
                                       String                           SenderId,
                                       EventTracking_Id                 EventTrackingId,
                                       Provider_Id                      ProviderId,
                                       GeoCoordinate?                   SearchCenter,
                                       Single                           DistanceKM,
                                       DateTime?                        LastCall,
                                       GeoCoordinatesResponseFormats?   GeoCoordinatesResponseFormat,
                                       TimeSpan                         RequestTimeout,
                                       EVSEData                         Result,
                                       TimeSpan                         Duration);

    #endregion

    #region OnPullEVSEStatus    (Request|Response)Handler

    /// <summary>
    /// A delegate called whenever a PullEVSEStatus request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusRequestDelegate (DateTime                         LogTimestamp,
                                         DateTime                         RequestTimestamp,
                                         CentralServer                    Sender,
                                         String                           SenderId,
                                         EventTracking_Id                 EventTrackingId,
                                         Provider_Id                      ProviderId,
                                         GeoCoordinate?                   SearchCenter,
                                         Single                           DistanceKM,
                                         EVSEStatusTypes?                 EVSEStatusFilter,
                                         TimeSpan                         RequestTimeout);


    /// <summary>
    /// Send a PullEVSEStatus request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<EVSEStatus>

        OnPullEVSEStatusDelegate(DateTime               Timestamp,
                                 CentralServer          Sender,
                                 PullEVSEStatusRequest  Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatus response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusResponseDelegate(DateTime                         Timestamp,
                                         CentralServer                    Sender,
                                         String                           SenderId,
                                         EventTracking_Id                 EventTrackingId,
                                         Provider_Id                      ProviderId,
                                         GeoCoordinate?                   SearchCenter,
                                         Single                           DistanceKM,
                                         EVSEStatusTypes?                 EVSEStatusFilter,
                                         TimeSpan                         RequestTimeout,
                                         EVSEStatus                       Result,
                                         TimeSpan                         Duration);

    #endregion

    #region OnPullEVSEStatusById(Request|Response)Handler

    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByIdRequestDelegate (DateTime                LogTimestamp,
                                             DateTime                RequestTimestamp,
                                             CentralServer           Sender,
                                             String                  SenderId,
                                             EventTracking_Id        EventTrackingId,
                                             Provider_Id             ProviderId,
                                             IEnumerable<EVSE_Id>    EVSEIds,
                                             TimeSpan                RequestTimeout);


    /// <summary>
    /// Send a PullEVSEStatusById request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<EVSEStatusById>

        OnPullEVSEStatusByIdDelegate(DateTime                   Timestamp,
                                     CentralServer              Sender,
                                     PullEVSEStatusByIdRequest  Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByIdResponseDelegate(DateTime                Timestamp,
                                             CentralServer           Sender,
                                             String                  SenderId,
                                             EventTracking_Id        EventTrackingId,
                                             Provider_Id             ProviderId,
                                             IEnumerable<EVSE_Id>    EVSEIds,
                                             TimeSpan                RequestTimeout,
                                             EVSEStatusById          Result,
                                             TimeSpan                Duration);

    #endregion



    // CPO event delegates...

    #region OnPushEvseData

    ///// <summary>
    ///// Add charge detail records.
    ///// </summary>
    ///// <param name="Timestamp">The timestamp of the request.</param>
    ///// <param name="Sender">The sender of the request.</param>
    ///// <param name="CancellationToken">A token to cancel this task.</param>
    ///// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    ///// 
    ///// <param name="CDRInfos">An enumeration of charge detail records.</param>
    ///// 
    ///// <param name="QueryTimeout">An optional timeout for this request.</param>
    //public delegate Task<PushEVSEDataResponse>

    //    OnPushEvseDataDelegate(DateTime               Timestamp,
    //                           CentralServer          Sender,
    //                           CancellationToken      CancellationToken,
    //                           EventTracking_Id       EventTrackingId,

    //                           //IEnumerable<CDRInfo>   CDRInfos,

    //                           TimeSpan?              QueryTimeout = null);

    #endregion



    // EMP event delegates...


}
