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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO Sending ChargeDetailRecord tests.
    /// </summary>
    [TestFixture]
    public class SendChargeDetailRecordTests : ACPOClientAPITests
    {

        #region SendChargeDetailRecord_Test1()

        [Test]
        public async Task SendChargeDetailRecord_Test1()
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

            var request                = new ChargeDetailRecordRequest(

                                             new ChargeDetailRecord(

                                                 SessionId:                       Session_Id.Parse("4cfe3192-87ec-4757-9560-a6ce896bb88b"),
                                                 EVSEId:                          EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                                 Identification:                  Identification.FromUID(UID.Parse("AABBCCDD")),
                                                 SessionStart:                    DateTimeOffset.Parse("2022-08-09T10:18:25.229Z"),
                                                 SessionEnd:                      DateTimeOffset.Parse("2022-08-09T11:18:25.229Z"),
                                                 ChargingStart:                   DateTimeOffset.Parse("2022-08-09T10:20:25.229Z"),
                                                 ChargingEnd:                     DateTimeOffset.Parse("2022-08-09T11:13:25.229Z"),
                                                 ConsumedEnergy:                  WattHour.ParseKWh(35),

                                                 PartnerProductId:                PartnerProduct_Id.   Parse("AC3"),
                                                 CPOPartnerSessionId:             CPOPartnerSession_Id.Parse("e9c6faad-75c8-4f5b-9b5c-164ae7459804"),
                                                 EMPPartnerSessionId:             EMPPartnerSession_Id.Parse("290b96b3-57df-4021-b8f8-50d9c211c767"),
                                                 MeterValueStart:                 WattHour.ParseKWh( 3),
                                                 MeterValueEnd:                   WattHour.ParseKWh(38),
                                                 MeterValuesInBetween:            [
                                                                                      WattHour.ParseKWh(4),
                                                                                      WattHour.ParseKWh(5),
                                                                                      WattHour.ParseKWh(6)
                                                                                  ],
                                                 SignedMeteringValues:            [
                                                                                      new SignedMeteringValue(
                                                                                          "loooong start...",
                                                                                          MeteringStatusType.Start
                                                                                      ),
                                                                                      new SignedMeteringValue(
                                                                                          "loooong progress...",
                                                                                          MeteringStatusType.Progress
                                                                                      ),
                                                                                      new SignedMeteringValue(
                                                                                          "loooong end...",
                                                                                          MeteringStatusType.End
                                                                                      )
                                                                                  ],
                                                 CalibrationLawVerificationInfo:  new CalibrationLawVerification(
                                                                                      CalibrationLawCertificateId:                  "4c6da173-6427-49ed-9b7d-ab0c674d4bc2",
                                                                                      PublicKey:                                    "0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9",
                                                                                      MeteringSignatureURL:                         URL.Parse("https://open.charging.cloud"),
                                                                                      MeteringSignatureEncodingFormat:              "plain",
                                                                                      SignedMeteringValuesVerificationInstruction:  "Just use the Chargy Transparency Software!",
                                                                                      CustomData:                                   null
                                                                                  ),
                                                 HubOperatorId:                   Operator_Id.Parse("DE*GEF"),
                                                 HubProviderId:                   Provider_Id.Parse("DE-GDF"),

                                                 CustomData:                      new JObject(
                                                                                      new JProperty("hello", "CDR world!")
                                                                                  )

                                             ),

                                             OperatorId:   Operator_Id.Parse("DE*GEF")

                                         );

            Assert.That(request,                                                                                                                            Is.Not.Null);

            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Requests_OK,                                                                           Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Requests_Error,                                                                        Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Responses_OK,                                                                          Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Responses_Error,                                                                       Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Requests_OK,                                                                           Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Requests_Error,                                                                        Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Responses_OK,                                                                          Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Responses_Error,                                                                       Is.EqualTo(0));

            cpoClient.   OnSendChargeDetailRecordRequest  += (timestamp, cpoClient,    sendChargeDetailRecordRequest) => {

                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SessionId,                                                                     Is.EqualTo(Session_Id.Parse("4cfe3192-87ec-4757-9560-a6ce896bb88b")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.EVSEId,                                                                        Is.EqualTo(EVSE_Id.Parse("DE*GEF*E1234567*A*1")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.Identification.  ToString(),                                                   Is.EqualTo("AABBCCDD"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SessionStart.    ToISO8601(),                                                  Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SessionEnd.      ToISO8601(),                                                  Is.EqualTo("2022-08-09T11:18:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.ChargingStart.   ToISO8601(),                                                  Is.EqualTo("2022-08-09T10:20:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.ChargingEnd.     ToISO8601(),                                                  Is.EqualTo("2022-08-09T11:13:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.ConsumedEnergy.  kWh,                                                          Is.EqualTo(35));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.PartnerProductId.ToString(),                                                   Is.EqualTo("AC3"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId?.ToString(),                                               Is.EqualTo("e9c6faad-75c8-4f5b-9b5c-164ae7459804"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId?.ToString(),                                               Is.EqualTo("290b96b3-57df-4021-b8f8-50d9c211c767"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValueStart?.kWh,                                                          Is.EqualTo(3));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValueEnd?.  kWh,                                                          Is.EqualTo(38));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.Count(),                                                 Is.EqualTo(3));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.ElementAt(0).kWh,                                        Is.EqualTo(4));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.ElementAt(1).kWh,                                        Is.EqualTo(5));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.ElementAt(2).kWh,                                        Is.EqualTo(6));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.Count(),                                                 Is.EqualTo(3));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(0).Value,                                      Is.EqualTo("loooong start..."));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(0).MeteringStatus,                             Is.EqualTo(MeteringStatusType.Start));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(1).Value,                                      Is.EqualTo("loooong progress..."));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(1).MeteringStatus,                             Is.EqualTo(MeteringStatusType.Progress));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(2).Value,                                      Is.EqualTo("loooong end..."));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(2).MeteringStatus,                             Is.EqualTo(MeteringStatusType.End));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.CalibrationLawCertificateId,                   Is.EqualTo("4c6da173-6427-49ed-9b7d-ab0c674d4bc2"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.PublicKey,                                     Is.EqualTo("0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureURL,                          Is.EqualTo(URL.Parse("https://open.charging.cloud")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat,               Is.EqualTo("plain"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.SignedMeteringValuesVerificationInstruction,   Is.EqualTo("Just use the Chargy Transparency Software!"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.HubOperatorId,                                                                 Is.EqualTo(Operator_Id.Parse("DE*GEF")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.HubProviderId,                                                                 Is.EqualTo(Provider_Id.Parse("DE-GDF")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CustomData?.Count,                                                             Is.EqualTo(1));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CustomData?["hello"]?.Value<String>(),                                         Is.EqualTo("CDR world!"));

                Assert.That(sendChargeDetailRecordRequest.OperatorId.         ToString(),                                                                   Is.EqualTo("DE*GEF"));

                Assert.That(sendChargeDetailRecordRequest.CustomData?.Count,                                                                                Is.EqualTo(1));
                Assert.That(sendChargeDetailRecordRequest.CustomData?["hello"]?.Value<String>(),                                                            Is.EqualTo("CDR world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnSendChargeDetailRecordResponse += (timestamp, cpoClient,    sendChargeDetailRecordRequest, oicpResponse, runtime) => {

                var sendChargeDetailRecordResponse = oicpResponse.Response;

                Assert.That(sendChargeDetailRecordResponse,                                                                                                 Is.Not.Null);
                Assert.That(sendChargeDetailRecordResponse?.Result,                                                                                         Is.True);
                Assert.That(sendChargeDetailRecordResponse?.StatusCode.Code,                                                                                Is.EqualTo(StatusCodes.Success));

                Assert.That(sendChargeDetailRecordResponse?.SessionId,                                                                                      Is.Null);
                Assert.That(sendChargeDetailRecordResponse?.CPOPartnerSessionId,                                                                            Is.Null);
                Assert.That(sendChargeDetailRecordResponse?.EMPPartnerSessionId,                                                                            Is.Null);

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargeDetailRecordRequest  += (timestamp, cpoClientAPI, sendChargeDetailRecordRequest) => {

                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SessionId,                                                                     Is.EqualTo(Session_Id.Parse("4cfe3192-87ec-4757-9560-a6ce896bb88b")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.EVSEId,                                                                        Is.EqualTo(EVSE_Id.Parse("DE*GEF*E1234567*A*1")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.Identification.  ToString(),                                                   Is.EqualTo("AABBCCDD"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SessionStart.    ToISO8601(),                                                  Is.EqualTo("2022-08-09T10:18:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SessionEnd.      ToISO8601(),                                                  Is.EqualTo("2022-08-09T11:18:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.ChargingStart.   ToISO8601(),                                                  Is.EqualTo("2022-08-09T10:20:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.ChargingEnd.     ToISO8601(),                                                  Is.EqualTo("2022-08-09T11:13:25.229Z"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.ConsumedEnergy.  kWh,                                                          Is.EqualTo(35));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.PartnerProductId.ToString(),                                                   Is.EqualTo("AC3"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CPOPartnerSessionId?.ToString(),                                               Is.EqualTo("e9c6faad-75c8-4f5b-9b5c-164ae7459804"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.EMPPartnerSessionId?.ToString(),                                               Is.EqualTo("290b96b3-57df-4021-b8f8-50d9c211c767"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValueStart?.kWh,                                                          Is.EqualTo(3));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValueEnd?.  kWh,                                                          Is.EqualTo(38));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.Count(),                                                 Is.EqualTo(3));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.ElementAt(0).kWh,                                        Is.EqualTo(4));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.ElementAt(1).kWh,                                        Is.EqualTo(5));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.MeterValuesInBetween?.ElementAt(2).kWh,                                        Is.EqualTo(6));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.Count(),                                                 Is.EqualTo(3));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(0).Value,                                      Is.EqualTo("loooong start..."));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(0).MeteringStatus,                             Is.EqualTo(MeteringStatusType.Start));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(1).Value,                                      Is.EqualTo("loooong progress..."));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(1).MeteringStatus,                             Is.EqualTo(MeteringStatusType.Progress));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(2).Value,                                      Is.EqualTo("loooong end..."));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.SignedMeteringValues?.ElementAt(2).MeteringStatus,                             Is.EqualTo(MeteringStatusType.End));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.CalibrationLawCertificateId,                   Is.EqualTo("4c6da173-6427-49ed-9b7d-ab0c674d4bc2"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.PublicKey,                                     Is.EqualTo("0x046eb5c26727e9477f916eb5c26727e9477f91f872d3d79b2bd9f872d3d79b2bd9"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureURL,                          Is.EqualTo(URL.Parse("https://open.charging.cloud")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.MeteringSignatureEncodingFormat,               Is.EqualTo("plain"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CalibrationLawVerificationInfo?.SignedMeteringValuesVerificationInstruction,   Is.EqualTo("Just use the Chargy Transparency Software!"));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.HubOperatorId,                                                                 Is.EqualTo(Operator_Id.Parse("DE*GEF")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.HubProviderId,                                                                 Is.EqualTo(Provider_Id.Parse("DE-GDF")));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CustomData?.Count,                                                             Is.EqualTo(1));
                Assert.That(sendChargeDetailRecordRequest.ChargeDetailRecord.CustomData?["hello"]?.Value<String>(),                                         Is.EqualTo("CDR world!"));

                Assert.That(sendChargeDetailRecordRequest.OperatorId.         ToString(),                                                                   Is.EqualTo("DE*GEF"));

                Assert.That(sendChargeDetailRecordRequest.CustomData?.Count,                                                                                Is.EqualTo(1));
                Assert.That(sendChargeDetailRecordRequest.CustomData?["hello"]?.Value<String>(),                                                            Is.EqualTo("CDR world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnChargeDetailRecordResponse += (timestamp, cpoClientAPI, sendChargeDetailRecordRequest, oicpResponse, runtime) => {

                var sendChargeDetailRecordResponse = oicpResponse.Response;

                Assert.That(sendChargeDetailRecordResponse,                                                                                                 Is.Not.Null);
                Assert.That(sendChargeDetailRecordResponse?.Result,                                                                                         Is.True);
                Assert.That(sendChargeDetailRecordResponse?.StatusCode.Code,                                                                                Is.EqualTo(StatusCodes.Success));

                Assert.That(sendChargeDetailRecordResponse?.SessionId,                                                                                      Is.Null);
                Assert.That(sendChargeDetailRecordResponse?.CPOPartnerSessionId,                                                                            Is.Null);
                Assert.That(sendChargeDetailRecordResponse?.EMPPartnerSessionId,                                                                            Is.Null);

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.SendChargeDetailRecord(request);

            Assert.That(oicpResult,                                                                                                                         Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                                                                            Is.True);
            Assert.That(oicpResult.Response?.Result,                                                                                                        Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                                                                               Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Requests_OK,                                                                           Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Requests_Error,                                                                        Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Responses_OK,                                                                          Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.SendChargeDetailRecord.Responses_Error,                                                                       Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Requests_OK,                                                                           Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Requests_Error,                                                                        Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Responses_OK,                                                                          Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.    ChargeDetailRecord.Responses_Error,                                                                       Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                                                                               Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                                                                              Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                                                                               Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                                                                              Is.EqualTo(1));

        }

        #endregion

    }

}
