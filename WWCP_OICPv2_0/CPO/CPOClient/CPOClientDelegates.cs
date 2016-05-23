/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
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

#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnEVSEDataPushDelegate(DateTime                               Timestamp,
                                                CPOClient                              Sender,
                                                String                                 SenderId,
                                                ActionType                             ActionType,
                                                ILookup<EVSEOperator, EVSEDataRecord>  EVSEData,
                                                UInt32                                 NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnEVSEDataPushedDelegate(DateTime                               Timestamp,
                                                  CPOClient                              Sender,
                                                  String                                 SenderId,
                                                  ActionType                             ActionType,
                                                  ILookup<EVSEOperator, EVSEDataRecord>  EVSEData,
                                                  UInt32                                 NumberOfEVSEs,
                                                  eRoamingAcknowledgement                Result,
                                                  TimeSpan                               Duration);

    
    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate Task OnEVSEStatusPushDelegate(DateTime                       Timestamp,
                                                  CPOClient                      Sender,
                                                  String                         SenderId,
                                                  ActionType                     ActionType,
                                                  IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                                  UInt32                         NumberOfEVSEs);


    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate Task OnEVSEStatusPushedDelegate(DateTime                       Timestamp,
                                                    CPOClient                      Sender,
                                                    String                         SenderId,
                                                    ActionType                     ActionType,
                                                    IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                                                    UInt32                         NumberOfEVSEs,
                                                    eRoamingAcknowledgement        Result,
                                                    TimeSpan                       Duration);


    /// <summary>
    /// A delegate called whenever an authorize start request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartHandler(DateTime                       Timestamp,
                                                 CPOClient                      Sender,
                                                 String                         SenderId,
                                                 EVSEOperator_Id                OperatorId,
                                                 Auth_Token                     AuthToken,
                                                 EVSE_Id                        EVSEId,
                                                 ChargingSession_Id             SessionId,
                                                 ChargingProduct_Id             PartnerProductId,
                                                 ChargingSession_Id             PartnerSessionId,
                                                 TimeSpan?                      QueryTimeout);

    /// <summary>
    /// A delegate called whenever an authorize start request was sent.
    /// </summary>
    public delegate Task OnAuthorizeStartedHandler(DateTime                       Timestamp,
                                                   CPOClient                      Sender,
                                                   String                         SenderId,
                                                   EVSEOperator_Id                OperatorId,
                                                   Auth_Token                     AuthToken,
                                                   EVSE_Id                        EVSEId,
                                                   ChargingSession_Id             SessionId,
                                                   ChargingProduct_Id             PartnerProductId,
                                                   ChargingSession_Id             PartnerSessionId,
                                                   TimeSpan?                      QueryTimeout,
                                                   eRoamingAuthorizationStart     Result,
                                                   TimeSpan                       Duration);


    /// <summary>
    /// A delegate called whenever an authorize stop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopHandler(DateTime                       Timestamp,
                                                CPOClient                      Sender,
                                                String                         SenderId,
                                                EVSEOperator_Id                OperatorId,
                                                ChargingSession_Id             SessionId,
                                                Auth_Token                     AuthToken,
                                                EVSE_Id                        EVSEId,
                                                ChargingSession_Id             PartnerSessionId,
                                                TimeSpan?                      QueryTimeout);

    /// <summary>
    /// A delegate called whenever an authorize stop request was sent.
    /// </summary>
    public delegate Task OnAuthorizeStoppedHandler(DateTime                       Timestamp,
                                                   CPOClient                      Sender,
                                                   String                         SenderId,
                                                   EVSEOperator_Id                OperatorId,
                                                   ChargingSession_Id             SessionId,
                                                   Auth_Token                     AuthToken,
                                                   EVSE_Id                        EVSEId,
                                                   ChargingSession_Id             PartnerSessionId,
                                                   TimeSpan?                      QueryTimeout,
                                                   eRoamingAuthorizationStop      Result,
                                                   TimeSpan                       Duration);


    public delegate Task OnPullAuthenticationDataHandler(DateTime                       Timestamp,
                                                         CPOClient                      Sender,
                                                         String                         SenderId,
                                                         EVSEOperator_Id                OperatorId,
                                                         TimeSpan?                      QueryTimeout);


    public delegate Task OnAuthenticationDataPulledHandler(DateTime                       Timestamp,
                                                           CPOClient                      Sender,
                                                           String                         SenderId,
                                                           EVSEOperator_Id                OperatorId,
                                                           TimeSpan?                      QueryTimeout,
                                                           eRoamingAuthenticationData     Result,
                                                           TimeSpan                       Duration);


    
    public delegate Task OnSendChargeDetailRecordHandler(DateTime                       Timestamp,
                                                         CPOClient                      Sender,
                                                         String                         SenderId,
                                                         ChargeDetailRecord     ChargeDetailRecord,
                                                         TimeSpan?                      QueryTimeout);

    public delegate Task OnChargeDetailRecordSentHandler(DateTime                       Timestamp,
                                                         CPOClient                      Sender,
                                                         String                         SenderId,
                                                         ChargeDetailRecord     ChargeDetailRecord,
                                                         TimeSpan?                      QueryTimeout,
                                                         eRoamingAcknowledgement        Result,
                                                         TimeSpan                       Duration);




}
