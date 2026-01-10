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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.EMP.server
{

    /// <summary>
    /// EMP Authorization tests.
    /// </summary>
    [TestFixture]
    public class AuthorizationTests : AEMPTests
    {

        #region AuthorizeStart_RFIDIdentification_Test1()

        [Test]
        public async Task AuthorizeStart_RFIDIdentification_Test1()
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

            var request                = new AuthorizeStartRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             Identification:       Identification.FromRFIDIdentification(
                                                                       new RFIDIdentification(
                                                                           UID.Parse("AABBCCDD"),
                                                                           RFIDTypes.MifareClassic
                                                                       )
                                                                   ),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  null,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "MifareClassic world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                              Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            empServerAPIClient.OnAuthorizeStartRequest  += (timestamp, empServerAPIClient, authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),             Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),             Is.EqualTo("AABBCCDD"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                                  Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                        Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                        Is.Null);
                Assert.That(authorizeStartRequest.CustomData?.Count,                          Is.EqualTo(1));
                Assert.That(authorizeStartRequest.CustomData?["hello"]?.Value<String>(),      Is.EqualTo("MifareClassic world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnAuthorizeStartResponse += (timestamp, empServerAPIClient, authorizeStartRequest, oicpResponse, runtime) => {

                var authorizeStartResponse = oicpResponse.Response;

                Assert.That(authorizeStartResponse,                                           Is.Not.Null);
                Assert.That(authorizeStartResponse?.AuthorizationStatus,                      Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStartResponse?.StatusCode.Code,                          Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartRequest  += (timestamp, empServerAPI,       authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),             Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),             Is.EqualTo("AABBCCDD"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                                  Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                        Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                        Is.Null);
                Assert.That(authorizeStartRequest.CustomData?.Count,                          Is.EqualTo(1));
                Assert.That(authorizeStartRequest.CustomData?["hello"]?.Value<String>(),      Is.EqualTo("MifareClassic world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartResponse += (timestamp, empServerAPI,       authorizeStartRequest, authorizeStartResponse, runtime) => {

                Assert.That(authorizeStartResponse.AuthorizationStatus,                       Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStartResponse.StatusCode.Code,                           Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            Assert.That(oicpResult,                                                           Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                              Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                             Is.EqualTo(AuthorizationStatusTypes.Authorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                                 Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_OK,               Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_Error,            Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_OK,              Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_Error,           Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_OK,               Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_Error,            Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_OK,              Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_Error,           Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                Is.EqualTo(1));

        }

        #endregion

        #region AuthorizeStart_RFIDIdentification_Test2()

        [Test]
        public async Task AuthorizeStart_RFIDIdentification_Test2()
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

            var request                = new AuthorizeStartRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             Identification:       Identification.FromRFIDIdentification(
                                                                       new RFIDIdentification(
                                                                           UID.Parse("CCDDAABB"),
                                                                           RFIDTypes.MifareClassic
                                                                       )
                                                                   ),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  null,
                                             CustomData:           null,

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                              Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            empServerAPIClient.OnAuthorizeStartRequest  += (timestamp, empServerAPIClient, authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),             Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),             Is.EqualTo("CCDDAABB"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                                  Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                        Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                        Is.Null);
                Assert.That(authorizeStartRequest.CustomData,                                 Is.Null);

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnAuthorizeStartResponse += (timestamp, empServerAPIClient, authorizeStartRequest, oicpResponse, runtime) => {

                var authorizeStartResponse = oicpResponse.Response;

                Assert.That(authorizeStartResponse,                                           Is.Not.Null);
                Assert.That(authorizeStartResponse?.AuthorizationStatus,                      Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
                Assert.That(authorizeStartResponse?.StatusCode.Code,                          Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartRequest  += (timestamp, empServerAPI,       authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),             Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),             Is.EqualTo("CCDDAABB"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                                  Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                        Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                        Is.Null);
                Assert.That(authorizeStartRequest.CustomData,                                 Is.Null);

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartResponse += (timestamp, empServerAPI,       authorizeStartRequest, authorizeStartResponse, runtime) => {

                Assert.That(authorizeStartResponse.AuthorizationStatus,                       Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
                Assert.That(authorizeStartResponse.StatusCode.Code,                           Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            Assert.That(oicpResult,                                                           Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                              Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                             Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                                 Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_OK,               Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_Error,            Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_OK,              Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_Error,           Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_OK,               Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_Error,            Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_OK,              Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_Error,           Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                Is.EqualTo(1));

        }

        #endregion


        #region AuthorizeStart_UID_Test1()

        [Test]
        public async Task AuthorizeStart_UID_Test1()
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

            var request                = new AuthorizeStartRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  null,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "legacy world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                           Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_OK,            Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_Error,         Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_OK,           Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_Error,        Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_OK,            Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_Error,         Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_OK,           Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_Error,        Is.EqualTo(0));

            empServerAPIClient.OnAuthorizeStartRequest  += (timestamp, empServerAPIClient, authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                     Is.Null);
                Assert.That(authorizeStartRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeStartRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("legacy world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnAuthorizeStartResponse += (timestamp, empServerAPIClient, authorizeStartRequest, oicpResponse, runtime) => {

                var authorizeStartResponse = oicpResponse.Response;

                Assert.That(authorizeStartResponse,                                        Is.Not.Null);
                Assert.That(authorizeStartResponse?.AuthorizationStatus,                   Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStartResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartRequest  += (timestamp, empServerAPI,       authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),          Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),          Is.EqualTo("AABBCCDD"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),          Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),          Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                     Is.Null);
                Assert.That(authorizeStartRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeStartRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("legacy world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartResponse += (timestamp, empServerAPI,       authorizeStartRequest, authorizeStartResponse, runtime) => {

                Assert.That(authorizeStartResponse.AuthorizationStatus,                    Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStartResponse.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            Assert.That(oicpResult,                                                        Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                           Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                          Is.EqualTo(AuthorizationStatusTypes.Authorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                              Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_OK,            Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_Error,         Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_OK,           Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_Error,        Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_OK,            Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_Error,         Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_OK,           Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_Error,        Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                              Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                             Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                              Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                             Is.EqualTo(1));

        }

        #endregion

        #region AuthorizeStart_UID_Test2()

        [Test]
        public async Task AuthorizeStart_UID_Test2()
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

            var request                = new AuthorizeStartRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             Identification:       Identification.FromUID(UID.Parse("CCDDAABB")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  null,
                                             CustomData:           null,

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                              Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            empServerAPIClient.OnAuthorizeStartRequest  += (timestamp, empServerAPIClient, authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),             Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),             Is.EqualTo("CCDDAABB"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                                  Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                        Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                        Is.Null);
                Assert.That(authorizeStartRequest.CustomData,                                 Is.Null);

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnAuthorizeStartResponse += (timestamp, empServerAPIClient, authorizeStartRequest, oicpResponse, runtime) => {

                var authorizeStartResponse = oicpResponse.Response;

                Assert.That(authorizeStartResponse,                                           Is.Not.Null);
                Assert.That(authorizeStartResponse?.AuthorizationStatus,                      Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
                Assert.That(authorizeStartResponse?.StatusCode.Code,                          Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartRequest  += (timestamp, empServerAPI,       authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),             Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),             Is.EqualTo("CCDDAABB"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeStartRequest.SessionId,                                  Is.EqualTo(sessionId));
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                        Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId,                        Is.Null);
                Assert.That(authorizeStartRequest.CustomData,                                 Is.Null);

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStartResponse += (timestamp, empServerAPI,       authorizeStartRequest, authorizeStartResponse, runtime) => {

                Assert.That(authorizeStartResponse.AuthorizationStatus,                       Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
                Assert.That(authorizeStartResponse.StatusCode.Code,                           Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.AuthorizeStart(request);

            Assert.That(oicpResult,                                                           Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                              Is.True); // ???
            Assert.That(oicpResult.Response?.AuthorizationStatus,                             Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                                 Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_OK,               Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Requests_Error,            Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_OK,              Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStart.Responses_Error,           Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_OK,               Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Requests_Error,            Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_OK,              Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStart.Responses_Error,           Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                Is.EqualTo(1));

        }

        #endregion


        #region AuthorizeStop_UID_Test1()

        [Test]
        public async Task AuthorizeStop_UID_Test1()
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

            var request                = new AuthorizeStopRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             Identification:       Identification.FromUID(UID.Parse("AABBCCDD")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  null,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "legacy world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                              Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            empServerAPIClient.OnAuthorizeStopRequest  += (timestamp, empServerAPIClient, authorizeStopRequest) => {

                Assert.That(authorizeStopRequest.OperatorId.         ToString(),              Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStopRequest.Identification.     ToString(),              Is.EqualTo("AABBCCDD"));
                Assert.That(authorizeStopRequest.EVSEId.             ToString(),              Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStopRequest.SessionId,                                   Is.EqualTo(sessionId));
                Assert.That(authorizeStopRequest.CPOPartnerSessionId,                         Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStopRequest.EMPPartnerSessionId,                         Is.Null);
                Assert.That(authorizeStopRequest.CustomData?.Count,                           Is.EqualTo(1));
                Assert.That(authorizeStopRequest.CustomData?["hello"]?.Value<String>(),       Is.EqualTo("legacy world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnAuthorizeStopResponse += (timestamp, empServerAPIClient, authorizeStopRequest, oicpResponse, runtime) => {

                var authorizeStopResponse = oicpResponse.Response;

                Assert.That(authorizeStopResponse,                                            Is.Not.Null);
                Assert.That(authorizeStopResponse?.AuthorizationStatus,                       Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStopResponse?.StatusCode.Code,                           Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStopRequest  += (timestamp, empServerAPI,       authorizeStopRequest) => {

                Assert.That(authorizeStopRequest.OperatorId.         ToString(),              Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStopRequest.Identification.     ToString(),              Is.EqualTo("AABBCCDD"));
                Assert.That(authorizeStopRequest.EVSEId.             ToString(),              Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStopRequest.SessionId,                                   Is.EqualTo(sessionId));
                Assert.That(authorizeStopRequest.CPOPartnerSessionId,                         Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStopRequest.EMPPartnerSessionId,                         Is.Null);
                Assert.That(authorizeStopRequest.CustomData?.Count,                           Is.EqualTo(1));
                Assert.That(authorizeStopRequest.CustomData?["hello"]?.Value<String>(),       Is.EqualTo("legacy world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStopResponse += (timestamp, empServerAPI,       authorizeStopRequest, authorizeStopResponse, runtime) => {

                Assert.That(authorizeStopResponse.AuthorizationStatus,                        Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStopResponse.StatusCode.Code,                            Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.AuthorizeStop(request);

            Assert.That(oicpResult,                                                           Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                              Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                             Is.EqualTo(AuthorizationStatusTypes.Authorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                                 Is.EqualTo(StatusCodes.Success));

            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Requests_OK,                Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Requests_Error,             Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Responses_OK,               Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Responses_Error,            Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStop.Requests_OK,                Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStop.Requests_Error,             Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStop.Responses_OK,               Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStop.Responses_Error,            Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                Is.EqualTo(1));

        }

        #endregion

        #region AuthorizeStop_UID_Test2()

        [Test]
        public async Task AuthorizeStop_UID_Test2()
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

            var request                = new AuthorizeStopRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             Identification:       Identification.FromUID(UID.Parse("CCDDAABB")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  null,
                                             CustomData:           null,

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                              Is.Not.Null);

            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.SendChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_OK,       Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Requests_Error,    Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_OK,      Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.    ChargeDetailRecord.Responses_Error,   Is.EqualTo(0));

            empServerAPIClient.OnAuthorizeStopRequest  += (timestamp, empServerAPIClient, authorizeStopRequest) => {

                Assert.That(authorizeStopRequest.OperatorId.         ToString(),              Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStopRequest.Identification.     ToString(),              Is.EqualTo("CCDDAABB"));
                Assert.That(authorizeStopRequest.EVSEId.             ToString(),              Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStopRequest.SessionId,                                   Is.EqualTo(sessionId));
                Assert.That(authorizeStopRequest.CPOPartnerSessionId,                         Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStopRequest.EMPPartnerSessionId,                         Is.Null);
                Assert.That(authorizeStopRequest.CustomData,                                  Is.Null);

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPIClient.OnAuthorizeStopResponse += (timestamp, empServerAPIClient, authorizeStopRequest, oicpResponse, runtime) => {

                var authorizeStopResponse = oicpResponse.Response;

                Assert.That(authorizeStopResponse,                                            Is.Not.Null);
                Assert.That(authorizeStopResponse?.AuthorizationStatus,                       Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
                Assert.That(authorizeStopResponse?.StatusCode.Code,                           Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStopRequest  += (timestamp, empServerAPI,       authorizeStopRequest) => {

                Assert.That(authorizeStopRequest.OperatorId.         ToString(),              Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStopRequest.Identification.     ToString(),              Is.EqualTo("CCDDAABB"));
                Assert.That(authorizeStopRequest.EVSEId.             ToString(),              Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStopRequest.SessionId,                                   Is.EqualTo(sessionId));
                Assert.That(authorizeStopRequest.CPOPartnerSessionId,                         Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStopRequest.EMPPartnerSessionId,                         Is.Null);
                Assert.That(authorizeStopRequest.CustomData,                                  Is.Null);

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            empServerAPI.      OnAuthorizeStopResponse += (timestamp, empServerAPI,       authorizeStopRequest, authorizeStopResponse, runtime) => {

                Assert.That(authorizeStopResponse.AuthorizationStatus,                        Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
                Assert.That(authorizeStopResponse.StatusCode.Code,                            Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await empServerAPIClient.AuthorizeStop(request);

            Assert.That(oicpResult,                                                           Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                              Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                             Is.EqualTo(AuthorizationStatusTypes.NotAuthorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                                 Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Requests_OK,                Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Requests_Error,             Is.EqualTo(0));
            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Responses_OK,               Is.EqualTo(1));
            Assert.That(empServerAPIClient.Counters.AuthorizeStop.Responses_Error,            Is.EqualTo(0));

            Assert.That(empServerAPI.      Counters.AuthorizeStop.Requests_OK,                Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStop.Requests_Error,             Is.EqualTo(0));
            Assert.That(empServerAPI.      Counters.AuthorizeStop.Responses_OK,               Is.EqualTo(1));
            Assert.That(empServerAPI.      Counters.AuthorizeStop.Responses_Error,            Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                 Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                Is.EqualTo(1));

        }

        #endregion

    }

}
