/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using NUnit.Framework;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO Sending ChargingNotifications tests.
    /// </summary>
    [TestFixture]
    public class SendChargingNotificationsTests : ACPOClientAPITests
    {

        #region SendChargingEndNotification_Test1()

        [Test]
        public async Task SendChargingEndNotification_Test1()
        {

            if (cpoClientAPI is null)
            {
                Assert.Fail("cpoClientAPI must not be null!");
                return;
            }

            if (cpoClient is null)
            {
                Assert.Fail("cpoClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var request = new ChargingEndNotificationRequest(

                              SessionId:              Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:         Identification.FromUID(UID.Parse("11223344")),
                              EVSEId:                 EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ChargingStart:          DateTimeOffset.Parse("2022-08-09T10:20:25.229Z"),
                              ChargingEnd:            DateTimeOffset.Parse("2022-08-09T11:13:25.229Z"),

                              CPOPartnerSessionId:    CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:    EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              SessionStart:           DateTimeOffset.Parse("2022-08-09T10:18:25.229Z"),
                              SessionEnd:             DateTimeOffset.Parse("2022-08-09T11:18:25.229Z"),
                              ConsumedEnergy:         WattHour.ParseKWh(35),
                              MeterValueStart:        WattHour.ParseKWh( 3),
                              MeterValueEnd:          WattHour.ParseKWh(38),
                              MeterValuesInBetween:   [ WattHour.ParseKWh(4), WattHour.ParseKWh(5), WattHour.ParseKWh(6) ],
                              OperatorId:             Operator_Id.Parse("DE*GEF"),
                              PartnerProductId:       PartnerProduct_Id.AC1,
                              PenaltyTimeStart:       DateTimeOffset.Parse("2022-08-09T11:19:00.000Z"),

                              CustomData:             new JObject(
                                                          new JProperty("hello", "EndNotification world!")
                                                      )

                          );

            Assert.That(request,                                                                     Is.Not.Null);

            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Requests_OK,               Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Responses_OK,              Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Responses_Error,           Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Requests_OK,               Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Responses_OK,              Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Responses_Error,           Is.EqualTo(0));

            cpoClient.   OnChargingEndNotificationRequest  += (timestamp, cpoClient,    chargingEndNotificationRequest) => {

                Assert.That(chargingEndNotificationRequest.SessionId.     ToString(),                Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingEndNotificationRequest.Identification.ToString(),                Is.EqualTo("11223344"));
                Assert.That(chargingEndNotificationRequest.EVSEId.        ToString(),                Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingEndNotificationRequest.ChargingStart. ToISO8601(),               Is.EqualTo("2022-08-09T10:20:25.229Z"));
                Assert.That(chargingEndNotificationRequest.ChargingEnd.   ToISO8601(),               Is.EqualTo("2022-08-09T11:13:25.229Z"));

                Assert.That(chargingEndNotificationRequest.CPOPartnerSessionId.ToString(),           Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingEndNotificationRequest.EMPPartnerSessionId.ToString(),           Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingEndNotificationRequest.SessionStart?.      ToISO8601(),          Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(chargingEndNotificationRequest.SessionEnd?.        ToISO8601(),          Is.EqualTo("2022-08-09T11:18:25.229Z"));
                Assert.That(chargingEndNotificationRequest.ConsumedEnergy?.    kWh,                  Is.EqualTo(35));
                Assert.That(chargingEndNotificationRequest.MeterValueStart?.   kWh,                  Is.EqualTo(3));
                Assert.That(chargingEndNotificationRequest.MeterValueEnd?.     kWh,                  Is.EqualTo(38));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.Count(),            Is.EqualTo(3));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(0).kWh,   Is.EqualTo(4));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(1).kWh,   Is.EqualTo(5));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(2).kWh,   Is.EqualTo(6));
                Assert.That(chargingEndNotificationRequest.OperatorId.         ToString(),           Is.EqualTo("DE*GEF"));
                Assert.That(chargingEndNotificationRequest.PartnerProductId.   ToString(),           Is.EqualTo("AC1"));
                Assert.That(chargingEndNotificationRequest.PenaltyTimeStart?.  ToISO8601(),          Is.EqualTo("2022-08-09T11:19:00.000Z"));

                Assert.That(chargingEndNotificationRequest.CustomData?.Count,                        Is.EqualTo(1));
                Assert.That(chargingEndNotificationRequest.CustomData?["hello"]?.Value<String>(),    Is.EqualTo("EndNotification world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnChargingEndNotificationResponse += (timestamp, cpoClient,    chargingEndNotificationRequest, oicpResponse, runtime) => {

                var chargingEndNotificationResponse = oicpResponse.Response;

                Assert.That(chargingEndNotificationResponse,                                         Is.Not.Null);
                Assert.That(chargingEndNotificationResponse?.Result,                                 Is.True);
                Assert.That(chargingEndNotificationResponse?.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingEndNotificationResponse?.SessionId,                              Is.Null);
                Assert.That(chargingEndNotificationResponse?.CPOPartnerSessionId,                    Is.Null);
                Assert.That(chargingEndNotificationResponse?.EMPPartnerSessionId,                    Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingEndNotificationRequest  += (timestamp, cpoClientAPI, chargingEndNotificationRequest) => {

                Assert.That(chargingEndNotificationRequest.SessionId.     ToString(),                Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingEndNotificationRequest.Identification.ToString(),                Is.EqualTo("11223344"));
                Assert.That(chargingEndNotificationRequest.EVSEId.        ToString(),                Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingEndNotificationRequest.ChargingStart. ToISO8601(),               Is.EqualTo("2022-08-09T10:20:25.229Z"));
                Assert.That(chargingEndNotificationRequest.ChargingEnd.   ToISO8601(),               Is.EqualTo("2022-08-09T11:13:25.229Z"));

                Assert.That(chargingEndNotificationRequest.CPOPartnerSessionId.ToString(),           Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingEndNotificationRequest.EMPPartnerSessionId.ToString(),           Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingEndNotificationRequest.SessionStart?.      ToISO8601(),          Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(chargingEndNotificationRequest.SessionEnd?.        ToISO8601(),          Is.EqualTo("2022-08-09T11:18:25.229Z"));
                Assert.That(chargingEndNotificationRequest.ConsumedEnergy?.    kWh,                  Is.EqualTo(35));
                Assert.That(chargingEndNotificationRequest.MeterValueStart?.   kWh,                  Is.EqualTo(3));
                Assert.That(chargingEndNotificationRequest.MeterValueEnd?.     kWh,                  Is.EqualTo(38));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.Count(),            Is.EqualTo(3));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(0).kWh,   Is.EqualTo(4));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(1).kWh,   Is.EqualTo(5));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(2).kWh,   Is.EqualTo(6));
                Assert.That(chargingEndNotificationRequest.OperatorId.         ToString(),           Is.EqualTo("DE*GEF"));
                Assert.That(chargingEndNotificationRequest.PartnerProductId.   ToString(),           Is.EqualTo("AC1"));
                Assert.That(chargingEndNotificationRequest.PenaltyTimeStart?.  ToISO8601(),          Is.EqualTo("2022-08-09T11:19:00.000Z"));

                Assert.That(chargingEndNotificationRequest.CustomData?.Count,                        Is.EqualTo(1));
                Assert.That(chargingEndNotificationRequest.CustomData?["hello"]?.Value<String>(),    Is.EqualTo("EndNotification world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingEndNotificationResponse += (timestamp, cpoClientAPI, chargingEndNotificationRequest, oicpResponse, runtime) => {

                var chargingEndNotificationResponse = oicpResponse.Response;

                Assert.That(chargingEndNotificationResponse,                                         Is.Not.Null);
                Assert.That(chargingEndNotificationResponse?.Result,                                 Is.True);
                Assert.That(chargingEndNotificationResponse?.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingEndNotificationResponse?.SessionId,                              Is.Null);
                Assert.That(chargingEndNotificationResponse?.CPOPartnerSessionId,                    Is.Null);
                Assert.That(chargingEndNotificationResponse?.EMPPartnerSessionId,                    Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.SendChargingEndNotification(request);

            Assert.That(oicpResult,                                                                  Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                     Is.True);
            Assert.That(oicpResult.Response?.Result,                                                 Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Requests_OK,               Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Responses_OK,              Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingEndNotification.Responses_Error,           Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Requests_OK,               Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Responses_OK,              Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingEndNotification.Responses_Error,           Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                        Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                       Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                        Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                       Is.EqualTo(1));

        }

        #endregion

        #region SendChargingErrorNotification_Test1()

        [Test]
        public async Task SendChargingErrorNotification_Test1()
        {

            if (cpoClientAPI is null)
            {
                Assert.Fail("cpoClientAPI must not be null!");
                return;
            }

            if (cpoClient is null)
            {
                Assert.Fail("cpoClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var request = new ChargingErrorNotificationRequest(

                              SessionId:             Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:        Identification.FromUID(UID.Parse("11223344")),
                              OperatorId:            Operator_Id.Parse("DE*GEF"),
                              EVSEId:                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ErrorType:             ErrorClassTypes.CriticalError,

                              CPOPartnerSessionId:   CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:   EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              ErrorAdditionalInfo:   "Something wicked happened!",

                              CustomData:            new JObject(
                                                         new JProperty("hello", "ErrorNotification world!")
                                                     )

                          );

            Assert.That(request,                                                                      Is.Not.Null);

            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Requests_OK,              Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Responses_OK,             Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Responses_Error,          Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Requests_OK,              Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Responses_OK,             Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Responses_Error,          Is.EqualTo(0));

            cpoClient.   OnChargingErrorNotificationRequest  += (timestamp, cpoClient,    chargingErrorNotificationRequest) => {

                Assert.That(chargingErrorNotificationRequest.SessionId.     ToString(),               Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingErrorNotificationRequest.Identification.ToString(),               Is.EqualTo("11223344"));
                Assert.That(chargingErrorNotificationRequest.OperatorId.    ToString(),               Is.EqualTo("DE*GEF"));
                Assert.That(chargingErrorNotificationRequest.EVSEId.        ToString(),               Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingErrorNotificationRequest.ErrorType,                               Is.EqualTo(ErrorClassTypes.CriticalError));

                Assert.That(chargingErrorNotificationRequest.CPOPartnerSessionId.ToString(),          Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingErrorNotificationRequest.EMPPartnerSessionId.ToString(),          Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingErrorNotificationRequest.ErrorAdditionalInfo,                     Is.EqualTo("Something wicked happened!"));

                Assert.That(chargingErrorNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingErrorNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("ErrorNotification world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnChargingErrorNotificationResponse += (timestamp, cpoClient,    chargingErrorNotificationRequest, oicpResponse, runtime) => {

                var chargingErrorNotificationResponse = oicpResponse.Response;

                Assert.That(chargingErrorNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingErrorNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingErrorNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingErrorNotificationResponse?.SessionId,                             Is.Null);
                Assert.That(chargingErrorNotificationResponse?.CPOPartnerSessionId,                   Is.Null);
                Assert.That(chargingErrorNotificationResponse?.EMPPartnerSessionId,                   Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingErrorNotificationRequest  += (timestamp, cpoClientAPI, chargingErrorNotificationRequest) => {

                Assert.That(chargingErrorNotificationRequest.SessionId.     ToString(),               Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingErrorNotificationRequest.Identification.ToString(),               Is.EqualTo("11223344"));
                Assert.That(chargingErrorNotificationRequest.OperatorId.    ToString(),               Is.EqualTo("DE*GEF"));
                Assert.That(chargingErrorNotificationRequest.EVSEId.        ToString(),               Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingErrorNotificationRequest.ErrorType,                               Is.EqualTo(ErrorClassTypes.CriticalError));

                Assert.That(chargingErrorNotificationRequest.CPOPartnerSessionId.ToString(),          Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingErrorNotificationRequest.EMPPartnerSessionId.ToString(),          Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingErrorNotificationRequest.ErrorAdditionalInfo,                     Is.EqualTo("Something wicked happened!"));

                Assert.That(chargingErrorNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingErrorNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("ErrorNotification world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingErrorNotificationResponse += (timestamp, cpoClientAPI, chargingErrorNotificationRequest, oicpResponse, runtime) => {

                var chargingErrorNotificationResponse = oicpResponse.Response;

                Assert.That(chargingErrorNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingErrorNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingErrorNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingErrorNotificationResponse?.SessionId,                             Is.Null);
                Assert.That(chargingErrorNotificationResponse?.CPOPartnerSessionId,                   Is.Null);
                Assert.That(chargingErrorNotificationResponse?.EMPPartnerSessionId,                   Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.SendChargingErrorNotification(request);

            Assert.That(oicpResult,                                                                   Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                      Is.True);
            Assert.That(oicpResult.Response?.Result,                                                  Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                         Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Requests_OK,              Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Responses_OK,             Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingErrorNotification.Responses_Error,          Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Requests_OK,              Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Responses_OK,             Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingErrorNotification.Responses_Error,          Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                        Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                        Is.EqualTo(1));

        }

        #endregion

        #region SendChargingProgressNotification_Test1()

        [Test]
        public async Task SendChargingProgressNotification_Test1()
        {

            if (cpoClientAPI is null)
            {
                Assert.Fail("cpoClientAPI must not be null!");
                return;
            }

            if (cpoClient is null)
            {
                Assert.Fail("cpoClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var request = new ChargingProgressNotificationRequest(

                              SessionId:                Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:           Identification.FromUID(UID.Parse("11223344")),
                              EVSEId:                   EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ChargingStart:            DateTimeOffset.Parse("2022-08-09T10:20:25.229Z"),
                              EventOccurred:            DateTimeOffset.Parse("2022-08-09T10:21:13.451Z"),

                              CPOPartnerSessionId:      CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:      EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              ChargingDuration:         TimeSpan.FromSeconds(48),
                              SessionStart:             DateTimeOffset.Parse("2022-08-09T10:18:25.229Z"),
                              ConsumedEnergyProgress:   WattHour.ParseKWh(5),
                              MeterValueStart:          WattHour.ParseKWh(3),
                              MeterValuesInBetween:     [ WattHour.ParseKWh(4), WattHour.ParseKWh(5) ],
                              OperatorId:               Operator_Id.Parse("DE*GEF"),
                              PartnerProductId:         PartnerProduct_Id.AC1,

                              CustomData:               new JObject(
                                                            new JProperty("hello", "ProgressNotification world!")
                                                        )

                          );

            Assert.That(request,                                                                          Is.Not.Null);

            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Requests_OK,               Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Responses_OK,              Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Responses_Error,           Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Requests_OK,               Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Responses_OK,              Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Responses_Error,           Is.EqualTo(0));

            cpoClient.   OnChargingProgressNotificationRequest  += (timestamp, cpoClient,    chargingProgressNotificationRequest) => {

                Assert.That(chargingProgressNotificationRequest.SessionId.     ToString(),                Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingProgressNotificationRequest.Identification.ToString(),                Is.EqualTo("11223344"));
                Assert.That(chargingProgressNotificationRequest.EVSEId.        ToString(),                Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingProgressNotificationRequest.ChargingStart. ToISO8601(),               Is.EqualTo("2022-08-09T10:20:25.229Z"));
                Assert.That(chargingProgressNotificationRequest.EventOccurred. ToISO8601(),               Is.EqualTo("2022-08-09T10:21:13.451Z"));

                Assert.That(chargingProgressNotificationRequest.CPOPartnerSessionId.ToString(),           Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingProgressNotificationRequest.EMPPartnerSessionId.ToString(),           Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingProgressNotificationRequest.ChargingDuration,                         Is.EqualTo(TimeSpan.FromSeconds(48)));
                Assert.That(chargingProgressNotificationRequest.SessionStart?.      ToISO8601(),          Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(chargingProgressNotificationRequest.ConsumedEnergyProgress?.    kWh,          Is.EqualTo(5));
                Assert.That(chargingProgressNotificationRequest.MeterValueStart?.   kWh,                  Is.EqualTo(3));
                Assert.That(chargingProgressNotificationRequest.MeterValuesInBetween?.Count(),            Is.EqualTo(2));
                Assert.That(chargingProgressNotificationRequest.MeterValuesInBetween?.ElementAt(0).kWh,   Is.EqualTo(4));
                Assert.That(chargingProgressNotificationRequest.MeterValuesInBetween?.ElementAt(1).kWh,   Is.EqualTo(5));
                Assert.That(chargingProgressNotificationRequest.OperatorId.         ToString(),           Is.EqualTo("DE*GEF"));
                Assert.That(chargingProgressNotificationRequest.PartnerProductId.   ToString(),           Is.EqualTo("AC1"));

                Assert.That(chargingProgressNotificationRequest.CustomData?.Count,                        Is.EqualTo(1));
                Assert.That(chargingProgressNotificationRequest.CustomData?["hello"]?.Value<String>(),    Is.EqualTo("ProgressNotification world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnChargingProgressNotificationResponse += (timestamp, cpoClient,    chargingProgressNotificationRequest, oicpResponse, runtime) => {

                var chargingProgressNotificationResponse = oicpResponse.Response;

                Assert.That(chargingProgressNotificationResponse,                                         Is.Not.Null);
                Assert.That(chargingProgressNotificationResponse?.Result,                                 Is.True);
                Assert.That(chargingProgressNotificationResponse?.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingProgressNotificationResponse?.SessionId,                              Is.Null);
                Assert.That(chargingProgressNotificationResponse?.CPOPartnerSessionId,                    Is.Null);
                Assert.That(chargingProgressNotificationResponse?.EMPPartnerSessionId,                    Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingProgressNotificationRequest  += (timestamp, cpoClientAPI, chargingProgressNotificationRequest) => {

                Assert.That(chargingProgressNotificationRequest.SessionId.     ToString(),                Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingProgressNotificationRequest.Identification.ToString(),                Is.EqualTo("11223344"));
                Assert.That(chargingProgressNotificationRequest.EVSEId.        ToString(),                Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingProgressNotificationRequest.ChargingStart. ToISO8601(),               Is.EqualTo("2022-08-09T10:20:25.229Z"));
                Assert.That(chargingProgressNotificationRequest.EventOccurred. ToISO8601(),               Is.EqualTo("2022-08-09T10:21:13.451Z"));

                Assert.That(chargingProgressNotificationRequest.CPOPartnerSessionId.ToString(),           Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingProgressNotificationRequest.EMPPartnerSessionId.ToString(),           Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingProgressNotificationRequest.ChargingDuration,                         Is.EqualTo(TimeSpan.FromSeconds(48)));
                Assert.That(chargingProgressNotificationRequest.SessionStart?.      ToISO8601(),          Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(chargingProgressNotificationRequest.ConsumedEnergyProgress?.    kWh,          Is.EqualTo(5));
                Assert.That(chargingProgressNotificationRequest.MeterValueStart?.   kWh,                  Is.EqualTo(3));
                Assert.That(chargingProgressNotificationRequest.MeterValuesInBetween?.Count(),            Is.EqualTo(2));
                Assert.That(chargingProgressNotificationRequest.MeterValuesInBetween?.ElementAt(0).kWh,   Is.EqualTo(4));
                Assert.That(chargingProgressNotificationRequest.MeterValuesInBetween?.ElementAt(1).kWh,   Is.EqualTo(5));
                Assert.That(chargingProgressNotificationRequest.OperatorId.         ToString(),           Is.EqualTo("DE*GEF"));
                Assert.That(chargingProgressNotificationRequest.PartnerProductId.   ToString(),           Is.EqualTo("AC1"));

                Assert.That(chargingProgressNotificationRequest.CustomData?.Count,                        Is.EqualTo(1));
                Assert.That(chargingProgressNotificationRequest.CustomData?["hello"]?.Value<String>(),    Is.EqualTo("ProgressNotification world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingProgressNotificationResponse += (timestamp, cpoClientAPI, chargingProgressNotificationRequest, oicpResponse, runtime) => {

                var chargingProgressNotificationResponse = oicpResponse.Response;

                Assert.That(chargingProgressNotificationResponse,                                         Is.Not.Null);
                Assert.That(chargingProgressNotificationResponse?.Result,                                 Is.True);
                Assert.That(chargingProgressNotificationResponse?.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingProgressNotificationResponse?.SessionId,                              Is.Null);
                Assert.That(chargingProgressNotificationResponse?.CPOPartnerSessionId,                    Is.Null);
                Assert.That(chargingProgressNotificationResponse?.EMPPartnerSessionId,                    Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.SendChargingProgressNotification(request);

            Assert.That(oicpResult,                                                                       Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                          Is.True);
            Assert.That(oicpResult.Response?.Result,                                                      Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                             Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Requests_OK,               Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Responses_OK,              Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingProgressNotification.Responses_Error,           Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Requests_OK,               Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Requests_Error,            Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Responses_OK,              Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingProgressNotification.Responses_Error,           Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                             Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                            Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                             Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                            Is.EqualTo(1));

        }

        #endregion

        #region SendChargingStartNotification_Test1()

        [Test]
        public async Task SendChargingStartNotification_Test1()
        {

            if (cpoClientAPI is null)
            {
                Assert.Fail("cpoClientAPI must not be null!");
                return;
            }

            if (cpoClient is null)
            {
                Assert.Fail("cpoClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var request = new ChargingStartNotificationRequest(

                              SessionId:             Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                              Identification:        Identification.FromUID(UID.Parse("11223344")),
                              EVSEId:                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              ChargingStart:         DateTimeOffset.Parse("2022-08-09T10:20:25.229Z"),

                              CPOPartnerSessionId:   CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da"),
                              EMPPartnerSessionId:   EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                              SessionStart:          DateTimeOffset.Parse("2022-08-09T10:18:25.229Z"),
                              MeterValueStart:       WattHour.ParseKWh(3),
                              OperatorId:            Operator_Id.Parse("DE*GEF"),
                              PartnerProductId:      PartnerProduct_Id.AC1,

                              CustomData:            new JObject(
                                                         new JProperty("hello", "StartNotification world!")
                                                     )

                          );

            Assert.That(request,                                                                      Is.Not.Null);

            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Requests_OK,              Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Responses_OK,             Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Responses_Error,          Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Requests_OK,              Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Responses_OK,             Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Responses_Error,          Is.EqualTo(0));

            cpoClient.   OnChargingStartNotificationRequest  += (timestamp, cpoClient,    chargingStartNotificationRequest) => {

                Assert.That(chargingStartNotificationRequest.SessionId.     ToString(),               Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingStartNotificationRequest.Identification.ToString(),               Is.EqualTo("11223344"));
                Assert.That(chargingStartNotificationRequest.EVSEId.        ToString(),               Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingStartNotificationRequest.ChargingStart. ToISO8601(),              Is.EqualTo("2022-08-09T10:20:25.229Z"));

                Assert.That(chargingStartNotificationRequest.CPOPartnerSessionId.ToString(),          Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingStartNotificationRequest.EMPPartnerSessionId.ToString(),          Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingStartNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(chargingStartNotificationRequest.MeterValueStart?.   kWh,                 Is.EqualTo(3));
                Assert.That(chargingStartNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingStartNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));

                Assert.That(chargingStartNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingStartNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("StartNotification world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnChargingStartNotificationResponse += (timestamp, cpoClient,    chargingStartNotificationRequest, oicpResponse, runtime) => {

                var chargingStartNotificationResponse = oicpResponse.Response;

                Assert.That(chargingStartNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingStartNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingStartNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingStartNotificationResponse?.SessionId,                             Is.Null);
                Assert.That(chargingStartNotificationResponse?.CPOPartnerSessionId,                   Is.Null);
                Assert.That(chargingStartNotificationResponse?.EMPPartnerSessionId,                   Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingStartNotificationRequest  += (timestamp, cpoClientAPI, chargingStartNotificationRequest) => {

                Assert.That(chargingStartNotificationRequest.SessionId.     ToString(),               Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(chargingStartNotificationRequest.Identification.ToString(),               Is.EqualTo("11223344"));
                Assert.That(chargingStartNotificationRequest.EVSEId.        ToString(),               Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingStartNotificationRequest.ChargingStart. ToISO8601(),              Is.EqualTo("2022-08-09T10:20:25.229Z"));

                Assert.That(chargingStartNotificationRequest.CPOPartnerSessionId.ToString(),          Is.EqualTo("9b217a90-9924-4229-a217-3d67a4de00da"));
                Assert.That(chargingStartNotificationRequest.EMPPartnerSessionId.ToString(),          Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(chargingStartNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(chargingStartNotificationRequest.MeterValueStart?.   kWh,                 Is.EqualTo(3));
                Assert.That(chargingStartNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingStartNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));

                Assert.That(chargingStartNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingStartNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("StartNotification world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargingStartNotificationResponse += (timestamp, cpoClientAPI, chargingStartNotificationRequest, oicpResponse, runtime) => {

                var chargingStartNotificationResponse = oicpResponse.Response;

                Assert.That(chargingStartNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingStartNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingStartNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                Assert.That(chargingStartNotificationResponse?.SessionId,                             Is.Null);
                Assert.That(chargingStartNotificationResponse?.CPOPartnerSessionId,                   Is.Null);
                Assert.That(chargingStartNotificationResponse?.EMPPartnerSessionId,                   Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.SendChargingStartNotification(request);

            Assert.That(oicpResult,                                                                   Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                      Is.True);
            Assert.That(oicpResult.Response?.Result,                                                  Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                         Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Requests_OK,              Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Responses_OK,             Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargingStartNotification.Responses_Error,          Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Requests_OK,              Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Requests_Error,           Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Responses_OK,             Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargingStartNotification.Responses_Error,          Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                        Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                        Is.EqualTo(1));

        }

        #endregion

    }

}
