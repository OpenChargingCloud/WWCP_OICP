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

namespace cloud.charging.open.protocols.OICPv2_3.tests.CPO.client
{

    /// <summary>
    /// CPO sending AuthorizeStart/-Stop tests.
    /// </summary>
    [TestFixture]
    public class AuthorizeStartStopTests : ACPOClientAPITests
    {

        private readonly CPOPartnerSession_Id cpoPartnerSessionId  = CPOPartnerSession_Id.Parse("9b217a90-9924-4229-a217-3d67a4de00da");

        #region AuthorizeStart_Test1()

        [Test]
        public async Task AuthorizeStart_Test1()
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

            var request = new AuthorizeStartRequest(

                              OperatorId:           Operator_Id.Parse("DE*GEF"),
                              Identification:       Identification.FromUID(UID.Parse("11223344")),
                              EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                              PartnerProductId:     PartnerProduct_Id.AC1,
                              CPOPartnerSessionId:  cpoPartnerSessionId,
                              CustomData:           new JObject(
                                                        new JProperty("hello", "MifareClassic Start world!")
                                                    ),

                              RequestTimeout:       TimeSpan.FromSeconds(10)

                          );

            Assert.That(request,                                                                      Is.Not.Null);

            Assert.That(cpoClient.   Counters.AuthorizeStart.Requests_OK,                             Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStart.Requests_Error,                          Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStart.Responses_OK,                            Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStart.Responses_Error,                         Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Requests_OK,                             Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Requests_Error,                          Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Responses_OK,                            Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Responses_Error,                         Is.EqualTo(0));

            cpoClient.   OnAuthorizeStartRequest  += (timestamp, cpoClient,    authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),                     Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),                     Is.EqualTo("11223344"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),                     Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),                     Is.EqualTo("AC1"));
                Assert.That(authorizeStartRequest.SessionId?.         ToString(),                     Is.Null);
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                                Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId?.ToString(),                    Is.Null);
                Assert.That(authorizeStartRequest.CustomData?.Count,                                  Is.EqualTo(1));
                Assert.That(authorizeStartRequest.CustomData?["hello"]?.Value<String>(),              Is.EqualTo("MifareClassic Start world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnAuthorizeStartResponse += (timestamp, cpoClient,    authorizeStartRequest, oicpResponse, runtime) => {

                var authorizeStartResponse = oicpResponse.Response;

                Assert.That(authorizeStartResponse,                                                   Is.Not.Null);
                Assert.That(authorizeStartResponse?.AuthorizationStatus,                              Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStartResponse?.StatusCode.Code,                                  Is.EqualTo(StatusCodes.Success));

                Assert.That(authorizeStartResponse?.SessionId?.          ToString(),                  Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(authorizeStartResponse?.EMPPartnerSessionId?.ToString(),                  Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnAuthorizeStartRequest  += (timestamp, cpoClientAPI, authorizeStartRequest) => {

                Assert.That(authorizeStartRequest.OperatorId.         ToString(),                     Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStartRequest.Identification.     ToString(),                     Is.EqualTo("11223344"));
                Assert.That(authorizeStartRequest.EVSEId.             ToString(),                     Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStartRequest.PartnerProductId.   ToString(),                     Is.EqualTo("AC1"));
                Assert.That(authorizeStartRequest.SessionId?.         ToString(),                     Is.Null);
                Assert.That(authorizeStartRequest.CPOPartnerSessionId,                                Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStartRequest.EMPPartnerSessionId?.ToString(),                    Is.Null);
                Assert.That(authorizeStartRequest.CustomData?.Count,                                  Is.EqualTo(1));
                Assert.That(authorizeStartRequest.CustomData?["hello"]?.Value<String>(),              Is.EqualTo("MifareClassic Start world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnAuthorizeStartResponse += (timestamp, cpoClientAPI, authorizeStartRequest, oicpResponse, runtime) => {

                var authorizeStartResponse = oicpResponse.Response;

                Assert.That(authorizeStartResponse,                                                   Is.Not.Null);
                Assert.That(authorizeStartResponse?.AuthorizationStatus,                              Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStartResponse?.StatusCode.Code,                                  Is.EqualTo(StatusCodes.Success));

                Assert.That(authorizeStartResponse?.SessionId?.          ToString(),                  Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(authorizeStartResponse?.EMPPartnerSessionId?.ToString(),                  Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult     = await cpoClient.AuthorizeStart(request);

            var remoteSocket1  = oicpResult.Response?.HTTPResponse?.RemoteSocket;

            Assert.That(oicpResult,                                                                   Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                                      Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                                     Is.EqualTo(AuthorizationStatusTypes.Authorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                                         Is.EqualTo(StatusCodes.Success));

            Assert.That(oicpResult.Response?.SessionId,                                               Is.EqualTo(Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff")));
            Assert.That(oicpResult.Response?.CPOPartnerSessionId,                                     Is.EqualTo(cpoPartnerSessionId));
            Assert.That(oicpResult.Response?.EMPPartnerSessionId,                                     Is.EqualTo(EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c")));
            Assert.That(oicpResult.Response?.ProviderId,                                              Is.EqualTo(Provider_Id.Parse("DE-GDF")));
            Assert.That(oicpResult.Response?.AuthorizationStopIdentifications?.Count(),               Is.EqualTo(2));
            Assert.That(oicpResult.Response?.AuthorizationStopIdentifications?.ElementAt(0).RFIDId,   Is.EqualTo(UID.Parse("11223344")));
            Assert.That(oicpResult.Response?.AuthorizationStopIdentifications?.ElementAt(1).RFIDId,   Is.EqualTo(UID.Parse("55667788")));

            Assert.That(cpoClient.   Counters.AuthorizeStart.Requests_OK,                             Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.AuthorizeStart.Requests_Error,                          Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStart.Responses_OK,                            Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.AuthorizeStart.Responses_Error,                         Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Requests_OK,                             Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Requests_Error,                          Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Responses_OK,                            Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.AuthorizeStart.Responses_Error,                         Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                                        Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                                         Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                                        Is.EqualTo(1));


            //// ---------------------------------------------------------------------------------------------------------------------------
            //// Validate HTTP Keep-Alives
            //// ---------------------------------------------------------------------------------------------------------------------------

            //Assert.That(oicpResult.Response?.HTTPResponse?.Connection,                           Is.EqualTo(ConnectionType.KeepAlive));

            //var sessionId2             = Session_Id.          NewRandom();
            //var empPartnerSessionId2   = EMPPartnerSession_Id.NewRandom();

            //var oicpResult2            = await cpoClient.AuthorizeStart(request);
            //var remoteSocket2          = oicpResult.Response?.HTTPResponse?.RemoteSocket;


            //Assert.That(oicpResult2,                                                             Is.Not.Null);
            //Assert.That(oicpResult2.IsSuccessful,                                                Is.True);
            //Assert.That(oicpResult2.Response?.AuthorizationStatus,                               Is.EqualTo(AuthorizationStatusTypes.Authorized));
            //Assert.That(oicpResult2.Response?.StatusCode.Code,                                   Is.EqualTo(StatusCodes.Success));

            //Assert.That(remoteSocket1,                                                           Is.EqualTo(remoteSocket2), "HTTP Keep-Alives do not work as expected!");

        }

        #endregion


        #region AuthorizeStop_Test1()

        [Test]
        public async Task AuthorizeStop_Test1()
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

            var request                = new AuthorizeStopRequest(

                                             OperatorId:           Operator_Id.Parse("DE*GEF"),
                                             SessionId:            Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"),
                                             Identification:       Identification.FromUID(UID.Parse("11223344")),
                                             EVSEId:               EVSE_Id.Parse("DE*GEF*E1234567*A*1"),
                                             CPOPartnerSessionId:  cpoPartnerSessionId,
                                             EMPPartnerSessionId:  EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c"),
                                             CustomData:           new JObject(
                                                                       new JProperty("hello", "MifareClassic Stop world!")
                                                                   ),

                                             RequestTimeout:       TimeSpan.FromSeconds(10)

                                         );

            Assert.That(request,                                                          Is.Not.Null);

            Assert.That(cpoClient.   Counters.AuthorizeStop.Requests_OK,                  Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStop.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStop.Responses_OK,                 Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStop.Responses_Error,              Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Requests_OK,                  Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Responses_OK,                 Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Responses_Error,              Is.EqualTo(0));

            cpoClient.   OnAuthorizeStopRequest  += (timestamp, cpoClient,    authorizeStopRequest) => {

                Assert.That(authorizeStopRequest.OperatorId.          ToString(),         Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStopRequest.SessionId.           ToString(),         Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(authorizeStopRequest.Identification.      ToString(),         Is.EqualTo("11223344"));
                Assert.That(authorizeStopRequest.EVSEId.              ToString(),         Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStopRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStopRequest.EMPPartnerSessionId?.ToString(),         Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(authorizeStopRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeStopRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("MifareClassic Stop world!"));

                clientRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClient.   OnAuthorizeStopResponse += (timestamp, cpoClient,    authorizeStopRequest, oicpResponse, runtime) => {

                var authorizeStopResponse = oicpResponse.Response;

                Assert.That(authorizeStopResponse,                                        Is.Not.Null);
                Assert.That(authorizeStopResponse?.AuthorizationStatus,                   Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStopResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                Assert.That(authorizeStopResponse?.SessionId?.          ToString(),       Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(authorizeStopResponse?.EMPPartnerSessionId?.ToString(),       Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));

                clientResponseLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnAuthorizeStopRequest  += (timestamp, cpoClientAPI, authorizeStopRequest) => {

                Assert.That(authorizeStopRequest.OperatorId.          ToString(),         Is.EqualTo("DE*GEF"));
                Assert.That(authorizeStopRequest.SessionId.           ToString(),         Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(authorizeStopRequest.Identification.      ToString(),         Is.EqualTo("11223344"));
                Assert.That(authorizeStopRequest.EVSEId.              ToString(),         Is.EqualTo("DE*GEF*E1234567*A*1"));
                Assert.That(authorizeStopRequest.CPOPartnerSessionId,                     Is.EqualTo(cpoPartnerSessionId));
                Assert.That(authorizeStopRequest.EMPPartnerSessionId?.ToString(),         Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));
                Assert.That(authorizeStopRequest.CustomData?.Count,                       Is.EqualTo(1));
                Assert.That(authorizeStopRequest.CustomData?["hello"]?.Value<String>(),   Is.EqualTo("MifareClassic Stop world!"));

                serverRequestLogging++;

                return Task.CompletedTask;

            };

            cpoClientAPI.OnAuthorizeStopResponse += (timestamp, cpoClientAPI, authorizeStopRequest, oicpResponse, runtime) => {

                var authorizeStopResponse = oicpResponse.Response;

                Assert.That(authorizeStopResponse,                                        Is.Not.Null);
                Assert.That(authorizeStopResponse?.AuthorizationStatus,                   Is.EqualTo(AuthorizationStatusTypes.Authorized));
                Assert.That(authorizeStopResponse?.StatusCode.Code,                       Is.EqualTo(StatusCodes.Success));

                Assert.That(authorizeStopResponse?.SessionId?.          ToString(),       Is.EqualTo("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff"));
                Assert.That(authorizeStopResponse?.EMPPartnerSessionId?.ToString(),       Is.EqualTo("bce77f78-6966-48f4-9abd-007f04862d6c"));

                serverResponseLogging++;

                return Task.CompletedTask;

            };

            var oicpResult  = await cpoClient.AuthorizeStop(request);

            Assert.That(oicpResult,                                                       Is.Not.Null);
            Assert.That(oicpResult.IsSuccessful,                                          Is.True);
            Assert.That(oicpResult.Response?.AuthorizationStatus,                         Is.EqualTo(AuthorizationStatusTypes.Authorized));
            Assert.That(oicpResult.Response?.StatusCode.Code,                             Is.EqualTo(StatusCodes.Success));

            Assert.That(oicpResult.Response?.SessionId,                                   Is.EqualTo(Session_Id.Parse("f8c7c2bf-10dc-46a1-929b-a2bf52bcfaff")));
            Assert.That(oicpResult.Response?.CPOPartnerSessionId,                         Is.EqualTo(cpoPartnerSessionId));
            Assert.That(oicpResult.Response?.EMPPartnerSessionId,                         Is.EqualTo(EMPPartnerSession_Id.Parse("bce77f78-6966-48f4-9abd-007f04862d6c")));
            Assert.That(oicpResult.Response?.ProviderId,                                  Is.EqualTo(Provider_Id.Parse("DE-GDF")));

            Assert.That(cpoClient.   Counters.AuthorizeStop.Requests_OK,                  Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.AuthorizeStop.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoClient.   Counters.AuthorizeStop.Responses_OK,                 Is.EqualTo(1));
            Assert.That(cpoClient.   Counters.AuthorizeStop.Responses_Error,              Is.EqualTo(0));

            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Requests_OK,                  Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Requests_Error,               Is.EqualTo(0));
            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Responses_OK,                 Is.EqualTo(1));
            Assert.That(cpoClientAPI.Counters.AuthorizeStop.Responses_Error,              Is.EqualTo(0));

            Assert.That(clientRequestLogging,                                             Is.EqualTo(1));
            Assert.That(clientResponseLogging,                                            Is.EqualTo(1));
            Assert.That(serverRequestLogging,                                             Is.EqualTo(1));
            Assert.That(serverResponseLogging,                                            Is.EqualTo(1));

        }

        #endregion


    }

}
