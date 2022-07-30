/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using cloud.charging.open.protocols.OICPv2_3.EMP;
using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CentralService.tests
{

    /// <summary>
    /// OICP central service test defaults.
    /// </summary>
    public abstract class ACentralServiceTests
    {

        #region Data

        protected CentralServiceAPI?  centralServiceAPI;

        protected EMPClient?          empClient;
        protected CPOClient?          cpoClient;

        #endregion


        #region SetupOnce()

        [OneTimeSetUp]
        public void SetupOnce()
        {

        }

        #endregion

        #region SetupEachTest()

        [SetUp]
        public void SetupEachTest()
        {

            Timestamp.Reset();

            centralServiceAPI = new CentralServiceAPI(
                                    ExternalDNSName:  "open.charging.cloud",
                                    HTTPServerPort:   IPPort.Parse(6000),
                                    LoggingPath:      "tests",
                                    Autostart:        true
                                );

            Assert.IsNotNull(centralServiceAPI);


            #region EMPClientAPI delegates...

            centralServiceAPI.EMPClientAPI.OnAuthorizeRemoteStart += (timestamp, sender, authorizeRemoteStartRequest) => {

                return Task.FromResult(
                           OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Success(
                               authorizeRemoteStartRequest,
                               Acknowledgement<AuthorizeRemoteStartRequest>.Success(
                                   Request:                   authorizeRemoteStartRequest,
                                   SessionId:                 Session_Id.NewRandom,
                                   CPOPartnerSessionId:       CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:       EMPPartnerSession_Id.NewRandom,
                                   StatusCodeDescription:     null,
                                   StatusCodeAdditionalInfo:  null,
                                   ResponseTimestamp:         Timestamp.Now,
                                   EventTrackingId:           authorizeRemoteStartRequest.EventTrackingId,
                                   Runtime:                   TimeSpan.FromMilliseconds(23),
                                   ProcessId:                 Process_Id.NewRandom,
                                   HTTPResponse:              null,
                                   CustomData:                null
                               )
                           )
                       );;

            };

            #endregion

            #region CPOClientAPI delegates...

            centralServiceAPI.CPOClientAPI.OnAuthorizeStart += (timestamp, sender, authorizeStartRequest) => {

                return Task.FromResult(
                           OICPResult<AuthorizationStartResponse>.Success(
                               authorizeStartRequest,
                               AuthorizationStartResponse.Authorized(
                                   Request:                           authorizeStartRequest,
                                   SessionId:                         Session_Id.NewRandom,
                                   CPOPartnerSessionId:               CPOPartnerSession_Id.NewRandom,
                                   EMPPartnerSessionId:               EMPPartnerSession_Id.NewRandom,
                                   ProviderId:                        Provider_Id.Parse("DE-GDF"),
                                   StatusCodeDescription:             null,
                                   StatusCodeAdditionalInfo:          null,
                                   AuthorizationStopIdentifications:  null,
                                   ResponseTimestamp:                 Timestamp.Now,
                                   EventTrackingId:                   authorizeStartRequest.EventTrackingId,
                                   Runtime:                           TimeSpan.FromMilliseconds(23),
                                   ProcessId:                         Process_Id.NewRandom,
                                   HTTPResponse:                      null,
                                   CustomData:                        null
                               )
                           )
                       );;

            };

            #endregion

            #region Setup EMPClient...

            empClient = new EMPClient(URL.Parse("http://127.0.0.1:6000"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            Assert.IsNotNull(empClient);

            #endregion

            #region Setup CPOClient...

            cpoClient = new CPOClient(URL.Parse("http://127.0.0.1:6000"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            Assert.IsNotNull(cpoClient);

            #endregion

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            centralServiceAPI?.Shutdown();
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion

    }

}
