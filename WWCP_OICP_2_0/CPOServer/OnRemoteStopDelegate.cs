///*
// * Copyright (c) 2014-2015 GraphDefined GmbH
// * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Threading;
//using System.Threading.Tasks;

//using org.GraphDefined.WWCP.LocalService;

//#endregion

//namespace org.GraphDefined.WWCP.OICP_2_0
//{

//    /// <summary>
//    /// Initiate a remote stop of the given charging session at the given EVSE.
//    /// </summary>
//    /// <param name="CancellationToken">A token to cancel this task.</param>
//    /// <param name="Timestamp">The timestamp of the request.</param>
//    /// <param name="RoamingNetworkId">The unique identification for the roaming network.</param>
//    /// <param name="EVSEId">The unique identification of an EVSE.</param>
//    /// <param name="SessionId">The unique identification for this charging session.</param>
//    /// <param name="PartnerSessionId">The unique identification for this charging session on the partner side.</param>
//    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
//    /// <returns>A RemoteStopResult task.</returns>
//    public delegate Task<RemoteStopResult> OnRemoteStopDelegate(CancellationToken   CancellationToken,
//                                                                DateTime            Timestamp,
//                                                                RoamingNetwork_Id   RoamingNetworkId,
//                                                                EVSE_Id             EVSEId,
//                                                                ChargingSession_Id  SessionId,
//                                                                ChargingSession_Id  PartnerSessionId,
//                                                                EVSP_Id             ProviderId);

//}
