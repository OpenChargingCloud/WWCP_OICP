/*
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

/// <summary>
/// The common interface of all OICP Central clients.
/// </summary>
namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    public interface ICentralClient
    {

        #region Properties

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        TimeSpan?  RequestTimeout   { get; }

        #endregion


        Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>>
            AuthorizeRemoteReservationStart(EMP.AuthorizeRemoteReservationStartRequest Request);

        Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>>
            AuthorizeRemoteReservationStop (EMP.AuthorizeRemoteReservationStopRequest  Request);

        Task<HTTPResponse<Acknowledgement>> AuthorizeRemoteStart(Provider_Id ProviderId, EVSE_Id EVSEId, EVCO_Id EVCOId, Session_Id? SessionId = default(Session_Id?), PartnerProduct_Id? ChargingProductId = default(PartnerProduct_Id?), PartnerSession_Id? PartnerSessionId = default(PartnerSession_Id?), TimeSpan? RequestTimeout = default(TimeSpan?));
        Task<HTTPResponse<Acknowledgement>> AuthorizeRemoteStop(Session_Id SessionId, Provider_Id ProviderId, EVSE_Id EVSEId, PartnerSession_Id? PartnerSessionId = default(PartnerSession_Id?), TimeSpan? RequestTimeout = default(TimeSpan?));

    }

}