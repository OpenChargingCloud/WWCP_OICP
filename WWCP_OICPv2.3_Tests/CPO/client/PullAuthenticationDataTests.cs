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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO Pulling AuthenticationData tests.
    /// </summary>
    [TestFixture]
    public class PullAuthenticationDataTests : ACPOClientAPITests
    {

        #region PullAuthenticationData_Test_Empty()

        [Test]
        public async Task PullAuthenticationData_Test_Empty()
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

            var request = new PullAuthenticationDataRequest(

                              OperatorId:       Operator_Id.Parse("DE*XXX"),
                              CustomData:       new JObject(
                                                    new JProperty("hello", "PullAuthenticationData world!")
                                                ),

                              RequestTimeout:   TimeSpan.FromSeconds(10)

                          );

            Assert.That(request,                                                         Is.Not.Null);

            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            cpoClient.   OnPullAuthenticationDataRequest  += (timestamp, cpoClient,    pushEVSEDataRequest) => {

                Assert.That(pushEVSEDataRequest.OperatorId.ToString(),                   Is.EqualTo("DE*XXX"));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("PullAuthenticationData world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPullAuthenticationDataResponse += (timestamp, cpoClient,    pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                        Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.StatusCode?.Code,                      Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPullAuthenticationDataRequest  += (timestamp, cpoClientAPI, pushEVSEDataRequest) => {

                Assert.That(pushEVSEDataRequest.OperatorId.ToString(),                   Is.EqualTo("DE*XXX"));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("PullAuthenticationData world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPullAuthenticationDataResponse += (timestamp, cpoClientAPI, pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                        Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.StatusCode?.Code,                      Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PullAuthenticationData(request);

            Assert.That(oicpResult,                                                      Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                         Is.True);
            Assert.That(oicpResult.Response?.StatusCode?.Code,                           Is.EqualTo(StatusCodes.Success));

            Assert.That(oicpResult.Response?.ProviderAuthenticationData,                 Is.Not.Null);
            Assert.That(oicpResult.Response?.ProviderAuthenticationData,                 Is.Empty);

            Assert.That(oicpResult.Response?.Number,                                     Is.Null);
            Assert.That(oicpResult.Response?.Size,                                       Is.Null);
            Assert.That(oicpResult.Response?.TotalElements,                              Is.Null);
            Assert.That(oicpResult.Response?.LastPage,                                   Is.Null);
            Assert.That(oicpResult.Response?.FirstPage,                                  Is.Null);
            Assert.That(oicpResult.Response?.TotalPages,                                 Is.Null);
            Assert.That(oicpResult.Response?.NumberOfElements,                           Is.Null);

            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                            Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                           Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                            Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                           Is.EqualTo(1));

        }

        #endregion

        #region PullAuthenticationData_Test1()

        [Test]
        public async Task PullAuthenticationData_Test1()
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

            var request = new PullAuthenticationDataRequest(

                              OperatorId:       Operator_Id.Parse("DE*GEF"),
                              CustomData:       new JObject(
                                                    new JProperty("hello", "PullAuthenticationData world!")
                                                ),

                              RequestTimeout:   TimeSpan.FromSeconds(10)

                          );

            Assert.That(request,                                                         Is.Not.Null);

            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            cpoClient.   OnPullAuthenticationDataRequest  += (timestamp, cpoClient,    pushEVSEDataRequest) => {

                Assert.That(pushEVSEDataRequest.OperatorId.ToString(),                   Is.EqualTo("DE*GEF"));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("PullAuthenticationData world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnPullAuthenticationDataResponse += (timestamp, cpoClient,    pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                        Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.StatusCode?.Code,                      Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPullAuthenticationDataRequest  += (timestamp, cpoClientAPI, pushEVSEDataRequest) => {

                Assert.That(pushEVSEDataRequest.OperatorId.ToString(),                   Is.EqualTo("DE*GEF"));
                Assert.That(pushEVSEDataRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(pushEVSEDataRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("PullAuthenticationData world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnPullAuthenticationDataResponse += (timestamp, cpoClientAPI, pushEVSEDataRequest, oicpResponse, runtime) => {

                var pushEVSEDataResponse = oicpResponse.Response;

                Assert.That(pushEVSEDataResponse,                                        Is.Not.Null);
                Assert.That(pushEVSEDataResponse?.StatusCode?.Code,                      Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.PullAuthenticationData(request);

            Assert.That(oicpResult,                                                      Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                         Is.True);
            Assert.That(oicpResult.Response?.StatusCode?.Code,                           Is.EqualTo(StatusCodes.Success));

            Assert.That(oicpResult.Response?.ProviderAuthenticationData,                 Is.Not.Null);
            Assert.That(oicpResult.Response?.ProviderAuthenticationData.Count(),         Is.EqualTo(1));

            //Assert.That(oicpResult.Response?.Number,                                     Is.EqualTo(0));
            //Assert.That(oicpResult.Response?.Size,                                       Is.EqualTo(0));
            //Assert.That(oicpResult.Response?.TotalElements,                              Is.EqualTo(0));
            //Assert.That(oicpResult.Response?.LastPage,                                   Is.True);
            //Assert.That(oicpResult.Response?.FirstPage,                                  Is.True);
            //Assert.That(oicpResult.Response?.TotalPages,                                 Is.EqualTo(0));
            //Assert.That(oicpResult.Response?.NumberOfElements,                           Is.EqualTo(0));

            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_OK,        Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Requests_Error,     Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_OK,       Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.PullAuthenticationData.Responses_Error,    Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                            Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                           Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                            Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                           Is.EqualTo(1));


            var providerAuthenticationData = oicpResult.Response?.ProviderAuthenticationData.FirstOrDefault();
            Assert.That(providerAuthenticationData,                                      Is.Not.Null);
            Assert.That(providerAuthenticationData?.ProviderId,                          Is.EqualTo(Provider_Id.Parse("DE-GDF")));
            Assert.That(providerAuthenticationData?.Identifications.Count(),             Is.EqualTo(5));

            var identification1 = providerAuthenticationData?.Identifications.ElementAt(0);
            Assert.That(identification1,                                                 Is.Not.Null);
            Assert.That(identification1?.RFIDId,                                         Is.EqualTo(UID.Parse("11223344")));

            var identification2 = providerAuthenticationData?.Identifications.ElementAt(1);
            Assert.That(identification2,                                                 Is.Not.Null);
            Assert.That(identification2?.RFIDIdentification?.UID,                        Is.EqualTo(UID.Parse("55667788")));
            Assert.That(identification2?.RFIDIdentification?.RFIDType,                   Is.EqualTo(RFIDTypes.MifareClassic));
            Assert.That(identification2?.RFIDIdentification?.EVCOId,                     Is.EqualTo(EVCO_Id.Parse("DE-GDF-C12345678-X")));
            Assert.That(identification2?.RFIDIdentification?.PrintedNumber,              Is.EqualTo("GDF-0001"));
            Assert.That(identification2?.RFIDIdentification?.ExpiryDate,                 Is.EqualTo(DateTimeOffset.Parse("2022-08-09T10:18:25.229Z").ToUniversalTime()));

            var identification3 = providerAuthenticationData?.Identifications.ElementAt(2);
            Assert.That(identification3,                                                 Is.Not.Null);
            Assert.That(identification3?.QRCodeIdentification?.EVCOId,                   Is.EqualTo(EVCO_Id.Parse("DE-GDF-C56781234-X")));
            Assert.That(identification3?.QRCodeIdentification?.HashedPIN?.Function,      Is.EqualTo(HashFunctions.Bcrypt));
            Assert.That(identification3?.QRCodeIdentification?.HashedPIN?.Value,         Is.EqualTo(Hash_Value.Parse("XXX")));

            var identification4 = providerAuthenticationData?.Identifications.ElementAt(3);
            Assert.That(identification4,                                                 Is.Not.Null);
            Assert.That(identification4?.RemoteIdentification,                           Is.EqualTo(EVCO_Id.Parse("DE-GDF-C23456781-X")));

            var identification5 = providerAuthenticationData?.Identifications.ElementAt(4);
            Assert.That(identification5,                                                 Is.Not.Null);
            Assert.That(identification5?.PlugAndChargeIdentification,                    Is.EqualTo(EVCO_Id.Parse("DE-GDF-C81235674-X")));

        }

        #endregion

    }

}
