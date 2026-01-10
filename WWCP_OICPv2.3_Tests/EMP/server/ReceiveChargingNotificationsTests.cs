/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.server
{

    /// <summary>
    /// EMP Receive ChargingNotifications tests.
    /// </summary>
    [TestFixture]
    public class ReceiveChargingNotificationsTests : AEMPTests
    {

        #region ReceiveChargingEndNotification_Test1()

        [Test]
        public async Task ReceiveChargingEndNotification_Test1()
        {

            if (empServerAPI is null)
            {
                Assert.Fail("empServerAPI must not be null!");
                return;
            }

            if (empServerAPIClient is null)
            {
                Assert.Fail("empServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.NewRandom();
            var cpoPartnerSessionId    = CPOPartnerSession_Id.NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();
            var chargingStart          = Timestamp.Now - TimeSpan.FromSeconds(5);
            var chargingEnd            = Timestamp.Now - TimeSpan.FromSeconds(1);
            var sessionStart           = Timestamp.Now - TimeSpan.FromSeconds(10);
            var sessionEnd             = Timestamp.Now;
            var penaltyTimeStart       = Timestamp.Now - TimeSpan.FromSeconds(3);
            var eventOccurred          = Timestamp.Now;

            var request                = new ChargingEndNotificationRequest(

                                             SessionId:             sessionId,
                                             Identification:        Identification.FromUID(UID.Parse("AABBCCDD")),
                                             EVSEId:                EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             ChargingStart:         chargingStart,
                                             ChargingEnd:           chargingEnd,
                                             CPOPartnerSessionId:   cpoPartnerSessionId,
                                             EMPPartnerSessionId:   empPartnerSessionId,
                                             SessionStart:          sessionStart,
                                             SessionEnd:            sessionEnd,
                                             ConsumedEnergy:        WattHour.ParseKWh(12),
                                             MeterValueStart:       WattHour.ParseKWh(23),
                                             MeterValueEnd:         WattHour.ParseKWh(42),
                                             MeterValuesInBetween:  [ WattHour.ParseKWh(23.5M), WattHour.ParseKWh(32.25M) ],
                                             OperatorId:            Operator_Id.Parse("DE*GEF"),
                                             PartnerProductId:      PartnerProduct_Id.AC1,
                                             PenaltyTimeStart:      penaltyTimeStart,
                                             CustomData:            new JObject(
                                                                        new JProperty("hello", "notification end world!")
                                                                    ),

                                             RequestTimeout:        TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                                    Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Requests_OK,        Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Responses_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Requests_OK,        Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Responses_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Responses_Error,    Is.EqualTo(0));

            empServerAPIClient.OnChargingEndNotificationRequest  += (timestamp, empServerAPIClient, chargingEndNotificationRequest) => {

                Assert.That(chargingEndNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingEndNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingEndNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingEndNotificationRequest.ChargingStart.      ToISO8601(),         Is.EqualTo(chargingStart.ToISO8601()));
                Assert.That(chargingEndNotificationRequest.ChargingEnd.        ToISO8601(),         Is.EqualTo(chargingEnd.  ToISO8601()));
                Assert.That(chargingEndNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingEndNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingEndNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo(sessionStart. ToISO8601()));
                Assert.That(chargingEndNotificationRequest.SessionEnd?.        ToISO8601(),         Is.EqualTo(sessionEnd.   ToISO8601()));
                Assert.That(chargingEndNotificationRequest.ConsumedEnergy?. kWh,                    Is.EqualTo(12M));
                Assert.That(chargingEndNotificationRequest.MeterValueStart?.kWh,                    Is.EqualTo(23M));
                Assert.That(chargingEndNotificationRequest.MeterValueEnd?.  kWh,                    Is.EqualTo(42M));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.Count(),           Is.EqualTo(2));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(0).kWh,  Is.EqualTo(23.5M));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(1).kWh,  Is.EqualTo(32.25M));
                Assert.That(chargingEndNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingEndNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));
                Assert.That(chargingEndNotificationRequest.PenaltyTimeStart?.  ToISO8601(),         Is.EqualTo(penaltyTimeStart.ToISO8601()));
                Assert.That(chargingEndNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingEndNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification end world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnChargingEndNotificationResponse += (timestamp, empServerAPIClient, chargingEndNotificationRequest, oicpResponse, runtime) => {

                var chargingEndNotificationResponse = oicpResponse.Response;

                Assert.That(chargingEndNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingEndNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingEndNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingEndNotificationRequest  += (timestamp, empServerAPI,       chargingEndNotificationRequest) => {

                Assert.That(chargingEndNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingEndNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingEndNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingEndNotificationRequest.ChargingStart.      ToISO8601(),         Is.EqualTo(chargingStart.ToISO8601()));
                Assert.That(chargingEndNotificationRequest.ChargingEnd.        ToISO8601(),         Is.EqualTo(chargingEnd.  ToISO8601()));
                Assert.That(chargingEndNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingEndNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingEndNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo(sessionStart. ToISO8601()));
                Assert.That(chargingEndNotificationRequest.SessionEnd?.        ToISO8601(),         Is.EqualTo(sessionEnd.   ToISO8601()));
                Assert.That(chargingEndNotificationRequest.ConsumedEnergy?. kWh,                    Is.EqualTo(12M));
                Assert.That(chargingEndNotificationRequest.MeterValueStart?.kWh,                    Is.EqualTo(23M));
                Assert.That(chargingEndNotificationRequest.MeterValueEnd?.  kWh,                    Is.EqualTo(42M));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.Count(),           Is.EqualTo(2));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(0).kWh,  Is.EqualTo(23.5M));
                Assert.That(chargingEndNotificationRequest.MeterValuesInBetween?.ElementAt(1).kWh,  Is.EqualTo(32.25M));
                Assert.That(chargingEndNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingEndNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));
                Assert.That(chargingEndNotificationRequest.PenaltyTimeStart?.  ToISO8601(),         Is.EqualTo(penaltyTimeStart.ToISO8601()));
                Assert.That(chargingEndNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingEndNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification end world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingEndNotificationResponse += (timestamp, empServerAPI,       chargingEndNotificationRequest, chargingEndNotificationResponse, runtime) => {

                Assert.That(chargingEndNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingEndNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.SendChargingEndNotification(request);

            Assert.That(oicpResult,                                                                 Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                    Is.True);
            Assert.That(oicpResult.Response?.Result,                                                Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                       Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Requests_OK,        Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Responses_OK,       Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingEndNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Requests_OK,        Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Responses_OK,       Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingEndNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                       Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                      Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                       Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                      Is.EqualTo(1));

        }

        #endregion

        #region ReceiveChargingErrorNotification_Test1()

        [Test]
        public async Task ReceiveChargingErrorNotification_Test1()
        {

            if (empServerAPI is null)
            {
                Assert.Fail("empServerAPI must not be null!");
                return;
            }

            if (empServerAPIClient is null)
            {
                Assert.Fail("empServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.NewRandom();
            var cpoPartnerSessionId    = CPOPartnerSession_Id.NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();

            var request                = new ChargingErrorNotificationRequest(

                                             SessionId:            sessionId,
                                             Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             ErrorType:            ErrorClassTypes.CriticalError,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  empPartnerSessionId,
                                             ErrorAdditionalInfo:  "No space left of device!",
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "notification error world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                                     Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Responses_Error,   Is.EqualTo(0));

            empServerAPIClient.OnChargingErrorNotificationRequest  += (timestamp, empServerAPIClient, chargingErrorNotificationRequest) => {

                Assert.That(chargingErrorNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingErrorNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingErrorNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingErrorNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingErrorNotificationRequest.ErrorType,                               Is.EqualTo(ErrorClassTypes.CriticalError));
                Assert.That(chargingErrorNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingErrorNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingErrorNotificationRequest.ErrorAdditionalInfo,                     Is.EqualTo("No space left of device!"));
                Assert.That(chargingErrorNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingErrorNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification error world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnChargingErrorNotificationResponse += (timestamp, empServerAPIClient, chargingErrorNotificationRequest, oicpResponse, runtime) => {

                var chargingErrorNotificationResponse = oicpResponse.Response;

                Assert.That(chargingErrorNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingErrorNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingErrorNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingErrorNotificationRequest  += (timestamp, empServerAPI,       chargingErrorNotificationRequest) => {

                Assert.That(chargingErrorNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingErrorNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingErrorNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingErrorNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingErrorNotificationRequest.ErrorType,                               Is.EqualTo(ErrorClassTypes.CriticalError));
                Assert.That(chargingErrorNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingErrorNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingErrorNotificationRequest.ErrorAdditionalInfo,                     Is.EqualTo("No space left of device!"));
                Assert.That(chargingErrorNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingErrorNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification error world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingErrorNotificationResponse += (timestamp, empServerAPI,       chargingErrorNotificationRequest, chargingErrorNotificationResponse, runtime) => {

                Assert.That(chargingErrorNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingErrorNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.SendChargingErrorNotification(request);

            Assert.That(oicpResult,                                                                  Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                     Is.True);
            Assert.That(oicpResult.Response?.Result,                                                 Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                        Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Requests_OK,       Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Responses_OK,      Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingErrorNotification.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Requests_OK,       Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Responses_OK,      Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingErrorNotification.Responses_Error,   Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                        Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                       Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                        Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                       Is.EqualTo(1));

        }

        #endregion

        #region ReceiveChargingProgressNotification_Test1()

        [Test]
        public async Task ReceiveChargingProgressNotification_Test1()
        {

            if (empServerAPI is null)
            {
                Assert.Fail("empServerAPI must not be null!");
                return;
            }

            if (empServerAPIClient is null)
            {
                Assert.Fail("empServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.NewRandom();
            var cpoPartnerSessionId    = CPOPartnerSession_Id.NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();
            var chargingStart          = Timestamp.Now - TimeSpan.FromSeconds(5);
            var sessionStart           = Timestamp.Now - TimeSpan.FromSeconds(10);
            var eventOccurred          = Timestamp.Now;

            var request                = new ChargingProgressNotificationRequest(

                                             SessionId:               sessionId,
                                             Identification:          Identification.FromUID(UID.Parse("AABBCCDD")),
                                             EVSEId:                  EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             ChargingStart:           chargingStart,
                                             EventOccurred:           eventOccurred,
                                             CPOPartnerSessionId:     cpoPartnerSessionId,
                                             EMPPartnerSessionId:     empPartnerSessionId,
                                             ChargingDuration:        TimeSpan.FromSeconds(5),
                                             SessionStart:            sessionStart,
                                             ConsumedEnergyProgress:  WattHour.ParseKWh(12),
                                             MeterValueStart:         WattHour.ParseKWh(23),
                                             MeterValuesInBetween:    [ WattHour.ParseKWh(23.5M), WattHour.ParseKWh(24.25M) ],
                                             OperatorId:              Operator_Id.Parse("DE*GEF"),
                                             PartnerProductId:        PartnerProduct_Id.AC1,
                                             CustomData:              new JObject(
                                                                          new JProperty("hello", "notification progress world!")
                                                                      ),

                                             RequestTimeout:          TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                                         Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Requests_OK,        Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Responses_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Requests_OK,        Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Responses_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Responses_Error,    Is.EqualTo(0));

            empServerAPIClient.OnChargingProgressNotificationRequest  += (timestamp, empServerAPIClient, chargingProgressNotificationRequest) => {

                Assert.That(chargingProgressNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingProgressNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingProgressNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingProgressNotificationRequest.ChargingStart.      ToISO8601(),         Is.EqualTo(chargingStart.ToISO8601()));
                Assert.That(chargingProgressNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingProgressNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingProgressNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo(sessionStart. ToISO8601()));
                Assert.That(chargingProgressNotificationRequest.MeterValueStart?.kWh,                    Is.EqualTo(23M));
                Assert.That(chargingProgressNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingProgressNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));
                Assert.That(chargingProgressNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingProgressNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification progress world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnChargingProgressNotificationResponse += (timestamp, empServerAPIClient, chargingProgressNotificationRequest, oicpResponse, runtime) => {

                var chargingProgressNotificationResponse = oicpResponse.Response;

                Assert.That(chargingProgressNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingProgressNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingProgressNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingProgressNotificationRequest  += (timestamp, empServerAPI,       chargingProgressNotificationRequest) => {

                Assert.That(chargingProgressNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingProgressNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingProgressNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingProgressNotificationRequest.ChargingStart.      ToISO8601(),         Is.EqualTo(chargingStart.ToISO8601()));
                Assert.That(chargingProgressNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingProgressNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingProgressNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo(sessionStart. ToISO8601()));
                Assert.That(chargingProgressNotificationRequest.MeterValueStart?.kWh,                    Is.EqualTo(23M));
                Assert.That(chargingProgressNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingProgressNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));
                Assert.That(chargingProgressNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingProgressNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification progress world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingProgressNotificationResponse += (timestamp, empServerAPI,       chargingProgressNotificationRequest, chargingProgressNotificationResponse, runtime) => {

                Assert.That(chargingProgressNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingProgressNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.SendChargingProgressNotification(request);

            Assert.That(oicpResult,                                                                      Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                         Is.True);
            Assert.That(oicpResult.Response?.Result,                                                     Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                            Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Requests_OK,        Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Responses_OK,       Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingProgressNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Requests_OK,        Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Responses_OK,       Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingProgressNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                            Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                           Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                            Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                           Is.EqualTo(1));

        }

        #endregion

        #region ReceiveChargingStartNotification_Test1()

        [Test]
        public async Task ReceiveChargingStartNotification_Test1()
        {

            if (empServerAPI is null)
            {
                Assert.Fail("empServerAPI must not be null!");
                return;
            }

            if (empServerAPIClient is null)
            {
                Assert.Fail("empServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.NewRandom();
            var cpoPartnerSessionId    = CPOPartnerSession_Id.NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();
            var chargingStart          = Timestamp.Now - TimeSpan.FromSeconds(5);
            var sessionStart           = Timestamp.Now - TimeSpan.FromSeconds(10);

            var request                = new ChargingStartNotificationRequest(

                                             SessionId:            sessionId,
                                             Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             ChargingStart:        chargingStart,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  empPartnerSessionId,
                                             SessionStart:         sessionStart,
                                             MeterValueStart:      WattHour.ParseKWh(23),
                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             PartnerProductId:     PartnerProduct_Id.AC1,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "notification start world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                                      Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Requests_OK,        Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Responses_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Requests_OK,        Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Responses_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Responses_Error,    Is.EqualTo(0));

            empServerAPIClient.OnChargingStartNotificationRequest  += (timestamp, empServerAPIClient, chargingStartNotificationRequest) => {

                Assert.That(chargingStartNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingStartNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingStartNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingStartNotificationRequest.ChargingStart.      ToISO8601(),         Is.EqualTo(chargingStart.ToISO8601()));
                Assert.That(chargingStartNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingStartNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingStartNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo(sessionStart. ToISO8601()));
                Assert.That(chargingStartNotificationRequest.MeterValueStart?.kWh,                    Is.EqualTo(23M));
                Assert.That(chargingStartNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingStartNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));
                Assert.That(chargingStartNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingStartNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification start world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnChargingStartNotificationResponse += (timestamp, empServerAPIClient, chargingStartNotificationRequest, oicpResponse, runtime) => {

                var chargingStartNotificationResponse = oicpResponse.Response;

                Assert.That(chargingStartNotificationResponse,                                        Is.Not.Null);
                Assert.That(chargingStartNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingStartNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingStartNotificationRequest  += (timestamp, empServerAPI,       chargingStartNotificationRequest) => {

                Assert.That(chargingStartNotificationRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(chargingStartNotificationRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(chargingStartNotificationRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(chargingStartNotificationRequest.ChargingStart.      ToISO8601(),         Is.EqualTo(chargingStart.ToISO8601()));
                Assert.That(chargingStartNotificationRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(chargingStartNotificationRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(chargingStartNotificationRequest.SessionStart?.      ToISO8601(),         Is.EqualTo(sessionStart. ToISO8601()));
                Assert.That(chargingStartNotificationRequest.MeterValueStart?.kWh,                    Is.EqualTo(23M));
                Assert.That(chargingStartNotificationRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(chargingStartNotificationRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC1"));
                Assert.That(chargingStartNotificationRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(chargingStartNotificationRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("notification start world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnChargingStartNotificationResponse += (timestamp, empServerAPI,       chargingStartNotificationRequest, chargingStartNotificationResponse, runtime) => {

                Assert.That(chargingStartNotificationResponse?.Result,                                Is.True);
                Assert.That(chargingStartNotificationResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.SendChargingStartNotification(request);

            Assert.That(oicpResult,                                                                   Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                      Is.True);
            Assert.That(oicpResult.Response?.Result,                                                  Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                         Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Requests_OK,        Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Responses_OK,       Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.SendChargingStartNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Requests_OK,        Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Requests_Error,     Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Responses_OK,       Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.    ChargingStartNotification.Responses_Error,    Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                        Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                        Is.EqualTo(1));

        }

        #endregion

    }

}
