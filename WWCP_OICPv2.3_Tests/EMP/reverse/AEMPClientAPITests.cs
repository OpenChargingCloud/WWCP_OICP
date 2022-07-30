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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP.reverse.tests
{

    /// <summary>
    /// OICP EMP Client API test defaults.
    /// </summary>
    public abstract class AEMPClientAPITests
    {

        #region Data

        protected EMPClientAPI?  empClientAPI;
        protected EMPClient?     empClient;

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

            empClientAPI = new EMPClientAPI(
                               ExternalDNSName:  "open.charging.cloud",
                               HTTPServerPort:   IPPort.Parse(8500),
                               LoggingPath:      "tests",
                               Autostart:        true
                           );

            Assert.IsNotNull(empClientAPI);

            empClientAPI.OnPullEVSEData += (timestamp, sender, pullEVSEDataRequest) => {

                return Task.FromResult(
                    OICPResult<PullEVSEDataResponse>.Success(
                        pullEVSEDataRequest,
                        new PullEVSEDataResponse(
                            pullEVSEDataRequest,
                            Timestamp.Now,
                            pullEVSEDataRequest.EventTrackingId ?? EventTracking_Id.New,
                            Timestamp.Now - pullEVSEDataRequest.Timestamp,
                            Array.Empty<EVSEDataRecord>(),
                            StatusCode: new StatusCode(
                                            StatusCodes.Success
                                        )
                        )
                    )
                );

            };



            empClient = new EMPClient(URL.Parse("http://127.0.0.1:8500"),
                                      RequestTimeout: TimeSpan.FromSeconds(10));

            Assert.IsNotNull(empClient);

        }


        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public void ShutdownEachTest()
        {
            empClientAPI?.Shutdown();
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public void ShutdownOnce()
        {

        }

        #endregion



        //ToDo: RAW tests: OperatorId != OperatorIdURL


        //protected static async Task<HTTPResponse> SendEMPAuthorizeStart(AuthorizeStartRequest Request)
        //{

        //    return await new HTTPSClient(URL.Parse("http://127.0.0.1:8000")).
        //                     Execute(client => client.POSTRequest(HTTPPath.Parse("/api/oicp/charging/v21/operators/DE*GEF/authorize/start"),
        //                                                          requestbuilder => {
        //                                                              requestbuilder.Host         = HTTPHostname.Localhost;
        //                                                              requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
        //                                                              requestbuilder.Content      = Request.ToJSON().ToUTF8Bytes(Newtonsoft.Json.Formatting.None);
        //                                                              requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
        //                                                              requestbuilder.Connection   = "close";
        //                                                          }),
        //                             //CancellationToken:    CancellationToken,
        //                             //EventTrackingId:      EventTrackingId,
        //                             RequestTimeout:       TimeSpan.FromSeconds(10)).

        //                     ConfigureAwait(false);

        //}

    }

}
