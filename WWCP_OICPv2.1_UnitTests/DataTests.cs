/*
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.WWCP.OICPv2_1.EMP;
using org.GraphDefined.WWCP.OICPv2_1.CPO;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.UnitTests
{

    public class DataTests
    {

        public async Task TestPushEVSEData(CPOClient HubjectCPO)
        {

            var RN = new RoamingNetwork(RoamingNetwork_Id.Parse("TEST"));

            var EVSEDataRecords = Enumeration.Create(

                new EVSEDataRecord(
                    EVSE_Id.Parse("DE*GEF*E123456789*2"),
                    DeltaTypes.insert,
                    DateTime.Now,
                    //EVSEOperator:         RN.CreateNewEVSEOperator(EVSEOperator_Id.Parse("TEST"), I18NString.Create(Languages.de, "TEST")),
                    ChargingStationId:    ChargingStation_Id.Parse("DE*GEF*S123456789").ToString(),
                    ChargingStationName:  I18NString.Create(Languages.deu, "Testbox 1").
                                                        Add(Languages.eng, "Testbox One"),

                    Address:              Address.Create(
                                              Country.Germany,
                                              "07749", I18NString.Create(Languages.deu, "Jena"),
                                              "Biberweg", "18"
                                          ),

                    GeoCoordinate:        GeoCoordinate.Create(
                                              Latitude. Parse(49.731102),
                                              Longitude.Parse(10.142530)
                                          ),

                    Plugs:                PlugTypes.TypeFSchuko|
                                          PlugTypes.Type2Outlet,

                    AuthenticationModes:  AuthenticationModes.NFC_RFID_Classic|
                                          AuthenticationModes.NFC_RFID_DESFire|
                                          AuthenticationModes.REMOTE,

                    PaymentOptions:       PaymentOptions.Contract|
                                          PaymentOptions.Direct,

                    Accessibility:        AccessibilityTypes.Paying_publicly_accessible,

                    HotlinePhoneNumber:   "+4955512345678",

                    IsHubjectCompatible:  true,
                    DynamicInfoAvailable: true,
                    IsOpen24Hours:        true

                )

            );

            var req1 = HubjectCPO.

                          PushEVSEData(EVSEDataRecords,
                                       Operator_Id.Parse("DE*GEF"),
                                       null,
                                       ActionTypes.insert,
                                       IncludeEVSEDataRecords: evse => evse.Id.ToString().StartsWith("DE")).

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

        public async Task TestPullEVSEData(EMP.EMPClient HubjectEMP)
        {

            var req2 = HubjectEMP.

                          PullEVSEData(ProviderId:      Provider_Id.Parse("DE*GDF"),
                                       SearchCenter:    new GeoCoordinate(Latitude. Parse(49.731102),
                                                                          Longitude.Parse(10.142533)),
                                       DistanceKM:      100,
                                       RequestTimeout:  TimeSpan.FromSeconds(120)).

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

            var EVSEStatus = new List<EVSEStatusRecord>();
            EVSEStatus.Add(new EVSEStatusRecord(EVSE_Id.Parse("DE*GEF*E123456789*1"), EVSEStatusTypes.Available));
            EVSEStatus.Add(new EVSEStatusRecord(EVSE_Id.Parse("DE*GEF*E123456789*2"), EVSEStatusTypes.Occupied));

            var req3 = HubjectCPO.

                          PushEVSEStatus(EVSEStatus,
                                         Operator_Id.Parse("DE*GEF"),
                                         "GraphDefined",
                                         ActionTypes.insert,
                                         RequestTimeout:  TimeSpan.FromSeconds(120)).

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

        public async Task TestPullEVSEStatus(EMP.EMPClient HubjectEMP)
        {

            var req4 = HubjectEMP.

                          PullEVSEStatus(ProviderId:        Provider_Id.Parse("DE*GDF"),
                                         SearchCenter:      new GeoCoordinate(Latitude. Parse(49.731102),
                                                                              Longitude.Parse(10.142533)),
                                         DistanceKM:        100,
                                         EVSEStatusFilter:  EVSEStatusTypes.Available,
                                         RequestTimeout:    TimeSpan.FromSeconds(120)).

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

        public async Task TestPullEVSEStatusById(EMP.EMPClient HubjectEMP)
        {

            var req5 = HubjectEMP.
                PullEVSEStatusById(ProviderId:      Provider_Id.Parse("DE*GDF"),
                                   EVSEIds:         Enumeration.Create(EVSE_Id.Parse("DE*GEF*E123456789*1"),
                                                                       EVSE_Id.Parse("+49*822*083431571*1")),
                                   RequestTimeout:  TimeSpan.FromSeconds(120)).

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

        public async Task TestSearchEVSE(EMP.EMPClient HubjectEMP)
        {

            Task.Factory.StartNew(async () => {

                var result = await HubjectEMP.
                    SearchEVSE(Provider_Id.Parse("DE*GDF"),
                               SearchCenter:    new GeoCoordinate(Latitude. Parse(49.731102),
                                                                  Longitude.Parse(10.142533)),
                               DistanceKM:      100,
                               Plug:            PlugTypes.Type2Outlet,
                               RequestTimeout:  TimeSpan.FromSeconds(120));

                if (result.Content.HasResults())
                    Console.WriteLine(result.Content.
                                          EVSEMatches.
                                          Select(match => "'" + match.EVSEDataRecord.ChargingStationName.FirstText  + " / " +
                                                                match.EVSEDataRecord.Id.             ToString() + "' in " +
                                                                match.Distance + " km").
                                          AggregateWith(Environment.NewLine) +
                                          Environment.NewLine);

            }).

            // Wait for the task to complete...
            Wait();

        }


        public void TestPushAuthenticationData(EMPClient HubjectEMP)
        {

            Task.Factory.StartNew(async () => {

                var result = await HubjectEMP.
                    PushAuthenticationData(Enumeration.Create(  // ([A-Za-z]{2} \-? [A-Za-z0-9]{3} \-? C[A-Za-z0-9]{8}[\*|\-]?[\d|X])

                                               AuthorizationIdentification.FromRFIDId(UID.Parse("08152305")),

                                               AuthorizationIdentification.FromQRCodeIdentification
                                                   (EVCO_Id.Parse("DE-GDF-C123ABC56-X"),
                                                    "1234") //DE**GDF*CAETE4*3"), "1234") //

                                           ),
                                           ProviderId:     Provider_Id.Parse("DE*GDF"),
                                           OICPAction:     ActionTypes.fullLoad,
                                           RequestTimeout: TimeSpan.  FromSeconds(120));


                if (result.Content.Result)
                    Console.WriteLine("success!");
                else
                {
                    ConsoleX.WriteLines("PushAuthenticationData result:",
                                        result.Content.StatusCode.Code,
                                        result.Content.StatusCode.Description,
                                        result.Content.StatusCode.AdditionalInfo);
                }

            }).

            // Wait for the task to complete...
            Wait();

        }

        public async Task TestPullAuthenticationData(CPOClient HubjectCPO)
        {

            Task.Factory.StartNew(async () => {

                var result = await HubjectCPO.
                    PullAuthenticationData(Operator_Id.Parse("DE*GEF"),
                                           RequestTimeout: TimeSpan.FromSeconds(120));


                if (result.Content.StatusCode.HasResult)
                    Console.WriteLine(result.Content.
                                          ProviderAuthenticationDataRecords.
                                          Select(authdata => "Provider '" + authdata.ProviderId.ToString() +
                                                             "' has " +
                                                             authdata.AuthorizationIdentifications.Count() +
                                                             " credentials").
                                          AggregateWith(Environment.NewLine) +
                                          Environment.NewLine);

            }).

            // Wait for the task to complete...
            Wait();

        }

    }

}
