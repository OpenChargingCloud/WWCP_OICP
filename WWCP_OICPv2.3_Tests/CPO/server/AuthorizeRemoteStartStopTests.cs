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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.server
{

    /// <summary>
    /// CPO AuthorizeRemote[Start/Stop] tests.
    /// </summary>
    [TestFixture]
    public class AuthorizeRemoteStartStopTests : ACPOTests
    {

        #region AuthorizeRemoteStart_Test1()

        [Test]
        public async Task AuthorizeRemoteStart_Test1()
        {

            if (cpoServerAPI is null)
            {
                Assert.Fail("cpoServerAPI must not be null!");
                return;
            }

            if (cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.          NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();

            var request                = new AuthorizeRemoteStartRequest(

                                             ProviderId:           Provider_Id.   Parse("DE-GDF"),
                                             Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C12345678X")),
                                             EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  null,
                                             EMPPartnerSessionId:  empPartnerSessionId,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "app world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                                 Is.Not.Null);

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_OK,            Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_Error,         Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_OK,           Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_Error,        Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK,                  Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK,                 Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error,              Is.EqualTo(0));

            cpoServerAPIClient.OnAuthorizeRemoteStartRequest  += (timestamp, cpoServerAPIClient, authorizeRemoteStartRequest) => {

                Assert.That(authorizeRemoteStartRequest.ProviderId.      ToString(),             Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStartRequest.Identification.  ToString(),             Is.EqualTo("DE-GDF-C12345678X"));
                Assert.That(authorizeRemoteStartRequest.EVSEId.          ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeRemoteStartRequest.PartnerProductId.ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeRemoteStartRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStartRequest.CPOPartnerSessionId,                     Is.Null);
                Assert.That(authorizeRemoteStartRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStartRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeRemoteStartRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("app world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPIClient.OnAuthorizeRemoteStartResponse += (timestamp, cpoServerAPIClient, authorizeRemoteStartRequest, oicpResponse, runtime) => {

                var authorizeRemoteStartResponse = oicpResponse.Response;

                Assert.That(authorizeRemoteStartResponse,                                        Is.Not.Null);
                Assert.That(authorizeRemoteStartResponse?.Result,                                Is.True);
                Assert.That(authorizeRemoteStartResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStartRequest  += (timestamp, cpoServerAPI,       authorizeRemoteStartRequest) => {

                Assert.That(authorizeRemoteStartRequest.ProviderId.      ToString(),             Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStartRequest.Identification.  ToString(),             Is.EqualTo("DE-GDF-C12345678X"));
                Assert.That(authorizeRemoteStartRequest.EVSEId.          ToString(),             Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeRemoteStartRequest.PartnerProductId.ToString(),             Is.EqualTo("AC3"));
                Assert.That(authorizeRemoteStartRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStartRequest.CPOPartnerSessionId,                     Is.Null);
                Assert.That(authorizeRemoteStartRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStartRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeRemoteStartRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("app world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStartResponse += (timestamp, cpoServerAPI,       authorizeRemoteStartRequest, authorizeRemoteStartResponse, runtime) => {

                Assert.That(authorizeRemoteStartResponse.Result,                                 Is.True);
                Assert.That(authorizeRemoteStartResponse.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult     = await cpoServerAPIClient.AuthorizeRemoteStart(request);

            var remoteSocket1  = oicpResult.Response?.HTTPResponse?.RemoteSocket;

            Assert.That(oicpResult,                                                              Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                 Is.True);
            Assert.That(oicpResult.Response?.Result,                                             Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                    Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_OK,            Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_Error,         Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_OK,           Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_Error,        Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK,                  Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK,                 Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error,              Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                    Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                   Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                    Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                   Is.EqualTo(1));


            // ---------------------------------------------------------------------------------------------------------------------------
            // Validate HTTP Keep-Alives
            // ---------------------------------------------------------------------------------------------------------------------------

            Assert.That(oicpResult.Response?.HTTPResponse?.Connection,                           Is.EqualTo(ConnectionType.KeepAlive));

            var sessionId2             = Session_Id.          NewRandom();
            var empPartnerSessionId2   = EMPPartnerSession_Id.NewRandom();

            var request2               = new AuthorizeRemoteStartRequest(

                                             ProviderId:           Provider_Id.   Parse("DE-GDF"),
                                             Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C23456789X")),
                                             EVSEId:               EVSE_Id.       Parse("DE*GEF*E1234567*A*1"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC1"),
                                             SessionId:            sessionId2,
                                             CPOPartnerSessionId:  null,
                                             EMPPartnerSessionId:  empPartnerSessionId2,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "app world (2)!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            var oicpResult2            = await cpoServerAPIClient.AuthorizeRemoteStart(request);
            var remoteSocket2          = oicpResult.Response?.HTTPResponse?.RemoteSocket;


            Assert.That(oicpResult2,                                                             Is.Not.Null);
            Assert.That(oicpResult2.IsSuccessful,                                                Is.True);
            Assert.That(oicpResult2.Response?.Result,                                            Is.True);
            Assert.That(oicpResult2.Response?.StatusCode.Code,                                   Is.EqualTo(StatusCodes.Success));

            Assert.That(remoteSocket1,                                                           Is.EqualTo(remoteSocket2), "HTTP Keep-Alives do not work as expected!");

        }

        #endregion

        #region AuthorizeRemoteStart_Test2()

        [Test]
        public async Task AuthorizeRemoteStart_Test2()
        {

            if (cpoServerAPI is null)
            {
                Assert.Fail("cpoServerAPI must not be null!");
                return;
            }

            if (cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.          NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();

            var request                = new AuthorizeRemoteStartRequest(

                                             ProviderId:           Provider_Id.Parse("DE-GDF"),
                                             Identification:       Identification.FromRemoteIdentification(EVCO_Id.Parse("DE-GDF-C22222222X")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*2"),
                                             PartnerProductId:     PartnerProduct_Id.Parse("AC3"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  null,
                                             EMPPartnerSessionId:  empPartnerSessionId,
                                             CustomData:           null,

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                            Is.Not.Null);

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_OK,       Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_Error,    Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_OK,      Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_Error,   Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK,             Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error,          Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK,            Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error,         Is.EqualTo(0));

            cpoServerAPIClient.OnAuthorizeRemoteStartRequest  += (timestamp, cpoServerAPIClient, authorizeRemoteStartRequest) => {

                Assert.That(authorizeRemoteStartRequest.ProviderId.      ToString(),        Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStartRequest.Identification.  ToString(),        Is.EqualTo("DE-GDF-C22222222X"));
                Assert.That(authorizeRemoteStartRequest.EVSEId.          ToString(),        Is.EqualTo("DE*GEF*E1234567*A*2"));
                Assert.That(authorizeRemoteStartRequest.PartnerProductId.ToString(),        Is.EqualTo("AC3"));
                Assert.That(authorizeRemoteStartRequest.SessionId,                          Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStartRequest.CPOPartnerSessionId,                Is.Null);
                Assert.That(authorizeRemoteStartRequest.EMPPartnerSessionId,                Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStartRequest.CustomData,                         Is.Null);

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPIClient.OnAuthorizeRemoteStartResponse += (timestamp, cpoServerAPIClient, authorizeRemoteStartRequest, oicpResponse, runtime) => {

                var authorizeRemoteStartResponse = oicpResponse.Response;

                Assert.That(authorizeRemoteStartResponse,                                   Is.Not.Null);
                Assert.That(authorizeRemoteStartResponse?.Result,                           Is.False);
                Assert.That(authorizeRemoteStartResponse?.StatusCode.Code,                  Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStartRequest  += (timestamp, cpoServerAPI,       authorizeRemoteStartRequest) => {

                Assert.That(authorizeRemoteStartRequest.ProviderId.      ToString(),        Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStartRequest.Identification.  ToString(),        Is.EqualTo("DE-GDF-C22222222X"));
                Assert.That(authorizeRemoteStartRequest.EVSEId.          ToString(),        Is.EqualTo("DE*GEF*E1234567*A*2"));
                Assert.That(authorizeRemoteStartRequest.PartnerProductId.ToString(),        Is.EqualTo("AC3"));
                Assert.That(authorizeRemoteStartRequest.SessionId,                          Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStartRequest.CPOPartnerSessionId,                Is.Null);
                Assert.That(authorizeRemoteStartRequest.EMPPartnerSessionId,                Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStartRequest.CustomData,                         Is.Null);

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStartResponse += (timestamp, cpoServerAPI,       authorizeRemoteStartRequest, authorizeRemoteStartResponse, runtime) => {

                Assert.That(authorizeRemoteStartResponse.Result,                            Is.False);
                Assert.That(authorizeRemoteStartResponse.StatusCode.Code,                   Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStart(request);

            Assert.That(oicpResult,                                                         Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                            Is.True);
            Assert.That(oicpResult.Response?.Result,                                        Is.False);
            Assert.That(oicpResult.Response?.StatusCode.Code,                               Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_OK,       Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Requests_Error,    Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_OK,      Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStart.Responses_Error,   Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_OK,             Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Requests_Error,          Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_OK,            Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStart.Responses_Error,         Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                               Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                              Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                               Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                              Is.EqualTo(1));

        }

        #endregion


        #region AuthorizeRemoteStop_Test1()

        [Test]
        public async Task AuthorizeRemoteStop_Test1()
        {

            if (cpoServerAPI is null)
            {
                Assert.Fail("cpoServerAPI must not be null!");
                return;
            }

            if (cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.Parse("7e8f35a6-13c8-4b37-8099-b21323c83e85");
            var cpoPartnerSessionId    = CPOPartnerSession_Id.NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();

            var request                = new AuthorizeRemoteStopRequest(

                                             ProviderId:           Provider_Id.Parse("DE-GDF"),
                                             EVSEId:               EVSE_Id.    Parse("DE*GEF*E1234567*A*1"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  empPartnerSessionId,
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "app world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                                Is.Not.Null);

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_OK,            Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_Error,         Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_OK,           Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_Error,        Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK,                  Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK,                 Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error,              Is.EqualTo(0));

            cpoServerAPIClient.OnAuthorizeRemoteStopRequest  += (timestamp, cpoServerAPIClient, authorizeRemoteStopRequest) => {

                Assert.That(authorizeRemoteStopRequest.ProviderId.ToString(),                   Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStopRequest.EVSEId.    ToString(),                   Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeRemoteStopRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStopRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeRemoteStopRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("app world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPIClient.OnAuthorizeRemoteStopResponse += (timestamp, cpoServerAPIClient, authorizeRemoteStopRequest, oicpResponse, runtime) => {

                var authorizeRemoteStopResponse = oicpResponse.Response;

                Assert.That(authorizeRemoteStopResponse,                                        Is.Not.Null);
                Assert.That(authorizeRemoteStopResponse?.Result,                                Is.True);
                Assert.That(authorizeRemoteStopResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStopRequest  += (timestamp, cpoServerAPI,      authorizeRemoteStopRequest) => {

                Assert.That(authorizeRemoteStopRequest.ProviderId.ToString(),                   Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStopRequest.EVSEId.    ToString(),                   Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeRemoteStopRequest.SessionId,                               Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStopRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.EMPPartnerSessionId,                     Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeRemoteStopRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("app world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStopResponse += (timestamp, cpoServerAPI,      authorizeRemoteStopRequest, authorizeRemoteStopResponse, runtime) => {

                Assert.That(authorizeRemoteStopResponse.Result,                                 Is.True);
                Assert.That(authorizeRemoteStopResponse.StatusCode.Code,                        Is.EqualTo(StatusCodes.Success));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStop(request);

            Assert.That(oicpResult,                                                             Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                Is.True);
            Assert.That(oicpResult.Response?.Result,                                            Is.True);
            Assert.That(oicpResult.Response?.StatusCode.Code,                                   Is.EqualTo(StatusCodes.Success));

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_OK,            Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_Error,         Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_OK,           Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_Error,        Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK,                  Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK,                 Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error,              Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                   Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                  Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                   Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                  Is.EqualTo(1));

        }

        #endregion

        #region AuthorizeRemoteStop_Test2()

        [Test]
        public async Task AuthorizeRemoteStop_Test2()
        {

            if (cpoServerAPI is null)
            {
                Assert.Fail("cpoServerAPI must not be null!");
                return;
            }

            if (cpoServerAPIClient is null)
            {
                Assert.Fail("cpoServerAPIClient must not be null!");
                return;
            }

            var clientRequestLogging   = 0;
            var clientResponseLogging  = 0;
            var serverRequestLogging   = 0;
            var serverResponseLogging  = 0;

            var sessionId              = Session_Id.Parse("ae8f35a6-23d4-4b37-1994-21314c83e85c");
            var cpoPartnerSessionId    = CPOPartnerSession_Id.NewRandom();
            var empPartnerSessionId    = EMPPartnerSession_Id.NewRandom();

            var request                = new AuthorizeRemoteStopRequest(

                                             ProviderId:           Provider_Id.Parse("DE-GDF"),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*2"),
                                             SessionId:            sessionId,
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  empPartnerSessionId,
                                             CustomData:           null,

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                           Is.Not.Null);

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_OK,       Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_Error,    Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_OK,      Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_Error,   Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK,             Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error,          Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK,            Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error,         Is.EqualTo(0));

            cpoServerAPIClient.OnAuthorizeRemoteStopRequest  += (timestamp, cpoServerAPIClient, authorizeRemoteStopRequest) => {

                Assert.That(authorizeRemoteStopRequest.ProviderId.ToString(),              Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStopRequest.EVSEId.    ToString(),              Is.EqualTo("DE*GEF*E1234567*A*2"));
                Assert.That(authorizeRemoteStopRequest.SessionId,                          Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStopRequest.CPOPartnerSessionId,                Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.EMPPartnerSessionId,                Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.CustomData,                         Is.Null);

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPIClient.OnAuthorizeRemoteStopResponse += (timestamp, cpoServerAPIClient, authorizeRemoteStopRequest, oicpResponse, runtime) => {

                var authorizeRemoteStopResponse = oicpResponse.Response;

                Assert.That(authorizeRemoteStopResponse,                                   Is.Not.Null);
                Assert.That(authorizeRemoteStopResponse?.Result,                           Is.False);
                Assert.That(authorizeRemoteStopResponse?.StatusCode.Code,                  Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStopRequest  += (timestamp, cpoServerAPI,      authorizeRemoteStopRequest) => {

                Assert.That(authorizeRemoteStopRequest.ProviderId.ToString(),              Is.EqualTo("DE-GDF"));
                Assert.That(authorizeRemoteStopRequest.EVSEId.    ToString(),              Is.EqualTo("DE*GEF*E1234567*A*2"));
                Assert.That(authorizeRemoteStopRequest.SessionId,                          Is.EqualTo(sessionId));
                Assert.That(authorizeRemoteStopRequest.CPOPartnerSessionId,                Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.EMPPartnerSessionId,                Is.EqualTo(empPartnerSessionId));
                Assert.That(authorizeRemoteStopRequest.CustomData,                         Is.Null);

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoServerAPI.      OnAuthorizeRemoteStopResponse += (timestamp, cpoServerAPI,      authorizeRemoteStopRequest, authorizeRemoteStopResponse, runtime) => {

                Assert.That(authorizeRemoteStopResponse.Result,                            Is.False);
                Assert.That(authorizeRemoteStopResponse.StatusCode.Code,                   Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult = await cpoServerAPIClient.AuthorizeRemoteStop(request);

            Assert.That(oicpResult,                                                        Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                           Is.True);
            Assert.That(oicpResult.Response?.Result,                                       Is.False);
            Assert.That(oicpResult.Response?.StatusCode.Code,                              Is.EqualTo(StatusCodes.CommunicationToEVSEFailed));

            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_OK,       Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Requests_Error,    Is.EqualTo(0));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_OK,      Is.EqualTo(1));
            Assert.That(cpoServerAPIClient.Counters.AuthorizeRemoteStop.Responses_Error,   Is.EqualTo(0));

            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_OK,             Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Requests_Error,          Is.EqualTo(0));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_OK,            Is.EqualTo(1));
            Assert.That(cpoServerAPI.Counters.AuthorizeRemoteStop.Responses_Error,         Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                              Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                             Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                              Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                             Is.EqualTo(1));

        }

        #endregion

    }

}
