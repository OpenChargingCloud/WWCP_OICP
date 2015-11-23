/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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

using org.GraphDefined.WWCP.OICP_2_0;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_1_2.UnitTests
{

    public class OICPTests
    {

        public async Task TestPushEVSEData(CPOClient HubjectCPO)
        {

            var EVSEDataRecords = Enumeration.Create(

                new EVSEDataRecord(
                    EVSEId:               EVSE_Id.Parse("DE*GEF*E123456789*2"),
                    ChargingStationId:    ChargingStation_Id.Parse("DE*GEF*S123456789").ToString(),
                    ChargingStationName:  I18NString.Create(Languages.de, "Testbox 1").
                                                        Add(Languages.en, "Testbox One"),

                    Address:              Address.Create(
                                              Country.Germany,
                                              "07749", I18NString.Create(Languages.de, "Jena"),
                                              "Biberweg", "18"
                                          ),

                    GeoCoordinate:        GeoCoordinate.Create(
                                              Latitude.Parse(49.731102),
                                              Longitude.Parse(10.142530)
                                          ),

                    Plugs:                Enumeration.Create(
                                              PlugTypes.TypeFSchuko,
                                              PlugTypes.Type2Outlet
                                          ),

                    AuthenticationModes:  Enumeration.Create(
                                              AuthenticationModes.NFC_RFID_Classic,
                                              AuthenticationModes.NFC_RFID_DESFire,
                                              AuthenticationModes.REMOTE
                                          ),

                    PaymentOptions:       Enumeration.Create(
                                              PaymentOptions.Contract,
                                              PaymentOptions.Direct
                                          ),

                    Accessibility:        AccessibilityTypes.Paying_publicly_accessible,

                    HotlinePhoneNumber:   "+4955512345678",

                    IsHubjectCompatible:  true,
                    DynamicInfoAvailable: true,
                    IsOpen24Hours:        true

                )

            );

            var req1 = HubjectCPO.

                          PushEVSEData(EVSEDataRecords,
                                       ActionType.insert,
                                       IncludeEVSEs: evse => evse.EVSEId.ToString().StartsWith("DE")).

                          ContinueWith(task =>
                          {

                              var Acknowledgement = task.Result.Content;

                              if (Acknowledgement.Result)
                                  Console.WriteLine("success!");

                              else
                              {
                                  Console.WriteLine(task.Result.Content.StatusCode.Code);
                                  Console.WriteLine(task.Result.Content.StatusCode.Description);
                                  Console.WriteLine(task.Result.Content.StatusCode.AdditionalInfo);
                              }

                          });

        }

        public async Task TestPullEVSEData(EMPClient HubjectEMP)
        {

            var req2 = HubjectEMP.

                          PullEVSEData(ProviderId:    EVSP_Id.Parse("DE*GDF"),
                                       SearchCenter:  new GeoCoordinate(Latitude. Parse(49.731102),
                                                                        Longitude.Parse(10.142533)),
                                       DistanceKM:    100,
                                       QueryTimeout:  TimeSpan.FromSeconds(120)).

                          ContinueWith(task =>
                          {

                              var eRoamingEVSEData = task.Result.Content;

                              if (eRoamingEVSEData.StatusCode.HasResult)
                              {

                                  Console.WriteLine(eRoamingEVSEData.
                                                        OperatorEVSEData.
                                                        Select(evsedata => "'" + evsedata.OperatorName +
                                                                           "' has " +
                                                                           evsedata.EVSEDataRecords.Count() +
                                                                           " EVSEs").
                                                        AggregateWith(Environment.NewLine) +
                                                        Environment.NewLine);

                              }
                              else
                              {
                                  Console.WriteLine(eRoamingEVSEData.StatusCode.Code);
                                  Console.WriteLine(eRoamingEVSEData.StatusCode.Description);
                                  Console.WriteLine(eRoamingEVSEData.StatusCode.AdditionalInfo);
                              }

                          });

        }

        public async Task TestPushEVSEStatus(CPOClient HubjectCPO)
        {

            var EVSEStatus = new Dictionary<EVSE_Id, OICPEVSEStatusType>();
            EVSEStatus.Add(EVSE_Id.Parse("DE*GEF*E123456789*1"), OICPEVSEStatusType.Available);
            EVSEStatus.Add(EVSE_Id.Parse("DE*GEF*E123456789*2"), OICPEVSEStatusType.Occupied);

            var req3 = HubjectCPO.

                          PushEVSEStatus(EVSEStatus,
                                         ActionType.insert,
                                         OperatorId: EVSEOperator_Id.Parse("DE*GEF"),
                                         OperatorName: "Test CPO 1",
                                         QueryTimeout: TimeSpan.FromSeconds(120)).

                          ContinueWith(task =>
                          {

                              var Acknowledgement = task.Result.Content;

                              if (Acknowledgement.Result)
                                  Console.WriteLine("success!");

                              else
                              {
                                  Console.WriteLine(Acknowledgement.StatusCode.Code);
                                  Console.WriteLine(Acknowledgement.StatusCode.Description);
                                  Console.WriteLine(Acknowledgement.StatusCode.AdditionalInfo);
                              }

                          });

        }

        public async Task TestPullEVSEStatus(EMPClient HubjectEMP)
        {

            var req4 = HubjectEMP.

                          PullEVSEStatus(ProviderId:        EVSP_Id.Parse("DE*GDF"),
                                         SearchCenter:      new GeoCoordinate(Latitude. Parse(49.731102),
                                                                              Longitude.Parse(10.142533)),
                                         DistanceKM:        100,
                                         EVSEStatusFilter:  OICPEVSEStatusType.Available,
                                         QueryTimeout:      TimeSpan.FromSeconds(120)).

                          ContinueWith(task =>
                          {

                              var eRoamingEVSEStatus = task.Result.Content;

                              if (eRoamingEVSEStatus.StatusCode.HasResult)
                              {

                                  Console.WriteLine(eRoamingEVSEStatus.
                                                        OperatorEVSEStatus.
                                                        Select(evsestatusrecord => "'" + evsestatusrecord.OperatorName +
                                                                                   "' has " +
                                                                                   evsestatusrecord.EVSEStatusRecords.Count() +
                                                                                   " available EVSEs").
                                                        AggregateWith(Environment.NewLine) +
                                                        Environment.NewLine);

                              }
                              else
                              {
                                  Console.WriteLine(eRoamingEVSEStatus.StatusCode.Code);
                                  Console.WriteLine(eRoamingEVSEStatus.StatusCode.Description);
                                  Console.WriteLine(eRoamingEVSEStatus.StatusCode.AdditionalInfo);
                              }

                          });

        }

        public async Task TestPullEVSEStatusById(EMPClient HubjectEMP)
        {

            var req5 = HubjectEMP.
                PullEVSEStatusById(ProviderId:    EVSP_Id.Parse("DE*GDF"),
                                   EVSEIds:       Enumeration.Create(EVSE_Id.Parse("DE*GEF*E123456789*1"),
                                                                     EVSE_Id.Parse("+49*822*083431571*1")),
                                   QueryTimeout:  TimeSpan.FromSeconds(120)).

                ContinueWith(task =>
                {

                    var eRoamingEVSEStatusById = task.Result.Content;

                    if (eRoamingEVSEStatusById.StatusCode.HasResult)
                    {

                        Console.WriteLine(eRoamingEVSEStatusById.
                                              EVSEStatusRecords.
                                              Select(evsestatusrecord => "EVSE '" + evsestatusrecord.Id +
                                                                         "' has status '" +
                                                                         evsestatusrecord.Status.ToString() +
                                                                         "'").
                                              AggregateWith(Environment.NewLine) +
                                              Environment.NewLine);

                    }
                    else
                    {
                        Console.WriteLine(eRoamingEVSEStatusById.StatusCode.Code);
                        Console.WriteLine(eRoamingEVSEStatusById.StatusCode.Description);
                        Console.WriteLine(eRoamingEVSEStatusById.StatusCode.AdditionalInfo);
                    }

                });

        }

        public async Task TestSearchEVSE(EMPClient HubjectEMP)
        {

            var req = HubjectEMP.

                SearchEVSE(EVSP_Id.Parse("DE*GDF"),
                           SearchCenter:  new GeoCoordinate(Latitude. Parse(49.731102),
                                                            Longitude.Parse(10.142533)),
                           DistanceKM:    100,
                           Plug:          PlugTypes.Type2Outlet,
                           QueryTimeout:  TimeSpan.FromSeconds(120)).

                ContinueWith(task =>
                {

                    var eRoamingEvseSearchResult = task.Result.Content;

                    if (eRoamingEvseSearchResult.HasResults())
                        Console.WriteLine(eRoamingEvseSearchResult.
                                              EVSEMatches.
                                              Select(match => "'" + match.EVSEDataRecord.ChargingStationName.FirstText + " / " + match.EVSEDataRecord.EVSEId.ToString() +
                                                                 "' in " +
                                                                 match.Distance +
                                                                 " km").
                                              AggregateWith(Environment.NewLine) +
                                              Environment.NewLine);

                });

        }


        public async Task TestPushAuthenticationData(EMPClient HubjectEMP)
        {

            var req = HubjectEMP.

                PushAuthenticationData(Enumeration.Create(  // ([A-Za-z]{2} \-? [A-Za-z0-9]{3} \-? C[A-Za-z0-9]{8}[\*|\-]?[\d|X])

                                           AuthorizationIdentification.FromAuthToken
                                               (Auth_Token.Parse("08152305")),

                                           AuthorizationIdentification.FromQRCodeIdentification
                                               (eMA_Id.Parse("DE-GDF-C123ABC56-X"),
                                                "1234") //DE**GDF*CAETE4*3"), "1234") //
                                       ),
                                       ProviderId: EVSP_Id.Parse("DE*GDF"),
                                       OICPAction: ActionType.fullLoad,
                                       QueryTimeout: TimeSpan.FromSeconds(120)).

                ContinueWith(task =>
                {

                    var Acknowledgement = task.Result.Content;

                    if (Acknowledgement.Result)
                        Console.WriteLine("success!");

                    else
                    {
                        Console.WriteLine(Acknowledgement.StatusCode.Code);
                        Console.WriteLine(Acknowledgement.StatusCode.Description);
                        Console.WriteLine(Acknowledgement.StatusCode.AdditionalInfo);
                    }

                });

        }

        public async Task TestPullAuthenticationData(CPOClient HubjectCPO)
        {

            var req = HubjectCPO.
                PullAuthenticationData(EVSEOperator_Id.Parse("DE*GEF"),
                                       QueryTimeout:  TimeSpan.FromSeconds(120)).

                ContinueWith(task =>
                {

                    var AuthenticationDataResult = task.Result.Content;

                    if (AuthenticationDataResult.StatusCode.HasResult)
                        Console.WriteLine(AuthenticationDataResult.
                                              ProviderAuthenticationDataRecords.
                                              Select(authdata => "'" + authdata.ProviderId.ToString() +
                                                                 "' has " +
                                                                 authdata.AuthorizationIdentifications.Count() +
                                                                 " credentials").
                                              AggregateWith(Environment.NewLine) +
                                              Environment.NewLine);

                });

        }


        public async Task TestAuth(CPOClient   HubjectCPO,
                                   Auth_Token  AuthToken)
        {

            var AuthStartResult = await HubjectCPO.AuthorizeStart(EVSEOperator_Id.Parse("DE*GEF"),
                                                                  AuthToken,
                                                                  EVSE_Id.        Parse("DE*GEF*E123456789*1"));

            ConsoleX.WriteLines("AuthStart result:",
                                AuthStartResult.Content.AuthorizationStatus,
                                AuthStartResult.Content.StatusCode.Code,
                                AuthStartResult.Content.StatusCode.Description,
                                AuthStartResult.Content.StatusCode.AdditionalInfo);

            await Task.Delay(1000);


            var AuthStopResult = await HubjectCPO.
                AuthorizeStop(EVSEOperator_Id.Parse("DE*GEF"),
                              AuthStartResult.Content.SessionId,
                              AuthToken,
                              EVSE_Id.        Parse("DE*GEF*E123456789*1"));

            ConsoleX.WriteLines("AuthStop result:",
                                AuthStopResult.Content.AuthorizationStatus,
                                AuthStopResult.Content.StatusCode.Code,
                                AuthStopResult.Content.StatusCode.Description,
                                AuthStopResult.Content.StatusCode.AdditionalInfo);

            await Task.Delay(1000);


            var SendCDRResult = await HubjectCPO.
                SendChargeDetailRecord(EVSEId:                EVSE_Id.Parse("DE*GEF*E123456789*1"),
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
                                       ConsumedEnergy:        111.111,
                                       QueryTimeout:          TimeSpan.FromSeconds(120));

            ConsoleX.WriteLines("SendCDR result:",
                                SendCDRResult.Content.Result,
                                SendCDRResult.Content.StatusCode.Code,
                                SendCDRResult.Content.StatusCode.Description,
                                SendCDRResult.Content.StatusCode.AdditionalInfo);

        }

        public async Task TestMobileAuth(MobileClient  HubjectMobile,
                                         eMAIdWithPIN  eMAIdWithPin)
        {

            var req = HubjectMobile.

                MobileAuthorizeStart(EVSE_Id.Parse("+49*822*285808576*1"),
                                     eMAIdWithPin,
                                     QueryTimeout: TimeSpan.FromSeconds(120)).

                ContinueWith(task =>
                {

                    var MobileAuthorizationStart = task.Result.Content;

                    if (MobileAuthorizationStart.AuthorizationStatus == AuthorizationStatusType.Authorized)
                    {

                        Console.WriteLine("Ready to charge at charging station: " +
                                          MobileAuthorizationStart.ChargingStationName[Languages.de] +
                                          Environment.NewLine +
                                          MobileAuthorizationStart.AdditionalInfo[Languages.de]);

                        return MobileAuthorizationStart.SessionId;

                    }

                    else
                    {
                        Console.WriteLine(MobileAuthorizationStart.StatusCode.Code);
                        Console.WriteLine(MobileAuthorizationStart.StatusCode.Description);
                        Console.WriteLine(MobileAuthorizationStart.StatusCode.AdditionalInfo);
                        return null;
                    }

                }).

                ContinueWith(task =>
                {

                    var SessionId = task.Result;
                    if (SessionId == null)
                        return;

                    Thread.Sleep(1000);

                    HubjectMobile.
                        MobileRemoteStart(SessionId,
                                          QueryTimeout: TimeSpan.FromSeconds(240)).

                        ContinueWith(task2 =>
                        {

                            var Acknowledgement = task2.Result.Content;

                            if (Acknowledgement.Result)
                            {
                                Console.WriteLine("Charging session started!");
                                return SessionId;
                            }

                            else
                            {
                                Console.WriteLine(Acknowledgement.StatusCode.Code);
                                Console.WriteLine(Acknowledgement.StatusCode.Description);
                                Console.WriteLine(Acknowledgement.StatusCode.AdditionalInfo);
                                return null;
                            }

                        }).

                        ContinueWith(task3 =>
                        {

                            var SessionId2 = task3.Result;
                            if (SessionId2 == null)
                                return;

                            Thread.Sleep(2000);

                            HubjectMobile.
                                MobileRemoteStop(SessionId2,
                                                 QueryTimeout: TimeSpan.FromSeconds(120)).

                                ContinueWith(task2 =>
                                {

                                    var Acknowledgement = task2.Result.Content;

                                    if (Acknowledgement.Result)
                                        Console.WriteLine("Charging session stopped!");

                                    else
                                    {
                                        Console.WriteLine(Acknowledgement.StatusCode.Code);
                                        Console.WriteLine(Acknowledgement.StatusCode.Description);
                                        Console.WriteLine(Acknowledgement.StatusCode.AdditionalInfo);
                                    }

                                });

                        });

                });

        }

        public async Task TestGetChargeDetailRecords(EMPClient HubjectEMP)
        {

            var result = await HubjectEMP.
                GetChargeDetailRecords(EVSP_Id.Parse("DE*GDF"),
                                       new DateTime(2015, 10, 1),
                                       DateTime.Now);

            Console.WriteLine(result.Content.Count() + " charge detail records found!");

        }


    }

}
