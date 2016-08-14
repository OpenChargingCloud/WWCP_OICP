/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0.UnitTests
{

    public class OICPTests
    {

        #region TestAuthStart(HubjectCPO, AuthToken)

        public async Task TestAuthStart(CPOClient   HubjectCPO,
                                        Auth_Token  AuthToken)
        {

            var result = await HubjectCPO.
                                   AuthorizeStart(ChargingStationOperator_Id.Parse("DE*GEF"),
                                                  AuthToken,
                                                  EVSE_Id.        Parse("DE*GEF*E123456789*1"));

            ConsoleX.WriteLines("AuthStart result:",
                                result.Content.AuthorizationStatus,
                                result.Content.StatusCode.Code,
                                result.Content.StatusCode.Description,
                                result.Content.StatusCode.AdditionalInfo);

        }

        #endregion

        #region TestAuthStop(HubjectCPO, SessionId, AuthToken)

        public async Task TestAuthStop(CPOClient           HubjectCPO,
                                       ChargingSession_Id  SessionId,
                                       Auth_Token          AuthToken)
        {

            var result = await HubjectCPO.
                                   AuthorizeStop(ChargingStationOperator_Id.Parse("DE*GEF"),
                                                 SessionId,
                                                 AuthToken,
                                                 EVSE_Id.        Parse("DE*GEF*E123456789*1"));

            ConsoleX.WriteLines("AuthStop result:",
                                result.Content.AuthorizationStatus,
                                result.Content.StatusCode.Code,
                                result.Content.StatusCode.Description,
                                result.Content.StatusCode.AdditionalInfo);

        }

        #endregion

        #region TestChargeDetailRecord(HubjectCPO)

        public async Task TestChargeDetailRecord(CPOClient HubjectCPO)
        {

            var EVSEOperatorId  = ChargingStationOperator_Id.Parse("DE*GEF");
            var EVSEId          = EVSE_Id.        Parse("DE*GEF*E123456789*1");
            var AuthToken       = Auth_Token.     Parse("08152305");


            var AuthStartResult = await HubjectCPO.
                                            AuthorizeStart(EVSEOperatorId,
                                                           AuthToken,
                                                           EVSEId);

            ConsoleX.WriteLines("AuthStart result:",
                                AuthStartResult.Content.AuthorizationStatus,
                                AuthStartResult.Content.StatusCode.Code,
                                AuthStartResult.Content.StatusCode.Description);

            await Task.Delay(1000);


            var AuthStopResult = await HubjectCPO.
                                           AuthorizeStop(EVSEOperatorId,
                                                         AuthStartResult.Content.SessionId,
                                                         AuthToken,
                                                         EVSEId);

            ConsoleX.WriteLines("AuthStop result:",
                                AuthStopResult.Content.AuthorizationStatus,
                                AuthStopResult.Content.StatusCode.Code,
                                AuthStopResult.Content.StatusCode.Description,
                                AuthStopResult.Content.StatusCode.AdditionalInfo);

            await Task.Delay(1000);


            var SendCDRResult = await HubjectCPO.
                                          SendChargeDetailRecord(new ChargeDetailRecord(
                                                                     EVSEId:                EVSEId,
                                                                     SessionId:             AuthStartResult.Content.SessionId,
                                                                     PartnerProductId:      ChargingProduct_Id.Parse("AC1"),
                                                                     SessionStart:          DateTime.Now,
                                                                     SessionEnd:            DateTime.Now - TimeSpan.FromHours(3),
                                                                     Identification:        AuthorizationIdentification.FromAuthToken(AuthToken),
                                                                     PartnerSessionId:      ChargingSession_Id.Parse("0815"),
                                                                     ChargingStart:         DateTime.Now,
                                                                     ChargingEnd:           DateTime.Now - TimeSpan.FromHours(3),
                                                                     MeterValueStart:       123.456,
                                                                     MeterValueEnd:         234.567,
                                                                     MeterValuesInBetween:  Enumeration.Create(123.456, 189.768, 223.312, 234.560, 234.567),
                                                                     ConsumedEnergy:        111.111),

                                                                 RequestTimeout:          TimeSpan.FromSeconds(120)
                                                                );

            ConsoleX.WriteLines("SendCDR result:",
                                SendCDRResult.Content.Result,
                                SendCDRResult.Content.StatusCode.Code,
                                SendCDRResult.Content.StatusCode.Description,
                                SendCDRResult.Content.StatusCode.AdditionalInfo);

        }

        #endregion

        #region TestGetChargeDetailRecords(HubjectEMP)

        public async Task TestGetChargeDetailRecords(EMPClient HubjectEMP)
        {

            var result = await HubjectEMP.
                                   GetChargeDetailRecords(EVSP_Id.Parse("DE*GDF"),
                                                          new DateTime(2015, 10,  1),
                                                          new DateTime(2015, 10, 31));

            Console.WriteLine(result.Content.Count() + " charge detail records found!");

        }

        #endregion

    }

}
