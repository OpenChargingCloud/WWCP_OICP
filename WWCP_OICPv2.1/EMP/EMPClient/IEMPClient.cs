﻿/*
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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// The common interface of all OICP EMP clients.
    /// </summary>
    public interface IEMPClient
    {

        #region Properties

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        TimeSpan?  RequestTimeout   { get; }

        #endregion



        Task<HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(PushAuthenticationDataRequest Request);



        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            ReservationStart(AuthorizeRemoteReservationStartRequest  Request);


        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            ReservationStop (AuthorizeRemoteReservationStopRequest   Request);



        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>>

            RemoteStart(AuthorizeRemoteStartRequest Request);


        Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>>

            RemoteStop(AuthorizeRemoteStopRequest Request);



        Task<HTTPResponse<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(GetChargeDetailRecordsRequest Request);

    }

}