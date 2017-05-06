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
using System.Linq;
using System.Threading.Tasks;

using org.GraphDefined.WWCP.OICPv2_1.EMP;
using org.GraphDefined.WWCP.OICPv2_1.CPO;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.UnitTests
{

    public class OICPTests
    {

        #region TestAuthStart(HubjectCPO, UID)

        public async Task TestAuthStart(CPOClient   HubjectCPO,
                                        UID         UID)
        {

            var result = await HubjectCPO.
                                   AuthorizeStart(Operator_Id.Parse("DE*GEF"),
                                                  UID,
                                                  EVSE_Id.    Parse("DE*GEF*E123456789*1"));

            ConsoleX.WriteLines("AuthStart result:",
                                result.Content.AuthorizationStatus,
                                result.Content.StatusCode.Code,
                                result.Content.StatusCode.Description,
                                result.Content.StatusCode.AdditionalInfo);

        }

        #endregion

        #region TestAuthStop(HubjectCPO, SessionId, UserId)

        public async Task TestAuthStop(CPOClient   HubjectCPO,
                                       Session_Id  SessionId,
                                       UID         UserId)
        {

            var result = await HubjectCPO.
                                   AuthorizeStop(Operator_Id.Parse("DE*GEF"),
                                                 SessionId,
                                                 UserId,
                                                 EVSE_Id.    Parse("DE*GEF*E123456789*1"));

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

            var OperatorId  = Operator_Id.Parse("DE*GEF");
            var EVSEId      = EVSE_Id.    Parse("DE*GEF*E123456789*1");
            var UserId      = UID.        Parse("08152305");


            var AuthStartResult = await HubjectCPO.
                                            AuthorizeStart(OperatorId,
                                                           UserId,
                                                           EVSEId);

            ConsoleX.WriteLines("AuthStart result:",
                                AuthStartResult.Content.AuthorizationStatus,
                                AuthStartResult.Content.StatusCode.Code,
                                AuthStartResult.Content.StatusCode.Description);

            await Task.Delay(1000);


            var AuthStopResult = await HubjectCPO.
                                           AuthorizeStop(OperatorId,
                                                         AuthStartResult.Content.SessionId.Value,
                                                         UserId,
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
                                                                     SessionId:             AuthStartResult.Content.SessionId.Value,
                                                                     PartnerProductId:      PartnerProduct_Id.Parse("AC1"),
                                                                     SessionStart:          DateTime.Now,
                                                                     SessionEnd:            DateTime.Now - TimeSpan.FromHours(3),
                                                                     Identification:        Identification.FromRFIDId(UserId),
                                                                     PartnerSessionId:      PartnerSession_Id.Parse("081508150815-0815-0815-0815-0815081508150815"),
                                                                     ChargingStart:         DateTime.Now,
                                                                     ChargingEnd:           DateTime.Now - TimeSpan.FromHours(3),
                                                                     MeterValueStart:       123.456f,
                                                                     MeterValueEnd:         234.567f,
                                                                     MeterValuesInBetween:  Enumeration.Create(123.456f, 189.768f, 223.312f, 234.560f, 234.567f),
                                                                     ConsumedEnergy:        111.111f),

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
                                   GetChargeDetailRecords(Provider_Id.Parse("DE*GDF"),
                                                          new DateTime(2015, 10,  1),
                                                          new DateTime(2015, 10, 31));

            Console.WriteLine(result.Content.ChargeDetailRecords.Count() + " charge detail records found!");

        }

        #endregion

    }

}
